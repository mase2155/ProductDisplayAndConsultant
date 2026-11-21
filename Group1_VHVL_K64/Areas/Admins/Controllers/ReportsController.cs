using Group1_VHVL_K64.Areas.Admins.Models;
using Group1_VHVL_K64.Models.DBModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group1_VHVL_K64.Areas.Admins.Controllers
{
    /// <summary>Báo cáo doanh thu Admin — chỉ theo tuần (ISO).</summary>
    public class ReportsController : AdminBaseController
    {
        private readonly HoangHoanFurnitureContext _context;

        private const string CancelledStatus = "Cancelled";

        private const int DaysPerWeek = 7;

        public ReportsController(HoangHoanFurnitureContext context)
        {
            _context = context;
        }

        /// <param name="weekYear">Năm ISO.</param>
        /// <param name="week">Tuần ISO 1–53.</param>
        public async Task<IActionResult> Index(int? weekYear, int? week)
        {
            (int isoYear, int isoWeek, DateTime weekStart, DateTime weekEnd) =
                IsoWeekRange.Resolve(weekYear, week);

            WeeklyRevenueReportViewModel model = new WeeklyRevenueReportViewModel
            {
                Year = isoYear,
                Week = isoWeek,
                WeekStart = weekStart,
                WeekEndExclusive = weekEnd,
                PeriodLabel = $"Tuần {isoWeek}/{isoYear} (ISO)"
            };

            var ordersInWeek = _context.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate >= weekStart && o.OrderDate < weekEnd)
                .Where(o => o.Status != CancelledStatus);

            model.TotalOrderCount = await ordersInWeek.CountAsync();
            model.TotalRevenue = await ordersInWeek.SumAsync(o => (decimal?)o.TotalAmount) ?? 0;
            model.AverageOrdersPerDay = Math.Round((decimal)model.TotalOrderCount / DaysPerWeek, 2);

            return View(model);
        }
    }
}
