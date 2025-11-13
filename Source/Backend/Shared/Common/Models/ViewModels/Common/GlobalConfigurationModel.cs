using System;
using System.Collections.Generic;
using Manifest.Common.Models.ViewModels.Account;
using Framework.Infrastructure.Models;

namespace Manifest.Common.Models.ViewModels
{
    public class GlobalConfigurationModel
    {
        public bool IsUserLoggedIn { get; set; }

        public LoginOptionModel LoginOptionModel { get; set; }

        public string AppVersion { get; set; }

        public string DBVersion { get; set; }

        public DateTime? AppReleaseDate { get; set; }

        public string HelpUrl { get; set; }

        public List<TextWithValue> OnboardingCodes { get; set; }

        public bool UseMP { get; set; }

        public bool EnableNewFeatures { get; set; }

        public bool ShowAllLinksWithoutPermissionCheck { get; set; }
    }
}