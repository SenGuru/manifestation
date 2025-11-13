using System;
using System.IO;
using Framework.Infrastructure.Config;
using Microsoft.Extensions.Configuration;

namespace Manifest.Service.Config
{
    public class DbConfiguration : BaseDbConfiguration
    {
        private readonly AppConfigurationFile appConfigurationFile;

        public DbConfiguration(AppConfigurationFile appConfigurationFile)
        {
            this.appConfigurationFile = appConfigurationFile;
            PopulateFromConfigFile();
            PrepareFolders();
        }

        protected new void PrepareFolders()
        {
            base.PrepareFolders();
        }

        protected override string GetConfigFileLocation()
        {
            return appConfigurationFile.GetFileName();
        }

        protected void PopulateFromConfigFile()
        {
            var configLocation = GetConfigFileLocation();
            if (configLocation == null || configLocation.Trim() == string.Empty)
            {
                throw new Exception("Unable to find the Config location");
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(configLocation))
                .AddJsonFile(Path.GetFileName(configLocation), optional: false, reloadOnChange: true);

            var config = builder.Build();

            //var appSettings = config.GetSection("configuration:appSettings");
            var frameworkLogSettings = config.GetSection("Logging");
            var connectionSettings = config.GetSection("configuration:appSettings:connectionStrings");

            PopulateFromConfigFile(frameworkLogSettings, connectionSettings, configLocation);
        }
    }
}