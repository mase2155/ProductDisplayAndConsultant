using Microsoft.AspNetCore.Mvc;

namespace Group1_VHVL_K64.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            // Tạm thời bạn có thể dùng đoạn code đơn giản này để test xem đã nhận được request chưa
            // (Sau này bạn có thể thay bằng logic lưu Session hoặc Database thực tế của bạn)

            // Ví dụ kiểm tra sản phẩm có tồn tại không
            // var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            // if (product == null) {
            //     return Json(new { success = false, message = "Không tìm thấy sản phẩm!" });
            // }

            // Giả lập thêm thành công để test giao diện
            return Json(new
            {
                success = true,
                message = "Thêm sản phẩm vào giỏ hàng thành công!",
                totalCartItems = 1 // Số lượng hiển thị trên giỏ hàng (tùy biến sau)
            });
        }
    }
}
