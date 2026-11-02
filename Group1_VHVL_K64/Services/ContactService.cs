using Group1_VHVL_K64.Constants;
using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Models.DBModel;
using System.Text.RegularExpressions;

namespace Group1_VHVL_K64.Services
{
    public class ContactService
    {
        private readonly HoangHoanFurnitureContext _context;

        public ContactService(HoangHoanFurnitureContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lưu tin liên hệ từ form Contact/Index vào bảng CustomerInquiries.
        /// </summary>
        public async Task SaveInquiryAsync(ContactFormViewModel model)
        {
            CustomerInquiry inquiry = new CustomerInquiry
            {
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim(),
                Phone = model.Phone.Trim(),
                Content = model.Content.Trim(),
                CreatedAt = DateTime.Now
            };

            _context.CustomerInquiries.Add(inquiry);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Lưu liên hệ nhanh từ form cuối trang chủ (một ô email hoặc SĐT).
        /// </summary>
        public async Task SaveHomeQuickContactAsync(string emailOrPhone)
        {
            string trimmedValue = emailOrPhone.Trim();
            bool isEmail = Regex.IsMatch(trimmedValue, InputValidationPatterns.Email);

            string emailForDb;
            string phoneForDb;

            if (isEmail)
            {
                emailForDb = trimmedValue;
                phoneForDb = "0000000000";
            }
            else
            {
                phoneForDb = trimmedValue;
                string phoneDigits = trimmedValue.Replace("+", string.Empty);
                emailForDb = "quick_" + phoneDigits + "@hoanghoan.local";
            }

            CustomerInquiry inquiry = new CustomerInquiry
            {
                FullName = "Liên hệ nhanh - Trang chủ",
                Email = emailForDb,
                Phone = phoneForDb,
                Content = "Khách gửi từ form trang chủ: " + trimmedValue,
                CreatedAt = DateTime.Now
            };

            _context.CustomerInquiries.Add(inquiry);
            await _context.SaveChangesAsync();
        }
    }
}
