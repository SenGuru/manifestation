using System;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.ViewModels;
using Manifest.Repository.Common;
using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.Exceptions;
using Framework.Infrastructure.Interfaces.DbAccess;
using Framework.Infrastructure.Interfaces.Helpers;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;

namespace Manifest.Service.App
{
    public partial class AuditService : BaseAppService
    {
        private readonly AuditRepository auditRepository;
        private readonly string currentUserName;
        private readonly string currentUserAddress;
        private readonly ICurrentSessionService currentSessionService;
        private readonly ITransactionManager transactionManager;
        private readonly string browser, operatingSystemName, deviceName;

        public AuditService(ILog log, ObjectMapperService objectMapperService, ITransactionManager transactionManager, ICurrentUser currentUser, AuditRepository auditRepository, ICurrentSessionService currentSessionService)
            : base(log, objectMapperService)
        {
            this.transactionManager = transactionManager;
            this.auditRepository = auditRepository;
            this.currentSessionService = currentSessionService;
            currentUserName = currentUser.GetCurrentUserName();
            currentUserAddress = currentUser.GetCurrentUserAddress();
            var ua = currentUser.GetCurrentUserAgent();
            var clientInfo = (!ua.IsTrimmedStringNullOrEmpty()) ? UAParserUtils.GetWebClientInfoFromUserAgent(ua) : null;
            browser = clientInfo == null ? null : clientInfo.Browser;
            operatingSystemName = clientInfo == null ? null : clientInfo.OS;
            deviceName = clientInfo == null ? null : clientInfo.DeviceType;
        }

