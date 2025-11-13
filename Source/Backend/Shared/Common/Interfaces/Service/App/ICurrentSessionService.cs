using System.Collections.Generic;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Models.DB;

namespace Manifest.Common.Interfaces.Service.App
{
    public interface ICurrentSessionService
    {
        Task<User> GetCurrentUserAsync();

        Task<long?> GetCurrentUserIdAsync();

        Task<long?> GetCurrentUserTypeIdAsync();

        Task<string> GetCurrentUserNameAsStringAsync();

        Task<List<long>> GetCurrentUserResourceIdsAsync();

        Task<List<UserPermission>> GetUserPermissionsByResourceIdAsync(long resourceId);

        Task<bool> IsCurrentUserASuperAdminAsync();

        Task<bool> IsCurrentUserACompanyAdminAsync();

        bool DoesUserHavePermission(params PredefinedUserPermissionTypes[] permissions);
    }
}