using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Manifest.Common.Constants;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.ViewModels;
using Manifest.Common.Models.ViewModels.UserProfile;
using Manifest.Repository.Common;
using Manifest.Service.Config;
using Manifest.Service.ObjectMapping;
using Manifest.Service.Storage;
using Framework.Infrastructure.Exceptions;
using Framework.Infrastructure.Interfaces.DbAccess;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;

namespace Manifest.Service.App
{
    public class CompanyService : BaseAppService
    {
        private readonly CompanyRepository companyRepository;
        private readonly SystemRepository systemRepository;
        private readonly ITransactionManager transactionManager;
        private readonly AuditService auditService;
        private readonly PermissionService permissionService;
        private readonly ICurrentSessionService currentSessionService;
        private readonly AppConfigurationFile appConfigurationFile;
        private readonly ProfilePictureFileStorage profilePictureFileStorage;
        private readonly CompanyRepository wipRepository;

        public CompanyService(
                           ILog log,
                           CompanyRepository companyRepository,
                           SystemRepository systemRepository,
                           ObjectMapperService objectMapperService,
                           AuditService auditService,
                           PermissionService permissionService,
                           ITransactionManager transactionManager,
                           ICurrentSessionService currentSessionService,
                           AppConfigurationFile appConfigurationFile,
                           ProfilePictureFileStorage profilePictureFileStorage)
            : base(log, objectMapperService)
        {
            this.companyRepository = companyRepository;
            this.systemRepository = systemRepository;
            this.auditService = auditService;
            this.permissionService = permissionService;
            this.transactionManager = transactionManager;
            this.currentSessionService = currentSessionService;
            this.appConfigurationFile = appConfigurationFile;
            this.profilePictureFileStorage = profilePictureFileStorage;
            wipRepository = companyRepository;
        }

        #region User
        public async Task<ReturnModel<bool>> UploadUserPictureAsync(long userId, FileStreamModel streamModel)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);
                if (!user.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(user);
                }

                if (streamModel == null)
                {
                    return log.ErrorAndReturnModel<bool>("File Content is not provided");
                }

