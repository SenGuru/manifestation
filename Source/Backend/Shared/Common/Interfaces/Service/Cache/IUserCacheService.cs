using System.Threading.Tasks;
using Manifest.Common.Models.DB;

namespace Manifest.Common.Interfaces.Service.Cache
{
    public interface IUserCacheService
    {
        Task<User> GetUserByUserNameAsync(string userName);

        Task<long?> GetUserIdByUserNameAsync(string userName);

        Task<bool> IsCurrentUserSuperAdmin();

        Task<bool> IsCurrentUserCompanyAdmin();

        Task<bool> IsCurrentUserCompanyConfigurator();

        Task<bool> IsCurrentUserLoggedIn();

        Task<SingleSignOnServer> GetUserSignInServerAsync();
    }
}