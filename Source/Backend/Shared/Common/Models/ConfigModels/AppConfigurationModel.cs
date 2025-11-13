using System.Text.Json.Serialization;
using Manifest.Common.Attributes;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Models.Config;

namespace Manifest.Common.Models.ConfigModels
{
    public class AppConfigurationModel : IBaseAppConfiguration
    {
        [JsonIgnore]
        public string AppVersion { get; set; }

        [JsonIgnore]
        public string AppName { get; set; }

        [JsonIgnore]
        public string ApplicationVersion { get; set; }

        [JsonIgnore]
        public string DatabaseVersion { get; set; }

        public bool EnableNewFeatures { get; set; }

        [ComplexConfigItem]
        public LogSettings LogSettings { get; set; }

        public int CacheHighRefreshInMinutes { get; set; }

        public int CachMediumRefreshInMinutes { get; set; }

        public int CachLowRefreshInMinutes { get; set; }

        public bool UseMiniProfiler { get; set; }

        public bool ShowAllLinksWithoutPermissionCheck { get; set; }
    }
}