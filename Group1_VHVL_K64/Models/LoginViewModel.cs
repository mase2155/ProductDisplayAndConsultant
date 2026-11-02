using System.ComponentModel.DataAnnotations;

namespace Group1_VHVL_K64.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }
    }
}
