/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace Framework.Infrastructure.Models.Config
{
    public class LogSettings : BaseSettings
    {
        public LogSettings()
            : base(null, null)
        {
        }

        public LogSettings(
                IConfiguration configuration,
                List<KeyValuePair<string, Microsoft.Extensions.Logging.LogLevel>> otherFrameworkLogSettings,
                Func<string, string> configUpdator)
            : base(configuration, configUpdator)
        {
            OtherFrameworkLogSettings = otherFrameworkLogSettings;
        }

        //log related
        public bool LogTrace { get; private set; }

        public bool LogDebug { get; private set; }

        public bool LogInfo { get; private set; }

        public bool LogSql { get; private set; }

        public bool LogWarn { get; private set; }

        public bool LogError { get; private set; }

        public bool LogPerformance { get; private set; } = true;

        public string LogLocation { get; private set; } = "";

        public bool LogToFile { get; private set; } = true;

        public bool LogToConsole { get; private set; } = true;

        public bool LogToDebugger { get; private set; } = false;

        public bool LogToServer { get; private set; } = false;

        public string LogServerHost { get; private set; } = "";

        public int LogServerPort { get; private set; } = 0;

        public string LogServerUserName { get; private set; } = "";

        public string LogServerPassword { get; private set; } = "";

        public string LogServerAccessKey { get; private set; } = "";

        [JsonIgnore]
        public List<KeyValuePair<string, Microsoft.Extensions.Logging.LogLevel>> OtherFrameworkLogSettings { get; private set; }

        public LogSettings TraceLogSettings()
        {
            var result = (LogSettings)this.MemberwiseClone();
            result.LogTrace = true;
            result.LogDebug = true;
            result.LogInfo = true;
            result.LogSql = true;
            result.LogWarn = true;
            result.LogError = true;
            return result;
        }

        public LogSettings InfoLogSettings()
        {
            var result = (LogSettings)this.MemberwiseClone();
            result.LogTrace = false;
            result.LogDebug = false;
            result.LogInfo = true;
            result.LogSql = true;
            result.LogWarn = true;
            result.LogError = true;
            return result;
        }

        public LogSettings WarnLogSettings()
        {
            var result = (LogSettings)this.MemberwiseClone();
            result.LogTrace = false;
            result.LogDebug = false;
            result.LogInfo = false;
            result.LogSql = false;
            result.LogWarn = true;
            result.LogError = true;
            return result;
        }

        public LogSettings ErrorLogSettings()
        {
            var result = (LogSettings)this.MemberwiseClone();
            result.LogTrace = false;
            result.LogDebug = false;
            result.LogInfo = false;
            result.LogSql = false;
            result.LogWarn = false;
            result.LogError = true;
            return result;
        }

        public LogSettings NoOpLogSettings()
        {
            var result = (LogSettings)this.MemberwiseClone();
            result.LogTrace = false;
            result.LogDebug = false;
            result.LogInfo = false;
            result.LogSql = false;
            result.LogWarn = false;
            result.LogError = false;
            return result;
        }
    }
}
