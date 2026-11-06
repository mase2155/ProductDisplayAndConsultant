using System.ComponentModel.DataAnnotations;

namespace Group1_VHVL_K64.Areas.Admins.Models
{
    /// <summary>
    /// Form tạo/sửa sản phẩm trong khu vực Admin.
    /// </summary>
    public class AdminProductFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm.")]
        [Display(Name = "Tên sản phẩm")]
        [StringLength(250, ErrorMessage = "Tên tối đa 250 ký tự.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Mô tả chi tiết")]
        public string? Details { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá niêm yết.")]
        [Display(Name = "Giá niêm yết (đ)")]
        [Range(typeof(decimal), "0", "999999999999", ErrorMessage = "Giá phải từ 0 trở lên.")]
        public decimal Price { get; set; }

        [Display(Name = "Giá khuyến mãi (đ) — để trống nếu không có")]
        [Range(typeof(decimal), "0", "999999999999", ErrorMessage = "Giá phải từ 0 trở lên.")]
        public decimal? NewPrice { get; set; }

        [Display(Name = "Đánh giá (0–5 sao)")]
        [Range(0, 5, ErrorMessage = "Điểm đánh giá từ 0 đến 5.")]
        public decimal? RateByStars { get; set; }

        [Display(Name = "Đường dẫn ảnh (URLImage)")]
        [StringLength(500, ErrorMessage = "Đường dẫn ảnh tối đa 500 ký tự.")]
        public string? Urlimage { get; set; }

        [Display(Name = "Danh mục")]
        public int? CategoryId { get; set; }
    }
}
