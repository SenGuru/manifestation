using Manifest.Common.Interfaces.Service.Config;
using Framework.Infrastructure.Config;

namespace Manifest.Service.Config
{
    public class SystemConfiguration : ISystemConfiguration
    {
        public SystemConfiguration(
            IManifestWebConfiguration opWebConfiguration,
            IBaseAppConfiguration appConfiguration,
            ICommonConfiguration commonConfiguration,
            ICoordinatorConfiguration coordinatorConfiguration,
            IReportServerConfiguration reportServerConfiguration)
        {
            ManifestWebConfiguration = opWebConfiguration;
            AppConfiguration = appConfiguration;
            CommonConfiguration = commonConfiguration;
            CoordinatorConfiguration = coordinatorConfiguration;
            ReportServerConfiguration = reportServerConfiguration;
        }

        public IManifestWebConfiguration ManifestWebConfiguration { get; private set; }

        public IBaseAppConfiguration AppConfiguration { get; private set; }

        public ICommonConfiguration CommonConfiguration { get; private set; }

        public ICoordinatorConfiguration CoordinatorConfiguration { get; private set; }

        public IReportServerConfiguration ReportServerConfiguration { get; private set; }
    }
}
