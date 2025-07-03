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
    public class MovieRepository : BaseRepository
    {
        public MovieRepository(IAppDBManager appDBManager)
            : base(appDBManager)
        {
        }

        #region NoteAttachment

        public IQueryable<NoteAttachmentView> GetNoteAttachmentsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<NoteAttachment>()
                   join user in appDBManager.GetTable<User>() on item.UserId equals user.UserId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new NoteAttachmentView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       NoteAttachmentId = item.NoteAttachmentId,
                       Title = item.Title,
                       Description = item.Description,
                       UserId = item.UserId,
                       UserName = user.UserName,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<NoteAttachmentView> GetNoteAttachmentsViewWithAllDetailsAsIQueryable()
        {
            return from item in appDBManager.GetTable<NoteAttachment>()
                   join user in appDBManager.GetTable<User>() on item.UserId equals user.UserId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                select new NoteAttachmentView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       NoteAttachmentId = item.NoteAttachmentId,
                       Title = item.Title,
                       Description = item.Description,
                       UserId = item.UserId,
                       UserName = user.UserName,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<NoteAttachmentView> GetNoteAttachmentsInternalWithSearchCriteriaAsync(NoteAttachmentSearchCriteria search, bool withAllDetails = false)
        {
            var sql1 = withAllDetails ? GetNoteAttachmentsViewWithAllDetailsAsIQueryable() : GetNoteAttachmentsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                 x.Title.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Description.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.NoteAttachmentIds.IsNotNullOrEmpty())
            {
                sql1 = sql1.Where(x => search.NoteAttachmentIds.Contains(x.NoteAttachmentId));
            }

            if (search.UserId != null && search.UserId > 0)
                sql1 = sql1.Where(x => search.UserId == x.UserId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);
            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<NoteAttachmentView>> GetNoteAttachmentsInternalAsync(NoteAttachmentSearchCriteria search, bool withAllDetails = false)
        {
            var sql1 = GetNoteAttachmentsInternalWithSearchCriteriaAsync(search, withAllDetails);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.NoteAttachmentId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetNoteAttachmentsViewAsIQueryable().FirstOrDefaultAsync(x => x.NoteAttachmentId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<NoteAttachmentView>> GetNoteAttachmentsAsync(NoteAttachmentSearchCriteria search, bool withAllDetails = false)
        {
            return GetNoteAttachmentsInternalAsync(search, withAllDetails);
        }

        public Task<NoteAttachment> GetNoteAttachmentFromAnyByIdAsync(long noteAttachmentId)
        {
            return appDBManager.FirstOrDefaultAsync<NoteAttachment>((System.Linq.Expressions.Expression<Func<NoteAttachment, bool>>)(x => x.NoteAttachmentId == noteAttachmentId));
        }

        public Task<NoteAttachmentView> GetNoteAttachmentViewFromAnyByIdAsync(long noteAttachmentId)
        {
            return GetNoteAttachmentsViewAsIQueryable().FirstOrDefaultAsync(x => x.NoteAttachmentId == noteAttachmentId);
        }

        public Task<NoteAttachment> GetNoteAttachmentByUniqueIdentifierAsync(string noteAttachmentId)
        {
            return appDBManager.FirstOrDefaultAsync<NoteAttachment>((System.Linq.Expressions.Expression<Func<NoteAttachment, bool>>)(x => x.NoteAttachmentId == SafeUtils.Int(noteAttachmentId) && x.IsDeleted == false));
        }

        public Task<NoteAttachment> GetNoteAttachmentByIdAsync(long noteAttachmentId)
        {
            return appDBManager.FirstOrDefaultAsync<NoteAttachment>((System.Linq.Expressions.Expression<Func<NoteAttachment, bool>>)(x => x.NoteAttachmentId == noteAttachmentId && x.IsDeleted == false));
        }

        public Task<NoteAttachmentView> GetNoteAttachmentViewByIdAsync(long noteAttachmentId)
        {
            var sql1 = GetNoteAttachmentsViewAsIQueryable();

            sql1 = sql1.Where(x => x.NoteAttachmentId == noteAttachmentId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<NoteAttachmentView> GetNoteAttachmentViewByDetailsAsync(string title, long userId)
        {
            return GetNoteAttachmentsViewAsIQueryable()
                .Where(x => x.IsDeleted == false
                            && x.UserId == userId
                            && x.Title.Trim().ToLower() == title.Trim().ToLower())
                .FirstOrDefaultAsync();
        }

        public Task<List<NoteAttachment>> GetAllNoteAttachments()
        {
            return appDBManager.SelectAllAsync<NoteAttachment>();
        }

        public async Task<NoteAttachment> CreateNoteAttachmentAsync(NoteAttachment noteAttachment, long? createdBy)
        {
            noteAttachment.NoteAttachmentId = await appDBManager.InsertWithAuditAsync<NoteAttachment, long>(noteAttachment, createdBy);
            return noteAttachment;
        }

        public async Task<List<NoteAttachment>> CreateNoteAttachmentsAsync(List<NoteAttachment> noteAttachments, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<NoteAttachment, long>(noteAttachments, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                noteAttachments[i].NoteAttachmentId = lst[i];
            return noteAttachments;
        }

        public Task<bool> UpdateNoteAttachmentAsync(NoteAttachment noteAttachment, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<NoteAttachment>(noteAttachment, modifiedBy);
        }

        public async Task<bool> MarkNoteAttachmentAsNotDeletedByIdAsync(long noteAttachmentId, long? modifiedBy)
        {
            return await appDBManager.GetTable<NoteAttachment>().Where(x => x.NoteAttachmentId == noteAttachmentId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkNoteAttachmentAsDeletedByIdAsync(long noteAttachmentId, long? modifiedBy)
        {
            return await appDBManager.GetTable<NoteAttachment>().Where(x => x.NoteAttachmentId == noteAttachmentId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteNoteAttachmentByIdAsync(long noteAttachmentId)
        {
            return await appDBManager.GetTable<NoteAttachment>().Where(x => x.NoteAttachmentId == noteAttachmentId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetNoteAttachmentActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.NoteAttachment);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsNoteAttachmentAlreadyExistsForOtherNoteAttachmentAsync(long noteAttachmentId, string title, long userId)
        {
            var sql = GetNoteAttachmentsViewAsIQueryable().Where(x => x.IsDeleted == false && x.NoteAttachmentId != noteAttachmentId && x.Title.Trim().ToLower() == title.Trim().ToLower() && x.UserId == userId);
            return sql.AnyAsync();
        }

        public Task<bool> IsNoteAttachmentAlreadyExistsForOtherNoteAttachmentAsync(string title, long userId)
        {
            var sql = GetNoteAttachmentsViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId && x.Title.Trim().ToLower() == title.Trim().ToLower());
            return sql.AnyAsync();
        }

        /*

        public async Task<bool> EnableNoteAttachmentByIdAsync(long noteAttachmentId, long? modifiedBy)
        {
            return await appDBManager.GetTable<NoteAttachment>().Where(x => x.NoteAttachmentId == noteAttachmentId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableNoteAttachmentByIdAsync(long noteAttachmentId, long? modifiedBy)
        {
            return await appDBManager.GetTable<NoteAttachment>().Where(x => x.NoteAttachmentId == noteAttachmentId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateNoteAttachmentWithXYZZYXAsync(long noteAttachmentId, long xYZZYXId, long? createdBy)
        {
            var item = new NoteAttachmentXYZZYXAssociationModel();
            item.NoteAttachmentId = noteAttachmentId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<NoteAttachmentXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateNoteAttachmentWithXYZZYXAsync(long noteAttachmentId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<NoteAttachmentXYZZYX>(x => x.NoteAttachmentId == noteAttachmentId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesNoteAttachmentWithXYZZYXAssociationExistsAsync(long noteAttachmentId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<NoteAttachmentXYZZYX>(x => x.NoteAttachmentId == noteAttachmentId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<NoteAttachmentView>> GetNoteAttachmentsByXYZZYXAsync(long xYZZYXId)
        {
            var noteAttachmentIds = GetNoteAttachmentXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.NoteAttachmentId);
            var noteAttachmentSql = GetNoteAttachmentsViewAsIQueryable().Where(x => noteAttachmentIds.Contains(x.NoteAttachmentId));
            return noteAttachmentSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByNoteAttachmentsAsync(long noteAttachmentId)
        {
            var xYZZYXIds = GetNoteAttachmentXYZZYXsViewAsIQueryable().Where(x => x.NoteAttachmentId == noteAttachment).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<NoteAttachmentXYZZYX> GetNoteAttachmentXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<NoteAttachmentXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetNoteAttachmentXYZZYXActivitiesAsync(long companyId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.CompanyId == companyId && x.ResourceTypeId == (int)ResourceTypes.NoteAttachmentXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region FileAttachmentFolder

        public IQueryable<FileAttachmentFolderView> GetFileAttachmentFoldersViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<FileAttachmentFolder>()
                join b in appDBManager.GetTable<User>() on item.UserId equals b.UserId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new FileAttachmentFolderView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       UserName = b.UserName,
                       FileAttachmentFolderId = item.FileAttachmentFolderId,
                       FileAttachmentFolderName = item.FileAttachmentFolderName,
                       ParentFileAttachmentFolderId = item.ParentFileAttachmentFolderId,
                       Description = item.Description,
                       UserId = item.UserId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<FileAttachmentFolderView> GetFileAttachmentFoldersInternalWithSearchCriteriaAsync(FileAttachmentFolderSearchCriteria search)
        {
            var sql1 = GetFileAttachmentFoldersViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.FileAttachmentFolderId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.FileAttachmentFolderName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Description.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.ParentFileAttachmentFolderId != null && search.ParentFileAttachmentFolderId > 0)
                sql1 = sql1.Where(x => search.ParentFileAttachmentFolderId == x.ParentFileAttachmentFolderId);
            if (search.UserId != null && search.UserId > 0)
                sql1 = sql1.Where(x => search.UserId == x.UserId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<FileAttachmentFolderView>> GetFileAttachmentFoldersInternalAsync(FileAttachmentFolderSearchCriteria search)
        {
            var sql1 = GetFileAttachmentFoldersInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.FileAttachmentFolderId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetFileAttachmentFoldersViewAsIQueryable().FirstOrDefaultAsync(x => x.FileAttachmentFolderId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<FileAttachmentFolderView>> GetFileAttachmentFoldersAsync(FileAttachmentFolderSearchCriteria search)
        {
            return GetFileAttachmentFoldersInternalAsync(search);
        }

        public Task<FileAttachmentFolder> GetFileAttachmentFolderFromAnyByIdAsync(long fileAttachmentFolderId)
        {
            return appDBManager.FirstOrDefaultAsync<FileAttachmentFolder>(x => x.FileAttachmentFolderId == fileAttachmentFolderId);
        }

        public Task<FileAttachmentFolderView> GetFileAttachmentFolderViewFromAnyByIdAsync(long fileAttachmentFolderId)
        {
            return GetFileAttachmentFoldersViewAsIQueryable().FirstOrDefaultAsync(x => x.FileAttachmentFolderId == fileAttachmentFolderId);
        }

        public Task<FileAttachmentFolder> GetFileAttachmentFolderByUniqueIdentifierAsync(string fileAttachmentFolderId)
        {
            return appDBManager.FirstOrDefaultAsync<FileAttachmentFolder>(x => x.FileAttachmentFolderId == SafeUtils.Int(fileAttachmentFolderId) && x.IsDeleted == false);
        }

        public Task<FileAttachmentFolder> GetFileAttachmentFolderByIdAsync(long fileAttachmentFolderId)
        {
            return appDBManager.FirstOrDefaultAsync<FileAttachmentFolder>(x => x.FileAttachmentFolderId == fileAttachmentFolderId && x.IsDeleted == false);
        }

        public Task<FileAttachmentFolderView> GetFileAttachmentFolderViewByIdAsync(long fileAttachmentFolderId)
        {
            var sql1 = GetFileAttachmentFoldersViewAsIQueryable();

            sql1 = sql1.Where(x => x.FileAttachmentFolderId == fileAttachmentFolderId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<FileAttachmentFolder>> GetAllFileAttachmentFolders()
        {
            return appDBManager.SelectAllAsync<FileAttachmentFolder>();
        }

        public async Task<FileAttachmentFolder> CreateFileAttachmentFolderAsync(FileAttachmentFolder fileAttachmentFolder, long? createdBy)
        {
            fileAttachmentFolder.FileAttachmentFolderId = await appDBManager.InsertWithAuditAsync<FileAttachmentFolder, long>(fileAttachmentFolder, createdBy);
            return fileAttachmentFolder;
        }

        public async Task<List<FileAttachmentFolder>> CreateFileAttachmentFoldersAsync(List<FileAttachmentFolder> fileAttachmentFolders, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<FileAttachmentFolder, long>(fileAttachmentFolders, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                fileAttachmentFolders[i].FileAttachmentFolderId = lst[i];
            return fileAttachmentFolders;
        }

        public Task<bool> UpdateFileAttachmentFolderAsync(FileAttachmentFolder fileAttachmentFolder, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<FileAttachmentFolder>(fileAttachmentFolder, modifiedBy);
        }

        public async Task<bool> MarkFileAttachmentFolderAsNotDeletedByIdAsync(long fileAttachmentFolderId, long? modifiedBy)
        {
            return await appDBManager.GetTable<FileAttachmentFolder>().Where(x => x.FileAttachmentFolderId == fileAttachmentFolderId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkFileAttachmentFolderAsDeletedByIdAsync(long fileAttachmentFolderId, long? modifiedBy)
        {
            return await appDBManager.GetTable<FileAttachmentFolder>().Where(x => x.FileAttachmentFolderId == fileAttachmentFolderId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteFileAttachmentFolderByIdAsync(long fileAttachmentFolderId)
        {
            return await appDBManager.GetTable<FileAttachmentFolder>().Where(x => x.FileAttachmentFolderId == fileAttachmentFolderId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetFileAttachmentFolderActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.FileAttachmentFolder);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsFileAttachmentFolderNameAlreadyExistsForOtherFileAttachmentFolderAsync(long fileAttachmentFolderId, string fileAttachmentFolderName, long userId)
        {
            var sql = GetFileAttachmentFoldersViewAsIQueryable().Where(x => x.IsDeleted == false && x.FileAttachmentFolderId != fileAttachmentFolderId && x.FileAttachmentFolderName.ToLower().Trim() == fileAttachmentFolderName.ToLower().Trim() && x.UserId == userId);
            return sql.AnyAsync();
        }

        public Task<bool> IsFileAttachmentFolderNameAlreadyExistsForOtherFileAttachmentFolderAsync(string fileAttachmentFolderName, long userId)
        {
            var sql = GetFileAttachmentFoldersViewAsIQueryable().Where(x => x.IsDeleted == false && x.FileAttachmentFolderName.ToLower().Trim() == fileAttachmentFolderName.ToLower().Trim() && x.UserId == userId);
            return sql.AnyAsync();
        }

        /*

        public async Task<bool> UpdateFileAttachmentFolderPicturePathAsync(long fileAttachmentFolderId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<FileAttachmentFolder>().Where(x => x.FileAttachmentFolderId == fileAttachmentFolderId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearFileAttachmentFolderProfilePathAsync(long fileAttachmentFolderId, long? modifiedBy)
        {
            return await appDBManager.GetTable<FileAttachmentFolder>().Where(x => x.FileAttachmentFolderId == fileAttachmentFolderId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableFileAttachmentFolderByIdAsync(long fileAttachmentFolderId, long? modifiedBy)
        {
            return await appDBManager.GetTable<FileAttachmentFolder>().Where(x => x.FileAttachmentFolderId == fileAttachmentFolderId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableFileAttachmentFolderByIdAsync(long fileAttachmentFolderId, long? modifiedBy)
        {
            return await appDBManager.GetTable<FileAttachmentFolder>().Where(x => x.FileAttachmentFolderId == fileAttachmentFolderId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateFileAttachmentFolderWithXYZZYXAsync(long fileAttachmentFolderId, long xYZZYXId, long? createdBy)
        {
            var item = new FileAttachmentFolderXYZZYXAssociationModel();
            item.FileAttachmentFolderId = fileAttachmentFolderId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<FileAttachmentFolderXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateFileAttachmentFolderWithXYZZYXAsync(long fileAttachmentFolderId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<FileAttachmentFolderXYZZYX>(x => x.FileAttachmentFolderId == fileAttachmentFolderId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesFileAttachmentFolderWithXYZZYXAssociationExistsAsync(long fileAttachmentFolderId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<FileAttachmentFolderXYZZYX>(x => x.FileAttachmentFolderId == fileAttachmentFolderId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<FileAttachmentFolderView>> GetFileAttachmentFoldersByXYZZYXAsync(long xYZZYXId)
        {
            var fileAttachmentFolderIds = GetFileAttachmentFolderXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.FileAttachmentFolderId);
            var fileAttachmentFolderSql = GetFileAttachmentFoldersViewAsIQueryable().Where(x => fileAttachmentFolderIds.Contains(x.FileAttachmentFolderId));
            return fileAttachmentFolderSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByFileAttachmentFoldersAsync(long fileAttachmentFolderId)
        {
            var xYZZYXIds = GetFileAttachmentFolderXYZZYXsViewAsIQueryable().Where(x => x.FileAttachmentFolderId == fileAttachmentFolder).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<FileAttachmentFolderXYZZYX> GetFileAttachmentFolderXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<FileAttachmentFolderXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetFileAttachmentFolderXYZZYXActivitiesAsync(long companyId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.CompanyId == companyId && x.ResourceTypeId == (int)ResourceTypes.FileAttachmentFolderXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region FileAttachment

        public IQueryable<FileAttachmentView> GetFileAttachmentsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<FileAttachment>()
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new FileAttachmentView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       FileAttachmentId = item.FileAttachmentId,
                       FileAttachmentFileName = item.FileAttachmentFileName,
                       FileAttachmentFolderId = item.FileAttachmentFolderId,
                       FileAttachmentFileLocation = item.FileAttachmentFileLocation,
                       FileAttachmentPreviewFileLocation = item.FileAttachmentPreviewFileLocation,
                       FileAttachmentHasPreview = item.FileAttachmentHasPreview,
                       FileAttachmentFileSizeInBytes = item.FileAttachmentFileSizeInBytes,
                       Description = item.Description,
                       UserId = item.UserId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<FileAttachmentView> GetFileAttachmentsInternalWithSearchCriteriaAsync(FileAttachmentSearchCriteria search)
        {
            var sql1 = GetFileAttachmentsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x => x.Description.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.FileAttachmentIds.IsNotNullOrEmpty())
            {
                sql1 = sql1.Where(x => search.FileAttachmentIds.Contains(x.FileAttachmentId));
            }

            if (search.FileAttachmentFolderId != null && search.FileAttachmentFolderId > 0)
                sql1 = sql1.Where(x => search.FileAttachmentFolderId == x.FileAttachmentFolderId);
            if (search.UserId != null && search.UserId > 0)
                sql1 = sql1.Where(x => search.UserId == x.UserId);
            //if (search.MoviePropertyId != null && search.MoviePropertyId > 0)
            //    sql1 = sql1.Where(x => search.MoviePropertyId == x.MoviePropertyId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false && !search.ShowOnlyDeleted)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            if (search.ShowOnlyDeleted)
                sql1 = sql1.Where(x => x.IsDeleted == true);

            return sql1;
        }

        public async Task<DbReturnListModel<FileAttachmentView>> GetFileAttachmentsInternalAsync(FileAttachmentSearchCriteria search)
        {
            var sql1 = GetFileAttachmentsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.FileAttachmentId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetFileAttachmentsViewAsIQueryable().FirstOrDefaultAsync(x => x.FileAttachmentId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<FileAttachmentView>> GetFileAttachmentsAsync(FileAttachmentSearchCriteria search)
        {
            return GetFileAttachmentsInternalAsync(search);
        }

        public Task<FileAttachment> GetFileAttachmentFromAnyByIdAsync(long fileAttachmentId)
        {
            return appDBManager.FirstOrDefaultAsync<FileAttachment>(x => x.FileAttachmentId == fileAttachmentId);
        }

        public Task<FileAttachmentView> GetFileAttachmentViewFromAnyByIdAsync(long fileAttachmentId)
        {
            return GetFileAttachmentsViewAsIQueryable().FirstOrDefaultAsync(x => x.FileAttachmentId == fileAttachmentId);
        }

        public Task<FileAttachment> GetFileAttachmentByUniqueIdentifierAsync(string fileAttachmentId)
        {
            return appDBManager.FirstOrDefaultAsync<FileAttachment>(x => x.FileAttachmentId == SafeUtils.Int(fileAttachmentId) && x.IsDeleted == false);
        }

        public Task<FileAttachment> GetFileAttachmentByIdAsync(long fileAttachmentId)
        {
            return appDBManager.FirstOrDefaultAsync<FileAttachment>(x => x.FileAttachmentId == fileAttachmentId && x.IsDeleted == false);
        }

        public Task<FileAttachmentView> GetFileAttachmentViewByIdAsync(long fileAttachmentId)
        {
            var sql1 = GetFileAttachmentsViewAsIQueryable();

            sql1 = sql1.Where(x => x.FileAttachmentId == fileAttachmentId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<FileAttachment>> GetAllFileAttachments()
        {
            return appDBManager.SelectAllAsync<FileAttachment>();
        }

        public async Task<FileAttachment> CreateFileAttachmentAsync(FileAttachment fileAttachment, long? createdBy)
        {
            fileAttachment.FileAttachmentId = await appDBManager.InsertWithAuditAsync<FileAttachment, long>(fileAttachment, createdBy);
            return fileAttachment;
        }

        public async Task<List<FileAttachment>> CreateFileAttachmentsAsync(List<FileAttachment> fileAttachments, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<FileAttachment, long>(fileAttachments, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                fileAttachments[i].FileAttachmentId = lst[i];
            return fileAttachments;
        }

        public Task<bool> UpdateFileAttachmentAsync(FileAttachment fileAttachment, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<FileAttachment>(fileAttachment, modifiedBy);
        }

        public async Task<bool> MarkFileAttachmentAsNotDeletedByIdAsync(long fileAttachmentId, long? modifiedBy)
        {
            return await appDBManager.GetTable<FileAttachment>().Where(x => x.FileAttachmentId == fileAttachmentId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkFileAttachmentAsDeletedByIdAsync(long fileAttachmentId, long? modifiedBy)
        {
            return await appDBManager.GetTable<FileAttachment>().Where(x => x.FileAttachmentId == fileAttachmentId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteFileAttachmentByIdAsync(long fileAttachmentId)
        {
            return await appDBManager.GetTable<FileAttachment>().Where(x => x.FileAttachmentId == fileAttachmentId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetFileAttachmentActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.FileAttachment);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public async Task<bool> UpdateFileAttachmentFileLocationAsync(long fileAttachmentId, string fileLocation, string fileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<FileAttachment>().Where(x => x.FileAttachmentId == fileAttachmentId).Set(x => x.FileAttachmentFileLocation, fileLocation).Set(x => x.FileAttachmentFileName, fileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        /*
        public async Task<bool> EnableFileAttachmentByIdAsync(long fileAttachmentId, long? modifiedBy)
        {
            return await appDBManager.GetTable<FileAttachment>().Where(x => x.FileAttachmentId == fileAttachmentId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableFileAttachmentByIdAsync(long fileAttachmentId, long? modifiedBy)
        {
            return await appDBManager.GetTable<FileAttachment>().Where(x => x.FileAttachmentId == fileAttachmentId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateFileAttachmentWithXYZZYXAsync(long fileAttachmentId, long xYZZYXId, long? createdBy)
        {
            var item = new FileAttachmentXYZZYXAssociationModel();
            item.FileAttachmentId = fileAttachmentId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<FileAttachmentXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateFileAttachmentWithXYZZYXAsync(long fileAttachmentId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<FileAttachmentXYZZYX>(x => x.FileAttachmentId == fileAttachmentId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesFileAttachmentWithXYZZYXAssociationExistsAsync(long fileAttachmentId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<FileAttachmentXYZZYX>(x => x.FileAttachmentId == fileAttachmentId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<FileAttachmentView>> GetFileAttachmentsByXYZZYXAsync(long xYZZYXId)
        {
            var fileAttachmentIds = GetFileAttachmentXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.FileAttachmentId);
            var fileAttachmentSql = GetFileAttachmentsViewAsIQueryable().Where(x => fileAttachmentIds.Contains(x.FileAttachmentId));
            return fileAttachmentSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByFileAttachmentsAsync(long fileAttachmentId)
        {
            var xYZZYXIds = GetFileAttachmentXYZZYXsViewAsIQueryable().Where(x => x.FileAttachmentId == fileAttachment).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<FileAttachmentXYZZYX> GetFileAttachmentXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<FileAttachmentXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetFileAttachmentXYZZYXActivitiesAsync(long companyId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.CompanyId == companyId && x.ResourceTypeId == (int)ResourceTypes.FileAttachmentXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion
    }
}