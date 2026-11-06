using Group1_VHVL_K64.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Group1_VHVL_K64.ViewComponents
{
    /// <summary>
    /// Hiển thị số lượng sản phẩm trong giỏ (session) trên header — không cần JavaScript.
    /// </summary>
    public class CartBadgeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            int total = SessionCartHelper.GetTotalQuantity(HttpContext.Session);
            return View(total);
        }
    }
}
