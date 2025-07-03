using System;
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    public class GlobalSettingModel
    {
        public string Html5DocumentViewerPath { get; set; }

        public List<string> Html5SupportedDocTypes { get; set; }

        public string MobileTheme { get; set; }

        public string MobileTitle { get; set; }

        public string MobileCopyright { get; set; }

        public string DesktopTheme { get; set; }

        public string DesktopCourtUrl { get; set; }

        public string DesktopTitle { get; set; }

        public string DesktopCopyright { get; set; }

        public string ApplicationVersion { get; set; }

        public string DatabaseVersion { get; set; }

        public bool EnableNewFeatures { get; set; }

        public bool EnableFrontPageDisplayMessage { get; set; }

        public string FrontPageDisplayMessage { get; set; }

        public string FrontPageDisplayMessageTitle { get; set; }

        public int FrontPageDisplayMessageDisplayTime { get; set; }

        public DateTime? FrontPageDisplayMessageUntilDate { get; set; }

        public bool ReadOnlyAccess { get; set; }

        public string ReadOnlyAccessMessage { get; set; }

        public bool IsProduction { get; set; }

        public bool ShowAllLinksWithoutPermissionCheck { get; set; }
    }
}
