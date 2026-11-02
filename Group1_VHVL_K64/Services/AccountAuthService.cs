using Group1_VHVL_K64.Constants;
using Group1_VHVL_K64.Models.DBModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Group1_VHVL_K64.Services
{
    public class AccountAuthResult
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public ClaimsPrincipal? Principal { get; set; }
    }

    public class AccountAuthService
    {
        private readonly HoangHoanFurnitureContext _context;

        public AccountAuthService(HoangHoanFurnitureContext context)
        {
            _context = context;
        }

        public async Task<AccountAuthResult> LoginAsync(string email, string password)
        {
            string normalizedEmail = email.Trim().ToLowerInvariant();
            string trimmedPassword = password;

            Admin? admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Email.ToLower() == normalizedEmail);

            if (admin != null)
            {
                bool passwordOk = VerifyPassword(trimmedPassword, admin.PlainPassword, admin.PasswordHash);
                if (!passwordOk)
                {
                    return Fail("Email hoặc mật khẩu không đúng.");
                }

                ClaimsPrincipal principal = BuildPrincipal(
                    userId: admin.Id,
                    fullName: admin.Fullname,
                    email: admin.Email,
                    role: AppRoles.Admin,
                    accountType: AppClaims.AccountTypeAdmin);

                return new AccountAuthResult { Success = true, Principal = principal };
            }

            Customer? customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email.ToLower() == normalizedEmail);

            if (customer != null)
            {
                bool passwordOk = VerifyPassword(trimmedPassword, customer.PlainPassword, customer.PasswordHash);
                if (!passwordOk)
                {
                    return Fail("Email hoặc mật khẩu không đúng.");
                }

                ClaimsPrincipal principal = BuildPrincipal(
                    userId: customer.Id,
                    fullName: customer.Fullname,
                    email: customer.Email,
                    role: AppRoles.User,
                    accountType: AppClaims.AccountTypeCustomer);

                return new AccountAuthResult { Success = true, Principal = principal };
            }

            return Fail("Email hoặc mật khẩu không đúng.");
        }

        private static AccountAuthResult Fail(string message)
        {
            return new AccountAuthResult
            {
                Success = false,
                ErrorMessage = message
            };
        }

        private static bool VerifyPassword(string inputPassword, string plainPassword, string? passwordHash)
        {
            if (!string.IsNullOrWhiteSpace(passwordHash))
            {
                bool hashLooksValid = passwordHash.StartsWith("AQAAAA", StringComparison.Ordinal);

                if (hashLooksValid)
                {
                    try
                    {
                        PasswordHasher<string> hasher = new PasswordHasher<string>();
                        PasswordVerificationResult verifyResult = hasher.VerifyHashedPassword(
                            null!,
                            passwordHash,
                            inputPassword);

                        if (verifyResult == PasswordVerificationResult.Success
                            || verifyResult == PasswordVerificationResult.SuccessRehashNeeded)
                        {
                            return true;
                        }

                        return false;
                    }
                    catch (FormatException)
                    {
                        // Hash lỗi định dạng → thử PlainPassword bên dưới
                    }
                }
            }

            return string.Equals(inputPassword, plainPassword, StringComparison.Ordinal);
        }

        private static ClaimsPrincipal BuildPrincipal(
            int userId,
            string fullName,
            string email,
            string role,
            string accountType)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, fullName),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(AppClaims.DbUserId, userId.ToString()),
                new Claim(AppClaims.AccountType, accountType)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "Cookies");
            return new ClaimsPrincipal(identity);
        }
    }
}
