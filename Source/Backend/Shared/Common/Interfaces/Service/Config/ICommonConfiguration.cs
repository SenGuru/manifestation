namespace Manifest.Common.Interfaces.Service.Config
{
    public interface ICommonConfiguration
    {
        string PrivacyPolicyUrl { get; }

        string TermsOfUseUrl { get; }

        string HelpUrl { get; }

        string SupportEmail { get; }

        string CopyrightInfo { get; }

        string AdminEmailForEmail { get; }

        string AdminNameForEmail { get; }

        public string NativeTempFileLocation { get; }

        string DefaultCommonEmailDomain { get; }

        string EmailFromEmail { get; }

        string EmailFromName { get; }

        string EmailServer { get; }

        string EmailServerPassword { get; }

        int EmailServerPort { get; }

        string EmailServerUserName { get; }

        bool EmailUseSSL { get; }

        bool ForceSSL { get; }

        string JwtSecretKey { get; }

        int MaxZipFileSizeinMB { get; }

        string LocalLogServerUrl { get; }

        string ZipFileDownloadFileStorageConnectionString { get; }

        bool IsDedicatedQueue { get; }

        string ActiveMQHost { get; }

        string ActiveMQUsername { get; }

        string ActiveMQPassword { get; }

        string APIServerUrl { get; }

        string WhatsAppGraphAPIServerUrl { get; }

        string WebApplicationServerUrl { get; }
    }
}