using Manifest.Common.Constants;
using Manifest.Service.Config;
using Framework.Data.DbAccess;

namespace Framework.Infrastructure.Models.Config
{
    public class LogConfiguration : DefaultConfigurationPopulator, ILogConfiguration
    {
        public LogConfiguration(IConfigDBManager appDBManager, AppConfigurationFile appConfigurationFile)
            : base(appDBManager, ConfigurationConstants.ConfigurationGroup.BaseConfiguration, appConfigurationFile)
        {
            this.RefreshConfig();
        }

        //log related
        public bool LogTrace { get; private set; } = true;

        public bool LogDebug { get; private set; } = true;

        public bool LogInfo { get; private set; } = true;

        public bool LogSql { get; private set; } = true;

        public bool LogWarn { get; private set; } = true;

        public bool LogError { get; private set; } = true;

        public bool LogPerformance { get; private set; } = true;

        public string LogLocation { get; private set; } = "";

        public bool LogToFile { get; private set; } = true;

        public bool LogToConsole { get; private set; } = true;

        public bool LogToDebugger { get; private set; } = true;

        public bool LogToServer { get; private set; } = true;

        public string LogServerHost { get; private set; } = "";

        public int LogServerPort { get; private set; } = 0;

        public string LogServerUserName { get; private set; } = "";

        public string LogServerPassword { get; private set; } = "";

        public string LogServerAccessKey { get; private set; } = "";

        public bool LogPerformanceToMainLogger { get; private set; } = false;
    }
}
