using Manifest.Common.Interfaces.Service.Config;

namespace Manifest.Common.Models.ConfigModels
{
    public class ReportServerConfigurationModel : IReportServerConfiguration
    {
        public string ReportServerFileStorageConnectionString { get; set; }

        public string ReportServerQueueConnectionString { get; set; }

        public int ReportServerProcessThreadCount { get; set; }

        public bool EnableReportServerAutoCleanup { get; set; }

        public int ReportServerAutoCleanUpAfterDays { get; set; }

        public string ReportServerViewerWebPath { get; set; }

        public string ReportStoragePrefix { get; set; }

        public string ReportIntermediateDataFilePath { get; set; }
    }
}
