using Group1_VHVL_K64.Constants;
using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Models.DBModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Group1_VHVL_K64.Services
{
    public class CustomerProfileService
    {
        private readonly HoangHoanFurnitureContext _context;
        private readonly AccountAuthService _accountAuthService;

        public CustomerProfileService(
            HoangHoanFurnitureContext context,
            AccountAuthService accountAuthService)
        {
            _context = context;
            _accountAuthService = accountAuthService;
        }

        public bool IsCustomer(ClaimsPrincipal user)
        {
            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }

            if (!user.IsInRole(AppRoles.User))
            {
                return false;
            }

            string? accountType = user.FindFirstValue(AppClaims.AccountType);
            return accountType == AppClaims.AccountTypeCustomer;
        }

        public int? GetCustomerId(ClaimsPrincipal user)
        {
            string? idText = user.FindFirstValue(AppClaims.DbUserId);
            if (string.IsNullOrEmpty(idText))
            {
                return null;
            }

            if (int.TryParse(idText, out int customerId))
            {
                return customerId;
            }

            return null;
        }

        public async Task<Customer?> GetCustomerAsync(ClaimsPrincipal user)
        {
            int? customerId = GetCustomerId(user);
            if (customerId == null)
            {
                return null;
            }

            return await _context.Customers.FindAsync(customerId.Value);
        }

        public CustomerProfileViewModel MapToProfileViewModel(Customer customer)
        {
            return new CustomerProfileViewModel
            {
                CustomerId = customer.Id,
                Fullname = customer.Fullname,
                Email = customer.Email,
                Phone = customer.Phone,
                ShippingAddress = customer.ShippingAddress
            };
        }

        public async Task<(bool Success, string Message)> UpdateProfileAsync(
            ClaimsPrincipal user,
            CustomerProfileViewModel model)
        {
            Customer? customer = await GetCustomerAsync(user);
            if (customer == null)
            {
                return (false, "Không tìm thấy tài khoản khách hàng.");
            }

            if (customer.Id != model.CustomerId)
            {
                return (false, "Không được sửa hồ sơ người khác.");
            }

            string newEmail = model.Email.Trim().ToLowerInvariant();
            bool emailUsedByOther = await _context.Customers
                .AnyAsync(c => c.Id != customer.Id && c.Email.ToLower() == newEmail);

            if (emailUsedByOther)
            {
                return (false, "Email này đã được tài khoản khác sử dụng.");
            }

            customer.Fullname = model.Fullname.Trim();
            customer.Email = model.Email.Trim();
            customer.Phone = string.IsNullOrWhiteSpace(model.Phone) ? null : model.Phone.Trim();
            customer.ShippingAddress = string.IsNullOrWhiteSpace(model.ShippingAddress)
                ? null
                : model.ShippingAddress.Trim();

            await _context.SaveChangesAsync();

            return (true, "Cập nhật thông tin cá nhân thành công.");
        }

        public async Task<(bool Success, string Message, ClaimsPrincipal? NewPrincipal)> ChangePasswordAsync(
            ClaimsPrincipal user,
            ChangePasswordViewModel model)
        {
            Customer? customer = await GetCustomerAsync(user);
            if (customer == null)
            {
                return (false, "Không tìm thấy tài khoản khách hàng.", null);
            }

            bool currentOk = AccountAuthService.VerifyPasswordForCustomer(
                model.CurrentPassword,
                customer.PlainPassword,
                customer.PasswordHash);

            if (!currentOk)
            {
                return (false, "Mật khẩu hiện tại không đúng.", null);
            }

            PasswordHasher<string> hasher = new PasswordHasher<string>();
            customer.PlainPassword = model.NewPassword;
            customer.PasswordHash = hasher.HashPassword(null!, model.NewPassword);

            await _context.SaveChangesAsync();

            AccountAuthResult authResult = await _accountAuthService.LoginAsync(customer.Email, model.NewPassword);
            if (!authResult.Success || authResult.Principal == null)
            {
                return (true, "Đổi mật khẩu thành công. Vui lòng đăng nhập lại.", null);
            }

            return (true, "Đổi mật khẩu thành công.", authResult.Principal);
        }
    }
}
