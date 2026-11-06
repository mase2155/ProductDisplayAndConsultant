using System.ComponentModel.DataAnnotations;

namespace Group1_VHVL_K64.Areas.Admins.Models
{
    /// <summary>
    /// Dữ liệu trình bày báo cáo doanh thu theo một ngày (Admin).
    /// </summary>
    public class DailyRevenueReportViewModel
    {
        /// <summary>Ngày báo cáo (chỉ phần ngày, theo múi giờ server).</summary>
        [Display(Name = "Ngày báo cáo")]
        public DateTime ReportDate { get; set; }

        /// <summary>Tổng doanh thu trong ngày (theo TotalAmount đơn không hủy).</summary>
        [Display(Name = "Tổng doanh thu")]
        public decimal TotalRevenue { get; set; }

        /// <summary>Tổng hợp sản phẩm đã bán trong ngày.</summary>
        public List<DailyProductSaleRow> Products { get; set; } = new List<DailyProductSaleRow>();

        /// <summary>Khách hàng có đơn trong ngày (theo tổng tiền đơn).</summary>
        public List<DailyCustomerSaleRow> Customers { get; set; } = new List<DailyCustomerSaleRow>();
    }

    public class DailyProductSaleRow
    {
        public string ProductName { get; set; } = string.Empty;

        [Display(Name = "Số lượng bán")]
        public int QuantitySold { get; set; }

        [Display(Name = "Thành tiền")]
        public decimal LineTotalSum { get; set; }
    }

    public class DailyCustomerSaleRow
    {
        [Display(Name = "Khách hàng")]
        public string CustomerName { get; set; } = string.Empty;

        [Display(Name = "Số đơn")]
        public int OrderCount { get; set; }

        [Display(Name = "Tổng tiền")]
        public decimal TotalSpent { get; set; }
    }
}
