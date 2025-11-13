using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using Manifest.Common.Constants;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Interfaces.Web.Authentication;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.ViewModels.Account;
using Manifest.Repository.Common;
using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using OAuth2.Client;
using OAuth2.Client.Impl;
using OAuth2.Infrastructure;
using OAuth2.Models;

namespace Manifest.Web.Authentication
{
    public partial class SingleSignOnService : ISingleSignOnService
    {
        private readonly ILog log;
        private readonly ITokenGeneratorService tokenGeneratorService;
        private readonly CompanyRepository companyRepository;
        private readonly ObjectMapperService objectMapperService;

        public SingleSignOnService(ILog log, CompanyRepository companyRepository, ITokenGeneratorService tokenGeneratorService, ObjectMapperService objectMapperService)
        {
            this.log = log;
            this.companyRepository = companyRepository;
            this.tokenGeneratorService = tokenGeneratorService;
            this.objectMapperService = objectMapperService;
        }

        public async Task<ReturnModel<string>> GetProviderLoginUrlAsync(string singleSignOnIdentifier, string state, Uri callbackUri)
        {
            if (singleSignOnIdentifier == null)
            {
                return log.ErrorAndReturnModel<string>($"Invalid Company Details {singleSignOnIdentifier}", (int)HttpStatusCode.BadRequest);
            }

            var ssm = await companyRepository.GetSingleSignOnServerByCodeAsync(singleSignOnIdentifier);
            if (ssm == null || ssm.IsDeleted)
            {
                return ReturnModel<string>.Success("");
            }

            (var oauth, _) = GetOAuthProvider(ssm, callbackUri);
            if (oauth == null)
            {
                return log.ErrorAndReturnModel<string>($"OAuth2 Client details are empty {oauth} ", (int)HttpStatusCode.InternalServerError);
            }

            string authorizatioUrl = await oauth.GetLoginLinkUriAsync(ssm.SingleSignOnServerCode ?? "");

            if (string.IsNullOrEmpty(authorizatioUrl))
                return log.ErrorAndReturnModel<string>("Unexpected error occurred while getting the Authorization url." + (int)HttpStatusCode.BadRequest);

            return ReturnModel<string>.Success(authorizatioUrl);
        }

