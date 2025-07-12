using DocumentFormat.OpenXml.Spreadsheet;
using Framework.Infrastructure.Exceptions;
using Framework.Infrastructure.Interfaces.Services;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Interfaces.Service.Cache;
using Manifest.Common.Interfaces.Service.Config;
using Manifest.Common.Interfaces.Web.Authentication;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.DBView;
using Manifest.Common.Models.ViewModels;
using Manifest.Common.Models.ViewModels.Account;
using Manifest.Repository.Common;
using Manifest.Service.ObjectMapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Manifest.Service.App
{
    public class AccountService : BaseAppService //, IAccountService
    {
        private readonly CompanyRepository companyRepository;
        private readonly SystemRepository systemRepository;
        private readonly CompanyService companyService;
        private readonly IUserCacheService userCacheService;
        private readonly ISystemConfiguration systemConfiguration;
        private readonly ITokenGeneratorService tokenGeneratorService;
        private readonly ICurrentSessionService currentSessionService;
        private readonly IEmailService emailService;
        private readonly ISingleSignOnService singleSignOnService;
        private readonly AuditService auditService;

        public AccountService(
            CompanyRepository companyRepository,
            SystemRepository systemRepository,
            ILog log,
            IUserCacheService userCacheService,
            ISystemConfiguration systemConfiguration,
            ObjectMapperService objectMapperService,
            ITokenGeneratorService tokenGeneratorService,
            AuditService auditService,
            ICurrentSessionService currentSessionService,
            IEmailService emailService,
            CompanyService companyService,
            ISingleSignOnService singleSignOnService)
            : base(log, objectMapperService)
        {
            this.companyRepository = companyRepository;
            this.systemRepository = systemRepository;
            this.userCacheService = userCacheService;
            this.systemConfiguration = systemConfiguration;
            this.tokenGeneratorService = tokenGeneratorService;
            this.auditService = auditService;
            this.currentSessionService = currentSessionService;
            this.emailService = emailService;
            this.companyService = companyService;
            this.singleSignOnService = singleSignOnService;
        }

        public async Task<ReturnModel<LoggedInUserModel>> AuthenticateAsync(string username, string password)
        {
            try
            {
                var result = await ValidateUserAsync(username, password, false);
                if (!result.IsSuccess)
                {
                    await auditService.LoginFailedAsync(username);
                    return ReturnModel<LoggedInUserModel>.Error(result.ErrorHolder, result.HttpCode);
                }

                return new ReturnModel<LoggedInUserModel>(tokenGeneratorService.GetUserToken(result.Model, 0));
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<LoggedInUserModel>("Unexpected error when authenticating user", ex);
            }
        }

        public Task<ReturnModel<bool>> GenerateOtpForAnonymousUser(AnonymousLoginModel model)
        {
            try
            {
                //var item = companyService.GetCrewDetailsWhichHasMatchingPhoneOrEmailAsync(model.AuthValue);
                //if (item?.Model == null)
                //{
                //    return Task.FromResult(ReturnModel<bool>.Error("User not found"));
                //}

                //generation of otp
                //...

                return Task.FromResult(new ReturnModel<bool>(true));
            }
            catch (Exception ex)
            {
                return Task.FromResult(log.ErrorAndReturnModel<bool>("Unexpected error when generating Otp for anonymous user", ex));
            }
        }

        //public async Task<ReturnModel<LoggedInUserModel>> AnonymousAuthenticateAsync(string username, string password)
        //{
        //    try
        //    {
        //        // 5 tables email/phone
        //        // email/phone anoymous login
        //        var result = await ValidateUserAsync(username, password, false);
        //        if (!result.IsSuccess)
        //        {
        //            await auditService.LoginFailedAsync(username);
        //            return ReturnModel<LoggedInUserModel>.Error(result.ErrorHolder, result.HttpCode);
        //        }

        //        var company = await companyRepository.GetCompanyByIdAsync(result.Model.CompanyId);
        //        return new ReturnModel<LoggedInUserModel>(tokenGeneratorService.GetUserToken(result.Model, company?.PredefinedCurrencyTypeId ?? 0));
        //    }
        //    catch (Exception ex)
        //    {
        //        return log.ErrorAndReturnModel<LoggedInUserModel>("Unexpected error when authenticating user", ex);
        //    }
        //}

        public async Task<ReturnModel<LoggedInUserModel>> AlternateAuthenticateAsync(string username, string password, string loginAsUsername)
        {
            try
            {
                var result = await ValidateUserAsync(username, password, true);
                if (!result.IsSuccess)
                {
                    await auditService.AlternateLoginFailed(username);
                    return ReturnModel<LoggedInUserModel>.Error(result.ErrorHolder, result.HttpCode);
                }

                var user = await userCacheService.GetUserByUserNameAsync(loginAsUsername);

                if (user == null)
                {
                    return log.WarnAndReturnModel<LoggedInUserModel>("Account not available.", $"Unable to find user {loginAsUsername}");
                }

                return new ReturnModel<LoggedInUserModel>(tokenGeneratorService.GetUserToken(objectMapperService.UserToUserModel(user), 0));
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<LoggedInUserModel>("Unexpected error when authenticating user", ex);
            }
        }

        public Task<ReturnModel<GlobalSettingModel>> GetGlobalSettingsBySubdomainAsync(string subdomain)
        {
            try
            {
                var result = new GlobalSettingModel();
                result.ApplicationVersion = systemConfiguration.AppConfiguration.ApplicationVersion;
                result.DatabaseVersion = systemConfiguration.AppConfiguration.DatabaseVersion;
                result.EnableNewFeatures = systemConfiguration.AppConfiguration.EnableNewFeatures;
                result.EnableFrontPageDisplayMessage = systemConfiguration.ManifestWebConfiguration.EnableFrontPageDisplayMessage;
                result.FrontPageDisplayMessage = systemConfiguration.ManifestWebConfiguration.FrontPageDisplayMessage;
                result.FrontPageDisplayMessageTitle = systemConfiguration.ManifestWebConfiguration.FrontPageDisplayMessageTitle;
                result.FrontPageDisplayMessageDisplayTime = systemConfiguration.ManifestWebConfiguration.FrontPageDisplayMessageDisplayTime;
                result.FrontPageDisplayMessageUntilDate = systemConfiguration.ManifestWebConfiguration.FrontPageDisplayMessageUntilDate;
                result.ReadOnlyAccess = systemConfiguration.ManifestWebConfiguration.ReadOnlyAccess;
                result.ReadOnlyAccessMessage = systemConfiguration.ManifestWebConfiguration.ReadOnlyAccessMessage;
                result.IsProduction = systemConfiguration.ManifestWebConfiguration.IsProduction;
                result.ShowAllLinksWithoutPermissionCheck = systemConfiguration.AppConfiguration.ShowAllLinksWithoutPermissionCheck;
                return Task.FromResult(ReturnModel<GlobalSettingModel>.Success(result));
            }
            catch (Exception ex)
            {
                return Task.FromResult(log.ErrorAndReturnModel<GlobalSettingModel>("Unexpected error when getting the Global Settings", ex));
            }
        }

        public async Task<ReturnModel<GlobalConfigurationModel>> GetSiteConfigurationAsync(string site, Func<string, Uri> urlCallbackGet, Func<long, Uri> desktopUrlCallbackGet, Func<long, Uri> mobileUrlCallbackGet)
        {
            try
            {
                DateTime? appReleaseDate = null;
                try
                {
                    appReleaseDate = File.GetCreationTime(Assembly.GetExecutingAssembly().Location);
                }
                catch (Exception ex2)
                {
                    log.Error(ex2, "Error when getting the App Release Date");
                }

                List<TextWithValue> onboardingCodes = null;
                var lo = await GetLoginOptionByCompanyAsync(urlCallbackGet, desktopUrlCallbackGet, mobileUrlCallbackGet);
                var glocalConfig = new GlobalConfigurationModel { LoginOptionModel = lo?.Model, AppVersion = systemConfiguration.AppConfiguration.ApplicationVersion, DBVersion = systemConfiguration.AppConfiguration.DatabaseVersion, AppReleaseDate = appReleaseDate, HelpUrl = systemConfiguration.CommonConfiguration.HelpUrl, OnboardingCodes = onboardingCodes, UseMP = false, EnableNewFeatures = systemConfiguration.AppConfiguration.EnableNewFeatures, ShowAllLinksWithoutPermissionCheck = systemConfiguration.AppConfiguration.ShowAllLinksWithoutPermissionCheck };
                return ReturnModel<GlobalConfigurationModel>.Success(glocalConfig);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<GlobalConfigurationModel>("Unexpected error when getting Site Configuration", ex);
            }
        }

        public async Task<ReturnModel<LoginOptionModel>> GetLoginOptionAsync(string userName, Func<string, Uri> urlCallbackGet, Func<long, Uri> desktopUrlCallbackGet, Func<long, Uri> mobileUrlCallbackGet)
        {
            try
            {
                return await GetLoginOptionByCompanyAsync(urlCallbackGet, desktopUrlCallbackGet, mobileUrlCallbackGet);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<LoginOptionModel>("Unexpected error when getting User Login Information", ex);
            }
        }

        public async Task<ReturnModel<bool>> ResetPasswordRequestAsync(string userName)
        {
            try
            {
                if (userName.IsTrimmedStringNullOrEmpty())
                    return log.ErrorAndReturnModel<bool>($"User Name is not valid");
                var user = await companyRepository.GetUserByEmailOrUserNameAsync(userName);
                if (user == null)
                    return log.ErrorAndReturnModel<bool>($"Unable to find the user - {userName}");
                if (!user.Active)
                    return log.ErrorAndReturnModel<bool>($"Account is disabled- {userName}");
                if (user.AccountLockedUntil > DateTime.UtcNow)
                    return log.ErrorAndReturnModel<bool>($"Account is temporarily locked. Try after {(user.AccountLockedUntil.Value - DateTime.UtcNow).ToReadableString()} ");

                var passwordResetGuid = Guid.NewGuid();
                //var param = new ForgotPasswordMessageTemplateParam { UserName = userName, ResetPwdLink = currentUser.GetUrlRequestLink() + "/auth/forgotpassword/" + passwordResetGuid };
                //var msg = await messageTemplateService.ForgotPasswordAsync(param);

                //if (msg == null)
                //    return ReturnModel<bool>.Error("Internal Error : Email template is not configured. Please call support.");

                await companyRepository.UpdatePasswordCodeAsync(user.UserId, passwordResetGuid, DateTime.UtcNow.AddHours(6), await currentSessionService.GetCurrentUserIdAsync());

                await auditService.ResetPasswordAsync(userName);

                //var eResult = await messageSenderService.SendMessageEmailAsync(user.Email, msg);

                //if (!eResult.IsSuccess)
                //    return log.ErrorAndReturnModel<bool>(eResult.ErrorHolder);

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>("Unexpected error when resetting the password", ex);
            }
        }

        public async Task<ReturnModel<bool>> UserRecoverForgotPasswordRequestAsync(RecoverPasswordRequestModel model)
        {
            try
            {
                var username = await companyRepository.GetUserByUserNameAsync(model.UserName);
                if (username == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find username -{model.UserName}");
                }

                if (!ValidationUtils.IsUrlValid(model.ClientRedirectURL))
                {
                    return log.ErrorAndReturnModel<bool>("Invalid or null URL");
                }

                var userToken = tokenGeneratorService.CreateExpiringUserToken(objectMapperService.UserToUserModel(username), expirationMinutes: 30);
                if (string.IsNullOrEmpty(userToken.Token))
                {
                    return log.ErrorAndReturnModel<bool>("Failed to generate user token");
                }

                if (!ValidationUtils.IsEmail(username.Email))
                {
                    return log.ErrorAndReturnModel<bool>("Email is incorrect");
                }

                var mailNotificationConfig = await systemRepository.GetDefaultMailServerAsync();
                if (mailNotificationConfig == null)
                {
                    return log.ErrorAndReturnModel<bool>("Critical Error: Mail server not configured, Contact System Admin");
                }

                var expirationTimeMinutes = DateTime.Now.AddMinutes(31) - DateTime.Now;
                var expirationMinutes = (int)expirationTimeMinutes.TotalMinutes;
                var emailBody = $@" <div style='color: black;'> Hi <b style='color: black;'>{username.UserName}</b>,<br/><br/>
                                        We received a request to reset your password. Click the link below to set a new password for your account:<br/><br/>
                                        <a href='{model.ClientRedirectURL}?token={userToken.Token}' style='background-color:#1F82F1; border: none; color: white; padding: 10px 20px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; margin: 4px 2px; cursor: pointer; border-radius: 8px;'>Reset Password</a><br/><br/></a><br/><br/>
                                        For security reasons, this link will expire in <b>{expirationMinutes} </b>minutes.<br/><br/>
                                        If you didn’t request a password reset, you can safely ignore this email.<br/><br/>
                                        Need help? Contact our support team at <a href='mailto:info@landessware.com' style='color: black;'>info@landessware.com</a>.<br/><br/>
                                        Stay secure,<br/>
                                        <b style='color: black;'>Landessware</b> </div> ";
                var email = await emailService.SendAsync(
                    mailNotificationConfig.Host,
                    (int)mailNotificationConfig.Port,
                    mailNotificationConfig.UserName,
                    mailNotificationConfig.Password,
                    true,
                    "App-Template",
                    mailNotificationConfig.SenderEmail,
                    username.UserName,
                    username.Email,
                    null,
                    null,
                    "Reset Your Password",
                    emailBody,
                    useMailKit: true,
                    isHtml: true);
                if (!ValidationUtils.IsEmail(username.Email))
                {
                    return log.ErrorAndReturnModel<bool>("Email is incorrect");
                }

                if (!email.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>("Critical Error: There were some issues when sending the email, please contact the system admin");
                }

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>("Unexpected error when generating Token for anonymous user", ex);
            }
        }

        public async Task<ReturnModel<bool>> SetUserNewPasswordAsync(ResetPasswordRequestModel model)
        {
            try
            {
                var (authValue, validUntil, ex) = tokenGeneratorService.GetUserInfoFromToken(model.Token);
                if (ex != null || DateTime.UtcNow > validUntil)
                {
                    return log.ErrorAndReturnModel<bool>("Invalid or expired token");
                }

                var user = await companyRepository.GetUserByUsernameAsync(authValue);

                if (user == null)
                    return log.ErrorAndReturnModel<bool>($"Unable to find user - {authValue}");

                var validationErrors = ValidatePassword(model.NewPassword);

                if (model.NewPassword != model.ConfirmPassword)
                {
                    return log.ErrorAndReturnModel<bool>("New password and confirm password do not match");
                }
                if (PasswordUtils.ValidatePassword(model.NewPassword, user.PasswordHash))
                {
                    return log.ErrorAndReturnModel<bool>("New password cannot be the same as the current password.");
                }
                if (validationErrors.Any())
                {
                    return log.ErrorAndReturnModel<bool>(string.Join(" ", validationErrors));
                }

                var newPassword = PasswordUtils.CreateHash(model.NewPassword, out var _);

                await companyRepository.UpdatePasswordAsync(user.UserId, newPassword, await currentSessionService.GetCurrentUserIdAsync());

                var mailNotificationConfig = await systemRepository.GetDefaultMailServerAsync();
                if (mailNotificationConfig == null)
                {
                    return log.ErrorAndReturnModel<bool>("Critical Error: Mail server not configured, Contact System Admin");
                }

                if (newPassword != null)
                {
                    var emailBody = $@" <div style=""color: black;"">
                                  <p>Hi <b>{user.UserName}</b>,</p>
                            <p>Your password has been successfully updated <br>. If you didn’t make this change, please secure your account immediately by resetting your password again or contacting support.<br><br> <a href=""{model.LoginRedirecturl}"" style=""background-color:#1F82F1; border: none; color: white; padding: 5px 20px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; margin: 1px 1px 4px 1px; cursor: pointer; border-radius: 8px;"">Login Now</a></p>
                            <p>Thanks,</p>
                            <p><b>Landessware</b></p> </div> ";
                    var email = await emailService.SendAsync(
                   mailNotificationConfig.Host,
                   (int)mailNotificationConfig.Port,
                   mailNotificationConfig.UserName,
                   mailNotificationConfig.Password,
                   true,
                   "App-Template",
                   mailNotificationConfig.SenderEmail,
                   user.UserName,
                   user.Email,
                   null,
                   null,
                   "Reset Successfully",
                   emailBody,
                   useMailKit: true,
                   isHtml: true);
                    if (!ValidationUtils.IsEmail(user.Email))
                    {
                        return log.ErrorAndReturnModel<bool>("Email is incorrect");
                    }

                    if (!email.IsSuccess)
                    {
                        return log.ErrorAndReturnModel<bool>("Critical Error: There were some issues when sending the email, please contact the system admin");
                    }
                }

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>("Unexpected error when generating Token for anonymous user", ex);
            }
        }

        public async Task<ReturnModel<bool>> ForgotPasswordRequestAsync(ForgotPasswordModel model)
        {
            try
            {
                var user = await companyRepository.GetUserByUsernameAsync(model.UserLogin);

                if (user == null)
                    return log.ErrorAndReturnModel<bool>($"Unable to find user - {model.UserLogin}");

                if (user.OneTimeToken != model.Token)
                    return log.ErrorAndReturnModel<bool>($"Token is not valid");

                if (user.OneTimeExpireDateTime <= DateTime.UtcNow)
                    return log.ErrorAndReturnModel<bool>($"Token has expired");

                if (model.Password.IsTrimmedStringNullOrEmpty())
                    return log.ErrorAndReturnModel<bool>($"Password is not valid.");

                if (model.Password != model.RetypedPassword)
                    return log.ErrorAndReturnModel<bool>($"Password does not match.");

                var erroLst = ValidatePassword(model.Password);

                if (!erroLst.IsNullOrEmpty())
                    return log.ErrorAndReturnModel<bool>($"Password format is not correct", erroLst.ToReturnErrorItemList());

                var newPassword = PasswordUtils.CreateHash(model.Password, out var _);

                user.OneTimeToken = null;
                user.OneTimeExpireDateTime = null;
                await companyRepository.UpdatePasswordAsync(user.UserId, newPassword, await currentSessionService.GetCurrentUserIdAsync());
                try
                {
                    var user_new = ObjectUtils.DeepCopy<User>(user);
                    user_new.OneTimeToken = null;
                    user_new.OneTimeExpireDateTime = null;
                    user_new.PasswordHash = newPassword;
                    user_new.PasswordChangedDate = DateTime.UtcNow;

                    await auditService.ForgotPasswordAsync();
                }
                catch (Exception ex)
                {
                    log.Error("Exception occured while adding auditlog :" + ex.ToString());
                }

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>("Unexpected error when forgot password", ex);
            }
        }

        public async Task<ReturnModel<User>> ValidAndGetUserFromSessionAsync(string sessionId)
        {
            try
            {
                var session = SafeUtils.Guid(sessionId, Guid.Empty);
                if (session == Guid.Empty)
                    return log.ErrorAndReturnModel<User>($"Invalid session Id");

                var user = await companyRepository.GetUserByPasswordResetCodeAsync(session);

                if (user == null)
                    return log.ErrorAndReturnModel<User>($"Unable to find the user for session - {sessionId}");

                if (!user.Active)
                    return log.ErrorAndReturnModel<User>($"Account is disabled");

                if (user.AccountLockedUntil > DateTime.UtcNow)
                    return log.ErrorAndReturnModel<User>($"Account is temprorily locked. Try after {(user.AccountLockedUntil.Value - DateTime.UtcNow).ToReadableString()} ");

                if (user.OneTimeExpireDateTime > DateTime.UtcNow)
                    return log.ErrorAndReturnModel<User>($"Reset code has expired.");

                return ReturnModel<User>.Success(user);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<User>("Unexpected error when checking if the change password session is valid", ex);
            }
        }

        public async Task<ReturnModel<bool>> IsChangePasswordSessionValidAsync(string sessionId)
        {
            try
            {
                var result = await ValidAndGetUserFromSessionAsync(sessionId);
                if (!result.IsSuccess)
                    return ReturnModel<bool>.Error(result.ErrorHolder);

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>("Unexpected error when checking if the change password session is valid", ex);
            }
        }

        public async Task<ReturnModel<LoggedInUserModel>> ChangePasswordAsync(string userLogin, string passwordHash, ChangePasswordModel model)
        {
            try
            {
                var user = await companyRepository.GetUserByUsernameAsync(userLogin);
                if (user == null)
                    return log.ErrorAndReturnModel<LoggedInUserModel>($"Unable to find user - {userLogin}");

                if (!PasswordUtils.ValidatePassword(model.CurrentPassword, user.PasswordHash))
                {
                    return log.ErrorAndReturnModel<LoggedInUserModel>($"Current Password is not valid.");
                }

                if (model.Password.IsTrimmedStringNullOrEmpty())
                    return log.ErrorAndReturnModel<LoggedInUserModel>($"Password is not valid.");

                if (model.Password != model.RetypedPassword)
                    return log.ErrorAndReturnModel<LoggedInUserModel>($"Password does not match.");

                var erroLst = ValidatePassword(model.Password);

                if (!erroLst.IsNullOrEmpty())
                    return log.ErrorAndReturnModel<LoggedInUserModel>($"Password format is not correct", erroLst.ToReturnErrorItemList());

                var newPassword = PasswordUtils.CreateHash(model.Password, out var _);

                await companyRepository.UpdatePasswordAsync(user.UserId, newPassword, await currentSessionService.GetCurrentUserIdAsync());

                await auditService.ChangePasswordAsync();

                var userModel = objectMapperService.UserToUserModel(user);
                //userModel.ExistingPermissionTypeIds = await userRepository.GetPermissionsForUserAsync(user.UserId);

                return ReturnModel<LoggedInUserModel>.Success(tokenGeneratorService.GetUserToken(userModel, 0));
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<LoggedInUserModel>("Unexpected error when changing the password", ex);
            }
        }

        public async Task<ReturnModel<LoggedInUserModel>> ChangePasswordAsync(ChangeForgotPasswordModel model)
        {
            try
            {
                var result = await ValidAndGetUserFromSessionAsync(model.SessionId);
                if (!result.IsSuccess)
                    return ReturnModel<LoggedInUserModel>.Error(result.ErrorHolder);

                return await ChangePasswordAsync(result.Model.UserName, result.Model.PasswordHash, new ChangePasswordModel { Password = model.Password, RetypedPassword = model.RetypedPassword });
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<LoggedInUserModel>("Unexpected error when changing the password", ex);
            }
        }

        public async Task<ReturnModel<bool>> LogoffAsync()
        {
            try
            {
                if (!await userCacheService.IsCurrentUserLoggedIn())
                    return ReturnModel<bool>.Success(false);

                await auditService.LogOutAsync();

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>("Unexpected error when logging out", ex);
            }
        }

        public async Task<ReturnModel<UserView>> GetUserViewByEmailAsync(string email)
        {
            try
            {
                var result = await companyRepository.GetUserViewByEmailAsync(email);
                if (result == null)
                    return ReturnModel<UserView>.Error("User does not exist", null, 404);

                return ReturnModel<UserView>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<UserView>("Unexpected error when changing the password", ex);
            }
        }

        public async Task<ReturnModel<LoggedInUserModel>> GetLoginDetailsWithTokenAsync(string accessToken)
        {
            try
            {
                if (accessToken.IsTrimmedStringNullOrEmpty())
                {
                    return log.ErrorAndReturnModel<LoggedInUserModel>($"Invalid UserToken provided");
                }

                var userDetails = await currentSessionService.GetCurrentUserAsync();
                if (userDetails == null)
                {
                    return log.ErrorAndReturnModel<LoggedInUserModel>($"Unable to retrieve user details.");
                }

                var userView = await companyRepository.GetUserViewByUserNameAsync(userDetails.UserName);
                if (userView == null)
                {
                    return log.ErrorAndReturnModel<LoggedInUserModel>($"Unable to retrieve user details.");
                }

                var result = tokenGeneratorService.GetUserToken(objectMapperService.UserViewToUserModel(userView),0);
                return ReturnModel<LoggedInUserModel>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<LoggedInUserModel>("Unexpected error when user details", ex);
            }
        }

        private async Task<ReturnModel<LoginOptionModel>> GetLoginOptionByCompanyAsync(Func<string, Uri> urlCallbackGet, Func<long, Uri> desktopUrlCallbackGet, Func<long, Uri> mobileUrlCallbackGet)
        {
            string signInUrl = null;

            var singleSignOnServer = await companyRepository.GetDefaultSingleSignOnServerAsync();

            if (singleSignOnServer != null)
            {
                var callbackUri = urlCallbackGet(singleSignOnServer.SingleSignOnServerCode);
                var sResult = await singleSignOnService.GetProviderLoginUrlAsync(singleSignOnServer.SingleSignOnServerCode, singleSignOnServer.SingleSignOnServerCode, callbackUri);
                if (sResult.IsSuccess)
                    signInUrl = sResult.Model;
            }

            return ReturnModel<LoginOptionModel>.Success(new LoginOptionModel { UseBuiltInAuthentication = signInUrl == null, SignInUrl = signInUrl, CompanyDesktopUrl = "", CompanyMobileUrl = "", CompanySlogan = "" });
        }

        private List<string> ValidatePassword(string newPass)
        {
            var errors = new List<string>();

            if (!newPass.HasCharactersCount(8, 16))
            {
                errors.Add("Password must be between 8 and 16 characters.");
            }

            if (!newPass.HasUpperCaseLetters(1, newPass.Length))
            {
                errors.Add("Password must contain at least one uppercase letter.");
            }

            if (!newPass.HasLowerCaseLetters(1, newPass.Length))
            {
                errors.Add("Password must contain at least one lowercase letter.");
            }

            if (!newPass.HasNumbers(1, newPass.Length))
            {
                errors.Add("Password must contain at least one number.");
            }

            return errors;
        }

        private async Task<ReturnModel<UserModel>> ValidateUserAsync(string username, string password, bool allowSuperAdmin)
        {
            if (username.IsTrimmedStringNullOrEmpty())
            {
                return log.WarnAndReturnModel<UserModel>("Username cannot be empty.");
            }

            if (password.IsTrimmedStringNullOrEmpty())
            {
                return log.WarnAndReturnModel<UserModel>("Password cannot be empty.");
            }

            var user = await companyRepository.GetUserViewByUsernameAsync(username);

            if (user == null)
            {
                return log.WarnAndReturnModel<UserModel>("Account not available.", $"Unable to find user {username}");
            }

            if (!user.Active)
            {
                return log.WarnAndReturnModel<UserModel>("Account is disabled. Please call support for help.", $"Account is disabled for user {username}");
            }

            if (!PasswordUtils.ValidatePassword(password, user.PasswordHash))
            {
                return log.WarnAndReturnModel<UserModel>("Password does not match.", $"Password does not match for {username}");
            }

            if (allowSuperAdmin)
            {
                if (user.PredefinedUserTypeId != (int)PredefinedUserTypes.SuperAdmin)
                {
                    return log.WarnAndReturnModel<UserModel>("Only Super Admin can use this interface.");
                }
            }

            await auditService.LoginAsync(user.UserId, user.UserName);

            var memberModel = objectMapperService.UserViewToUserModel(user);
            return ReturnModel<UserModel>.Success(memberModel);
        }

        
        public async Task<ReturnModel<LoggedInUserModel>> CreateUserAsync(AccountCreateModel accountModel)
        {
            if (accountModel == null)
            {
                return log.ErrorAndReturnModel<LoggedInUserModel>("Registration information is not valid");
            }

            var userModel = objectMapperService.AccountCreateModelToUserCreateModel(accountModel);
            userModel.PredefinedUserTypeId = (int)PredefinedUserTypes.User;
            userModel.UserName = userModel.Email;
            userModel.InitialPassword = accountModel.InitialPassword;

            var result = await companyService.CreateUserAsync(userModel, null, useAnonymnous: true);
            if (!result.IsSuccess)
                return log.ErrorAndReturnModel<LoggedInUserModel>(result);

            return await AuthenticateAsync(userModel.Email, userModel.InitialPassword);
        }
    }
}