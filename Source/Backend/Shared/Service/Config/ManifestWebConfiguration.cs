using System;
using Manifest.Common.Constants;
using Manifest.Common.Interfaces.Service.Config;
using Framework.Data.DbAccess;

namespace Manifest.Service.Config
{
    public class ManifestWebConfiguration : DefaultConfigurationPopulator, IManifestWebConfiguration
    {
        public ManifestWebConfiguration(IConfigDBManager appDBManager, AppConfigurationFile appConfigurationFile)
            : base(appDBManager, ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration, appConfigurationFile)
        {
            this.RefreshConfig();
        }

        public int OpWebServerPort { get; protected set; }

        public string DesktopSocialLoginErrorUrlPath { get; protected set; }

        public string DesktopSocialLoginSuccessUrlPath { get; protected set; }

        public string ViewUrl { get; protected set; }

        public string ReadOnlyViewUrl { get; protected set; }

        public bool EnableFrontPageDisplayMessage { get; protected set; }

        public string FrontPageDisplayMessage { get; protected set; }

        public string FrontPageDisplayMessageTitle { get; protected set; }

        public int FrontPageDisplayMessageDisplayTime { get; protected set; }

        public DateTime? FrontPageDisplayMessageUntilDate { get; protected set; }

        public bool ReadOnlyAccess { get; protected set; }

        public string ReadOnlyAccessMessage { get; protected set; }

        public bool UseLocalNppesServer { get; protected set; }

        public string LocalNppesServerUrl { get; protected set; }

        public bool IsProduction { get; protected set; }

        public string WebsiteUrl { get; protected set; }

        public string LetterTemplateFileStorageConnectionString { get; protected set; }
    }
}