                try
                {
                    using (var trx = await transactionManager.BeginTransactionAsync())
                    {
                        var path = profilePictureFileStorage.ProfilePicturePathForUser(0, user.Model.UserId, streamModel.FileName);
                        await companyRepository.UpdateUserPicturePathAsync(user.Model.UserId, path, streamModel.FileName, await currentSessionService.GetCurrentUserIdAsync(),user.Model.ProfileImageGuid);
                        await profilePictureFileStorage.WriteAsync(0, path, streamModel.FileData);
                        await trx.CompleteAsync();
                    }
                }
                finally
                {
                    FileUtils.CloseSafely(streamModel.FileData);
                }

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting Users", ex);
            }
        }

        public async Task<ReturnModel<bool>> ClearUserPictureAsync(long userId)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);
                if (!user.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(user);
                }

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    await companyRepository.ClearUserProfilePathAsync(user.Model.UserId, await currentSessionService.GetCurrentUserIdAsync());
                    await profilePictureFileStorage.DeleteAsync(0, user.Model.ProfileImageLocation);
                }

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting Users", ex);
            }
        }

        public async Task<ReturnModel<FileStreamModel>> GetUserPictureStreamAsync(long userId)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);
                if (!user.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>(user);
                }

                if (user.Model.ProfileImageLocation.IsTrimmedStringNullOrEmpty())
                {
                    return ReturnModel<FileStreamModel>.Error("Profile picture is empty");
                }

                if (!await profilePictureFileStorage.ExistsAsync(0, user.Model.ProfileImageLocation))
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the picture for User", new { userId });

                return ReturnModel<FileStreamModel>.Success(new FileStreamModel { FileData = await profilePictureFileStorage.OpenReadAsync(0, user.Model.ProfileImageLocation), ContentType = FileUtils.GetMimeTypeFromFileName(user.Model.ProfileImageLocation), FileName = user.Model.ProfileImageActualFileName });
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting User\'s Picture", ex);
            }
        }

        public async Task<ReturnListModel<UserModel>> GetUsersAsync(UserSearchCriteria search)
        {
            try
            {
                return this.ConvertToList(await companyRepository.GetUsersAsync(search), objectMapperService.UserViewListToUserModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<UserModel>($"Unexpected error when getting Users", ex);
            }
        }

        public async Task<ReturnModel<UserModel>> GetUserByIdAsync(long userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return log.ErrorAndReturnModel<UserModel>($"Invalid Guid {userId}");
                }

                var item = await companyRepository.GetUserViewByIdAsync(userId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<UserModel>($"Unable to find the user for {userId}");
                }

                return this.ConvertToItem(item, objectMapperService.UserViewToUserModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<UserModel>($"Unexpected error when getting user for - {userId}", ex);
            }
        }

        public async Task<ReturnModel<UserModel>> CreateUserAsync(UserCreateModel user, FileStreamModel streamModel)
        {
            try
            {
                if (user == null)
                {
                    return log.ErrorAndReturnModel<UserModel>($"Unable to create User", new ReturnErrorItem("", "User information is not valid"));
                }

                if (!await currentSessionService.IsCurrentUserASuperAdminAsync())
                {
                    if (user.PredefinedUserTypeId == (int)PredefinedUserTypes.SuperAdmin)
                    {
                        return log.ErrorAndReturnModel<UserModel>($"You cannot create Super Admin Account");
                    }
                }

                user.PasswordHash = PasswordHash.CreateHash(user.InitialPassword, out _);
                user.IsDeleted = false;
                user.Active = true;
                user.UserGuid = Guid.NewGuid();
                user.NewUserActivationGuid = Guid.NewGuid();
                user.CellPhone = user.CellPhone?.Trim();
                user.WorkPhone = user.WorkPhone?.Trim();
                user.Email = user.Email?.Trim();
                user.WorkPhoneExt = user.WorkPhoneExt?.Trim();
                user.UserName = user.UserName?.Trim();
                user.LastName = user.LastName?.Trim();
                user.FirstName = user.FirstName?.Trim();

                var errorList = await IsUserModelValidForCreateAsync(user);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<UserModel>($"Unable to create new user", errorList);
                }

                User item;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await companyRepository.CreateUserAsync(user, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create new user");
                    }

                    if (streamModel != null)
                    {
                        try
                        {
                            var path = profilePictureFileStorage.ProfilePicturePathForUser(0, item.UserId, streamModel.FileName);
                            await profilePictureFileStorage.WriteAsync(0, path, streamModel.FileData);
                            await companyRepository.UpdateUserPicturePathAsync(item.UserId, path, streamModel.FileName, await currentSessionService.GetCurrentUserIdAsync(), item.ProfileImageGuid);
                        }
                        finally
                        {
                            FileUtils.CloseSafely(streamModel.FileData);
                        }
                    }

                    await trx.CompleteAsync();
                }

                await auditService.UserCreatedAsync(item.UserName);

                var view = await companyRepository.GetUserViewByIdAsync(item.UserId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<UserModel>($"Unable to find the user for {item.UserId}");
                }

                return this.ConvertToItem(view,objectMapperService.UserViewToUserModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<UserModel>($"Unexpected error when creating User", new { user?.UserId }, ex);
            }
        }

        public async Task<ReturnModel<UserModel>> UpdateUserAsync(UserUpdateModel user, FileStreamModel streamModel)
        {
            try
            {
                if (user == null)
                {
                    return log.ErrorAndReturnModel<UserModel>($"Unable to update User", new ReturnErrorItem("", "User information is not valid"));
                }

                if (!await currentSessionService.IsCurrentUserASuperAdminAsync())
                {
                    return log.ErrorAndReturnModel<UserModel>($"Only admins can update the User information");
                }

                var existingUser = await companyRepository.GetUserViewByIdAsync(user.UserId);
                if (existingUser == null)
                {
                    return log.ErrorAndReturnModel<UserModel>($"Unable to find the user for {user.UserId}");
                }

                user.CreatedByUserId = existingUser.CreatedByUserId;
                user.CreatedDate = existingUser.CreatedDate;
                user.PasswordHash = existingUser.PasswordHash;
                user.PasswordChangedDate = existingUser.PasswordChangedDate;
                user.NotificationThroughEmail = existingUser.NotificationThroughEmail;
                user.OneTimeToken = existingUser.OneTimeToken;
                user.OneTimeExpireDateTime = existingUser.OneTimeExpireDateTime;
                user.LoginFailedAttempt = existingUser.LoginFailedAttempt;
                user.IsAccountLocked = existingUser.IsAccountLocked;
                user.AccountLockedUntil = existingUser.AccountLockedUntil;
                user.NewUserActivationGuid = existingUser.NewUserActivationGuid;
                user.NewUserActivationCompletedOn = existingUser.NewUserActivationCompletedOn;
                user.IsDeleted = existingUser.IsDeleted;
                //user.Active = existingUser.Active;
                user.ProfileImageActualFileName = existingUser.ProfileImageActualFileName;
                user.ProfileImageLocation = existingUser.ProfileImageLocation;
                user.UserGuid = existingUser.UserGuid;

                user.CellPhone = user.CellPhone?.Trim();
                user.WorkPhone = user.WorkPhone?.Trim();
                user.Email = user.Email?.Trim();
                user.WorkPhoneExt = user.WorkPhoneExt?.Trim();
                user.UserName = user.UserName?.Trim();
                user.LastName = user.LastName?.Trim();
                user.FirstName = user.FirstName?.Trim();

                //user.IsDeleted = existingUser.IsDeleted;

                var errorList = await IsUserModelValidForUpdateAsync(user);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<UserModel>($"Unable to update user", errorList);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await companyRepository.UpdateUserAsync(user, await currentSessionService.GetCurrentUserIdAsync());
                    if (!result)
                    {
                        throw new RollbackException($"Unable to update user");
                    }

                    if (streamModel != null)
                    {
                        try
                        {
                            var path = profilePictureFileStorage.ProfilePicturePathForUser(0, user.UserId, streamModel.FileName);
                            await profilePictureFileStorage.WriteAsync(0, path, streamModel.FileData);
                            await companyRepository.UpdateUserPicturePathAsync(user.UserId, path, streamModel.FileName, await currentSessionService.GetCurrentUserIdAsync(), user.ProfileImageGuid);
                        }
                        finally
                        {
                            FileUtils.CloseSafely(streamModel.FileData);
                        }
                    }

                    await trx.CompleteAsync();
                }

                await auditService.UserUpdatedAsync(user.UserName);

                var view = await companyRepository.GetUserViewByIdAsync(user.UserId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<UserModel>($"Unable to find the user for {user.UserId}");
                }

                return this.ConvertToItem(view, objectMapperService.UserViewToUserModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<UserModel>($"Unexpected error when updating User", new { user?.UserId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> EnableUserByIdAsync(long userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return log.ErrorAndReturnModel<bool>($"Invalid User Id - {userId}");
                }

                var user = await companyRepository.GetUserViewByIdAsync(userId);
                if (user == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the user for {userId}");
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await companyRepository.EnableUserByIdAsync(userId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when enabling the user {userId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> DisableUserByIdAsync(long userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return log.ErrorAndReturnModel<bool>($"Invalid User Id - {userId}");
                }

                var user = await companyRepository.GetUserViewByIdAsync(userId);
                if (user == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the user for {userId}");
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await companyRepository.DisableUserByIdAsync(userId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when disabling the user {userId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkUserAsNotDeletedByIdAsync(long userId)
        {
            try
            {
                var existingUserModel = await GetUserByIdAsync(userId);
                if (!existingUserModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingUserModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await companyRepository.MarkUserAsNotDeletedByIdAsync(userId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.UserDeletedAsync(existingUserModel.Model.UserName);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking user as not deleted", new { userId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkUserAsDeletedByIdAsync(long userId)
        {
            try
            {
                if (userId == await currentSessionService.GetCurrentUserIdAsync())
                {
                    return log.ErrorAndReturnModel<bool>($"You cannot delete yourself");
                }

                var existingUserModel = await GetUserByIdAsync(userId);
                if (!existingUserModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingUserModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await companyRepository.MarkUserAsDeletedByIdAsync(userId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.UserDeletedAsync(existingUserModel.Model.UserName);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking user as deleted", new { userId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteUserByAsync(long userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return log.ErrorAndReturnModel<bool>($"Invalid Guid {userId}");
                }

                var user = await companyRepository.GetUserViewByIdAsync(userId);
                if (user == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the user for {userId}");
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await companyRepository.HardDeleteUserByIdAsync(userId);
                    await trx.CompleteAsync();
                }

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting user {userId}", ex);
            }
        }

        public Task<List<ReturnErrorItem>> IsUserModelValidForCreateAsync(UserCreateModel userModel)
        {
            //if (userModel.NewPermissionTypeIds == null || userModel.NewPermissionTypeIds.Count == 0)
            //{
            //    return new List<ReturnErrorItem>() { new ReturnErrorItem("NewPermissionIds", $"Atleast one permission needs to be selected") };
            //}

            return IsUserModelValidForAllAsync(true, userModel);
        }

        public Task<List<ReturnErrorItem>> IsUserModelValidForUpdateAsync(UserModel userModel)
        {
            //if (userModel.ExistingPermissionTypeIds.Count == 0)
            //{
            //    return new List<ReturnErrorItem>() { new ReturnErrorItem("ExistingPermissionTypeIds", $"Atleast one permission needs to be selected") };
            //}

            return IsUserModelValidForAllAsync(false, userModel);
        }

        public async Task<List<ReturnErrorItem>> IsUserModelValidForAllAsync(bool isCreate, UserModel userModel)
        {
            List<ReturnErrorItem> result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (await companyRepository.IsUserNameAlreadyExistsForOtherUserAsync(userModel.UserName))
                {
                    result.Add(new ReturnErrorItem("", "Another User already exists"));
                }

                if (userModel.UserId > 0)
                {
                    result.Add(new ReturnErrorItem("", "User Identifier is not valid"));
                }
            }
            else
            {
                if (await companyRepository.IsUserNameAlreadyExistsForOtherUserAsync(userModel.UserId, userModel.UserName))
                {
                    result.Add(new ReturnErrorItem("", "Another User already exists"));
                }

                if (userModel.UserId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "User Unique Identifier is not valid"));
                }
            }

            if (userModel.UserName != null && userModel.UserName.Length > UserModel.COLUMN_SIZE_UserName)
            {
                result.Add(new ReturnErrorItem("UserName", $"UserName size should not exceed {UserModel.COLUMN_SIZE_UserName}"));
            }

            if (userModel.UserName == null)
            {
                result.Add(new ReturnErrorItem("UserName", "User Name is required"));
            }

            if (userModel.PredefinedUserTypeId == 0)
            {
                result.Add(new ReturnErrorItem("UserTypeId", "User Type cannot be empty"));
            }

            if (!ValidationUtils.IsEnumValid<PredefinedUserTypes, long>(userModel.PredefinedUserTypeId))
            {
                result.Add(new ReturnErrorItem("PredefinedUserTypeId", "User Type is not valid"));
            }

            if (userModel.PasswordHash != null && userModel.PasswordHash.Length > UserModel.COLUMN_SIZE_PasswordHash)
            {
                result.Add(new ReturnErrorItem("PasswordHash", $"Password Hash size should not exceed {UserModel.COLUMN_SIZE_PasswordHash}"));
            }

            if (userModel.PasswordHash == null)
            {
                result.Add(new ReturnErrorItem("PasswordHash", "Password Hash is required "));
            }

            if (userModel.PasswordChangedDate != null && !DateUtils.IsValidDate(userModel.PasswordChangedDate.Value))
            {
                result.Add(new ReturnErrorItem("PasswordChangedDate", $"Password Changed Date is not a valid datetime"));
            }

            if (userModel.Email != null && userModel.Email.Length > UserModel.COLUMN_SIZE_Email)
            {
                result.Add(new ReturnErrorItem("Email", $"Email size should not exceed {UserModel.COLUMN_SIZE_Email}"));
            }

            if (userModel.Email == null)
            {
                result.Add(new ReturnErrorItem("Email", "Email is required "));
            }

            if (!ValidationUtils.IsEmail(userModel.Email))
            {
                result.Add(new ReturnErrorItem("Email", "Email is not valid."));
            }

            if (userModel.FirstName != null && userModel.FirstName.Length > UserModel.COLUMN_SIZE_FirstName)
            {
                result.Add(new ReturnErrorItem("FirstName", $"First Name size should not exceed {UserModel.COLUMN_SIZE_FirstName}"));
            }

            if (userModel.FirstName == null)
            {
                result.Add(new ReturnErrorItem("FirstName", "First Name is required "));
            }

            if (userModel.LastName != null && userModel.LastName.Length > UserModel.COLUMN_SIZE_LastName)
            {
                result.Add(new ReturnErrorItem("LastName", $"Last Name size should not exceed {UserModel.COLUMN_SIZE_LastName}"));
            }

            if (userModel.LastName == null)
            {
                result.Add(new ReturnErrorItem("LastName", "Last Name is required "));
            }

            if (userModel.WorkPhone != null && userModel.WorkPhone.Length > UserModel.COLUMN_SIZE_WorkPhone)
            {
                result.Add(new ReturnErrorItem("WorkPhone", $"Work Phone size should not exceed {UserModel.COLUMN_SIZE_WorkPhone}"));
            }

            if (userModel.WorkPhone.IsNotNullOrEmpty() && !ValidationUtils.IsIndianPhoneNumber(userModel.WorkPhone))
            {
                result.Add(new ReturnErrorItem("WorkPhone", "WorkPhone is not valid."));
            }

            if (userModel.WorkPhoneExt != null && userModel.WorkPhoneExt.Length > UserModel.COLUMN_SIZE_WorkPhoneExt)
            {
                result.Add(new ReturnErrorItem("WorkPhoneExt", $"Work Phone Ext size should not exceed {UserModel.COLUMN_SIZE_WorkPhoneExt}"));
            }

            if (userModel.CellPhone != null && userModel.CellPhone.Length > UserModel.COLUMN_SIZE_CellPhone)
            {
                result.Add(new ReturnErrorItem("CellPhone", $"Cell Phone size should not exceed {UserModel.COLUMN_SIZE_CellPhone}"));
            }

            if (userModel.CellPhone.IsNotNullOrEmpty() && !ValidationUtils.IsIndianPhoneNumber(userModel.CellPhone))
            {
                result.Add(new ReturnErrorItem("CellPhone", "CellPhone is not valid."));
            }

            if (userModel.OneTimeToken != null && userModel.OneTimeToken.Value == Guid.Empty)
            {
                result.Add(new ReturnErrorItem("OneTimeToken", $"One Time Token is not a valid guid"));
            }

            if (userModel.OneTimeExpireDateTime != null && !DateUtils.IsValidDate(userModel.OneTimeExpireDateTime.Value))
            {
                result.Add(new ReturnErrorItem("OneTimeExpireDateTime", $"One Time Expiration Datetime is not a valid datetime"));
            }

            if (userModel.AccountLockedUntil != null && !DateUtils.IsValidDate(userModel.AccountLockedUntil.Value))
            {
                result.Add(new ReturnErrorItem("AccountLockedUntil", $"Account Locked Until Datetime is not a valid datetime"));
            }

            if (userModel.NewUserActivationGuid != null && userModel.NewUserActivationGuid.Value == Guid.Empty)
            {
                result.Add(new ReturnErrorItem("NewUserActivationGuid", $"New User Activation Code is not a valid Identifier"));
            }

            if (userModel.NewUserActivationCompletedOn != null && !DateUtils.IsValidDate(userModel.NewUserActivationCompletedOn.Value))
            {
                result.Add(new ReturnErrorItem("NewUserActivationCompletedOn", $"New User Activation Completed On is not a valid datetime"));
            }

            return result.Count > 0 ? result : null;
        }

        public Task<ReturnModel<UserProfileModel>> UpdateUserForProfileByAdminAsync(UserProfileModel user)
        {
            return UpdateUserForProfileInternalAsync(user, true);
        }

        public Task<ReturnModel<UserProfileModel>> UpdateUserForProfileByReporterAsync(UserProfileModel user)
        {
            return UpdateUserForProfileInternalAsync(user, false);
        }

        public async Task<ReturnModel<UserProfileModel>> UpdateUserForProfileInternalAsync(UserProfileModel user, bool isAdmin)
        {
            try
            {
                if (user == null)
                {
                    return log.ErrorAndReturnModel<UserProfileModel>($"Unable to update User", new ReturnErrorItem("", "User information is not valid"));
                }

                var existingUser = await companyRepository.GetUserByIdAsync(user.UserId);
                if (existingUser == null)
                {
                    return log.ErrorAndReturnModel<UserProfileModel>($"Unable to find the user for {user.UserId}");
                }

                var errorList = await IsUserProfileModelValidForAllAsync(user);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<UserProfileModel>($"Unable to update user profile", errorList);
                }

                User userItem = new User
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CellPhone = user.CellPhone,
                    WorkPhone = user.WorkPhone,
                    Email = user.Email
                };

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await companyRepository.UpdateUserForProfileAsync(userItem, await currentSessionService.GetCurrentUserIdAsync());
                    if (!result)
                    {
                        throw new RollbackException($"Unable to update user profile");
                    }

                    await trx.CompleteAsync();
                }

                await auditService.UserUpdatedAsync(existingUser.UserName);

                var view = await companyRepository.GetUserViewByIdAsync(existingUser.UserId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<UserProfileModel>($"Unable to find the user profile for {user.UserId}");
                }

                return this.ConvertToItem(view, objectMapperService.UserViewToUserProfileModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<UserProfileModel>($"Unexpected error when updating user profile.", new { user?.UserId }, ex);
            }
        }

        public Task<List<ReturnErrorItem>> IsUserProfileModelValidForAllAsync(UserProfileModel userModel)
        {
            List<ReturnErrorItem> result = new List<ReturnErrorItem>();

            if (userModel.UserName != null && userModel.UserName.Length > UserModel.COLUMN_SIZE_UserName)
            {
                result.Add(new ReturnErrorItem("UserName", $"User Name size should not exceed {UserModel.COLUMN_SIZE_UserName}"));
            }

            if (userModel.UserName == null)
            {
                result.Add(new ReturnErrorItem("UserName", "User Name is required"));
            }

            if (!ValidationUtils.IsEmail(userModel.UserName))
            {
                result.Add(new ReturnErrorItem("UserName", "User Name is not valid."));
            }

            if (userModel.Email != null && userModel.Email.Length > UserModel.COLUMN_SIZE_Email)
            {
                result.Add(new ReturnErrorItem("Email", $"Email size should not exceed {UserModel.COLUMN_SIZE_Email}"));
            }

            if (userModel.Email == null)
            {
                result.Add(new ReturnErrorItem("Email", "Email is required."));
            }

            if (!ValidationUtils.IsEmail(userModel.Email))
            {
                result.Add(new ReturnErrorItem("Email", "Email is not valid."));
            }

            if (userModel.FirstName != null && userModel.FirstName.Length > UserModel.COLUMN_SIZE_FirstName)
            {
                result.Add(new ReturnErrorItem("FirstName", $"First Name size should not exceed {UserModel.COLUMN_SIZE_FirstName}"));
            }

            if (userModel.FirstName == null)
            {
                result.Add(new ReturnErrorItem("FirstName", "First Name is required"));
            }

            if (userModel.LastName != null && userModel.LastName.Length > UserModel.COLUMN_SIZE_LastName)
            {
                result.Add(new ReturnErrorItem("LastName", $"Last Name size should not exceed {UserModel.COLUMN_SIZE_LastName}"));
            }

            if (userModel.LastName == null)
            {
                result.Add(new ReturnErrorItem("LastName", "Last Name is required "));
            }

            if (userModel.CellPhone != null && userModel.CellPhone.Length > UserModel.COLUMN_SIZE_CellPhone)
            {
                result.Add(new ReturnErrorItem("CellPhone", $"Cell Phone size should not exceed {UserModel.COLUMN_SIZE_CellPhone}"));
            }

            return Task<List<ReturnErrorItem>>.FromResult(result.Count > 0 ? result : null);
        }

        public async Task<ReturnListModel<ImportStatusModel>> ImportUserAsync(UserImportModel model, bool isTest)
        {
            try
            {
                if (model == null)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("User Info is not provided");
                }

                if (model.Users == null || model.Users.Count == 0)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("Users Info is not available to import");
                }

                var resultLst = new List<ImportStatusModel>();

                foreach (var item in model.Users)
                {
                    var data = await companyRepository.GetUserByUniqueIdentifierAsync(item.Email);
                    if (data == null)
                    {
                        //var newProd = new ProductCreateModel();
                        UserCreateModel userCreateModel = new UserCreateModel
                        {
                            IsDeleted = false
                        };

                        var newlyCreatedUser = await CreateUserAsync(userCreateModel, null);
                        if (!newlyCreatedUser.IsSuccess)
                        {
                            resultLst.Add(new ImportStatusModel() { IsSucess = false, Message = "", ErrorHolder = newlyCreatedUser.ErrorHolder });
                        }
                        else
                        {
                            resultLst.Add(new ImportStatusModel() { IsSucess = true });
                        }
                    }
                    else
                    {
                        var userUpdateModel = objectMapperService.UserCreateModelToUserUpdateModel(item);

                        //update product
                        var updatedUserModel = await UpdateUserAsync(userUpdateModel, null);
                        if (!updatedUserModel.IsSuccess)
                        {
                            resultLst.Add(new ImportStatusModel() { IsSucess = false, Message = "", ErrorHolder = updatedUserModel.ErrorHolder });
                        }
                        else
                        {
                            resultLst.Add(new ImportStatusModel() { IsSucess = true });
                        }
                    }
                }

                return ReturnListModel<ImportStatusModel>.Success(resultLst);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ImportStatusModel>("Unexpected error occurred when importing User Info", ex);
            }
        }

        public async Task<ReturnListModel<UserExportModel>> ExportUserAsync(UserSearchCriteria filter, bool isTest)
        {
            try
            {
                var userCount = await companyRepository.GetUserCountAsync(filter);
                if (userCount == 0)
                {
                    return log.ErrorAndReturnListModel<UserExportModel>("No User available for export.");
                }

                var pageSize = 100;
                var itemsToReturn = new List<UserExportModel>();
                var numberOfLoops = (userCount / pageSize) + 1;
                for (var i = 0; i < numberOfLoops; ++i)
                {
                    var dataResult = await GetUsersAsync(new UserSearchCriteria { Page = i, PageSize = pageSize, Keyword = filter.Keyword });
                    if (dataResult.IsSuccess)
                    {
                        return log.ErrorAndReturnListModel<UserExportModel>(dataResult);
                    }

                    itemsToReturn.AddRange(objectMapperService.UserModelListToUserExportModelList(dataResult.Model));
                }

                return ReturnListModel<UserExportModel>.Success(itemsToReturn);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<UserExportModel>("Unexpected error occurred when exporting User", ex);
            }
        }
        #endregion

        #region UserPermission

        public async Task<ReturnListModel<UserPermissionModel>> GetUserPermissionsAsync(UserPermissionSearchCriteria search)
        {
            try
            {
                return this.ConvertToList(await companyRepository.GetUserPermissionsAsync(search), objectMapperService.UserPermissionViewListToUserPermissionModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<UserPermissionModel>($"Unexpected error when getting UserPermissions", ex);
            }
        }

        public Task<ReturnModel<UserPermissionModel>> GetUserPermissionByIdForReadAsync(long userPermissionId)
        {
            return GetUserPermissionByIdInternalAsync(forWrite: false, userPermissionId);
        }

        public Task<ReturnModel<UserPermissionModel>> GetUserPermissionByIdForWriteAsync(long userPermissionId)
        {
            return GetUserPermissionByIdInternalAsync(forWrite: true, userPermissionId);
        }

        public async Task<ReturnModel<UserPermissionModel>> GetUserPermissionByIdInternalAsync(bool forWrite, long userPermissionId)
        {
            try
            {
                var item = await companyRepository.GetUserPermissionViewByIdAsync(userPermissionId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<UserPermissionModel>($"Unable to find the userPermission", new { userPermissionId });
                }

                //var existingMovieModel = forWrite ? await GetMovieByIdForWriteAsync(item.MovieId) : await GetMovieByIdForReadAsync(item.MovieId);
                //if (!existingMovieModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<CompanyModel>(existingMovieModel);
                //}

                return this.ConvertToItem(item, objectMapperService.UserPermissionViewToUserPermissionModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<UserPermissionModel>($"Unexpected error when getting userPermission", new { userPermissionId }, ex);
            }
        }

        public async Task<ReturnModel<UserPermissionModel>> CreateUserPermissionAsync(UserPermissionCreateModel userPermission)
        {
            try
            {
                if (userPermission == null)
                {
                    return log.ErrorAndReturnModel<UserPermissionModel>($"Unable to create UserPermission", new ReturnErrorItem("", "UserPermission information is not valid"));
                }

                //var companyModel = await GetCompanyForReadById(userPermission.CompanyId);
                //if (!companyModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<UserPermissionModel>(companyModel);
                //}
                //
                //var movieModel = await GetMovieForReadById(userPermission.MovieId);
                //if (!movieModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<UserPermissionModel>(movieModel);
                //}

                var errorList = await IsUserPermissionModelValidForCreateAsync(userPermission);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<UserPermissionModel>($"Unable to create userPermission", errorList);
                }

                userPermission.IsDeleted = false;

                var item = objectMapperService.UserPermissionCreateModelToUserPermission(userPermission);

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await companyRepository.CreateUserPermissionAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create userPermission");
                    }

                    await trx.CompleteAsync();
                }

                await auditService.UserPermissionCreatedAsync(item.UserPermissionId);

                var view = await companyRepository.GetUserPermissionViewByIdAsync(item.UserPermissionId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<UserPermissionModel>($"Unable to find the userPermission", new { item.UserPermissionId });
                }

                return this.ConvertToItem(view, objectMapperService.UserPermissionViewToUserPermissionModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<UserPermissionModel>($"Unexpected error when creating UserPermission", new { userPermission?.UserPermissionId }, ex);
            }
        }

        public async Task<ReturnModel<UserPermissionModel>> UpdateUserPermissionAsync(UserPermissionUpdateModel userPermission)
        {
            try
            {
                if (userPermission == null)
                {
                    return log.ErrorAndReturnModel<UserPermissionModel>($"Unable to update UserPermission", new ReturnErrorItem("", "UserPermission information is not valid"));
                }

                var existingUserPermissionModel = await GetUserPermissionByIdForWriteAsync(userPermission.UserPermissionId);
                if (!existingUserPermissionModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<UserPermissionModel>(existingUserPermissionModel);
                }

                userPermission.CreatedByUserId = existingUserPermissionModel.Model.CreatedByUserId;
                userPermission.CreatedDate = existingUserPermissionModel.Model.CreatedDate;
                userPermission.IsDeleted = existingUserPermissionModel.Model.IsDeleted;
                //userPermission.CompanyId = existingUserPermissionModel.Model.CompanyId;

                var errorList = await IsUserPermissionModelValidForUpdateAsync(userPermission);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<UserPermissionModel>($"Unable to update userPermission", errorList);
                }

                var item = objectMapperService.UserPermissionUpdateModelToUserPermission(userPermission);

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await companyRepository.UpdateUserPermissionAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update userPermission");
                    }

                    await trx.CompleteAsync();
                }

                await auditService.UserPermissionUpdatedAsync(userPermission.UserPermissionId, existingUserPermissionModel.Model, item);

                var view = await companyRepository.GetUserPermissionViewByIdAsync(userPermission.UserPermissionId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<UserPermissionModel>($"Unable to find the userPermission", new { userPermission.UserPermissionId });
                }

                return this.ConvertToItem(view, objectMapperService.UserPermissionViewToUserPermissionModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<UserPermissionModel>($"Unexpected error when updating UserPermission", new { userPermission?.UserPermissionId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkUserPermissionAsNotDeletedByIdAsync(long userPermissionId)
        {
            try
            {
                var existingUserPermissionModel = await GetUserPermissionByIdForWriteAsync(userPermissionId);
                if (!existingUserPermissionModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingUserPermissionModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await companyRepository.MarkUserPermissionAsNotDeletedByIdAsync(userPermissionId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.UserPermissionUnDeletedAsync(userPermissionId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking userPermission as not deleted", new { userPermissionId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkUserPermissionAsDeletedByIdAsync(long userPermissionId)
        {
            try
            {
                var existingUserPermissionModel = await GetUserPermissionByIdForWriteAsync(userPermissionId);
                if (!existingUserPermissionModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingUserPermissionModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await companyRepository.MarkUserPermissionAsDeletedByIdAsync(userPermissionId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.UserPermissionDeletedAsync(userPermissionId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking userPermission as deleted", new { userPermissionId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteUserPermissionByAsync(long userPermissionId)
        {
            try
            {
                var existingUserPermissionModel = await GetUserPermissionByIdForWriteAsync(userPermissionId);
                if (!existingUserPermissionModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingUserPermissionModel);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await companyRepository.HardDeleteUserPermissionByIdAsync(userPermissionId);
                    await trx.CompleteAsync();
                }

                await auditService.HardDeleteUserPermissionAsync(userPermissionId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting userPermission", new { userPermissionId }, ex);
            }
        }

        public Task<List<ReturnErrorItem>> IsUserPermissionModelValidForCreateAsync(UserPermissionUpdateModel userPermissionModel)
        {
            return IsUserPermissionModelValidForAllAsync(true, userPermissionModel);
        }

        public Task<List<ReturnErrorItem>> IsUserPermissionModelValidForUpdateAsync(UserPermissionUpdateModel userPermissionModel)
        {
            return IsUserPermissionModelValidForAllAsync(false, userPermissionModel);
        }

        public async Task<List<ReturnErrorItem>> IsUserPermissionModelValidForAllAsync(bool isCreate, UserPermissionUpdateModel userPermissionModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (await companyRepository.IsUserPermissionNameAlreadyExistsForOtherUserPermissionAsync(userPermissionModel.UserId, userPermissionModel.PredefinedUserPermissionTypeId))
                {
                    result.Add(new ReturnErrorItem("", "Another UserPermission already exists"));
                }

                if (userPermissionModel.UserPermissionId > 0)
                {
                    result.Add(new ReturnErrorItem("", "UserPermission Identifier is not valid"));
                }
            }
            else
            {
                if (await companyRepository.IsUserPermissionNameAlreadyExistsForOtherUserPermissionAsync(userPermissionModel.UserPermissionId, userPermissionModel.UserId, userPermissionModel.PredefinedUserPermissionTypeId))
                {
                    result.Add(new ReturnErrorItem("", "Another UserPermission already exists"));
                }

                if (userPermissionModel.UserPermissionId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "UserPermission Unique Identifier is not valid"));
                }
            }

            var userItem = await companyRepository.GetUserViewByIdAsync(userPermissionModel.UserId);
            if (userItem == null)
            {
                result.Add(new ReturnErrorItem("", $"Unable to Get the User Information"));
                return result;
            }

            if (!ValidationUtils.IsLongValid(userPermissionModel.UserId))
            {
                result.Add(new ReturnErrorItem("UserId", "User cannot be empty"));
            }

            if (userPermissionModel.PredefinedUserPermissionTypeId == 0)
            {
                result.Add(new ReturnErrorItem("PredefinedUserPermissionTypeId", "PredefinedUserPermissionTypeId cannot be empty"));
            }

            if (!ValidationUtils.IsEnumValid<PredefinedUserPermissionTypes, long>(userPermissionModel.PredefinedUserPermissionTypeId))
            {
                result.Add(new ReturnErrorItem("PredefinedUserPermissionTypeId", "User Permission Type is not valid"));
            }

            return result.Count > 0 ? result : null;
        }

        /*
        public async Task<ReturnModel<bool>> IsUserPermissionWordOrPdfFileValidAsync(long userPermissionId, bool isPdf)
        {
            var tName = isPdf ? "preview" : "source";

            try
            {
                var fileModel = await GetUserPermissionWordOrPdfFileAsync(userPermissionId,isPdf);
                if (!fileModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the {tName} file", fileModel.ErrorHolder);
                }

                FileUtils.CloseSafely(fileModel.Model.FileData);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the {tName} file", ex);
            }
        }

        public async Task<ReturnModel<FileStreamModel>> GetUserPermissionWordOrdPdfFileAsync(long userPermissionId, bool isPdf)
        {
            try
            {
                var view = await GetUserPermissionByIdForReadAsync(userPermissionId);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the UserPermission", new { userPermissionId });
                }

                var fileName = view.Model.SourceStoredFilePath;
                var docType = "Word";
                var docExt = ".docx";

                if (isPdf)
                {
                    fileName = view.Model.PreviewStoredFilePath;
                    docType = "Pdf";
                    docExt = ".pdf";
                }

                if (fileName.IsTrimmedStringNullOrEmpty())
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: {docType} file is not available.");
                }

                if (!await contractFileStorage.ExistsAsync(view.Model.TenantId, fileName))
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: {docType} file is not available.");
                }

                var result = new FileStreamModel
                {
                    FileName = view.Model.ActualFileName.IsTrimmedStringNullOrEmpty() ? $"UserPermissionFile.{docType}" : view.Model.ActualFileName,
                    ContentType = FileUtils.GetMimeTypeFromFileName(view.Model.ActualFileName),
                    FileData = await wipFileStorage.OpenReadAsync(view.Model.TenantId, fileName)
                };

                return ReturnModel<FileStreamModel>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for UserPermission ", new { userPermissionId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> IsUserPermissionFileValidAsync(long userPermissionId)
        {
            try
            {
                var fileModel = await GetUserPermissionFileAsync(userPermissionId);
                if (!fileModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the UserPermission file", fileModel.ErrorHolder);
                }

                FileUtils.CloseSafely(fileModel.Model.FileData);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the UserPermission file", ex);
            }
        }

        public async Task<ReturnModel<FileStreamModel>> GetUserPermissionFileAsync(long userPermissionId)
        {
            try
            {
                var view = await GetUserPermissionByIdForReadAsync(userPermissionId);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the UserPermission", new { userPermissionId });
                }

                var fileName = view.Model.SourceStoredFilePath;

                if (fileName.IsTrimmedStringNullOrEmpty())
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: file is not available.");
                }

                if (!await wipFileStorage.ExistsAsync(view.Model.TenantId, fileName))
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: file is not available.");
                }

                var actualFileName = view.Model.ActualFileName.IsTrimmedStringNullOrEmpty() ? $"UserPermissionFile" : view.Model.ActualFileName;

                var result = new FileStreamModel
                {
                    FileName = actualFileName,
                    ContentType = FileUtils.GetMimeTypeFromFileName(actualFileName),
                    FileData = await wipFileStorage.OpenReadAsync(view.Model.TenantId, fileName)
                };

                return ReturnModel<FileStreamModel>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for UserPermission ", new { userPermissionId }, ex);
            }
        }

        public async Task CreateOrUpdateUserPermissionDependentsAsync(UserPermissionModel model)
        {
            if (model.NewXYZZYXs == null)
                model.NewXYZZYXs = new List<UserPermissionXYZZYXCreateModel>();

            if (model.ExistingXYZZYXs == null)
                model.ExistingXYZZYXs = new List<UserPermissionXYZZYXModel>();

            var dbExistingXYZZYX = await companyRepository.GetUserPermissionXYZZYXsByUserPermissionIdAsync(item.UserPermissionId);
            if (dbExistingXYZZYX.Result != null && dbExistingUserPermission.TotalRows > 0)
            {
                var dbExistingUserPermissionIds = dbExistingUserPermission.Result.SelectToList(x => x.UserPermissionXYZZYXId);

                var xYZZYXItemsToDelete = model.ExistingXYZZYXs.Where(x => !dbExistingUserPermissionIds.Contains(x.UserPermissionXYZZYXId));
                foreach (var x in xYZZYXItemsToDelete)
                {
                    await companyRepository.HardDeleteUserPermissionXYZZYXByIdAsync(x.UserPermissionXYZZYXId);
                }

                var xYZZYXItemsToUpdate = model.ExistingXYZZYXs.Where(x => dbExistingUserPermissionIds.Contains(x.UserPermissionXYZZYXId));
                foreach (var x in xYZZYXItemsToUpdate)
                {
                    x.UserPermissionId = item.UserPermissionId;
                    var mappedItem = objectMapperService.UserPermissionXYZZYXModelToUserPermissionXYZZYXUpdateModel(x);
                    var result = await UpdateUserPermissionXYZZYXAsync(mappedItem);
                    if (!result.IsSuccess)
                    {
                        throw new RollbackException(result);
                    }
                }
            }

            foreach (var x in item.NewXYZZYXs)
            {
                x.UserPermissionId = item.UserPermissionId;
                var result = await CreateUserPermissionXYZZYXAsync(x);
                if (!result.IsSuccess)
                {
                    throw new RollbackException(result);
                }
            }
        }

        public async Task<ReturnModel<bool>> EnableUserPermissionByIdAsync(long userPermissionId)
        {
            try
            {
                var existingUserPermissionModel = await GetUserPermissionByIdForWriteAsync(userPermissionId);
                if (!existingUserPermissionModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<UserPermissionModel>(existingUserPermissionModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await companyRepository.EnableUserPermissionByIdAsync(existingUserPermissionModel.Model.UserPermissionId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.UserPermissionEnabledAsync(userPermissionId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking userPermission as deleted", new { userPermissionId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> DisableUserPermissionByIdAsync(long userPermissionId)
        {
            try
            {
                var existingUserPermissionModel = await GetUserPermissionByIdForWriteAsync(userPermissionId);
                if (!existingUserPermissionModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<UserPermissionModel>(existingUserPermissionModel);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await companyRepository.DisableUserPermissionByIdAsync(existingUserPermissionModel.Model.UserPermissionId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.UserPermissionDisabledAsync(existingUserPermissionModel.Model.UserPermissionId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting userPermission",new { userPermissionId }, ex);
            }
        }


        public async Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByUserPermissionAsync(long userPermissionId)
        {
            try
            {
                var contract = await companyRepository.GetUserPermissionByIdAsync(userPermissionId);
                if (contract == null)
                {
                    return log.ErrorAndReturnListModel<UserPermissionModel>($"Unable to find UserPermission", new { userPermissionId });
                }
                return this.ConvertToList(await companyRepository.GetXYZZYXsByUserPermissionsAsync(contract.UserPermissionId), objectMapperService.UserPermissionViewListToUserPermissionModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<UserPermissionModel>($"Unexpected error when getting Related UserPermissions of the UserPermission");
            }
        }

        public async Task<ReturnModel<bool>> AssociateXYZZYXWithUserPermissionAsync(UserPermissionXYZZYXAssociationModel model)
        {
            try
            {
                var errorLst = await IsUserPermissionWithUserPermissionAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("UserPermission association is invalid", errorLst);
                }

                if (await companyRepository.DoesUserPermissionWithXYZZYXAssociationExistsAsync(model.UserPermissionId, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related UserPermission is already associated with the UserPermission", new { model.UserPermissionId, model.XYZZYXId});
                }

                var rValue = await companyRepository.AssociateUserPermissionWithXYZZYXAsync(model.UserPermissionId, model.XYZZYXId, await currentSessionService.GetCurrentUserIdAsync());
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }
                return log.ErrorAndReturnModel<bool>("Unable to associate UserPermission with XYZZYX", new { model.UserPermissionId, model.XYZZYXId});
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when associating UserPermission with XYZZYX", new { model.UserPermissionId, model.XYZZYXId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> UnAssociateXYZZYXWithUserPermissionAsync(UserPermissionXYZZYXAssociationModel model)
        {
            try
            {
                var errorLst = await IsUserPermissionWithXYZZYXAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("UserPermission association is not valid", errorLst);
                }

                if (!await companyRepository.DoesUserPermissionWithXYZZYXAssociationExistsAsync(model.UserPermissionId, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related UserPermission is not associated with the UserPermission", new { model.UserPermissionId, model.XYZZYXId });
                }

                var rValue = await companyRepository.UnAssociateUserPermissionWithUserPermissionAsync(model.UserPermissionId, model.XYZZYXId);
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }

                return log.ErrorAndReturnModel<bool>("Unable to unassociate Related UserPermission with the UserPermission", new { model.UserPermissionId, model.XYZZYXId });
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when unassociating Related UserPermission with the UserPermission", new { model.UserPermissionId, model.XYZZYXId }, ex);
            }
        }

        public async Task<List<ReturnErrorItem>> IsUserPermissionWithXYZZYXAssociationValidAsync(UserPermissionXYZZYXAssociationModel model)
        {
            var result = new List<ReturnErrorItem>();
            if (model == null)
            {
                result.Add(new ReturnErrorItem("", "Association is not valid"));
                return result;
            }

            if (model.UserPermissionId == 0)
            {
                result.Add(new ReturnErrorItem("", "UserPermission is not valid"));
                return result;
            }

            if (model.XYZZYXId == 0)
            {
                result.Add(new ReturnErrorItem("", "Related UserPermission  is not valid"));
                return result;
            }

            var entity = await GetUserPermissionForWriteByIdAsync(model.UserPermissionId);
            if(!entity.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "UserPermission information is not avialable"));
                return result;
            }

            var xYZZYX = await GetXYZZYXForWriteByIdAsync(model.XYZZYXId);
            if (!xYZZYX.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "Related XYZZYX information is not avialable"));
                return result;
            }

            if (entity.CompanyId != xYZZYX.CompanyId)
            {
                result.Add(new ReturnErrorItem("", "UserPermissions does not belong to the same company"));
            }

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }

        public async Task<ReturnListModel<ImportStatusModel>> ImportUserPermissionAsync(UserPermissionImportModel model, bool isTest)
        {
            try
            {
                if (model == null)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("UserPermission Info is not provided");
                }

                if (model.UserPermissions == null || model.UserPermissions.Count == 0)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("UserPermissions Info is not available to import");
                }

                var resultLst = new List<ImportStatusModel>();

                foreach (var item in model.UserPermissions)
                {
                    var prod = await wipService.GetUserPermissionByExternalIdentifierAsync(item.UserPermissionIdentifier);
                    if (prod.Model == null)
                    {
                        //var newProd = new ProductCreateModel();
                        UserPermissionCreateModel userPermissionCreateModel = new UserPermissionCreateModel
                        {
                            IsEnabled = item.IsProductEnabled
                        };

                        var newlyCreatedUserPermission = await CreateUserPermissionAsync(userPermissionCreateModel);
                        if (!newlyCreatedUserPermission.IsSuccess)
                        {
                            var errorStr = string.Empty;
                            if (newlyCreatedUserPermission.ErrorHolder.ErrorItemList != null && newlyCreatedProd.ErrorHolder.ErrorItemList.Count > 0)
                            {
                                var errorValueList = newlyCreatedUserPermission.ErrorHolder.ErrorItemList.SelectToList(x => x.Value);
                                errorStr = string.Join(",", errorValueList);
                            }

                            resultLst.Add(new ImportStatusModel() { IsSucess = false, Message = errorStr.IsNotNullOrEmpty() ? errorStr : newlyCreatedProd.ErrorHolder.FriendlyMessage });
                            continue;
                        }
                        else
                        {
                            resultLst.Add(new ImportStatusModel() { IsSucess = true });
                        }
                    }
                    else
                    {
                        UserPermissionUpdateModel userPermissionUpdateModel = new UserPermissionUpdateModel
                        {
                        };

                        //update product
                        var updatedUserPermissionModel = await wipService.UpdateUserPermissionAsync(userPermissionUpdateModel);
                        if (!updatedUserPermissionModel.IsSuccess)
                        {
                            var errorStr = string.Empty;
                            if (updateProdModel.ErrorHolder.ErrorItemList != null && updateProdModel.ErrorHolder.ErrorItemList.Count > 0)
                            {
                                var errorValueList = updateProdModel.ErrorHolder.ErrorItemList.SelectToList(x => x.Value);
                                errorStr = string.Join(",", errorValueList);
                            }

                            resultLst.Add(new ImportStatusModel() { IsSucess = false, Message = errorStr.IsNotNullOrEmpty() ? errorStr : updateProdModel.ErrorHolder.FriendlyMessage });
                            continue;
                        }
                        else
                        {
                            resultLst.Add(new ImportStatusModel() { IsSucess = true });
                        }
                    }
                }

                return ReturnListModel<ImportStatusModel>.Success(resultLst);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ImportStatusModel>("Unexpected error occured when importing UserPermission Info", ex);
            }
        }

        public async Task<ReturnListModel<UserPermissionExportModel>> ExportUserPermissionAsync(UserPermissionExportParameter filter, bool isTest)
        {
            try
            {
                var userPermissionCount = await companyRepository.GetUserPermissionCountAsync(filter);
                if (userPermissionCount == 0)
                {
                    return log.ErrorAndReturnListModel<UserPermissionExportModel>("No UserPermission available for export.");
                }

                var itemsToReturn = new List<UserPermissionExportModel>();
                var numberOfLoops = (userPermissionCount / pageSize) + 1;
                for (var i = 0; i < numberOfLoops; ++i)
                {
                    var dataResult = await wipService.GetUserViewListAsync(new UserPermissionSearchCriteria { Page = i, PageSize = pageSize, Keyword = filter.Keyword });
                    if (dataResult.IsSuccess)
                    {
                        return log.ErrorAndReturnListModel<UserPermissionExportModel>(dataResult);
                    }

                    itemsToReturn.AddRange(objectMapperService.MapList<UserPermissionModel, UserPermissionExportModel>(dataResult.Model));
                }

                return ReturnListModel<UserPermissionExportModel>.Success(itemsToReturn);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<UserPermissionExportModel>("Unexpected error occured when exporting UserPermission", ex);
            }
        }

        */
        #endregion

        #region UserGroup

        public async Task<ReturnListModel<UserGroupModel>> GetUserGroupsAsync(UserGroupSearchCriteria search)
        {
            try
            {
                return this.ConvertToList(await wipRepository.GetUserGroupsAsync(search), objectMapperService.UserGroupViewListToUserGroupModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<UserGroupModel>($"Unexpected error when getting User Groups", ex);
            }
        }

        public Task<ReturnModel<UserGroupModel>> GetUserGroupByIdForReadAsync(long userGroupId)
        {
            return GetUserGroupByIdInternalAsync(forWrite: false, userGroupId);
        }

        public Task<ReturnModel<UserGroupModel>> GetUserGroupByIdForWriteAsync(long userGroupId)
        {
            return GetUserGroupByIdInternalAsync(forWrite: true, userGroupId);
        }

        public async Task<ReturnModel<UserGroupModel>> GetUserGroupByIdInternalAsync(bool forWrite, long userGroupId)
        {
            try
            {
                var item = await wipRepository.GetUserGroupViewByIdAsync(userGroupId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<UserGroupModel>($"Unable to find the User Group", new { userGroupId });
                }

                //var existingMovieModel = forWrite ? await GetMovieByIdForWriteAsync(item.MovieId) : await GetMovieByIdForReadAsync(item.MovieId);
                //if (!existingMovieModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<CompanyModel>(existingMovieModel);
                //}

                //var result = this.ConvertToItem(item,objectMapperService.UserGroupViewToUserGroupModel);
                //var childItems = await GetUserGroupXYZZYXsAsync(new UserGroupXYZZYXSearchCriteria { UserGroupId = item.UserGroupId, Page = 1, PageSize = int.MaxValue });
                //result.Model.ExistingUserGroupXYZZYXs = childItems.Model.Where(x => x.UserGroupId == item.UserGroupId).ToList();
                //return result;

                return this.ConvertToItem(item, objectMapperService.UserGroupViewToUserGroupModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<UserGroupModel>($"Unexpected error when getting User Group", new { userGroupId }, ex);
            }
        }

        public async Task<ReturnListModel<UserGroupModel>> CreateMultipleUserGroupAsync(MultipleUserGroupCreateModel userGroups)
        {
            try
            {
                if (userGroups == null)
                {
                    return log.ErrorAndReturnListModel<UserGroupModel>($"Unable to Create User Groups", new ReturnErrorItem("", "User Groups information is not valid"));
                }

                if (userGroups.NewUserGroups.IsNullOrEmpty())
                {
                    return log.ErrorAndReturnListModel<UserGroupModel>($"Unable to Create User Groups", new ReturnErrorItem("", "User Groups information is not provided"));
                }

                var lst = new List<UserGroupModel>();

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    foreach (var userGroup in userGroups.NewUserGroups)
                    {
                        var cResult = await CreateUserGroupAsync(userGroup);
                        if (!cResult.IsSuccess)
                            throw new RollbackException(cResult);

                        lst.Add(cResult.Model);
                    }

                    await trx.CompleteAsync();
                }

                return ReturnListModel<UserGroupModel>.Success(lst);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModelWithRollbackExceptionHandler<UserGroupModel>($"Unexpected error when creating User Groups", new { userGroups }, ex);
            }
        }

        public async Task<ReturnModel<UserGroupModel>> CreateUserGroupAsync(UserGroupCreateModel userGroup)
        {
            try
            {
                if (userGroup == null)
                {
                    return log.ErrorAndReturnModel<UserGroupModel>($"Unable to create User Group", new ReturnErrorItem("", "User Group information is not valid"));
                }

                //var companyModel = await companyService.GetCompanyByIdForReadAsync(userGroup.CompanyId);
                //if (!companyModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<UserGroupModel>(companyModel);
                //}
                //
                //var movieModel = await GetMovieByIdForReadAsync(userGroup.MovieId);
                //if (!movieModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<UserGroupModel>(movieModel);
                //}
                //

                /*var ignoreCode = false;
                if (userGroup.UserGroupCode.IsNullOrEmpty())
                {
                    ignoreCode = true;
                    userGroup.UserGroupCode = StringConstants.PrefixForCode.UserGroup + Guid.NewGuid().ToString();
                }
                */

                var errorList = await IsUserGroupModelValidForCreateAsync(userGroup);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<UserGroupModel>($"Unable to create User Group", errorList);
                }

                userGroup.IsDeleted = false;

                var item = objectMapperService.UserGroupCreateModelToUserGroup(userGroup);

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await wipRepository.CreateUserGroupAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create User Group");
                    }

                    //await CreateOrUpdateUserGroupDependentsAsync(userGroup)

                    /*if (ignoreCode)
                    {
                        if (!await wipRepository.UpdateUserGroupCodeAsync(item.CompanyId, item.UserGroupId, StringConstants.PrefixForCode.UserGroup + item.UserGroupId, await currentSessionService.GetCurrentUserIdAsync()))
                        {
                            throw new RollbackException($"Unable update the User Group Code", item);
                        }
                    }*/

                    await trx.CompleteAsync();
                }

                await auditService.UserGroupCreatedAsync(item.UserGroupId);

                var view = await wipRepository.GetUserGroupViewByIdAsync(item.UserGroupId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<UserGroupModel>($"Unable to find the User Group", new { item.UserGroupId });
                }

                return this.ConvertToItem(view, objectMapperService.UserGroupViewToUserGroupModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<UserGroupModel>($"Unexpected error when creating User Group", new { userGroup?.UserGroupId }, ex);
            }
        }

        public async Task<ReturnModel<UserGroupModel>> UpdateUserGroupAsync(UserGroupUpdateModel userGroup)
        {
            try
            {
                if (userGroup == null)
                {
                    return log.ErrorAndReturnModel<UserGroupModel>($"Unable to update User Group", new ReturnErrorItem("", "User Group information is not valid"));
                }

                var existingUserGroupModel = await GetUserGroupByIdForWriteAsync(userGroup.UserGroupId);
                if (!existingUserGroupModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<UserGroupModel>(existingUserGroupModel);
                }

                userGroup.CreatedByUserId = existingUserGroupModel.Model.CreatedByUserId;
                userGroup.CreatedDate = existingUserGroupModel.Model.CreatedDate;
                userGroup.IsDeleted = existingUserGroupModel.Model.IsDeleted;
                //userGroup.MovieId = existingUserGroupModel.Model.MovieId;
                //userGroup.ProfileImageActualFileName = existingUserGroupModel.Model.ProfileImageActualFileName;
                //userGroup.ProfileImageLocation = existingUserGroupModel.Model.ProfileImageLocation;

                var errorList = await IsUserGroupModelValidForUpdateAsync(userGroup);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<UserGroupModel>($"Unable to update User Group", errorList);
                }

                var item = objectMapperService.UserGroupUpdateModelToUserGroup(userGroup);

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await wipRepository.UpdateUserGroupAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update User Group");
                    }

                    //await CreateOrUpdateUserGroupDependentsAsync(userGroup)

                    await trx.CompleteAsync();
                }

                await auditService.UserGroupUpdatedAsync(userGroup.UserGroupId);

                var view = await wipRepository.GetUserGroupViewByIdAsync(userGroup.UserGroupId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<UserGroupModel>($"Unable to find the User Group", new { userGroup.UserGroupId });
                }

                return this.ConvertToItem(view, objectMapperService.UserGroupViewToUserGroupModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<UserGroupModel>($"Unexpected error when updating User Group", new { userGroup?.UserGroupId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkUserGroupAsNotDeletedByIdAsync(long userGroupId)
        {
            try
            {
                var existingUserGroupModel = await GetUserGroupByIdForWriteAsync(userGroupId);
                if (!existingUserGroupModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingUserGroupModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await wipRepository.MarkUserGroupAsNotDeletedByIdAsync(userGroupId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.UserGroupDeletedAsync(userGroupId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking User Group as not deleted", new { userGroupId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkUserGroupAsDeletedByIdAsync(long userGroupId)
        {
            try
            {
                var existingUserGroupModel = await GetUserGroupByIdForWriteAsync(userGroupId);
                if (!existingUserGroupModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingUserGroupModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await wipRepository.MarkUserGroupAsDeletedByIdAsync(userGroupId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.UserGroupDeletedAsync(userGroupId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking User Group as deleted", new { userGroupId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteUserGroupByAsync(long userGroupId)
        {
            try
            {
                var existingUserGroupModel = await GetUserGroupByIdForWriteAsync(userGroupId);
                if (!existingUserGroupModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingUserGroupModel);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await wipRepository.HardDeleteUserGroupByIdAsync(userGroupId);
                    await trx.CompleteAsync();
                }

                await auditService.HardDeleteUserGroupAsync(userGroupId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting User Group", new { userGroupId }, ex);
            }
        }

        public Task<List<ReturnErrorItem>> IsUserGroupModelValidForCreateAsync(UserGroupUpdateModel userGroupModel)
        {
            return IsUserGroupModelValidForAllAsync(true, userGroupModel);
        }

        public Task<List<ReturnErrorItem>> IsUserGroupModelValidForUpdateAsync(UserGroupUpdateModel userGroupModel)
        {
            return IsUserGroupModelValidForAllAsync(false, userGroupModel);
        }

        public async Task<List<ReturnErrorItem>> IsUserGroupModelValidForAllAsync(bool isCreate, UserGroupUpdateModel userGroupModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (userGroupModel.UserGroupId > 0)
                {
                    result.Add(new ReturnErrorItem("", "User Group Identifier is not valid"));
                }
            }
            else
            {
                if (userGroupModel.UserGroupId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "User Group Unique Identifier is not valid"));
                }
            }

            await currentSessionService.GetCurrentUserIdAsync();

            if (userGroupModel.UserGroupName != null && userGroupModel.UserGroupName.Length > UserGroupModel.COLUMN_SIZE_UserGroupName)
            {
                result.Add(new ReturnErrorItem("UserGroupName", $"UserGroupName size should not exceed {UserGroupModel.COLUMN_SIZE_UserGroupName}"));
            }

            if (userGroupModel.UserGroupName == null)
            {
                result.Add(new ReturnErrorItem("UserGroupName", "UserGroupName is required "));
            }

            if (!ValidationUtils.IsLongValid(userGroupModel.CompanyEntityId))
            {
                result.Add(new ReturnErrorItem("CompanyEntityId", "CompanyEntity cannot be empty"));
            }

            return result.Count > 0 ? result : null;
        }
        #endregion
    }
}
