using System;
using System.Linq;
using System.Threading.Tasks;
using Manifest.Repository.Common;
using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.Logging;

namespace Manifest.Service.App
{
    public class AppExecutionService : BaseAppService
    {
        private readonly SystemRepository systemRepository;

        public AppExecutionService(
            SystemRepository systemRepository,
            ILog log,
            ObjectMapperService objectMapperService)
            : base(log, objectMapperService)
        {
            this.systemRepository = systemRepository;
        }

        public async Task<bool> ExecuteAsync()
        {
            var versions = await systemRepository.GetAppExecutionVersionAsync();
            versions = versions.OrderBy(x => x.Version).ToList();
            var lastVersion = versions.Max(x => x.Version);

            if (lastVersion <= 0)
            {
                throw new Exception();
            }

            return true;
        }
    }
}