using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Services;
using Microsoft.AspNetCore.Mvc;

namespace Group1_VHVL_K64.Controllers
{
    public class ContactController : Controller
    {
        private readonly ContactService _contactService;

        public ContactController(ContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new ContactFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _contactService.SaveInquiryAsync(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Không gửi được tin liên hệ. Vui lòng thử lại sau.");
                return View(model);
            }

            TempData["ContactSuccessMessage"] = "Cảm ơn bạn! Chúng tôi đã nhận tin liên hệ và sẽ phản hồi sớm.";
            return RedirectToAction(nameof(Index));
        }
    }
}
