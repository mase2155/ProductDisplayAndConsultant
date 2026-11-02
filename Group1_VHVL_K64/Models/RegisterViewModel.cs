using Group1_VHVL_K64.Constants;
using System.ComponentModel.DataAnnotations;

namespace Group1_VHVL_K64.Models
{
    /// <summary>
    /// Form đăng ký khách hàng (role User / bảng Customers).
    /// </summary>
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [Display(Name = "Email (tên đăng nhập)")]
        [StringLength(150, ErrorMessage = "Email tối đa 150 ký tự.")]
        [RegularExpression(InputValidationPatterns.Email, ErrorMessage = InputValidationPatterns.EmailErrorMessage)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        [Display(Name = "Họ và tên")]
        [StringLength(150, ErrorMessage = "Họ và tên tối đa 150 ký tự.")]
        [RegularExpression(InputValidationPatterns.FullName, ErrorMessage = InputValidationPatterns.FullNameErrorMessage)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        [RegularExpression(InputValidationPatterns.PasswordStrong, ErrorMessage = InputValidationPatterns.PasswordStrongErrorMessage)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare(nameof(Password), ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
