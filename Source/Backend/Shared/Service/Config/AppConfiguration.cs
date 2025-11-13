using System;
using System.IO;
using Manifest.Common.Constants;
using Framework.Data.DbAccess;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Utils;

namespace Manifest.Service.Config
{
    public class AppConfiguration : DefaultConfigurationPopulator, IBaseAppConfiguration
    {
        public AppConfiguration(IConfigDBManager appDBManager, AppConfigurationFile appConfigurationFile)
            : base(appDBManager, ConfigurationConstants.ConfigurationGroup.BaseConfiguration, appConfigurationFile)
        {
            this.RefreshConfig();
            ApplicationVersion = GetAppVersionNumber();
        }

        public string ApplicationVersion { get; private set; }

        public string DatabaseVersion { get; private set; } = "";

        public bool EnableNewFeatures { get; private set; } = false;

        public int CacheHighRefreshInMinutes { get; private set; } = 0;

        public int CachMediumRefreshInMinutes { get; private set; } = 0;

        public int CachLowRefreshInMinutes { get; private set; } = 0;

        public bool UseMiniProfiler { get; private set; } = false;

        public bool ShowAllLinksWithoutPermissionCheck { get; private set; } = false;

        public string GetVersionFromFile(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    return "Unknown";
                }

                return File.ReadAllText(filename);
            }
            catch (Exception)
            {
                return "Unable to Find the version";
            }
        }

        public string GetAppVersionNumber()
        {
            try
            {
                string versionFileName = "version_number.txt";

                versionFileName = Path.Combine(FileUtils.GetApplicationExeDirectory(), versionFileName);

                if (File.Exists(versionFileName))
                    return GetVersionFromFile(versionFileName);

                versionFileName = Path.Combine(FileUtils.GetApplicationExeDirectory(), "..", versionFileName);
                if (File.Exists(versionFileName))
                    return GetVersionFromFile(versionFileName);

                versionFileName = Path.Combine(FileUtils.GetApplicationExeDirectory(), "..", "..", versionFileName);
                if (File.Exists(versionFileName))
                    return GetVersionFromFile(versionFileName);

                return GetVersionFromFile(versionFileName);
            }
            catch (Exception)
            {
                return "Version Unknown";
            }
        }
    }
}