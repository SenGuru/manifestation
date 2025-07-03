using LinqToDB.Mapping;

namespace Manifest.Common.Models.DBView
{
    public class HomeDashboardView
    {
        [NotColumn]
        public long TotalEnrollments { get; set; }

        [NotColumn]
        public long TotalYearlyEnrollments { get; set; }

        [NotColumn]
        public long TotalMonthlyEnrollments { get; set; }

        [NotColumn]
        public long TotalMonthlyPointEarning { get; set; }

        [NotColumn]
        public long TotalYearlyPointEarning { get; set; }

        [NotColumn]
        public long TotalMonthlyPointRedemption { get; set; }

        [NotColumn]
        public long TotalYearlyPointRedemption { get; set; }

        [NotColumn]
        public long TotalWeeklyLPCount { get; set; }

        [NotColumn]
        public long TotalMonthlyLPCount { get; set; }
    }
}
