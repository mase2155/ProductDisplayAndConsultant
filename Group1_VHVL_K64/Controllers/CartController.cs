using Group1_VHVL_K64.Constants;
using Group1_VHVL_K64.Helpers;
using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Models.DBModel;
using Group1_VHVL_K64.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace Group1_VHVL_K64.Controllers
{
    public class CartController : Controller
    {
        private readonly HoangHoanFurnitureContext _context;
        private readonly CheckoutService _checkoutService;

        public CartController(
            HoangHoanFurnitureContext context,
            CheckoutService checkoutService)
        {
            _context = context;
            _checkoutService = checkoutService;
        }

        /// <summary>
        /// Hiển thị giỏ hàng: đọc Session → lấy thông tin sản phẩm từ DB → trả View.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            List<CartLineDisplay> cartLines = await BuildCartLinesFromSessionAsync();
            return View(cartLines);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId)
        {
            Product? product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                TempData["CartErrorMessage"] = "Không tìm thấy sản phẩm!";
                return RedirectBackOrProduct();
            }

            List<CartSessionItem> cartItems = SessionCartHelper.GetCartItems(HttpContext.Session);
            CartSessionItem? existingLine = cartItems.FirstOrDefault(c => c.ProductId == productId);

            if (existingLine == null)
            {
                cartItems.Add(new CartSessionItem
                {
                    ProductId = productId,
                    Quantity = 1
                });
            }
            else
            {
                existingLine.Quantity += 1;
            }

            SessionCartHelper.SaveCartItems(HttpContext.Session, cartItems);

            TempData["CartSuccessMessage"] = "Thêm sản phẩm vào giỏ hàng thành công!";
            return RedirectBackOrProduct();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            if (quantity < 1)
            {
                TempData["CartErrorMessage"] = "Số lượng tối thiểu là 1.";
                return RedirectToAction(nameof(Index));
            }

            if (quantity > 99)
            {
                quantity = 99;
            }

            List<CartSessionItem> cartItems = SessionCartHelper.GetCartItems(HttpContext.Session);
            CartSessionItem? line = cartItems.FirstOrDefault(c => c.ProductId == productId);

            if (line == null)
            {
                TempData["CartErrorMessage"] = "Sản phẩm không có trong giỏ hàng.";
                return RedirectToAction(nameof(Index));
            }

            line.Quantity = quantity;
            SessionCartHelper.SaveCartItems(HttpContext.Session, cartItems);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            List<CartSessionItem> cartItems = SessionCartHelper.GetCartItems(HttpContext.Session);
            CartSessionItem? line = cartItems.FirstOrDefault(c => c.ProductId == productId);

            if (line == null)
            {
                TempData["CartErrorMessage"] = "Sản phẩm không có trong giỏ hàng.";
                return RedirectToAction(nameof(Index));
            }

            cartItems.Remove(line);
            SessionCartHelper.SaveCartItems(HttpContext.Session, cartItems);

            TempData["CartSuccessMessage"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearCart()
        {
            SessionCartHelper.ClearCart(HttpContext.Session);

            TempData["CartSuccessMessage"] = "Đã xóa toàn bộ sản phẩm trong giỏ hàng.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Khách hàng (role User) xác nhận thanh toán → tạo Order + OrderDetail → xóa giỏ.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            CheckoutResult result = await _checkoutService.PlaceOrderFromCartAsync(User, HttpContext.Session);

            if (result.NeedLogin)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action(nameof(Index)) });
            }

            if (result.NeedProfileUpdate)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Profile", "Account");
            }

            if (!result.Success || result.OrderId == null)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(CheckoutSuccess), new { id = result.OrderId });
        }

        [HttpGet]
        [Authorize(Roles = AppRoles.User)]
        public async Task<IActionResult> CheckoutSuccess(int id)
        {
            Order? order = await _context.Orders
                .Include(o => o.OrderDetails)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return RedirectToAction(nameof(Index));
            }

            int? customerId = _checkoutService.GetCustomerIdFromUser(User);
            if (customerId == null || order.CustomerId != customerId.Value)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }

        private IActionResult RedirectBackOrProduct()
        {
            if (Request.Headers.TryGetValue("Referer", out StringValues refererValues))
            {
                string referer = refererValues.ToString();
                if (!string.IsNullOrEmpty(referer) && Uri.TryCreate(referer, UriKind.Absolute, out Uri? uri))
                {
                    if (string.Equals(uri.Host, Request.Host.Host, StringComparison.OrdinalIgnoreCase))
                    {
                        string pathAndQuery = uri.PathAndQuery;
                        if (!string.IsNullOrEmpty(pathAndQuery) && pathAndQuery.StartsWith('/'))
                        {
                            return LocalRedirect(pathAndQuery);
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Product");
        }

        private async Task<List<CartLineDisplay>> BuildCartLinesFromSessionAsync()
        {
            List<CartSessionItem> cartItems = SessionCartHelper.GetCartItems(HttpContext.Session);
            List<CartLineDisplay> result = new List<CartLineDisplay>();

            if (cartItems.Count == 0)
            {
                return result;
            }

            List<int> productIds = cartItems.Select(c => c.ProductId).ToList();

            List<Product> products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            foreach (CartSessionItem sessionItem in cartItems)
            {
                Product? product = products.FirstOrDefault(p => p.Id == sessionItem.ProductId);
                if (product == null)
                {
                    continue;
                }

                decimal unitPrice = product.NewPrice ?? product.Price;

                result.Add(new CartLineDisplay
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Urlimage = product.Urlimage,
                    UnitPrice = unitPrice,
                    Quantity = sessionItem.Quantity
                });
            }

            return result;
        }
    }
}
