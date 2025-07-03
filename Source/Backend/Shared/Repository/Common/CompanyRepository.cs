using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Repository.DbAccess;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.DBView;
using Manifest.Common.Models.SearchCriteria;
using Framework.Data.Utils;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using LinqToDB;

namespace Manifest.Repository.Common
{
    public class CompanyRepository : BaseRepository
    {
        public CompanyRepository(IAppDBManager appDBManager)
            : base(appDBManager)
        {
        }

        #region User Old

        public Task<UserView> GetUserViewByUsernameAsync(string userName)
        {
            return GetUsersViewAsIQueryable().FirstOrDefaultAsync(x => x.UserName.Trim().ToUpper() == (userName ?? "").Trim().ToUpper());
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return appDBManager.FirstOrDefaultAsync<User>(x => x.Email.Trim().ToUpper() == (email ?? "").Trim().ToUpper());
        }

        public Task<User> GetUserByUsernameAsync(string userName)
        {
            return appDBManager.FirstOrDefaultAsync<User>(x => x.UserName.Trim().ToUpper() == (userName ?? "").Trim().ToUpper());
        }

        public Task<User> GetUserByEmailOrUserNameAsync(string emailOrUserName)
        {
            return appDBManager.FirstOrDefaultAsync<User>(x => x.Email.Trim().ToUpper() == (emailOrUserName ?? "").Trim().ToUpper() || x.UserName.Trim().ToUpper() == (emailOrUserName ?? "").Trim().ToUpper());
        }

        public Task<User> GetUserByPasswordResetCodeAsync(Guid passwordResetCode)
        {
            return appDBManager.FirstOrDefaultAsync<User>(x => x.OneTimeToken == passwordResetCode);
        }

        public Task<List<User>> GetUserByEmailListAsync(IEnumerable<string> emails)
        {
            emails = emails == null ? new List<string>() : emails.SelectToList(x => x.Trim().ToUpper());
            return appDBManager.SelectAsync<User>(x => emails.Contains(x.Email.ToUpper().Trim()));
        }

        public Task<User> GetUserByLoginAsync(string userLogin)
        {
            return appDBManager.FirstOrDefaultAsync<User>(x => x.UserName == userLogin);
        }

        public async Task<bool> DeleteUserByIdAsync(long id)
        {
            return await appDBManager.GetTable<User>().DeleteAsync(x => x.UserId == id) > 0;
        }

        public async Task UpdatePasswordCodeAsync(long userId, Guid? resetCode, DateTime? codeValidUntil, long? modifiedBy)
        {
            _ = await appDBManager.GetTable<User>().Where(x => x.UserId == userId).Set(x => x.OneTimeToken, resetCode).Set(x => x.OneTimeExpireDateTime, codeValidUntil).UpdateWithAuditAsync(modifiedBy);
        }

        public async Task UpdatePasswordAsync(long userId, string passwordHash, long? modifiedBy)
        {
            _ = await appDBManager.GetTable<User>()
                .Where(x => x.UserId == userId)
                .Set(x => x.OneTimeToken, (Guid?)null)
                .Set(x => x.OneTimeExpireDateTime, (DateTime?)null)
                .Set(x => x.PasswordHash, passwordHash)
                .Set(x => x.PasswordChangedDate, DateTime.UtcNow)
                .UpdateWithAuditAsync(modifiedBy);
        }
        #endregion User Old

