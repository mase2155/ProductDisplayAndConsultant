using Group1_VHVL_K64.Constants;
using System.ComponentModel.DataAnnotations;

namespace Group1_VHVL_K64.Models
{
    /// <summary>
    /// Form "Thông tin liên hệ" ở cuối trang Home/Index.
    /// </summary>
    public class HomeQuickContactViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email hoặc số điện thoại.")]
        [Display(Name = "Email / Số điện thoại")]
        [StringLength(150)]
        [RegularExpression(InputValidationPatterns.EmailOrPhone, ErrorMessage = InputValidationPatterns.EmailOrPhoneErrorMessage)]
        public string EmailOrPhone { get; set; } = string.Empty;
    }
}
