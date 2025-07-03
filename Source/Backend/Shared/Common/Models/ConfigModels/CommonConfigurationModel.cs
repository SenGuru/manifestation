using Manifest.Common.Interfaces.Service.Config;

namespace Manifest.Common.Models.ConfigModels
{
    public class CommonConfigurationModel : ICommonConfiguration
    {
        public string PrivacyPolicyUrl { get; set; }

        public string TermsOfUseUrl { get; set; }

        public string HelpUrl { get; set; }

        public string SupportEmail { get; set; }

        public string CopyrightInfo { get; set; }

        public string JwtSecretKey { get; set; }

        public bool UseLocalNppesServer { get; set; }

        public string LocalLogServerUrl { get; set; }

        public bool ForceSSL { get; set; }

        public string EmailServer { get; set; }

        public int EmailServerPort { get; set; }

        public string EmailServerUserName { get; set; }

        public string EmailServerPassword { get; set; }

        public bool EmailUseSSL { get; set; }

        public string EmailFromName { get; set; }

        public string EmailFromEmail { get; set; }

        public string AdminNameForEmail { get; set; }

        public string AdminEmailForEmail { get; set; }

        public string DefaultCommonEmailDomain { get; set; }

        public int MaxZipFileSizeinMB { get; set; }

        public string NativeTempFileLocation { get; set; }

        public string ZipFileDownloadFileStorageConnectionString { get; set; }

        public bool IsDedicatedQueue { get; set; }

        public string ActiveMQHost { get; set; }

        public string ActiveMQUsername { get; set; }

        public string ActiveMQPassword { get; set; }

        public string APIServerUrl { get; set; }

        public string WhatsAppGraphAPIServerUrl { get; set; }

        public string WebApplicationServerUrl { get; set; }
    }
}
