using System.ComponentModel.DataAnnotations;

namespace Group1_VHVL_K64.Models
{
    public class CustomerProfileViewModel
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
        [Display(Name = "Họ và tên")]
        [StringLength(150)]
        public string Fullname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Số điện thoại")]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        [StringLength(500)]
        public string? ShippingAddress { get; set; }
    }
}
