using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using Framework.TestUtilies;

namespace Manifest.Tests.Common
{
    public class HttpService<T> : IDisposable
    {
        private readonly string site;
        private readonly FrameworkTestEnvironment<T> env;
        private bool disposedValue;
        public HttpService(string site, FrameworkTestEnvironment<T> env)
        {
            this.site = site ?? "";
            this.env = env;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
         public string Url(string fragment)
         {
             return RestClientUtils.Url(site, fragment);
         }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> Ping(string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Object>(token,Url("/api/v1/account/ping"),null,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> AuthorizedPing(string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Object>(token,Url("/api/v1/account/authorizedping"),null,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<LoggedInUserModel> result)> Login(LoginModel loginModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<LoggedInUserModel>,LoginModel>(token,Url("/api/v1/account/login"),loginModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> UserSetNewPassword(ResetPasswordRequestModel model, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,ResetPasswordRequestModel>(token,Url("/api/v1/account/usersetnewpassword"),model,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> UserRecoverForgotPassword(RecoverPasswordRequestModel model, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,RecoverPasswordRequestModel>(token,Url("/api/v1/account/userrecoverforgotpassword"),model,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> GenerateOtpForAnonymousUser(AnonymousLoginModel loginModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,AnonymousLoginModel>(token,Url("/api/v1/account/generateotpforanonymoususer"),loginModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<GlobalConfigurationModel> result)> GetSiteConfiguration(String site, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<GlobalConfigurationModel>,Object>(token,Url("/api/v1/account/getsiteconfiguration"),new { site },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<LoginOptionModel> result)> GetLoginOption(String userName, String callbackUrl, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<LoginOptionModel>,Object>(token,Url("/api/v1/account/getloginoption"),new {userName, callbackUrl},appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<LoggedInUserModel> result)> AlternateLogin(AlternateLoginModel loginModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<LoggedInUserModel>,AlternateLoginModel>(token,Url("/api/v1/account/alternatelogin"),loginModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<LoggedInUserModel> result)> ExternalLoginCallBack(ExternalLoginCallBackModel callBackModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<LoggedInUserModel>,ExternalLoginCallBackModel>(token,Url("/api/v1/account/externallogincallback"),callBackModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> ResetPassword(String userName, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Object>(token,Url("/api/v1/account/resetpassword"),new { userName },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> IsChangePasswordSessionValid(String sessionId, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Object>(token,Url("/api/v1/account/ischangepasswordsessionvalid"),new { sessionId },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<LoggedInUserModel> result)> ChangePassword(ChangeForgotPasswordModel model, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<LoggedInUserModel>,ChangeForgotPasswordModel>(token,Url("/api/v1/account/changepassword"),model,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> ForgotPassword(ForgotPasswordModel model, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,ForgotPasswordModel>(token,Url("/api/v1/account/forgotpassword"),model,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<LoggedInUserModel> result)> ChangeCurrentPassword(ChangePasswordModel model, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<LoggedInUserModel>,ChangePasswordModel>(token,Url("/api/v1/account/changecurrentpassword"),model,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<LoggedInUserModel> result)> LoginWithToken(String access_token, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<LoggedInUserModel>,Object>(token,Url("/api/v1/account/loginwithtoken"),new { access_token },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<GlobalSettingModel> result)> GlobalSettings(String subdomain, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<GlobalSettingModel>,Object>(token,Url("/api/v1/account/globalsettings"),new { subdomain },appHeader);
        }

        public virtual Task<HttpResponseMessage> AzureADLogin(string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetWithNewTokenClientAsync<Object>(token,Url("/api/v1/account/azureadlogin"),null,appHeader);
        }

        public virtual Task<HttpResponseMessage> AzureADCallback(string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetWithNewTokenClientAsync<Object>(token,Url("/api/v1/account/azureadcallback"),null,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<AppLogModel> result)> GetAppLogs(AppLogSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<AppLogModel>,AppLogSearchCriteria>(token,Url("/api/v1/adminutils/getapplogs"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<String> result)> GetLogUniqueSeverityTypes(string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<String>,Object>(token,Url("/api/v1/adminutils/getloguniqueseveritytypes"),null,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<String> result)> GetLogUniqueFunctionTypes(string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<String>,Object>(token,Url("/api/v1/adminutils/getloguniquefunctiontypes"),null,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<String> result)> GetLogUniqueFileNames(string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<String>,Object>(token,Url("/api/v1/adminutils/getloguniquefilenames"),null,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<PerfLogModel> result)> GetPerfLogs(PerfLogSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<PerfLogModel>,PerfLogSearchCriteria>(token,Url("/api/v1/adminutils/getperflogs"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<PerfLogModel> result)> GetAppFunctionAggregateLogs(AppFunctionAggregateSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<PerfLogModel>,AppFunctionAggregateSearchCriteria>(token,Url("/api/v1/adminutils/getappfunctionaggregatelogs"),search,appHeader);
        }

        public virtual Task<HttpResponseMessage> ExportMovieSceneArtistsInJsonLFormat(string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetWithNewTokenClientAsync<Object>(token,Url("/api/v1/backendtest/exportmoviesceneartistsinjsonlformat"),null,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<ChatModel> result)> GetChats(ChatSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<ChatModel>,ChatSearchCriteria>(token,Url("/api/v1/chat/getchats"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ChatModel> result)> GetChat(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ChatModel>,Object>(token,Url("/api/v1/chat/getchat"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ChatModel> result)> CreateChat(ChatCreateModel chatModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ChatModel>,ChatCreateModel>(token,Url("/api/v1/chat/createchat"),chatModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<ChatModel> result)> CreateMultipleChat(MultipleChatCreateModel chatModels, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<ChatModel>,MultipleChatCreateModel>(token,Url("/api/v1/chat/createmultiplechat"),chatModels,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ChatModel> result)> UpdateChat(ChatUpdateModel chatModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ChatModel>,ChatUpdateModel>(token,Url("/api/v1/chat/updatechat"),chatModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkChatAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/chat/markchatasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkChatAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/chat/markchatasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<ChatMessageModel> result)> GetChatMessages(ChatMessageSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<ChatMessageModel>,ChatMessageSearchCriteria>(token,Url("/api/v1/chat/getchatmessages"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ChatMessageModel> result)> GetChatMessage(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ChatMessageModel>,Object>(token,Url("/api/v1/chat/getchatmessage"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ChatMessageModel> result)> CreateChatMessage(ChatMessageCreateModel chatMessageModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ChatMessageModel>,ChatMessageCreateModel>(token,Url("/api/v1/chat/createchatmessage"),chatMessageModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<ChatMessageModel> result)> CreateMultipleChatMessage(MultipleChatMessageCreateModel chatMessageModels, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<ChatMessageModel>,MultipleChatMessageCreateModel>(token,Url("/api/v1/chat/createmultiplechatmessage"),chatMessageModels,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ChatMessageModel> result)> UpdateChatMessage(ChatMessageUpdateModel chatMessageModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ChatMessageModel>,ChatMessageUpdateModel>(token,Url("/api/v1/chat/updatechatmessage"),chatMessageModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkChatMessageAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/chat/markchatmessageasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkChatMessageAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/chat/markchatmessageasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<ChatMessageStatusModel> result)> GetChatMessageStatuss(ChatMessageStatusSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<ChatMessageStatusModel>,ChatMessageStatusSearchCriteria>(token,Url("/api/v1/chat/getchatmessagestatuss"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ChatMessageStatusModel> result)> GetChatMessageStatus(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ChatMessageStatusModel>,Object>(token,Url("/api/v1/chat/getchatmessagestatus"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ChatMessageStatusModel> result)> CreateChatMessageStatus(ChatMessageStatusCreateModel chatMessageStatusModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ChatMessageStatusModel>,ChatMessageStatusCreateModel>(token,Url("/api/v1/chat/createchatmessagestatus"),chatMessageStatusModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<ChatMessageStatusModel> result)> CreateMultipleChatMessageStatus(MultipleChatMessageStatusCreateModel chatMessageStatusModels, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<ChatMessageStatusModel>,MultipleChatMessageStatusCreateModel>(token,Url("/api/v1/chat/createmultiplechatmessagestatus"),chatMessageStatusModels,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ChatMessageStatusModel> result)> UpdateChatMessageStatus(ChatMessageStatusUpdateModel chatMessageStatusModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ChatMessageStatusModel>,ChatMessageStatusUpdateModel>(token,Url("/api/v1/chat/updatechatmessagestatus"),chatMessageStatusModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkChatMessageStatusAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/chat/markchatmessagestatusasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkChatMessageStatusAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/chat/markchatmessagestatusasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<ChatUserModel> result)> GetChatUsers(ChatUserSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<ChatUserModel>,ChatUserSearchCriteria>(token,Url("/api/v1/chat/getchatusers"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ChatUserModel> result)> GetChatUser(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ChatUserModel>,Object>(token,Url("/api/v1/chat/getchatuser"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ChatUserModel> result)> CreateChatUser(ChatUserCreateModel chatUserModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ChatUserModel>,ChatUserCreateModel>(token,Url("/api/v1/chat/createchatuser"),chatUserModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<ChatUserModel> result)> CreateMultipleChatUser(MultipleChatUserCreateModel chatUserModels, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<ChatUserModel>,MultipleChatUserCreateModel>(token,Url("/api/v1/chat/createmultiplechatuser"),chatUserModels,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ChatUserModel> result)> UpdateChatUser(ChatUserUpdateModel chatUserModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ChatUserModel>,ChatUserUpdateModel>(token,Url("/api/v1/chat/updatechatuser"),chatUserModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkChatUserAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/chat/markchatuserasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkChatUserAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/chat/markchatuserasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> UploadUserPicture(UserPictureWithFileFormModel model, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsFormAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,UserPictureWithFileFormModel>(token,Url("/api/v1/company/uploaduserpicture"),model,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> ClearUserPicture(Int64 userId, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/company/clearuserpicture"),new { userId },appHeader);
        }

        public virtual Task<HttpResponseMessage> GetUserPicture(Int64 userId, String profileGuid, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetWithNewTokenClientAsync<Object>(token,Url("/api/v1/company/getuserpicture"),new {userId, profileGuid},appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<UserModel> result)> GetUsers(UserSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<UserModel>,UserSearchCriteria>(token,Url("/api/v1/company/getusers"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<UserModel> result)> CreateUser(UserCreateWithFileFormModel userModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsFormAndReadAsJsonWithNewTokenClientAsync<ReturnModel<UserModel>,UserCreateWithFileFormModel>(token,Url("/api/v1/company/createuser"),userModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<UserModel> result)> GetUser(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<UserModel>,Object>(token,Url("/api/v1/company/getuser"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<UserModel> result)> UpdateUserByAdmin(UserUpdateWithFileFormModel userModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsFormAndReadAsJsonWithNewTokenClientAsync<ReturnModel<UserModel>,UserUpdateWithFileFormModel>(token,Url("/api/v1/company/updateuserbyadmin"),userModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> EnableUserById(Int64 userId, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Object>(token,Url("/api/v1/company/enableuserbyid"),new { userId },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> DisableUserById(Int64 userId, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Object>(token,Url("/api/v1/company/disableuserbyid"),new { userId },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkUserAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/company/markuserasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkUserAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/company/markuserasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<UserPermissionModel> result)> GetUserPermissions(UserPermissionSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<UserPermissionModel>,UserPermissionSearchCriteria>(token,Url("/api/v1/company/getuserpermissions"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<UserPermissionModel> result)> GetUserPermission(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<UserPermissionModel>,Object>(token,Url("/api/v1/company/getuserpermission"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<UserPermissionModel> result)> CreateUserPermission(UserPermissionCreateModel userPermissionModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<UserPermissionModel>,UserPermissionCreateModel>(token,Url("/api/v1/company/createuserpermission"),userPermissionModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<UserPermissionModel> result)> UpdateUserPermission(UserPermissionUpdateModel userPermissionModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<UserPermissionModel>,UserPermissionUpdateModel>(token,Url("/api/v1/company/updateuserpermission"),userPermissionModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkUserPermissionAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/company/markuserpermissionasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkUserPermissionAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/company/markuserpermissionasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<UserGroupModel> result)> GetUserGroups(UserGroupSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<UserGroupModel>,UserGroupSearchCriteria>(token,Url("/api/v1/company/getusergroups"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<UserGroupModel> result)> GetUserGroup(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<UserGroupModel>,Object>(token,Url("/api/v1/company/getusergroup"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<UserGroupModel> result)> CreateUserGroup(UserGroupCreateModel userGroupModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<UserGroupModel>,UserGroupCreateModel>(token,Url("/api/v1/company/createusergroup"),userGroupModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<UserGroupModel> result)> CreateMultipleUserGroup(MultipleUserGroupCreateModel userGroupModels, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<UserGroupModel>,MultipleUserGroupCreateModel>(token,Url("/api/v1/company/createmultipleusergroup"),userGroupModels,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<UserGroupModel> result)> UpdateUserGroup(UserGroupUpdateModel userGroupModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<UserGroupModel>,UserGroupUpdateModel>(token,Url("/api/v1/company/updateusergroup"),userGroupModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkUserGroupAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/company/markusergroupasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkUserGroupAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/company/markusergroupasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<FileAttachmentFolderModel> result)> GetFileAttachmentFolders(FileAttachmentFolderSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<FileAttachmentFolderModel>,FileAttachmentFolderSearchCriteria>(token,Url("/api/v1/movie/getfileattachmentfolders"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<FileAttachmentFolderModel> result)> GetFileAttachmentFolder(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<FileAttachmentFolderModel>,Object>(token,Url("/api/v1/movie/getfileattachmentfolder"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<FileAttachmentFolderModel> result)> CreateFileAttachmentFolder(FileAttachmentFolderCreateModel fileAttachmentFolderModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<FileAttachmentFolderModel>,FileAttachmentFolderCreateModel>(token,Url("/api/v1/movie/createfileattachmentfolder"),fileAttachmentFolderModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<FileAttachmentFolderModel> result)> CreateMultipleFileAttachmentFolder(MultipleFileAttachmentFolderCreateModel fileAttachmentFolderModels, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<FileAttachmentFolderModel>,MultipleFileAttachmentFolderCreateModel>(token,Url("/api/v1/movie/createmultiplefileattachmentfolder"),fileAttachmentFolderModels,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<FileAttachmentFolderModel> result)> UpdateFileAttachmentFolder(FileAttachmentFolderUpdateModel fileAttachmentFolderModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<FileAttachmentFolderModel>,FileAttachmentFolderUpdateModel>(token,Url("/api/v1/movie/updatefileattachmentfolder"),fileAttachmentFolderModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkFileAttachmentFolderAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/movie/markfileattachmentfolderasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkFileAttachmentFolderAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/movie/markfileattachmentfolderasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<FileAttachmentModel> result)> GetFileAttachments(FileAttachmentSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<FileAttachmentModel>,FileAttachmentSearchCriteria>(token,Url("/api/v1/movie/getfileattachments"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<FileAttachmentModel> result)> GetFileAttachment(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<FileAttachmentModel>,Object>(token,Url("/api/v1/movie/getfileattachment"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<FileAttachmentModel> result)> CreateFileAttachment(FileAttachmentCreateWithFileFormModel model, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsFormAndReadAsJsonWithNewTokenClientAsync<ReturnModel<FileAttachmentModel>,FileAttachmentCreateWithFileFormModel>(token,Url("/api/v1/movie/createfileattachment"),model,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<FileAttachmentModel> result)> UpdateFileAttachment(FileAttachmentUpdateModel fileAttachmentModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<FileAttachmentModel>,FileAttachmentUpdateModel>(token,Url("/api/v1/movie/updatefileattachment"),fileAttachmentModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkFileAttachmentAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/movie/markfileattachmentasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkFileAttachmentAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/movie/markfileattachmentasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> IsFileAttachmentFileValid(Int64 fileAttachmentId, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Object>(token,Url("/api/v1/movie/isfileattachmentfilevalid"),new { fileAttachmentId },appHeader);
        }

        public virtual Task<HttpResponseMessage> DownloadFileAttachmentFile(Int64 fileAttachmentId, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetWithNewTokenClientAsync<Object>(token,Url("/api/v1/movie/downloadfileattachmentfile"),new { fileAttachmentId },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<CountryModel> result)> GetSupportedCountries(string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<CountryModel>,Object>(token,Url("/api/v1/system/getsupportedcountries"),null,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<CountryModel> result)> GetCountriesByKeyword(String keyword, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<CountryModel>,Object>(token,Url("/api/v1/system/getcountriesbykeyword"),new { keyword },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<StateModel> result)> GetStates(StateSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<StateModel>,StateSearchCriteria>(token,Url("/api/v1/system/getstates"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<StateModel> result)> GetState(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<StateModel>,Object>(token,Url("/api/v1/system/getstate"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<LogConfigurationModel> result)> GetLogConfigurationAsync(string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<LogConfigurationModel>,Object>(token,Url("/api/v1/system/getlogconfiguration"),null,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> SaveLogConfigurationAsync(LogConfigurationModel obj, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,LogConfigurationModel>(token,Url("/api/v1/system/savelogconfiguration"),obj,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ManifestWebConfigurationModel> result)> GetWebConfigurationAsync(string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ManifestWebConfigurationModel>,Object>(token,Url("/api/v1/system/getwebconfiguration"),null,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> SaveWebConfigurationAsync(ManifestWebConfigurationModel obj, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,ManifestWebConfigurationModel>(token,Url("/api/v1/system/savewebconfiguration"),obj,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<AppConfigurationModel> result)> GetAppConfigurationAsync(string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<AppConfigurationModel>,Object>(token,Url("/api/v1/system/getappconfiguration"),null,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> SaveAppConfigurationAsync(AppConfigurationModel obj, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,AppConfigurationModel>(token,Url("/api/v1/system/saveappconfiguration"),obj,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<CommonConfigurationModel> result)> GetCommonConfigurationAsync(string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<CommonConfigurationModel>,Object>(token,Url("/api/v1/system/getcommonconfiguration"),null,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> SaveCommonConfigurationAsync(CommonConfigurationModel obj, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,CommonConfigurationModel>(token,Url("/api/v1/system/savecommonconfiguration"),obj,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<CoordinatorConfigurationModel> result)> GetCoordinatorConfigurationAsync(string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<CoordinatorConfigurationModel>,Object>(token,Url("/api/v1/system/getcoordinatorconfiguration"),null,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> SaveCoordinatorConfigurationAsync(CoordinatorConfigurationModel obj, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,CoordinatorConfigurationModel>(token,Url("/api/v1/system/savecoordinatorconfiguration"),obj,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ReportServerConfigurationModel> result)> GetReportServerConfigurationAsync(string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ReportServerConfigurationModel>,Object>(token,Url("/api/v1/system/getreportserverconfiguration"),null,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> SaveReportServerConfigurationAsync(ReportServerConfigurationModel obj, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,ReportServerConfigurationModel>(token,Url("/api/v1/system/savereportserverconfiguration"),obj,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<ConfigurationGroupModel> result)> GetConfigurationGroups(ConfigurationGroupSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<ConfigurationGroupModel>,ConfigurationGroupSearchCriteria>(token,Url("/api/v1/system/getconfigurationgroups"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ConfigurationGroupModel> result)> GetConfigurationGroup(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ConfigurationGroupModel>,Object>(token,Url("/api/v1/system/getconfigurationgroup"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ConfigurationGroupModel> result)> CreateConfigurationGroup(ConfigurationGroupCreateModel configurationGroupModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ConfigurationGroupModel>,ConfigurationGroupCreateModel>(token,Url("/api/v1/system/createconfigurationgroup"),configurationGroupModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ConfigurationGroupModel> result)> UpdateConfigurationGroup(ConfigurationGroupUpdateModel configurationGroupModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ConfigurationGroupModel>,ConfigurationGroupUpdateModel>(token,Url("/api/v1/system/updateconfigurationgroup"),configurationGroupModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkConfigurationGroupAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markconfigurationgroupasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkConfigurationGroupAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markconfigurationgroupasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<ConfigurationItemModel> result)> GetConfigurationItems(ConfigurationItemSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<ConfigurationItemModel>,ConfigurationItemSearchCriteria>(token,Url("/api/v1/system/getconfigurationitems"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ConfigurationItemModel> result)> GetConfigurationItem(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ConfigurationItemModel>,Object>(token,Url("/api/v1/system/getconfigurationitem"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ConfigurationItemModel> result)> CreateConfigurationItem(ConfigurationItemCreateModel configurationItemModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ConfigurationItemModel>,ConfigurationItemCreateModel>(token,Url("/api/v1/system/createconfigurationitem"),configurationItemModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ConfigurationItemModel> result)> UpdateConfigurationItem(ConfigurationItemUpdateModel configurationItemModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ConfigurationItemModel>,ConfigurationItemUpdateModel>(token,Url("/api/v1/system/updateconfigurationitem"),configurationItemModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkConfigurationItemAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markconfigurationitemasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkConfigurationItemAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markconfigurationitemasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<EnumGroupModel> result)> GetEnumGroups(EnumGroupSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<EnumGroupModel>,EnumGroupSearchCriteria>(token,Url("/api/v1/system/getenumgroups"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<EnumGroupModel> result)> GetEnumGroup(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<EnumGroupModel>,Object>(token,Url("/api/v1/system/getenumgroup"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<EnumGroupModel> result)> CreateEnumGroup(EnumGroupCreateModel enumGroupModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<EnumGroupModel>,EnumGroupCreateModel>(token,Url("/api/v1/system/createenumgroup"),enumGroupModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<EnumGroupModel> result)> UpdateEnumGroup(EnumGroupUpdateModel enumGroupModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<EnumGroupModel>,EnumGroupUpdateModel>(token,Url("/api/v1/system/updateenumgroup"),enumGroupModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkEnumGroupAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markenumgroupasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkEnumGroupAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markenumgroupasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<EnumTypeModel> result)> GetEnumTypes(EnumTypeSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<EnumTypeModel>,EnumTypeSearchCriteria>(token,Url("/api/v1/system/getenumtypes"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<EnumTypeModel> result)> GetEnumType(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<EnumTypeModel>,Object>(token,Url("/api/v1/system/getenumtype"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<EnumTypeModel> result)> CreateEnumType(EnumTypeCreateModel enumTypeModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<EnumTypeModel>,EnumTypeCreateModel>(token,Url("/api/v1/system/createenumtype"),enumTypeModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<EnumTypeModel> result)> UpdateEnumType(EnumTypeUpdateModel enumTypeModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<EnumTypeModel>,EnumTypeUpdateModel>(token,Url("/api/v1/system/updateenumtype"),enumTypeModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkEnumTypeAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markenumtypeasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkEnumTypeAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markenumtypeasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<JobModel> result)> GetJobs(JobSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<JobModel>,JobSearchCriteria>(token,Url("/api/v1/system/getjobs"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobModel> result)> GetJob(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobModel>,Object>(token,Url("/api/v1/system/getjob"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobModel> result)> CreateJob(JobCreateModel jobModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobModel>,JobCreateModel>(token,Url("/api/v1/system/createjob"),jobModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobModel> result)> CreateJobFromScheduledJob(Int64 scheduleJobId, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobModel>,Object>(token,Url("/api/v1/system/createjobfromscheduledjob"),new { scheduleJobId },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobModel> result)> CreateJobFromJobTemplate(JobCreateModel jobTemplateJob, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobModel>,JobCreateModel>(token,Url("/api/v1/system/createjobfromjobtemplate"),jobTemplateJob,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobModel> result)> UpdateJob(JobUpdateModel jobModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobModel>,JobUpdateModel>(token,Url("/api/v1/system/updatejob"),jobModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkJobAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markjobasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkJobAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markjobasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<JobAttachmentModel> result)> GetJobAttachments(JobAttachmentSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<JobAttachmentModel>,JobAttachmentSearchCriteria>(token,Url("/api/v1/system/getjobattachments"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobAttachmentModel> result)> GetJobAttachment(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobAttachmentModel>,Object>(token,Url("/api/v1/system/getjobattachment"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobAttachmentModel> result)> CreateJobAttachment(JobAttachmentCreateModel jobAttachmentModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobAttachmentModel>,JobAttachmentCreateModel>(token,Url("/api/v1/system/createjobattachment"),jobAttachmentModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobAttachmentModel> result)> UpdateJobAttachment(JobAttachmentUpdateModel jobAttachmentModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobAttachmentModel>,JobAttachmentUpdateModel>(token,Url("/api/v1/system/updatejobattachment"),jobAttachmentModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkJobAttachmentAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markjobattachmentasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkJobAttachmentAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markjobattachmentasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<JobNotificationEmailModel> result)> GetJobNotificationEmails(JobNotificationEmailSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<JobNotificationEmailModel>,JobNotificationEmailSearchCriteria>(token,Url("/api/v1/system/getjobnotificationemails"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobNotificationEmailModel> result)> GetJobNotificationEmail(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobNotificationEmailModel>,Object>(token,Url("/api/v1/system/getjobnotificationemail"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobNotificationEmailModel> result)> CreateJobNotificationEmail(JobNotificationEmailCreateModel jobNotificationEmailModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobNotificationEmailModel>,JobNotificationEmailCreateModel>(token,Url("/api/v1/system/createjobnotificationemail"),jobNotificationEmailModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobNotificationEmailModel> result)> UpdateJobNotificationEmail(JobNotificationEmailUpdateModel jobNotificationEmailModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobNotificationEmailModel>,JobNotificationEmailUpdateModel>(token,Url("/api/v1/system/updatejobnotificationemail"),jobNotificationEmailModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkJobNotificationEmailAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markjobnotificationemailasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkJobNotificationEmailAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markjobnotificationemailasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<JobTemplateCategoryModel> result)> GetJobTemplateCategorys(JobTemplateCategorySearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<JobTemplateCategoryModel>,JobTemplateCategorySearchCriteria>(token,Url("/api/v1/system/getjobtemplatecategorys"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobTemplateCategoryModel> result)> GetJobTemplateCategory(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobTemplateCategoryModel>,Object>(token,Url("/api/v1/system/getjobtemplatecategory"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobTemplateCategoryModel> result)> CreateJobTemplateCategory(JobTemplateCategoryCreateModel jobTemplateCategoryModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobTemplateCategoryModel>,JobTemplateCategoryCreateModel>(token,Url("/api/v1/system/createjobtemplatecategory"),jobTemplateCategoryModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobTemplateCategoryModel> result)> UpdateJobTemplateCategory(JobTemplateCategoryUpdateModel jobTemplateCategoryModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobTemplateCategoryModel>,JobTemplateCategoryUpdateModel>(token,Url("/api/v1/system/updatejobtemplatecategory"),jobTemplateCategoryModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkJobTemplateCategoryAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markjobtemplatecategoryasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkJobTemplateCategoryAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markjobtemplatecategoryasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<JobTemplateModel> result)> GetJobTemplates(JobTemplateSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<JobTemplateModel>,JobTemplateSearchCriteria>(token,Url("/api/v1/system/getjobtemplates"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobTemplateModel> result)> GetJobTemplate(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobTemplateModel>,Object>(token,Url("/api/v1/system/getjobtemplate"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobTemplateModel> result)> CreateJobTemplate(JobTemplateCreateModel jobTemplateModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobTemplateModel>,JobTemplateCreateModel>(token,Url("/api/v1/system/createjobtemplate"),jobTemplateModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobTemplateModel> result)> UpdateJobTemplate(JobTemplateUpdateModel jobTemplateModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobTemplateModel>,JobTemplateUpdateModel>(token,Url("/api/v1/system/updatejobtemplate"),jobTemplateModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkJobTemplateAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markjobtemplateasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkJobTemplateAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markjobtemplateasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<JobTemplateParameterModel> result)> GetJobTemplateParameters(JobTemplateParameterSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<JobTemplateParameterModel>,JobTemplateParameterSearchCriteria>(token,Url("/api/v1/system/getjobtemplateparameters"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobTemplateParameterModel> result)> GetJobTemplateParameter(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobTemplateParameterModel>,Object>(token,Url("/api/v1/system/getjobtemplateparameter"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobTemplateParameterModel> result)> CreateJobTemplateParameter(JobTemplateParameterCreateModel jobTemplateParameterModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobTemplateParameterModel>,JobTemplateParameterCreateModel>(token,Url("/api/v1/system/createjobtemplateparameter"),jobTemplateParameterModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<JobTemplateParameterModel> result)> UpdateJobTemplateParameter(JobTemplateParameterUpdateModel jobTemplateParameterModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<JobTemplateParameterModel>,JobTemplateParameterUpdateModel>(token,Url("/api/v1/system/updatejobtemplateparameter"),jobTemplateParameterModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkJobTemplateParameterAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markjobtemplateparameterasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkJobTemplateParameterAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markjobtemplateparameterasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<MessageToSendModel> result)> GetMessageToSends(MessageToSendSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<MessageToSendModel>,MessageToSendSearchCriteria>(token,Url("/api/v1/system/getmessagetosends"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<MessageToSendModel> result)> GetMessageToSend(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<MessageToSendModel>,Object>(token,Url("/api/v1/system/getmessagetosend"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<MessageToSendModel> result)> CreateMessageToSend(MessageToSendCreateModel messageToSendModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<MessageToSendModel>,MessageToSendCreateModel>(token,Url("/api/v1/system/createmessagetosend"),messageToSendModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<RealtimeMessageModel> result)> GetRealtimeMessages(RealtimeMessageSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<RealtimeMessageModel>,RealtimeMessageSearchCriteria>(token,Url("/api/v1/system/getrealtimemessages"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<RealtimeMessageModel> result)> GetRealtimeMessage(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<RealtimeMessageModel>,Object>(token,Url("/api/v1/system/getrealtimemessage"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<RealtimeMessageModel> result)> CreateRealtimeMessage(RealtimeMessageCreateModel realtimeMessageModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<RealtimeMessageModel>,RealtimeMessageCreateModel>(token,Url("/api/v1/system/createrealtimemessage"),realtimeMessageModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<RealtimeMessageModel> result)> UpdateRealtimeMessage(RealtimeMessageUpdateModel realtimeMessageModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<RealtimeMessageModel>,RealtimeMessageUpdateModel>(token,Url("/api/v1/system/updaterealtimemessage"),realtimeMessageModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<SavedJobModel> result)> GetSavedJobs(SavedJobSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<SavedJobModel>,SavedJobSearchCriteria>(token,Url("/api/v1/system/getsavedjobs"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<SavedJobModel> result)> GetSavedJob(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<SavedJobModel>,Object>(token,Url("/api/v1/system/getsavedjob"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<SavedJobModel> result)> CreateSavedJob(SavedJobCreateModel savedJobModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<SavedJobModel>,SavedJobCreateModel>(token,Url("/api/v1/system/createsavedjob"),savedJobModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<SavedJobModel> result)> UpdateSavedJob(SavedJobUpdateModel savedJobModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<SavedJobModel>,SavedJobUpdateModel>(token,Url("/api/v1/system/updatesavedjob"),savedJobModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkSavedJobAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/marksavedjobasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkSavedJobAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/marksavedjobasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<ScheduledJobModel> result)> GetScheduledJobs(ScheduledJobSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<ScheduledJobModel>,ScheduledJobSearchCriteria>(token,Url("/api/v1/system/getscheduledjobs"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ScheduledJobModel> result)> GetScheduledJob(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ScheduledJobModel>,Object>(token,Url("/api/v1/system/getscheduledjob"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ScheduledJobModel> result)> CreateScheduledJob(ScheduledJobCreateModel scheduledJobModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ScheduledJobModel>,ScheduledJobCreateModel>(token,Url("/api/v1/system/createscheduledjob"),scheduledJobModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ScheduledJobModel> result)> UpdateScheduledJob(ScheduledJobUpdateModel scheduledJobModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ScheduledJobModel>,ScheduledJobUpdateModel>(token,Url("/api/v1/system/updatescheduledjob"),scheduledJobModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkScheduledJobAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markscheduledjobasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkScheduledJobAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markscheduledjobasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<AuditOperationModel> result)> GetAuditOperations(AuditOperationSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<AuditOperationModel>,AuditOperationSearchCriteria>(token,Url("/api/v1/system/getauditoperations"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<AuditOperationModel> result)> GetAuditOperation(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<AuditOperationModel>,Object>(token,Url("/api/v1/system/getauditoperation"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<AuditOperationModel> result)> CreateAuditOperation(AuditOperationCreateModel auditOperationModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<AuditOperationModel>,AuditOperationCreateModel>(token,Url("/api/v1/system/createauditoperation"),auditOperationModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<AuditOperationModel> result)> UpdateAuditOperation(AuditOperationUpdateModel auditOperationModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<AuditOperationModel>,AuditOperationUpdateModel>(token,Url("/api/v1/system/updateauditoperation"),auditOperationModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkAuditOperationAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markauditoperationasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkAuditOperationAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markauditoperationasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<AuditOperationDetailModel> result)> GetAuditOperationDetailByAuditOperationId(Int64 auditOperationId, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<AuditOperationDetailModel>,Object>(token,Url("/api/v1/system/getauditoperationdetailbyauditoperationid"),new { auditOperationId },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> IsAuditOperationDetailAvailableByAuditOperationId(Int64 auditOperationId, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Object>(token,Url("/api/v1/system/isauditoperationdetailavailablebyauditoperationid"),new { auditOperationId },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<AuditOperationDetailModel> result)> GetAuditOperationDetails(AuditOperationDetailSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<AuditOperationDetailModel>,AuditOperationDetailSearchCriteria>(token,Url("/api/v1/system/getauditoperationdetails"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<AuditOperationDetailModel> result)> GetAuditOperationDetail(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<AuditOperationDetailModel>,Object>(token,Url("/api/v1/system/getauditoperationdetail"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<AuditOperationDetailModel> result)> CreateAuditOperationDetail(AuditOperationDetailCreateModel auditOperationDetailModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<AuditOperationDetailModel>,AuditOperationDetailCreateModel>(token,Url("/api/v1/system/createauditoperationdetail"),auditOperationDetailModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<AuditOperationDetailModel> result)> UpdateAuditOperationDetail(AuditOperationDetailUpdateModel auditOperationDetailModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<AuditOperationDetailModel>,AuditOperationDetailUpdateModel>(token,Url("/api/v1/system/updateauditoperationdetail"),auditOperationDetailModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkAuditOperationDetailAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markauditoperationdetailasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkAuditOperationDetailAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markauditoperationdetailasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<SavedJobNotificationEmailModel> result)> GetSavedJobNotificationEmails(SavedJobNotificationEmailSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<SavedJobNotificationEmailModel>,SavedJobNotificationEmailSearchCriteria>(token,Url("/api/v1/system/getsavedjobnotificationemails"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<SavedJobNotificationEmailModel> result)> GetSavedJobNotificationEmail(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<SavedJobNotificationEmailModel>,Object>(token,Url("/api/v1/system/getsavedjobnotificationemail"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<SavedJobNotificationEmailModel> result)> CreateSavedJobNotificationEmail(SavedJobNotificationEmailCreateModel savedJobNotificationEmailModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<SavedJobNotificationEmailModel>,SavedJobNotificationEmailCreateModel>(token,Url("/api/v1/system/createsavedjobnotificationemail"),savedJobNotificationEmailModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<SavedJobNotificationEmailModel> result)> UpdateSavedJobNotificationEmail(SavedJobNotificationEmailUpdateModel savedJobNotificationEmailModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<SavedJobNotificationEmailModel>,SavedJobNotificationEmailUpdateModel>(token,Url("/api/v1/system/updatesavedjobnotificationemail"),savedJobNotificationEmailModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkSavedJobNotificationEmailAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/marksavedjobnotificationemailasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkSavedJobNotificationEmailAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/marksavedjobnotificationemailasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<ScheduledJobHistoryModel> result)> GetScheduledJobHistorys(ScheduledJobHistorySearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<ScheduledJobHistoryModel>,ScheduledJobHistorySearchCriteria>(token,Url("/api/v1/system/getscheduledjobhistorys"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ScheduledJobHistoryModel> result)> GetScheduledJobHistory(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ScheduledJobHistoryModel>,Object>(token,Url("/api/v1/system/getscheduledjobhistory"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ScheduledJobHistoryModel> result)> CreateScheduledJobHistory(ScheduledJobHistoryCreateModel scheduledJobHistoryModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ScheduledJobHistoryModel>,ScheduledJobHistoryCreateModel>(token,Url("/api/v1/system/createscheduledjobhistory"),scheduledJobHistoryModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ScheduledJobHistoryModel> result)> UpdateScheduledJobHistory(ScheduledJobHistoryUpdateModel scheduledJobHistoryModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ScheduledJobHistoryModel>,ScheduledJobHistoryUpdateModel>(token,Url("/api/v1/system/updatescheduledjobhistory"),scheduledJobHistoryModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkScheduledJobHistoryAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markscheduledjobhistoryasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkScheduledJobHistoryAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markscheduledjobhistoryasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<ScheduledJobNotificationEmailModel> result)> GetScheduledJobNotificationEmails(ScheduledJobNotificationEmailSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<ScheduledJobNotificationEmailModel>,ScheduledJobNotificationEmailSearchCriteria>(token,Url("/api/v1/system/getscheduledjobnotificationemails"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ScheduledJobNotificationEmailModel> result)> GetScheduledJobNotificationEmail(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ScheduledJobNotificationEmailModel>,Object>(token,Url("/api/v1/system/getscheduledjobnotificationemail"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ScheduledJobNotificationEmailModel> result)> CreateScheduledJobNotificationEmail(ScheduledJobNotificationEmailCreateModel scheduledJobNotificationEmailModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ScheduledJobNotificationEmailModel>,ScheduledJobNotificationEmailCreateModel>(token,Url("/api/v1/system/createscheduledjobnotificationemail"),scheduledJobNotificationEmailModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<ScheduledJobNotificationEmailModel> result)> UpdateScheduledJobNotificationEmail(ScheduledJobNotificationEmailUpdateModel scheduledJobNotificationEmailModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<ScheduledJobNotificationEmailModel>,ScheduledJobNotificationEmailUpdateModel>(token,Url("/api/v1/system/updatescheduledjobnotificationemail"),scheduledJobNotificationEmailModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkScheduledJobNotificationEmailAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markscheduledjobnotificationemailasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkScheduledJobNotificationEmailAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markscheduledjobnotificationemailasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<CompanyStorageConfigurationModel> result)> GetCompanyStorageConfiguration(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<CompanyStorageConfigurationModel>,Object>(token,Url("/api/v1/system/getcompanystorageconfiguration"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<CompanyStorageConfigurationModel> result)> GetCompanyStorageConfigurationByCompanyId(Int64 companyId, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<CompanyStorageConfigurationModel>,Object>(token,Url("/api/v1/system/getcompanystorageconfigurationbycompanyid"),new { companyId },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> DoesCompanyStorageConfigurationByCompanyIdExists(string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Object>(token,Url("/api/v1/system/doescompanystorageconfigurationbycompanyidexists"),null,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<CompanyStorageConfigurationModel> result)> CreateCompanyStorageConfiguration(CompanyStorageConfigurationCreateModel companyStorageConfigurationModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<CompanyStorageConfigurationModel>,CompanyStorageConfigurationCreateModel>(token,Url("/api/v1/system/createcompanystorageconfiguration"),companyStorageConfigurationModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<CompanyStorageConfigurationModel> result)> UpdateCompanyStorageConfiguration(CompanyStorageConfigurationUpdateModel companyStorageConfigurationModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<CompanyStorageConfigurationModel>,CompanyStorageConfigurationUpdateModel>(token,Url("/api/v1/system/updatecompanystorageconfiguration"),companyStorageConfigurationModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<MailServerModel> result)> GetMailServers(MailServerSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<MailServerModel>,MailServerSearchCriteria>(token,Url("/api/v1/system/getmailservers"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<MailServerModel> result)> GetMailServer(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<MailServerModel>,Object>(token,Url("/api/v1/system/getmailserver"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<MailServerModel> result)> CreateMailServer(MailServerCreateModel mailServerModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<MailServerModel>,MailServerCreateModel>(token,Url("/api/v1/system/createmailserver"),mailServerModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<MailServerModel> result)> UpdateMailServer(MailServerUpdateModel mailServerModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<MailServerModel>,MailServerUpdateModel>(token,Url("/api/v1/system/updatemailserver"),mailServerModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkMailServerAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markmailserverasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkMailServerAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markmailserverasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<SmsServerModel> result)> GetSmsServers(SmsServerSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<SmsServerModel>,SmsServerSearchCriteria>(token,Url("/api/v1/system/getsmsservers"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<SmsServerModel> result)> GetSmsServer(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<SmsServerModel>,Object>(token,Url("/api/v1/system/getsmsserver"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<SmsServerModel> result)> CreateSmsServer(SmsServerCreateModel smsServerModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<SmsServerModel>,SmsServerCreateModel>(token,Url("/api/v1/system/createsmsserver"),smsServerModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<SmsServerModel> result)> UpdateSmsServer(SmsServerUpdateModel smsServerModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<SmsServerModel>,SmsServerUpdateModel>(token,Url("/api/v1/system/updatesmsserver"),smsServerModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkSmsServerAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/marksmsserverasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkSmsServerAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/marksmsserverasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<SingleSignOnServerModel> result)> GetSingleSignOnServers(SingleSignOnServerSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<SingleSignOnServerModel>,SingleSignOnServerSearchCriteria>(token,Url("/api/v1/system/getsinglesignonservers"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<SingleSignOnServerModel> result)> GetSingleSignOnServer(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<SingleSignOnServerModel>,Object>(token,Url("/api/v1/system/getsinglesignonserver"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<SingleSignOnServerModel> result)> CreateSingleSignOnServer(SingleSignOnServerCreateModel singleSignOnServerModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<SingleSignOnServerModel>,SingleSignOnServerCreateModel>(token,Url("/api/v1/system/createsinglesignonserver"),singleSignOnServerModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<SingleSignOnServerModel> result)> UpdateSingleSignOnServer(SingleSignOnServerUpdateModel singleSignOnServerModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<SingleSignOnServerModel>,SingleSignOnServerUpdateModel>(token,Url("/api/v1/system/updatesinglesignonserver"),singleSignOnServerModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkSingleSignOnServerAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/marksinglesignonserverasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkSingleSignOnServerAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/marksinglesignonserverasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<PushNotificationServerModel> result)> GetPushNotificationServers(PushNotificationServerSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<PushNotificationServerModel>,PushNotificationServerSearchCriteria>(token,Url("/api/v1/system/getpushnotificationservers"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<PushNotificationServerModel> result)> GetPushNotificationServer(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<PushNotificationServerModel>,Object>(token,Url("/api/v1/system/getpushnotificationserver"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<PushNotificationServerModel> result)> CreatePushNotificationServer(PushNotificationServerCreateModel pushNotificationConfigurationModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<PushNotificationServerModel>,PushNotificationServerCreateModel>(token,Url("/api/v1/system/createpushnotificationserver"),pushNotificationConfigurationModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<PushNotificationServerModel> result)> CreateMultiplePushNotificationServer(MultiplePushNotificationServerCreateModel pushNotificationConfigurationModels, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<PushNotificationServerModel>,MultiplePushNotificationServerCreateModel>(token,Url("/api/v1/system/createmultiplepushnotificationserver"),pushNotificationConfigurationModels,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<PushNotificationServerModel> result)> UpdatePushNotificationServer(PushNotificationServerUpdateModel pushNotificationConfigurationModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<PushNotificationServerModel>,PushNotificationServerUpdateModel>(token,Url("/api/v1/system/updatepushnotificationserver"),pushNotificationConfigurationModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkPushNotificationServerAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markpushnotificationserverasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkPushNotificationServerAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markpushnotificationserverasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<VoiceNotificationServerModel> result)> GetVoiceNotificationServers(VoiceNotificationServerSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<VoiceNotificationServerModel>,VoiceNotificationServerSearchCriteria>(token,Url("/api/v1/system/getvoicenotificationservers"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<VoiceNotificationServerModel> result)> GetVoiceNotificationServer(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<VoiceNotificationServerModel>,Object>(token,Url("/api/v1/system/getvoicenotificationserver"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<VoiceNotificationServerModel> result)> CreateVoiceNotificationServer(VoiceNotificationServerCreateModel voiceNotificationServerModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<VoiceNotificationServerModel>,VoiceNotificationServerCreateModel>(token,Url("/api/v1/system/createvoicenotificationserver"),voiceNotificationServerModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<VoiceNotificationServerModel> result)> CreateMultipleVoiceNotificationServer(MultipleVoiceNotificationServerCreateModel voiceNotificationServerModels, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<VoiceNotificationServerModel>,MultipleVoiceNotificationServerCreateModel>(token,Url("/api/v1/system/createmultiplevoicenotificationserver"),voiceNotificationServerModels,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<VoiceNotificationServerModel> result)> UpdateVoiceNotificationServer(VoiceNotificationServerUpdateModel voiceNotificationServerModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<VoiceNotificationServerModel>,VoiceNotificationServerUpdateModel>(token,Url("/api/v1/system/updatevoicenotificationserver"),voiceNotificationServerModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkVoiceNotificationServerAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markvoicenotificationserverasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkVoiceNotificationServerAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markvoicenotificationserverasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<WhatsAppNotificationServerModel> result)> GetWhatsAppNotificationServers(WhatsAppNotificationServerSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<WhatsAppNotificationServerModel>,WhatsAppNotificationServerSearchCriteria>(token,Url("/api/v1/system/getwhatsappnotificationservers"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<WhatsAppNotificationServerModel> result)> GetWhatsAppNotificationServer(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<WhatsAppNotificationServerModel>,Object>(token,Url("/api/v1/system/getwhatsappnotificationserver"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<WhatsAppNotificationServerModel> result)> CreateWhatsAppNotificationServer(WhatsAppNotificationServerCreateModel whatsAppNotificationServerModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<WhatsAppNotificationServerModel>,WhatsAppNotificationServerCreateModel>(token,Url("/api/v1/system/createwhatsappnotificationserver"),whatsAppNotificationServerModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<WhatsAppNotificationServerModel> result)> CreateMultipleWhatsAppNotificationServer(MultipleWhatsAppNotificationServerCreateModel whatsAppNotificationServerModels, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<WhatsAppNotificationServerModel>,MultipleWhatsAppNotificationServerCreateModel>(token,Url("/api/v1/system/createmultiplewhatsappnotificationserver"),whatsAppNotificationServerModels,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<WhatsAppNotificationServerModel> result)> UpdateWhatsAppNotificationServer(WhatsAppNotificationServerUpdateModel whatsAppNotificationServerModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<WhatsAppNotificationServerModel>,WhatsAppNotificationServerUpdateModel>(token,Url("/api/v1/system/updatewhatsappnotificationserver"),whatsAppNotificationServerModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkWhatsAppNotificationServerAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markwhatsappnotificationserverasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkWhatsAppNotificationServerAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markwhatsappnotificationserverasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnListModel<MessageTemplateModel> result)> GetMessageTemplates(MessageTemplateSearchCriteria search, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnListModel<MessageTemplateModel>,MessageTemplateSearchCriteria>(token,Url("/api/v1/system/getmessagetemplates"),search,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<MessageTemplateModel> result)> GetMessageTemplate(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<MessageTemplateModel>,Object>(token,Url("/api/v1/system/getmessagetemplate"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<MessageTemplateModel> result)> CreateMessageTemplate(MessageTemplateCreateModel messageTemplateModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<MessageTemplateModel>,MessageTemplateCreateModel>(token,Url("/api/v1/system/createmessagetemplate"),messageTemplateModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<MessageTemplateModel> result)> UpdateMessageTemplate(MessageTemplateUpdateModel messageTemplateModel, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAndReadAsJsonWithNewTokenClientAsync<ReturnModel<MessageTemplateModel>,MessageTemplateUpdateModel>(token,Url("/api/v1/system/updatemessagetemplate"),messageTemplateModel,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkMessageTemplateAsDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.DeleteAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>>(token,Url("/api/v1/system/markmessagetemplateasdeleted"),new { id },appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<Boolean> result)> MarkMessageTemplateAsNotDeleted(Int64 id, string token = null, AppHeaderModel appHeader = null)
        {
           return env.PostAsStringAndReadAsJsonWithNewTokenClientAsync<ReturnModel<Boolean>,Int64>(token,Url("/api/v1/system/markmessagetemplateasnotdeleted"),id,appHeader);
        }

        public virtual Task<(HttpResponseMessage response, ReturnModel<SystemSummaryModel> result)> GetAppDetails(string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetAndReadAsJsonWithNewTokenClientAsync<ReturnModel<SystemSummaryModel>,Object>(token,Url("/api/v1/system/getappdetails"),null,appHeader);
        }

        public virtual Task<HttpResponseMessage> InitiateCall(TwilioInitiateCallModel model, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetWithNewTokenClientAsync<TwilioInitiateCallModel>(token,Url("/api/v1/twilio/initiatecall"),model,appHeader);
        }

        public virtual Task<HttpResponseMessage> HandleQuestion(string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetWithNewTokenClientAsync<Object>(token,Url("/api/v1/twilio/handlequestion"),null,appHeader);
        }

        public virtual Task<HttpResponseMessage> HandleResponse(String digits, string token = null, AppHeaderModel appHeader = null)
        {
           return env.GetWithNewTokenClientAsync<Object>(token,Url("/api/v1/twilio/handleresponse"),new { digits },appHeader);
        }

    }
}
