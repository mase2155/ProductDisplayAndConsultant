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

    public class CustomerRegistrationResult
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;
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
                bool passwordOk = VerifyPasswordInternal(trimmedPassword, admin.PlainPassword, admin.PasswordHash);
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
                bool passwordOk = VerifyPasswordInternal(trimmedPassword, customer.PlainPassword, customer.PasswordHash);
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

        public ClaimsPrincipal CreateCustomerPrincipal(Customer customer)
        {
            return BuildPrincipal(
                customer.Id,
                customer.Fullname,
                customer.Email,
                AppRoles.User,
                AppClaims.AccountTypeCustomer);
        }

        /// <summary>
        /// Đăng ký khách hàng mới (chỉ bảng Customers, role User).
        /// </summary>
        public async Task<CustomerRegistrationResult> RegisterCustomerAsync(
            string email,
            string fullName,
            string password)
        {
            string trimmedEmail = email.Trim();
            string normalizedEmail = trimmedEmail.ToLowerInvariant();
            string trimmedFullName = fullName.Trim();

            bool emailUsedByAdmin = await _context.Admins
                .AnyAsync(a => a.Email.ToLower() == normalizedEmail);

            if (emailUsedByAdmin)
            {
                return RegistrationFail("Tài khoản đã trùng lặp. Email này không thể đăng ký.");
            }

            bool emailUsedByCustomer = await _context.Customers
                .AnyAsync(c => c.Email.ToLower() == normalizedEmail);

            if (emailUsedByCustomer)
            {
                return RegistrationFail("Tài khoản đã trùng lặp. Email này đã được sử dụng.");
            }

            PasswordHasher<string> hasher = new PasswordHasher<string>();
            string passwordHash = hasher.HashPassword(null!, password);

            Customer newCustomer = new Customer
            {
                Email = trimmedEmail,
                Fullname = trimmedFullName,
                PlainPassword = password,
                PasswordHash = passwordHash,
                Phone = null,
                ShippingAddress = null
            };

            _context.Customers.Add(newCustomer);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return RegistrationFail("Tài khoản đã trùng lặp. Vui lòng dùng email khác.");
            }

            return new CustomerRegistrationResult { Success = true };
        }

        private static CustomerRegistrationResult RegistrationFail(string message)
        {
            return new CustomerRegistrationResult
            {
                Success = false,
                ErrorMessage = message
            };
        }

        private static AccountAuthResult Fail(string message)
        {
            return new AccountAuthResult
            {
                Success = false,
                ErrorMessage = message
            };
        }

        public static bool VerifyPasswordForCustomer(
            string inputPassword,
            string plainPassword,
            string? passwordHash)
        {
            return VerifyPasswordInternal(inputPassword, plainPassword, passwordHash);
        }

        private static bool VerifyPasswordInternal(string inputPassword, string plainPassword, string? passwordHash)
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
