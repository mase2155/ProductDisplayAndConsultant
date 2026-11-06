using Group1_VHVL_K64.Constants;
using Group1_VHVL_K64.Helpers;
using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Models.DBModel;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Group1_VHVL_K64.Services
{
    public class CheckoutResult
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public int? OrderId { get; set; }

        public bool NeedLogin { get; set; }

        public bool NeedProfileUpdate { get; set; }
    }

    public class CheckoutService
    {
        private readonly HoangHoanFurnitureContext _context;
        private readonly CustomerProfileService _customerProfileService;
        private readonly OrderPersistenceService _orderPersistence;

        public CheckoutService(
            HoangHoanFurnitureContext context,
            CustomerProfileService customerProfileService,
            OrderPersistenceService orderPersistence)
        {
            _context = context;
            _customerProfileService = customerProfileService;
            _orderPersistence = orderPersistence;
        }

        public async Task<CheckoutResult> PlaceOrderFromCartAsync(ClaimsPrincipal user, ISession session)
        {
            if (!_customerProfileService.IsCustomer(user))
            {
                return new CheckoutResult
                {
                    Success = false,
                    NeedLogin = true,
                    Message = "Vui lòng đăng nhập tài khoản khách hàng để thanh toán."
                };
            }

            Customer? customer = await _customerProfileService.GetCustomerAsync(user);
            if (customer == null)
            {
                return new CheckoutResult
                {
                    Success = false,
                    Message = "Không tìm thấy thông tin khách hàng."
                };
            }

            if (string.IsNullOrWhiteSpace(customer.Phone)
                || string.IsNullOrWhiteSpace(customer.ShippingAddress))
            {
                return new CheckoutResult
                {
                    Success = false,
                    NeedProfileUpdate = true,
                    Message = "Vui lòng cập nhật số điện thoại và địa chỉ giao hàng trong tài khoản trước khi thanh toán."
                };
            }

            List<CartSessionItem> cartItems = SessionCartHelper.GetCartItems(session);
            if (cartItems.Count == 0)
            {
                return new CheckoutResult
                {
                    Success = false,
                    Message = "Giỏ hàng đang trống, không thể tạo đơn."
                };
            }

            List<int> productIds = cartItems.Select(c => c.ProductId).ToList();
            List<Product> products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            decimal totalAmount = 0;

            foreach (CartSessionItem cartLine in cartItems)
            {
                Product? product = products.FirstOrDefault(p => p.Id == cartLine.ProductId);
                if (product == null)
                {
                    continue;
                }

                decimal unitPrice = product.NewPrice ?? product.Price;
                decimal lineTotal = unitPrice * cartLine.Quantity;
                totalAmount += lineTotal;

                orderDetails.Add(new OrderDetail
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = cartLine.Quantity,
                    UnitPrice = unitPrice,
                    LineTotal = lineTotal
                });
            }

            if (orderDetails.Count == 0)
            {
                return new CheckoutResult
                {
                    Success = false,
                    Message = "Không có sản phẩm hợp lệ trong giỏ hàng."
                };
            }

            string orderNote = BuildOrderNoteForAdmin(customer);

            Order newOrder = new Order
            {
                CustomerId = customer.Id,
                OrderDate = DateTime.Now,
                Status = "Pending",
                TotalAmount = totalAmount,
                Phone = customer.Phone,
                ShippingAddress = customer.ShippingAddress,
                Note = orderNote
            };

            foreach (OrderDetail detail in orderDetails)
            {
                newOrder.OrderDetails.Add(detail);
            }

            await _orderPersistence.SaveNewOrderAsync(newOrder);

            SessionCartHelper.ClearCart(session);

            return new CheckoutResult
            {
                Success = true,
                OrderId = newOrder.Id,
                Message = "Đặt hàng thành công! Mã đơn: #" + newOrder.OrderNumber + ". Admin sẽ xử lý đơn của bạn."
            };
        }

        public int? GetCustomerIdFromUser(ClaimsPrincipal user)
        {
            return _customerProfileService.GetCustomerId(user);
        }

        private static string BuildOrderNoteForAdmin(Customer customer)
        {
            return "Khách: " + customer.Fullname
                + " | Email: " + customer.Email
                + " | Đặt qua giỏ hàng website";
        }
    }
}
