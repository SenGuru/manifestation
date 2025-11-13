/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
namespace Framework.Infrastructure.Models.Config
{
    public interface ILogConfiguration
    {
        //log related
        bool LogTrace { get; }

        bool LogDebug { get; }

        bool LogInfo { get; }

        bool LogSql { get; }

        bool LogWarn { get; }

        bool LogError { get; }

        bool LogPerformance { get; }

        string LogLocation { get; }

        bool LogToFile { get; }

        bool LogToConsole { get; }

        bool LogToDebugger { get; }

        bool LogToServer { get; }

        string LogServerHost { get; }

        int LogServerPort { get; }

        string LogServerUserName { get; }

        string LogServerPassword { get; }

        string LogServerAccessKey { get; }

        bool LogPerformanceToMainLogger { get; }
    }
}
