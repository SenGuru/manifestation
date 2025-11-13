using System.Threading.Tasks;
using Manifest.Common.Models.DB;
using Manifest.Repository.Common;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Utils;
using Microsoft.Extensions.Caching.Memory;

namespace Manifest.Service.Cache
{
    public partial class CompanyCacheService// : ICompanyCacheService
    {
        private readonly string customerStorageConfigCacheKeyPrefix = "CompanyStorageConfig_CompanyId_";

        private readonly SystemRepository systemRepository;
        private readonly IMemoryCache cache;
        private readonly IBaseAppConfiguration appConfiguration;

        public CompanyCacheService(SystemRepository systemRepository, IMemoryCache cache, IBaseAppConfiguration appConfiguration)
        {
            this.systemRepository = systemRepository;
            this.cache = cache;
            this.appConfiguration = appConfiguration;
        }

        public async Task<CompanyStorageConfiguration> GetCompanyStorageAsync()
        {

            var key = customerStorageConfigCacheKeyPrefix;
            var part = await cache.GetAsync<CompanyStorageConfiguration>(key);
            if (part == null)
            {
                var result = await systemRepository.GetCompanyStorageConfigurationByCompanyIdAsync();
                if (result != null)
                {
                    await cache.SetAsync<CompanyStorageConfiguration>(key, result, appConfiguration.CachLowRefreshOption());
                }

                return result;
            }

            return part;
        }
    }
}
