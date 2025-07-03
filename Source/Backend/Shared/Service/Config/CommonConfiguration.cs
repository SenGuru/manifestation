using Manifest.Common.Constants;
using Manifest.Common.Interfaces.Service.Config;
using Framework.Data.DbAccess;

namespace Manifest.Service.Config
{
    public class CommonConfiguration : DefaultConfigurationPopulator, ICommonConfiguration
    {
        public CommonConfiguration(IConfigDBManager appDBManager, AppConfigurationFile appConfigurationFile)
            : base(appDBManager, ConfigurationConstants.ConfigurationGroup.CommonConfiguration, appConfigurationFile)
        {
            this.RefreshConfig();
        }

        public string PrivacyPolicyUrl { get; private set; } = "";

        public string TermsOfUseUrl { get; private set; } = "";

        public string HelpUrl { get; private set; } = "";

        public string APIServerUrl { get; private set; } = "";

        public string WebApplicationServerUrl { get; private set; } = "";

        public string SupportEmail { get; private set; } = "";

        public string CopyrightInfo { get; private set; } = "";

        public string JwtSecretKey { get; protected set; }

        public string USPSUrl { get; protected set; }

        public string USPSUserId { get; protected set; }

        public bool UseLocalNppesServer { get; protected set; }

        public string LocalNppesServerUrl { get; protected set; }

        public string LocalLogServerUrl { get; protected set; }

        public bool ForceSSL { get; protected set; }

        public string EmailServer { get; protected set; }

        public int EmailServerPort { get; protected set; }

        public string EmailServerUserName { get; protected set; }

        public string EmailServerPassword { get; protected set; }

        public bool EmailUseSSL { get; protected set; }

        public string EmailFromName { get; protected set; }

        public string EmailFromEmail { get; protected set; }

        public string AdminNameForEmail { get; protected set; }

        public string AdminEmailForEmail { get; protected set; }

        public string DefaultCommonEmailDomain { get; protected set; }

        public int MaxZipFileSizeinMB { get; protected set; }

        public string ZipFileDownloadFileStorageConnectionString { get; protected set; }

        public string NativeTempFileLocation { get; protected set; }

        public bool IsDedicatedQueue { get; protected set; }

        public string ActiveMQHost { get; protected set; }

        public string ActiveMQUsername { get; protected set; }

        public string ActiveMQPassword { get; protected set; }

        public string WhatsAppGraphAPIServerUrl { get; protected set; }
    }
}
