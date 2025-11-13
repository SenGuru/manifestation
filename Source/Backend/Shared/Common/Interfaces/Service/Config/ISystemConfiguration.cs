using Framework.Infrastructure.Config;

namespace Manifest.Common.Interfaces.Service.Config
{
    public interface ISystemConfiguration
    {
        IBaseAppConfiguration AppConfiguration { get; }

        ICommonConfiguration CommonConfiguration { get; }

        ICoordinatorConfiguration CoordinatorConfiguration { get; }

        IManifestWebConfiguration ManifestWebConfiguration { get; }

        IReportServerConfiguration ReportServerConfiguration { get; }
    }
}