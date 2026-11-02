using System.Diagnostics;
using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Services;
using Microsoft.AspNetCore.Mvc;

namespace Group1_VHVL_K64.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ContactService _contactService;

        public HomeController(ILogger<HomeController> logger, ContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new HomeQuickContactViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(HomeQuickContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _contactService.SaveHomeQuickContactAsync(model.EmailOrPhone);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi lưu liên hệ nhanh trang chủ.");
                ModelState.AddModelError(string.Empty, "Không gửi được thông tin. Vui lòng thử lại sau.");
                return View(model);
            }

            TempData["HomeQuickContactSuccess"] = "Cảm ơn bạn! Chúng tôi sẽ liên hệ sớm.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Partner()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