        #region User
        public async Task<bool> UpdateUserPicturePathAsync(long userId, string path, string actualFileName, long? modifiedBy, Guid? profileGuid)
        {
            return await appDBManager.GetTable<User>().Where(x => x.UserId == userId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).Set(x => x.ProfileImageGuid, profileGuid).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearUserProfilePathAsync(long userId, long? modifiedBy)
        {
            return await appDBManager.GetTable<User>().Where(x => x.UserId == userId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).Set(x => x.ProfileImageGuid, (Guid?)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public IQueryable<UserView> GetUsersViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<User>()
                   join a in GetEnumQueryableOfPredefinedUserType() on item.PredefinedUserTypeId equals a.EnumTypeId
                   select new UserView
                   {
                       PredefinedUserTypeName = a.EnumTypeName,
                       UserId = item.UserId,
                       UserGuid = item.UserGuid,
                       UserName = item.UserName,
                       PredefinedUserTypeId = item.PredefinedUserTypeId,
                       PasswordHash = item.PasswordHash,
                       PasswordChangedDate = item.PasswordChangedDate,
                       Email = item.Email,
                       FirstName = item.FirstName,
                       LastName = item.LastName,
                       WorkPhone = item.WorkPhone,
                       WorkPhoneExt = item.WorkPhoneExt,
                       CellPhone = item.CellPhone,
                       Active = item.Active,
                       NotificationThroughEmail = item.NotificationThroughEmail,
                       OneTimeToken = item.OneTimeToken,
                       OneTimeExpireDateTime = item.OneTimeExpireDateTime,
                       LoginFailedAttempt = item.LoginFailedAttempt,
                       IsAccountLocked = item.IsAccountLocked,
                       AccountLockedUntil = item.AccountLockedUntil,
                       NewUserActivationGuid = item.NewUserActivationGuid,
                       NewUserActivationCompletedOn = item.NewUserActivationCompletedOn,
                       ProfileImageActualFileName = item.ProfileImageActualFileName,
                       ProfileImageLocation = item.ProfileImageLocation,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       ModifiedByUserFirstName = item.FirstName,
                       ModifiedByUserLastName = item.LastName,
                       ModifiedByUserEmail = item.Email,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<UserView> GetUsersViewAsIQueryableWithSearchCriteria(UserSearchCriteria search)
        {
            var sql1 = GetUsersViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                 x.UserName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Email.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.FirstName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.LastName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.PredefinedUserTypeIds != null && search.PredefinedUserTypeIds.Count > 0)
            {
                sql1 = sql1.Where(x => search.PredefinedUserTypeIds.Contains(x.PredefinedUserTypeId));
            }

            if (search.ShowDeleted != null)
            {
                sql1 = sql1.Where(x => x.IsDeleted == search.ShowDeleted.Value);
            }

            if (search.Active != null)
            {
                sql1 = sql1.Where(x => x.Active == search.Active.Value);
            }

            return sql1;
        }

        public async Task<DbReturnListModel<UserView>> GetUsersInternalAsync(UserSearchCriteria search)
        {
            var sql1 = GetUsersViewAsIQueryableWithSearchCriteria(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.UserId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetUsersViewAsIQueryable().FirstOrDefaultAsync(x => x.UserId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<UserView>> GetUsersAsync(UserSearchCriteria search)
        {
            return GetUsersInternalAsync(search);
        }

        public Task<User> GetUserByIdAsync(long userId)
        {
            return appDBManager.FirstOrDefaultAsync<User>(x => x.UserId == userId);
        }

        public Task<User> GetUserByUserGuidAsync(Guid userGuid)
        {
            return appDBManager.FirstOrDefaultAsync<User>(x => x.UserGuid == userGuid);
        }

        public Task<User> GetUserByUserNameAsync(string userName)
        {
            return appDBManager.FirstOrDefaultAsync<User>(x => x.UserName == userName);
        }

        public Task<User> GetUserByUniqueIdentifierAsync(string email)
        {
            return appDBManager.FirstOrDefaultAsync<User>(x => x.Email.Trim().ToLower() == email.Trim().ToLower());
        }

        public Task<UserView> GetUserViewByIdAsync(long userId)
        {
            var sql1 = GetUsersViewAsIQueryable();

            sql1 = sql1.Where(x => x.UserId == userId);
            return sql1.FirstOrDefaultAsync();
        }

        public Task<UserView> GetUserViewByEmailAsync(string email)
        {
            var sql1 = GetUsersViewAsIQueryable();

            sql1 = sql1.Where(x => x.Email == email);
            return sql1.FirstOrDefaultAsync();
        }

        public Task<UserView> GetUserViewByUserNameAsync(string username)
        {
            var sql1 = GetUsersViewAsIQueryable();

            sql1 = sql1.Where(x => x.UserName == username);
            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<User>> GetAllUsers()
        {
            return appDBManager.SelectAllAsync<User>();
        }

        public async Task<User> CreateUserAsync(User user, long? createdBy)
        {
            user.UserId = await appDBManager.InsertWithAuditAsync<User, long>(user, createdBy);
            return user;
        }

        public async Task<List<User>> CreateUsersAsync(List<User> users, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<User, long>(users, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                users[i].UserId = lst[i];
            return users;
        }

        public Task<bool> UpdateUserAsync(User user, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<User>(user, modifiedBy);
        }

        public Task<bool> IsUserAlreadyExistsByUserGuidAsync(Guid userGuid)
        {
            var sql = GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserGuid == userGuid);
            return sql.AnyAsync();
        }

        public Task<bool> IsUserNameAlreadyExistsForOtherUserAsync(long userId, string userName)
        {
            var sql = GetUsersViewAsIQueryable().Where(x => x.UserId != userId && x.UserName.ToLower().Trim() == userName.ToLower().Trim() && x.IsDeleted == false);
            return sql.AnyAsync();
        }

        public Task<bool> IsUserNameAlreadyExistsForOtherUserAsync(string userName)
        {
            var sql = GetUsersViewAsIQueryable().Where(x => x.UserName.ToLower().Trim() == userName.ToLower().Trim() && x.IsDeleted == false);
            return sql.AnyAsync();
        }

        public async Task<bool> HardDeleteUserByIdAsync(long userId)
        {
            return await appDBManager.GetTable<User>().Where(x => x.UserId == userId).DeleteAsync() > 0;
        }

        public async Task<bool> UpdateUserForProfileAsync(User profile, long? modifiedBy)
        {
            return await appDBManager.GetTable<User>()
               .Where(x => x.UserId == profile.UserId)
               .Set(x => x.FirstName, profile.FirstName)
               .Set(x => x.LastName, profile.LastName)
               .Set(x => x.CellPhone, profile.CellPhone)
               .Set(x => x.WorkPhone, profile.WorkPhone)
               .Set(x => x.Email, profile.Email)
               .Set(x => x.UserName, profile.UserName)
               .Set(x => x.NotificationThroughEmail, profile.NotificationThroughEmail)
               .UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public Task<int> GetUserCountAsync()
        {
            var sql = GetUsersViewAsIQueryable();
            return sql.Select(x => x.UserId).CountAsync();
        }

        public Task<int> GetUserCountAsync(UserSearchCriteria search)
        {
            var sql = GetUsersViewAsIQueryableWithSearchCriteria(search);
            return sql.Select(x => x.UserId).CountAsync();
        }

        public async Task<bool> EnableUserByIdAsync(long userId, long? modifiedBy)
        {
            return await appDBManager.GetTable<User>().Where(x => x.UserId == userId).Set(x => x.Active, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableUserByIdAsync(long userId, long? modifiedBy)
        {
            return await appDBManager.GetTable<User>().Where(x => x.UserId == userId).Set(x => x.Active, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkUserAsNotDeletedByIdAsync(long userId, long? modifiedBy)
        {
            return await appDBManager.GetTable<User>().Where(x => x.UserId == userId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkUserAsDeletedByIdAsync(long userId, long? modifiedBy)
        {
            return await appDBManager.GetTable<User>().Where(x => x.UserId == userId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        #endregion User

        #region UserPermission

        public IQueryable<UserPermissionView> GetUserPermissionsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<UserPermission>()
                   join a in appDBManager.GetTable<User>() on item.UserId equals a.UserId
                   join b in appDBManager.GetTable<EnumType>() on item.PredefinedUserPermissionTypeId equals b.EnumTypeId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new UserPermissionView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       UserName = a.UserName,
                       PredefinedUserPermissionTypeName = b.EnumTypeName,
                       UserPermissionId = item.UserPermissionId,
                       UserId = item.UserId,
                       PredefinedUserPermissionTypeId = item.PredefinedUserPermissionTypeId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<UserPermissionView> GetUserPermissionsInternalWithSearchCriteriaAsync(UserPermissionSearchCriteria search)
        {
            var sql1 = GetUserPermissionsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.UserPermissionId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper());
            }

            if (search.UserId != null && search.UserId > 0)
                sql1 = sql1.Where(x => search.UserId == x.UserId);
            if (search.PredefinedUserPermissionTypeIds != null && search.PredefinedUserPermissionTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedUserPermissionTypeIds.Contains(x.PredefinedUserPermissionTypeId));
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<UserPermissionView>> GetUserPermissionsInternalAsync(UserPermissionSearchCriteria search)
        {
            var sql1 = GetUserPermissionsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.UserPermissionId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetUserPermissionsViewAsIQueryable().FirstOrDefaultAsync(x => x.UserPermissionId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<UserPermissionView>> GetUserPermissionsAsync(UserPermissionSearchCriteria search)
        {
            return GetUserPermissionsInternalAsync(search);
        }

        public Task<UserPermission> GetUserPermissionFromAnyByIdAsync(long userPermissionId)
        {
            return appDBManager.FirstOrDefaultAsync<UserPermission>(x => x.UserPermissionId == userPermissionId);
        }

        public Task<UserPermissionView> GetUserPermissionViewFromAnyByIdAsync(long userPermissionId)
        {
            return GetUserPermissionsViewAsIQueryable().FirstOrDefaultAsync(x => x.UserPermissionId == userPermissionId);
        }

        public Task<UserPermission> GetUserPermissionByIdAsync(long userPermissionId)
        {
            return appDBManager.FirstOrDefaultAsync<UserPermission>(x => x.UserPermissionId == userPermissionId && x.IsDeleted == false);
        }

        public Task<UserPermission> GetUserPermissionByUniqueIdentifierAsync(string userPermissionId)
        {
            return appDBManager.FirstOrDefaultAsync<UserPermission>(x => x.UserPermissionId == userPermissionId.Int() && x.IsDeleted == false);
        }

        public Task<UserPermissionView> GetUserPermissionViewByIdAsync(long userPermissionId)
        {
            var sql1 = GetUserPermissionsViewAsIQueryable();

            sql1 = sql1.Where(x => x.UserPermissionId == userPermissionId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<UserPermission>> GetAllUserPermissions()
        {
            return appDBManager.SelectAllAsync<UserPermission>();
        }

        public async Task<UserPermission> CreateUserPermissionAsync(UserPermission userPermission, long? createdBy)
        {
            userPermission.UserPermissionId = await appDBManager.InsertWithAuditAsync<UserPermission, long>(userPermission, createdBy);
            return userPermission;
        }

        public async Task<List<UserPermission>> CreateUserPermissionsAsync(List<UserPermission> userPermissions, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<UserPermission, long>(userPermissions, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                userPermissions[i].UserPermissionId = lst[i];
            return userPermissions;
        }

        public Task<bool> UpdateUserPermissionAsync(UserPermission userPermission, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<UserPermission>(userPermission, modifiedBy);
        }

        public async Task<bool> MarkUserPermissionAsNotDeletedByIdAsync(long userPermissionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<UserPermission>().Where(x => x.UserPermissionId == userPermissionId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkUserPermissionAsDeletedByIdAsync(long userPermissionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<UserPermission>().Where(x => x.UserPermissionId == userPermissionId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteUserPermissionByIdAsync(long userPermissionId)
        {
            return await appDBManager.GetTable<UserPermission>().Where(x => x.UserPermissionId == userPermissionId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetUserPermissionActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.UserPermission);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsUserPermissionNameAlreadyExistsForOtherUserPermissionAsync(long userPermissionId, long userId, long predefinedUserPermissionTypeId)
        {
            var sql = GetUserPermissionsViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserPermissionId != userPermissionId && x.UserId == userId && x.PredefinedUserPermissionTypeId == predefinedUserPermissionTypeId);
            return sql.AnyAsync();
        }

        public Task<bool> IsUserPermissionNameAlreadyExistsForOtherUserPermissionAsync(long userId, long predefinedUserPermissionTypeId)
        {
            var sql = GetUserPermissionsViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId && x.PredefinedUserPermissionTypeId == predefinedUserPermissionTypeId);
            return sql.AnyAsync();
        }

        /*

        public async Task<bool> EnableUserPermissionByIdAsync(long userPermissionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<UserPermission>().Where(x => x.UserPermissionId == userPermissionId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableUserPermissionByIdAsync(long userPermissionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<UserPermission>().Where(x => x.UserPermissionId == userPermissionId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateUserPermissionWithXYZZYXAsync(long userPermissionId, long xYZZYXId, long? createdBy)
        {
            var item = new UserPermissionXYZZYXAssociationModel();
            item.UserPermissionId = userPermissionId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<UserPermissionXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateUserPermissionWithXYZZYXAsync(long userPermissionId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<UserPermissionXYZZYX>(x => x.UserPermissionId == userPermissionId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesUserPermissionWithXYZZYXAssociationExistsAsync(long userPermissionId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<UserPermissionXYZZYX>(x => x.UserPermissionId == userPermissionId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<UserPermissionView>> GetUserPermissionsByXYZZYXAsync(long xYZZYXId)
        {
            var userPermissionIds = GetUserPermissionXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.UserPermissionId);
            var userPermissionSql = GetUserPermissionsViewAsIQueryable().Where(x => userPermissionIds.Contains(x.UserPermissionId));
            return userPermissionSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByUserPermissionsAsync(long userPermissionId)
        {
            var xYZZYXIds = GetUserPermissionXYZZYXsViewAsIQueryable().Where(x => x.UserPermissionId == userPermission).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<UserPermissionXYZZYX> GetUserPermissionXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<UserPermissionXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetUserPermissionXYZZYXActivitiesAsync(long companyId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.CompanyId == companyId && x.ResourceTypeId == (int)ResourceTypes.UserPermissionXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region SingleSignOnServer
        public Task<SingleSignOnServer> GetSingleSignOnServerBySingleSignOnServerIdAsync(long singleSignOnServerId)
        {
            return appDBManager.FirstOrDefaultAsync<SingleSignOnServer>(x => x.SingleSignOnServerId == singleSignOnServerId);
        }

        public Task<SingleSignOnServer> GetSingleSignOnServerByCodeAsync(string singleSignOnServerCode)
        {
            return appDBManager.FirstOrDefaultAsync<SingleSignOnServer>(x => x.SingleSignOnServerCode == singleSignOnServerCode);
        }

        public Task<SingleSignOnServer> GetDefaultSingleSignOnServerAsync()
        {
            return appDBManager.FirstOrDefaultAsync<SingleSignOnServer>(x => x.IsDefault == true && x.IsDeleted == false);
        }

        public async Task<bool> UpdateSingleSignOnServerCodeAsync(long singlesignonserverId, string singlesignonserverCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<SingleSignOnServer>().Where(x => x.SingleSignOnServerId == singlesignonserverId && x.IsDeleted == false).Set(x => x.SingleSignOnServerCode, singlesignonserverCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<SingleSignOnServer> CreateSingleSignOnServerAsync(SingleSignOnServer singleSignOnServer, long? createdBy)
        {
            singleSignOnServer.SingleSignOnServerId = await appDBManager.InsertWithAuditAsync<SingleSignOnServer, long>(singleSignOnServer, createdBy);
            return singleSignOnServer;
        }

        public Task<bool> UpdateSingleSignOnServerAsync(SingleSignOnServer singleSignOnServer, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<SingleSignOnServer>(singleSignOnServer, modifiedBy);
        }

        public async Task<bool> HardDeleteSingleSignOnServerBySingleSignOnServerIdAsync(long singleSignOnServerId)
        {
            return await appDBManager.GetTable<SingleSignOnServer>().Where(x => x.SingleSignOnServerId == singleSignOnServerId).DeleteAsync() > 0;
        }
        #endregion

        #region UserGroup

        public IQueryable<UserGroupView> GetUserGroupsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<UserGroup>()
                   join ocu in appDBManager.GetTable<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTable<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new UserGroupView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       UserGroupId = item.UserGroupId,
                       UserGroupName = item.UserGroupName,
                       CompanyEntityId = item.CompanyEntityId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<UserGroupView> GetUserGroupsInternalWithSearchCriteriaAsync(UserGroupSearchCriteria search)
        {
            var sql1 = GetUserGroupsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.UserGroupId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.UserGroupName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
            {
                if (search.StartDate.Value <= search.EndDate.Value)
                {
                    sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate <= search.EndDate.Value.EndOfDay());
                }
            }
            else if (search.StartDate.IsValidDate())
            {
                sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
            }
            else if (search.EndDate.IsValidDate())
            {
                sql1 = sql1.Where(x => x.OrderDate <= search.EndDate.Value.EndOfDay());
            }*/

            if (search.CompanyEntityId != null && search.CompanyEntityId > 0)
                sql1 = sql1.Where(x => search.CompanyEntityId == x.CompanyEntityId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<UserGroupView>> GetUserGroupsInternalAsync(UserGroupSearchCriteria search)
        {
            var sql1 = GetUserGroupsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.UserGroupId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetUserGroupsViewAsIQueryable().FirstOrDefaultAsync(x => x.UserGroupId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<UserGroupView>> GetUserGroupsAsync(UserGroupSearchCriteria search)
        {
            return GetUserGroupsInternalAsync(search);
        }

        public Task<UserGroup> GetUserGroupFromAnyByIdAsync(long userGroupId)
        {
            return appDBManager.FirstOrDefaultAsync<UserGroup>(x => x.UserGroupId == userGroupId);
        }

        public Task<UserGroupView> GetUserGroupViewFromAnyByIdAsync(long userGroupId)
        {
            return GetUserGroupsViewAsIQueryable().FirstOrDefaultAsync(x => x.UserGroupId == userGroupId);
        }

        public Task<UserGroup> GetUserGroupByUniqueIdentifierAsync(string userGroupId)
        {
            return appDBManager.FirstOrDefaultAsync<UserGroup>(x => x.UserGroupId == SafeUtils.Int(userGroupId) && x.IsDeleted == false);
        }

        public Task<UserGroup> GetUserGroupByIdAsync(long userGroupId)
        {
            return appDBManager.FirstOrDefaultAsync<UserGroup>(x => x.UserGroupId == userGroupId && x.IsDeleted == false);
        }

        public Task<List<UserGroup>> GetUserGroupByIdsAsync(List<long> userGroupIds)
        {
            return appDBManager.GetTable<UserGroup>().Where(x => userGroupIds.Contains(x.UserGroupId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<UserGroupView> GetUserGroupViewByIdAsync(long userGroupId)
        {
            var sql1 = GetUserGroupsViewAsIQueryable();

            sql1 = sql1.Where(x => x.UserGroupId == userGroupId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<UserGroup>> GetAllUserGroups()
        {
            return appDBManager.SelectAllAsync<UserGroup>();
        }

        /*
        public Task<UserGroup> GetUserGroupByUserGroupCodeAsync(long companyId, string userGroupCode)
        {
            return appDBManager.FirstOrDefaultAsync<UserGroup>(x => x.CompanyId == companyId && x.UserGroupCode == userGroupCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateUserGroupCodeAsync(long companyId, long userGroupId, string userGroupCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<UserGroup>().Where(x => x.CompanyId == companyId && x.UserGroupId == userGroupId && x.IsDeleted == false).Set(x => x.UserGroupCode, userGroupCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<UserGroup> CreateUserGroupAsync(UserGroup userGroup, long? createdBy)
        {
            userGroup.UserGroupId = await appDBManager.InsertWithAuditAsync<UserGroup, long>(userGroup, createdBy);
            return userGroup;
        }

        public async Task<List<UserGroup>> CreateUserGroupsAsync(List<UserGroup> userGroups, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<UserGroup, long>(userGroups, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                userGroups[i].UserGroupId = lst[i];
            return userGroups;
        }

        public Task<bool> UpdateUserGroupAsync(UserGroup userGroup, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<UserGroup>(userGroup, modifiedBy);
        }

        public async Task<bool> MarkUserGroupAsNotDeletedByIdAsync(long userGroupId, long? modifiedBy)
        {
            return await appDBManager.GetTable<UserGroup>().Where(x => x.UserGroupId == userGroupId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkUserGroupAsDeletedByIdAsync(long userGroupId, long? modifiedBy)
        {
            return await appDBManager.GetTable<UserGroup>().Where(x => x.UserGroupId == userGroupId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteUserGroupByIdAsync(long userGroupId)
        {
            return await appDBManager.GetTable<UserGroup>().Where(x => x.UserGroupId == userGroupId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetUserGroupActivitiesAsync(ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.UserGroup);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<List<UserGroupView>> GetUserGroupViewByCompanyEntityIdAsync(long companyEntityId)
        {
            return GetUserGroupsViewAsIQueryable().Where(x => x.IsDeleted == false && x.CompanyEntityId == companyEntityId).ToListAsync();
        }

        public Task<DbReturnListModel<UserGroupView>> GetUserGroupViewByCompanyEntityIdAsync(UserGroupByCompanyEntityIdSearchCriteria search)
        {
            return GetUserGroupsViewAsIQueryable().Where(x => x.IsDeleted == false && x.CompanyEntityId == search.CompanyEntityId).ReturnListModelResultAsync(search);
        }

        public Task<List<UserGroup>> GetBareUserGroupByCompanyEntityIdAsync(long companyEntityId)
        {
            return appDBManager.GetTable<UserGroup>().Where(x => x.IsDeleted == false && x.CompanyEntityId == companyEntityId).ToListAsync();
        }

        public Task<DbReturnListModel<UserGroup>> GetBareUserGroupByCompanyEntityIdAsync(UserGroupByCompanyEntityIdSearchCriteria search)
        {
            return appDBManager.GetTable<UserGroup>().Where(x => x.IsDeleted == false && x.CompanyEntityId == search.CompanyEntityId).ReturnListModelResultAsync(search);
        }
        /*

        public async Task<bool> UpdateUserGroupPicturePathAsync(long userGroupId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<UserGroup>().Where(x => x.UserGroupId == userGroupId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearUserGroupProfilePathAsync(long userGroupId, long? modifiedBy)
        {
            return await appDBManager.GetTable<UserGroup>().Where(x => x.UserGroupId == userGroupId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableUserGroupByIdAsync(long userGroupId, long? modifiedBy)
        {
            return await appDBManager.GetTable<UserGroup>().Where(x => x.UserGroupId == userGroupId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableUserGroupByIdAsync(long userGroupId, long? modifiedBy)
        {
            return await appDBManager.GetTable<UserGroup>().Where(x => x.UserGroupId == userGroupId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateUserGroupWithXYZZYXAsync(long userGroupId, long xYZZYXId, long? createdBy)
        {
            var item = new UserGroupXYZZYXAssociationModel();
            item.UserGroupId = userGroupId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<UserGroupXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateUserGroupWithXYZZYXAsync(long userGroupId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<UserGroupXYZZYX>(x => x.UserGroupId == userGroupId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesUserGroupWithXYZZYXAssociationExistsAsync(long userGroupId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<UserGroupXYZZYX>(x => x.UserGroupId == userGroupId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<UserGroupView>> GetUserGroupsByXYZZYXAsync(long xYZZYXId)
        {
            var userGroupIds = GetUserGroupXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.UserGroupId);
            var userGroupSql = GetUserGroupsViewAsIQueryable().Where(x => userGroupIds.Contains(x.UserGroupId));
            return userGroupSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByUserGroupsAsync(long userGroupId)
        {
            var xYZZYXIds = GetUserGroupXYZZYXsViewAsIQueryable().Where(x => x.UserGroupId == userGroup).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<UserGroupXYZZYX> GetUserGroupXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<UserGroupXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetUserGroupXYZZYXActivitiesAsync(long companyId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.CompanyId == companyId && x.ResourceTypeId == (int)ResourceTypes.UserGroupXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion
    }
}
