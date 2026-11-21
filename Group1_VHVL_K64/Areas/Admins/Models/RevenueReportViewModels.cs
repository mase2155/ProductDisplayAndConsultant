using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Group1_VHVL_K64.Areas.Admins.Models
{
    /// <summary>Báo cáo doanh thu theo tuần ISO (Admin).</summary>
    public class WeeklyRevenueReportViewModel
    {
        [Display(Name = "Năm")]
        public int Year { get; set; }

        [Display(Name = "Tuần")]
        public int Week { get; set; }

        public DateTime WeekStart { get; set; }

        public DateTime WeekEndExclusive { get; set; }

        public string PeriodLabel { get; set; } = string.Empty;

        public decimal TotalRevenue { get; set; }

        public int TotalOrderCount { get; set; }

        [Display(Name = "Trung bình đơn/ngày trong tuần")]
        public decimal AverageOrdersPerDay { get; set; }

        public string RangeDisplay =>
            $"{WeekStart:dd/MM/yyyy} – {WeekEndExclusive.AddDays(-1):dd/MM/yyyy}";
    }

    public static class IsoWeekRange
    {
        public static (int isoYear, int isoWeek, DateTime weekStart, DateTime weekEndExclusive) Resolve(
            int? year,
            int? week)
        {
            if (year is int y && week is int w)
            {
                w = Math.Clamp(w, 1, ISOWeek.GetWeeksInYear(y));
                DateTime start = ISOWeek.ToDateTime(y, w, DayOfWeek.Monday);
                return (y, w, start, start.AddDays(7));
            }

            DateTime refDay = DateTime.Today.Date;
            int isoYear = ISOWeek.GetYear(refDay);
            int isoWeek = ISOWeek.GetWeekOfYear(refDay);
            DateTime weekStart = ISOWeek.ToDateTime(isoYear, isoWeek, DayOfWeek.Monday);
            return (isoYear, isoWeek, weekStart, weekStart.AddDays(7));
        }
    }
}
