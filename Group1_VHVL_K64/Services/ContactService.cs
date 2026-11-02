using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Models.DBModel;

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
    }
}
