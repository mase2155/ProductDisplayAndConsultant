using Group1_VHVL_K64.Areas.Admins.Models;
using Group1_VHVL_K64.Models.DBModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group1_VHVL_K64.Areas.Admins.Controllers
{
    /// <summary>
    /// Báo cáo cho Admin (doanh thu theo ngày, v.v.).
    /// </summary>
    public class ReportsController : AdminBaseController
    {
        private readonly HoangHoanFurnitureContext _context;

        /// <summary>Đơn hủy không tính vào doanh thu.</summary>
        private const string CancelledStatus = "Cancelled";

        public ReportsController(HoangHoanFurnitureContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Báo cáo chi tiết doanh thu trong một ngày: tổng tiền, sản phẩm bán, khách có đơn.
        /// Bước 1: Xác định khoảng [00:00, 24:00) của ngày chọn (mặc định hôm nay).
        /// Bước 2: Truy vấn đơn không ở trạng thái Cancelled, gom nhóm theo sản phẩm và theo khách.
        /// </summary>
        /// <param name="reportDate">Ngày cần xem (query ?reportDate=yyyy-MM-dd); null = ngày hiện tại trên server.</param>
        public async Task<IActionResult> DailyRevenue(DateTime? reportDate)
        {
            DateTime dayStart = (reportDate ?? DateTime.Today).Date;
            DateTime dayEnd = dayStart.AddDays(1);

            DailyRevenueReportViewModel model = new DailyRevenueReportViewModel
            {
                ReportDate = dayStart
            };

            // Tổng doanh thu: cộng TotalAmount của các đơn trong ngày (không hủy).
            model.TotalRevenue = await _context.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate >= dayStart && o.OrderDate < dayEnd)
                .Where(o => o.Status != CancelledStatus)
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

            // Hàng đã bán: gom theo tên sản phẩm (theo dòng chi tiết đơn).
            model.Products = await (
                from d in _context.OrderDetails.AsNoTracking()
                join o in _context.Orders.AsNoTracking() on d.OrderId equals o.Id
                where o.OrderDate >= dayStart && o.OrderDate < dayEnd && o.Status != CancelledStatus
                group d by d.ProductName into g
                select new DailyProductSaleRow
                {
                    ProductName = g.Key,
                    QuantitySold = g.Sum(x => x.Quantity),
                    LineTotalSum = g.Sum(x => x.LineTotal)
                })
                .OrderByDescending(p => p.LineTotalSum)
                .ToListAsync();

            // Khách có đơn trong ngày: gom theo khách (họ tên), có thể nhiều đơn / một khách.
            model.Customers = await (
                from o in _context.Orders.AsNoTracking()
                join c in _context.Customers.AsNoTracking() on o.CustomerId equals c.Id
                where o.OrderDate >= dayStart && o.OrderDate < dayEnd && o.Status != CancelledStatus
                group o by c.Fullname into g
                select new DailyCustomerSaleRow
                {
                    CustomerName = g.Key,
                    OrderCount = g.Count(),
                    TotalSpent = g.Sum(x => x.TotalAmount)
                })
                .OrderByDescending(c => c.TotalSpent)
                .ToListAsync();

            return View(model);
        }
    }
}
