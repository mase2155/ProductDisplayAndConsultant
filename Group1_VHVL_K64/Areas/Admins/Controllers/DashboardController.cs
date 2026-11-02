using Microsoft.AspNetCore.Mvc;

namespace Group1_VHVL_K64.Areas.Admins.Controllers
{
    public class DashboardController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
