using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.Cache;
using Manifest.Common.Models.DB;
using Manifest.Repository.Common;
using Manifest.Service.App;
using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Interfaces.Helpers;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;
using Microsoft.Extensions.Caching.Distributed;

namespace Manifest.Service.Cache
{
    public class UserCacheService : BaseAppService, IUserCacheService
    {
        private readonly CompanyRepository companyRepository;
        private readonly string userNameCacheKeyPrefix = "UserCache_UserName_";
        private readonly IDistributedCache cache;
        private readonly IBaseAppConfiguration appConfiguration;
        private readonly ICurrentUser currentUser;
        private readonly CompanyCacheService companyCacheService;

        public UserCacheService(CompanyRepository companyRepository, IDistributedCache cache, IBaseAppConfiguration appConfiguration, ICurrentUser currentUser, ILog log, ObjectMapperService objectMapperService, CompanyCacheService companyCacheService)
         : base(log, objectMapperService)
        {
            this.companyRepository = companyRepository;
            this.cache = cache;
            this.appConfiguration = appConfiguration;
            this.currentUser = currentUser;
            this.companyCacheService = companyCacheService;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            if (userName == null || userName.Trim() == "")
            {
                return null;
            }

            var key = userNameCacheKeyPrefix + userName;
            var usr = await cache.GetAsync<User>(key);
            if (usr == null)
            {
                var result = await companyRepository.GetUserByUsernameAsync(userName);
                if (result != null)
                {
                    await cache.SetAsync<User>(key, result, appConfiguration.CachMediumRefreshOption());
                }

                return result;
            }

            return usr;
        }

        public async Task<long?> GetUserIdByUserNameAsync(string userName)
        {
            var usr = await GetUserByUserNameAsync(userName);
            if (usr == null)
                return 0;

            return usr.UserId;
        }

        public async Task<bool> IsUserSuperAdmin(string username)
        {
            if (username.IsTrimmedStringNullOrEmpty())
                return false;

            var usr = await GetUserByUserNameAsync(username);
            if (usr == null)
                return false;

            return usr.PredefinedUserTypeId == (int)PredefinedUserTypes.SuperAdmin;
        }

        public async Task<bool> IsUserHasRole(string username, PredefinedUserTypes role)
        {
            if (username.IsTrimmedStringNullOrEmpty())
                return false;

            var usr = await GetUserByUserNameAsync(username);
            if (usr == null)
                return false;

            return usr.PredefinedUserTypeId == (int)role;
        }

        public Task<bool> IsUserCompanyAdmin(string username)
        {
            return IsUserHasRole(username, PredefinedUserTypes.Manager);
        }

        public Task<bool> IsUserCompanyConfigurator(string username)
        {
            return IsUserHasRole(username, PredefinedUserTypes.Manager);
        }

        public Task<bool> IsCurrentUserSuperAdmin()
        {
            return IsUserSuperAdmin(currentUser.GetCurrentUserName());
        }

        public Task<bool> IsCurrentUserCompanyAdmin()
        {
            return IsUserCompanyAdmin(currentUser.GetCurrentUserName());
        }

        public Task<bool> IsCurrentUserCompanyConfigurator()
        {
            return IsUserCompanyConfigurator(currentUser.GetCurrentUserName());
        }

        public async Task<bool> IsCurrentUserLoggedIn()
        {
            var user = await GetUserByUserNameAsync(currentUser.GetCurrentUserName());
            if (user == null)
                return false;

            return true;
        }

        public async Task<SingleSignOnServer> GetUserSignInServerAsync()
        {
            var ss = await companyRepository.GetDefaultSingleSignOnServerAsync();

            if (ss == null)
            {
                log.Info($"Single Sign on is not configured");
                return null;
            }

            return ss;
        }
    }
}
