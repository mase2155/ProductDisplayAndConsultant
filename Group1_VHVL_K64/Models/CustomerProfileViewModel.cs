using Group1_VHVL_K64.Constants;
using System.ComponentModel.DataAnnotations;

namespace Group1_VHVL_K64.Models
{
    public class CustomerProfileViewModel
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
        [Display(Name = "Họ và tên")]
        [StringLength(150)]
        [RegularExpression(InputValidationPatterns.FullName, ErrorMessage = InputValidationPatterns.FullNameErrorMessage)]
        public string Fullname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [Display(Name = "Email")]
        [StringLength(150)]
        [RegularExpression(InputValidationPatterns.Email, ErrorMessage = InputValidationPatterns.EmailErrorMessage)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Số điện thoại")]
        [StringLength(20)]
        [RegularExpression(InputValidationPatterns.PhoneOptional, ErrorMessage = InputValidationPatterns.PhoneOptionalErrorMessage)]
        public string? Phone { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        [StringLength(500)]
        public string? ShippingAddress { get; set; }
    }
}
