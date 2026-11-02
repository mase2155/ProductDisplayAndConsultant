using Group1_VHVL_K64.Constants;
using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Group1_VHVL_K64.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountAuthService _accountAuthService;

        public AccountController(AccountAuthService accountAuthService)
        {
            _accountAuthService = accountAuthService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectAfterLogin(returnUrl);
            }

            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AccountAuthResult authResult = await _accountAuthService.LoginAsync(model.Email, model.Password);

            if (!authResult.Success || authResult.Principal == null)
            {
                ModelState.AddModelError(string.Empty, authResult.ErrorMessage);
                return View(model);
            }

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                authResult.Principal,
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                });

            if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            if (authResult.Principal.IsInRole(AppRoles.Admin))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admins" });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectAfterLogin(string? returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (User.IsInRole(AppRoles.Admin))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admins" });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
