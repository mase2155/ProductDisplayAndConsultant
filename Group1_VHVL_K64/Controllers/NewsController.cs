using Microsoft.AspNetCore.Mvc;

namespace Group1_VHVL_K64.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
