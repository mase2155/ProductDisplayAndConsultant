using System.ComponentModel.DataAnnotations;

namespace Group1_VHVL_K64.Areas.Admins.Models
{
    public class OrderFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn khách hàng.")]
        [Display(Name = "Khách hàng")]
        public int CustomerId { get; set; }

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
