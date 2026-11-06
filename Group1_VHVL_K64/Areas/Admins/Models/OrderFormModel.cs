using System.ComponentModel.DataAnnotations;

namespace Group1_VHVL_K64.Areas.Admins.Models
{
    /// <summary>
    /// ViewModel form đơn hàng (Admin): có thể gán đơn cho khách đã có hoặc tạo khách walk-in mới (nhập tay).
    /// </summary>
    public class OrderFormModel : IValidatableObject
    {
        /// <summary>Chọn khách từ danh sách đã đăng ký.</summary>
        public const string CustomerModeExisting = "Existing";

        /// <summary>Khách mới / offline — chỉ cần tên (và tùy chọn email, SĐT); hệ thống tạo bản ghi Customers để giữ khóa ngoại Order.</summary>
        public const string CustomerModeNew = "New";

        public int Id { get; set; }

        /// <summary>Số đơn hiển thị (1, 2, 3…); chỉ đọc/ghi khi sửa để giữ nguyên.</summary>
        public int OrderNumber { get; set; }

        [Display(Name = "Cách chọn khách hàng")]
        public string CustomerSelectionMode { get; set; } = CustomerModeExisting;

        [Display(Name = "Khách hàng")]
        public int CustomerId { get; set; }

        [Display(Name = "Họ tên khách mới")]
        [StringLength(150)]
        public string? NewCustomerFullName { get; set; }

        [Display(Name = "Email khách mới (tùy chọn)")]
        [StringLength(150)]
        public string? NewCustomerEmail { get; set; }

        [Display(Name = "SĐT khách mới (tùy chọn)")]
        [StringLength(20)]
        public string? NewCustomerPhone { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái.")]
        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Ngày đặt")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Display(Name = "Địa chỉ giao hàng")]
        [StringLength(500)]
        public string? ShippingAddress { get; set; }

        [Display(Name = "Số điện thoại")]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(1000)]
        public string? Note { get; set; }

        [Display(Name = "Chi tiết sản phẩm")]
        public List<OrderDetailFormLine> Details { get; set; } = new List<OrderDetailFormLine>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CustomerSelectionMode == CustomerModeExisting)
            {
                if (CustomerId <= 0)
                {
                    yield return new ValidationResult(
                        "Vui lòng chọn khách hàng.",
                        new[] { nameof(CustomerId) });
                }
            }
            else if (CustomerSelectionMode == CustomerModeNew)
            {
                if (string.IsNullOrWhiteSpace(NewCustomerFullName))
                {
                    yield return new ValidationResult(
                        "Vui lòng nhập họ tên khách hàng mới.",
                        new[] { nameof(NewCustomerFullName) });
                }

                if (!string.IsNullOrWhiteSpace(NewCustomerEmail))
                {
                    EmailAddressAttribute emailCheck = new EmailAddressAttribute();
                    if (!emailCheck.IsValid(NewCustomerEmail.Trim()))
                    {
                        yield return new ValidationResult(
                            "Email không đúng định dạng.",
                            new[] { nameof(NewCustomerEmail) });
                    }
                }
            }
            else
            {
                yield return new ValidationResult(
                    "Chế độ khách hàng không hợp lệ.",
                    new[] { nameof(CustomerSelectionMode) });
            }
        }
    }

    public class OrderDetailFormLine
    {
        [Display(Name = "Sản phẩm")]
        public int ProductId { get; set; }

        [Range(1, 999, ErrorMessage = "Số lượng tối thiểu là 1.")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }
    }
}