        #region Account
        public Task LoginAsync(long userId, string userName)
        {
            return auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.Login, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: userId, userName: userName, ipAddress: currentUserAddress, createdBy: null, modifiedBy: null, description: $"User logged in successfully.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public Task LoginFailedAsync(string userName)
        {
            return auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.LoginFailed, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userName: userName, ipAddress: currentUserAddress, createdBy: null, modifiedBy: null, description: "User failed to log in.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task LogOutAsync()
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.Logout, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, description: "User logged off successfully.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ChangePasswordAsync()
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ChangedPassword, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, description: "Password has been changed successfully.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ForgotPasswordAsync()
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ForgotPasswordRequest, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, description: "Forgot Password request has been provided successfully.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        #endregion

        #region Others
        public async Task SuperAdminLoginAsync()
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.SuperAdminLogin, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: await currentSessionService.GetCurrentUserNameAsStringAsync(), ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"User logged in successfully.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task LogoutAsync()
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.Logout, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: await currentSessionService.GetCurrentUserNameAsStringAsync(), ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"User logged out successfully.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ForgotPasswordFailedRequestAsync(string userName)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ForgotPasswordFailedRequest, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: await currentSessionService.GetCurrentUserNameAsStringAsync(), ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"Forget password for username : {userName}.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ResetPasswordAsync(string userName)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResetPassword, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: await currentSessionService.GetCurrentUserNameAsStringAsync(), ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"Password reset request from {userName}.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task SocialLoginAsync(long userId, string userName)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.SocialLogin, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: userId, userName: userName, ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"User logged in successfully.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task SocialLoginAsync()
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.SocialLogin, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: await currentSessionService.GetCurrentUserNameAsStringAsync(), ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"User logged in successfully.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task SocialLoginFailedAsync(string userName)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.SocialLoginFailed, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: userName, ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"User : {userName}.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task AlternateLoginAsync()
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.AlternateLogin, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: await currentSessionService.GetCurrentUserNameAsStringAsync(), ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"User logged in successfully.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task AlternateLoginFailed(string userName)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.AlternateLogin, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: userName, ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"User Name : {userName}.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ChangedPasswordAsync()
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ChangedPassword, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: await currentSessionService.GetCurrentUserNameAsStringAsync(), ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"Changed password..", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task UserCreatedAsync(string userName)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.UserCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: await currentSessionService.GetCurrentUserNameAsStringAsync(), ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"user : {userName} created.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task UserUpdatedAsync(string userName)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.UserUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: await currentSessionService.GetCurrentUserNameAsStringAsync(), ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"user : {userName} updated.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task UserDeletedAsync(string userName)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.UserDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: await currentSessionService.GetCurrentUserNameAsStringAsync(), ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"user : {userName}.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task LoginEnabledAsync(string userName)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.LoginEnabled, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: await currentSessionService.GetCurrentUserNameAsStringAsync(), ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"user : {userName}.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task LoginDisabledAsync(string userName)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.LoginDisabled, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: await currentSessionService.GetCurrentUserNameAsStringAsync(), ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"user : {userName}.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task UpdateConfigurationAsync()
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.UpdateConfiguration, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: await currentSessionService.GetCurrentUserNameAsStringAsync(), ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"Updated the System Configuration.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ViewReportAsync(string reportName)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ViewReport, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: await currentSessionService.GetCurrentUserNameAsStringAsync(), ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"Report - {reportName} viewed.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task DownloadReportAsync(string reportName)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.DownloadReport, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: null, resourceName: "Account", userId: await currentSessionService.GetCurrentUserIdAsync(), userName: await currentSessionService.GetCurrentUserNameAsStringAsync(), ipAddress: currentUserAddress, createdBy: await currentSessionService.GetCurrentUserIdAsync(), modifiedBy: await currentSessionService.GetCurrentUserIdAsync(), description: $"Report - {reportName} downloaded.", browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region AuditOperation
        public async Task<ReturnListModel<AuditOperationModel>> GetAuditOperationsAsync(AuditOperationSearchCriteria search)
        {
            try
            {
                return this.ConvertToList(await auditRepository.GetAuditOperationsAsync(search), objectMapperService.AuditOperationViewListToAuditOperationModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<AuditOperationModel>($"Unexpected error when getting AuditOperations", ex);
            }
        }

        public async Task<ReturnModel<AuditOperationModel>> GetAuditOperationByIdAsync(long auditOperationId)
        {
            try
            {
                var item = await auditRepository.GetAuditOperationViewByIdAsync(auditOperationId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<AuditOperationModel>($"Unable to find the auditOperation for {auditOperationId}");
                }

                return this.ConvertToItem(item, objectMapperService.AuditOperationViewToAuditOperationModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<AuditOperationModel>($"Unexpected error when getting auditOperation for - {auditOperationId}", ex);
            }
        }

        public async Task<ReturnModel<AuditOperationModel>> CreateAuditOperationAsync(AuditOperationCreateModel auditOperation)
        {
            try
            {
                if (auditOperation == null)
                {
                    return log.ErrorAndReturnModel<AuditOperationModel>($"Unable to create AuditOperation", new ReturnErrorItem("", "AuditOperation information is not valid"));
                }

                //auditOperation.IsDeleted = false;

                AuditOperation item;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await auditRepository.CreateAuditOperationAsync(auditOperation, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create auditOperation");
                    }

                    await trx.CompleteAsync();
                }

                var view = await auditRepository.GetAuditOperationViewByIdAsync(item.AuditOperationId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<AuditOperationModel>($"Unable to find the auditOperation for {item.AuditOperationId}");
                }

                return this.ConvertToItem(view, objectMapperService.AuditOperationViewToAuditOperationModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<AuditOperationModel>($"Unexpected error when creating AuditOperation {auditOperation?.AuditOperationId}", ex);
            }
        }

        #endregion AuditOperation

        #region State
        public async Task StateCreatedAsync(long stateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.State, resourceName: "State", resourceId: stateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task StateUpdatedAsync(long stateId, State objectBeforeUpdate, State objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.State, resourceName: "State", resourceId: stateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task StateDeletedAsync(long stateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.State, resourceName: "State", resourceId: stateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task StateUnDeletedAsync(long stateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.State, resourceName: "State", resourceId: stateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task StateEnabledAsync(long stateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: ResourceTypes.State, resourceName: "State", resourceId: stateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, tenantName: await currentSessionService.GetCurrentTenantNameAsync(), browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task StateDisabledAsync(long stateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: ResourceTypes.State, resourceName: "State", resourceId: stateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, tenantName: await currentSessionService.GetCurrentTenantNameAsync(), browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteStateAsync(long stateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.State, resourceName: "State", resourceId: stateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region UserPermission
        public async Task UserPermissionCreatedAsync(long userPermissionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.UserPermission, resourceName: "UserPermission", resourceId: userPermissionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task UserPermissionUpdatedAsync(long userPermissionId, UserPermission objectBeforeUpdate, UserPermission objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.UserPermission, resourceName: "UserPermission", resourceId: userPermissionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task UserPermissionDeletedAsync(long userPermissionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.UserPermission, resourceName: "UserPermission", resourceId: userPermissionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task UserPermissionUnDeletedAsync(long userPermissionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.UserPermission, resourceName: "UserPermission", resourceId: userPermissionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task UserPermissionEnabledAsync(long userPermissionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.UserPermission, resourceName: "UserPermission", resourceId: userPermissionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task UserPermissionDisabledAsync(long userPermissionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.UserPermission, resourceName: "UserPermission", resourceId: userPermissionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteUserPermissionAsync(long userPermissionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.UserPermission, resourceName: "UserPermission", resourceId: userPermissionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        //System
        #region AuditOperation
        public async Task AuditOperationCreatedAsync(long auditOperationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.AuditOperation, resourceName: "AuditOperation", resourceId: auditOperationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task AuditOperationUpdatedAsync(long auditOperationId, AuditOperation objectBeforeUpdate, AuditOperation objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.AuditOperation, resourceName: "AuditOperation", resourceId: auditOperationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task AuditOperationDeletedAsync(long auditOperationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.AuditOperation, resourceName: "AuditOperation", resourceId: auditOperationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task AuditOperationUnDeletedAsync(long auditOperationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.AuditOperation, resourceName: "AuditOperation", resourceId: auditOperationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task AuditOperationEnabledAsync(long auditOperationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.AuditOperation, resourceName: "AuditOperation", resourceId: auditOperationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task AuditOperationDisabledAsync(long auditOperationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.AuditOperation, resourceName: "AuditOperation", resourceId: auditOperationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteAuditOperationAsync(long auditOperationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.AuditOperation, resourceName: "AuditOperation", resourceId: auditOperationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region ConfigurationGroup
        public async Task ConfigurationGroupCreatedAsync(long configurationGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ConfigurationGroup, resourceName: "ConfigurationGroup", resourceId: configurationGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ConfigurationGroupUpdatedAsync(long configurationGroupId, ConfigurationGroup objectBeforeUpdate, ConfigurationGroup objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.ConfigurationGroup, resourceName: "ConfigurationGroup", resourceId: configurationGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ConfigurationGroupDeletedAsync(long configurationGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ConfigurationGroup, resourceName: "ConfigurationGroup", resourceId: configurationGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ConfigurationGroupUnDeletedAsync(long configurationGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ConfigurationGroup, resourceName: "ConfigurationGroup", resourceId: configurationGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task ConfigurationGroupEnabledAsync(long configurationGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ConfigurationGroup, resourceName: "ConfigurationGroup", resourceId: configurationGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ConfigurationGroupDisabledAsync(long configurationGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ConfigurationGroup, resourceName: "ConfigurationGroup", resourceId: configurationGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteConfigurationGroupAsync(long configurationGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ConfigurationGroup, resourceName: "ConfigurationGroup", resourceId: configurationGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region ConfigurationItem
        public async Task ConfigurationItemCreatedAsync(long configurationItemId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ConfigurationItem, resourceName: "ConfigurationItem", resourceId: configurationItemId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ConfigurationItemUpdatedAsync(long configurationItemId, ConfigurationItem objectBeforeUpdate, ConfigurationItem objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.ConfigurationItem, resourceName: "ConfigurationItem", resourceId: configurationItemId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ConfigurationItemDeletedAsync(long configurationItemId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ConfigurationItem, resourceName: "ConfigurationItem", resourceId: configurationItemId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ConfigurationItemUnDeletedAsync(long configurationItemId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ConfigurationItem, resourceName: "ConfigurationItem", resourceId: configurationItemId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task ConfigurationItemEnabledAsync(long configurationItemId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ConfigurationItem, resourceName: "ConfigurationItem", resourceId: configurationItemId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ConfigurationItemDisabledAsync(long configurationItemId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ConfigurationItem, resourceName: "ConfigurationItem", resourceId: configurationItemId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteConfigurationItemAsync(long configurationItemId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ConfigurationItem, resourceName: "ConfigurationItem", resourceId: configurationItemId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region EnumGroup
        public async Task EnumGroupCreatedAsync(long enumGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EnumGroup, resourceName: "EnumGroup", resourceId: enumGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task EnumGroupUpdatedAsync(long enumGroupId, EnumGroup objectBeforeUpdate, EnumGroup objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.EnumGroup, resourceName: "EnumGroup", resourceId: enumGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task EnumGroupDeletedAsync(long enumGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EnumGroup, resourceName: "EnumGroup", resourceId: enumGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task EnumGroupUnDeletedAsync(long enumGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EnumGroup, resourceName: "EnumGroup", resourceId: enumGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task EnumGroupEnabledAsync(long enumGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EnumGroup, resourceName: "EnumGroup", resourceId: enumGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task EnumGroupDisabledAsync(long enumGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EnumGroup, resourceName: "EnumGroup", resourceId: enumGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteEnumGroupAsync(long enumGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EnumGroup, resourceName: "EnumGroup", resourceId: enumGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region EnumType
        public async Task EnumTypeCreatedAsync(long enumTypeId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EnumType, resourceName: "EnumType", resourceId: enumTypeId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task EnumTypeUpdatedAsync(long enumTypeId, EnumType objectBeforeUpdate, EnumType objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.EnumType, resourceName: "EnumType", resourceId: enumTypeId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task EnumTypeDeletedAsync(long enumTypeId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EnumType, resourceName: "EnumType", resourceId: enumTypeId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task EnumTypeUnDeletedAsync(long enumTypeId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EnumType, resourceName: "EnumType", resourceId: enumTypeId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task EnumTypeEnabledAsync(long enumTypeId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EnumType, resourceName: "EnumType", resourceId: enumTypeId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task EnumTypeDisabledAsync(long enumTypeId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EnumType, resourceName: "EnumType", resourceId: enumTypeId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteEnumTypeAsync(long enumTypeId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EnumType, resourceName: "EnumType", resourceId: enumTypeId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region Job
        public async Task JobCreatedAsync(long jobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Job, resourceName: "Job", resourceId: jobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobUpdatedAsync(long jobId, Job objectBeforeUpdate, Job objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.Job, resourceName: "Job", resourceId: jobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobDeletedAsync(long jobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Job, resourceName: "Job", resourceId: jobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobUnDeletedAsync(long jobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Job, resourceName: "Job", resourceId: jobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task JobEnabledAsync(long jobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Job, resourceName: "Job", resourceId: jobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobDisabledAsync(long jobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Job, resourceName: "Job", resourceId: jobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteJobAsync(long jobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Job, resourceName: "Job", resourceId: jobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region JobAttachment
        public async Task JobAttachmentCreatedAsync(long jobAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobAttachment, resourceName: "JobAttachment", resourceId: jobAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobAttachmentUpdatedAsync(long jobAttachmentId, JobAttachment objectBeforeUpdate, JobAttachment objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.JobAttachment, resourceName: "JobAttachment", resourceId: jobAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobAttachmentDeletedAsync(long jobAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobAttachment, resourceName: "JobAttachment", resourceId: jobAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobAttachmentUnDeletedAsync(long jobAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobAttachment, resourceName: "JobAttachment", resourceId: jobAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task JobAttachmentEnabledAsync(long jobAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobAttachment, resourceName: "JobAttachment", resourceId: jobAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobAttachmentDisabledAsync(long jobAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobAttachment, resourceName: "JobAttachment", resourceId: jobAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteJobAttachmentAsync(long jobAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobAttachment, resourceName: "JobAttachment", resourceId: jobAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region JobNotificationEmail
        public async Task JobNotificationEmailCreatedAsync(long jobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobNotificationEmail, resourceName: "JobNotificationEmail", resourceId: jobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobNotificationEmailUpdatedAsync(long jobNotificationEmailId, JobNotificationEmail objectBeforeUpdate, JobNotificationEmail objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.JobNotificationEmail, resourceName: "JobNotificationEmail", resourceId: jobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobNotificationEmailDeletedAsync(long jobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobNotificationEmail, resourceName: "JobNotificationEmail", resourceId: jobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobNotificationEmailUnDeletedAsync(long jobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobNotificationEmail, resourceName: "JobNotificationEmail", resourceId: jobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task JobNotificationEmailEnabledAsync(long jobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobNotificationEmail, resourceName: "JobNotificationEmail", resourceId: jobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobNotificationEmailDisabledAsync(long jobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobNotificationEmail, resourceName: "JobNotificationEmail", resourceId: jobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteJobNotificationEmailAsync(long jobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobNotificationEmail, resourceName: "JobNotificationEmail", resourceId: jobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region JobTemplate
        public async Task JobTemplateCreatedAsync(long jobTemplateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplate, resourceName: "JobTemplate", resourceId: jobTemplateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobTemplateUpdatedAsync(long jobTemplateId, JobTemplate objectBeforeUpdate, JobTemplate objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.JobTemplate, resourceName: "JobTemplate", resourceId: jobTemplateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobTemplateDeletedAsync(long jobTemplateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplate, resourceName: "JobTemplate", resourceId: jobTemplateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobTemplateUnDeletedAsync(long jobTemplateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplate, resourceName: "JobTemplate", resourceId: jobTemplateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task JobTemplateEnabledAsync(long jobTemplateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplate, resourceName: "JobTemplate", resourceId: jobTemplateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobTemplateDisabledAsync(long jobTemplateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplate, resourceName: "JobTemplate", resourceId: jobTemplateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteJobTemplateAsync(long jobTemplateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplate, resourceName: "JobTemplate", resourceId: jobTemplateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region JobTemplateCategory
        public async Task JobTemplateCategoryCreatedAsync(long jobTemplateCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplateCategory, resourceName: "JobTemplateCategory", resourceId: jobTemplateCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobTemplateCategoryUpdatedAsync(long jobTemplateCategoryId, JobTemplateCategory objectBeforeUpdate, JobTemplateCategory objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.JobTemplateCategory, resourceName: "JobTemplateCategory", resourceId: jobTemplateCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobTemplateCategoryDeletedAsync(long jobTemplateCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplateCategory, resourceName: "JobTemplateCategory", resourceId: jobTemplateCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobTemplateCategoryUnDeletedAsync(long jobTemplateCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplateCategory, resourceName: "JobTemplateCategory", resourceId: jobTemplateCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task JobTemplateCategoryEnabledAsync(long jobTemplateCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplateCategory, resourceName: "JobTemplateCategory", resourceId: jobTemplateCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobTemplateCategoryDisabledAsync(long jobTemplateCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplateCategory, resourceName: "JobTemplateCategory", resourceId: jobTemplateCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteJobTemplateCategoryAsync(long jobTemplateCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplateCategory, resourceName: "JobTemplateCategory", resourceId: jobTemplateCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region JobTemplateParameter
        public async Task JobTemplateParameterCreatedAsync(long jobTemplateParameterId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplateParameter, resourceName: "JobTemplateParameter", resourceId: jobTemplateParameterId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobTemplateParameterUpdatedAsync(long jobTemplateParameterId, JobTemplateParameter objectBeforeUpdate, JobTemplateParameter objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.JobTemplateParameter, resourceName: "JobTemplateParameter", resourceId: jobTemplateParameterId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobTemplateParameterDeletedAsync(long jobTemplateParameterId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplateParameter, resourceName: "JobTemplateParameter", resourceId: jobTemplateParameterId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobTemplateParameterUnDeletedAsync(long jobTemplateParameterId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplateParameter, resourceName: "JobTemplateParameter", resourceId: jobTemplateParameterId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task JobTemplateParameterEnabledAsync(long jobTemplateParameterId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplateParameter, resourceName: "JobTemplateParameter", resourceId: jobTemplateParameterId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task JobTemplateParameterDisabledAsync(long jobTemplateParameterId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplateParameter, resourceName: "JobTemplateParameter", resourceId: jobTemplateParameterId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteJobTemplateParameterAsync(long jobTemplateParameterId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.JobTemplateParameter, resourceName: "JobTemplateParameter", resourceId: jobTemplateParameterId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region MessageTemplate
        public async Task MessageTemplateCreatedAsync(long messageTemplateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MessageTemplate, resourceName: "MessageTemplate", resourceId: messageTemplateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task MessageTemplateUpdatedAsync(long messageTemplateId, MessageTemplate objectBeforeUpdate, MessageTemplate objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.MessageTemplate, resourceName: "MessageTemplate", resourceId: messageTemplateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task MessageTemplateDeletedAsync(long messageTemplateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MessageTemplate, resourceName: "MessageTemplate", resourceId: messageTemplateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task MessageTemplateUnDeletedAsync(long messageTemplateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MessageTemplate, resourceName: "MessageTemplate", resourceId: messageTemplateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task MessageTemplateEnabledAsync(long messageTemplateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.MessageTemplate, resourceName: "MessageTemplate", resourceId: messageTemplateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task MessageTemplateDisabledAsync(long messageTemplateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.MessageTemplate, resourceName: "MessageTemplate", resourceId: messageTemplateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteMessageTemplateAsync(long messageTemplateId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MessageTemplate, resourceName: "MessageTemplate", resourceId: messageTemplateId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

        #region MessageToSend
        public async Task MessageToSendCreatedAsync(long messageToSendId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MessageToSend, resourceName: "MessageToSend", resourceId: messageToSendId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task MessageToSendUpdatedAsync(long messageToSendId, MessageToSend objectBeforeUpdate, MessageToSend objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.MessageToSend, resourceName: "MessageToSend", resourceId: messageToSendId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task MessageToSendDeletedAsync(long messageToSendId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MessageToSend, resourceName: "MessageToSend", resourceId: messageToSendId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task MessageToSendUnDeletedAsync(long messageToSendId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MessageToSend, resourceName: "MessageToSend", resourceId: messageToSendId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task MessageToSendEnabledAsync(long messageToSendId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MessageToSend, resourceName: "MessageToSend", resourceId: messageToSendId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task MessageToSendDisabledAsync(long messageToSendId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MessageToSend, resourceName: "MessageToSend", resourceId: messageToSendId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteMessageToSendAsync(long messageToSendId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MessageToSend, resourceName: "MessageToSend", resourceId: messageToSendId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region RealtimeMessage
        public async Task RealtimeMessageCreatedAsync(long realtimeMessageId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.RealtimeMessage, resourceName: "RealtimeMessage", resourceId: realtimeMessageId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task RealtimeMessageUpdatedAsync(long realtimeMessageId, RealtimeMessage objectBeforeUpdate, RealtimeMessage objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.RealtimeMessage, resourceName: "RealtimeMessage", resourceId: realtimeMessageId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task RealtimeMessageDeletedAsync(long realtimeMessageId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.RealtimeMessage, resourceName: "RealtimeMessage", resourceId: realtimeMessageId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task RealtimeMessageUnDeletedAsync(long realtimeMessageId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.RealtimeMessage, resourceName: "RealtimeMessage", resourceId: realtimeMessageId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task RealtimeMessageEnabledAsync(long realtimeMessageId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.RealtimeMessage, resourceName: "RealtimeMessage", resourceId: realtimeMessageId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task RealtimeMessageDisabledAsync(long realtimeMessageId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.RealtimeMessage, resourceName: "RealtimeMessage", resourceId: realtimeMessageId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteRealtimeMessageAsync(long realtimeMessageId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.RealtimeMessage, resourceName: "RealtimeMessage", resourceId: realtimeMessageId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region SavedJob
        public async Task SavedJobCreatedAsync(long savedJobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SavedJob, resourceName: "SavedJob", resourceId: savedJobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task SavedJobUpdatedAsync(long savedJobId, SavedJob objectBeforeUpdate, SavedJob objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.SavedJob, resourceName: "SavedJob", resourceId: savedJobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task SavedJobDeletedAsync(long savedJobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SavedJob, resourceName: "SavedJob", resourceId: savedJobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task SavedJobUnDeletedAsync(long savedJobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SavedJob, resourceName: "SavedJob", resourceId: savedJobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task SavedJobEnabledAsync(long savedJobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SavedJob, resourceName: "SavedJob", resourceId: savedJobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task SavedJobDisabledAsync(long savedJobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SavedJob, resourceName: "SavedJob", resourceId: savedJobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteSavedJobAsync(long savedJobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SavedJob, resourceName: "SavedJob", resourceId: savedJobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region ScheduledJob
        public async Task ScheduledJobCreatedAsync(long scheduledJobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJob, resourceName: "ScheduledJob", resourceId: scheduledJobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ScheduledJobUpdatedAsync(long scheduledJobId, ScheduledJob objectBeforeUpdate, ScheduledJob objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.ScheduledJob, resourceName: "ScheduledJob", resourceId: scheduledJobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ScheduledJobDeletedAsync(long scheduledJobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJob, resourceName: "ScheduledJob", resourceId: scheduledJobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ScheduledJobUnDeletedAsync(long scheduledJobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJob, resourceName: "ScheduledJob", resourceId: scheduledJobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task ScheduledJobEnabledAsync(long scheduledJobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJob, resourceName: "ScheduledJob", resourceId: scheduledJobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ScheduledJobDisabledAsync(long scheduledJobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJob, resourceName: "ScheduledJob", resourceId: scheduledJobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteScheduledJobAsync(long scheduledJobId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJob, resourceName: "ScheduledJob", resourceId: scheduledJobId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region SavedJobNotificationEmail
        public async Task SavedJobNotificationEmailCreatedAsync(long savedJobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SavedJobNotificationEmail, resourceName: "SavedJobNotificationEmail", resourceId: savedJobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task SavedJobNotificationEmailUpdatedAsync(long savedJobNotificationEmailId, SavedJobNotificationEmail objectBeforeUpdate, SavedJobNotificationEmail objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.SavedJobNotificationEmail, resourceName: "SavedJobNotificationEmail", resourceId: savedJobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task SavedJobNotificationEmailDeletedAsync(long savedJobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SavedJobNotificationEmail, resourceName: "SavedJobNotificationEmail", resourceId: savedJobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task SavedJobNotificationEmailUnDeletedAsync(long savedJobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SavedJobNotificationEmail, resourceName: "SavedJobNotificationEmail", resourceId: savedJobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task SavedJobNotificationEmailEnabledAsync(long savedJobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SavedJobNotificationEmail, resourceName: "SavedJobNotificationEmail", resourceId: savedJobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task SavedJobNotificationEmailDisabledAsync(long savedJobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SavedJobNotificationEmail, resourceName: "SavedJobNotificationEmail", resourceId: savedJobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteSavedJobNotificationEmailAsync(long savedJobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SavedJobNotificationEmail, resourceName: "SavedJobNotificationEmail", resourceId: savedJobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region ScheduledJobHistory
        public async Task ScheduledJobHistoryCreatedAsync(long scheduledJobHistoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJobHistory, resourceName: "ScheduledJobHistory", resourceId: scheduledJobHistoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ScheduledJobHistoryUpdatedAsync(long scheduledJobHistoryId, ScheduledJobHistory objectBeforeUpdate, ScheduledJobHistory objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.ScheduledJobHistory, resourceName: "ScheduledJobHistory", resourceId: scheduledJobHistoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ScheduledJobHistoryDeletedAsync(long scheduledJobHistoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJobHistory, resourceName: "ScheduledJobHistory", resourceId: scheduledJobHistoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ScheduledJobHistoryUnDeletedAsync(long scheduledJobHistoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJobHistory, resourceName: "ScheduledJobHistory", resourceId: scheduledJobHistoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task ScheduledJobHistoryEnabledAsync(long scheduledJobHistoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJobHistory, resourceName: "ScheduledJobHistory", resourceId: scheduledJobHistoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ScheduledJobHistoryDisabledAsync(long scheduledJobHistoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJobHistory, resourceName: "ScheduledJobHistory", resourceId: scheduledJobHistoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteScheduledJobHistoryAsync(long scheduledJobHistoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJobHistory, resourceName: "ScheduledJobHistory", resourceId: scheduledJobHistoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region ScheduledJobNotificationEmail
        public async Task ScheduledJobNotificationEmailCreatedAsync(long scheduledJobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJobNotificationEmail, resourceName: "ScheduledJobNotificationEmail", resourceId: scheduledJobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ScheduledJobNotificationEmailUpdatedAsync(long scheduledJobNotificationEmailId, ScheduledJobNotificationEmail objectBeforeUpdate, ScheduledJobNotificationEmail objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.ScheduledJobNotificationEmail, resourceName: "ScheduledJobNotificationEmail", resourceId: scheduledJobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ScheduledJobNotificationEmailDeletedAsync(long scheduledJobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJobNotificationEmail, resourceName: "ScheduledJobNotificationEmail", resourceId: scheduledJobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ScheduledJobNotificationEmailUnDeletedAsync(long scheduledJobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJobNotificationEmail, resourceName: "ScheduledJobNotificationEmail", resourceId: scheduledJobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task ScheduledJobNotificationEmailEnabledAsync(long scheduledJobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJobNotificationEmail, resourceName: "ScheduledJobNotificationEmail", resourceId: scheduledJobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task ScheduledJobNotificationEmailDisabledAsync(long scheduledJobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJobNotificationEmail, resourceName: "ScheduledJobNotificationEmail", resourceId: scheduledJobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteScheduledJobNotificationEmailAsync(long scheduledJobNotificationEmailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ScheduledJobNotificationEmail, resourceName: "ScheduledJobNotificationEmail", resourceId: scheduledJobNotificationEmailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region CompanyStorageConfiguration
        public async Task CompanyStorageConfigurationCreatedAsync(long companyStorageConfigurationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.CompanyStorageConfiguration, resourceName: "CompanyStorageConfiguration", resourceId: companyStorageConfigurationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task CompanyStorageConfigurationUpdatedAsync(long companyStorageConfigurationId, CompanyStorageConfiguration objectBeforeUpdate, CompanyStorageConfiguration objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.CompanyStorageConfiguration, resourceName: "CompanyStorageConfiguration", resourceId: companyStorageConfigurationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task CompanyStorageConfigurationDeletedAsync(long companyStorageConfigurationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.CompanyStorageConfiguration, resourceName: "CompanyStorageConfiguration", resourceId: companyStorageConfigurationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task CompanyStorageConfigurationUnDeletedAsync(long companyStorageConfigurationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.CompanyStorageConfiguration, resourceName: "CompanyStorageConfiguration", resourceId: companyStorageConfigurationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task CompanyStorageConfigurationEnabledAsync(long companyStorageConfigurationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.CompanyStorageConfiguration, resourceName: "CompanyStorageConfiguration", resourceId: companyStorageConfigurationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task CompanyStorageConfigurationDisabledAsync(long companyStorageConfigurationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.CompanyStorageConfiguration, resourceName: "CompanyStorageConfiguration", resourceId: companyStorageConfigurationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteCompanyStorageConfigurationAsync(long companyStorageConfigurationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.CompanyStorageConfiguration, resourceName: "CompanyStorageConfiguration", resourceId: companyStorageConfigurationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region MailServer
        public async Task MailServerCreatedAsync(long mailServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MailServer, resourceName: "MailServer", resourceId: mailServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task MailServerUpdatedAsync(long mailServerId, MailServer objectBeforeUpdate, MailServer objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.MailServer, resourceName: "MailServer", resourceId: mailServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task MailServerDeletedAsync(long mailServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MailServer, resourceName: "MailServer", resourceId: mailServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task MailServerUnDeletedAsync(long mailServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MailServer, resourceName: "MailServer", resourceId: mailServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task MailServerEnabledAsync(long mailServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MailServer, resourceName: "MailServer", resourceId: mailServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task MailServerDisabledAsync(long mailServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MailServer, resourceName: "MailServer", resourceId: mailServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteMailServerAsync(long mailServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MailServer, resourceName: "MailServer", resourceId: mailServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region SmsServer
        public async Task SmsServerCreatedAsync(long smsServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SmsServer, resourceName: "SmsServer", resourceId: smsServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task SmsServerUpdatedAsync(long smsServerId, SmsServer objectBeforeUpdate, SmsServer objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.SmsServer, resourceName: "SmsServer", resourceId: smsServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task SmsServerDeletedAsync(long smsServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SmsServer, resourceName: "SmsServer", resourceId: smsServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task SmsServerUnDeletedAsync(long smsServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SmsServer, resourceName: "SmsServer", resourceId: smsServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task SmsServerEnabledAsync(long smsServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SmsServer, resourceName: "SmsServer", resourceId: smsServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task SmsServerDisabledAsync(long smsServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SmsServer, resourceName: "SmsServer", resourceId: smsServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteSmsServerAsync(long smsServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SmsServer, resourceName: "SmsServer", resourceId: smsServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region NoteAttachment
        public async Task NoteAttachmentCreatedAsync(long noteAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.NoteAttachment, resourceName: "NoteAttachment", resourceId: noteAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task NoteAttachmentUpdatedAsync(long noteAttachmentId, NoteAttachment objectBeforeUpdate, NoteAttachment objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.NoteAttachment, resourceName: "NoteAttachment", resourceId: noteAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task NoteAttachmentDeletedAsync(long noteAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.NoteAttachment, resourceName: "NoteAttachment", resourceId: noteAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task NoteAttachmentUnDeletedAsync(long noteAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.NoteAttachment, resourceName: "NoteAttachment", resourceId: noteAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        /*
        public async Task NoteAttachmentEnabledAsync(long noteAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.NoteAttachment, resourceName: "NoteAttachment", resourceId: noteAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }

        public async Task NoteAttachmentDisabledAsync(long noteAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.NoteAttachment, resourceName: "NoteAttachment", resourceId: noteAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        */

        public async Task HardDeleteNoteAttachmentAsync(long noteAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.NoteAttachment, resourceName: "NoteAttachment", resourceId: noteAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, browserName: browser, operatingSystemName: operatingSystemName, deviceName: deviceName);
        }
        #endregion

        #region FileAttachment
        public async Task FileAttachmentCreatedAsync(long fileAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.FileAttachment, resourceName: "FileAttachment", resourceId: fileAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task FileAttachmentUpdatedAsync(long fileAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.FileAttachment, resourceName: "FileAttachment", resourceId: fileAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task FileAttachmentDeletedAsync(long fileAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.FileAttachment, resourceName: "FileAttachment", resourceId: fileAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task FileAttachmentUnDeletedAsync(long fileAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.FileAttachment, resourceName: "FileAttachment", resourceId: fileAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task FileAttachmentEnabledAsync(long fileAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.FileAttachment, resourceName: "FileAttachment", resourceId: fileAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task FileAttachmentDisabledAsync(long fileAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.FileAttachment, resourceName: "FileAttachment", resourceId: fileAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteFileAttachmentAsync(long fileAttachmentId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.FileAttachment, resourceName: "FileAttachment", resourceId: fileAttachmentId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

        #region FileAttachmentFolder
        public async Task FileAttachmentFolderCreatedAsync(long fileAttachmentFolderId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.FileAttachmentFolder, resourceName: "FileAttachmentFolder", resourceId: fileAttachmentFolderId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task FileAttachmentFolderUpdatedAsync(long fileAttachmentFolderId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.FileAttachmentFolder, resourceName: "FileAttachmentFolder", resourceId: fileAttachmentFolderId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task FileAttachmentFolderDeletedAsync(long fileAttachmentFolderId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.FileAttachmentFolder, resourceName: "FileAttachmentFolder", resourceId: fileAttachmentFolderId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task FileAttachmentFolderUnDeletedAsync(long fileAttachmentFolderId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.FileAttachmentFolder, resourceName: "FileAttachmentFolder", resourceId: fileAttachmentFolderId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task FileAttachmentFolderEnabledAsync(long fileAttachmentFolderId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.FileAttachmentFolder, resourceName: "FileAttachmentFolder", resourceId: fileAttachmentFolderId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task FileAttachmentFolderDisabledAsync(long fileAttachmentFolderId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.FileAttachmentFolder, resourceName: "FileAttachmentFolder", resourceId: fileAttachmentFolderId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteFileAttachmentFolderAsync(long fileAttachmentFolderId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.FileAttachmentFolder, resourceName: "FileAttachmentFolder", resourceId: fileAttachmentFolderId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

        #region SingleSignOnServer
        public async Task SingleSignOnServerCreatedAsync(long singleSignOnServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SingleSignOnServer, resourceName: "SingleSignOnServer", resourceId: singleSignOnServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task SingleSignOnServerUpdatedAsync(long singleSignOnServerId, SingleSignOnServer objectBeforeUpdate, SingleSignOnServer objectAfterUpdate)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: objectBeforeUpdate, objectAfterUpdate: objectAfterUpdate, resourceType: PredefinedResourceTypes.SingleSignOnServer, resourceName: "SingleSignOnServer", resourceId: singleSignOnServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task SingleSignOnServerDeletedAsync(long singleSignOnServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SingleSignOnServer, resourceName: "SingleSignOnServer", resourceId: singleSignOnServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task SingleSignOnServerUnDeletedAsync(long singleSignOnServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SingleSignOnServer, resourceName: "SingleSignOnServer", resourceId: singleSignOnServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task SingleSignOnServerEnabledAsync(long singleSignOnServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SingleSignOnServer, resourceName: "SingleSignOnServer", resourceId: singleSignOnServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task SingleSignOnServerDisabledAsync(long singleSignOnServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SingleSignOnServer, resourceName: "SingleSignOnServer", resourceId: singleSignOnServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteSingleSignOnServerAsync(long singleSignOnServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.SingleSignOnServer, resourceName: "SingleSignOnServer", resourceId: singleSignOnServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

        #region AuditOperationDetail
        public async Task AuditOperationDetailCreatedAsync(long auditOperationDetailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.AuditOperationDetail, resourceName: "AuditOperationDetail", resourceId: auditOperationDetailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task AuditOperationDetailUpdatedAsync(long auditOperationDetailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.AuditOperationDetail, resourceName: "AuditOperationDetail", resourceId: auditOperationDetailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task AuditOperationDetailDeletedAsync(long auditOperationDetailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.AuditOperationDetail, resourceName: "AuditOperationDetail", resourceId: auditOperationDetailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task AuditOperationDetailUnDeletedAsync(long auditOperationDetailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.AuditOperationDetail, resourceName: "AuditOperationDetail", resourceId: auditOperationDetailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task AuditOperationDetailEnabledAsync(long auditOperationDetailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.AuditOperationDetail, resourceName: "AuditOperationDetail", resourceId: auditOperationDetailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task AuditOperationDetailDisabledAsync(long auditOperationDetailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.AuditOperationDetail, resourceName: "AuditOperationDetail", resourceId: auditOperationDetailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteAuditOperationDetailAsync(long auditOperationDetailId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.AuditOperationDetail, resourceName: "AuditOperationDetail", resourceId: auditOperationDetailId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

        #region Chat
        public async Task ChatCreatedAsync(long chatId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Chat, resourceName: "Chat", resourceId: chatId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task ChatUpdatedAsync(long chatId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Chat, resourceName: "Chat", resourceId: chatId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task ChatDeletedAsync(long chatId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Chat, resourceName: "Chat", resourceId: chatId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task ChatUnDeletedAsync(long chatId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Chat, resourceName: "Chat", resourceId: chatId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task ChatEnabledAsync(long chatId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.Chat, resourceName: "Chat", resourceId: chatId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task ChatDisabledAsync(long chatId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.Chat, resourceName: "Chat", resourceId: chatId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteChatAsync(long chatId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Chat, resourceName: "Chat", resourceId: chatId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

        #region ChatMessage
        public async Task ChatMessageCreatedAsync(long chatMessageId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ChatMessage, resourceName: "ChatMessage", resourceId: chatMessageId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task ChatMessageUpdatedAsync(long chatMessageId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ChatMessage, resourceName: "ChatMessage", resourceId: chatMessageId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task ChatMessageDeletedAsync(long chatMessageId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ChatMessage, resourceName: "ChatMessage", resourceId: chatMessageId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task ChatMessageUnDeletedAsync(long chatMessageId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ChatMessage, resourceName: "ChatMessage", resourceId: chatMessageId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task ChatMessageEnabledAsync(long chatMessageId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.ChatMessage, resourceName: "ChatMessage", resourceId: chatMessageId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task ChatMessageDisabledAsync(long chatMessageId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.ChatMessage, resourceName: "ChatMessage", resourceId: chatMessageId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteChatMessageAsync(long chatMessageId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ChatMessage, resourceName: "ChatMessage", resourceId: chatMessageId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

        #region ChatMessageStatus
        public async Task ChatMessageStatusCreatedAsync(long chatMessageStatusId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ChatMessageStatus, resourceName: "ChatMessageStatus", resourceId: chatMessageStatusId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task ChatMessageStatusUpdatedAsync(long chatMessageStatusId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ChatMessageStatus, resourceName: "ChatMessageStatus", resourceId: chatMessageStatusId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task ChatMessageStatusDeletedAsync(long chatMessageStatusId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ChatMessageStatus, resourceName: "ChatMessageStatus", resourceId: chatMessageStatusId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task ChatMessageStatusUnDeletedAsync(long chatMessageStatusId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ChatMessageStatus, resourceName: "ChatMessageStatus", resourceId: chatMessageStatusId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task ChatMessageStatusEnabledAsync(long chatMessageStatusId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.ChatMessageStatus, resourceName: "ChatMessageStatus", resourceId: chatMessageStatusId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task ChatMessageStatusDisabledAsync(long chatMessageStatusId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.ChatMessageStatus, resourceName: "ChatMessageStatus", resourceId: chatMessageStatusId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteChatMessageStatusAsync(long chatMessageStatusId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ChatMessageStatus, resourceName: "ChatMessageStatus", resourceId: chatMessageStatusId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

        #region ChatUser
        public async Task ChatUserCreatedAsync(long chatUserId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ChatUser, resourceName: "ChatUser", resourceId: chatUserId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task ChatUserUpdatedAsync(long chatUserId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ChatUser, resourceName: "ChatUser", resourceId: chatUserId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task ChatUserDeletedAsync(long chatUserId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ChatUser, resourceName: "ChatUser", resourceId: chatUserId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task ChatUserUnDeletedAsync(long chatUserId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ChatUser, resourceName: "ChatUser", resourceId: chatUserId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task ChatUserEnabledAsync(long chatUserId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.ChatUser, resourceName: "ChatUser", resourceId: chatUserId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task ChatUserDisabledAsync(long chatUserId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.ChatUser, resourceName: "ChatUser", resourceId: chatUserId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteChatUserAsync(long chatUserId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.ChatUser, resourceName: "ChatUser", resourceId: chatUserId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

        #region PushNotificationServer
        public async Task PushNotificationServerCreatedAsync(long pushNotificationConfigurationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.PushNotificationServer, resourceName: "PushNotificationServer", resourceId: pushNotificationConfigurationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task PushNotificationServerUpdatedAsync(long pushNotificationConfigurationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.PushNotificationServer, resourceName: "PushNotificationServer", resourceId: pushNotificationConfigurationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task PushNotificationServerDeletedAsync(long pushNotificationConfigurationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.PushNotificationServer, resourceName: "PushNotificationServer", resourceId: pushNotificationConfigurationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task PushNotificationServerUnDeletedAsync(long pushNotificationConfigurationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.PushNotificationServer, resourceName: "PushNotificationServer", resourceId: pushNotificationConfigurationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task PushNotificationServerEnabledAsync(long pushNotificationConfigurationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.PushNotificationServer, resourceName: "PushNotificationServer", resourceId: pushNotificationConfigurationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task PushNotificationServerDisabledAsync(long pushNotificationConfigurationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.PushNotificationServer, resourceName: "PushNotificationServer", resourceId: pushNotificationConfigurationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task HardDeletePushNotificationServerAsync(long pushNotificationConfigurationId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.PushNotificationServer, resourceName: "PushNotificationServer", resourceId: pushNotificationConfigurationId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

        #region VoiceNotificationServer
        public async Task VoiceNotificationServerCreatedAsync(long voiceNotificationServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VoiceNotificationServer, resourceName: "VoiceNotificationServer", resourceId: voiceNotificationServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VoiceNotificationServerUpdatedAsync(long voiceNotificationServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VoiceNotificationServer, resourceName: "VoiceNotificationServer", resourceId: voiceNotificationServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VoiceNotificationServerDeletedAsync(long voiceNotificationServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VoiceNotificationServer, resourceName: "VoiceNotificationServer", resourceId: voiceNotificationServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VoiceNotificationServerUnDeletedAsync(long voiceNotificationServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VoiceNotificationServer, resourceName: "VoiceNotificationServer", resourceId: voiceNotificationServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VoiceNotificationServerEnabledAsync(long voiceNotificationServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VoiceNotificationServer, resourceName: "VoiceNotificationServer", resourceId: voiceNotificationServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VoiceNotificationServerDisabledAsync(long voiceNotificationServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VoiceNotificationServer, resourceName: "VoiceNotificationServer", resourceId: voiceNotificationServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task HardDeleteVoiceNotificationServerAsync(long voiceNotificationServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VoiceNotificationServer, resourceName: "VoiceNotificationServer", resourceId: voiceNotificationServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

        #region WhatsAppNotificationServer
        public async Task WhatsAppNotificationServerCreatedAsync(long whatsAppNotificationServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.WhatsAppNotificationServer, resourceName: "WhatsAppNotificationServer", resourceId: whatsAppNotificationServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task WhatsAppNotificationServerUpdatedAsync(long whatsAppNotificationServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.WhatsAppNotificationServer, resourceName: "WhatsAppNotificationServer", resourceId: whatsAppNotificationServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task WhatsAppNotificationServerDeletedAsync(long whatsAppNotificationServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.WhatsAppNotificationServer, resourceName: "WhatsAppNotificationServer", resourceId: whatsAppNotificationServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task WhatsAppNotificationServerUnDeletedAsync(long whatsAppNotificationServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.WhatsAppNotificationServer, resourceName: "WhatsAppNotificationServer", resourceId: whatsAppNotificationServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task WhatsAppNotificationServerEnabledAsync(long whatsAppNotificationServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.WhatsAppNotificationServer, resourceName: "WhatsAppNotificationServer", resourceId: whatsAppNotificationServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task WhatsAppNotificationServerDisabledAsync(long whatsAppNotificationServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.WhatsAppNotificationServer, resourceName: "WhatsAppNotificationServer", resourceId: whatsAppNotificationServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task HardDeleteWhatsAppNotificationServerAsync(long whatsAppNotificationServerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.WhatsAppNotificationServer, resourceName: "WhatsAppNotificationServer", resourceId: whatsAppNotificationServerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

        #region UserGroup
        public async Task UserGroupCreatedAsync(long userGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.UserGroup, resourceName: "UserGroup", resourceId: userGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task UserGroupUpdatedAsync(long userGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.UserGroup, resourceName: "UserGroup", resourceId: userGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task UserGroupDeletedAsync(long userGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.UserGroup, resourceName: "UserGroup", resourceId: userGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task UserGroupUnDeletedAsync(long userGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.UserGroup, resourceName: "UserGroup", resourceId: userGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task UserGroupEnabledAsync(long userGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.UserGroup, resourceName: "UserGroup", resourceId: userGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task UserGroupDisabledAsync(long userGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.UserGroup, resourceName: "UserGroup", resourceId: userGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteUserGroupAsync(long userGroupId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.UserGroup, resourceName: "UserGroup", resourceId: userGroupId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion
        
        #region VisionBoardSessionPicture
        public async Task VisionBoardSessionPictureCreatedAsync(long visionBoardSessionPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VisionBoardSessionPicture,  resourceName: "VisionBoardSessionPicture", resourceId: visionBoardSessionPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VisionBoardSessionPictureUpdatedAsync(long visionBoardSessionPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VisionBoardSessionPicture,  resourceName: "VisionBoardSessionPicture", resourceId: visionBoardSessionPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VisionBoardSessionPictureDeletedAsync(long visionBoardSessionPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VisionBoardSessionPicture,  resourceName: "VisionBoardSessionPicture", resourceId: visionBoardSessionPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VisionBoardSessionPictureUnDeletedAsync(long visionBoardSessionPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VisionBoardSessionPicture,  resourceName: "VisionBoardSessionPicture", resourceId: visionBoardSessionPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task VisionBoardSessionPictureEnabledAsync(long visionBoardSessionPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.VisionBoardSessionPicture, resourceName: "VisionBoardSessionPicture", resourceId: visionBoardSessionPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VisionBoardSessionPictureDisabledAsync(long visionBoardSessionPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.VisionBoardSessionPicture, resourceName: "VisionBoardSessionPicture", resourceId: visionBoardSessionPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteVisionBoardSessionPictureAsync(long visionBoardSessionPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VisionBoardSessionPicture,  resourceName: "VisionBoardSessionPicture", resourceId: visionBoardSessionPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region Category
        public async Task CategoryCreatedAsync(long categoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Category,  resourceName: "Category", resourceId: categoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task CategoryUpdatedAsync(long categoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Category,  resourceName: "Category", resourceId: categoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task CategoryDeletedAsync(long categoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Category,  resourceName: "Category", resourceId: categoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task CategoryUnDeletedAsync(long categoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Category,  resourceName: "Category", resourceId: categoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task CategoryEnabledAsync(long categoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.Category, resourceName: "Category", resourceId: categoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task CategoryDisabledAsync(long categoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.Category, resourceName: "Category", resourceId: categoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteCategoryAsync(long categoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Category,  resourceName: "Category", resourceId: categoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region DailyAffirmationSession
        public async Task DailyAffirmationSessionCreatedAsync(long dailyAffirmationSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DailyAffirmationSession,  resourceName: "DailyAffirmationSession", resourceId: dailyAffirmationSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DailyAffirmationSessionUpdatedAsync(long dailyAffirmationSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DailyAffirmationSession,  resourceName: "DailyAffirmationSession", resourceId: dailyAffirmationSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DailyAffirmationSessionDeletedAsync(long dailyAffirmationSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DailyAffirmationSession,  resourceName: "DailyAffirmationSession", resourceId: dailyAffirmationSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DailyAffirmationSessionUnDeletedAsync(long dailyAffirmationSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DailyAffirmationSession,  resourceName: "DailyAffirmationSession", resourceId: dailyAffirmationSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task DailyAffirmationSessionEnabledAsync(long dailyAffirmationSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.DailyAffirmationSession, resourceName: "DailyAffirmationSession", resourceId: dailyAffirmationSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DailyAffirmationSessionDisabledAsync(long dailyAffirmationSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.DailyAffirmationSession, resourceName: "DailyAffirmationSession", resourceId: dailyAffirmationSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteDailyAffirmationSessionAsync(long dailyAffirmationSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DailyAffirmationSession,  resourceName: "DailyAffirmationSession", resourceId: dailyAffirmationSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region DailyAffirmationSessionCategory
        public async Task DailyAffirmationSessionCategoryCreatedAsync(long dailyAffirmationSessionCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DailyAffirmationSessionCategory,  resourceName: "DailyAffirmationSessionCategory", resourceId: dailyAffirmationSessionCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DailyAffirmationSessionCategoryUpdatedAsync(long dailyAffirmationSessionCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DailyAffirmationSessionCategory,  resourceName: "DailyAffirmationSessionCategory", resourceId: dailyAffirmationSessionCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DailyAffirmationSessionCategoryDeletedAsync(long dailyAffirmationSessionCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DailyAffirmationSessionCategory,  resourceName: "DailyAffirmationSessionCategory", resourceId: dailyAffirmationSessionCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DailyAffirmationSessionCategoryUnDeletedAsync(long dailyAffirmationSessionCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DailyAffirmationSessionCategory,  resourceName: "DailyAffirmationSessionCategory", resourceId: dailyAffirmationSessionCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task DailyAffirmationSessionCategoryEnabledAsync(long dailyAffirmationSessionCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.DailyAffirmationSessionCategory, resourceName: "DailyAffirmationSessionCategory", resourceId: dailyAffirmationSessionCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DailyAffirmationSessionCategoryDisabledAsync(long dailyAffirmationSessionCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.DailyAffirmationSessionCategory, resourceName: "DailyAffirmationSessionCategory", resourceId: dailyAffirmationSessionCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteDailyAffirmationSessionCategoryAsync(long dailyAffirmationSessionCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DailyAffirmationSessionCategory,  resourceName: "DailyAffirmationSessionCategory", resourceId: dailyAffirmationSessionCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region DailyAffirmationText
        public async Task DailyAffirmationTextCreatedAsync(long dailyAffirmationTextId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DailyAffirmationText,  resourceName: "DailyAffirmationText", resourceId: dailyAffirmationTextId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DailyAffirmationTextUpdatedAsync(long dailyAffirmationTextId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DailyAffirmationText,  resourceName: "DailyAffirmationText", resourceId: dailyAffirmationTextId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DailyAffirmationTextDeletedAsync(long dailyAffirmationTextId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DailyAffirmationText,  resourceName: "DailyAffirmationText", resourceId: dailyAffirmationTextId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DailyAffirmationTextUnDeletedAsync(long dailyAffirmationTextId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DailyAffirmationText,  resourceName: "DailyAffirmationText", resourceId: dailyAffirmationTextId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task DailyAffirmationTextEnabledAsync(long dailyAffirmationTextId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.DailyAffirmationText, resourceName: "DailyAffirmationText", resourceId: dailyAffirmationTextId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DailyAffirmationTextDisabledAsync(long dailyAffirmationTextId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.DailyAffirmationText, resourceName: "DailyAffirmationText", resourceId: dailyAffirmationTextId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteDailyAffirmationTextAsync(long dailyAffirmationTextId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DailyAffirmationText,  resourceName: "DailyAffirmationText", resourceId: dailyAffirmationTextId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region DefaultCategory
        public async Task DefaultCategoryCreatedAsync(long defaultCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultCategory,  resourceName: "DefaultCategory", resourceId: defaultCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultCategoryUpdatedAsync(long defaultCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultCategory,  resourceName: "DefaultCategory", resourceId: defaultCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultCategoryDeletedAsync(long defaultCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultCategory,  resourceName: "DefaultCategory", resourceId: defaultCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultCategoryUnDeletedAsync(long defaultCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultCategory,  resourceName: "DefaultCategory", resourceId: defaultCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task DefaultCategoryEnabledAsync(long defaultCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.DefaultCategory, resourceName: "DefaultCategory", resourceId: defaultCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultCategoryDisabledAsync(long defaultCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.DefaultCategory, resourceName: "DefaultCategory", resourceId: defaultCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteDefaultCategoryAsync(long defaultCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultCategory,  resourceName: "DefaultCategory", resourceId: defaultCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region DefaultDailyAffirmationText
        public async Task DefaultDailyAffirmationTextCreatedAsync(long defaultDailyAffirmationTextId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultDailyAffirmationText,  resourceName: "DefaultDailyAffirmationText", resourceId: defaultDailyAffirmationTextId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultDailyAffirmationTextUpdatedAsync(long defaultDailyAffirmationTextId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultDailyAffirmationText,  resourceName: "DefaultDailyAffirmationText", resourceId: defaultDailyAffirmationTextId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultDailyAffirmationTextDeletedAsync(long defaultDailyAffirmationTextId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultDailyAffirmationText,  resourceName: "DefaultDailyAffirmationText", resourceId: defaultDailyAffirmationTextId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultDailyAffirmationTextUnDeletedAsync(long defaultDailyAffirmationTextId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultDailyAffirmationText,  resourceName: "DefaultDailyAffirmationText", resourceId: defaultDailyAffirmationTextId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task DefaultDailyAffirmationTextEnabledAsync(long defaultDailyAffirmationTextId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.DefaultDailyAffirmationText, resourceName: "DefaultDailyAffirmationText", resourceId: defaultDailyAffirmationTextId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultDailyAffirmationTextDisabledAsync(long defaultDailyAffirmationTextId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.DefaultDailyAffirmationText, resourceName: "DefaultDailyAffirmationText", resourceId: defaultDailyAffirmationTextId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteDefaultDailyAffirmationTextAsync(long defaultDailyAffirmationTextId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultDailyAffirmationText,  resourceName: "DefaultDailyAffirmationText", resourceId: defaultDailyAffirmationTextId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region DefaultGoalTrackerCategory
        public async Task DefaultGoalTrackerCategoryCreatedAsync(long defaultGoalTrackerCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultGoalTrackerCategory,  resourceName: "DefaultGoalTrackerCategory", resourceId: defaultGoalTrackerCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultGoalTrackerCategoryUpdatedAsync(long defaultGoalTrackerCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultGoalTrackerCategory,  resourceName: "DefaultGoalTrackerCategory", resourceId: defaultGoalTrackerCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultGoalTrackerCategoryDeletedAsync(long defaultGoalTrackerCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultGoalTrackerCategory,  resourceName: "DefaultGoalTrackerCategory", resourceId: defaultGoalTrackerCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultGoalTrackerCategoryUnDeletedAsync(long defaultGoalTrackerCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultGoalTrackerCategory,  resourceName: "DefaultGoalTrackerCategory", resourceId: defaultGoalTrackerCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task DefaultGoalTrackerCategoryEnabledAsync(long defaultGoalTrackerCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.DefaultGoalTrackerCategory, resourceName: "DefaultGoalTrackerCategory", resourceId: defaultGoalTrackerCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultGoalTrackerCategoryDisabledAsync(long defaultGoalTrackerCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.DefaultGoalTrackerCategory, resourceName: "DefaultGoalTrackerCategory", resourceId: defaultGoalTrackerCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteDefaultGoalTrackerCategoryAsync(long defaultGoalTrackerCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultGoalTrackerCategory,  resourceName: "DefaultGoalTrackerCategory", resourceId: defaultGoalTrackerCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region DefaultGoalTracker
        public async Task DefaultGoalTrackerCreatedAsync(long defaultGoalTrackerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultGoalTracker,  resourceName: "DefaultGoalTracker", resourceId: defaultGoalTrackerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultGoalTrackerUpdatedAsync(long defaultGoalTrackerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultGoalTracker,  resourceName: "DefaultGoalTracker", resourceId: defaultGoalTrackerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultGoalTrackerDeletedAsync(long defaultGoalTrackerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultGoalTracker,  resourceName: "DefaultGoalTracker", resourceId: defaultGoalTrackerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultGoalTrackerUnDeletedAsync(long defaultGoalTrackerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultGoalTracker,  resourceName: "DefaultGoalTracker", resourceId: defaultGoalTrackerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task DefaultGoalTrackerEnabledAsync(long defaultGoalTrackerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.DefaultGoalTracker, resourceName: "DefaultGoalTracker", resourceId: defaultGoalTrackerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultGoalTrackerDisabledAsync(long defaultGoalTrackerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.DefaultGoalTracker, resourceName: "DefaultGoalTracker", resourceId: defaultGoalTrackerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteDefaultGoalTrackerAsync(long defaultGoalTrackerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultGoalTracker,  resourceName: "DefaultGoalTracker", resourceId: defaultGoalTrackerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region DefaultMeditationFile
        public async Task DefaultMeditationFileCreatedAsync(long defaultMeditationFileId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultMeditationFile,  resourceName: "DefaultMeditationFile", resourceId: defaultMeditationFileId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultMeditationFileUpdatedAsync(long defaultMeditationFileId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultMeditationFile,  resourceName: "DefaultMeditationFile", resourceId: defaultMeditationFileId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultMeditationFileDeletedAsync(long defaultMeditationFileId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultMeditationFile,  resourceName: "DefaultMeditationFile", resourceId: defaultMeditationFileId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultMeditationFileUnDeletedAsync(long defaultMeditationFileId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultMeditationFile,  resourceName: "DefaultMeditationFile", resourceId: defaultMeditationFileId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task DefaultMeditationFileEnabledAsync(long defaultMeditationFileId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.DefaultMeditationFile, resourceName: "DefaultMeditationFile", resourceId: defaultMeditationFileId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultMeditationFileDisabledAsync(long defaultMeditationFileId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.DefaultMeditationFile, resourceName: "DefaultMeditationFile", resourceId: defaultMeditationFileId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteDefaultMeditationFileAsync(long defaultMeditationFileId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultMeditationFile,  resourceName: "DefaultMeditationFile", resourceId: defaultMeditationFileId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region DefaultVisionBoardPicture
        public async Task DefaultVisionBoardPictureCreatedAsync(long defaultVisionBoardPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultVisionBoardPicture,  resourceName: "DefaultVisionBoardPicture", resourceId: defaultVisionBoardPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultVisionBoardPictureUpdatedAsync(long defaultVisionBoardPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultVisionBoardPicture,  resourceName: "DefaultVisionBoardPicture", resourceId: defaultVisionBoardPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultVisionBoardPictureDeletedAsync(long defaultVisionBoardPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultVisionBoardPicture,  resourceName: "DefaultVisionBoardPicture", resourceId: defaultVisionBoardPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultVisionBoardPictureUnDeletedAsync(long defaultVisionBoardPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultVisionBoardPicture,  resourceName: "DefaultVisionBoardPicture", resourceId: defaultVisionBoardPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task DefaultVisionBoardPictureEnabledAsync(long defaultVisionBoardPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.DefaultVisionBoardPicture, resourceName: "DefaultVisionBoardPicture", resourceId: defaultVisionBoardPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task DefaultVisionBoardPictureDisabledAsync(long defaultVisionBoardPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.DefaultVisionBoardPicture, resourceName: "DefaultVisionBoardPicture", resourceId: defaultVisionBoardPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteDefaultVisionBoardPictureAsync(long defaultVisionBoardPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.DefaultVisionBoardPicture,  resourceName: "DefaultVisionBoardPicture", resourceId: defaultVisionBoardPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region Feed
        public async Task FeedCreatedAsync(long feedId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Feed,  resourceName: "Feed", resourceId: feedId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task FeedUpdatedAsync(long feedId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Feed,  resourceName: "Feed", resourceId: feedId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task FeedDeletedAsync(long feedId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Feed,  resourceName: "Feed", resourceId: feedId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task FeedUnDeletedAsync(long feedId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Feed,  resourceName: "Feed", resourceId: feedId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task FeedEnabledAsync(long feedId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.Feed, resourceName: "Feed", resourceId: feedId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task FeedDisabledAsync(long feedId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.Feed, resourceName: "Feed", resourceId: feedId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteFeedAsync(long feedId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.Feed,  resourceName: "Feed", resourceId: feedId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region GoalTracker
        public async Task GoalTrackerCreatedAsync(long goalTrackerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.GoalTracker,  resourceName: "GoalTracker", resourceId: goalTrackerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task GoalTrackerUpdatedAsync(long goalTrackerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.GoalTracker,  resourceName: "GoalTracker", resourceId: goalTrackerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task GoalTrackerDeletedAsync(long goalTrackerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.GoalTracker,  resourceName: "GoalTracker", resourceId: goalTrackerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task GoalTrackerUnDeletedAsync(long goalTrackerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.GoalTracker,  resourceName: "GoalTracker", resourceId: goalTrackerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task GoalTrackerEnabledAsync(long goalTrackerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.GoalTracker, resourceName: "GoalTracker", resourceId: goalTrackerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task GoalTrackerDisabledAsync(long goalTrackerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.GoalTracker, resourceName: "GoalTracker", resourceId: goalTrackerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteGoalTrackerAsync(long goalTrackerId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.GoalTracker,  resourceName: "GoalTracker", resourceId: goalTrackerId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region GoalTrackerCategory
        public async Task GoalTrackerCategoryCreatedAsync(long goalTrackerCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.GoalTrackerCategory,  resourceName: "GoalTrackerCategory", resourceId: goalTrackerCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task GoalTrackerCategoryUpdatedAsync(long goalTrackerCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.GoalTrackerCategory,  resourceName: "GoalTrackerCategory", resourceId: goalTrackerCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task GoalTrackerCategoryDeletedAsync(long goalTrackerCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.GoalTrackerCategory,  resourceName: "GoalTrackerCategory", resourceId: goalTrackerCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task GoalTrackerCategoryUnDeletedAsync(long goalTrackerCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.GoalTrackerCategory,  resourceName: "GoalTrackerCategory", resourceId: goalTrackerCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task GoalTrackerCategoryEnabledAsync(long goalTrackerCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.GoalTrackerCategory, resourceName: "GoalTrackerCategory", resourceId: goalTrackerCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task GoalTrackerCategoryDisabledAsync(long goalTrackerCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.GoalTrackerCategory, resourceName: "GoalTrackerCategory", resourceId: goalTrackerCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteGoalTrackerCategoryAsync(long goalTrackerCategoryId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.GoalTrackerCategory,  resourceName: "GoalTrackerCategory", resourceId: goalTrackerCategoryId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region GoalTrackerMileStone
        public async Task GoalTrackerMileStoneCreatedAsync(long goalTrackerMileStoneId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.GoalTrackerMileStone,  resourceName: "GoalTrackerMileStone", resourceId: goalTrackerMileStoneId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task GoalTrackerMileStoneUpdatedAsync(long goalTrackerMileStoneId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.GoalTrackerMileStone,  resourceName: "GoalTrackerMileStone", resourceId: goalTrackerMileStoneId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task GoalTrackerMileStoneDeletedAsync(long goalTrackerMileStoneId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.GoalTrackerMileStone,  resourceName: "GoalTrackerMileStone", resourceId: goalTrackerMileStoneId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task GoalTrackerMileStoneUnDeletedAsync(long goalTrackerMileStoneId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.GoalTrackerMileStone,  resourceName: "GoalTrackerMileStone", resourceId: goalTrackerMileStoneId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task GoalTrackerMileStoneEnabledAsync(long goalTrackerMileStoneId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.GoalTrackerMileStone, resourceName: "GoalTrackerMileStone", resourceId: goalTrackerMileStoneId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task GoalTrackerMileStoneDisabledAsync(long goalTrackerMileStoneId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.GoalTrackerMileStone, resourceName: "GoalTrackerMileStone", resourceId: goalTrackerMileStoneId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteGoalTrackerMileStoneAsync(long goalTrackerMileStoneId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.GoalTrackerMileStone,  resourceName: "GoalTrackerMileStone", resourceId: goalTrackerMileStoneId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region MeditationFile
        public async Task MeditationFileCreatedAsync(long meditationFileId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MeditationFile,  resourceName: "MeditationFile", resourceId: meditationFileId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task MeditationFileUpdatedAsync(long meditationFileId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MeditationFile,  resourceName: "MeditationFile", resourceId: meditationFileId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task MeditationFileDeletedAsync(long meditationFileId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MeditationFile,  resourceName: "MeditationFile", resourceId: meditationFileId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task MeditationFileUnDeletedAsync(long meditationFileId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MeditationFile,  resourceName: "MeditationFile", resourceId: meditationFileId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task MeditationFileEnabledAsync(long meditationFileId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.MeditationFile, resourceName: "MeditationFile", resourceId: meditationFileId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task MeditationFileDisabledAsync(long meditationFileId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.MeditationFile, resourceName: "MeditationFile", resourceId: meditationFileId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteMeditationFileAsync(long meditationFileId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MeditationFile,  resourceName: "MeditationFile", resourceId: meditationFileId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region MeditationSession
        public async Task MeditationSessionCreatedAsync(long meditationSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MeditationSession,  resourceName: "MeditationSession", resourceId: meditationSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task MeditationSessionUpdatedAsync(long meditationSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MeditationSession,  resourceName: "MeditationSession", resourceId: meditationSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task MeditationSessionDeletedAsync(long meditationSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MeditationSession,  resourceName: "MeditationSession", resourceId: meditationSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task MeditationSessionUnDeletedAsync(long meditationSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MeditationSession,  resourceName: "MeditationSession", resourceId: meditationSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task MeditationSessionEnabledAsync(long meditationSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.MeditationSession, resourceName: "MeditationSession", resourceId: meditationSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task MeditationSessionDisabledAsync(long meditationSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.MeditationSession, resourceName: "MeditationSession", resourceId: meditationSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteMeditationSessionAsync(long meditationSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.MeditationSession,  resourceName: "MeditationSession", resourceId: meditationSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region VisionBoardPicture
        public async Task VisionBoardPictureCreatedAsync(long visionBoardPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VisionBoardPicture,  resourceName: "VisionBoardPicture", resourceId: visionBoardPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VisionBoardPictureUpdatedAsync(long visionBoardPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VisionBoardPicture,  resourceName: "VisionBoardPicture", resourceId: visionBoardPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VisionBoardPictureDeletedAsync(long visionBoardPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VisionBoardPicture,  resourceName: "VisionBoardPicture", resourceId: visionBoardPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VisionBoardPictureUnDeletedAsync(long visionBoardPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VisionBoardPicture,  resourceName: "VisionBoardPicture", resourceId: visionBoardPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task VisionBoardPictureEnabledAsync(long visionBoardPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.VisionBoardPicture, resourceName: "VisionBoardPicture", resourceId: visionBoardPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VisionBoardPictureDisabledAsync(long visionBoardPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.VisionBoardPicture, resourceName: "VisionBoardPicture", resourceId: visionBoardPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteVisionBoardPictureAsync(long visionBoardPictureId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VisionBoardPicture,  resourceName: "VisionBoardPicture", resourceId: visionBoardPictureId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion

#region VisionBoardSession
        public async Task VisionBoardSessionCreatedAsync(long visionBoardSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VisionBoardSession,  resourceName: "VisionBoardSession", resourceId: visionBoardSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VisionBoardSessionUpdatedAsync(long visionBoardSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VisionBoardSession,  resourceName: "VisionBoardSession", resourceId: visionBoardSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VisionBoardSessionDeletedAsync(long visionBoardSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VisionBoardSession,  resourceName: "VisionBoardSession", resourceId: visionBoardSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VisionBoardSessionUnDeletedAsync(long visionBoardSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VisionBoardSession,  resourceName: "VisionBoardSession", resourceId: visionBoardSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        /*
        public async Task VisionBoardSessionEnabledAsync(long visionBoardSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.VisionBoardSession, resourceName: "VisionBoardSession", resourceId: visionBoardSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }

        public async Task VisionBoardSessionDisabledAsync(long visionBoardSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.VisionBoardSession, resourceName: "VisionBoardSession", resourceId: visionBoardSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        */

        public async Task HardDeleteVisionBoardSessionAsync(long visionBoardSessionId)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.VisionBoardSession,  resourceName: "VisionBoardSession", resourceId: visionBoardSessionId, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress);
        }
        #endregion
    }
}
