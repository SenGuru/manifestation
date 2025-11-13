/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Concurrent;
using System.Linq;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Models.Config;
using Framework.Infrastructure.Utils;
using Microsoft.Extensions.Logging;

namespace Framework.Infrastructure.Logging
{
    public class FrameworkLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, FrameworkLogger> loggers = new ConcurrentDictionary<string, FrameworkLogger>();
        private readonly IBaseDbConfiguration dbConfig;
        private readonly ILog log;
        private bool disposed = false;

        public FrameworkLoggerProvider(IBaseDbConfiguration dbConfig, ILog log)
        {
            this.dbConfig = dbConfig;
            this.log = log;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            // if (disposing)
            // {
            // }

            disposed = true;
        }

        private FrameworkLogger CreateLoggerImplementation(string name)
        {
            var settings = GetLogSettingForSection(name);
            return new FrameworkLogger(settings, log);
        }

        private ILogConfiguration GetLogSettingForSection(string name)
        {
            if (dbConfig.OtherFrameworkLogSettings == null
                || dbConfig.OtherFrameworkLogSettings.Count == 0
                || dbConfig.OtherFrameworkLogSettings.Exists(x => name.ToLower().Trim().StartsWith(x.Key.ToLower().Trim())))
            {
                if (dbConfig.OtherFrameworkLogSettings.Exists(x => x.Key.ToLower().Trim() == "Default".ToLower()))
                {
                    var defaultItem = dbConfig.OtherFrameworkLogSettings.Find(x => x.Key.ToLower().Trim() == "Default".ToLower());
                    return GetSettingsForLogLevel(defaultItem.Value);
                }
                else
                {
                    return LogUtils.NoOpLogSettings();
                }
            }

            var entry = dbConfig.OtherFrameworkLogSettings.Find(x => name.ToLower().Trim().StartsWith(x.Key.ToLower().Trim()));
            return GetSettingsForLogLevel(entry.Value);
        }

        private ILogConfiguration GetSettingsForLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return LogUtils.TraceLogSettings();
                case LogLevel.Information:
                    return LogUtils.InfoLogSettings();

                case LogLevel.Warning:
                    return LogUtils.WarnLogSettings();

                case LogLevel.Critical:
                case LogLevel.Error:
                    return LogUtils.ErrorLogSettings();

                default:
                case LogLevel.None:
                    return LogUtils.NoOpLogSettings();
            }
        }
    }
}
