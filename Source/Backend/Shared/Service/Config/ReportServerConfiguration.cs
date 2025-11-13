using Manifest.Common.Constants;
using Manifest.Common.Interfaces.Service.Config;
using Framework.Data.DbAccess;

namespace Manifest.Service.Config
{
    public class ReportServerConfiguration : DefaultConfigurationPopulator, IReportServerConfiguration
    {
        public ReportServerConfiguration(IConfigDBManager appDBManager, AppConfigurationFile appConfigurationFile)
            : base(appDBManager, ConfigurationConstants.ConfigurationGroup.ReportServerConfiguration, appConfigurationFile)
        {
            this.RefreshConfig();
        }

        public string ReportServerFileStorageConnectionString { get; protected set; }

        public string ReportServerQueueConnectionString { get; protected set; }

        public int ReportServerProcessThreadCount { get; protected set; }

        public bool EnableReportServerAutoCleanup { get; protected set; }

        public int ReportServerAutoCleanUpAfterDays { get; protected set; }

        public string ReportServerViewerWebPath { get; protected set; }

        public string ReportIntermediateDataFilePath { get; protected set; }

        public string ReportStoragePrefix { get; protected set; }
    }
}
