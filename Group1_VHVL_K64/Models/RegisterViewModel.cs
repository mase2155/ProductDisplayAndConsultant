using System.ComponentModel.DataAnnotations;

namespace Group1_VHVL_K64.Models
{
    /// <summary>
    /// Form đăng ký khách hàng (role User / bảng Customers).
    /// </summary>
    public class RegisterViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        [Display(Name = "Email (tên đăng nhập)")]
        [StringLength(150, ErrorMessage = "Email tối đa 150 ký tự.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        [Display(Name = "Họ và tên")]
        [StringLength(150, ErrorMessage = "Họ và tên tối đa 150 ký tự.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu phải từ 8 ký tự trở lên.")]
        [RegularExpression(
            @"^(?=.*[!@#$%^&*(),.?""':{}|<>_\-+=\[\]\\;/`~]).+$",
            ErrorMessage = "Mật khẩu phải có ít nhất một ký tự đặc biệt.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare(nameof(Password), ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(FullName))
            {
                yield break;
            }

            string[] nameParts = FullName.Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length < 2)
            {
                yield return new ValidationResult(
                    "Họ và tên phải gồm ít nhất họ và tên (ví dụ: Nguyễn Văn A).",
                    new[] { nameof(FullName) });
            }
        }
    }
}
