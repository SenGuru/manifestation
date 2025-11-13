using Manifest.Common.Constants;
using Manifest.Common.Interfaces.Service.Config;
using Framework.Data.DbAccess;

namespace Manifest.Service.Config
{
    public class CoordinatorConfiguration : DefaultConfigurationPopulator, ICoordinatorConfiguration
    {
        public CoordinatorConfiguration(IConfigDBManager appDBManager, AppConfigurationFile appConfigurationFile)
            : base(appDBManager, ConfigurationConstants.ConfigurationGroup.CoordinatorConfiguration, appConfigurationFile)
        {
            this.RefreshConfig();
        }

        public int CoordinatorServerPort { get; protected set; }

        public int CoordinatorServerName { get; protected set; }

        public int CoordinatorLockTimeoutInSeconds { get; protected set; }

        public string RetryQueueConnectionString { get; protected set; }

        public string ConfigNotificationQueueConnectionString { get; protected set; }
    }
}
