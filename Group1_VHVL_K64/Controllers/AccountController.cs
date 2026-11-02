using Group1_VHVL_K64.Constants;
using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Models.DBModel;
using Group1_VHVL_K64.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Group1_VHVL_K64.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountAuthService _accountAuthService;
        private readonly CustomerProfileService _customerProfileService;

        public AccountController(
            AccountAuthService accountAuthService,
            CustomerProfileService customerProfileService)
        {
            _accountAuthService = accountAuthService;
            _customerProfileService = customerProfileService;
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            CustomerRegistrationResult registerResult = await _accountAuthService.RegisterCustomerAsync(
                model.Username,
                model.FullName,
                model.Password);

            if (!registerResult.Success)
            {
                ModelState.AddModelError(nameof(RegisterViewModel.Username), registerResult.ErrorMessage);
                return View(model);
            }

            TempData["SuccessMessage"] = "Đăng ký thành công. Vui lòng đăng nhập.";
            return RedirectToAction(nameof(Login));
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

        [HttpGet]
        [Authorize(Roles = AppRoles.User)]
        public async Task<IActionResult> Profile()
        {
            if (!_customerProfileService.IsCustomer(User))
            {
                return RedirectToAction(nameof(AccessDenied));
            }

            Customer? customer = await _customerProfileService.GetCustomerAsync(User);
            if (customer == null)
            {
                return RedirectToAction(nameof(AccessDenied));
            }

            CustomerProfileViewModel model = _customerProfileService.MapToProfileViewModel(customer);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.User)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(CustomerProfileViewModel model)
        {
            if (!_customerProfileService.IsCustomer(User))
            {
                return RedirectToAction(nameof(AccessDenied));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            (bool success, string message) = await _customerProfileService.UpdateProfileAsync(User, model);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(model);
            }

            Customer? customer = await _customerProfileService.GetCustomerAsync(User);
            if (customer != null)
            {
                ClaimsPrincipal principal = _accountAuthService.CreateCustomerPrincipal(customer);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = false,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                    });
            }

            TempData["SuccessMessage"] = message;
            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        [Authorize(Roles = AppRoles.User)]
        public IActionResult ChangePassword()
        {
            if (!_customerProfileService.IsCustomer(User))
            {
                return RedirectToAction(nameof(AccessDenied));
            }

            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.User)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!_customerProfileService.IsCustomer(User))
            {
                return RedirectToAction(nameof(AccessDenied));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            (bool success, string message, ClaimsPrincipal? newPrincipal) =
                await _customerProfileService.ChangePasswordAsync(User, model);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(model);
            }

            if (newPrincipal != null)
            {
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    newPrincipal,
                    new AuthenticationProperties
                    {
                        IsPersistent = false,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                    });
            }

            TempData["SuccessMessage"] = message;
            return RedirectToAction(nameof(Profile));
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
