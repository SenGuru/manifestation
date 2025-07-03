using System;
using System.Threading.Tasks;
using Manifest.Common.Models.DBView;
using Manifest.Common.Models.ViewModels;
using Manifest.Common.Models.ViewModels.Account;
using Framework.Infrastructure.Models.Result;

namespace Manifest.Common.Interfaces.Service.App
{
    public interface IAccountService
    {
        Task<ReturnModel<LoggedInUserModel>> AuthenticateAsync(string username, string password);

        Task<ReturnModel<LoggedInUserModel>> AlternateAuthenticateAsync(string username, string password, string loginAsUsername);

        Task<ReturnModel<GlobalSettingModel>> GetGlobalSettingsBySubdomainAsync(string subdomain);

        Task<ReturnModel<GlobalConfigurationModel>> GetSiteConfigurationAsync(string site, Func<string, Uri> urlCallbackGet, Func<long, Uri> desktopUrlCallbackGet, Func<long, Uri> mobileUrlCallbackGet);

        Task<ReturnModel<bool>> ResetPasswordRequestAsync(string userName);

        Task<ReturnModel<bool>> IsChangePasswordSessionValidAsync(string sessionId);

        Task<ReturnModel<LoggedInUserModel>> ChangePasswordAsync(ChangeForgotPasswordModel model);

        Task<ReturnModel<LoggedInUserModel>> ChangePasswordAsync(string userLogin, string passwordHash, ChangePasswordModel model);

        Task<ReturnModel<bool>> ForgotPasswordRequestAsync(ForgotPasswordModel model);

        Task<ReturnModel<UserView>> GetUserViewByEmailAsync(string email);

        Task<ReturnModel<LoggedInUserModel>> GetLoginDetailsWithTokenAsync(string accessToken);
    }
}
