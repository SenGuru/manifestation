using System;
using Manifest.Common.Interfaces.Service.Config;

namespace Manifest.Common.Models.ConfigModels
{
    public class ManifestWebConfigurationModel : IManifestWebConfiguration
    {
        public int OpWebServerPort { get; set; }

        public string DesktopSocialLoginErrorUrlPath { get; set; }

        public string DesktopSocialLoginSuccessUrlPath { get; set; }

        public string ViewUrl { get; set; }

        public string ReadOnlyViewUrl { get; set; }

        public bool EnableFrontPageDisplayMessage { get; set; }

        public string FrontPageDisplayMessage { get; set; }

        public string FrontPageDisplayMessageTitle { get; set; }

        public int FrontPageDisplayMessageDisplayTime { get; set; }

        public DateTime? FrontPageDisplayMessageUntilDate { get; set; }

        public bool ReadOnlyAccess { get; set; }

        public string ReadOnlyAccessMessage { get; set; }

        public bool IsProduction { get; set; }

        public string WebsiteUrl { get; set; }

        public string LetterTemplateFileStorageConnectionString { get; set; }
    }
}