#pragma warning disable S2094 // Classes should not be empty

namespace Manifest.Common.Models.ViewModels
{
    public class SystemSummaryModel
    {
        public long UsersCount { get; set; }

        public long CurrentRunningJobsCount { get; set; }

        public long MessageToSendTotalCount { get; set; }

        public long MessageToSendSuccessCount { get; set; }

        public long MessageToSendErrorCount { get; set; }

        public long TotalJobCount { get; set; }

        public long JobSuccessCount { get; set; }

        public long JobErrorCount { get; set; }
    }
}
#pragma warning restore S2094 // Classes should not be empty