namespace Manifest.Common.Interfaces.Service.Config
{
    public interface IReportServerConfiguration
    {
        bool EnableReportServerAutoCleanup { get; }

        int ReportServerAutoCleanUpAfterDays { get; }

        string ReportServerFileStorageConnectionString { get; }

        int ReportServerProcessThreadCount { get; }

        string ReportServerQueueConnectionString { get; }

        string ReportServerViewerWebPath { get; }

        string ReportIntermediateDataFilePath { get; }

        string ReportStoragePrefix { get; }
    }
}