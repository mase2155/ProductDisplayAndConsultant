using Group1_VHVL_K64.Constants;
using System.ComponentModel.DataAnnotations;

namespace Group1_VHVL_K64.Models
{
    /// <summary>
    /// Form liên hệ trang Contact/Index.
    /// </summary>
    public class ContactFormViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        [Display(Name = "Họ và tên")]
        [StringLength(150)]
        [RegularExpression(InputValidationPatterns.FullName, ErrorMessage = InputValidationPatterns.FullNameErrorMessage)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [Display(Name = "Email")]
        [StringLength(150)]
        [RegularExpression(InputValidationPatterns.Email, ErrorMessage = InputValidationPatterns.EmailErrorMessage)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [Display(Name = "Số điện thoại")]
        [StringLength(20)]
        [RegularExpression(InputValidationPatterns.PhoneRequired, ErrorMessage = InputValidationPatterns.PhoneRequiredErrorMessage)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập nội dung.")]
        [Display(Name = "Nội dung")]
        [StringLength(2000)]
        [RegularExpression(InputValidationPatterns.ContactMessage, ErrorMessage = InputValidationPatterns.ContactMessageErrorMessage)]
        public string Content { get; set; } = string.Empty;
    }
}
