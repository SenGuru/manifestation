namespace Manifest.Common.Interfaces.Service.Config
{
    public interface ICoordinatorConfiguration
    {
        string ConfigNotificationQueueConnectionString { get; }

        int CoordinatorLockTimeoutInSeconds { get; }

        int CoordinatorServerName { get; }

        int CoordinatorServerPort { get; }

        string RetryQueueConnectionString { get; }
    }
}