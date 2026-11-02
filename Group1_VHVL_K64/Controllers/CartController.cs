using Group1_VHVL_K64.Helpers;
using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Models.DBModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group1_VHVL_K64.Controllers
{
    public class CartController : Controller
    {
        private readonly HoangHoanFurnitureContext _context;

        public CartController(HoangHoanFurnitureContext context)
        {
            _context = context;
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
        public async Task<IActionResult> AddToCart(int productId)
        {
            Product? product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm!" });
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

            int totalCartItems = SessionCartHelper.GetTotalQuantity(HttpContext.Session);

            return Json(new
            {
                success = true,
                message = "Thêm sản phẩm vào giỏ hàng thành công!",
                totalCartItems = totalCartItems
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            if (quantity < 1)
            {
                return Json(new { success = false, message = "Số lượng tối thiểu là 1." });
            }

            List<CartSessionItem> cartItems = SessionCartHelper.GetCartItems(HttpContext.Session);
            CartSessionItem? line = cartItems.FirstOrDefault(c => c.ProductId == productId);

            if (line == null)
            {
                return Json(new { success = false, message = "Sản phẩm không có trong giỏ hàng." });
            }

            line.Quantity = quantity;
            SessionCartHelper.SaveCartItems(HttpContext.Session, cartItems);

            List<CartLineDisplay> cartLines = await BuildCartLinesFromSessionAsync();
            CartLineDisplay? updatedLine = cartLines.FirstOrDefault(c => c.ProductId == productId);
            decimal grandTotal = CalculateGrandTotal(cartLines);

            return Json(new
            {
                success = true,
                lineTotal = updatedLine?.LineTotal ?? 0,
                grandTotal = grandTotal,
                totalCartItems = SessionCartHelper.GetTotalQuantity(HttpContext.Session)
            });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            List<CartSessionItem> cartItems = SessionCartHelper.GetCartItems(HttpContext.Session);
            CartSessionItem? line = cartItems.FirstOrDefault(c => c.ProductId == productId);

            if (line == null)
            {
                return Json(new { success = false, message = "Sản phẩm không có trong giỏ hàng." });
            }

            cartItems.Remove(line);
            SessionCartHelper.SaveCartItems(HttpContext.Session, cartItems);

            List<CartLineDisplay> cartLines = await BuildCartLinesFromSessionAsync();
            decimal grandTotal = CalculateGrandTotal(cartLines);

            return Json(new
            {
                success = true,
                message = "Đã xóa sản phẩm khỏi giỏ hàng.",
                grandTotal = grandTotal,
                totalCartItems = SessionCartHelper.GetTotalQuantity(HttpContext.Session),
                isEmpty = cartItems.Count == 0
            });
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

        private static decimal CalculateGrandTotal(List<CartLineDisplay> lines)
        {
            decimal total = 0;

            foreach (CartLineDisplay line in lines)
            {
                total += line.LineTotal;
            }

            return total;
        }
    }
}
