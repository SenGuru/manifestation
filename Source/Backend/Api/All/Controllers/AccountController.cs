using System.Threading.Tasks;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using Manifest.Common.Constants;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Interfaces.Service.Config;
using Manifest.Common.Models.ViewModels;
using Manifest.Common.Models.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using Microsoft.AspNetCore.Authentication;
using Manifest.Web.Authentication;
using Manifest.Common.Interfaces.Web.Authentication;
using Manifest.Service.App;
using BotDetect.Web;
using SecurityAssessment.Common.Models.ViewModels;

namespace Manifest.Api.All.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class AccountController : Controller
    {
        private const string CaptchaId = "ExampleCaptcha-b2e2e2e2-7c7a-4e2a-9b1a-2e6e2e2e2e2e";
        private readonly AccountService accountService;
        private readonly ILog log;
        private readonly ICurrentSessionService currentSessionService;
        private readonly IHttpContextAccessor httpContext;
        private readonly IManifestWebConfiguration auditDuoCallWebConfiguration;
        private readonly ISingleSignOnService singleSignOnService;

        public AccountController(
            IHttpContextAccessor httpContext,
            AccountService accountService,
            ILog log,
            ICurrentSessionService currentSessionService,
            IManifestWebConfiguration auditDuoWebConfiguration,
            ISingleSignOnService singleSignOnService)
        {
            this.httpContext = httpContext;
            this.accountService = accountService;
            this.log = log;
            this.currentSessionService = currentSessionService;
            this.auditDuoCallWebConfiguration = auditDuoWebConfiguration;
            this.singleSignOnService = singleSignOnService;
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public Task<ReturnModel<bool>> Ping()
        {
            return Task.FromResult(ReturnModel<bool>.Success(true));
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public Task<ReturnModel<bool>> AuthorizedPing()
        {
            if (this.httpContext.HttpContext.User.Identity.IsAuthenticated)
                return Task.FromResult(ReturnModel<bool>.Success(true));
            return Task.FromResult(ReturnModel<bool>.Error("User is not authenticated"));
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public Task<ReturnModel<LoggedInUserModel>> Login([FromBody] LoginModel loginModel)
        {
            return accountService.AuthenticateAsync(loginModel.Username, loginModel.Password);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public Task<ReturnModel<bool>> UserSetNewPassword(ResetPasswordRequestModel model)
        {
            return accountService.SetUserNewPasswordAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Account" })]

        public Task<ReturnModel<bool>> UserRecoverForgotPassword(RecoverPasswordRequestModel model)
        {
            return accountService.UserRecoverForgotPasswordRequestAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public Task<ReturnModel<bool>> GenerateOtpForAnonymousUser([FromBody] AnonymousLoginModel loginModel)
        {
            return accountService.GenerateOtpForAnonymousUser(loginModel);
        }

        //[HttpPost]
        //[SwaggerOperation(Tags = new[] { "Account" })]
        //public Task<ReturnModel<LoggedInUserModel>> AnonymousLogin([FromBody] AnonymousLoginModel loginModel)
        //{
        //    return accountService.AnonymousAuthenticateAsync(loginModel);
        //}

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public async Task<ReturnModel<GlobalConfigurationModel>> GetSiteConfiguration(string site)
        {
            if (site.IsTrimmedStringNullOrEmpty())
            {
                site = this.Request.Host.Host;
            }

            var config = await accountService.GetSiteConfigurationAsync(site, (string sid) => { return new Uri(Url.Action("ExternalLoginCallBack", "Account", new { id = sid }, Request.Scheme, Request.Host.ToUriComponent())); }, (long companyId) => { return new Uri(Url.Action("GetCompanyPicture", "Company", new { companyId }, Request.Scheme, Request.Host.ToUriComponent())); }, (long companyId) => { return new Uri(Url.Action("GetCompanyPicture", "Company", new { companyId }, Request.Scheme, Request.Host.ToUriComponent())); });
            if (config.IsSuccess)
            {
                config.Model.IsUserLoggedIn = this.Request?.HttpContext?.User != null;
            }

            return config;
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public Task<ReturnModel<LoginOptionModel>> GetLoginOption([FromQuery] string userName, [FromQuery] string callbackUrl)
        {
            return accountService.GetLoginOptionAsync(userName, (string sid) => { return callbackUrl.IsTrimmedStringNotNullOrEmpty() ? new Uri(callbackUrl.Replace("|sid|", sid), UriKind.RelativeOrAbsolute) : new Uri(Url.Action("ExternalLoginCallBack", "Account", new { id = sid }, Request.Scheme, Request.Host.ToUriComponent())); }, (long tenantId) => { return new Uri(Url.Action("GetDesktopProfilePicture", "Account", new { tenantId }, Request.Scheme, Request.Host.ToUriComponent())); }, (long tenantId) => { return new Uri(Url.Action("GetDesktopProfilePicture", "Account", new { tenantId }, Request.Scheme, Request.Host.ToUriComponent())); });
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public Task<ReturnModel<LoggedInUserModel>> AlternateLogin([FromBody] AlternateLoginModel loginModel)
        {
            return accountService.AlternateAuthenticateAsync(loginModel.Username, loginModel.Password, loginModel.LoginInAsUsername);
        }

        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public Task<ReturnModel<LoggedInUserModel>> ExternalLoginCallBack([FromBody] ExternalLoginCallBackModel callBackModel)
        {
            // callBackModel.CallBackUri = new Uri(Url.Action("ExternalLoginCallBack", "Account", new { id = id }, Request.Scheme)).ToString();
            return singleSignOnService.GetProviderEmailAndTokenAsync(callBackModel);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public Task<ReturnModel<bool>> ResetPassword([FromQuery] string userName)
        {
            return accountService.ResetPasswordRequestAsync(userName);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public Task<ReturnModel<bool>> IsChangePasswordSessionValid([FromQuery] string sessionId)
        {
            return accountService.IsChangePasswordSessionValidAsync(sessionId);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public Task<ReturnModel<LoggedInUserModel>> ChangePassword([FromBody] ChangeForgotPasswordModel model)
        {
            return accountService.ChangePasswordAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public Task<ReturnModel<bool>> ForgotPassword(ForgotPasswordModel model)
        {
            return accountService.ForgotPasswordRequestAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Account" })]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ReturnModel<LoggedInUserModel>> ChangeCurrentPassword([FromBody] ChangePasswordModel model)
        {
            return await accountService.ChangePasswordAsync(await currentSessionService.GetCurrentUserNameAsStringAsync(), EncryptionUtils.GetSHA256Hash(model.CurrentPassword), model);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public string DesktopSiteErrorUrl([FromQuery] string errorCode)
        {
            return $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host.Host}:{httpContext.HttpContext.Request.Host.Port}/" + auditDuoCallWebConfiguration.DesktopSocialLoginErrorUrlPath + "/" + errorCode;
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public string DesktopSiteExternalLoginUrl([FromQuery] string key)
        {
            return $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host.Host}:{httpContext.HttpContext.Request.Host.Port}/" + auditDuoCallWebConfiguration.DesktopSocialLoginErrorUrlPath + "/" + key;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public Task<ReturnModel<LoggedInUserModel>> LoginWithToken([FromQuery(Name = "access_token")] string access_token = null)
        {
            return accountService.GetLoginDetailsWithTokenAsync(access_token);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public Task<ReturnModel<GlobalSettingModel>> GlobalSettings([FromQuery] string subdomain)
        {
            return accountService.GetGlobalSettingsBySubdomainAsync(subdomain);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public IActionResult AzureADLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("AzureADCallback"),
                Items =
                {
                    { "LoginProvider", "AzureAD" }
                }
            };
            return Challenge(properties, "AzureAD");
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public async Task<IActionResult> AzureADCallback()
        {
            // Handle the callback
            var result = await HttpContext.AuthenticateAsync("ExternalCookie");
            return Redirect("/");
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Account" })]
        public async Task<ReturnModel<LoggedInUserModel>> CreateAccount([FromForm] UserCreateModel companyModel)
        {
            //SimpleCaptcha captcha = new SimpleCaptcha(CaptchaId);
            //bool isValid = captcha.Validate(companyModel.CaptchaCode);
            //if (!isValid)
            //{
            //    return log.WarnAndReturnModel<LoggedInUserModel>("Unable to validate if you are a human.");
            //}

            return await accountService.CreateUserAsync(companyModel);
        }

        [HttpGet]
        public Task<ReturnModel<CaptchaUrlResponseModel>> GenerateCaptchUrl()
        {
            SimpleCaptcha captchaHandler = new SimpleCaptcha(CaptchaId);
            return Task<ReturnModel<CaptchaUrlResponseModel>>.FromResult(ReturnModel<CaptchaUrlResponseModel>.Success(new CaptchaUrlResponseModel { CaptchaImageUrl = captchaHandler.CaptchaImageUrl, CaptchaSoundUrl = captchaHandler.CaptchaSoundUrl, ReloadIconUrl = captchaHandler.ReloadIconUrl, SoundIconUrl = captchaHandler.SoundIconUrl }));
        }
    }
}