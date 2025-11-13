using System.Linq;
using System.Threading.Tasks;
using Manifest.Repository.Common;
using Framework.Infrastructure.Interfaces.DbAccess;
using Framework.Infrastructure.Utils;

namespace SecurityAssessment.Service.DataUpdate
{
    public class DataUpdateService
    {
        private readonly SystemRepository systemRepository;
        private readonly ITransactionManager transactionManager;

        public DataUpdateService(SystemRepository systemRepository, ITransactionManager transactionManager)
        {
            this.systemRepository = systemRepository;
            this.transactionManager = transactionManager;
        }

        public async Task<long> RunUpdaterAsync()
        {
            var currentVersion = await GetCurrentAppVersionAsync();

            if (currentVersion < 1)
            {
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    await RunFirstUpdaterAsync();
                    await systemRepository.UpdateAppExecutionVersionAsync(1, null);
                    currentVersion = await GetCurrentAppVersionAsync();
                    await trx.CompleteAsync();
                }
            }

            if (currentVersion < 2)
            {
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    RunSecondUpdater();
                    await systemRepository.UpdateAppExecutionVersionAsync(2, null);
                    currentVersion = await GetCurrentAppVersionAsync();
                    await trx.CompleteAsync();
                }
            }

            return currentVersion;
        }

        private Task RunFirstUpdaterAsync()
        {
#pragma warning disable S3626 // Jump statements should not be redundant
            return Task.CompletedTask;
#pragma warning restore S3626 // Jump statements should not be redundant
        }

        private void RunSecondUpdater()
        {
#pragma warning disable S3626 // Jump statements should not be redundant
            return;
#pragma warning restore S3626 // Jump statements should not be redundant
        }

        private async Task<long> GetCurrentAppVersionAsync()
        {
            var currentVersions = await systemRepository.GetAllAppExecutionVersions();
            if (currentVersions.IsNullOrEmpty())
            {
                return 0;
            }

            return currentVersions.Max(x => x.Version);
        }
    }
}
