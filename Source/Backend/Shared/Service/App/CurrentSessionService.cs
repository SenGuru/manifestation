using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Interfaces.Service.Cache;
using Manifest.Common.Models.DB;
using Manifest.Service.Cache;
using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.Interfaces.Helpers;
using Framework.Infrastructure.Logging;

namespace Manifest.Service.App
{
    public class CurrentSessionService : BaseAppService, ICurrentSessionService
    {
        private readonly ICurrentUser currentUserService;
        //private readonly ICourtCacheService courtCacheService;
        private readonly IUserCacheService userCacheService;
        private readonly CompanyCacheService companyCacheService;
        private readonly List<KeyValuePair<string, string>> claims;

        public CurrentSessionService(ILog log, ICurrentUser currentUserService, CompanyCacheService companyCacheService, IUserCacheService userCacheService, ObjectMapperService objectMapperService)
            : base(log, objectMapperService)
        {
            this.currentUserService = currentUserService;
            //this.courtCacheService = courtCacheService;
            this.userCacheService = userCacheService;
            this.companyCacheService = companyCacheService;
            claims = currentUserService.GetAllClaimKeyValuePairs();
        }

        public Task<User> GetCurrentUserAsync()
        {
            return userCacheService.GetUserByUserNameAsync(currentUserService.GetCurrentUserName());
        }

        public async Task<bool> IsCurrentUserASuperAdminAsync()
        {
            var user = await userCacheService.GetUserByUserNameAsync(currentUserService.GetCurrentUserName());
            if (user == null)
                return false;
            return user.PredefinedUserTypeId == (int)PredefinedUserTypes.SuperAdmin;
        }

        public async Task<bool> IsCurrentUserACompanyAdminAsync()
        {
            var user = await userCacheService.GetUserByUserNameAsync(currentUserService.GetCurrentUserName());
            if (user == null)
                return false;
            return user.PredefinedUserTypeId == (int)PredefinedUserTypes.Manager;
        }

        public async Task<long?> GetCurrentUserIdAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return null;

            return user.UserId;
        }

        public async Task<long?> GetCurrentUserTypeIdAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return null;

            return user.PredefinedUserTypeId;
        }

        public async Task<string> GetCurrentUserNameAsStringAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return null;

            return user.UserName;
        }

        public Task<List<long>> GetCurrentUserResourceIdsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<UserPermission>> GetUserPermissionsByResourceIdAsync(long resourceId)
        {
            throw new NotImplementedException();
        }

        public bool DoesUserHavePermission(params PredefinedUserPermissionTypes[] permissions)
        {
            foreach (var permission in permissions)
            {
                if (claims.Any(x => x.Value == permission.ToString()))
                    return true;
            }

            return false;
        }
    }
}
