/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using Framework.Infrastructure.Models.Config;

namespace Framework.Infrastructure.Utils
{
    public static class LogUtils
    {
        public static ILogConfiguration TestLogSettings()
        {
            var result = new DummyLogConfiguration
            {
                LogTrace = false,
                LogDebug = false,
                LogInfo = false,
                LogSql = true,
                LogWarn = true,
                LogError = true,
                LogToConsole = false,
                LogToDebugger = true,
                LogPerformance = true,
                LogToFile = true
            };
            return result;
        }

        public static ILogConfiguration TraceLogSettings()
        {
            var result = new DummyLogConfiguration
            {
                LogTrace = true,
                LogDebug = true,
                LogInfo = true,
                LogSql = true,
                LogWarn = true,
                LogError = true,
                LogToConsole = true,
                LogToDebugger = true,
                LogToFile = true
            };
            return result;
        }

        public static ILogConfiguration InfoLogSettings()
        {
            var result = new DummyLogConfiguration
            {
                LogTrace = false,
                LogDebug = false,
                LogInfo = true,
                LogSql = true,
                LogWarn = true,
                LogError = true,
                LogToConsole = true,
                LogToDebugger = true
            };
            return result;
        }

        public static ILogConfiguration WarnLogSettings()
        {
            var result = new DummyLogConfiguration
            {
                LogTrace = false,
                LogDebug = false,
                LogInfo = false,
                LogSql = false,
                LogWarn = true,
                LogError = true,
                LogToConsole = true,
                LogToDebugger = true
            };
            return result;
        }

        public static ILogConfiguration ErrorLogSettings()
        {
            var result = new DummyLogConfiguration
            {
                LogTrace = false,
                LogDebug = false,
                LogInfo = false,
                LogSql = false,
                LogWarn = false,
                LogError = true,
                LogToConsole = true,
                LogToDebugger = true
            };
            return result;
        }

        public static ILogConfiguration NoOpLogSettings()
        {
            var result = new DummyLogConfiguration
            {
                LogTrace = false,
                LogDebug = false,
                LogInfo = false,
                LogSql = false,
                LogWarn = false,
                LogError = false,
                LogToConsole = true,
                LogToDebugger = true
            };
            return result;
        }

        internal class DummyLogConfiguration : ILogConfiguration
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
}