        public async Task<ReturnModel<LoggedInUserModel>> GetProviderEmailAndTokenAsync(ExternalLoginCallBackModel callBackModel)
        {
            if (callBackModel == null)
            {
                return log.ErrorAndReturnModel<LoggedInUserModel>("Invalid Authorization Details" + callBackModel, (int)HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrEmpty(callBackModel.CallBackUri))
            {
                return log.ErrorAndReturnModel<LoggedInUserModel>("Call Back URI Should not be empty." + callBackModel.CallBackUri, (int)HttpStatusCode.BadRequest);
            }

            if (callBackModel.CallBackUri == null || !ValidationUtils.IsUrlValid(callBackModel.CallBackUri))
            {
                return log.ErrorAndReturnModel<LoggedInUserModel>("Invalid Callback URL -" + callBackModel.CallBackUri, (int)HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrEmpty(callBackModel.Code))
            {
                return log.ErrorAndReturnModel<LoggedInUserModel>("Authorization Code should not be empty." + callBackModel.Code, (int)HttpStatusCode.BadRequest);
            }

            if (!ValidationUtils.IsGuidValid(callBackModel.State))
            {
                return log.ErrorAndReturnModel<LoggedInUserModel>("Invalid Authentication State." + callBackModel.State, (int)HttpStatusCode.BadRequest);
            }

            string auhorizationCode = callBackModel.Code;

            if (auhorizationCode.IsTrimmedStringNullOrEmpty())
            {
                if (!callBackModel.Id_token.IsTrimmedStringNullOrEmpty())
                {
                    auhorizationCode = callBackModel.Id_token;
                }
                else if (!callBackModel.Access_token.IsTrimmedStringNullOrEmpty())
                {
                    auhorizationCode = callBackModel.Access_token;
                }
                else
                {
                    return log.ErrorAndReturnModel<LoggedInUserModel>("Code is empty for External Login Callback." + callBackModel.Provider, (int)HttpStatusCode.BadRequest);
                }
            }

            var ssm = await companyRepository.GetSingleSignOnServerByCodeAsync(callBackModel.SingleSignOnIdentifier);
            if (ssm == null)
            {
                return log.ErrorAndReturnModel<LoggedInUserModel>("Invalid Authentication Provider Details");
            }

            UserInfo userInfo;
            (var oauth, _) = GetOAuthProvider(ssm, new Uri(callBackModel.CallBackUri));

            if (oauth != null)
            {
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    userInfo = await oauth.GetUserInfoAsync(new NameValueCollection { { "code", auhorizationCode } });
                }
                catch (Exception ex)
                {
                    return log.ErrorAndReturnModel<LoggedInUserModel>($"Unexpected error occurred when getting the user details from {callBackModel.Provider} " + ex, (int)HttpStatusCode.OK);
                }

                if (userInfo == null || userInfo.Email.IsTrimmedStringNullOrEmpty())
                {
                    return log.ErrorAndReturnModel<LoggedInUserModel>($"Unable to get the user details from : " + callBackModel.Provider, 200);
                }

                var member = await companyRepository.GetUserByUsernameAsync(userInfo.Email);
                if (member == null)
                {
                    return log.ErrorAndReturnModel<LoggedInUserModel>($"User with email: {userInfo.Email} is not available in the system.", 200);
                }

                if (!member.Active)
                {
                    return log.ErrorAndReturnModel<LoggedInUserModel>($"User: " + string.Concat(member.FirstName, " ", member.LastName) + " is not active in the system.", 200);
                }

                if (!ssm.SingleSignOnServerCode.IsSameWithCaseIgnoreTrimmed(callBackModel.SingleSignOnIdentifier))
                {
                    return log.ErrorAndReturnModel<LoggedInUserModel>($"User {userInfo.Email} does not belong the company {callBackModel.SingleSignOnIdentifier}");
                }

                if (!member.Active || member.IsDeleted)
                {
                    return log.ErrorAndReturnModel<LoggedInUserModel>($"User {userInfo.Email} is not active for company {callBackModel.SingleSignOnIdentifier}");
                }

                var userModel = tokenGeneratorService.GetUserToken(objectMapperService.UserToUserModel(member), 0);

                var result = ReturnModel<LoggedInUserModel>.Success(userModel);
                if (!result.IsSuccess)
                {
                    return log.ErrorAndReturnModel<LoggedInUserModel>($"Unable to login {userInfo.Email} with the details provided.");
                }

                return result;
            }

            return log.ErrorAndReturnModel<LoggedInUserModel>(SingleSignOnErrorCodeConstants.ProviderNotAvailable.ErrorDescription, (int)HttpStatusCode.InternalServerError);
        }

        private CustomGoogleClient GetGoogleClient(SingleSignOnServer auth, string redirectUrl)
        {
            return new CustomGoogleClient(new RequestFactory(), new OAuth2.Configuration.ClientConfiguration
            {
                ClientId = auth.ClientId?.Trim(),
                ClientSecret = auth.ClientSecret?.Trim(),
                RedirectUri = redirectUrl,
                Scope = "profile email"
            });
        }

        private WindowsLiveClient GetMicrosoftClient(SingleSignOnServer auth, string redirectUrl)
        {
            return new WindowsLiveClient(new RequestFactory(), new OAuth2.Configuration.ClientConfiguration
            {
                ClientId = auth.ClientId?.Trim(),
                ClientSecret = auth.ClientSecret?.Trim(),
                RedirectUri = redirectUrl,
                Scope = "wl.emails"
            });
        }

        private AzureADClient GetActiveDirectoryClient(SingleSignOnServer auth, string redirectUrl)
        {
            return new AzureADClient(
                new RequestFactory(),
                new OAuth2.Configuration.ClientConfiguration
                {
                    ClientId = auth.ClientId?.Trim(),
                    ClientSecret = auth.ClientSecret?.Trim(),
                    RedirectUri = redirectUrl,
                    Scope = "openid profile email"
                },
                null);
        }

        private (OAuth2Client oauth, string scope) GetOAuthProvider(SingleSignOnServer authLoginInfo, Uri callbackUri)
        {
            OAuth2Client oauth;
            string scope;
            try
            {
                switch ((PredefinedSingleSignOnServerTypes)authLoginInfo.PredefinedSingleSignOnServerTypeId)
                {
                    case PredefinedSingleSignOnServerTypes.Google:
                        {
                            oauth = GetGoogleClient(authLoginInfo, callbackUri.AbsoluteUri);
                            scope = "";
                            break;
                        }

                    case PredefinedSingleSignOnServerTypes.Microsoft:
                        {
                            oauth = GetMicrosoftClient(authLoginInfo, callbackUri.AbsoluteUri);
                            scope = "";
                            break;
                        }

                    case PredefinedSingleSignOnServerTypes.Office360ActiveDirectory:
                        {
                            oauth = GetActiveDirectoryClient(authLoginInfo, callbackUri.AbsoluteUri);
                            scope = "";
                            break;
                        }

                    default:
                        {
                            log.Error("Unable to get Authorization Url - " + authLoginInfo.ToString());
                            return (null, null);
                        }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Unable to get Authorization Url" + authLoginInfo.ToString());
                return (null, null);
            }

            return (oauth, scope);
        }
    }
}
