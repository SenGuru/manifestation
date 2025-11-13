using Manifest.Common.Interfaces.Service.Config;

namespace Manifest.Common.Models.ConfigModels
{
    public class CoordinatorConfigurationModel : ICoordinatorConfiguration
    {
        public int CoordinatorServerPort { get; set; }

        public int CoordinatorServerName { get; set; }

        public int CoordinatorLockTimeoutInSeconds { get; set; }

        public string RetryQueueConnectionString { get; set; }

        public string ConfigNotificationQueueConnectionString { get; set; }
    }
}
