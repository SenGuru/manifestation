using System;

namespace Manifest.Common.Interfaces.Service.Config
{
    public interface IManifestWebConfiguration
    {
        string DesktopSocialLoginErrorUrlPath { get; }

        string DesktopSocialLoginSuccessUrlPath { get; }

        bool EnableFrontPageDisplayMessage { get; }

        string FrontPageDisplayMessage { get; }

        int FrontPageDisplayMessageDisplayTime { get; }

        string FrontPageDisplayMessageTitle { get; }

        DateTime? FrontPageDisplayMessageUntilDate { get; }

        bool IsProduction { get; }

        int OpWebServerPort { get; }

        bool ReadOnlyAccess { get; }

        string ReadOnlyAccessMessage { get; }

        string WebsiteUrl { get; }

        string LetterTemplateFileStorageConnectionString { get; }
    }
}