using Framework.Infrastructure.Models.Config;

namespace Manifest.Common.Models.ConfigModels
{
    public class LogConfigurationModel : ILogConfiguration
    {
        public bool LogTrace { get; set; }

        public bool LogDebug { get; set; }

        public bool LogInfo { get; set; }

        public bool LogSql { get; set; }

        public bool LogWarn { get; set; }

        public bool LogError { get; set; }

        public bool LogPerformance { get; set; }

        public string LogLocation { get; set; }

        public bool LogToFile { get; set; }

        public bool LogToConsole { get; set; }

        public bool LogToDebugger { get; set; }

        public bool LogToServer { get; set; }

        public string LogServerHost { get; set; }

        public int LogServerPort { get; set; }

        public string LogServerUserName { get; set; }

        public string LogServerPassword { get; set; }

        public string LogServerAccessKey { get; set; }

        public bool LogPerformanceToMainLogger { get; set; }
    }
}