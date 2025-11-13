using System;

namespace Manifest.Common.Models.ViewModels.UserProfile
{
    public class IsPremiumFeatureEnabledModel
    {
        public bool Enabled { get; set; }

        public DateTime? EnabledUntilDate { get; set; }
    }
}