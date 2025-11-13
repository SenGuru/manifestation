using System;
using Framework.Infrastructure.Logging;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.Config;
using Manifest.Service.Cache;
using Manifest.Service.Config;

namespace Manifest.Service.Storage
{
    public class NewReportServerFileStorage : NewBaseFileStorage
    {
        private readonly string reportStoragePrefix;

        public NewReportServerFileStorage(IReportServerConfiguration config, ILog log, CompanyCacheService companyCacheService, AppConfigurationFile appConfigurationFile)
            : base(PredefinedFileStorageTypes.ReportServerFileStorage, log, companyCacheService, appConfigurationFile)
        {
            reportStoragePrefix = config.ReportStoragePrefix;
        }

        public string ReportFilePath(string jobId, string filename, string outputFormat)
        {
            return $"{reportStoragePrefix}/{DateTime.Now.Year.ToString()}/{MonthFromNumberAsString(DateTime.Now.Month)}/{jobId}/{filename}{outputFormat}";
        }

        public string DBFilePath(string jobId, string filename)
        {
            return $"{reportStoragePrefix}/{DateTime.Now.Year.ToString()}/{MonthFromNumberAsString(DateTime.Now.Month)}/{jobId}/{filename}.db";
        }
    }
}
