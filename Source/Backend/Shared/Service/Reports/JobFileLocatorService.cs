using System;
using System.IO;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;

namespace Manifest.Service.JobServer
{
    public class JobFileLocatorService
    {
        private readonly ILog log;

        public JobFileLocatorService(ILog log)
        {
            this.log = log;
        }

        public string ResolveAndGetJasperExecutable()
        {
            var jasperexe = Environment.OSVersion.Platform == PlatformID.Win32NT ? "jasperstarter.exe" : "jasperstarter";

            var path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "jasperstarter", "bin", jasperexe);
            var oriPath = path;
            if (File.Exists(path))
            {
                log.Info("Jasper path :" + path);
                return path;
            }

            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), @"../" + "jasperstarter", "bin", jasperexe);
            if (File.Exists(path))
            {
                log.Info("Jasper path :" + path);
                return path;
            }

            var parentPath = "";
            for (int i = 0; i < 10; i++)
            {
                var modifiedPath = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), parentPath);
                var jasperPath = FileUtils.Combine(modifiedPath, "jasperstarter", "bin", jasperexe);
                if (File.Exists(jasperPath))
                {
                    log.Info("Jasper path :" + jasperPath);
                    return jasperPath;
                }

                string devJasperPath = FileUtils.Combine(modifiedPath, "Build", "support-apps", "apps", "jasperstarter", "bin", jasperexe);
                if (File.Exists(devJasperPath))
                {
                    log.Info("Jasper path :" + devJasperPath);
                    return devJasperPath;
                }

                log.Info("Secondary path :" + jasperPath);
#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop
                parentPath = @"../" + parentPath;
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
            }

            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", "..", "..", "..", "..", "..", "..", "Build", "support-apps", "apps", "JasperStarter", "bin", jasperexe);
            log.Info("Secondary path :" + path);
            if (File.Exists(path))
            {
                log.Info("Jasper path :" + path);
                return path;
            }

            log.Info("Jasper path path ::" + oriPath);
            return oriPath;
        }

        public string ResolveAndGetJobExecutable(string executableFileName, string executableFolder)
        {
            log.Info("Executable folder :" + executableFolder);
            log.Info("Executable Name :" + executableFileName);

            string path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), executableFolder, executableFileName);
            log.Info("Initial path :" + path);

            if (File.Exists(path))
            {
                return path;
            }

            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), @"../" + executableFolder, executableFileName);
            log.Info("Secondary path :" + path);
            if (File.Exists(path))
            {
                return path;
            }

            log.Info("Into Development Paths :" + path);

            var debugFolderBase = @"Backend\Apps\" + executableFolder + @"\bin\Debug\net8.0" + @"\" + executableFileName;
            var relaseFolderBase = @"Backend\Apps\" + executableFolder + @"\bin\Release\net8.0" + @"\" + executableFileName;

            var parentPath = "";
            for (int i = 0; i < 20; i++)
            {
                var modifiedPath = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), parentPath);
                path = FileUtils.Combine(modifiedPath, debugFolderBase);
                if (File.Exists(path))
                {
                    return path;
                }

                path = FileUtils.Combine(modifiedPath, relaseFolderBase);
                if (File.Exists(path))
                {
                    return path;
                }

#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop
                parentPath = @"..\" + parentPath;
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
            }

            return path;
        }

        public string ResolveAndGetReportFile(string templateFileName)
        {
            string templatePath = string.Empty;
            var path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "ReportsTemplate", templateFileName);
            if (File.Exists(path))
            {
                return path;
            }

            var parentPath = "";

            for (int i = 0; i < 20; i++)
            {
                var modifiedPath = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), parentPath);
                var repPath = FileUtils.Combine(modifiedPath, "ReportsTemplate", templateFileName);
                if (File.Exists(repPath))
                {
                    templatePath = repPath;
                    return templatePath;
                }

                modifiedPath = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), parentPath);
                repPath = FileUtils.Combine(modifiedPath, "ReportProcessor", "ReportsTemplate", templateFileName);
                if (File.Exists(repPath))
                {
                    templatePath = repPath;
                    return templatePath;
                }

#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop
                parentPath = @"..\" + parentPath;
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
            }

            return templatePath;
        }
    }
}
