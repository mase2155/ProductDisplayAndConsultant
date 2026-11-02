using Group1_VHVL_K64.Constants;
using System.ComponentModel.DataAnnotations;

namespace Group1_VHVL_K64.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [Display(Name = "Email")]
        [StringLength(150)]
        [RegularExpression(InputValidationPatterns.Email, ErrorMessage = InputValidationPatterns.EmailErrorMessage)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Vui lòng nhập mật khẩu.")]
        public string Password { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }
    }
}
