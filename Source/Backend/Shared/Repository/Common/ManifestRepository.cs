using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Data.Utils;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using LinqToDB;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Repository.DbAccess;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.DBView;
using Manifest.Common.Models.SearchCriteria;
using static LinqToDB.Common.Configuration;

namespace Manifest.Repository.Common
{
    public class ManifestRepository : BaseRepository
    {
        CompanyRepository companyRepository;
        public ManifestRepository(IAppDBManager appDBManager, CompanyRepository companyRepository)
            : base(appDBManager)
        {
            this.companyRepository = companyRepository;
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

        public Task<DbReturnListModel<ActivityView>> GetNoteAttachmentXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.NoteAttachmentXYZZYX);
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

        public Task<DbReturnListModel<ActivityView>> GetFileAttachmentFolderXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.FileAttachmentFolderXYZZYX);
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

        public Task<DbReturnListModel<ActivityView>> GetFileAttachmentXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.FileAttachmentXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion
        
        #region VisionBoardSessionPicture

        public IQueryable<VisionBoardSessionPictureView> GetVisionBoardSessionPicturesViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<VisionBoardSessionPicture>()
                       join a in appDBManager.GetTable<VisionBoardSession>() on item.VisionBoardSessionId equals a.VisionBoardSessionId
                   join b in appDBManager.GetTable<VisionBoardPicture>() on item.VisionBoardPictureId equals b.VisionBoardPictureId
                   join c in appDBManager.GetTable<User>() on item.UserId equals c.UserId

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new VisionBoardSessionPictureView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       UserName = c.UserName,
                       VisionBoardSessionPictureId = item.VisionBoardSessionPictureId,
                       VisionBoardSessionId = item.VisionBoardSessionId,
                       VisionBoardSessionPictureDate = item.VisionBoardSessionPictureDate,
                       VisionBoardPictureId = item.VisionBoardPictureId,
                       VisionBoardSessionPictureTimeSpentInSeconds = item.VisionBoardSessionPictureTimeSpentInSeconds,
                       UserId = item.UserId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<VisionBoardSessionPictureView> GetVisionBoardSessionPicturesInternalWithSearchCriteriaAsync(VisionBoardSessionPictureSearchCriteria search)
        {
            var sql1 = GetVisionBoardSessionPicturesViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.VisionBoardSessionPictureId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                );
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

    if (search.VisionBoardSessionId != null && search.VisionBoardSessionId > 0)
                    sql1 = sql1.Where(x => search.VisionBoardSessionId == x.VisionBoardSessionId);
                if (search.VisionBoardPictureId != null && search.VisionBoardPictureId > 0)
                    sql1 = sql1.Where(x => search.VisionBoardPictureId == x.VisionBoardPictureId);
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

        public async Task<DbReturnListModel<VisionBoardSessionPictureView>> GetVisionBoardSessionPicturesInternalAsync(VisionBoardSessionPictureSearchCriteria search)
        {
            var sql1 = GetVisionBoardSessionPicturesInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.VisionBoardSessionPictureId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetVisionBoardSessionPicturesViewAsIQueryable().FirstOrDefaultAsync(x => x.VisionBoardSessionPictureId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<VisionBoardSessionPictureView>> GetVisionBoardSessionPicturesAsync(VisionBoardSessionPictureSearchCriteria search)
        {
            return GetVisionBoardSessionPicturesInternalAsync(search);
        }

        public Task<VisionBoardSessionPicture> GetVisionBoardSessionPictureFromAnyByIdAsync(long visionBoardSessionPictureId)
        {
            return appDBManager.FirstOrDefaultAsync<VisionBoardSessionPicture>(x => x.VisionBoardSessionPictureId == visionBoardSessionPictureId);
        }

        public Task<VisionBoardSessionPictureView> GetVisionBoardSessionPictureViewFromAnyByIdAsync(long visionBoardSessionPictureId)
        {
            return GetVisionBoardSessionPicturesViewAsIQueryable().FirstOrDefaultAsync(x => x.VisionBoardSessionPictureId == visionBoardSessionPictureId);
        }

        public Task<VisionBoardSessionPicture> GetVisionBoardSessionPictureByUniqueIdentifierAsync(string visionBoardSessionPictureId)
        {
            return appDBManager.FirstOrDefaultAsync<VisionBoardSessionPicture>(x => x.VisionBoardSessionPictureId == SafeUtils.Int(visionBoardSessionPictureId) && x.IsDeleted == false);
        }

        public Task<VisionBoardSessionPicture> GetVisionBoardSessionPictureByIdAsync(long visionBoardSessionPictureId)
        {
            return appDBManager.FirstOrDefaultAsync<VisionBoardSessionPicture>(x => x.VisionBoardSessionPictureId == visionBoardSessionPictureId && x.IsDeleted == false);
        }

        public Task<List<VisionBoardSessionPicture>> GetVisionBoardSessionPictureByIdsAsync(List<long> visionBoardSessionPictureIds)
        {
            return appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => visionBoardSessionPictureIds.Contains(x.VisionBoardSessionPictureId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<VisionBoardSessionPictureView> GetVisionBoardSessionPictureViewByIdAsync(long visionBoardSessionPictureId)
        {
            var sql1 = GetVisionBoardSessionPicturesViewAsIQueryable();

            sql1 = sql1.Where(x => x.VisionBoardSessionPictureId == visionBoardSessionPictureId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<VisionBoardSessionPicture>> GetAllVisionBoardSessionPictures()
        {
            return appDBManager.SelectAllAsync<VisionBoardSessionPicture>();
        }

        /*
        public Task<VisionBoardSessionPicture> GetVisionBoardSessionPictureByVisionBoardSessionPictureCodeAsync(long userId, string visionBoardSessionPictureCode)
        {
            return appDBManager.FirstOrDefaultAsync<VisionBoardSessionPicture>(x => x.UserId == userId && x.VisionBoardSessionPictureCode == visionBoardSessionPictureCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateVisionBoardSessionPictureCodeAsync(long userId, long visionBoardSessionPictureId, string visionBoardSessionPictureCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.UserId == userId && x.VisionBoardSessionPictureId == visionBoardSessionPictureId && x.IsDeleted == false).Set(x => x.VisionBoardSessionPictureCode, visionBoardSessionPictureCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<VisionBoardSessionPicture> CreateVisionBoardSessionPictureAsync(VisionBoardSessionPicture visionBoardSessionPicture, long? createdBy)
        {
            visionBoardSessionPicture.VisionBoardSessionPictureId = await appDBManager.InsertWithAuditAsync<VisionBoardSessionPicture, long>(visionBoardSessionPicture, createdBy);
            return visionBoardSessionPicture;
        }

        public async Task<List<VisionBoardSessionPicture>> CreateVisionBoardSessionPicturesAsync(List<VisionBoardSessionPicture> visionBoardSessionPictures, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<VisionBoardSessionPicture, long>(visionBoardSessionPictures, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                visionBoardSessionPictures[i].VisionBoardSessionPictureId = lst[i];
            return visionBoardSessionPictures;
        }

        public Task<bool> UpdateVisionBoardSessionPictureAsync(VisionBoardSessionPicture visionBoardSessionPicture, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<VisionBoardSessionPicture>(visionBoardSessionPicture, modifiedBy);
        }

        public async Task<bool> MarkVisionBoardSessionPictureAsNotDeletedByIdAsync(long visionBoardSessionPictureId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.VisionBoardSessionPictureId == visionBoardSessionPictureId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkVisionBoardSessionPictureAsDeletedByIdAsync(long visionBoardSessionPictureId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.VisionBoardSessionPictureId == visionBoardSessionPictureId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteVisionBoardSessionPictureByIdAsync(long visionBoardSessionPictureId)
        {
            return await appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.VisionBoardSessionPictureId == visionBoardSessionPictureId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetVisionBoardSessionPictureActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.VisionBoardSessionPicture);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<List<VisionBoardSessionPictureView>> GetVisionBoardSessionPictureViewByVisionBoardSessionIdAsync(long visionBoardSessionId)
        {
                return GetVisionBoardSessionPicturesViewAsIQueryable().Where(x => x.IsDeleted == false && x.VisionBoardSessionId == visionBoardSessionId).ToListAsync();
        }

        public Task<DbReturnListModel<VisionBoardSessionPictureView>> GetVisionBoardSessionPictureViewByVisionBoardSessionIdAsync(VisionBoardSessionPictureByVisionBoardSessionIdSearchCriteria search)
        {
                return GetVisionBoardSessionPicturesViewAsIQueryable().Where(x => x.IsDeleted == false && x.VisionBoardSessionId == search.VisionBoardSessionId).ReturnListModelResultAsync(search);
        }

        public Task<List<VisionBoardSessionPicture>> GetBareVisionBoardSessionPictureByVisionBoardSessionIdAsync(long visionBoardSessionId)
        {
                return appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.VisionBoardSessionId == visionBoardSessionId).ToListAsync();
        }

        public Task<DbReturnListModel<VisionBoardSessionPicture>> GetBareVisionBoardSessionPictureByVisionBoardSessionIdAsync(VisionBoardSessionPictureByVisionBoardSessionIdSearchCriteria search)
        {
                return appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.VisionBoardSessionId == search.VisionBoardSessionId).ReturnListModelResultAsync(search);
        }


        public Task<List<VisionBoardSessionView>> GetVisionBoardSessionViewByVisionBoardSessionPictureIdAsync(long visionBoardSessionPictureId)
        {
                var idsSql = appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.VisionBoardSessionPictureId == visionBoardSessionPictureId).Select(x => x.VisionBoardSessionId);
                return GetVisionBoardSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.VisionBoardSessionId)).ToListAsync();
        }

        public Task<DbReturnListModel<VisionBoardSessionView>> GetVisionBoardSessionViewByVisionBoardSessionPictureIdAsync(VisionBoardSessionByVisionBoardSessionPictureIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.VisionBoardSessionPictureId == search.VisionBoardSessionPictureId).Select(x => x.VisionBoardSessionId);
                return GetVisionBoardSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.VisionBoardSessionId)).ReturnListModelResultAsync(search);
        }

        public Task<List<VisionBoardSession>> GetBareVisionBoardSessionByVisionBoardSessionPictureIdAsync(long visionBoardSessionPictureId)
        {
                var idsSql = appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.VisionBoardSessionPictureId == visionBoardSessionPictureId).Select(x => x.VisionBoardSessionId);
                return appDBManager.GetTable<VisionBoardSession>().Where(x => x.IsDeleted == false && idsSql.Contains(x.VisionBoardSessionId)).ToListAsync();
        }

        public Task<DbReturnListModel<VisionBoardSession>> GetBareVisionBoardSessionByVisionBoardSessionPictureIdAsync(VisionBoardSessionByVisionBoardSessionPictureIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.VisionBoardSessionPictureId == search.VisionBoardSessionPictureId).Select(x => x.VisionBoardSessionId);
                return appDBManager.GetTable<VisionBoardSession>().Where(x => x.IsDeleted == false && idsSql.Contains(x.VisionBoardSessionId)).ReturnListModelResultAsync(search);
        }


        public Task<List<VisionBoardSessionPictureView>> GetVisionBoardSessionPictureViewByVisionBoardPictureIdAsync(long visionBoardPictureId)
        {
                return GetVisionBoardSessionPicturesViewAsIQueryable().Where(x => x.IsDeleted == false && x.VisionBoardPictureId == visionBoardPictureId).ToListAsync();
        }

        public Task<DbReturnListModel<VisionBoardSessionPictureView>> GetVisionBoardSessionPictureViewByVisionBoardPictureIdAsync(VisionBoardSessionPictureByVisionBoardPictureIdSearchCriteria search)
        {
                return GetVisionBoardSessionPicturesViewAsIQueryable().Where(x => x.IsDeleted == false && x.VisionBoardPictureId == search.VisionBoardPictureId).ReturnListModelResultAsync(search);
        }

        public Task<List<VisionBoardSessionPicture>> GetBareVisionBoardSessionPictureByVisionBoardPictureIdAsync(long visionBoardPictureId)
        {
                return appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.VisionBoardPictureId == visionBoardPictureId).ToListAsync();
        }

        public Task<DbReturnListModel<VisionBoardSessionPicture>> GetBareVisionBoardSessionPictureByVisionBoardPictureIdAsync(VisionBoardSessionPictureByVisionBoardPictureIdSearchCriteria search)
        {
                return appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.VisionBoardPictureId == search.VisionBoardPictureId).ReturnListModelResultAsync(search);
        }


        public Task<List<VisionBoardPictureView>> GetVisionBoardPictureViewByVisionBoardSessionPictureIdAsync(long visionBoardSessionPictureId)
        {
                var idsSql = appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.VisionBoardSessionPictureId == visionBoardSessionPictureId).Select(x => x.VisionBoardPictureId);
                return GetVisionBoardPicturesViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.VisionBoardPictureId)).ToListAsync();
        }

        public Task<DbReturnListModel<VisionBoardPictureView>> GetVisionBoardPictureViewByVisionBoardSessionPictureIdAsync(VisionBoardPictureByVisionBoardSessionPictureIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.VisionBoardSessionPictureId == search.VisionBoardSessionPictureId).Select(x => x.VisionBoardPictureId);
                return GetVisionBoardPicturesViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.VisionBoardPictureId)).ReturnListModelResultAsync(search);
        }

        public Task<List<VisionBoardPicture>> GetBareVisionBoardPictureByVisionBoardSessionPictureIdAsync(long visionBoardSessionPictureId)
        {
                var idsSql = appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.VisionBoardSessionPictureId == visionBoardSessionPictureId).Select(x => x.VisionBoardPictureId);
                return appDBManager.GetTable<VisionBoardPicture>().Where(x => x.IsDeleted == false && idsSql.Contains(x.VisionBoardPictureId)).ToListAsync();
        }

        public Task<DbReturnListModel<VisionBoardPicture>> GetBareVisionBoardPictureByVisionBoardSessionPictureIdAsync(VisionBoardPictureByVisionBoardSessionPictureIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.VisionBoardSessionPictureId == search.VisionBoardSessionPictureId).Select(x => x.VisionBoardPictureId);
                return appDBManager.GetTable<VisionBoardPicture>().Where(x => x.IsDeleted == false && idsSql.Contains(x.VisionBoardPictureId)).ReturnListModelResultAsync(search);
        }


        public Task<List<VisionBoardSessionPictureView>> GetVisionBoardSessionPictureViewByUserIdAsync(long userId)
        {
                return GetVisionBoardSessionPicturesViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<VisionBoardSessionPictureView>> GetVisionBoardSessionPictureViewByUserIdAsync(VisionBoardSessionPictureByUserIdSearchCriteria search)
        {
                return GetVisionBoardSessionPicturesViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }

        public Task<List<VisionBoardSessionPicture>> GetBareVisionBoardSessionPictureByUserIdAsync(long userId)
        {
                return appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<VisionBoardSessionPicture>> GetBareVisionBoardSessionPictureByUserIdAsync(VisionBoardSessionPictureByUserIdSearchCriteria search)
        {
                return appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }


        public Task<List<UserView>> GetUserViewByVisionBoardSessionPictureIdAsync(long visionBoardSessionPictureId)
        {
                var idsSql = appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.VisionBoardSessionPictureId == visionBoardSessionPictureId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<UserView>> GetUserViewByVisionBoardSessionPictureIdAsync(UserByVisionBoardSessionPictureIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.VisionBoardSessionPictureId == search.VisionBoardSessionPictureId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }

        public Task<List<User>> GetBareUserByVisionBoardSessionPictureIdAsync(long visionBoardSessionPictureId)
        {
                var idsSql = appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.VisionBoardSessionPictureId == visionBoardSessionPictureId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<User>> GetBareUserByVisionBoardSessionPictureIdAsync(UserByVisionBoardSessionPictureIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.IsDeleted == false && x.VisionBoardSessionPictureId == search.VisionBoardSessionPictureId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }



        /*

        public async Task<bool> UpdateVisionBoardSessionPicturePicturePathAsync(long visionBoardSessionPictureId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.VisionBoardSessionPictureId == visionBoardSessionPictureId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearVisionBoardSessionPictureProfilePathAsync(long visionBoardSessionPictureId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.VisionBoardSessionPictureId == visionBoardSessionPictureId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableVisionBoardSessionPictureByIdAsync(long visionBoardSessionPictureId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.VisionBoardSessionPictureId == visionBoardSessionPictureId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableVisionBoardSessionPictureByIdAsync(long visionBoardSessionPictureId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardSessionPicture>().Where(x => x.VisionBoardSessionPictureId == visionBoardSessionPictureId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateVisionBoardSessionPictureWithXYZZYXAsync(long visionBoardSessionPictureId, long xYZZYXId, long? createdBy)
        {
            var item = new VisionBoardSessionPictureXYZZYXAssociationModel();
            item.VisionBoardSessionPictureId = visionBoardSessionPictureId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<VisionBoardSessionPictureXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateVisionBoardSessionPictureWithXYZZYXAsync(long visionBoardSessionPictureId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<VisionBoardSessionPictureXYZZYX>(x => x.VisionBoardSessionPictureId == visionBoardSessionPictureId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesVisionBoardSessionPictureWithXYZZYXAssociationExistsAsync(long visionBoardSessionPictureId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<VisionBoardSessionPictureXYZZYX>(x => x.VisionBoardSessionPictureId == visionBoardSessionPictureId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<VisionBoardSessionPictureView>> GetVisionBoardSessionPicturesByXYZZYXAsync(long xYZZYXId)
        {
            var visionBoardSessionPictureIds = GetVisionBoardSessionPictureXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.VisionBoardSessionPictureId);
            var visionBoardSessionPictureSql = GetVisionBoardSessionPicturesViewAsIQueryable().Where(x => visionBoardSessionPictureIds.Contains(x.VisionBoardSessionPictureId));
            return visionBoardSessionPictureSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByVisionBoardSessionPicturesAsync(long visionBoardSessionPictureId)
        {
            var xYZZYXIds = GetVisionBoardSessionPictureXYZZYXsViewAsIQueryable().Where(x => x.VisionBoardSessionPictureId == visionBoardSessionPicture).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<VisionBoardSessionPictureXYZZYX> GetVisionBoardSessionPictureXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<VisionBoardSessionPictureXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetVisionBoardSessionPictureXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.VisionBoardSessionPictureXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region Category

        public IQueryable<CategoryView> GetCategorysViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<Category>()
                       join a in appDBManager.GetTable<DefaultCategory>() on item.DefaultCategoryId equals a.DefaultCategoryId
                   join ob in appDBManager.GetTable<EnumType>() on item.PredefinedCategoryTypeId equals ob.EnumTypeId into bTemp
                   from b in bTemp.DefaultIfEmpty()
                   join c in appDBManager.GetTable<User>() on item.UserId equals c.UserId

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new CategoryView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       DefaultCategoryName = a.DefaultCategoryName,
                       PredefinedCategoryTypeName = b.EnumTypeName,
                       UserName = c.UserName,
                       ColorAsInteger = item.ColorAsInteger,
                       PredefinedIconTypeId = item.PredefinedIconTypeId,
                       CategoryId = item.CategoryId,
                       DefaultCategoryId = item.DefaultCategoryId,
                       CategoryName = item.CategoryName,
                       PredefinedCategoryTypeId = item.PredefinedCategoryTypeId,
                       UserId = item.UserId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<CategoryView> GetCategorysInternalWithSearchCriteriaAsync(CategorySearchCriteria search)
        {
            var sql1 = GetCategorysViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.CategoryId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.CategoryName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
);
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

    if (search.DefaultCategoryId != null && search.DefaultCategoryId > 0)
                    sql1 = sql1.Where(x => search.DefaultCategoryId == x.DefaultCategoryId);
                if (search.PredefinedCategoryTypeIds != null && search.PredefinedCategoryTypeIds.Count > 0)
                    sql1 = sql1.Where(x => search.PredefinedCategoryTypeIds.Contains(x.PredefinedCategoryTypeId ?? 0));
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

        public async Task<DbReturnListModel<CategoryView>> GetCategorysInternalAsync(CategorySearchCriteria search)
        {
            var sql1 = GetCategorysInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.CategoryId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetCategorysViewAsIQueryable().FirstOrDefaultAsync(x => x.CategoryId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<CategoryView>> GetCategorysAsync(CategorySearchCriteria search)
        {
            return GetCategorysInternalAsync(search);
        }

        public Task<Category> GetCategoryFromAnyByIdAsync(long categoryId)
        {
            return appDBManager.FirstOrDefaultAsync<Category>(x => x.CategoryId == categoryId);
        }

        public Task<CategoryView> GetCategoryViewFromAnyByIdAsync(long categoryId)
        {
            return GetCategorysViewAsIQueryable().FirstOrDefaultAsync(x => x.CategoryId == categoryId);
        }

        public Task<Category> GetCategoryByUniqueIdentifierAsync(string categoryId)
        {
            return appDBManager.FirstOrDefaultAsync<Category>(x => x.CategoryId == SafeUtils.Int(categoryId) && x.IsDeleted == false);
        }

        public Task<Category> GetCategoryByIdAsync(long categoryId)
        {
            return appDBManager.FirstOrDefaultAsync<Category>(x => x.CategoryId == categoryId && x.IsDeleted == false);
        }

        public Task<List<Category>> GetCategoryByIdsAsync(List<long> categoryIds)
        {
            return appDBManager.GetTable<Category>().Where(x => categoryIds.Contains(x.CategoryId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<CategoryView> GetCategoryViewByIdAsync(long categoryId)
        {
            var sql1 = GetCategorysViewAsIQueryable();

            sql1 = sql1.Where(x => x.CategoryId == categoryId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<Category>> GetAllCategorys()
        {
            return appDBManager.SelectAllAsync<Category>();
        }

        /*
        public Task<Category> GetCategoryByCategoryCodeAsync(long userId, string categoryCode)
        {
            return appDBManager.FirstOrDefaultAsync<Category>(x => x.UserId == userId && x.CategoryCode == categoryCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateCategoryCodeAsync(long userId, long categoryId, string categoryCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<Category>().Where(x => x.UserId == userId && x.CategoryId == categoryId && x.IsDeleted == false).Set(x => x.CategoryCode, categoryCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<Category> CreateCategoryAsync(Category category, long? createdBy)
        {
            category.CategoryId = await appDBManager.InsertWithAuditAsync<Category, long>(category, createdBy);
            return category;
        }

        public async Task<List<Category>> CreateCategorysAsync(List<Category> categorys, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<Category, long>(categorys, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                categorys[i].CategoryId = lst[i];
            return categorys;
        }

        public Task<bool> UpdateCategoryAsync(Category category, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<Category>(category, modifiedBy);
        }

        public async Task<bool> MarkCategoryAsNotDeletedByIdAsync(long categoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Category>().Where(x => x.CategoryId == categoryId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkCategoryAsDeletedByIdAsync(long categoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Category>().Where(x => x.CategoryId == categoryId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteCategoryByIdAsync(long categoryId)
        {
            return await appDBManager.GetTable<Category>().Where(x => x.CategoryId == categoryId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetCategoryActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.Category);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsCategoryNameAlreadyExistsForOtherCategoryAsync(long categoryId, string categoryName, long userId)
        {
            var sql = GetCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.CategoryId != categoryId && x.CategoryName.ToLower().Trim() == categoryName.ToLower().Trim() && x.UserId == userId);
            return sql.AnyAsync();
        }

        public Task<bool> IsCategoryNameAlreadyExistsForOtherCategoryAsync(string categoryName, long userId)
        {
            var sql = GetCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.CategoryName.ToLower().Trim() == categoryName.ToLower().Trim() && x.UserId == userId);
            return sql.AnyAsync();
        }

        

        public Task<List<CategoryView>> GetCategoryViewByDefaultCategoryIdAsync(long defaultCategoryId)
        {
                return GetCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultCategoryId == defaultCategoryId).ToListAsync();
        }

        public Task<DbReturnListModel<CategoryView>> GetCategoryViewByDefaultCategoryIdAsync(CategoryByDefaultCategoryIdSearchCriteria search)
        {
                return GetCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultCategoryId == search.DefaultCategoryId).ReturnListModelResultAsync(search);
        }

        public Task<List<Category>> GetBareCategoryByDefaultCategoryIdAsync(long defaultCategoryId)
        {
                return appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && x.DefaultCategoryId == defaultCategoryId).ToListAsync();
        }

        public Task<DbReturnListModel<Category>> GetBareCategoryByDefaultCategoryIdAsync(CategoryByDefaultCategoryIdSearchCriteria search)
        {
                return appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && x.DefaultCategoryId == search.DefaultCategoryId).ReturnListModelResultAsync(search);
        }


        public Task<List<DefaultCategoryView>> GetDefaultCategoryViewByCategoryIdAsync(long categoryId)
        {
                var idsSql = appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && x.CategoryId == categoryId).Select(x => x.DefaultCategoryId);
                return GetDefaultCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.DefaultCategoryId)).ToListAsync();
        }

        public Task<DbReturnListModel<DefaultCategoryView>> GetDefaultCategoryViewByCategoryIdAsync(DefaultCategoryByCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && x.CategoryId == search.CategoryId).Select(x => x.DefaultCategoryId);
                return GetDefaultCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.DefaultCategoryId)).ReturnListModelResultAsync(search);
        }

        public Task<List<DefaultCategory>> GetBareDefaultCategoryByCategoryIdAsync(long categoryId)
        {
                var idsSql = appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && x.CategoryId == categoryId).Select(x => x.DefaultCategoryId);
                return appDBManager.GetTable<DefaultCategory>().Where(x => x.IsDeleted == false && idsSql.Contains(x.DefaultCategoryId)).ToListAsync();
        }

        public Task<DbReturnListModel<DefaultCategory>> GetBareDefaultCategoryByCategoryIdAsync(DefaultCategoryByCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && x.CategoryId == search.CategoryId).Select(x => x.DefaultCategoryId);
                return appDBManager.GetTable<DefaultCategory>().Where(x => x.IsDeleted == false && idsSql.Contains(x.DefaultCategoryId)).ReturnListModelResultAsync(search);
        }


        public Task<List<CategoryView>> GetCategoryViewByUserIdAsync(long userId)
        {
                return GetCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<CategoryView>> GetCategoryViewByUserIdAsync(CategoryByUserIdSearchCriteria search)
        {
                return GetCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }

        public Task<List<Category>> GetBareCategoryByUserIdAsync(long userId)
        {
                return appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<Category>> GetBareCategoryByUserIdAsync(CategoryByUserIdSearchCriteria search)
        {
                return appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }


        public Task<List<UserView>> GetUserViewByCategoryIdAsync(long categoryId)
        {
                var idsSql = appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && x.CategoryId == categoryId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<UserView>> GetUserViewByCategoryIdAsync(UserByCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && x.CategoryId == search.CategoryId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }

        public Task<List<User>> GetBareUserByCategoryIdAsync(long categoryId)
        {
                var idsSql = appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && x.CategoryId == categoryId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<User>> GetBareUserByCategoryIdAsync(UserByCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && x.CategoryId == search.CategoryId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }



        /*

        public async Task<bool> UpdateCategoryPicturePathAsync(long categoryId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<Category>().Where(x => x.CategoryId == categoryId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearCategoryProfilePathAsync(long categoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Category>().Where(x => x.CategoryId == categoryId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableCategoryByIdAsync(long categoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Category>().Where(x => x.CategoryId == categoryId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableCategoryByIdAsync(long categoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Category>().Where(x => x.CategoryId == categoryId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateCategoryWithXYZZYXAsync(long categoryId, long xYZZYXId, long? createdBy)
        {
            var item = new CategoryXYZZYXAssociationModel();
            item.CategoryId = categoryId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<CategoryXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateCategoryWithXYZZYXAsync(long categoryId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<CategoryXYZZYX>(x => x.CategoryId == categoryId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesCategoryWithXYZZYXAssociationExistsAsync(long categoryId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<CategoryXYZZYX>(x => x.CategoryId == categoryId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<CategoryView>> GetCategorysByXYZZYXAsync(long xYZZYXId)
        {
            var categoryIds = GetCategoryXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.CategoryId);
            var categorySql = GetCategorysViewAsIQueryable().Where(x => categoryIds.Contains(x.CategoryId));
            return categorySql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByCategorysAsync(long categoryId)
        {
            var xYZZYXIds = GetCategoryXYZZYXsViewAsIQueryable().Where(x => x.CategoryId == category).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<CategoryXYZZYX> GetCategoryXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<CategoryXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetCategoryXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.CategoryXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region DailyAffirmationSession

        public IQueryable<DailyAffirmationSessionView> GetDailyAffirmationSessionsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DailyAffirmationSession>()
                       join a in appDBManager.GetTable<User>() on item.UserId equals a.UserId

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new DailyAffirmationSessionView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       UserName = a.UserName,

                       DailyAffirmationSessionId = item.DailyAffirmationSessionId,
                       DailyAffirmationSessionDate = item.DailyAffirmationSessionDate,
                       DailyAffirmationSessionTimeSpentInSeconds = item.DailyAffirmationSessionTimeSpentInSeconds,
                       UserId = item.UserId,
                       AddedToFeed = item.AddedToFeed,
                       AddedToFeedDate = item.AddedToFeedDate,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<DailyAffirmationSessionView> GetDailyAffirmationSessionsInternalWithSearchCriteriaAsync(DailyAffirmationSessionSearchCriteria search)
        {
            var sql1 = GetDailyAffirmationSessionsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.DailyAffirmationSessionId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                );
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

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

        public async Task<DbReturnListModel<DailyAffirmationSessionView>> GetDailyAffirmationSessionsInternalAsync(DailyAffirmationSessionSearchCriteria search)
        {
            var sql1 = GetDailyAffirmationSessionsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.DailyAffirmationSessionId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetDailyAffirmationSessionsViewAsIQueryable().FirstOrDefaultAsync(x => x.DailyAffirmationSessionId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<DailyAffirmationSessionView>> GetDailyAffirmationSessionsAsync(DailyAffirmationSessionSearchCriteria search)
        {
            return GetDailyAffirmationSessionsInternalAsync(search);
        }

        public Task<DailyAffirmationSession> GetDailyAffirmationSessionFromAnyByIdAsync(long dailyAffirmationSessionId)
        {
            return appDBManager.FirstOrDefaultAsync<DailyAffirmationSession>(x => x.DailyAffirmationSessionId == dailyAffirmationSessionId);
        }

        public Task<DailyAffirmationSessionView> GetDailyAffirmationSessionViewFromAnyByIdAsync(long dailyAffirmationSessionId)
        {
            return GetDailyAffirmationSessionsViewAsIQueryable().FirstOrDefaultAsync(x => x.DailyAffirmationSessionId == dailyAffirmationSessionId);
        }

        public Task<DailyAffirmationSession> GetDailyAffirmationSessionByUniqueIdentifierAsync(string dailyAffirmationSessionId)
        {
            return appDBManager.FirstOrDefaultAsync<DailyAffirmationSession>(x => x.DailyAffirmationSessionId == SafeUtils.Int(dailyAffirmationSessionId) && x.IsDeleted == false);
        }

        public Task<DailyAffirmationSession> GetDailyAffirmationSessionByIdAsync(long dailyAffirmationSessionId)
        {
            return appDBManager.FirstOrDefaultAsync<DailyAffirmationSession>(x => x.DailyAffirmationSessionId == dailyAffirmationSessionId && x.IsDeleted == false);
        }

        public Task<List<DailyAffirmationSession>> GetDailyAffirmationSessionByIdsAsync(List<long> dailyAffirmationSessionIds)
        {
            return appDBManager.GetTable<DailyAffirmationSession>().Where(x => dailyAffirmationSessionIds.Contains(x.DailyAffirmationSessionId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<DailyAffirmationSessionView> GetDailyAffirmationSessionViewByIdAsync(long dailyAffirmationSessionId)
        {
            var sql1 = GetDailyAffirmationSessionsViewAsIQueryable();

            sql1 = sql1.Where(x => x.DailyAffirmationSessionId == dailyAffirmationSessionId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<DailyAffirmationSession>> GetAllDailyAffirmationSessions()
        {
            return appDBManager.SelectAllAsync<DailyAffirmationSession>();
        }

        /*
        public Task<DailyAffirmationSession> GetDailyAffirmationSessionByDailyAffirmationSessionCodeAsync(long userId, string dailyAffirmationSessionCode)
        {
            return appDBManager.FirstOrDefaultAsync<DailyAffirmationSession>(x => x.UserId == userId && x.DailyAffirmationSessionCode == dailyAffirmationSessionCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateDailyAffirmationSessionCodeAsync(long userId, long dailyAffirmationSessionId, string dailyAffirmationSessionCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.UserId == userId && x.DailyAffirmationSessionId == dailyAffirmationSessionId && x.IsDeleted == false).Set(x => x.DailyAffirmationSessionCode, dailyAffirmationSessionCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<DailyAffirmationSession> CreateDailyAffirmationSessionAsync(DailyAffirmationSession dailyAffirmationSession, long? createdBy)
        {
            dailyAffirmationSession.DailyAffirmationSessionId = await appDBManager.InsertWithAuditAsync<DailyAffirmationSession, long>(dailyAffirmationSession, createdBy);
            return dailyAffirmationSession;
        }

        public async Task<List<DailyAffirmationSession>> CreateDailyAffirmationSessionsAsync(List<DailyAffirmationSession> dailyAffirmationSessions, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<DailyAffirmationSession, long>(dailyAffirmationSessions, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                dailyAffirmationSessions[i].DailyAffirmationSessionId = lst[i];
            return dailyAffirmationSessions;
        }

        public Task<bool> UpdateDailyAffirmationSessionAsync(DailyAffirmationSession dailyAffirmationSession, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<DailyAffirmationSession>(dailyAffirmationSession, modifiedBy);
        }

        public async Task<bool> MarkDailyAffirmationSessionAsNotDeletedByIdAsync(long dailyAffirmationSessionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.DailyAffirmationSessionId == dailyAffirmationSessionId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkDailyAffirmationSessionAsDeletedByIdAsync(long dailyAffirmationSessionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.DailyAffirmationSessionId == dailyAffirmationSessionId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteDailyAffirmationSessionByIdAsync(long dailyAffirmationSessionId)
        {
            return await appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.DailyAffirmationSessionId == dailyAffirmationSessionId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetDailyAffirmationSessionActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.DailyAffirmationSession);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<List<DailyAffirmationSessionView>> GetDailyAffirmationSessionViewByUserIdAsync(long userId)
        {
                return GetDailyAffirmationSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<DailyAffirmationSessionView>> GetDailyAffirmationSessionViewByUserIdAsync(DailyAffirmationSessionByUserIdSearchCriteria search)
        {
                return GetDailyAffirmationSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }

        public Task<List<DailyAffirmationSession>> GetBareDailyAffirmationSessionByUserIdAsync(long userId)
        {
                return appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<DailyAffirmationSession>> GetBareDailyAffirmationSessionByUserIdAsync(DailyAffirmationSessionByUserIdSearchCriteria search)
        {
                return appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }


        public Task<List<UserView>> GetUserViewByDailyAffirmationSessionIdAsync(long dailyAffirmationSessionId)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionId == dailyAffirmationSessionId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<UserView>> GetUserViewByDailyAffirmationSessionIdAsync(UserByDailyAffirmationSessionIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionId == search.DailyAffirmationSessionId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }

        public Task<List<User>> GetBareUserByDailyAffirmationSessionIdAsync(long dailyAffirmationSessionId)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionId == dailyAffirmationSessionId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<User>> GetBareUserByDailyAffirmationSessionIdAsync(UserByDailyAffirmationSessionIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionId == search.DailyAffirmationSessionId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }



        /*

        public async Task<bool> UpdateDailyAffirmationSessionPicturePathAsync(long dailyAffirmationSessionId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.DailyAffirmationSessionId == dailyAffirmationSessionId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearDailyAffirmationSessionProfilePathAsync(long dailyAffirmationSessionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.DailyAffirmationSessionId == dailyAffirmationSessionId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableDailyAffirmationSessionByIdAsync(long dailyAffirmationSessionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.DailyAffirmationSessionId == dailyAffirmationSessionId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableDailyAffirmationSessionByIdAsync(long dailyAffirmationSessionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.DailyAffirmationSessionId == dailyAffirmationSessionId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateDailyAffirmationSessionWithXYZZYXAsync(long dailyAffirmationSessionId, long xYZZYXId, long? createdBy)
        {
            var item = new DailyAffirmationSessionXYZZYXAssociationModel();
            item.DailyAffirmationSessionId = dailyAffirmationSessionId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<DailyAffirmationSessionXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateDailyAffirmationSessionWithXYZZYXAsync(long dailyAffirmationSessionId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<DailyAffirmationSessionXYZZYX>(x => x.DailyAffirmationSessionId == dailyAffirmationSessionId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesDailyAffirmationSessionWithXYZZYXAssociationExistsAsync(long dailyAffirmationSessionId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<DailyAffirmationSessionXYZZYX>(x => x.DailyAffirmationSessionId == dailyAffirmationSessionId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<DailyAffirmationSessionView>> GetDailyAffirmationSessionsByXYZZYXAsync(long xYZZYXId)
        {
            var dailyAffirmationSessionIds = GetDailyAffirmationSessionXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.DailyAffirmationSessionId);
            var dailyAffirmationSessionSql = GetDailyAffirmationSessionsViewAsIQueryable().Where(x => dailyAffirmationSessionIds.Contains(x.DailyAffirmationSessionId));
            return dailyAffirmationSessionSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByDailyAffirmationSessionsAsync(long dailyAffirmationSessionId)
        {
            var xYZZYXIds = GetDailyAffirmationSessionXYZZYXsViewAsIQueryable().Where(x => x.DailyAffirmationSessionId == dailyAffirmationSession).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<DailyAffirmationSessionXYZZYX> GetDailyAffirmationSessionXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DailyAffirmationSessionXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetDailyAffirmationSessionXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.DailyAffirmationSessionXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region DailyAffirmationText

        public IQueryable<DailyAffirmationTextView> GetDailyAffirmationTextsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DailyAffirmationText>()
                       join a in appDBManager.GetTable<Category>() on item.CategoryId equals a.CategoryId
                   join b in appDBManager.GetTable<User>() on item.UserId equals b.UserId

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new DailyAffirmationTextView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       CategoryName = a.CategoryName,
                       UserName = b.UserName,

                       DailyAffirmationTextId = item.DailyAffirmationTextId,
                       DailyAffirmationTextValue = item.DailyAffirmationTextValue,
                       CategoryId = item.CategoryId,
                       UserId = item.UserId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<DailyAffirmationTextView> GetDailyAffirmationTextsInternalWithSearchCriteriaAsync(DailyAffirmationTextSearchCriteria search)
        {
            var sql1 = GetDailyAffirmationTextsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.DailyAffirmationTextId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.DailyAffirmationTextValue.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
);
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

    if (search.CategoryId != null && search.CategoryId > 0)
                    sql1 = sql1.Where(x => search.CategoryId == x.CategoryId);
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

        public async Task<DbReturnListModel<DailyAffirmationTextView>> GetDailyAffirmationTextsInternalAsync(DailyAffirmationTextSearchCriteria search)
        {
            var sql1 = GetDailyAffirmationTextsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.DailyAffirmationTextId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetDailyAffirmationTextsViewAsIQueryable().FirstOrDefaultAsync(x => x.DailyAffirmationTextId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<DailyAffirmationTextView>> GetDailyAffirmationTextsAsync(DailyAffirmationTextSearchCriteria search)
        {
            return GetDailyAffirmationTextsInternalAsync(search);
        }

        public Task<DailyAffirmationText> GetDailyAffirmationTextFromAnyByIdAsync(long dailyAffirmationTextId)
        {
            return appDBManager.FirstOrDefaultAsync<DailyAffirmationText>(x => x.DailyAffirmationTextId == dailyAffirmationTextId);
        }

        public Task<DailyAffirmationTextView> GetDailyAffirmationTextViewFromAnyByIdAsync(long dailyAffirmationTextId)
        {
            return GetDailyAffirmationTextsViewAsIQueryable().FirstOrDefaultAsync(x => x.DailyAffirmationTextId == dailyAffirmationTextId);
        }

        public Task<DailyAffirmationText> GetDailyAffirmationTextByUniqueIdentifierAsync(string dailyAffirmationTextId)
        {
            return appDBManager.FirstOrDefaultAsync<DailyAffirmationText>(x => x.DailyAffirmationTextId == SafeUtils.Int(dailyAffirmationTextId) && x.IsDeleted == false);
        }

        public Task<DailyAffirmationText> GetDailyAffirmationTextByIdAsync(long dailyAffirmationTextId)
        {
            return appDBManager.FirstOrDefaultAsync<DailyAffirmationText>(x => x.DailyAffirmationTextId == dailyAffirmationTextId && x.IsDeleted == false);
        }

        public Task<List<DailyAffirmationText>> GetDailyAffirmationTextByIdsAsync(List<long> dailyAffirmationTextIds)
        {
            return appDBManager.GetTable<DailyAffirmationText>().Where(x => dailyAffirmationTextIds.Contains(x.DailyAffirmationTextId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<DailyAffirmationTextView> GetDailyAffirmationTextViewByIdAsync(long dailyAffirmationTextId)
        {
            var sql1 = GetDailyAffirmationTextsViewAsIQueryable();

            sql1 = sql1.Where(x => x.DailyAffirmationTextId == dailyAffirmationTextId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<DailyAffirmationText>> GetAllDailyAffirmationTexts()
        {
            return appDBManager.SelectAllAsync<DailyAffirmationText>();
        }

        /*
        public Task<DailyAffirmationText> GetDailyAffirmationTextByDailyAffirmationTextCodeAsync(long userId, string dailyAffirmationTextCode)
        {
            return appDBManager.FirstOrDefaultAsync<DailyAffirmationText>(x => x.UserId == userId && x.DailyAffirmationTextCode == dailyAffirmationTextCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateDailyAffirmationTextCodeAsync(long userId, long dailyAffirmationTextId, string dailyAffirmationTextCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationText>().Where(x => x.UserId == userId && x.DailyAffirmationTextId == dailyAffirmationTextId && x.IsDeleted == false).Set(x => x.DailyAffirmationTextCode, dailyAffirmationTextCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<DailyAffirmationText> CreateDailyAffirmationTextAsync(DailyAffirmationText dailyAffirmationText, long? createdBy)
        {
            dailyAffirmationText.DailyAffirmationTextId = await appDBManager.InsertWithAuditAsync<DailyAffirmationText, long>(dailyAffirmationText, createdBy);
            return dailyAffirmationText;
        }

        public async Task<List<DailyAffirmationText>> CreateDailyAffirmationTextsAsync(List<DailyAffirmationText> dailyAffirmationTexts, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<DailyAffirmationText, long>(dailyAffirmationTexts, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                dailyAffirmationTexts[i].DailyAffirmationTextId = lst[i];
            return dailyAffirmationTexts;
        }

        public Task<bool> UpdateDailyAffirmationTextAsync(DailyAffirmationText dailyAffirmationText, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<DailyAffirmationText>(dailyAffirmationText, modifiedBy);
        }

        public async Task<bool> MarkDailyAffirmationTextAsNotDeletedByIdAsync(long dailyAffirmationTextId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationText>().Where(x => x.DailyAffirmationTextId == dailyAffirmationTextId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkDailyAffirmationTextAsDeletedByIdAsync(long dailyAffirmationTextId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationText>().Where(x => x.DailyAffirmationTextId == dailyAffirmationTextId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteDailyAffirmationTextByIdAsync(long dailyAffirmationTextId)
        {
            return await appDBManager.GetTable<DailyAffirmationText>().Where(x => x.DailyAffirmationTextId == dailyAffirmationTextId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetDailyAffirmationTextActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.DailyAffirmationText);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsDailyAffirmationTextNameAlreadyExistsForOtherDailyAffirmationTextAsync(long dailyAffirmationTextId, string dailyAffirmationTextName, long userId)
        {
            var sql = GetDailyAffirmationTextsViewAsIQueryable().Where(x => x.IsDeleted == false && x.DailyAffirmationTextId != dailyAffirmationTextId && x.DailyAffirmationTextValue.ToLower().Trim() == dailyAffirmationTextName.ToLower().Trim() && x.UserId == userId);
            return sql.AnyAsync();
        }

        public Task<bool> IsDailyAffirmationTextNameAlreadyExistsForOtherDailyAffirmationTextAsync(string dailyAffirmationTextName, long userId)
        {
            var sql = GetDailyAffirmationTextsViewAsIQueryable().Where(x => x.IsDeleted == false && x.DailyAffirmationTextValue.ToLower().Trim() == dailyAffirmationTextName.ToLower().Trim() && x.UserId == userId);
            return sql.AnyAsync();
        }

        

        public Task<List<DailyAffirmationTextView>> GetDailyAffirmationTextViewByCategoryIdAsync(long categoryId)
        {
                return GetDailyAffirmationTextsViewAsIQueryable().Where(x => x.IsDeleted == false && x.CategoryId == categoryId).ToListAsync();
        }

        public Task<DbReturnListModel<DailyAffirmationTextView>> GetDailyAffirmationTextViewByCategoryIdAsync(DailyAffirmationTextByCategoryIdSearchCriteria search)
        {
                return GetDailyAffirmationTextsViewAsIQueryable().Where(x => x.IsDeleted == false && x.CategoryId == search.CategoryId).ReturnListModelResultAsync(search);
        }

        public Task<List<DailyAffirmationText>> GetBareDailyAffirmationTextByCategoryIdAsync(long categoryId)
        {
                return appDBManager.GetTable<DailyAffirmationText>().Where(x => x.IsDeleted == false && x.CategoryId == categoryId).ToListAsync();
        }

        public Task<DbReturnListModel<DailyAffirmationText>> GetBareDailyAffirmationTextByCategoryIdAsync(DailyAffirmationTextByCategoryIdSearchCriteria search)
        {
                return appDBManager.GetTable<DailyAffirmationText>().Where(x => x.IsDeleted == false && x.CategoryId == search.CategoryId).ReturnListModelResultAsync(search);
        }


        public Task<List<CategoryView>> GetCategoryViewByDailyAffirmationTextIdAsync(long dailyAffirmationTextId)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationText>().Where(x => x.IsDeleted == false && x.DailyAffirmationTextId == dailyAffirmationTextId).Select(x => x.CategoryId);
                return GetCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.CategoryId)).ToListAsync();
        }

        public Task<DbReturnListModel<CategoryView>> GetCategoryViewByDailyAffirmationTextIdAsync(CategoryByDailyAffirmationTextIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationText>().Where(x => x.IsDeleted == false && x.DailyAffirmationTextId == search.DailyAffirmationTextId).Select(x => x.CategoryId);
                return GetCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.CategoryId)).ReturnListModelResultAsync(search);
        }

        public Task<List<Category>> GetBareCategoryByDailyAffirmationTextIdAsync(long dailyAffirmationTextId)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationText>().Where(x => x.IsDeleted == false && x.DailyAffirmationTextId == dailyAffirmationTextId).Select(x => x.CategoryId);
                return appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && idsSql.Contains(x.CategoryId)).ToListAsync();
        }

        public Task<DbReturnListModel<Category>> GetBareCategoryByDailyAffirmationTextIdAsync(CategoryByDailyAffirmationTextIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationText>().Where(x => x.IsDeleted == false && x.DailyAffirmationTextId == search.DailyAffirmationTextId).Select(x => x.CategoryId);
                return appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && idsSql.Contains(x.CategoryId)).ReturnListModelResultAsync(search);
        }


        public Task<List<DailyAffirmationTextView>> GetDailyAffirmationTextViewByUserIdAsync(long userId)
        {
                return GetDailyAffirmationTextsViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<DailyAffirmationTextView>> GetDailyAffirmationTextViewByUserIdAsync(DailyAffirmationTextByUserIdSearchCriteria search)
        {
                return GetDailyAffirmationTextsViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }

        public Task<List<DailyAffirmationText>> GetBareDailyAffirmationTextByUserIdAsync(long userId)
        {
                return appDBManager.GetTable<DailyAffirmationText>().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<DailyAffirmationText>> GetBareDailyAffirmationTextByUserIdAsync(DailyAffirmationTextByUserIdSearchCriteria search)
        {
                return appDBManager.GetTable<DailyAffirmationText>().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }


        public Task<List<UserView>> GetUserViewByDailyAffirmationTextIdAsync(long dailyAffirmationTextId)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationText>().Where(x => x.IsDeleted == false && x.DailyAffirmationTextId == dailyAffirmationTextId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<UserView>> GetUserViewByDailyAffirmationTextIdAsync(UserByDailyAffirmationTextIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationText>().Where(x => x.IsDeleted == false && x.DailyAffirmationTextId == search.DailyAffirmationTextId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }

        public Task<List<User>> GetBareUserByDailyAffirmationTextIdAsync(long dailyAffirmationTextId)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationText>().Where(x => x.IsDeleted == false && x.DailyAffirmationTextId == dailyAffirmationTextId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<User>> GetBareUserByDailyAffirmationTextIdAsync(UserByDailyAffirmationTextIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationText>().Where(x => x.IsDeleted == false && x.DailyAffirmationTextId == search.DailyAffirmationTextId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }



        /*

        public async Task<bool> UpdateDailyAffirmationTextPicturePathAsync(long dailyAffirmationTextId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationText>().Where(x => x.DailyAffirmationTextId == dailyAffirmationTextId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearDailyAffirmationTextProfilePathAsync(long dailyAffirmationTextId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationText>().Where(x => x.DailyAffirmationTextId == dailyAffirmationTextId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableDailyAffirmationTextByIdAsync(long dailyAffirmationTextId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationText>().Where(x => x.DailyAffirmationTextId == dailyAffirmationTextId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableDailyAffirmationTextByIdAsync(long dailyAffirmationTextId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationText>().Where(x => x.DailyAffirmationTextId == dailyAffirmationTextId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateDailyAffirmationTextWithXYZZYXAsync(long dailyAffirmationTextId, long xYZZYXId, long? createdBy)
        {
            var item = new DailyAffirmationTextXYZZYXAssociationModel();
            item.DailyAffirmationTextId = dailyAffirmationTextId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<DailyAffirmationTextXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateDailyAffirmationTextWithXYZZYXAsync(long dailyAffirmationTextId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<DailyAffirmationTextXYZZYX>(x => x.DailyAffirmationTextId == dailyAffirmationTextId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesDailyAffirmationTextWithXYZZYXAssociationExistsAsync(long dailyAffirmationTextId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<DailyAffirmationTextXYZZYX>(x => x.DailyAffirmationTextId == dailyAffirmationTextId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<DailyAffirmationTextView>> GetDailyAffirmationTextsByXYZZYXAsync(long xYZZYXId)
        {
            var dailyAffirmationTextIds = GetDailyAffirmationTextXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.DailyAffirmationTextId);
            var dailyAffirmationTextSql = GetDailyAffirmationTextsViewAsIQueryable().Where(x => dailyAffirmationTextIds.Contains(x.DailyAffirmationTextId));
            return dailyAffirmationTextSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByDailyAffirmationTextsAsync(long dailyAffirmationTextId)
        {
            var xYZZYXIds = GetDailyAffirmationTextXYZZYXsViewAsIQueryable().Where(x => x.DailyAffirmationTextId == dailyAffirmationText).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<DailyAffirmationTextXYZZYX> GetDailyAffirmationTextXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DailyAffirmationTextXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetDailyAffirmationTextXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.DailyAffirmationTextXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region DailyAffirmationSessionCategory

        public IQueryable<DailyAffirmationSessionCategoryView> GetDailyAffirmationSessionCategorysViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DailyAffirmationSessionCategory>()
                       join a in appDBManager.GetTable<Category>() on item.CategoryId equals a.CategoryId
                   join b in appDBManager.GetTable<DailyAffirmationSession>() on item.DailyAffirmationSessionId equals b.DailyAffirmationSessionId
                   join c in appDBManager.GetTable<User>() on item.UserId equals c.UserId

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new DailyAffirmationSessionCategoryView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       CategoryName = a.CategoryName,
                       UserName = c.UserName,
                       DailyAffirmationSessionCategoryId = item.DailyAffirmationSessionCategoryId,
                       CategoryId = item.CategoryId,
                       DailyAffirmationSessionId = item.DailyAffirmationSessionId,
                       DailyAffirmationSessionCategoryTimeSpentInSeconds = item.DailyAffirmationSessionCategoryTimeSpentInSeconds,
                       UserId = item.UserId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<DailyAffirmationSessionCategoryView> GetDailyAffirmationSessionCategorysInternalWithSearchCriteriaAsync(DailyAffirmationSessionCategorySearchCriteria search)
        {
            var sql1 = GetDailyAffirmationSessionCategorysViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.DailyAffirmationSessionCategoryId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                );
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

    if (search.CategoryId != null && search.CategoryId > 0)
                    sql1 = sql1.Where(x => search.CategoryId == x.CategoryId);
                if (search.DailyAffirmationSessionId != null && search.DailyAffirmationSessionId > 0)
                    sql1 = sql1.Where(x => search.DailyAffirmationSessionId == x.DailyAffirmationSessionId);
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

        public async Task<DbReturnListModel<DailyAffirmationSessionCategoryView>> GetDailyAffirmationSessionCategorysInternalAsync(DailyAffirmationSessionCategorySearchCriteria search)
        {
            var sql1 = GetDailyAffirmationSessionCategorysInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.DailyAffirmationSessionCategoryId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetDailyAffirmationSessionCategorysViewAsIQueryable().FirstOrDefaultAsync(x => x.DailyAffirmationSessionCategoryId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<DailyAffirmationSessionCategoryView>> GetDailyAffirmationSessionCategorysAsync(DailyAffirmationSessionCategorySearchCriteria search)
        {
            return GetDailyAffirmationSessionCategorysInternalAsync(search);
        }

        public Task<DailyAffirmationSessionCategory> GetDailyAffirmationSessionCategoryFromAnyByIdAsync(long dailyAffirmationSessionCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<DailyAffirmationSessionCategory>(x => x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId);
        }

        public Task<DailyAffirmationSessionCategoryView> GetDailyAffirmationSessionCategoryViewFromAnyByIdAsync(long dailyAffirmationSessionCategoryId)
        {
            return GetDailyAffirmationSessionCategorysViewAsIQueryable().FirstOrDefaultAsync(x => x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId);
        }

        public Task<DailyAffirmationSessionCategory> GetDailyAffirmationSessionCategoryByUniqueIdentifierAsync(string dailyAffirmationSessionCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<DailyAffirmationSessionCategory>(x => x.DailyAffirmationSessionCategoryId == SafeUtils.Int(dailyAffirmationSessionCategoryId) && x.IsDeleted == false);
        }

        public Task<DailyAffirmationSessionCategory> GetDailyAffirmationSessionCategoryByIdAsync(long dailyAffirmationSessionCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<DailyAffirmationSessionCategory>(x => x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId && x.IsDeleted == false);
        }

        public Task<List<DailyAffirmationSessionCategory>> GetDailyAffirmationSessionCategoryByIdsAsync(List<long> dailyAffirmationSessionCategoryIds)
        {
            return appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => dailyAffirmationSessionCategoryIds.Contains(x.DailyAffirmationSessionCategoryId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<DailyAffirmationSessionCategoryView> GetDailyAffirmationSessionCategoryViewByIdAsync(long dailyAffirmationSessionCategoryId)
        {
            var sql1 = GetDailyAffirmationSessionCategorysViewAsIQueryable();

            sql1 = sql1.Where(x => x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<DailyAffirmationSessionCategory>> GetAllDailyAffirmationSessionCategorys()
        {
            return appDBManager.SelectAllAsync<DailyAffirmationSessionCategory>();
        }

        /*
        public Task<DailyAffirmationSessionCategory> GetDailyAffirmationSessionCategoryByDailyAffirmationSessionCategoryCodeAsync(long userId, string dailyAffirmationSessionCategoryCode)
        {
            return appDBManager.FirstOrDefaultAsync<DailyAffirmationSessionCategory>(x => x.UserId == userId && x.DailyAffirmationSessionCategoryCode == dailyAffirmationSessionCategoryCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateDailyAffirmationSessionCategoryCodeAsync(long userId, long dailyAffirmationSessionCategoryId, string dailyAffirmationSessionCategoryCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.UserId == userId && x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId && x.IsDeleted == false).Set(x => x.DailyAffirmationSessionCategoryCode, dailyAffirmationSessionCategoryCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<DailyAffirmationSessionCategory> CreateDailyAffirmationSessionCategoryAsync(DailyAffirmationSessionCategory dailyAffirmationSessionCategory, long? createdBy)
        {
            dailyAffirmationSessionCategory.DailyAffirmationSessionCategoryId = await appDBManager.InsertWithAuditAsync<DailyAffirmationSessionCategory, long>(dailyAffirmationSessionCategory, createdBy);
            return dailyAffirmationSessionCategory;
        }

        public async Task<List<DailyAffirmationSessionCategory>> CreateDailyAffirmationSessionCategorysAsync(List<DailyAffirmationSessionCategory> dailyAffirmationSessionCategorys, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<DailyAffirmationSessionCategory, long>(dailyAffirmationSessionCategorys, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                dailyAffirmationSessionCategorys[i].DailyAffirmationSessionCategoryId = lst[i];
            return dailyAffirmationSessionCategorys;
        }

        public Task<bool> UpdateDailyAffirmationSessionCategoryAsync(DailyAffirmationSessionCategory dailyAffirmationSessionCategory, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<DailyAffirmationSessionCategory>(dailyAffirmationSessionCategory, modifiedBy);
        }

        public async Task<bool> MarkDailyAffirmationSessionCategoryAsNotDeletedByIdAsync(long dailyAffirmationSessionCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkDailyAffirmationSessionCategoryAsDeletedByIdAsync(long dailyAffirmationSessionCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteDailyAffirmationSessionCategoryByIdAsync(long dailyAffirmationSessionCategoryId)
        {
            return await appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetDailyAffirmationSessionCategoryActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.DailyAffirmationSessionCategory);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<List<DailyAffirmationSessionCategoryView>> GetDailyAffirmationSessionCategoryViewByCategoryIdAsync(long categoryId)
        {
                return GetDailyAffirmationSessionCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.CategoryId == categoryId).ToListAsync();
        }

        public Task<DbReturnListModel<DailyAffirmationSessionCategoryView>> GetDailyAffirmationSessionCategoryViewByCategoryIdAsync(DailyAffirmationSessionCategoryByCategoryIdSearchCriteria search)
        {
                return GetDailyAffirmationSessionCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.CategoryId == search.CategoryId).ReturnListModelResultAsync(search);
        }

        public Task<List<DailyAffirmationSessionCategory>> GetBareDailyAffirmationSessionCategoryByCategoryIdAsync(long categoryId)
        {
                return appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.CategoryId == categoryId).ToListAsync();
        }

        public Task<DbReturnListModel<DailyAffirmationSessionCategory>> GetBareDailyAffirmationSessionCategoryByCategoryIdAsync(DailyAffirmationSessionCategoryByCategoryIdSearchCriteria search)
        {
                return appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.CategoryId == search.CategoryId).ReturnListModelResultAsync(search);
        }


        public Task<List<CategoryView>> GetCategoryViewByDailyAffirmationSessionCategoryIdAsync(long dailyAffirmationSessionCategoryId)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId).Select(x => x.CategoryId);
                return GetCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.CategoryId)).ToListAsync();
        }

        public Task<DbReturnListModel<CategoryView>> GetCategoryViewByDailyAffirmationSessionCategoryIdAsync(CategoryByDailyAffirmationSessionCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionCategoryId == search.DailyAffirmationSessionCategoryId).Select(x => x.CategoryId);
                return GetCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.CategoryId)).ReturnListModelResultAsync(search);
        }

        public Task<List<Category>> GetBareCategoryByDailyAffirmationSessionCategoryIdAsync(long dailyAffirmationSessionCategoryId)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId).Select(x => x.CategoryId);
                return appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && idsSql.Contains(x.CategoryId)).ToListAsync();
        }

        public Task<DbReturnListModel<Category>> GetBareCategoryByDailyAffirmationSessionCategoryIdAsync(CategoryByDailyAffirmationSessionCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionCategoryId == search.DailyAffirmationSessionCategoryId).Select(x => x.CategoryId);
                return appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && idsSql.Contains(x.CategoryId)).ReturnListModelResultAsync(search);
        }


        public Task<List<DailyAffirmationSessionCategoryView>> GetDailyAffirmationSessionCategoryViewByDailyAffirmationSessionIdAsync(long dailyAffirmationSessionId)
        {
                return GetDailyAffirmationSessionCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionId == dailyAffirmationSessionId).ToListAsync();
        }

        public Task<DbReturnListModel<DailyAffirmationSessionCategoryView>> GetDailyAffirmationSessionCategoryViewByDailyAffirmationSessionIdAsync(DailyAffirmationSessionCategoryByDailyAffirmationSessionIdSearchCriteria search)
        {
                return GetDailyAffirmationSessionCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionId == search.DailyAffirmationSessionId).ReturnListModelResultAsync(search);
        }

        public Task<List<DailyAffirmationSessionCategory>> GetBareDailyAffirmationSessionCategoryByDailyAffirmationSessionIdAsync(long dailyAffirmationSessionId)
        {
                return appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionId == dailyAffirmationSessionId).ToListAsync();
        }

        public Task<DbReturnListModel<DailyAffirmationSessionCategory>> GetBareDailyAffirmationSessionCategoryByDailyAffirmationSessionIdAsync(DailyAffirmationSessionCategoryByDailyAffirmationSessionIdSearchCriteria search)
        {
                return appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionId == search.DailyAffirmationSessionId).ReturnListModelResultAsync(search);
        }


        public Task<List<DailyAffirmationSessionView>> GetDailyAffirmationSessionViewByDailyAffirmationSessionCategoryIdAsync(long dailyAffirmationSessionCategoryId)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId).Select(x => x.DailyAffirmationSessionId);
                return GetDailyAffirmationSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.DailyAffirmationSessionId)).ToListAsync();
        }

        public Task<DbReturnListModel<DailyAffirmationSessionView>> GetDailyAffirmationSessionViewByDailyAffirmationSessionCategoryIdAsync(DailyAffirmationSessionByDailyAffirmationSessionCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionCategoryId == search.DailyAffirmationSessionCategoryId).Select(x => x.DailyAffirmationSessionId);
                return GetDailyAffirmationSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.DailyAffirmationSessionId)).ReturnListModelResultAsync(search);
        }

        public Task<List<DailyAffirmationSession>> GetBareDailyAffirmationSessionByDailyAffirmationSessionCategoryIdAsync(long dailyAffirmationSessionCategoryId)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId).Select(x => x.DailyAffirmationSessionId);
                return appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.IsDeleted == false && idsSql.Contains(x.DailyAffirmationSessionId)).ToListAsync();
        }

        public Task<DbReturnListModel<DailyAffirmationSession>> GetBareDailyAffirmationSessionByDailyAffirmationSessionCategoryIdAsync(DailyAffirmationSessionByDailyAffirmationSessionCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionCategoryId == search.DailyAffirmationSessionCategoryId).Select(x => x.DailyAffirmationSessionId);
                return appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.IsDeleted == false && idsSql.Contains(x.DailyAffirmationSessionId)).ReturnListModelResultAsync(search);
        }


        public Task<List<DailyAffirmationSessionCategoryView>> GetDailyAffirmationSessionCategoryViewByUserIdAsync(long userId)
        {
                return GetDailyAffirmationSessionCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<DailyAffirmationSessionCategoryView>> GetDailyAffirmationSessionCategoryViewByUserIdAsync(DailyAffirmationSessionCategoryByUserIdSearchCriteria search)
        {
                return GetDailyAffirmationSessionCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }

        public Task<List<DailyAffirmationSessionCategory>> GetBareDailyAffirmationSessionCategoryByUserIdAsync(long userId)
        {
                return appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<DailyAffirmationSessionCategory>> GetBareDailyAffirmationSessionCategoryByUserIdAsync(DailyAffirmationSessionCategoryByUserIdSearchCriteria search)
        {
                return appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }


        public Task<List<UserView>> GetUserViewByDailyAffirmationSessionCategoryIdAsync(long dailyAffirmationSessionCategoryId)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<UserView>> GetUserViewByDailyAffirmationSessionCategoryIdAsync(UserByDailyAffirmationSessionCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionCategoryId == search.DailyAffirmationSessionCategoryId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }

        public Task<List<User>> GetBareUserByDailyAffirmationSessionCategoryIdAsync(long dailyAffirmationSessionCategoryId)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<User>> GetBareUserByDailyAffirmationSessionCategoryIdAsync(UserByDailyAffirmationSessionCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionCategoryId == search.DailyAffirmationSessionCategoryId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }



        /*

        public async Task<bool> UpdateDailyAffirmationSessionCategoryPicturePathAsync(long dailyAffirmationSessionCategoryId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearDailyAffirmationSessionCategoryProfilePathAsync(long dailyAffirmationSessionCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableDailyAffirmationSessionCategoryByIdAsync(long dailyAffirmationSessionCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableDailyAffirmationSessionCategoryByIdAsync(long dailyAffirmationSessionCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyAffirmationSessionCategory>().Where(x => x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateDailyAffirmationSessionCategoryWithXYZZYXAsync(long dailyAffirmationSessionCategoryId, long xYZZYXId, long? createdBy)
        {
            var item = new DailyAffirmationSessionCategoryXYZZYXAssociationModel();
            item.DailyAffirmationSessionCategoryId = dailyAffirmationSessionCategoryId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<DailyAffirmationSessionCategoryXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateDailyAffirmationSessionCategoryWithXYZZYXAsync(long dailyAffirmationSessionCategoryId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<DailyAffirmationSessionCategoryXYZZYX>(x => x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesDailyAffirmationSessionCategoryWithXYZZYXAssociationExistsAsync(long dailyAffirmationSessionCategoryId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<DailyAffirmationSessionCategoryXYZZYX>(x => x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategoryId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<DailyAffirmationSessionCategoryView>> GetDailyAffirmationSessionCategorysByXYZZYXAsync(long xYZZYXId)
        {
            var dailyAffirmationSessionCategoryIds = GetDailyAffirmationSessionCategoryXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.DailyAffirmationSessionCategoryId);
            var dailyAffirmationSessionCategorySql = GetDailyAffirmationSessionCategorysViewAsIQueryable().Where(x => dailyAffirmationSessionCategoryIds.Contains(x.DailyAffirmationSessionCategoryId));
            return dailyAffirmationSessionCategorySql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByDailyAffirmationSessionCategorysAsync(long dailyAffirmationSessionCategoryId)
        {
            var xYZZYXIds = GetDailyAffirmationSessionCategoryXYZZYXsViewAsIQueryable().Where(x => x.DailyAffirmationSessionCategoryId == dailyAffirmationSessionCategory).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<DailyAffirmationSessionCategoryXYZZYX> GetDailyAffirmationSessionCategoryXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DailyAffirmationSessionCategoryXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetDailyAffirmationSessionCategoryXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.DailyAffirmationSessionCategoryXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region DefaultCategory

        public IQueryable<DefaultCategoryView> GetDefaultCategorysViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DefaultCategory>()
                       join oa in appDBManager.GetTable<EnumType>() on item.PredefinedCategoryTypeId equals oa.EnumTypeId into aTemp
                   from a in aTemp.DefaultIfEmpty()

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new DefaultCategoryView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       PredefinedCategoryTypeName = a.EnumTypeName,

                       DefaultCategoryId = item.DefaultCategoryId,
                       DefaultCategoryName = item.DefaultCategoryName,
                       PredefinedCategoryTypeId = item.PredefinedCategoryTypeId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<DefaultCategoryView> GetDefaultCategorysInternalWithSearchCriteriaAsync(DefaultCategorySearchCriteria search)
        {
            var sql1 = GetDefaultCategorysViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.DefaultCategoryId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.DefaultCategoryName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
);
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

    if (search.PredefinedCategoryTypeIds != null && search.PredefinedCategoryTypeIds.Count > 0)
                    sql1 = sql1.Where(x => search.PredefinedCategoryTypeIds.Contains(x.PredefinedCategoryTypeId ?? 0));
                if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                    sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
                if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                    sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

    if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<DefaultCategoryView>> GetDefaultCategorysInternalAsync(DefaultCategorySearchCriteria search)
        {
            var sql1 = GetDefaultCategorysInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.DefaultCategoryId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetDefaultCategorysViewAsIQueryable().FirstOrDefaultAsync(x => x.DefaultCategoryId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<DefaultCategoryView>> GetDefaultCategorysAsync(DefaultCategorySearchCriteria search)
        {
            return GetDefaultCategorysInternalAsync(search);
        }

        public Task<DefaultCategory> GetDefaultCategoryFromAnyByIdAsync(long defaultCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultCategory>(x => x.DefaultCategoryId == defaultCategoryId);
        }

        public Task<DefaultCategoryView> GetDefaultCategoryViewFromAnyByIdAsync(long defaultCategoryId)
        {
            return GetDefaultCategorysViewAsIQueryable().FirstOrDefaultAsync(x => x.DefaultCategoryId == defaultCategoryId);
        }

        public Task<DefaultCategory> GetDefaultCategoryByUniqueIdentifierAsync(string defaultCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultCategory>(x => x.DefaultCategoryId == SafeUtils.Int(defaultCategoryId) && x.IsDeleted == false);
        }

        public Task<DefaultCategory> GetDefaultCategoryByIdAsync(long defaultCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultCategory>(x => x.DefaultCategoryId == defaultCategoryId && x.IsDeleted == false);
        }

        public Task<List<DefaultCategory>> GetDefaultCategoryByIdsAsync(List<long> defaultCategoryIds)
        {
            return appDBManager.GetTable<DefaultCategory>().Where(x => defaultCategoryIds.Contains(x.DefaultCategoryId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<DefaultCategoryView> GetDefaultCategoryViewByIdAsync(long defaultCategoryId)
        {
            var sql1 = GetDefaultCategorysViewAsIQueryable();

            sql1 = sql1.Where(x => x.DefaultCategoryId == defaultCategoryId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<DefaultCategory>> GetAllDefaultCategorys()
        {
            return appDBManager.SelectAllAsync<DefaultCategory>();
        }

        /*
        public Task<DefaultCategory> GetDefaultCategoryByDefaultCategoryCodeAsync(long userId, string defaultCategoryCode)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultCategory>(x => x.UserId == userId && x.DefaultCategoryCode == defaultCategoryCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateDefaultCategoryCodeAsync(long userId, long defaultCategoryId, string defaultCategoryCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultCategory>().Where(x => x.UserId == userId && x.DefaultCategoryId == defaultCategoryId && x.IsDeleted == false).Set(x => x.DefaultCategoryCode, defaultCategoryCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<DefaultCategory> CreateDefaultCategoryAsync(DefaultCategory defaultCategory, long? createdBy)
        {
            defaultCategory.DefaultCategoryId = await appDBManager.InsertWithAuditAsync<DefaultCategory, long>(defaultCategory, createdBy);
            return defaultCategory;
        }

        public async Task<List<DefaultCategory>> CreateDefaultCategorysAsync(List<DefaultCategory> defaultCategorys, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<DefaultCategory, long>(defaultCategorys, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                defaultCategorys[i].DefaultCategoryId = lst[i];
            return defaultCategorys;
        }

        public Task<bool> UpdateDefaultCategoryAsync(DefaultCategory defaultCategory, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<DefaultCategory>(defaultCategory, modifiedBy);
        }

        public async Task<bool> MarkDefaultCategoryAsNotDeletedByIdAsync(long defaultCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultCategory>().Where(x => x.DefaultCategoryId == defaultCategoryId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkDefaultCategoryAsDeletedByIdAsync(long defaultCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultCategory>().Where(x => x.DefaultCategoryId == defaultCategoryId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteDefaultCategoryByIdAsync(long defaultCategoryId)
        {
            return await appDBManager.GetTable<DefaultCategory>().Where(x => x.DefaultCategoryId == defaultCategoryId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetDefaultCategoryActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.DefaultCategory);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsDefaultCategoryNameAlreadyExistsForOtherDefaultCategoryAsync(long defaultCategoryId, string defaultCategoryName)
        {
            var sql = GetDefaultCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultCategoryId != defaultCategoryId && x.DefaultCategoryName.ToLower().Trim() == defaultCategoryName.ToLower().Trim());
            return sql.AnyAsync();
        }

        public Task<bool> IsDefaultCategoryNameAlreadyExistsForOtherDefaultCategoryAsync(string defaultCategoryName)
        {
            var sql = GetDefaultCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultCategoryName.ToLower().Trim() == defaultCategoryName.ToLower().Trim());
            return sql.AnyAsync();
        }

        


        /*

        public async Task<bool> UpdateDefaultCategoryPicturePathAsync(long defaultCategoryId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultCategory>().Where(x => x.DefaultCategoryId == defaultCategoryId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearDefaultCategoryProfilePathAsync(long defaultCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultCategory>().Where(x => x.DefaultCategoryId == defaultCategoryId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableDefaultCategoryByIdAsync(long defaultCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultCategory>().Where(x => x.DefaultCategoryId == defaultCategoryId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableDefaultCategoryByIdAsync(long defaultCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultCategory>().Where(x => x.DefaultCategoryId == defaultCategoryId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateDefaultCategoryWithXYZZYXAsync(long defaultCategoryId, long xYZZYXId, long? createdBy)
        {
            var item = new DefaultCategoryXYZZYXAssociationModel();
            item.DefaultCategoryId = defaultCategoryId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<DefaultCategoryXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateDefaultCategoryWithXYZZYXAsync(long defaultCategoryId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<DefaultCategoryXYZZYX>(x => x.DefaultCategoryId == defaultCategoryId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesDefaultCategoryWithXYZZYXAssociationExistsAsync(long defaultCategoryId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<DefaultCategoryXYZZYX>(x => x.DefaultCategoryId == defaultCategoryId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<DefaultCategoryView>> GetDefaultCategorysByXYZZYXAsync(long xYZZYXId)
        {
            var defaultCategoryIds = GetDefaultCategoryXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.DefaultCategoryId);
            var defaultCategorySql = GetDefaultCategorysViewAsIQueryable().Where(x => defaultCategoryIds.Contains(x.DefaultCategoryId));
            return defaultCategorySql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByDefaultCategorysAsync(long defaultCategoryId)
        {
            var xYZZYXIds = GetDefaultCategoryXYZZYXsViewAsIQueryable().Where(x => x.DefaultCategoryId == defaultCategory).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<DefaultCategoryXYZZYX> GetDefaultCategoryXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DefaultCategoryXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetDefaultCategoryXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.DefaultCategoryXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region DefaultDailyAffirmationText

        public IQueryable<DefaultDailyAffirmationTextView> GetDefaultDailyAffirmationTextsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DefaultDailyAffirmationText>()
                       join a in appDBManager.GetTable<DefaultCategory>() on item.DefaultCategoryId equals a.DefaultCategoryId

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new DefaultDailyAffirmationTextView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       DefaultCategoryName = a.DefaultCategoryName,

                       DefaultDailyAffirmationTextId = item.DefaultDailyAffirmationTextId,
                       DefaultDailyAffirmationTextName = item.DefaultDailyAffirmationTextName,
                       DefaultCategoryId = item.DefaultCategoryId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<DefaultDailyAffirmationTextView> GetDefaultDailyAffirmationTextsInternalWithSearchCriteriaAsync(DefaultDailyAffirmationTextSearchCriteria search)
        {
            var sql1 = GetDefaultDailyAffirmationTextsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.DefaultDailyAffirmationTextId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.DefaultDailyAffirmationTextName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
);
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

    if (search.DefaultCategoryId != null && search.DefaultCategoryId > 0)
                    sql1 = sql1.Where(x => search.DefaultCategoryId == x.DefaultCategoryId);
                if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                    sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
                if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                    sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

    if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<DefaultDailyAffirmationTextView>> GetDefaultDailyAffirmationTextsInternalAsync(DefaultDailyAffirmationTextSearchCriteria search)
        {
            var sql1 = GetDefaultDailyAffirmationTextsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.DefaultDailyAffirmationTextId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetDefaultDailyAffirmationTextsViewAsIQueryable().FirstOrDefaultAsync(x => x.DefaultDailyAffirmationTextId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<DefaultDailyAffirmationTextView>> GetDefaultDailyAffirmationTextsAsync(DefaultDailyAffirmationTextSearchCriteria search)
        {
            return GetDefaultDailyAffirmationTextsInternalAsync(search);
        }

        public Task<DefaultDailyAffirmationText> GetDefaultDailyAffirmationTextFromAnyByIdAsync(long defaultDailyAffirmationTextId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultDailyAffirmationText>(x => x.DefaultDailyAffirmationTextId == defaultDailyAffirmationTextId);
        }

        public Task<DefaultDailyAffirmationTextView> GetDefaultDailyAffirmationTextViewFromAnyByIdAsync(long defaultDailyAffirmationTextId)
        {
            return GetDefaultDailyAffirmationTextsViewAsIQueryable().FirstOrDefaultAsync(x => x.DefaultDailyAffirmationTextId == defaultDailyAffirmationTextId);
        }

        public Task<DefaultDailyAffirmationText> GetDefaultDailyAffirmationTextByUniqueIdentifierAsync(string defaultDailyAffirmationTextId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultDailyAffirmationText>(x => x.DefaultDailyAffirmationTextId == SafeUtils.Int(defaultDailyAffirmationTextId) && x.IsDeleted == false);
        }

        public Task<DefaultDailyAffirmationText> GetDefaultDailyAffirmationTextByIdAsync(long defaultDailyAffirmationTextId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultDailyAffirmationText>(x => x.DefaultDailyAffirmationTextId == defaultDailyAffirmationTextId && x.IsDeleted == false);
        }

        public Task<List<DefaultDailyAffirmationText>> GetDefaultDailyAffirmationTextByIdsAsync(List<long> defaultDailyAffirmationTextIds)
        {
            return appDBManager.GetTable<DefaultDailyAffirmationText>().Where(x => defaultDailyAffirmationTextIds.Contains(x.DefaultDailyAffirmationTextId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<DefaultDailyAffirmationTextView> GetDefaultDailyAffirmationTextViewByIdAsync(long defaultDailyAffirmationTextId)
        {
            var sql1 = GetDefaultDailyAffirmationTextsViewAsIQueryable();

            sql1 = sql1.Where(x => x.DefaultDailyAffirmationTextId == defaultDailyAffirmationTextId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<DefaultDailyAffirmationText>> GetAllDefaultDailyAffirmationTexts()
        {
            return appDBManager.SelectAllAsync<DefaultDailyAffirmationText>();
        }

        /*
        public Task<DefaultDailyAffirmationText> GetDefaultDailyAffirmationTextByDefaultDailyAffirmationTextCodeAsync(long userId, string defaultDailyAffirmationTextCode)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultDailyAffirmationText>(x => x.UserId == userId && x.DefaultDailyAffirmationTextCode == defaultDailyAffirmationTextCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateDefaultDailyAffirmationTextCodeAsync(long userId, long defaultDailyAffirmationTextId, string defaultDailyAffirmationTextCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultDailyAffirmationText>().Where(x => x.UserId == userId && x.DefaultDailyAffirmationTextId == defaultDailyAffirmationTextId && x.IsDeleted == false).Set(x => x.DefaultDailyAffirmationTextCode, defaultDailyAffirmationTextCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<DefaultDailyAffirmationText> CreateDefaultDailyAffirmationTextAsync(DefaultDailyAffirmationText defaultDailyAffirmationText, long? createdBy)
        {
            defaultDailyAffirmationText.DefaultDailyAffirmationTextId = await appDBManager.InsertWithAuditAsync<DefaultDailyAffirmationText, long>(defaultDailyAffirmationText, createdBy);
            return defaultDailyAffirmationText;
        }

        public async Task<List<DefaultDailyAffirmationText>> CreateDefaultDailyAffirmationTextsAsync(List<DefaultDailyAffirmationText> defaultDailyAffirmationTexts, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<DefaultDailyAffirmationText, long>(defaultDailyAffirmationTexts, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                defaultDailyAffirmationTexts[i].DefaultDailyAffirmationTextId = lst[i];
            return defaultDailyAffirmationTexts;
        }

        public Task<bool> UpdateDefaultDailyAffirmationTextAsync(DefaultDailyAffirmationText defaultDailyAffirmationText, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<DefaultDailyAffirmationText>(defaultDailyAffirmationText, modifiedBy);
        }

        public async Task<bool> MarkDefaultDailyAffirmationTextAsNotDeletedByIdAsync(long defaultDailyAffirmationTextId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultDailyAffirmationText>().Where(x => x.DefaultDailyAffirmationTextId == defaultDailyAffirmationTextId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkDefaultDailyAffirmationTextAsDeletedByIdAsync(long defaultDailyAffirmationTextId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultDailyAffirmationText>().Where(x => x.DefaultDailyAffirmationTextId == defaultDailyAffirmationTextId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteDefaultDailyAffirmationTextByIdAsync(long defaultDailyAffirmationTextId)
        {
            return await appDBManager.GetTable<DefaultDailyAffirmationText>().Where(x => x.DefaultDailyAffirmationTextId == defaultDailyAffirmationTextId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetDefaultDailyAffirmationTextActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.DefaultDailyAffirmationText);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsDefaultDailyAffirmationTextNameAlreadyExistsForOtherDefaultDailyAffirmationTextAsync(long defaultDailyAffirmationTextId, string defaultDailyAffirmationTextName)
        {
            var sql = GetDefaultDailyAffirmationTextsViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultDailyAffirmationTextId != defaultDailyAffirmationTextId && x.DefaultDailyAffirmationTextName.ToLower().Trim() == defaultDailyAffirmationTextName.ToLower().Trim());
            return sql.AnyAsync();
        }

        public Task<bool> IsDefaultDailyAffirmationTextNameAlreadyExistsForOtherDefaultDailyAffirmationTextAsync(string defaultDailyAffirmationTextName)
        {
            var sql = GetDefaultDailyAffirmationTextsViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultDailyAffirmationTextName.ToLower().Trim() == defaultDailyAffirmationTextName.ToLower().Trim());
            return sql.AnyAsync();
        }

        

        public Task<List<DefaultDailyAffirmationTextView>> GetDefaultDailyAffirmationTextViewByDefaultCategoryIdAsync(long defaultCategoryId)
        {
                return GetDefaultDailyAffirmationTextsViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultCategoryId == defaultCategoryId).ToListAsync();
        }

        public Task<DbReturnListModel<DefaultDailyAffirmationTextView>> GetDefaultDailyAffirmationTextViewByDefaultCategoryIdAsync(DefaultDailyAffirmationTextByDefaultCategoryIdSearchCriteria search)
        {
                return GetDefaultDailyAffirmationTextsViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultCategoryId == search.DefaultCategoryId).ReturnListModelResultAsync(search);
        }

        public Task<List<DefaultDailyAffirmationText>> GetBareDefaultDailyAffirmationTextByDefaultCategoryIdAsync(long defaultCategoryId)
        {
                return appDBManager.GetTable<DefaultDailyAffirmationText>().Where(x => x.IsDeleted == false && x.DefaultCategoryId == defaultCategoryId).ToListAsync();
        }

        public Task<DbReturnListModel<DefaultDailyAffirmationText>> GetBareDefaultDailyAffirmationTextByDefaultCategoryIdAsync(DefaultDailyAffirmationTextByDefaultCategoryIdSearchCriteria search)
        {
                return appDBManager.GetTable<DefaultDailyAffirmationText>().Where(x => x.IsDeleted == false && x.DefaultCategoryId == search.DefaultCategoryId).ReturnListModelResultAsync(search);
        }


        public Task<List<DefaultCategoryView>> GetDefaultCategoryViewByDefaultDailyAffirmationTextIdAsync(long defaultDailyAffirmationTextId)
        {
                var idsSql = appDBManager.GetTable<DefaultDailyAffirmationText>().Where(x => x.IsDeleted == false && x.DefaultDailyAffirmationTextId == defaultDailyAffirmationTextId).Select(x => x.DefaultCategoryId);
                return GetDefaultCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.DefaultCategoryId)).ToListAsync();
        }

        public Task<DbReturnListModel<DefaultCategoryView>> GetDefaultCategoryViewByDefaultDailyAffirmationTextIdAsync(DefaultCategoryByDefaultDailyAffirmationTextIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DefaultDailyAffirmationText>().Where(x => x.IsDeleted == false && x.DefaultDailyAffirmationTextId == search.DefaultDailyAffirmationTextId).Select(x => x.DefaultCategoryId);
                return GetDefaultCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.DefaultCategoryId)).ReturnListModelResultAsync(search);
        }

        public Task<List<DefaultCategory>> GetBareDefaultCategoryByDefaultDailyAffirmationTextIdAsync(long defaultDailyAffirmationTextId)
        {
                var idsSql = appDBManager.GetTable<DefaultDailyAffirmationText>().Where(x => x.IsDeleted == false && x.DefaultDailyAffirmationTextId == defaultDailyAffirmationTextId).Select(x => x.DefaultCategoryId);
                return appDBManager.GetTable<DefaultCategory>().Where(x => x.IsDeleted == false && idsSql.Contains(x.DefaultCategoryId)).ToListAsync();
        }

        public Task<DbReturnListModel<DefaultCategory>> GetBareDefaultCategoryByDefaultDailyAffirmationTextIdAsync(DefaultCategoryByDefaultDailyAffirmationTextIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DefaultDailyAffirmationText>().Where(x => x.IsDeleted == false && x.DefaultDailyAffirmationTextId == search.DefaultDailyAffirmationTextId).Select(x => x.DefaultCategoryId);
                return appDBManager.GetTable<DefaultCategory>().Where(x => x.IsDeleted == false && idsSql.Contains(x.DefaultCategoryId)).ReturnListModelResultAsync(search);
        }



        /*

        public async Task<bool> UpdateDefaultDailyAffirmationTextPicturePathAsync(long defaultDailyAffirmationTextId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultDailyAffirmationText>().Where(x => x.DefaultDailyAffirmationTextId == defaultDailyAffirmationTextId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearDefaultDailyAffirmationTextProfilePathAsync(long defaultDailyAffirmationTextId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultDailyAffirmationText>().Where(x => x.DefaultDailyAffirmationTextId == defaultDailyAffirmationTextId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableDefaultDailyAffirmationTextByIdAsync(long defaultDailyAffirmationTextId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultDailyAffirmationText>().Where(x => x.DefaultDailyAffirmationTextId == defaultDailyAffirmationTextId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableDefaultDailyAffirmationTextByIdAsync(long defaultDailyAffirmationTextId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultDailyAffirmationText>().Where(x => x.DefaultDailyAffirmationTextId == defaultDailyAffirmationTextId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateDefaultDailyAffirmationTextWithXYZZYXAsync(long defaultDailyAffirmationTextId, long xYZZYXId, long? createdBy)
        {
            var item = new DefaultDailyAffirmationTextXYZZYXAssociationModel();
            item.DefaultDailyAffirmationTextId = defaultDailyAffirmationTextId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<DefaultDailyAffirmationTextXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateDefaultDailyAffirmationTextWithXYZZYXAsync(long defaultDailyAffirmationTextId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<DefaultDailyAffirmationTextXYZZYX>(x => x.DefaultDailyAffirmationTextId == defaultDailyAffirmationTextId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesDefaultDailyAffirmationTextWithXYZZYXAssociationExistsAsync(long defaultDailyAffirmationTextId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<DefaultDailyAffirmationTextXYZZYX>(x => x.DefaultDailyAffirmationTextId == defaultDailyAffirmationTextId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<DefaultDailyAffirmationTextView>> GetDefaultDailyAffirmationTextsByXYZZYXAsync(long xYZZYXId)
        {
            var defaultDailyAffirmationTextIds = GetDefaultDailyAffirmationTextXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.DefaultDailyAffirmationTextId);
            var defaultDailyAffirmationTextSql = GetDefaultDailyAffirmationTextsViewAsIQueryable().Where(x => defaultDailyAffirmationTextIds.Contains(x.DefaultDailyAffirmationTextId));
            return defaultDailyAffirmationTextSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByDefaultDailyAffirmationTextsAsync(long defaultDailyAffirmationTextId)
        {
            var xYZZYXIds = GetDefaultDailyAffirmationTextXYZZYXsViewAsIQueryable().Where(x => x.DefaultDailyAffirmationTextId == defaultDailyAffirmationText).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<DefaultDailyAffirmationTextXYZZYX> GetDefaultDailyAffirmationTextXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DefaultDailyAffirmationTextXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetDefaultDailyAffirmationTextXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.DefaultDailyAffirmationTextXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region DefaultGoalTrackerCategory

        public IQueryable<DefaultGoalTrackerCategoryView> GetDefaultGoalTrackerCategorysViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DefaultGoalTrackerCategory>()
                       join a in appDBManager.GetTable<DefaultCategory>() on item.DefaultCategoryId equals a.DefaultCategoryId
                   join b in appDBManager.GetTable<DefaultGoalTracker>() on item.DefaultGoalTrackerId equals b.DefaultGoalTrackerId

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new DefaultGoalTrackerCategoryView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       DefaultCategoryName = a.DefaultCategoryName,

                       DefaultGoalTrackerCategoryId = item.DefaultGoalTrackerCategoryId,
                       DefaultCategoryId = item.DefaultCategoryId,
                       DefaultGoalTrackerId = item.DefaultGoalTrackerId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<DefaultGoalTrackerCategoryView> GetDefaultGoalTrackerCategorysInternalWithSearchCriteriaAsync(DefaultGoalTrackerCategorySearchCriteria search)
        {
            var sql1 = GetDefaultGoalTrackerCategorysViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.DefaultGoalTrackerCategoryId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                );
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

    if (search.DefaultCategoryId != null && search.DefaultCategoryId > 0)
                    sql1 = sql1.Where(x => search.DefaultCategoryId == x.DefaultCategoryId);
                if (search.DefaultGoalTrackerId != null && search.DefaultGoalTrackerId > 0)
                    sql1 = sql1.Where(x => search.DefaultGoalTrackerId == x.DefaultGoalTrackerId);
                if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                    sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
                if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                    sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

    if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<DefaultGoalTrackerCategoryView>> GetDefaultGoalTrackerCategorysInternalAsync(DefaultGoalTrackerCategorySearchCriteria search)
        {
            var sql1 = GetDefaultGoalTrackerCategorysInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.DefaultGoalTrackerCategoryId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetDefaultGoalTrackerCategorysViewAsIQueryable().FirstOrDefaultAsync(x => x.DefaultGoalTrackerCategoryId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<DefaultGoalTrackerCategoryView>> GetDefaultGoalTrackerCategorysAsync(DefaultGoalTrackerCategorySearchCriteria search)
        {
            return GetDefaultGoalTrackerCategorysInternalAsync(search);
        }

        public Task<DefaultGoalTrackerCategory> GetDefaultGoalTrackerCategoryFromAnyByIdAsync(long defaultGoalTrackerCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultGoalTrackerCategory>(x => x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId);
        }

        public Task<DefaultGoalTrackerCategoryView> GetDefaultGoalTrackerCategoryViewFromAnyByIdAsync(long defaultGoalTrackerCategoryId)
        {
            return GetDefaultGoalTrackerCategorysViewAsIQueryable().FirstOrDefaultAsync(x => x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId);
        }

        public Task<DefaultGoalTrackerCategory> GetDefaultGoalTrackerCategoryByUniqueIdentifierAsync(string defaultGoalTrackerCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultGoalTrackerCategory>(x => x.DefaultGoalTrackerCategoryId == SafeUtils.Int(defaultGoalTrackerCategoryId) && x.IsDeleted == false);
        }

        public Task<DefaultGoalTrackerCategory> GetDefaultGoalTrackerCategoryByIdAsync(long defaultGoalTrackerCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultGoalTrackerCategory>(x => x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId && x.IsDeleted == false);
        }

        public Task<List<DefaultGoalTrackerCategory>> GetDefaultGoalTrackerCategoryByIdsAsync(List<long> defaultGoalTrackerCategoryIds)
        {
            return appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => defaultGoalTrackerCategoryIds.Contains(x.DefaultGoalTrackerCategoryId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<DefaultGoalTrackerCategoryView> GetDefaultGoalTrackerCategoryViewByIdAsync(long defaultGoalTrackerCategoryId)
        {
            var sql1 = GetDefaultGoalTrackerCategorysViewAsIQueryable();

            sql1 = sql1.Where(x => x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<DefaultGoalTrackerCategory>> GetAllDefaultGoalTrackerCategorys()
        {
            return appDBManager.SelectAllAsync<DefaultGoalTrackerCategory>();
        }

        /*
        public Task<DefaultGoalTrackerCategory> GetDefaultGoalTrackerCategoryByDefaultGoalTrackerCategoryCodeAsync(long userId, string defaultGoalTrackerCategoryCode)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultGoalTrackerCategory>(x => x.UserId == userId && x.DefaultGoalTrackerCategoryCode == defaultGoalTrackerCategoryCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateDefaultGoalTrackerCategoryCodeAsync(long userId, long defaultGoalTrackerCategoryId, string defaultGoalTrackerCategoryCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.UserId == userId && x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId && x.IsDeleted == false).Set(x => x.DefaultGoalTrackerCategoryCode, defaultGoalTrackerCategoryCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<DefaultGoalTrackerCategory> CreateDefaultGoalTrackerCategoryAsync(DefaultGoalTrackerCategory defaultGoalTrackerCategory, long? createdBy)
        {
            defaultGoalTrackerCategory.DefaultGoalTrackerCategoryId = await appDBManager.InsertWithAuditAsync<DefaultGoalTrackerCategory, long>(defaultGoalTrackerCategory, createdBy);
            return defaultGoalTrackerCategory;
        }

        public async Task<List<DefaultGoalTrackerCategory>> CreateDefaultGoalTrackerCategorysAsync(List<DefaultGoalTrackerCategory> defaultGoalTrackerCategorys, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<DefaultGoalTrackerCategory, long>(defaultGoalTrackerCategorys, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                defaultGoalTrackerCategorys[i].DefaultGoalTrackerCategoryId = lst[i];
            return defaultGoalTrackerCategorys;
        }

        public Task<bool> UpdateDefaultGoalTrackerCategoryAsync(DefaultGoalTrackerCategory defaultGoalTrackerCategory, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<DefaultGoalTrackerCategory>(defaultGoalTrackerCategory, modifiedBy);
        }

        public async Task<bool> MarkDefaultGoalTrackerCategoryAsNotDeletedByIdAsync(long defaultGoalTrackerCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkDefaultGoalTrackerCategoryAsDeletedByIdAsync(long defaultGoalTrackerCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteDefaultGoalTrackerCategoryByIdAsync(long defaultGoalTrackerCategoryId)
        {
            return await appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetDefaultGoalTrackerCategoryActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.DefaultGoalTrackerCategory);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<List<DefaultGoalTrackerCategoryView>> GetDefaultGoalTrackerCategoryViewByDefaultCategoryIdAsync(long defaultCategoryId)
        {
                return GetDefaultGoalTrackerCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultCategoryId == defaultCategoryId).ToListAsync();
        }

        public Task<DbReturnListModel<DefaultGoalTrackerCategoryView>> GetDefaultGoalTrackerCategoryViewByDefaultCategoryIdAsync(DefaultGoalTrackerCategoryByDefaultCategoryIdSearchCriteria search)
        {
                return GetDefaultGoalTrackerCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultCategoryId == search.DefaultCategoryId).ReturnListModelResultAsync(search);
        }

        public Task<List<DefaultGoalTrackerCategory>> GetBareDefaultGoalTrackerCategoryByDefaultCategoryIdAsync(long defaultCategoryId)
        {
                return appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.IsDeleted == false && x.DefaultCategoryId == defaultCategoryId).ToListAsync();
        }

        public Task<DbReturnListModel<DefaultGoalTrackerCategory>> GetBareDefaultGoalTrackerCategoryByDefaultCategoryIdAsync(DefaultGoalTrackerCategoryByDefaultCategoryIdSearchCriteria search)
        {
                return appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.IsDeleted == false && x.DefaultCategoryId == search.DefaultCategoryId).ReturnListModelResultAsync(search);
        }


        public Task<List<DefaultCategoryView>> GetDefaultCategoryViewByDefaultGoalTrackerCategoryIdAsync(long defaultGoalTrackerCategoryId)
        {
                var idsSql = appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.IsDeleted == false && x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId).Select(x => x.DefaultCategoryId);
                return GetDefaultCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.DefaultCategoryId)).ToListAsync();
        }

        public Task<DbReturnListModel<DefaultCategoryView>> GetDefaultCategoryViewByDefaultGoalTrackerCategoryIdAsync(DefaultCategoryByDefaultGoalTrackerCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.IsDeleted == false && x.DefaultGoalTrackerCategoryId == search.DefaultGoalTrackerCategoryId).Select(x => x.DefaultCategoryId);
                return GetDefaultCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.DefaultCategoryId)).ReturnListModelResultAsync(search);
        }

        public Task<List<DefaultCategory>> GetBareDefaultCategoryByDefaultGoalTrackerCategoryIdAsync(long defaultGoalTrackerCategoryId)
        {
                var idsSql = appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.IsDeleted == false && x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId).Select(x => x.DefaultCategoryId);
                return appDBManager.GetTable<DefaultCategory>().Where(x => x.IsDeleted == false && idsSql.Contains(x.DefaultCategoryId)).ToListAsync();
        }

        public Task<DbReturnListModel<DefaultCategory>> GetBareDefaultCategoryByDefaultGoalTrackerCategoryIdAsync(DefaultCategoryByDefaultGoalTrackerCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.IsDeleted == false && x.DefaultGoalTrackerCategoryId == search.DefaultGoalTrackerCategoryId).Select(x => x.DefaultCategoryId);
                return appDBManager.GetTable<DefaultCategory>().Where(x => x.IsDeleted == false && idsSql.Contains(x.DefaultCategoryId)).ReturnListModelResultAsync(search);
        }


        public Task<List<DefaultGoalTrackerCategoryView>> GetDefaultGoalTrackerCategoryViewByDefaultGoalTrackerIdAsync(long defaultGoalTrackerId)
        {
                return GetDefaultGoalTrackerCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultGoalTrackerId == defaultGoalTrackerId).ToListAsync();
        }

        public Task<DbReturnListModel<DefaultGoalTrackerCategoryView>> GetDefaultGoalTrackerCategoryViewByDefaultGoalTrackerIdAsync(DefaultGoalTrackerCategoryByDefaultGoalTrackerIdSearchCriteria search)
        {
                return GetDefaultGoalTrackerCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultGoalTrackerId == search.DefaultGoalTrackerId).ReturnListModelResultAsync(search);
        }

        public Task<List<DefaultGoalTrackerCategory>> GetBareDefaultGoalTrackerCategoryByDefaultGoalTrackerIdAsync(long defaultGoalTrackerId)
        {
                return appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.IsDeleted == false && x.DefaultGoalTrackerId == defaultGoalTrackerId).ToListAsync();
        }

        public Task<DbReturnListModel<DefaultGoalTrackerCategory>> GetBareDefaultGoalTrackerCategoryByDefaultGoalTrackerIdAsync(DefaultGoalTrackerCategoryByDefaultGoalTrackerIdSearchCriteria search)
        {
                return appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.IsDeleted == false && x.DefaultGoalTrackerId == search.DefaultGoalTrackerId).ReturnListModelResultAsync(search);
        }


        public Task<List<DefaultGoalTrackerView>> GetDefaultGoalTrackerViewByDefaultGoalTrackerCategoryIdAsync(long defaultGoalTrackerCategoryId)
        {
                var idsSql = appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.IsDeleted == false && x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId).Select(x => x.DefaultGoalTrackerId);
                return GetDefaultGoalTrackersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.DefaultGoalTrackerId)).ToListAsync();
        }

        public Task<DbReturnListModel<DefaultGoalTrackerView>> GetDefaultGoalTrackerViewByDefaultGoalTrackerCategoryIdAsync(DefaultGoalTrackerByDefaultGoalTrackerCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.IsDeleted == false && x.DefaultGoalTrackerCategoryId == search.DefaultGoalTrackerCategoryId).Select(x => x.DefaultGoalTrackerId);
                return GetDefaultGoalTrackersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.DefaultGoalTrackerId)).ReturnListModelResultAsync(search);
        }

        public Task<List<DefaultGoalTracker>> GetBareDefaultGoalTrackerByDefaultGoalTrackerCategoryIdAsync(long defaultGoalTrackerCategoryId)
        {
                var idsSql = appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.IsDeleted == false && x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId).Select(x => x.DefaultGoalTrackerId);
                return appDBManager.GetTable<DefaultGoalTracker>().Where(x => x.IsDeleted == false && idsSql.Contains(x.DefaultGoalTrackerId)).ToListAsync();
        }

        public Task<DbReturnListModel<DefaultGoalTracker>> GetBareDefaultGoalTrackerByDefaultGoalTrackerCategoryIdAsync(DefaultGoalTrackerByDefaultGoalTrackerCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.IsDeleted == false && x.DefaultGoalTrackerCategoryId == search.DefaultGoalTrackerCategoryId).Select(x => x.DefaultGoalTrackerId);
                return appDBManager.GetTable<DefaultGoalTracker>().Where(x => x.IsDeleted == false && idsSql.Contains(x.DefaultGoalTrackerId)).ReturnListModelResultAsync(search);
        }



        /*

        public async Task<bool> UpdateDefaultGoalTrackerCategoryPicturePathAsync(long defaultGoalTrackerCategoryId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearDefaultGoalTrackerCategoryProfilePathAsync(long defaultGoalTrackerCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableDefaultGoalTrackerCategoryByIdAsync(long defaultGoalTrackerCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableDefaultGoalTrackerCategoryByIdAsync(long defaultGoalTrackerCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultGoalTrackerCategory>().Where(x => x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateDefaultGoalTrackerCategoryWithXYZZYXAsync(long defaultGoalTrackerCategoryId, long xYZZYXId, long? createdBy)
        {
            var item = new DefaultGoalTrackerCategoryXYZZYXAssociationModel();
            item.DefaultGoalTrackerCategoryId = defaultGoalTrackerCategoryId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<DefaultGoalTrackerCategoryXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateDefaultGoalTrackerCategoryWithXYZZYXAsync(long defaultGoalTrackerCategoryId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<DefaultGoalTrackerCategoryXYZZYX>(x => x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesDefaultGoalTrackerCategoryWithXYZZYXAssociationExistsAsync(long defaultGoalTrackerCategoryId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<DefaultGoalTrackerCategoryXYZZYX>(x => x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategoryId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<DefaultGoalTrackerCategoryView>> GetDefaultGoalTrackerCategorysByXYZZYXAsync(long xYZZYXId)
        {
            var defaultGoalTrackerCategoryIds = GetDefaultGoalTrackerCategoryXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.DefaultGoalTrackerCategoryId);
            var defaultGoalTrackerCategorySql = GetDefaultGoalTrackerCategorysViewAsIQueryable().Where(x => defaultGoalTrackerCategoryIds.Contains(x.DefaultGoalTrackerCategoryId));
            return defaultGoalTrackerCategorySql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByDefaultGoalTrackerCategorysAsync(long defaultGoalTrackerCategoryId)
        {
            var xYZZYXIds = GetDefaultGoalTrackerCategoryXYZZYXsViewAsIQueryable().Where(x => x.DefaultGoalTrackerCategoryId == defaultGoalTrackerCategory).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<DefaultGoalTrackerCategoryXYZZYX> GetDefaultGoalTrackerCategoryXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DefaultGoalTrackerCategoryXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetDefaultGoalTrackerCategoryXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.DefaultGoalTrackerCategoryXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region DefaultGoalTracker

        public IQueryable<DefaultGoalTrackerView> GetDefaultGoalTrackersViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DefaultGoalTracker>()
                       
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new DefaultGoalTrackerView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       
DefaultGoalTrackerId = item.DefaultGoalTrackerId,
DefaultGoalTrackerText = item.DefaultGoalTrackerText,
IsDeleted = item.IsDeleted,
CreatedDate = item.CreatedDate,
CreatedByUserId = item.CreatedByUserId,
ModifiedDate = item.ModifiedDate,
ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<DefaultGoalTrackerView> GetDefaultGoalTrackersInternalWithSearchCriteriaAsync(DefaultGoalTrackerSearchCriteria search)
        {
            var sql1 = GetDefaultGoalTrackersViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.DefaultGoalTrackerId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.DefaultGoalTrackerText.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
);
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

    if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                    sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
                if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                    sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

    if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<DefaultGoalTrackerView>> GetDefaultGoalTrackersInternalAsync(DefaultGoalTrackerSearchCriteria search)
        {
            var sql1 = GetDefaultGoalTrackersInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.DefaultGoalTrackerId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetDefaultGoalTrackersViewAsIQueryable().FirstOrDefaultAsync(x => x.DefaultGoalTrackerId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<DefaultGoalTrackerView>> GetDefaultGoalTrackersAsync(DefaultGoalTrackerSearchCriteria search)
        {
            return GetDefaultGoalTrackersInternalAsync(search);
        }

        public Task<DefaultGoalTracker> GetDefaultGoalTrackerFromAnyByIdAsync(long defaultGoalTrackerId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultGoalTracker>(x => x.DefaultGoalTrackerId == defaultGoalTrackerId);
        }

        public Task<DefaultGoalTrackerView> GetDefaultGoalTrackerViewFromAnyByIdAsync(long defaultGoalTrackerId)
        {
            return GetDefaultGoalTrackersViewAsIQueryable().FirstOrDefaultAsync(x => x.DefaultGoalTrackerId == defaultGoalTrackerId);
        }

        public Task<DefaultGoalTracker> GetDefaultGoalTrackerByUniqueIdentifierAsync(string defaultGoalTrackerId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultGoalTracker>(x => x.DefaultGoalTrackerId == SafeUtils.Int(defaultGoalTrackerId) && x.IsDeleted == false);
        }

        public Task<DefaultGoalTracker> GetDefaultGoalTrackerByIdAsync(long defaultGoalTrackerId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultGoalTracker>(x => x.DefaultGoalTrackerId == defaultGoalTrackerId && x.IsDeleted == false);
        }

        public Task<List<DefaultGoalTracker>> GetDefaultGoalTrackerByIdsAsync(List<long> defaultGoalTrackerIds)
        {
            return appDBManager.GetTable<DefaultGoalTracker>().Where(x => defaultGoalTrackerIds.Contains(x.DefaultGoalTrackerId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<DefaultGoalTrackerView> GetDefaultGoalTrackerViewByIdAsync(long defaultGoalTrackerId)
        {
            var sql1 = GetDefaultGoalTrackersViewAsIQueryable();

            sql1 = sql1.Where(x => x.DefaultGoalTrackerId == defaultGoalTrackerId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<DefaultGoalTracker>> GetAllDefaultGoalTrackers()
        {
            return appDBManager.SelectAllAsync<DefaultGoalTracker>();
        }

        /*
        public Task<DefaultGoalTracker> GetDefaultGoalTrackerByDefaultGoalTrackerCodeAsync(long userId, string defaultGoalTrackerCode)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultGoalTracker>(x => x.UserId == userId && x.DefaultGoalTrackerCode == defaultGoalTrackerCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateDefaultGoalTrackerCodeAsync(long userId, long defaultGoalTrackerId, string defaultGoalTrackerCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultGoalTracker>().Where(x => x.UserId == userId && x.DefaultGoalTrackerId == defaultGoalTrackerId && x.IsDeleted == false).Set(x => x.DefaultGoalTrackerCode, defaultGoalTrackerCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<DefaultGoalTracker> CreateDefaultGoalTrackerAsync(DefaultGoalTracker defaultGoalTracker, long? createdBy)
        {
            defaultGoalTracker.DefaultGoalTrackerId = await appDBManager.InsertWithAuditAsync<DefaultGoalTracker, long>(defaultGoalTracker, createdBy);
            return defaultGoalTracker;
        }

        public async Task<List<DefaultGoalTracker>> CreateDefaultGoalTrackersAsync(List<DefaultGoalTracker> defaultGoalTrackers, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<DefaultGoalTracker, long>(defaultGoalTrackers, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                defaultGoalTrackers[i].DefaultGoalTrackerId = lst[i];
            return defaultGoalTrackers;
        }

        public Task<bool> UpdateDefaultGoalTrackerAsync(DefaultGoalTracker defaultGoalTracker, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<DefaultGoalTracker>(defaultGoalTracker, modifiedBy);
        }

        public async Task<bool> MarkDefaultGoalTrackerAsNotDeletedByIdAsync(long defaultGoalTrackerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultGoalTracker>().Where(x => x.DefaultGoalTrackerId == defaultGoalTrackerId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkDefaultGoalTrackerAsDeletedByIdAsync(long defaultGoalTrackerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultGoalTracker>().Where(x => x.DefaultGoalTrackerId == defaultGoalTrackerId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteDefaultGoalTrackerByIdAsync(long defaultGoalTrackerId)
        {
            return await appDBManager.GetTable<DefaultGoalTracker>().Where(x => x.DefaultGoalTrackerId == defaultGoalTrackerId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetDefaultGoalTrackerActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.DefaultGoalTracker);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsDefaultGoalTrackerNameAlreadyExistsForOtherDefaultGoalTrackerAsync(long defaultGoalTrackerId, string defaultGoalTrackerName)
        {
            var sql = GetDefaultGoalTrackersViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultGoalTrackerId != defaultGoalTrackerId && x.DefaultGoalTrackerText.ToLower().Trim() == defaultGoalTrackerName.ToLower().Trim());
            return sql.AnyAsync();
        }

        public Task<bool> IsDefaultGoalTrackerNameAlreadyExistsForOtherDefaultGoalTrackerAsync(string defaultGoalTrackerName)
        {
            var sql = GetDefaultGoalTrackersViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultGoalTrackerText.ToLower().Trim() == defaultGoalTrackerName.ToLower().Trim());
            return sql.AnyAsync();
        }

        


        /*

        public async Task<bool> UpdateDefaultGoalTrackerPicturePathAsync(long defaultGoalTrackerId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultGoalTracker>().Where(x => x.DefaultGoalTrackerId == defaultGoalTrackerId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearDefaultGoalTrackerProfilePathAsync(long defaultGoalTrackerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultGoalTracker>().Where(x => x.DefaultGoalTrackerId == defaultGoalTrackerId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableDefaultGoalTrackerByIdAsync(long defaultGoalTrackerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultGoalTracker>().Where(x => x.DefaultGoalTrackerId == defaultGoalTrackerId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableDefaultGoalTrackerByIdAsync(long defaultGoalTrackerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultGoalTracker>().Where(x => x.DefaultGoalTrackerId == defaultGoalTrackerId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateDefaultGoalTrackerWithXYZZYXAsync(long defaultGoalTrackerId, long xYZZYXId, long? createdBy)
        {
            var item = new DefaultGoalTrackerXYZZYXAssociationModel();
            item.DefaultGoalTrackerId = defaultGoalTrackerId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<DefaultGoalTrackerXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateDefaultGoalTrackerWithXYZZYXAsync(long defaultGoalTrackerId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<DefaultGoalTrackerXYZZYX>(x => x.DefaultGoalTrackerId == defaultGoalTrackerId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesDefaultGoalTrackerWithXYZZYXAssociationExistsAsync(long defaultGoalTrackerId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<DefaultGoalTrackerXYZZYX>(x => x.DefaultGoalTrackerId == defaultGoalTrackerId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<DefaultGoalTrackerView>> GetDefaultGoalTrackersByXYZZYXAsync(long xYZZYXId)
        {
            var defaultGoalTrackerIds = GetDefaultGoalTrackerXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.DefaultGoalTrackerId);
            var defaultGoalTrackerSql = GetDefaultGoalTrackersViewAsIQueryable().Where(x => defaultGoalTrackerIds.Contains(x.DefaultGoalTrackerId));
            return defaultGoalTrackerSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByDefaultGoalTrackersAsync(long defaultGoalTrackerId)
        {
            var xYZZYXIds = GetDefaultGoalTrackerXYZZYXsViewAsIQueryable().Where(x => x.DefaultGoalTrackerId == defaultGoalTracker).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<DefaultGoalTrackerXYZZYX> GetDefaultGoalTrackerXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DefaultGoalTrackerXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetDefaultGoalTrackerXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.DefaultGoalTrackerXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region DefaultMeditationFile

        public IQueryable<DefaultMeditationFileView> GetDefaultMeditationFilesViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DefaultMeditationFile>()
                       join a in appDBManager.GetTable<EnumType>() on item.PredefinedMeditationTypeId equals a.EnumTypeId

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new DefaultMeditationFileView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       PredefinedMeditationTypeName = a.EnumTypeName,

                       DefaultMeditationFileId = item.DefaultMeditationFileId,
                       DefaultMeditationFileName = item.DefaultMeditationFileName,
                       PredefinedMeditationTypeId = item.PredefinedMeditationTypeId,
                       Location = item.Location,
                       FileName = item.FileName,
                       FileSize = item.FileSize,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<DefaultMeditationFileView> GetDefaultMeditationFilesInternalWithSearchCriteriaAsync(DefaultMeditationFileSearchCriteria search)
        {
            var sql1 = GetDefaultMeditationFilesViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.DefaultMeditationFileId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.DefaultMeditationFileName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Location.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.FileName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
);
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

    if (search.PredefinedMeditationTypeIds != null && search.PredefinedMeditationTypeIds.Count > 0)
                    sql1 = sql1.Where(x => search.PredefinedMeditationTypeIds.Contains(x.PredefinedMeditationTypeId));
                if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                    sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
                if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                    sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

    if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<DefaultMeditationFileView>> GetDefaultMeditationFilesInternalAsync(DefaultMeditationFileSearchCriteria search)
        {
            var sql1 = GetDefaultMeditationFilesInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.DefaultMeditationFileId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetDefaultMeditationFilesViewAsIQueryable().FirstOrDefaultAsync(x => x.DefaultMeditationFileId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<DefaultMeditationFileView>> GetDefaultMeditationFilesAsync(DefaultMeditationFileSearchCriteria search)
        {
            return GetDefaultMeditationFilesInternalAsync(search);
        }

        public Task<DefaultMeditationFile> GetDefaultMeditationFileFromAnyByIdAsync(long defaultMeditationFileId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultMeditationFile>(x => x.DefaultMeditationFileId == defaultMeditationFileId);
        }

        public Task<DefaultMeditationFileView> GetDefaultMeditationFileViewFromAnyByIdAsync(long defaultMeditationFileId)
        {
            return GetDefaultMeditationFilesViewAsIQueryable().FirstOrDefaultAsync(x => x.DefaultMeditationFileId == defaultMeditationFileId);
        }

        public Task<DefaultMeditationFile> GetDefaultMeditationFileByUniqueIdentifierAsync(string defaultMeditationFileId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultMeditationFile>(x => x.DefaultMeditationFileId == SafeUtils.Int(defaultMeditationFileId) && x.IsDeleted == false);
        }

        public Task<DefaultMeditationFile> GetDefaultMeditationFileByIdAsync(long defaultMeditationFileId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultMeditationFile>(x => x.DefaultMeditationFileId == defaultMeditationFileId && x.IsDeleted == false);
        }

        public Task<List<DefaultMeditationFile>> GetDefaultMeditationFileByIdsAsync(List<long> defaultMeditationFileIds)
        {
            return appDBManager.GetTable<DefaultMeditationFile>().Where(x => defaultMeditationFileIds.Contains(x.DefaultMeditationFileId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<DefaultMeditationFileView> GetDefaultMeditationFileViewByIdAsync(long defaultMeditationFileId)
        {
            var sql1 = GetDefaultMeditationFilesViewAsIQueryable();

            sql1 = sql1.Where(x => x.DefaultMeditationFileId == defaultMeditationFileId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<DefaultMeditationFile>> GetAllDefaultMeditationFiles()
        {
            return appDBManager.SelectAllAsync<DefaultMeditationFile>();
        }

        /*
        public Task<DefaultMeditationFile> GetDefaultMeditationFileByDefaultMeditationFileCodeAsync(long userId, string defaultMeditationFileCode)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultMeditationFile>(x => x.UserId == userId && x.DefaultMeditationFileCode == defaultMeditationFileCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateDefaultMeditationFileCodeAsync(long userId, long defaultMeditationFileId, string defaultMeditationFileCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultMeditationFile>().Where(x => x.UserId == userId && x.DefaultMeditationFileId == defaultMeditationFileId && x.IsDeleted == false).Set(x => x.DefaultMeditationFileCode, defaultMeditationFileCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<DefaultMeditationFile> CreateDefaultMeditationFileAsync(DefaultMeditationFile defaultMeditationFile, long? createdBy)
        {
            defaultMeditationFile.DefaultMeditationFileId = await appDBManager.InsertWithAuditAsync<DefaultMeditationFile, long>(defaultMeditationFile, createdBy);
            return defaultMeditationFile;
        }

        public async Task<List<DefaultMeditationFile>> CreateDefaultMeditationFilesAsync(List<DefaultMeditationFile> defaultMeditationFiles, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<DefaultMeditationFile, long>(defaultMeditationFiles, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                defaultMeditationFiles[i].DefaultMeditationFileId = lst[i];
            return defaultMeditationFiles;
        }

        public Task<bool> UpdateDefaultMeditationFileAsync(DefaultMeditationFile defaultMeditationFile, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<DefaultMeditationFile>(defaultMeditationFile, modifiedBy);
        }

        public async Task<bool> MarkDefaultMeditationFileAsNotDeletedByIdAsync(long defaultMeditationFileId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultMeditationFile>().Where(x => x.DefaultMeditationFileId == defaultMeditationFileId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkDefaultMeditationFileAsDeletedByIdAsync(long defaultMeditationFileId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultMeditationFile>().Where(x => x.DefaultMeditationFileId == defaultMeditationFileId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteDefaultMeditationFileByIdAsync(long defaultMeditationFileId)
        {
            return await appDBManager.GetTable<DefaultMeditationFile>().Where(x => x.DefaultMeditationFileId == defaultMeditationFileId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetDefaultMeditationFileActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.DefaultMeditationFile);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsDefaultMeditationFileNameAlreadyExistsForOtherDefaultMeditationFileAsync(long defaultMeditationFileId, string defaultMeditationFileName)
        {
            var sql = GetDefaultMeditationFilesViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultMeditationFileId != defaultMeditationFileId && x.DefaultMeditationFileName.ToLower().Trim() == defaultMeditationFileName.ToLower().Trim());
            return sql.AnyAsync();
        }

        public Task<bool> IsDefaultMeditationFileNameAlreadyExistsForOtherDefaultMeditationFileAsync(string defaultMeditationFileName)
        {
            var sql = GetDefaultMeditationFilesViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultMeditationFileName.ToLower().Trim() == defaultMeditationFileName.ToLower().Trim());
            return sql.AnyAsync();
        }

        


        /*

        public async Task<bool> UpdateDefaultMeditationFilePicturePathAsync(long defaultMeditationFileId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultMeditationFile>().Where(x => x.DefaultMeditationFileId == defaultMeditationFileId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearDefaultMeditationFileProfilePathAsync(long defaultMeditationFileId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultMeditationFile>().Where(x => x.DefaultMeditationFileId == defaultMeditationFileId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableDefaultMeditationFileByIdAsync(long defaultMeditationFileId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultMeditationFile>().Where(x => x.DefaultMeditationFileId == defaultMeditationFileId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableDefaultMeditationFileByIdAsync(long defaultMeditationFileId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultMeditationFile>().Where(x => x.DefaultMeditationFileId == defaultMeditationFileId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateDefaultMeditationFileWithXYZZYXAsync(long defaultMeditationFileId, long xYZZYXId, long? createdBy)
        {
            var item = new DefaultMeditationFileXYZZYXAssociationModel();
            item.DefaultMeditationFileId = defaultMeditationFileId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<DefaultMeditationFileXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateDefaultMeditationFileWithXYZZYXAsync(long defaultMeditationFileId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<DefaultMeditationFileXYZZYX>(x => x.DefaultMeditationFileId == defaultMeditationFileId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesDefaultMeditationFileWithXYZZYXAssociationExistsAsync(long defaultMeditationFileId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<DefaultMeditationFileXYZZYX>(x => x.DefaultMeditationFileId == defaultMeditationFileId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<DefaultMeditationFileView>> GetDefaultMeditationFilesByXYZZYXAsync(long xYZZYXId)
        {
            var defaultMeditationFileIds = GetDefaultMeditationFileXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.DefaultMeditationFileId);
            var defaultMeditationFileSql = GetDefaultMeditationFilesViewAsIQueryable().Where(x => defaultMeditationFileIds.Contains(x.DefaultMeditationFileId));
            return defaultMeditationFileSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByDefaultMeditationFilesAsync(long defaultMeditationFileId)
        {
            var xYZZYXIds = GetDefaultMeditationFileXYZZYXsViewAsIQueryable().Where(x => x.DefaultMeditationFileId == defaultMeditationFile).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<DefaultMeditationFileXYZZYX> GetDefaultMeditationFileXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DefaultMeditationFileXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetDefaultMeditationFileXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.DefaultMeditationFileXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region DefaultVisionBoardPicture

        public IQueryable<DefaultVisionBoardPictureView> GetDefaultVisionBoardPicturesViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DefaultVisionBoardPicture>()
                       
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new DefaultVisionBoardPictureView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       
DefaultVisionBoardPictureId = item.DefaultVisionBoardPictureId,
Location = item.Location,
FileName = item.FileName,
FileSize = item.FileSize,
IsDeleted = item.IsDeleted,
CreatedDate = item.CreatedDate,
CreatedByUserId = item.CreatedByUserId,
ModifiedDate = item.ModifiedDate,
ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<DefaultVisionBoardPictureView> GetDefaultVisionBoardPicturesInternalWithSearchCriteriaAsync(DefaultVisionBoardPictureSearchCriteria search)
        {
            var sql1 = GetDefaultVisionBoardPicturesViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.DefaultVisionBoardPictureId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Location.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.FileName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
);
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

    if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                    sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
                if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                    sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

    if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<DefaultVisionBoardPictureView>> GetDefaultVisionBoardPicturesInternalAsync(DefaultVisionBoardPictureSearchCriteria search)
        {
            var sql1 = GetDefaultVisionBoardPicturesInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.DefaultVisionBoardPictureId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetDefaultVisionBoardPicturesViewAsIQueryable().FirstOrDefaultAsync(x => x.DefaultVisionBoardPictureId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<DefaultVisionBoardPictureView>> GetDefaultVisionBoardPicturesAsync(DefaultVisionBoardPictureSearchCriteria search)
        {
            return GetDefaultVisionBoardPicturesInternalAsync(search);
        }

        public Task<DefaultVisionBoardPicture> GetDefaultVisionBoardPictureFromAnyByIdAsync(long defaultVisionBoardPictureId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultVisionBoardPicture>(x => x.DefaultVisionBoardPictureId == defaultVisionBoardPictureId);
        }

        public Task<DefaultVisionBoardPictureView> GetDefaultVisionBoardPictureViewFromAnyByIdAsync(long defaultVisionBoardPictureId)
        {
            return GetDefaultVisionBoardPicturesViewAsIQueryable().FirstOrDefaultAsync(x => x.DefaultVisionBoardPictureId == defaultVisionBoardPictureId);
        }

        public Task<DefaultVisionBoardPicture> GetDefaultVisionBoardPictureByUniqueIdentifierAsync(string defaultVisionBoardPictureId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultVisionBoardPicture>(x => x.DefaultVisionBoardPictureId == SafeUtils.Int(defaultVisionBoardPictureId) && x.IsDeleted == false);
        }

        public Task<DefaultVisionBoardPicture> GetDefaultVisionBoardPictureByIdAsync(long defaultVisionBoardPictureId)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultVisionBoardPicture>(x => x.DefaultVisionBoardPictureId == defaultVisionBoardPictureId && x.IsDeleted == false);
        }

        public Task<List<DefaultVisionBoardPicture>> GetDefaultVisionBoardPictureByIdsAsync(List<long> defaultVisionBoardPictureIds)
        {
            return appDBManager.GetTable<DefaultVisionBoardPicture>().Where(x => defaultVisionBoardPictureIds.Contains(x.DefaultVisionBoardPictureId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<DefaultVisionBoardPictureView> GetDefaultVisionBoardPictureViewByIdAsync(long defaultVisionBoardPictureId)
        {
            var sql1 = GetDefaultVisionBoardPicturesViewAsIQueryable();

            sql1 = sql1.Where(x => x.DefaultVisionBoardPictureId == defaultVisionBoardPictureId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<DefaultVisionBoardPicture>> GetAllDefaultVisionBoardPictures()
        {
            return appDBManager.SelectAllAsync<DefaultVisionBoardPicture>();
        }

        /*
        public Task<DefaultVisionBoardPicture> GetDefaultVisionBoardPictureByDefaultVisionBoardPictureCodeAsync(long userId, string defaultVisionBoardPictureCode)
        {
            return appDBManager.FirstOrDefaultAsync<DefaultVisionBoardPicture>(x => x.UserId == userId && x.DefaultVisionBoardPictureCode == defaultVisionBoardPictureCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateDefaultVisionBoardPictureCodeAsync(long userId, long defaultVisionBoardPictureId, string defaultVisionBoardPictureCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultVisionBoardPicture>().Where(x => x.UserId == userId && x.DefaultVisionBoardPictureId == defaultVisionBoardPictureId && x.IsDeleted == false).Set(x => x.DefaultVisionBoardPictureCode, defaultVisionBoardPictureCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<DefaultVisionBoardPicture> CreateDefaultVisionBoardPictureAsync(DefaultVisionBoardPicture defaultVisionBoardPicture, long? createdBy)
        {
            defaultVisionBoardPicture.DefaultVisionBoardPictureId = await appDBManager.InsertWithAuditAsync<DefaultVisionBoardPicture, long>(defaultVisionBoardPicture, createdBy);
            return defaultVisionBoardPicture;
        }

        public async Task<List<DefaultVisionBoardPicture>> CreateDefaultVisionBoardPicturesAsync(List<DefaultVisionBoardPicture> defaultVisionBoardPictures, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<DefaultVisionBoardPicture, long>(defaultVisionBoardPictures, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                defaultVisionBoardPictures[i].DefaultVisionBoardPictureId = lst[i];
            return defaultVisionBoardPictures;
        }

        public Task<bool> UpdateDefaultVisionBoardPictureAsync(DefaultVisionBoardPicture defaultVisionBoardPicture, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<DefaultVisionBoardPicture>(defaultVisionBoardPicture, modifiedBy);
        }

        public async Task<bool> MarkDefaultVisionBoardPictureAsNotDeletedByIdAsync(long defaultVisionBoardPictureId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultVisionBoardPicture>().Where(x => x.DefaultVisionBoardPictureId == defaultVisionBoardPictureId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkDefaultVisionBoardPictureAsDeletedByIdAsync(long defaultVisionBoardPictureId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultVisionBoardPicture>().Where(x => x.DefaultVisionBoardPictureId == defaultVisionBoardPictureId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteDefaultVisionBoardPictureByIdAsync(long defaultVisionBoardPictureId)
        {
            return await appDBManager.GetTable<DefaultVisionBoardPicture>().Where(x => x.DefaultVisionBoardPictureId == defaultVisionBoardPictureId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetDefaultVisionBoardPictureActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.DefaultVisionBoardPicture);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsDefaultVisionBoardPictureNameAlreadyExistsForOtherDefaultVisionBoardPictureAsync(long defaultVisionBoardPictureId, string fileName)
        {
            var sql = GetDefaultVisionBoardPicturesViewAsIQueryable().Where(x => x.IsDeleted == false && x.DefaultVisionBoardPictureId != defaultVisionBoardPictureId && x.FileName.ToLower().Trim() == fileName.ToLower().Trim());
            return sql.AnyAsync();
        }

        public Task<bool> IsDefaultVisionBoardPictureNameAlreadyExistsForOtherDefaultVisionBoardPictureAsync(string fileName)
        {
            var sql = GetDefaultVisionBoardPicturesViewAsIQueryable().Where(x => x.IsDeleted == false && x.FileName.ToLower().Trim() == fileName.ToLower().Trim());
            return sql.AnyAsync();
        }

        


        /*

        public async Task<bool> UpdateDefaultVisionBoardPicturePicturePathAsync(long defaultVisionBoardPictureId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultVisionBoardPicture>().Where(x => x.DefaultVisionBoardPictureId == defaultVisionBoardPictureId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearDefaultVisionBoardPictureProfilePathAsync(long defaultVisionBoardPictureId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultVisionBoardPicture>().Where(x => x.DefaultVisionBoardPictureId == defaultVisionBoardPictureId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableDefaultVisionBoardPictureByIdAsync(long defaultVisionBoardPictureId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultVisionBoardPicture>().Where(x => x.DefaultVisionBoardPictureId == defaultVisionBoardPictureId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableDefaultVisionBoardPictureByIdAsync(long defaultVisionBoardPictureId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DefaultVisionBoardPicture>().Where(x => x.DefaultVisionBoardPictureId == defaultVisionBoardPictureId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateDefaultVisionBoardPictureWithXYZZYXAsync(long defaultVisionBoardPictureId, long xYZZYXId, long? createdBy)
        {
            var item = new DefaultVisionBoardPictureXYZZYXAssociationModel();
            item.DefaultVisionBoardPictureId = defaultVisionBoardPictureId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<DefaultVisionBoardPictureXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateDefaultVisionBoardPictureWithXYZZYXAsync(long defaultVisionBoardPictureId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<DefaultVisionBoardPictureXYZZYX>(x => x.DefaultVisionBoardPictureId == defaultVisionBoardPictureId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesDefaultVisionBoardPictureWithXYZZYXAssociationExistsAsync(long defaultVisionBoardPictureId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<DefaultVisionBoardPictureXYZZYX>(x => x.DefaultVisionBoardPictureId == defaultVisionBoardPictureId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<DefaultVisionBoardPictureView>> GetDefaultVisionBoardPicturesByXYZZYXAsync(long xYZZYXId)
        {
            var defaultVisionBoardPictureIds = GetDefaultVisionBoardPictureXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.DefaultVisionBoardPictureId);
            var defaultVisionBoardPictureSql = GetDefaultVisionBoardPicturesViewAsIQueryable().Where(x => defaultVisionBoardPictureIds.Contains(x.DefaultVisionBoardPictureId));
            return defaultVisionBoardPictureSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByDefaultVisionBoardPicturesAsync(long defaultVisionBoardPictureId)
        {
            var xYZZYXIds = GetDefaultVisionBoardPictureXYZZYXsViewAsIQueryable().Where(x => x.DefaultVisionBoardPictureId == defaultVisionBoardPicture).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<DefaultVisionBoardPictureXYZZYX> GetDefaultVisionBoardPictureXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DefaultVisionBoardPictureXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetDefaultVisionBoardPictureXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.DefaultVisionBoardPictureXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region Feed

        public IQueryable<FeedView> GetFeedsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<Feed>()
                       join a in appDBManager.GetTable<User>() on item.UserId equals a.UserId
                   join ob in appDBManager.GetTable<DailyAffirmationSession>() on item.DailyAffirmationSessionId equals ob.DailyAffirmationSessionId into bTemp
                   from b in bTemp.DefaultIfEmpty()
                   join oc in appDBManager.GetTable<VisionBoardSession>() on item.VisionBoardSessionId equals oc.VisionBoardSessionId into cTemp
                   from c in cTemp.DefaultIfEmpty()
                   join od in appDBManager.GetTable<MeditationSession>() on item.MeditationSessionId equals od.MeditationSessionId into dTemp
                   from d in dTemp.DefaultIfEmpty()
                   join oe in appDBManager.GetTable<EnumType>() on item.PredefinedMeditationTypeId equals oe.EnumTypeId into eTemp
                   from e in eTemp.DefaultIfEmpty()
                   join of in appDBManager.GetTable<GoalTracker>() on item.GoalTrackerId equals of.GoalTrackerId into fTemp
                   from f in fTemp.DefaultIfEmpty()
                   join og in appDBManager.GetTable<EnumType>() on item.PredefinedGoalStatusTypeId equals og.EnumTypeId into gTemp
                   from g in gTemp.DefaultIfEmpty()
                   join oh in appDBManager.GetTable<GoalTrackerMileStone>() on item.GoalTrackerMileStoneId equals oh.GoalTrackerMileStoneId into hTemp
                   from h in hTemp.DefaultIfEmpty()

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new FeedView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       UserName = a.UserName,
                       PredefinedMeditationTypeName = e.EnumTypeName,
                       PredefinedGoalStatusTypeName = g.EnumTypeName,

                       FeedId = item.FeedId,
                       FeedGuid = item.FeedGuid,
                       UserId = item.UserId,
                       DailyAffirmationSessionId = item.DailyAffirmationSessionId,
                       DailyAffirmationSessionDate = item.DailyAffirmationSessionDate,
                       DailyAffirmationTimeSpentInSeconds = item.DailyAffirmationTimeSpentInSeconds,
                       VisionBoardSessionId = item.VisionBoardSessionId,
                       VisionBoardSessionDate = item.VisionBoardSessionDate,
                       VisionBoardSessionTimeSpentInSeconds = item.VisionBoardSessionTimeSpentInSeconds,
                       MeditationSessionId = item.MeditationSessionId,
                       MeditationSessionDate = item.MeditationSessionDate,
                       MeditationSessionTimeSpentInSeconds = item.MeditationSessionTimeSpentInSeconds,
                       MeditationFileName = item.MeditationFileName,
                       PredefinedMeditationTypeId = item.PredefinedMeditationTypeId,
                       GoalTrackerId = item.GoalTrackerId,
                       GoalTrackerName = item.GoalTrackerName,
                       PredefinedGoalStatusTypeId = item.PredefinedGoalStatusTypeId,
                       GoalTrackerStartDate = item.GoalTrackerStartDate,
                       GoalTrackerEndDate = item.GoalTrackerEndDate,
                       GoalTrackerCompletedDate = item.GoalTrackerCompletedDate,
                       GoalTrackerMileStoneId = item.GoalTrackerMileStoneId,
                       GoalTrackerMileStoneName = item.GoalTrackerMileStoneName,
                       GoalTrackerMileStoneStartDate = item.GoalTrackerMileStoneStartDate,
                       GoalTrackerMileStoneEndDate = item.GoalTrackerMileStoneEndDate,
                       GoalTrackerMileStoneCompletedDate = item.GoalTrackerMileStoneCompletedDate,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<FeedView> GetFeedsInternalWithSearchCriteriaAsync(FeedSearchCriteria search)
        {
            var sql1 = GetFeedsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.FeedId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.MeditationFileName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.GoalTrackerName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.GoalTrackerMileStoneName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
);
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

    if (search.UserId != null && search.UserId > 0)
                    sql1 = sql1.Where(x => search.UserId == x.UserId);
                if (search.DailyAffirmationSessionId != null && search.DailyAffirmationSessionId > 0)
                    sql1 = sql1.Where(x => search.DailyAffirmationSessionId == x.DailyAffirmationSessionId);
                if (search.VisionBoardSessionId != null && search.VisionBoardSessionId > 0)
                    sql1 = sql1.Where(x => search.VisionBoardSessionId == x.VisionBoardSessionId);
                if (search.MeditationSessionId != null && search.MeditationSessionId > 0)
                    sql1 = sql1.Where(x => search.MeditationSessionId == x.MeditationSessionId);
                if (search.PredefinedMeditationTypeIds != null && search.PredefinedMeditationTypeIds.Count > 0)
                    sql1 = sql1.Where(x => search.PredefinedMeditationTypeIds.Contains(x.PredefinedMeditationTypeId ?? 0));
                if (search.GoalTrackerId != null && search.GoalTrackerId > 0)
                    sql1 = sql1.Where(x => search.GoalTrackerId == x.GoalTrackerId);
                if (search.PredefinedGoalStatusTypeIds != null && search.PredefinedGoalStatusTypeIds.Count > 0)
                    sql1 = sql1.Where(x => search.PredefinedGoalStatusTypeIds.Contains(x.PredefinedGoalStatusTypeId ?? 0));
                if (search.GoalTrackerMileStoneId != null && search.GoalTrackerMileStoneId > 0)
                    sql1 = sql1.Where(x => search.GoalTrackerMileStoneId == x.GoalTrackerMileStoneId);
                if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                    sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
                if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                    sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

    if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<FeedView>> GetFeedsInternalAsync(FeedSearchCriteria search)
        {
            var sql1 = GetFeedsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.FeedId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetFeedsViewAsIQueryable().FirstOrDefaultAsync(x => x.FeedId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<FeedView>> GetFeedsAsync(FeedSearchCriteria search)
        {
            return GetFeedsInternalAsync(search);
        }

        public Task<Feed> GetFeedFromAnyByIdAsync(long feedId)
        {
            return appDBManager.FirstOrDefaultAsync<Feed>(x => x.FeedId == feedId);
        }

        public Task<FeedView> GetFeedViewFromAnyByIdAsync(long feedId)
        {
            return GetFeedsViewAsIQueryable().FirstOrDefaultAsync(x => x.FeedId == feedId);
        }

        public Task<Feed> GetFeedByUniqueIdentifierAsync(string feedId)
        {
            return appDBManager.FirstOrDefaultAsync<Feed>(x => x.FeedId == SafeUtils.Int(feedId) && x.IsDeleted == false);
        }

        public Task<Feed> GetFeedByIdAsync(long feedId)
        {
            return appDBManager.FirstOrDefaultAsync<Feed>(x => x.FeedId == feedId && x.IsDeleted == false);
        }

        public Task<List<Feed>> GetFeedByIdsAsync(List<long> feedIds)
        {
            return appDBManager.GetTable<Feed>().Where(x => feedIds.Contains(x.FeedId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<FeedView> GetFeedViewByIdAsync(long feedId)
        {
            var sql1 = GetFeedsViewAsIQueryable();

            sql1 = sql1.Where(x => x.FeedId == feedId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<Feed>> GetAllFeeds()
        {
            return appDBManager.SelectAllAsync<Feed>();
        }

        /*
        public Task<Feed> GetFeedByFeedCodeAsync(long userId, string feedCode)
        {
            return appDBManager.FirstOrDefaultAsync<Feed>(x => x.UserId == userId && x.FeedCode == feedCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateFeedCodeAsync(long userId, long feedId, string feedCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<Feed>().Where(x => x.UserId == userId && x.FeedId == feedId && x.IsDeleted == false).Set(x => x.FeedCode, feedCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<Feed> CreateFeedAsync(Feed feed, long? createdBy)
        {
            feed.FeedId = await appDBManager.InsertWithAuditAsync<Feed, long>(feed, createdBy);
            return feed;
        }

        public async Task<List<Feed>> CreateFeedsAsync(List<Feed> feeds, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<Feed, long>(feeds, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                feeds[i].FeedId = lst[i];
            return feeds;
        }

        public Task<bool> UpdateFeedAsync(Feed feed, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<Feed>(feed, modifiedBy);
        }

        public async Task<bool> MarkFeedAsNotDeletedByIdAsync(long feedId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Feed>().Where(x => x.FeedId == feedId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkFeedAsDeletedByIdAsync(long feedId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Feed>().Where(x => x.FeedId == feedId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteFeedByIdAsync(long feedId)
        {
            return await appDBManager.GetTable<Feed>().Where(x => x.FeedId == feedId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetFeedActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.Feed);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<List<FeedView>> GetFeedViewByUserIdAsync(long userId)
        {
                return GetFeedsViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<FeedView>> GetFeedViewByUserIdAsync(FeedByUserIdSearchCriteria search)
        {
                return GetFeedsViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }

        public Task<List<Feed>> GetBareFeedByUserIdAsync(long userId)
        {
                return appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<Feed>> GetBareFeedByUserIdAsync(FeedByUserIdSearchCriteria search)
        {
                return appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }


        public Task<List<UserView>> GetUserViewByFeedIdAsync(long feedId)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == feedId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<UserView>> GetUserViewByFeedIdAsync(UserByFeedIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == search.FeedId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }

        public Task<List<User>> GetBareUserByFeedIdAsync(long feedId)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == feedId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<User>> GetBareUserByFeedIdAsync(UserByFeedIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == search.FeedId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }


        public Task<List<FeedView>> GetFeedViewByDailyAffirmationSessionIdAsync(long dailyAffirmationSessionId)
        {
                return GetFeedsViewAsIQueryable().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionId == dailyAffirmationSessionId).ToListAsync();
        }

        public Task<DbReturnListModel<FeedView>> GetFeedViewByDailyAffirmationSessionIdAsync(FeedByDailyAffirmationSessionIdSearchCriteria search)
        {
                return GetFeedsViewAsIQueryable().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionId == search.DailyAffirmationSessionId).ReturnListModelResultAsync(search);
        }

        public Task<List<Feed>> GetBareFeedByDailyAffirmationSessionIdAsync(long dailyAffirmationSessionId)
        {
                return appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionId == dailyAffirmationSessionId).ToListAsync();
        }

        public Task<DbReturnListModel<Feed>> GetBareFeedByDailyAffirmationSessionIdAsync(FeedByDailyAffirmationSessionIdSearchCriteria search)
        {
                return appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.DailyAffirmationSessionId == search.DailyAffirmationSessionId).ReturnListModelResultAsync(search);
        }


        public Task<List<DailyAffirmationSessionView>> GetDailyAffirmationSessionViewByFeedIdAsync(long feedId)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == feedId).Select(x => x.DailyAffirmationSessionId);
                return GetDailyAffirmationSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.DailyAffirmationSessionId)).ToListAsync();
        }

        public Task<DbReturnListModel<DailyAffirmationSessionView>> GetDailyAffirmationSessionViewByFeedIdAsync(DailyAffirmationSessionByFeedIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == search.FeedId).Select(x => x.DailyAffirmationSessionId);
                return GetDailyAffirmationSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.DailyAffirmationSessionId)).ReturnListModelResultAsync(search);
        }

        public Task<List<DailyAffirmationSession>> GetBareDailyAffirmationSessionByFeedIdAsync(long feedId)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == feedId).Select(x => x.DailyAffirmationSessionId);
                return appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.IsDeleted == false && idsSql.Contains(x.DailyAffirmationSessionId)).ToListAsync();
        }

        public Task<DbReturnListModel<DailyAffirmationSession>> GetBareDailyAffirmationSessionByFeedIdAsync(DailyAffirmationSessionByFeedIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == search.FeedId).Select(x => x.DailyAffirmationSessionId);
                return appDBManager.GetTable<DailyAffirmationSession>().Where(x => x.IsDeleted == false && idsSql.Contains(x.DailyAffirmationSessionId)).ReturnListModelResultAsync(search);
        }


        public Task<List<FeedView>> GetFeedViewByVisionBoardSessionIdAsync(long visionBoardSessionId)
        {
                return GetFeedsViewAsIQueryable().Where(x => x.IsDeleted == false && x.VisionBoardSessionId == visionBoardSessionId).ToListAsync();
        }

        public Task<DbReturnListModel<FeedView>> GetFeedViewByVisionBoardSessionIdAsync(FeedByVisionBoardSessionIdSearchCriteria search)
        {
                return GetFeedsViewAsIQueryable().Where(x => x.IsDeleted == false && x.VisionBoardSessionId == search.VisionBoardSessionId).ReturnListModelResultAsync(search);
        }

        public Task<List<Feed>> GetBareFeedByVisionBoardSessionIdAsync(long visionBoardSessionId)
        {
                return appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.VisionBoardSessionId == visionBoardSessionId).ToListAsync();
        }

        public Task<DbReturnListModel<Feed>> GetBareFeedByVisionBoardSessionIdAsync(FeedByVisionBoardSessionIdSearchCriteria search)
        {
                return appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.VisionBoardSessionId == search.VisionBoardSessionId).ReturnListModelResultAsync(search);
        }


        public Task<List<VisionBoardSessionView>> GetVisionBoardSessionViewByFeedIdAsync(long feedId)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == feedId).Select(x => x.VisionBoardSessionId);
                return GetVisionBoardSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.VisionBoardSessionId)).ToListAsync();
        }

        public Task<DbReturnListModel<VisionBoardSessionView>> GetVisionBoardSessionViewByFeedIdAsync(VisionBoardSessionByFeedIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == search.FeedId).Select(x => x.VisionBoardSessionId);
                return GetVisionBoardSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.VisionBoardSessionId)).ReturnListModelResultAsync(search);
        }

        public Task<List<VisionBoardSession>> GetBareVisionBoardSessionByFeedIdAsync(long feedId)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == feedId).Select(x => x.VisionBoardSessionId);
                return appDBManager.GetTable<VisionBoardSession>().Where(x => x.IsDeleted == false && idsSql.Contains(x.VisionBoardSessionId)).ToListAsync();
        }

        public Task<DbReturnListModel<VisionBoardSession>> GetBareVisionBoardSessionByFeedIdAsync(VisionBoardSessionByFeedIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == search.FeedId).Select(x => x.VisionBoardSessionId);
                return appDBManager.GetTable<VisionBoardSession>().Where(x => x.IsDeleted == false && idsSql.Contains(x.VisionBoardSessionId)).ReturnListModelResultAsync(search);
        }


        public Task<List<FeedView>> GetFeedViewByMeditationSessionIdAsync(long meditationSessionId)
        {
                return GetFeedsViewAsIQueryable().Where(x => x.IsDeleted == false && x.MeditationSessionId == meditationSessionId).ToListAsync();
        }

        public Task<DbReturnListModel<FeedView>> GetFeedViewByMeditationSessionIdAsync(FeedByMeditationSessionIdSearchCriteria search)
        {
                return GetFeedsViewAsIQueryable().Where(x => x.IsDeleted == false && x.MeditationSessionId == search.MeditationSessionId).ReturnListModelResultAsync(search);
        }

        public Task<List<Feed>> GetBareFeedByMeditationSessionIdAsync(long meditationSessionId)
        {
                return appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.MeditationSessionId == meditationSessionId).ToListAsync();
        }

        public Task<DbReturnListModel<Feed>> GetBareFeedByMeditationSessionIdAsync(FeedByMeditationSessionIdSearchCriteria search)
        {
                return appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.MeditationSessionId == search.MeditationSessionId).ReturnListModelResultAsync(search);
        }


        public Task<List<MeditationSessionView>> GetMeditationSessionViewByFeedIdAsync(long feedId)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == feedId).Select(x => x.MeditationSessionId);
                return GetMeditationSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.MeditationSessionId)).ToListAsync();
        }

        public Task<DbReturnListModel<MeditationSessionView>> GetMeditationSessionViewByFeedIdAsync(MeditationSessionByFeedIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == search.FeedId).Select(x => x.MeditationSessionId);
                return GetMeditationSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.MeditationSessionId)).ReturnListModelResultAsync(search);
        }

        public Task<List<MeditationSession>> GetBareMeditationSessionByFeedIdAsync(long feedId)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == feedId).Select(x => x.MeditationSessionId);
                return appDBManager.GetTable<MeditationSession>().Where(x => x.IsDeleted == false && idsSql.Contains(x.MeditationSessionId)).ToListAsync();
        }

        public Task<DbReturnListModel<MeditationSession>> GetBareMeditationSessionByFeedIdAsync(MeditationSessionByFeedIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == search.FeedId).Select(x => x.MeditationSessionId);
                return appDBManager.GetTable<MeditationSession>().Where(x => x.IsDeleted == false && idsSql.Contains(x.MeditationSessionId)).ReturnListModelResultAsync(search);
        }


        public Task<List<FeedView>> GetFeedViewByGoalTrackerIdAsync(long goalTrackerId)
        {
                return GetFeedsViewAsIQueryable().Where(x => x.IsDeleted == false && x.GoalTrackerId == goalTrackerId).ToListAsync();
        }

        public Task<DbReturnListModel<FeedView>> GetFeedViewByGoalTrackerIdAsync(FeedByGoalTrackerIdSearchCriteria search)
        {
                return GetFeedsViewAsIQueryable().Where(x => x.IsDeleted == false && x.GoalTrackerId == search.GoalTrackerId).ReturnListModelResultAsync(search);
        }

        public Task<List<Feed>> GetBareFeedByGoalTrackerIdAsync(long goalTrackerId)
        {
                return appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.GoalTrackerId == goalTrackerId).ToListAsync();
        }

        public Task<DbReturnListModel<Feed>> GetBareFeedByGoalTrackerIdAsync(FeedByGoalTrackerIdSearchCriteria search)
        {
                return appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.GoalTrackerId == search.GoalTrackerId).ReturnListModelResultAsync(search);
        }


        public Task<List<GoalTrackerView>> GetGoalTrackerViewByFeedIdAsync(long feedId)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == feedId).Select(x => x.GoalTrackerId);
                return GetGoalTrackersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.GoalTrackerId)).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerView>> GetGoalTrackerViewByFeedIdAsync(GoalTrackerByFeedIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == search.FeedId).Select(x => x.GoalTrackerId);
                return GetGoalTrackersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.GoalTrackerId)).ReturnListModelResultAsync(search);
        }

        public Task<List<GoalTracker>> GetBareGoalTrackerByFeedIdAsync(long feedId)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == feedId).Select(x => x.GoalTrackerId);
                return appDBManager.GetTable<GoalTracker>().Where(x => x.IsDeleted == false && idsSql.Contains(x.GoalTrackerId)).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTracker>> GetBareGoalTrackerByFeedIdAsync(GoalTrackerByFeedIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == search.FeedId).Select(x => x.GoalTrackerId);
                return appDBManager.GetTable<GoalTracker>().Where(x => x.IsDeleted == false && idsSql.Contains(x.GoalTrackerId)).ReturnListModelResultAsync(search);
        }


        public Task<List<FeedView>> GetFeedViewByGoalTrackerMileStoneIdAsync(long goalTrackerMileStoneId)
        {
                return GetFeedsViewAsIQueryable().Where(x => x.IsDeleted == false && x.GoalTrackerMileStoneId == goalTrackerMileStoneId).ToListAsync();
        }

        public Task<DbReturnListModel<FeedView>> GetFeedViewByGoalTrackerMileStoneIdAsync(FeedByGoalTrackerMileStoneIdSearchCriteria search)
        {
                return GetFeedsViewAsIQueryable().Where(x => x.IsDeleted == false && x.GoalTrackerMileStoneId == search.GoalTrackerMileStoneId).ReturnListModelResultAsync(search);
        }

        public Task<List<Feed>> GetBareFeedByGoalTrackerMileStoneIdAsync(long goalTrackerMileStoneId)
        {
                return appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.GoalTrackerMileStoneId == goalTrackerMileStoneId).ToListAsync();
        }

        public Task<List<GoalTrackerCategory>> GetGoalTrackerCategorysByGoalTrackerIdAsync(long goalTrackerId)
        {
            return appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.GoalTrackerId == goalTrackerId).ToListAsync();
        }

        public Task<DbReturnListModel<Feed>> GetBareFeedByGoalTrackerMileStoneIdAsync(FeedByGoalTrackerMileStoneIdSearchCriteria search)
        {
                return appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.GoalTrackerMileStoneId == search.GoalTrackerMileStoneId).ReturnListModelResultAsync(search);
        }


        public Task<List<GoalTrackerMileStoneView>> GetGoalTrackerMileStoneViewByFeedIdAsync(long feedId)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == feedId).Select(x => x.GoalTrackerMileStoneId);
                return GetGoalTrackerMileStonesViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.GoalTrackerMileStoneId)).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerMileStoneView>> GetGoalTrackerMileStoneViewByFeedIdAsync(GoalTrackerMileStoneByFeedIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == search.FeedId).Select(x => x.GoalTrackerMileStoneId);
                return GetGoalTrackerMileStonesViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.GoalTrackerMileStoneId)).ReturnListModelResultAsync(search);
        }

        public Task<List<GoalTrackerMileStone>> GetBareGoalTrackerMileStoneByFeedIdAsync(long feedId)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == feedId).Select(x => x.GoalTrackerMileStoneId);
                return appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.IsDeleted == false && idsSql.Contains(x.GoalTrackerMileStoneId)).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerMileStone>> GetBareGoalTrackerMileStoneByFeedIdAsync(GoalTrackerMileStoneByFeedIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<Feed>().Where(x => x.IsDeleted == false && x.FeedId == search.FeedId).Select(x => x.GoalTrackerMileStoneId);
                return appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.IsDeleted == false && idsSql.Contains(x.GoalTrackerMileStoneId)).ReturnListModelResultAsync(search);
        }



        /*

        public async Task<bool> UpdateFeedPicturePathAsync(long feedId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<Feed>().Where(x => x.FeedId == feedId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearFeedProfilePathAsync(long feedId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Feed>().Where(x => x.FeedId == feedId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableFeedByIdAsync(long feedId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Feed>().Where(x => x.FeedId == feedId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableFeedByIdAsync(long feedId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Feed>().Where(x => x.FeedId == feedId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateFeedWithXYZZYXAsync(long feedId, long xYZZYXId, long? createdBy)
        {
            var item = new FeedXYZZYXAssociationModel();
            item.FeedId = feedId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<FeedXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateFeedWithXYZZYXAsync(long feedId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<FeedXYZZYX>(x => x.FeedId == feedId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesFeedWithXYZZYXAssociationExistsAsync(long feedId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<FeedXYZZYX>(x => x.FeedId == feedId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<FeedView>> GetFeedsByXYZZYXAsync(long xYZZYXId)
        {
            var feedIds = GetFeedXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.FeedId);
            var feedSql = GetFeedsViewAsIQueryable().Where(x => feedIds.Contains(x.FeedId));
            return feedSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByFeedsAsync(long feedId)
        {
            var xYZZYXIds = GetFeedXYZZYXsViewAsIQueryable().Where(x => x.FeedId == feed).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<FeedXYZZYX> GetFeedXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<FeedXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetFeedXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.FeedXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region GoalTracker

        public IQueryable<GoalTrackerView> GetGoalTrackersViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<GoalTracker>()
                       join a in appDBManager.GetTable<EnumType>() on item.PredefinedGoalStatusTypeId equals a.EnumTypeId
                   join b in appDBManager.GetTable<User>() on item.UserId equals b.UserId

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new GoalTrackerView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       PredefinedGoalStatusTypeName = a.EnumTypeName,
                       UserName = b.UserName,

                       GoalTrackerId = item.GoalTrackerId,
                       GoalTrackerName = item.GoalTrackerName,
                       PredefinedGoalStatusTypeId = item.PredefinedGoalStatusTypeId,
                       GoalTrackerStartDate = item.GoalTrackerStartDate,
                       GoalTrackerEndDate = item.GoalTrackerEndDate,
                       GoalTrackerCompletedDate = item.GoalTrackerCompletedDate,
                       UserId = item.UserId,
                       AddedToFeed = item.AddedToFeed,
                       AddedToFeedDate = item.AddedToFeedDate,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<GoalTrackerView> GetGoalTrackersInternalWithSearchCriteriaAsync(GoalTrackerSearchCriteria search)
        {
            var sql1 = GetGoalTrackersViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.GoalTrackerId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.GoalTrackerName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
);
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

    if (search.PredefinedGoalStatusTypeIds != null && search.PredefinedGoalStatusTypeIds.Count > 0)
                    sql1 = sql1.Where(x => search.PredefinedGoalStatusTypeIds.Contains(x.PredefinedGoalStatusTypeId));
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

        public async Task<DbReturnListModel<GoalTrackerView>> GetGoalTrackersInternalAsync(GoalTrackerSearchCriteria search)
        {
            var sql1 = GetGoalTrackersInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.GoalTrackerId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetGoalTrackersViewAsIQueryable().FirstOrDefaultAsync(x => x.GoalTrackerId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<List<GoalTrackerView>> GetGoalTrackersByUserIdAsync(long userId)
        {
            return GetGoalTrackersViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<List<GoalTrackerView>> GetGoalTrackersByCatalogIdAsync(long categoryId)
        {
            var sql = appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.CategoryId == categoryId).Select(x=> x.GoalTrackerId).Distinct();
            return GetGoalTrackersViewAsIQueryable().Where(x => x.IsDeleted == false && sql.Contains(x.GoalTrackerId)).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerView>> GetGoalTrackersAsync(GoalTrackerSearchCriteria search)
        {
            return GetGoalTrackersInternalAsync(search);
        }

        public Task<GoalTracker> GetGoalTrackerFromAnyByIdAsync(long goalTrackerId)
        {
            return appDBManager.FirstOrDefaultAsync<GoalTracker>(x => x.GoalTrackerId == goalTrackerId);
        }

        public Task<GoalTrackerView> GetGoalTrackerViewFromAnyByIdAsync(long goalTrackerId)
        {
            return GetGoalTrackersViewAsIQueryable().FirstOrDefaultAsync(x => x.GoalTrackerId == goalTrackerId);
        }

        public Task<GoalTracker> GetGoalTrackerByUniqueIdentifierAsync(string goalTrackerId)
        {
            return appDBManager.FirstOrDefaultAsync<GoalTracker>(x => x.GoalTrackerId == SafeUtils.Int(goalTrackerId) && x.IsDeleted == false);
        }

        public Task<GoalTracker> GetGoalTrackerByIdAsync(long goalTrackerId)
        {
            return appDBManager.FirstOrDefaultAsync<GoalTracker>(x => x.GoalTrackerId == goalTrackerId && x.IsDeleted == false);
        }

        public Task<List<GoalTracker>> GetGoalTrackerByIdsAsync(List<long> goalTrackerIds)
        {
            return appDBManager.GetTable<GoalTracker>().Where(x => goalTrackerIds.Contains(x.GoalTrackerId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<GoalTrackerView> GetGoalTrackerViewByIdAsync(long goalTrackerId)
        {
            var sql1 = GetGoalTrackersViewAsIQueryable();

            sql1 = sql1.Where(x => x.GoalTrackerId == goalTrackerId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<GoalTracker>> GetAllGoalTrackers()
        {
            return appDBManager.SelectAllAsync<GoalTracker>();
        }

        /*
        public Task<GoalTracker> GetGoalTrackerByGoalTrackerCodeAsync(long userId, string goalTrackerCode)
        {
            return appDBManager.FirstOrDefaultAsync<GoalTracker>(x => x.UserId == userId && x.GoalTrackerCode == goalTrackerCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateGoalTrackerCodeAsync(long userId, long goalTrackerId, string goalTrackerCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTracker>().Where(x => x.UserId == userId && x.GoalTrackerId == goalTrackerId && x.IsDeleted == false).Set(x => x.GoalTrackerCode, goalTrackerCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<GoalTracker> CreateGoalTrackerAsync(GoalTracker goalTracker, long? createdBy)
        {
            goalTracker.GoalTrackerId = await appDBManager.InsertWithAuditAsync<GoalTracker, long>(goalTracker, createdBy);
            return goalTracker;
        }

        public async Task<List<GoalTracker>> CreateGoalTrackersAsync(List<GoalTracker> goalTrackers, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<GoalTracker, long>(goalTrackers, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                goalTrackers[i].GoalTrackerId = lst[i];
            return goalTrackers;
        }

        public Task<bool> UpdateGoalTrackerAsync(GoalTracker goalTracker, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<GoalTracker>(goalTracker, modifiedBy);
        }

        public async Task<bool> MarkGoalTrackerAsNotDeletedByIdAsync(long goalTrackerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTracker>().Where(x => x.GoalTrackerId == goalTrackerId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkGoalTrackerAsDeletedByIdAsync(long goalTrackerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTracker>().Where(x => x.GoalTrackerId == goalTrackerId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteGoalTrackerByIdAsync(long goalTrackerId)
        {
            return await appDBManager.GetTable<GoalTracker>().Where(x => x.GoalTrackerId == goalTrackerId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetGoalTrackerActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.GoalTracker);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsGoalTrackerNameAlreadyExistsForOtherGoalTrackerAsync(long goalTrackerId, string goalTrackerName, long userId)
        {
            var sql = GetGoalTrackersViewAsIQueryable().Where(x => x.IsDeleted == false && x.GoalTrackerId != goalTrackerId && x.GoalTrackerName.ToLower().Trim() == goalTrackerName.ToLower().Trim() && x.UserId == userId);
            return sql.AnyAsync();
        }

        public Task<bool> IsGoalTrackerNameAlreadyExistsForOtherGoalTrackerAsync(string goalTrackerName, long userId)
        {
            var sql = GetGoalTrackersViewAsIQueryable().Where(x => x.IsDeleted == false && x.GoalTrackerName.ToLower().Trim() == goalTrackerName.ToLower().Trim() && x.UserId == userId);
            return sql.AnyAsync();
        }

        

        public Task<List<GoalTrackerView>> GetGoalTrackerViewByUserIdAsync(long userId)
        {
                return GetGoalTrackersViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerView>> GetGoalTrackerViewByUserIdAsync(GoalTrackerByUserIdSearchCriteria search)
        {
                return GetGoalTrackersViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }

        public Task<List<GoalTracker>> GetBareGoalTrackerByUserIdAsync(long userId)
        {
                return appDBManager.GetTable<GoalTracker>().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTracker>> GetBareGoalTrackerByUserIdAsync(GoalTrackerByUserIdSearchCriteria search)
        {
                return appDBManager.GetTable<GoalTracker>().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }


        public Task<List<UserView>> GetUserViewByGoalTrackerIdAsync(long goalTrackerId)
        {
                var idsSql = appDBManager.GetTable<GoalTracker>().Where(x => x.IsDeleted == false && x.GoalTrackerId == goalTrackerId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<UserView>> GetUserViewByGoalTrackerIdAsync(UserByGoalTrackerIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<GoalTracker>().Where(x => x.IsDeleted == false && x.GoalTrackerId == search.GoalTrackerId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }

        public Task<List<User>> GetBareUserByGoalTrackerIdAsync(long goalTrackerId)
        {
                var idsSql = appDBManager.GetTable<GoalTracker>().Where(x => x.IsDeleted == false && x.GoalTrackerId == goalTrackerId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<User>> GetBareUserByGoalTrackerIdAsync(UserByGoalTrackerIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<GoalTracker>().Where(x => x.IsDeleted == false && x.GoalTrackerId == search.GoalTrackerId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }



        /*

        public async Task<bool> UpdateGoalTrackerPicturePathAsync(long goalTrackerId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTracker>().Where(x => x.GoalTrackerId == goalTrackerId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearGoalTrackerProfilePathAsync(long goalTrackerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTracker>().Where(x => x.GoalTrackerId == goalTrackerId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableGoalTrackerByIdAsync(long goalTrackerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTracker>().Where(x => x.GoalTrackerId == goalTrackerId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableGoalTrackerByIdAsync(long goalTrackerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTracker>().Where(x => x.GoalTrackerId == goalTrackerId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateGoalTrackerWithXYZZYXAsync(long goalTrackerId, long xYZZYXId, long? createdBy)
        {
            var item = new GoalTrackerXYZZYXAssociationModel();
            item.GoalTrackerId = goalTrackerId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<GoalTrackerXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateGoalTrackerWithXYZZYXAsync(long goalTrackerId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<GoalTrackerXYZZYX>(x => x.GoalTrackerId == goalTrackerId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesGoalTrackerWithXYZZYXAssociationExistsAsync(long goalTrackerId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<GoalTrackerXYZZYX>(x => x.GoalTrackerId == goalTrackerId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<GoalTrackerView>> GetGoalTrackersByXYZZYXAsync(long xYZZYXId)
        {
            var goalTrackerIds = GetGoalTrackerXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.GoalTrackerId);
            var goalTrackerSql = GetGoalTrackersViewAsIQueryable().Where(x => goalTrackerIds.Contains(x.GoalTrackerId));
            return goalTrackerSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByGoalTrackersAsync(long goalTrackerId)
        {
            var xYZZYXIds = GetGoalTrackerXYZZYXsViewAsIQueryable().Where(x => x.GoalTrackerId == goalTracker).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<GoalTrackerXYZZYX> GetGoalTrackerXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<GoalTrackerXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetGoalTrackerXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.GoalTrackerXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region GoalTrackerCategory

        public IQueryable<GoalTrackerCategoryView> GetGoalTrackerCategorysViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<GoalTrackerCategory>()
                       join a in appDBManager.GetTable<Category>() on item.CategoryId equals a.CategoryId
                   join b in appDBManager.GetTable<GoalTracker>() on item.GoalTrackerId equals b.GoalTrackerId
                   join c in appDBManager.GetTable<User>() on item.UserId equals c.UserId

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new GoalTrackerCategoryView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       CategoryName = a.CategoryName,
                       GoalTrackerName = b.GoalTrackerName,
                       UserName = c.UserName,

                       GoalTrackerCategoryId = item.GoalTrackerCategoryId,
                       CategoryId = item.CategoryId,
                       GoalTrackerId = item.GoalTrackerId,
                       UserId = item.UserId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<GoalTrackerCategoryView> GetGoalTrackerCategorysInternalWithSearchCriteriaAsync(GoalTrackerCategorySearchCriteria search)
        {
            var sql1 = GetGoalTrackerCategorysViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.GoalTrackerCategoryId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                );
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

    if (search.CategoryId != null && search.CategoryId > 0)
                    sql1 = sql1.Where(x => search.CategoryId == x.CategoryId);
                if (search.GoalTrackerId != null && search.GoalTrackerId > 0)
                    sql1 = sql1.Where(x => search.GoalTrackerId == x.GoalTrackerId);
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

        public async Task<DbReturnListModel<GoalTrackerCategoryView>> GetGoalTrackerCategorysInternalAsync(GoalTrackerCategorySearchCriteria search)
        {
            var sql1 = GetGoalTrackerCategorysInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.GoalTrackerCategoryId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetGoalTrackerCategorysViewAsIQueryable().FirstOrDefaultAsync(x => x.GoalTrackerCategoryId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<List<Category>> GetAllDistinctCategorysForAllGoalTrackerAsync(long userId)
        {
            var sql = appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.UserId == userId).Select(x => x.CategoryId).Distinct();
            return appDBManager.GetTable<Category>().Where(x => x.UserId == userId && sql.Contains(x.CategoryId)).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerCategoryView>> GetGoalTrackerCategorysAsync(GoalTrackerCategorySearchCriteria search)
        {
            return GetGoalTrackerCategorysInternalAsync(search);
        }

        public Task<GoalTrackerCategory> GetGoalTrackerCategoryFromAnyByIdAsync(long goalTrackerCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<GoalTrackerCategory>(x => x.GoalTrackerCategoryId == goalTrackerCategoryId);
        }

        public Task<GoalTrackerCategoryView> GetGoalTrackerCategoryViewFromAnyByIdAsync(long goalTrackerCategoryId)
        {
            return GetGoalTrackerCategorysViewAsIQueryable().FirstOrDefaultAsync(x => x.GoalTrackerCategoryId == goalTrackerCategoryId);
        }

        public Task<GoalTrackerCategory> GetGoalTrackerCategoryByUniqueIdentifierAsync(string goalTrackerCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<GoalTrackerCategory>(x => x.GoalTrackerCategoryId == SafeUtils.Int(goalTrackerCategoryId) && x.IsDeleted == false);
        }

        public Task<GoalTrackerCategory> GetGoalTrackerCategoryByIdAsync(long goalTrackerCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<GoalTrackerCategory>(x => x.GoalTrackerCategoryId == goalTrackerCategoryId && x.IsDeleted == false);
        }

        public Task<List<GoalTrackerCategory>> GetGoalTrackerCategoryByIdsAsync(List<long> goalTrackerCategoryIds)
        {
            return appDBManager.GetTable<GoalTrackerCategory>().Where(x => goalTrackerCategoryIds.Contains(x.GoalTrackerCategoryId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<GoalTrackerCategoryView> GetGoalTrackerCategoryViewByIdAsync(long goalTrackerCategoryId)
        {
            var sql1 = GetGoalTrackerCategorysViewAsIQueryable();

            sql1 = sql1.Where(x => x.GoalTrackerCategoryId == goalTrackerCategoryId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<GoalTrackerCategory>> GetAllGoalTrackerCategorys()
        {
            return appDBManager.SelectAllAsync<GoalTrackerCategory>();
        }

        /*
        public Task<GoalTrackerCategory> GetGoalTrackerCategoryByGoalTrackerCategoryCodeAsync(long userId, string goalTrackerCategoryCode)
        {
            return appDBManager.FirstOrDefaultAsync<GoalTrackerCategory>(x => x.UserId == userId && x.GoalTrackerCategoryCode == goalTrackerCategoryCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateGoalTrackerCategoryCodeAsync(long userId, long goalTrackerCategoryId, string goalTrackerCategoryCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.UserId == userId && x.GoalTrackerCategoryId == goalTrackerCategoryId && x.IsDeleted == false).Set(x => x.GoalTrackerCategoryCode, goalTrackerCategoryCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<GoalTrackerCategory> CreateGoalTrackerCategoryAsync(GoalTrackerCategory goalTrackerCategory, long? createdBy)
        {
            goalTrackerCategory.GoalTrackerCategoryId = await appDBManager.InsertWithAuditAsync<GoalTrackerCategory, long>(goalTrackerCategory, createdBy);
            return goalTrackerCategory;
        }

        public async Task<List<GoalTrackerCategory>> CreateGoalTrackerCategorysAsync(List<GoalTrackerCategory> goalTrackerCategorys, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<GoalTrackerCategory, long>(goalTrackerCategorys, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                goalTrackerCategorys[i].GoalTrackerCategoryId = lst[i];
            return goalTrackerCategorys;
        }

        public Task<bool> UpdateGoalTrackerCategoryAsync(GoalTrackerCategory goalTrackerCategory, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<GoalTrackerCategory>(goalTrackerCategory, modifiedBy);
        }

        public async Task<bool> MarkGoalTrackerCategoryAsNotDeletedByIdAsync(long goalTrackerCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.GoalTrackerCategoryId == goalTrackerCategoryId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkGoalTrackerCategoryAsDeletedByIdAsync(long goalTrackerCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.GoalTrackerCategoryId == goalTrackerCategoryId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteGoalTrackerCategoryByIdAsync(long goalTrackerCategoryId)
        {
            return await appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.GoalTrackerCategoryId == goalTrackerCategoryId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetGoalTrackerCategoryActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.GoalTrackerCategory);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<List<GoalTrackerCategoryView>> GetGoalTrackerCategoryViewByCategoryIdAsync(long categoryId)
        {
                return GetGoalTrackerCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.CategoryId == categoryId).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerCategoryView>> GetGoalTrackerCategoryViewByCategoryIdAsync(GoalTrackerCategoryByCategoryIdSearchCriteria search)
        {
                return GetGoalTrackerCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.CategoryId == search.CategoryId).ReturnListModelResultAsync(search);
        }

        public Task<List<GoalTrackerCategory>> GetBareGoalTrackerCategoryByCategoryIdAsync(long categoryId)
        {
                return appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.CategoryId == categoryId).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerCategory>> GetBareGoalTrackerCategoryByCategoryIdAsync(GoalTrackerCategoryByCategoryIdSearchCriteria search)
        {
                return appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.CategoryId == search.CategoryId).ReturnListModelResultAsync(search);
        }


        public Task<List<CategoryView>> GetCategoryViewByGoalTrackerCategoryIdAsync(long goalTrackerCategoryId)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.GoalTrackerCategoryId == goalTrackerCategoryId).Select(x => x.CategoryId);
                return GetCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.CategoryId)).ToListAsync();
        }

        public Task<DbReturnListModel<CategoryView>> GetCategoryViewByGoalTrackerCategoryIdAsync(CategoryByGoalTrackerCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.GoalTrackerCategoryId == search.GoalTrackerCategoryId).Select(x => x.CategoryId);
                return GetCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.CategoryId)).ReturnListModelResultAsync(search);
        }

        public Task<List<Category>> GetBareCategoryByGoalTrackerIdAsync(long goalTrackerId)
        {
            var idsSql = appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && goalTrackerId == x.GoalTrackerId).Select(x => x.CategoryId);
            return appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && idsSql.Contains(x.CategoryId)).ToListAsync();
        }

        public Task<List<Category>> GetBareCategoryByGoalTrackerCategoryIdAsync(List<long> goalTrackerCategoryIds)
        {
            var idsSql = appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && goalTrackerCategoryIds.Contains(x.GoalTrackerCategoryId)).Select(x => x.CategoryId);
            return appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && idsSql.Contains(x.CategoryId)).ToListAsync();
        }

        public Task<List<Category>> GetBareCategoryByGoalTrackerCategoryIdAsync(long goalTrackerCategoryId)
        {
            return GetBareCategoryByGoalTrackerCategoryIdAsync(new List<long> { goalTrackerCategoryId });
        }

        public Task<DbReturnListModel<Category>> GetBareCategoryByGoalTrackerCategoryIdAsync(CategoryByGoalTrackerCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.GoalTrackerCategoryId == search.GoalTrackerCategoryId).Select(x => x.CategoryId);
                return appDBManager.GetTable<Category>().Where(x => x.IsDeleted == false && idsSql.Contains(x.CategoryId)).ReturnListModelResultAsync(search);
        }


        public Task<List<GoalTrackerCategoryView>> GetGoalTrackerCategoryViewByGoalTrackerIdAsync(long goalTrackerId)
        {
                return GetGoalTrackerCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.GoalTrackerId == goalTrackerId).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerCategoryView>> GetGoalTrackerCategoryViewByGoalTrackerIdAsync(GoalTrackerCategoryByGoalTrackerIdSearchCriteria search)
        {
                return GetGoalTrackerCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.GoalTrackerId == search.GoalTrackerId).ReturnListModelResultAsync(search);
        }

        public Task<List<GoalTrackerCategory>> GetBareGoalTrackerCategoryByGoalTrackerIdAsync(long goalTrackerId)
        {
                return appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.GoalTrackerId == goalTrackerId).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerCategory>> GetBareGoalTrackerCategoryByGoalTrackerIdAsync(GoalTrackerCategoryByGoalTrackerIdSearchCriteria search)
        {
                return appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.GoalTrackerId == search.GoalTrackerId).ReturnListModelResultAsync(search);
        }


        public Task<List<GoalTrackerView>> GetGoalTrackerViewByGoalTrackerCategoryIdAsync(long goalTrackerCategoryId)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.GoalTrackerCategoryId == goalTrackerCategoryId).Select(x => x.GoalTrackerId);
                return GetGoalTrackersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.GoalTrackerId)).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerView>> GetGoalTrackerViewByGoalTrackerCategoryIdAsync(GoalTrackerByGoalTrackerCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.GoalTrackerCategoryId == search.GoalTrackerCategoryId).Select(x => x.GoalTrackerId);
                return GetGoalTrackersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.GoalTrackerId)).ReturnListModelResultAsync(search);
        }

        public Task<List<GoalTracker>> GetBareGoalTrackerByGoalTrackerCategoryIdAsync(long goalTrackerCategoryId)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.GoalTrackerCategoryId == goalTrackerCategoryId).Select(x => x.GoalTrackerId);
                return appDBManager.GetTable<GoalTracker>().Where(x => x.IsDeleted == false && idsSql.Contains(x.GoalTrackerId)).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTracker>> GetBareGoalTrackerByGoalTrackerCategoryIdAsync(GoalTrackerByGoalTrackerCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.GoalTrackerCategoryId == search.GoalTrackerCategoryId).Select(x => x.GoalTrackerId);
                return appDBManager.GetTable<GoalTracker>().Where(x => x.IsDeleted == false && idsSql.Contains(x.GoalTrackerId)).ReturnListModelResultAsync(search);
        }


        public Task<List<GoalTrackerCategoryView>> GetGoalTrackerCategoryViewByUserIdAsync(long userId)
        {
                return GetGoalTrackerCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerCategoryView>> GetGoalTrackerCategoryViewByUserIdAsync(GoalTrackerCategoryByUserIdSearchCriteria search)
        {
                return GetGoalTrackerCategorysViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }

        public Task<List<GoalTrackerCategory>> GetBareGoalTrackerCategoryByUserIdAsync(long userId)
        {
                return appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerCategory>> GetBareGoalTrackerCategoryByUserIdAsync(GoalTrackerCategoryByUserIdSearchCriteria search)
        {
                return appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }


        public Task<List<UserView>> GetUserViewByGoalTrackerCategoryIdAsync(long goalTrackerCategoryId)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.GoalTrackerCategoryId == goalTrackerCategoryId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<UserView>> GetUserViewByGoalTrackerCategoryIdAsync(UserByGoalTrackerCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.GoalTrackerCategoryId == search.GoalTrackerCategoryId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }

        public Task<List<User>> GetBareUserByGoalTrackerCategoryIdAsync(long goalTrackerCategoryId)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.GoalTrackerCategoryId == goalTrackerCategoryId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<User>> GetBareUserByGoalTrackerCategoryIdAsync(UserByGoalTrackerCategoryIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.IsDeleted == false && x.GoalTrackerCategoryId == search.GoalTrackerCategoryId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }



        /*

        public async Task<bool> UpdateGoalTrackerCategoryPicturePathAsync(long goalTrackerCategoryId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.GoalTrackerCategoryId == goalTrackerCategoryId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearGoalTrackerCategoryProfilePathAsync(long goalTrackerCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.GoalTrackerCategoryId == goalTrackerCategoryId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableGoalTrackerCategoryByIdAsync(long goalTrackerCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.GoalTrackerCategoryId == goalTrackerCategoryId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableGoalTrackerCategoryByIdAsync(long goalTrackerCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTrackerCategory>().Where(x => x.GoalTrackerCategoryId == goalTrackerCategoryId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateGoalTrackerCategoryWithXYZZYXAsync(long goalTrackerCategoryId, long xYZZYXId, long? createdBy)
        {
            var item = new GoalTrackerCategoryXYZZYXAssociationModel();
            item.GoalTrackerCategoryId = goalTrackerCategoryId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<GoalTrackerCategoryXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateGoalTrackerCategoryWithXYZZYXAsync(long goalTrackerCategoryId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<GoalTrackerCategoryXYZZYX>(x => x.GoalTrackerCategoryId == goalTrackerCategoryId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesGoalTrackerCategoryWithXYZZYXAssociationExistsAsync(long goalTrackerCategoryId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<GoalTrackerCategoryXYZZYX>(x => x.GoalTrackerCategoryId == goalTrackerCategoryId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<GoalTrackerCategoryView>> GetGoalTrackerCategorysByXYZZYXAsync(long xYZZYXId)
        {
            var goalTrackerCategoryIds = GetGoalTrackerCategoryXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.GoalTrackerCategoryId);
            var goalTrackerCategorySql = GetGoalTrackerCategorysViewAsIQueryable().Where(x => goalTrackerCategoryIds.Contains(x.GoalTrackerCategoryId));
            return goalTrackerCategorySql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByGoalTrackerCategorysAsync(long goalTrackerCategoryId)
        {
            var xYZZYXIds = GetGoalTrackerCategoryXYZZYXsViewAsIQueryable().Where(x => x.GoalTrackerCategoryId == goalTrackerCategory).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<GoalTrackerCategoryXYZZYX> GetGoalTrackerCategoryXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<GoalTrackerCategoryXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetGoalTrackerCategoryXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.GoalTrackerCategoryXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region GoalTrackerMileStone

        public IQueryable<GoalTrackerMileStoneView> GetGoalTrackerMileStonesViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<GoalTrackerMileStone>()
                       join a in appDBManager.GetTable<GoalTracker>() on item.GoalTrackerId equals a.GoalTrackerId
                   join b in appDBManager.GetTable<EnumType>() on item.PredefinedGoalStatusTypeId equals b.EnumTypeId
                   join c in appDBManager.GetTable<User>() on item.UserId equals c.UserId

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new GoalTrackerMileStoneView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       GoalTrackerName = a.GoalTrackerName,
                       PredefinedGoalStatusTypeName = b.EnumTypeName,
                       UserName = c.UserName,

                       GoalTrackerMileStoneId = item.GoalTrackerMileStoneId,
                       GoalTrackerId = item.GoalTrackerId,
                       GoalTrackerMileStoneName = item.GoalTrackerMileStoneName,
                       GoalTrackerMileStoneStartDate = item.GoalTrackerMileStoneStartDate,
                       GoalTrackerMileStoneEndDate = item.GoalTrackerMileStoneEndDate,
                       PredefinedGoalStatusTypeId = item.PredefinedGoalStatusTypeId,
                       GoalTrackerMileStoneCompletedDate = item.GoalTrackerMileStoneCompletedDate,
                       UserId = item.UserId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<GoalTrackerMileStoneView> GetGoalTrackerMileStonesInternalWithSearchCriteriaAsync(GoalTrackerMileStoneSearchCriteria search)
        {
            var sql1 = GetGoalTrackerMileStonesViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.GoalTrackerMileStoneId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.GoalTrackerMileStoneName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
);
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

    if (search.GoalTrackerId != null && search.GoalTrackerId > 0)
                    sql1 = sql1.Where(x => search.GoalTrackerId == x.GoalTrackerId);
                if (search.PredefinedGoalStatusTypeIds != null && search.PredefinedGoalStatusTypeIds.Count > 0)
                    sql1 = sql1.Where(x => search.PredefinedGoalStatusTypeIds.Contains(x.PredefinedGoalStatusTypeId));
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

        public async Task<DbReturnListModel<GoalTrackerMileStoneView>> GetGoalTrackerMileStonesInternalAsync(GoalTrackerMileStoneSearchCriteria search)
        {
            var sql1 = GetGoalTrackerMileStonesInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.GoalTrackerMileStoneId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetGoalTrackerMileStonesViewAsIQueryable().FirstOrDefaultAsync(x => x.GoalTrackerMileStoneId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<GoalTrackerMileStoneView>> GetGoalTrackerMileStonesAsync(GoalTrackerMileStoneSearchCriteria search)
        {
            return GetGoalTrackerMileStonesInternalAsync(search);
        }

        public Task<GoalTrackerMileStone> GetGoalTrackerMileStoneFromAnyByIdAsync(long goalTrackerMileStoneId)
        {
            return appDBManager.FirstOrDefaultAsync<GoalTrackerMileStone>(x => x.GoalTrackerMileStoneId == goalTrackerMileStoneId);
        }

        public Task<GoalTrackerMileStoneView> GetGoalTrackerMileStoneViewFromAnyByIdAsync(long goalTrackerMileStoneId)
        {
            return GetGoalTrackerMileStonesViewAsIQueryable().FirstOrDefaultAsync(x => x.GoalTrackerMileStoneId == goalTrackerMileStoneId);
        }

        public Task<GoalTrackerMileStone> GetGoalTrackerMileStoneByUniqueIdentifierAsync(string goalTrackerMileStoneId)
        {
            return appDBManager.FirstOrDefaultAsync<GoalTrackerMileStone>(x => x.GoalTrackerMileStoneId == SafeUtils.Int(goalTrackerMileStoneId) && x.IsDeleted == false);
        }

        public Task<GoalTrackerMileStone> GetGoalTrackerMileStoneByIdAsync(long goalTrackerMileStoneId)
        {
            return appDBManager.FirstOrDefaultAsync<GoalTrackerMileStone>(x => x.GoalTrackerMileStoneId == goalTrackerMileStoneId && x.IsDeleted == false);
        }

        public Task<List<GoalTrackerMileStone>> GetGoalTrackerMileStoneByIdsAsync(List<long> goalTrackerMileStoneIds)
        {
            return appDBManager.GetTable<GoalTrackerMileStone>().Where(x => goalTrackerMileStoneIds.Contains(x.GoalTrackerMileStoneId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<GoalTrackerMileStoneView> GetGoalTrackerMileStoneViewByIdAsync(long goalTrackerMileStoneId)
        {
            var sql1 = GetGoalTrackerMileStonesViewAsIQueryable();

            sql1 = sql1.Where(x => x.GoalTrackerMileStoneId == goalTrackerMileStoneId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<GoalTrackerMileStone>> GetAllGoalTrackerMileStones()
        {
            return appDBManager.SelectAllAsync<GoalTrackerMileStone>();
        }

        /*
        public Task<GoalTrackerMileStone> GetGoalTrackerMileStoneByGoalTrackerMileStoneCodeAsync(long userId, string goalTrackerMileStoneCode)
        {
            return appDBManager.FirstOrDefaultAsync<GoalTrackerMileStone>(x => x.UserId == userId && x.GoalTrackerMileStoneCode == goalTrackerMileStoneCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateGoalTrackerMileStoneCodeAsync(long userId, long goalTrackerMileStoneId, string goalTrackerMileStoneCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.UserId == userId && x.GoalTrackerMileStoneId == goalTrackerMileStoneId && x.IsDeleted == false).Set(x => x.GoalTrackerMileStoneCode, goalTrackerMileStoneCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<GoalTrackerMileStone> CreateGoalTrackerMileStoneAsync(GoalTrackerMileStone goalTrackerMileStone, long? createdBy)
        {
            goalTrackerMileStone.GoalTrackerMileStoneId = await appDBManager.InsertWithAuditAsync<GoalTrackerMileStone, long>(goalTrackerMileStone, createdBy);
            return goalTrackerMileStone;
        }

        public async Task<List<GoalTrackerMileStone>> CreateGoalTrackerMileStonesAsync(List<GoalTrackerMileStone> goalTrackerMileStones, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<GoalTrackerMileStone, long>(goalTrackerMileStones, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                goalTrackerMileStones[i].GoalTrackerMileStoneId = lst[i];
            return goalTrackerMileStones;
        }

        public Task<bool> UpdateGoalTrackerMileStoneAsync(GoalTrackerMileStone goalTrackerMileStone, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<GoalTrackerMileStone>(goalTrackerMileStone, modifiedBy);
        }

        public async Task<bool> MarkGoalTrackerMileStoneAsNotDeletedByIdAsync(long goalTrackerMileStoneId, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.GoalTrackerMileStoneId == goalTrackerMileStoneId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkGoalTrackerMileStoneAsDeletedByIdAsync(long goalTrackerMileStoneId, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.GoalTrackerMileStoneId == goalTrackerMileStoneId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteGoalTrackerMileStoneByIdAsync(long goalTrackerMileStoneId)
        {
            return await appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.GoalTrackerMileStoneId == goalTrackerMileStoneId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetGoalTrackerMileStoneActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.GoalTrackerMileStone);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsGoalTrackerMileStoneNameAlreadyExistsForOtherGoalTrackerMileStoneAsync(long goalTrackerMileStoneId, string goalTrackerMileStoneName, long userId)
        {
            var sql = GetGoalTrackerMileStonesViewAsIQueryable().Where(x => x.IsDeleted == false && x.GoalTrackerMileStoneId != goalTrackerMileStoneId && x.GoalTrackerMileStoneName.ToLower().Trim() == goalTrackerMileStoneName.ToLower().Trim() && x.UserId == userId);
            return sql.AnyAsync();
        }

        public Task<bool> IsGoalTrackerMileStoneNameAlreadyExistsForOtherGoalTrackerMileStoneAsync(string goalTrackerMileStoneName, long userId)
        {
            var sql = GetGoalTrackerMileStonesViewAsIQueryable().Where(x => x.IsDeleted == false && x.GoalTrackerMileStoneName.ToLower().Trim() == goalTrackerMileStoneName.ToLower().Trim() && x.UserId == userId);
            return sql.AnyAsync();
        }

        

        public Task<List<GoalTrackerMileStoneView>> GetGoalTrackerMileStoneViewByGoalTrackerIdAsync(long goalTrackerId)
        {
                return GetGoalTrackerMileStonesViewAsIQueryable().Where(x => x.IsDeleted == false && x.GoalTrackerId == goalTrackerId).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerMileStoneView>> GetGoalTrackerMileStoneViewByGoalTrackerIdAsync(GoalTrackerMileStoneByGoalTrackerIdSearchCriteria search)
        {
                return GetGoalTrackerMileStonesViewAsIQueryable().Where(x => x.IsDeleted == false && x.GoalTrackerId == search.GoalTrackerId).ReturnListModelResultAsync(search);
        }

        public Task<List<GoalTrackerMileStone>> GetBareGoalTrackerMileStoneByGoalTrackerIdAsync(long goalTrackerId)
        {
                return appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.IsDeleted == false && x.GoalTrackerId == goalTrackerId).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerMileStone>> GetBareGoalTrackerMileStoneByGoalTrackerIdAsync(GoalTrackerMileStoneByGoalTrackerIdSearchCriteria search)
        {
                return appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.IsDeleted == false && x.GoalTrackerId == search.GoalTrackerId).ReturnListModelResultAsync(search);
        }


        public Task<List<GoalTrackerView>> GetGoalTrackerViewByGoalTrackerMileStoneIdAsync(long goalTrackerMileStoneId)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.IsDeleted == false && x.GoalTrackerMileStoneId == goalTrackerMileStoneId).Select(x => x.GoalTrackerId);
                return GetGoalTrackersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.GoalTrackerId)).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerView>> GetGoalTrackerViewByGoalTrackerMileStoneIdAsync(GoalTrackerByGoalTrackerMileStoneIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.IsDeleted == false && x.GoalTrackerMileStoneId == search.GoalTrackerMileStoneId).Select(x => x.GoalTrackerId);
                return GetGoalTrackersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.GoalTrackerId)).ReturnListModelResultAsync(search);
        }

        public Task<List<GoalTracker>> GetBareGoalTrackerByGoalTrackerMileStoneIdAsync(long goalTrackerMileStoneId)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.IsDeleted == false && x.GoalTrackerMileStoneId == goalTrackerMileStoneId).Select(x => x.GoalTrackerId);
                return appDBManager.GetTable<GoalTracker>().Where(x => x.IsDeleted == false && idsSql.Contains(x.GoalTrackerId)).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTracker>> GetBareGoalTrackerByGoalTrackerMileStoneIdAsync(GoalTrackerByGoalTrackerMileStoneIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.IsDeleted == false && x.GoalTrackerMileStoneId == search.GoalTrackerMileStoneId).Select(x => x.GoalTrackerId);
                return appDBManager.GetTable<GoalTracker>().Where(x => x.IsDeleted == false && idsSql.Contains(x.GoalTrackerId)).ReturnListModelResultAsync(search);
        }


        public Task<List<GoalTrackerMileStoneView>> GetGoalTrackerMileStoneViewByUserIdAsync(long userId)
        {
                return GetGoalTrackerMileStonesViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerMileStoneView>> GetGoalTrackerMileStoneViewByUserIdAsync(GoalTrackerMileStoneByUserIdSearchCriteria search)
        {
                return GetGoalTrackerMileStonesViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }

        public Task<List<GoalTrackerMileStone>> GetBareGoalTrackerMileStoneByUserIdAsync(long userId)
        {
                return appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<GoalTrackerMileStone>> GetBareGoalTrackerMileStoneByUserIdAsync(GoalTrackerMileStoneByUserIdSearchCriteria search)
        {
                return appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }


        public Task<List<UserView>> GetUserViewByGoalTrackerMileStoneIdAsync(long goalTrackerMileStoneId)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.IsDeleted == false && x.GoalTrackerMileStoneId == goalTrackerMileStoneId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<UserView>> GetUserViewByGoalTrackerMileStoneIdAsync(UserByGoalTrackerMileStoneIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.IsDeleted == false && x.GoalTrackerMileStoneId == search.GoalTrackerMileStoneId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }

        public Task<List<User>> GetBareUserByGoalTrackerMileStoneIdAsync(long goalTrackerMileStoneId)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.IsDeleted == false && x.GoalTrackerMileStoneId == goalTrackerMileStoneId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<User>> GetBareUserByGoalTrackerMileStoneIdAsync(UserByGoalTrackerMileStoneIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.IsDeleted == false && x.GoalTrackerMileStoneId == search.GoalTrackerMileStoneId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }



        /*

        public async Task<bool> UpdateGoalTrackerMileStonePicturePathAsync(long goalTrackerMileStoneId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.GoalTrackerMileStoneId == goalTrackerMileStoneId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearGoalTrackerMileStoneProfilePathAsync(long goalTrackerMileStoneId, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.GoalTrackerMileStoneId == goalTrackerMileStoneId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableGoalTrackerMileStoneByIdAsync(long goalTrackerMileStoneId, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.GoalTrackerMileStoneId == goalTrackerMileStoneId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableGoalTrackerMileStoneByIdAsync(long goalTrackerMileStoneId, long? modifiedBy)
        {
            return await appDBManager.GetTable<GoalTrackerMileStone>().Where(x => x.GoalTrackerMileStoneId == goalTrackerMileStoneId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateGoalTrackerMileStoneWithXYZZYXAsync(long goalTrackerMileStoneId, long xYZZYXId, long? createdBy)
        {
            var item = new GoalTrackerMileStoneXYZZYXAssociationModel();
            item.GoalTrackerMileStoneId = goalTrackerMileStoneId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<GoalTrackerMileStoneXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateGoalTrackerMileStoneWithXYZZYXAsync(long goalTrackerMileStoneId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<GoalTrackerMileStoneXYZZYX>(x => x.GoalTrackerMileStoneId == goalTrackerMileStoneId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesGoalTrackerMileStoneWithXYZZYXAssociationExistsAsync(long goalTrackerMileStoneId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<GoalTrackerMileStoneXYZZYX>(x => x.GoalTrackerMileStoneId == goalTrackerMileStoneId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<GoalTrackerMileStoneView>> GetGoalTrackerMileStonesByXYZZYXAsync(long xYZZYXId)
        {
            var goalTrackerMileStoneIds = GetGoalTrackerMileStoneXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.GoalTrackerMileStoneId);
            var goalTrackerMileStoneSql = GetGoalTrackerMileStonesViewAsIQueryable().Where(x => goalTrackerMileStoneIds.Contains(x.GoalTrackerMileStoneId));
            return goalTrackerMileStoneSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByGoalTrackerMileStonesAsync(long goalTrackerMileStoneId)
        {
            var xYZZYXIds = GetGoalTrackerMileStoneXYZZYXsViewAsIQueryable().Where(x => x.GoalTrackerMileStoneId == goalTrackerMileStone).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<GoalTrackerMileStoneXYZZYX> GetGoalTrackerMileStoneXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<GoalTrackerMileStoneXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetGoalTrackerMileStoneXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.GoalTrackerMileStoneXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region MeditationFile

        public IQueryable<MeditationFileView> GetMeditationFilesViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<MeditationFile>()
                       join a in appDBManager.GetTable<EnumType>() on item.PredefinedMeditationTypeId equals a.EnumTypeId
                   join b in appDBManager.GetTable<User>() on item.UserId equals b.UserId

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new MeditationFileView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       PredefinedMeditationTypeName = a.EnumTypeName,
                       UserName = b.UserName,

                       MeditationFileId = item.MeditationFileId,
                       PredefinedMeditationTypeId = item.PredefinedMeditationTypeId,
                       MeditationFileName = item.MeditationFileName,
                       Location = item.Location,
                       FileName = item.FileName,
                       FileSize = item.FileSize,
                       UserId = item.UserId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<MeditationFileView> GetMeditationFilesInternalWithSearchCriteriaAsync(MeditationFileSearchCriteria search)
        {
            var sql1 = GetMeditationFilesViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.MeditationFileId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.MeditationFileName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Location.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.FileName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
);
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

    if (search.PredefinedMeditationTypeIds != null && search.PredefinedMeditationTypeIds.Count > 0)
                    sql1 = sql1.Where(x => search.PredefinedMeditationTypeIds.Contains(x.PredefinedMeditationTypeId));
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

        public async Task<DbReturnListModel<MeditationFileView>> GetMeditationFilesInternalAsync(MeditationFileSearchCriteria search)
        {
            var sql1 = GetMeditationFilesInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.MeditationFileId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetMeditationFilesViewAsIQueryable().FirstOrDefaultAsync(x => x.MeditationFileId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<MeditationFileView>> GetMeditationFilesAsync(MeditationFileSearchCriteria search)
        {
            return GetMeditationFilesInternalAsync(search);
        }

        public Task<MeditationFile> GetMeditationFileFromAnyByIdAsync(long meditationFileId)
        {
            return appDBManager.FirstOrDefaultAsync<MeditationFile>(x => x.MeditationFileId == meditationFileId);
        }

        public Task<MeditationFileView> GetMeditationFileViewFromAnyByIdAsync(long meditationFileId)
        {
            return GetMeditationFilesViewAsIQueryable().FirstOrDefaultAsync(x => x.MeditationFileId == meditationFileId);
        }

        public Task<MeditationFile> GetMeditationFileByUniqueIdentifierAsync(string meditationFileId)
        {
            return appDBManager.FirstOrDefaultAsync<MeditationFile>(x => x.MeditationFileId == SafeUtils.Int(meditationFileId) && x.IsDeleted == false);
        }

        public Task<MeditationFile> GetMeditationFileByIdAsync(long meditationFileId)
        {
            return appDBManager.FirstOrDefaultAsync<MeditationFile>(x => x.MeditationFileId == meditationFileId && x.IsDeleted == false);
        }

        public Task<List<MeditationFile>> GetMeditationFileByIdsAsync(List<long> meditationFileIds)
        {
            return appDBManager.GetTable<MeditationFile>().Where(x => meditationFileIds.Contains(x.MeditationFileId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<MeditationFileView> GetMeditationFileViewByIdAsync(long meditationFileId)
        {
            var sql1 = GetMeditationFilesViewAsIQueryable();

            sql1 = sql1.Where(x => x.MeditationFileId == meditationFileId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<MeditationFile>> GetAllMeditationFiles()
        {
            return appDBManager.SelectAllAsync<MeditationFile>();
        }

        /*
        public Task<MeditationFile> GetMeditationFileByMeditationFileCodeAsync(long userId, string meditationFileCode)
        {
            return appDBManager.FirstOrDefaultAsync<MeditationFile>(x => x.UserId == userId && x.MeditationFileCode == meditationFileCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateMeditationFileCodeAsync(long userId, long meditationFileId, string meditationFileCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<MeditationFile>().Where(x => x.UserId == userId && x.MeditationFileId == meditationFileId && x.IsDeleted == false).Set(x => x.MeditationFileCode, meditationFileCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<MeditationFile> CreateMeditationFileAsync(MeditationFile meditationFile, long? createdBy)
        {
            meditationFile.MeditationFileId = await appDBManager.InsertWithAuditAsync<MeditationFile, long>(meditationFile, createdBy);
            return meditationFile;
        }

        public async Task<List<MeditationFile>> CreateMeditationFilesAsync(List<MeditationFile> meditationFiles, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<MeditationFile, long>(meditationFiles, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                meditationFiles[i].MeditationFileId = lst[i];
            return meditationFiles;
        }

        public Task<bool> UpdateMeditationFileAsync(MeditationFile meditationFile, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<MeditationFile>(meditationFile, modifiedBy);
        }

        public async Task<bool> MarkMeditationFileAsNotDeletedByIdAsync(long meditationFileId, long? modifiedBy)
        {
            return await appDBManager.GetTable<MeditationFile>().Where(x => x.MeditationFileId == meditationFileId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkMeditationFileAsDeletedByIdAsync(long meditationFileId, long? modifiedBy)
        {
            return await appDBManager.GetTable<MeditationFile>().Where(x => x.MeditationFileId == meditationFileId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteMeditationFileByIdAsync(long meditationFileId)
        {
            return await appDBManager.GetTable<MeditationFile>().Where(x => x.MeditationFileId == meditationFileId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetMeditationFileActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.MeditationFile);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsMeditationFileNameAlreadyExistsForOtherMeditationFileAsync(long meditationFileId, string meditationFileName, long userId)
        {
            var sql = GetMeditationFilesViewAsIQueryable().Where(x => x.IsDeleted == false && x.MeditationFileId != meditationFileId && x.MeditationFileName.ToLower().Trim() == meditationFileName.ToLower().Trim() && x.UserId == userId);
            return sql.AnyAsync();
        }

        public Task<bool> IsMeditationFileNameAlreadyExistsForOtherMeditationFileAsync(string meditationFileName, long userId)
        {
            var sql = GetMeditationFilesViewAsIQueryable().Where(x => x.IsDeleted == false && x.MeditationFileName.ToLower().Trim() == meditationFileName.ToLower().Trim() && x.UserId == userId);
            return sql.AnyAsync();
        }

        

        public Task<List<MeditationFileView>> GetMeditationFileViewByUserIdAsync(long userId)
        {
                return GetMeditationFilesViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<MeditationFileView>> GetMeditationFileViewByUserIdAsync(MeditationFileByUserIdSearchCriteria search)
        {
                return GetMeditationFilesViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }

        public Task<List<MeditationFile>> GetBareMeditationFileByUserIdAsync(long userId)
        {
                return appDBManager.GetTable<MeditationFile>().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<MeditationFile>> GetBareMeditationFileByUserIdAsync(MeditationFileByUserIdSearchCriteria search)
        {
                return appDBManager.GetTable<MeditationFile>().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }


        public Task<List<UserView>> GetUserViewByMeditationFileIdAsync(long meditationFileId)
        {
                var idsSql = appDBManager.GetTable<MeditationFile>().Where(x => x.IsDeleted == false && x.MeditationFileId == meditationFileId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<UserView>> GetUserViewByMeditationFileIdAsync(UserByMeditationFileIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<MeditationFile>().Where(x => x.IsDeleted == false && x.MeditationFileId == search.MeditationFileId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }

        public Task<List<User>> GetBareUserByMeditationFileIdAsync(long meditationFileId)
        {
                var idsSql = appDBManager.GetTable<MeditationFile>().Where(x => x.IsDeleted == false && x.MeditationFileId == meditationFileId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<User>> GetBareUserByMeditationFileIdAsync(UserByMeditationFileIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<MeditationFile>().Where(x => x.IsDeleted == false && x.MeditationFileId == search.MeditationFileId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }



        /*

        public async Task<bool> UpdateMeditationFilePicturePathAsync(long meditationFileId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<MeditationFile>().Where(x => x.MeditationFileId == meditationFileId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearMeditationFileProfilePathAsync(long meditationFileId, long? modifiedBy)
        {
            return await appDBManager.GetTable<MeditationFile>().Where(x => x.MeditationFileId == meditationFileId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableMeditationFileByIdAsync(long meditationFileId, long? modifiedBy)
        {
            return await appDBManager.GetTable<MeditationFile>().Where(x => x.MeditationFileId == meditationFileId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableMeditationFileByIdAsync(long meditationFileId, long? modifiedBy)
        {
            return await appDBManager.GetTable<MeditationFile>().Where(x => x.MeditationFileId == meditationFileId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateMeditationFileWithXYZZYXAsync(long meditationFileId, long xYZZYXId, long? createdBy)
        {
            var item = new MeditationFileXYZZYXAssociationModel();
            item.MeditationFileId = meditationFileId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<MeditationFileXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateMeditationFileWithXYZZYXAsync(long meditationFileId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<MeditationFileXYZZYX>(x => x.MeditationFileId == meditationFileId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesMeditationFileWithXYZZYXAssociationExistsAsync(long meditationFileId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<MeditationFileXYZZYX>(x => x.MeditationFileId == meditationFileId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<MeditationFileView>> GetMeditationFilesByXYZZYXAsync(long xYZZYXId)
        {
            var meditationFileIds = GetMeditationFileXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.MeditationFileId);
            var meditationFileSql = GetMeditationFilesViewAsIQueryable().Where(x => meditationFileIds.Contains(x.MeditationFileId));
            return meditationFileSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByMeditationFilesAsync(long meditationFileId)
        {
            var xYZZYXIds = GetMeditationFileXYZZYXsViewAsIQueryable().Where(x => x.MeditationFileId == meditationFile).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<MeditationFileXYZZYX> GetMeditationFileXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<MeditationFileXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetMeditationFileXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.MeditationFileXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region MeditationSession

        public IQueryable<MeditationSessionView> GetMeditationSessionsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<MeditationSession>()
                       join a in appDBManager.GetTable<MeditationFile>() on item.MeditationFileId equals a.MeditationFileId
                   join b in appDBManager.GetTable<User>() on item.UserId equals b.UserId

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new MeditationSessionView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       MeditationFileName = a.MeditationFileName,
                       UserName = b.UserName,

                       MeditationSessionId = item.MeditationSessionId,
                       MeditationSessionDate = item.MeditationSessionDate,
                       MeditationSessionTimeSpentInSeconds = item.MeditationSessionTimeSpentInSeconds,
                       MeditationFileId = item.MeditationFileId,
                       UserId = item.UserId,
                       AddedToFeed = item.AddedToFeed,
                       AddedToFeedDate = item.AddedToFeedDate,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<MeditationSessionView> GetMeditationSessionsInternalWithSearchCriteriaAsync(MeditationSessionSearchCriteria search)
        {
            var sql1 = GetMeditationSessionsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.MeditationSessionId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                );
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

    if (search.MeditationFileId != null && search.MeditationFileId > 0)
                    sql1 = sql1.Where(x => search.MeditationFileId == x.MeditationFileId);
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

        public async Task<DbReturnListModel<MeditationSessionView>> GetMeditationSessionsInternalAsync(MeditationSessionSearchCriteria search)
        {
            var sql1 = GetMeditationSessionsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.MeditationSessionId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetMeditationSessionsViewAsIQueryable().FirstOrDefaultAsync(x => x.MeditationSessionId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<MeditationSessionView>> GetMeditationSessionsAsync(MeditationSessionSearchCriteria search)
        {
            return GetMeditationSessionsInternalAsync(search);
        }

        public Task<MeditationSession> GetMeditationSessionFromAnyByIdAsync(long meditationSessionId)
        {
            return appDBManager.FirstOrDefaultAsync<MeditationSession>(x => x.MeditationSessionId == meditationSessionId);
        }

        public Task<MeditationSessionView> GetMeditationSessionViewFromAnyByIdAsync(long meditationSessionId)
        {
            return GetMeditationSessionsViewAsIQueryable().FirstOrDefaultAsync(x => x.MeditationSessionId == meditationSessionId);
        }

        public Task<MeditationSession> GetMeditationSessionByUniqueIdentifierAsync(string meditationSessionId)
        {
            return appDBManager.FirstOrDefaultAsync<MeditationSession>(x => x.MeditationSessionId == SafeUtils.Int(meditationSessionId) && x.IsDeleted == false);
        }

        public Task<MeditationSession> GetMeditationSessionByIdAsync(long meditationSessionId)
        {
            return appDBManager.FirstOrDefaultAsync<MeditationSession>(x => x.MeditationSessionId == meditationSessionId && x.IsDeleted == false);
        }

        public Task<List<MeditationSession>> GetMeditationSessionByIdsAsync(List<long> meditationSessionIds)
        {
            return appDBManager.GetTable<MeditationSession>().Where(x => meditationSessionIds.Contains(x.MeditationSessionId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<MeditationSessionView> GetMeditationSessionViewByIdAsync(long meditationSessionId)
        {
            var sql1 = GetMeditationSessionsViewAsIQueryable();

            sql1 = sql1.Where(x => x.MeditationSessionId == meditationSessionId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<MeditationSession>> GetAllMeditationSessions()
        {
            return appDBManager.SelectAllAsync<MeditationSession>();
        }

        /*
        public Task<MeditationSession> GetMeditationSessionByMeditationSessionCodeAsync(long userId, string meditationSessionCode)
        {
            return appDBManager.FirstOrDefaultAsync<MeditationSession>(x => x.UserId == userId && x.MeditationSessionCode == meditationSessionCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateMeditationSessionCodeAsync(long userId, long meditationSessionId, string meditationSessionCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<MeditationSession>().Where(x => x.UserId == userId && x.MeditationSessionId == meditationSessionId && x.IsDeleted == false).Set(x => x.MeditationSessionCode, meditationSessionCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<MeditationSession> CreateMeditationSessionAsync(MeditationSession meditationSession, long? createdBy)
        {
            meditationSession.MeditationSessionId = await appDBManager.InsertWithAuditAsync<MeditationSession, long>(meditationSession, createdBy);
            return meditationSession;
        }

        public async Task<List<MeditationSession>> CreateMeditationSessionsAsync(List<MeditationSession> meditationSessions, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<MeditationSession, long>(meditationSessions, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                meditationSessions[i].MeditationSessionId = lst[i];
            return meditationSessions;
        }

        public Task<bool> UpdateMeditationSessionAsync(MeditationSession meditationSession, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<MeditationSession>(meditationSession, modifiedBy);
        }

        public async Task<bool> MarkMeditationSessionAsNotDeletedByIdAsync(long meditationSessionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<MeditationSession>().Where(x => x.MeditationSessionId == meditationSessionId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkMeditationSessionAsDeletedByIdAsync(long meditationSessionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<MeditationSession>().Where(x => x.MeditationSessionId == meditationSessionId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteMeditationSessionByIdAsync(long meditationSessionId)
        {
            return await appDBManager.GetTable<MeditationSession>().Where(x => x.MeditationSessionId == meditationSessionId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetMeditationSessionActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.MeditationSession);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<List<MeditationSessionView>> GetMeditationSessionViewByMeditationFileIdAsync(long meditationFileId)
        {
                return GetMeditationSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && x.MeditationFileId == meditationFileId).ToListAsync();
        }

        public Task<DbReturnListModel<MeditationSessionView>> GetMeditationSessionViewByMeditationFileIdAsync(MeditationSessionByMeditationFileIdSearchCriteria search)
        {
                return GetMeditationSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && x.MeditationFileId == search.MeditationFileId).ReturnListModelResultAsync(search);
        }

        public Task<List<MeditationSession>> GetBareMeditationSessionByMeditationFileIdAsync(long meditationFileId)
        {
                return appDBManager.GetTable<MeditationSession>().Where(x => x.IsDeleted == false && x.MeditationFileId == meditationFileId).ToListAsync();
        }

        public Task<DbReturnListModel<MeditationSession>> GetBareMeditationSessionByMeditationFileIdAsync(MeditationSessionByMeditationFileIdSearchCriteria search)
        {
                return appDBManager.GetTable<MeditationSession>().Where(x => x.IsDeleted == false && x.MeditationFileId == search.MeditationFileId).ReturnListModelResultAsync(search);
        }


        public Task<List<MeditationFileView>> GetMeditationFileViewByMeditationSessionIdAsync(long meditationSessionId)
        {
                var idsSql = appDBManager.GetTable<MeditationSession>().Where(x => x.IsDeleted == false && x.MeditationSessionId == meditationSessionId).Select(x => x.MeditationFileId);
                return GetMeditationFilesViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.MeditationFileId)).ToListAsync();
        }

        public Task<DbReturnListModel<MeditationFileView>> GetMeditationFileViewByMeditationSessionIdAsync(MeditationFileByMeditationSessionIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<MeditationSession>().Where(x => x.IsDeleted == false && x.MeditationSessionId == search.MeditationSessionId).Select(x => x.MeditationFileId);
                return GetMeditationFilesViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.MeditationFileId)).ReturnListModelResultAsync(search);
        }

        public Task<List<MeditationFile>> GetBareMeditationFileByMeditationSessionIdAsync(long meditationSessionId)
        {
                var idsSql = appDBManager.GetTable<MeditationSession>().Where(x => x.IsDeleted == false && x.MeditationSessionId == meditationSessionId).Select(x => x.MeditationFileId);
                return appDBManager.GetTable<MeditationFile>().Where(x => x.IsDeleted == false && idsSql.Contains(x.MeditationFileId)).ToListAsync();
        }

        public Task<DbReturnListModel<MeditationFile>> GetBareMeditationFileByMeditationSessionIdAsync(MeditationFileByMeditationSessionIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<MeditationSession>().Where(x => x.IsDeleted == false && x.MeditationSessionId == search.MeditationSessionId).Select(x => x.MeditationFileId);
                return appDBManager.GetTable<MeditationFile>().Where(x => x.IsDeleted == false && idsSql.Contains(x.MeditationFileId)).ReturnListModelResultAsync(search);
        }


        public Task<List<MeditationSessionView>> GetMeditationSessionViewByUserIdAsync(long userId)
        {
                return GetMeditationSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<MeditationSessionView>> GetMeditationSessionViewByUserIdAsync(MeditationSessionByUserIdSearchCriteria search)
        {
                return GetMeditationSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }

        public Task<List<MeditationSession>> GetBareMeditationSessionByUserIdAsync(long userId)
        {
                return appDBManager.GetTable<MeditationSession>().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<MeditationSession>> GetBareMeditationSessionByUserIdAsync(MeditationSessionByUserIdSearchCriteria search)
        {
                return appDBManager.GetTable<MeditationSession>().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }


        public Task<List<UserView>> GetUserViewByMeditationSessionIdAsync(long meditationSessionId)
        {
                var idsSql = appDBManager.GetTable<MeditationSession>().Where(x => x.IsDeleted == false && x.MeditationSessionId == meditationSessionId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<UserView>> GetUserViewByMeditationSessionIdAsync(UserByMeditationSessionIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<MeditationSession>().Where(x => x.IsDeleted == false && x.MeditationSessionId == search.MeditationSessionId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }

        public Task<List<User>> GetBareUserByMeditationSessionIdAsync(long meditationSessionId)
        {
                var idsSql = appDBManager.GetTable<MeditationSession>().Where(x => x.IsDeleted == false && x.MeditationSessionId == meditationSessionId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<User>> GetBareUserByMeditationSessionIdAsync(UserByMeditationSessionIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<MeditationSession>().Where(x => x.IsDeleted == false && x.MeditationSessionId == search.MeditationSessionId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }



        /*

        public async Task<bool> UpdateMeditationSessionPicturePathAsync(long meditationSessionId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<MeditationSession>().Where(x => x.MeditationSessionId == meditationSessionId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearMeditationSessionProfilePathAsync(long meditationSessionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<MeditationSession>().Where(x => x.MeditationSessionId == meditationSessionId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableMeditationSessionByIdAsync(long meditationSessionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<MeditationSession>().Where(x => x.MeditationSessionId == meditationSessionId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableMeditationSessionByIdAsync(long meditationSessionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<MeditationSession>().Where(x => x.MeditationSessionId == meditationSessionId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateMeditationSessionWithXYZZYXAsync(long meditationSessionId, long xYZZYXId, long? createdBy)
        {
            var item = new MeditationSessionXYZZYXAssociationModel();
            item.MeditationSessionId = meditationSessionId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<MeditationSessionXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateMeditationSessionWithXYZZYXAsync(long meditationSessionId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<MeditationSessionXYZZYX>(x => x.MeditationSessionId == meditationSessionId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesMeditationSessionWithXYZZYXAssociationExistsAsync(long meditationSessionId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<MeditationSessionXYZZYX>(x => x.MeditationSessionId == meditationSessionId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<MeditationSessionView>> GetMeditationSessionsByXYZZYXAsync(long xYZZYXId)
        {
            var meditationSessionIds = GetMeditationSessionXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.MeditationSessionId);
            var meditationSessionSql = GetMeditationSessionsViewAsIQueryable().Where(x => meditationSessionIds.Contains(x.MeditationSessionId));
            return meditationSessionSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByMeditationSessionsAsync(long meditationSessionId)
        {
            var xYZZYXIds = GetMeditationSessionXYZZYXsViewAsIQueryable().Where(x => x.MeditationSessionId == meditationSession).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<MeditationSessionXYZZYX> GetMeditationSessionXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<MeditationSessionXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetMeditationSessionXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.MeditationSessionXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

#region VisionBoardPicture

        public IQueryable<VisionBoardPictureView> GetVisionBoardPicturesViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<VisionBoardPicture>()
                       join a in appDBManager.GetTable<User>() on item.UserId equals a.UserId

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new VisionBoardPictureView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       UserName = a.UserName,

                       VisionBoardPictureId = item.VisionBoardPictureId,
                       Location = item.Location,
                       FileName = item.FileName,
                       FileSize = item.FileSize,
                       UserId = item.UserId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<VisionBoardPictureView> GetVisionBoardPicturesInternalWithSearchCriteriaAsync(VisionBoardPictureSearchCriteria search)
        {
            var sql1 = GetVisionBoardPicturesViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.VisionBoardPictureId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Location.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.FileName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
);
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

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

        public async Task<DbReturnListModel<VisionBoardPictureView>> GetVisionBoardPicturesInternalAsync(VisionBoardPictureSearchCriteria search)
        {
            var sql1 = GetVisionBoardPicturesInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.VisionBoardPictureId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetVisionBoardPicturesViewAsIQueryable().FirstOrDefaultAsync(x => x.VisionBoardPictureId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<VisionBoardPictureView>> GetVisionBoardPicturesAsync(VisionBoardPictureSearchCriteria search)
        {
            return GetVisionBoardPicturesInternalAsync(search);
        }

        public Task<VisionBoardPicture> GetVisionBoardPictureFromAnyByIdAsync(long visionBoardPictureId)
        {
            return appDBManager.FirstOrDefaultAsync<VisionBoardPicture>(x => x.VisionBoardPictureId == visionBoardPictureId);
        }

        public Task<VisionBoardPictureView> GetVisionBoardPictureViewFromAnyByIdAsync(long visionBoardPictureId)
        {
            return GetVisionBoardPicturesViewAsIQueryable().FirstOrDefaultAsync(x => x.VisionBoardPictureId == visionBoardPictureId);
        }

        public Task<VisionBoardPicture> GetVisionBoardPictureByUniqueIdentifierAsync(string visionBoardPictureId)
        {
            return appDBManager.FirstOrDefaultAsync<VisionBoardPicture>(x => x.VisionBoardPictureId == SafeUtils.Int(visionBoardPictureId) && x.IsDeleted == false);
        }

        public Task<VisionBoardPicture> GetVisionBoardPictureByIdAsync(long visionBoardPictureId)
        {
            return appDBManager.FirstOrDefaultAsync<VisionBoardPicture>(x => x.VisionBoardPictureId == visionBoardPictureId && x.IsDeleted == false);
        }

        public Task<List<VisionBoardPicture>> GetVisionBoardPictureByIdsAsync(List<long> visionBoardPictureIds)
        {
            return appDBManager.GetTable<VisionBoardPicture>().Where(x => visionBoardPictureIds.Contains(x.VisionBoardPictureId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<VisionBoardPictureView> GetVisionBoardPictureViewByIdAsync(long visionBoardPictureId)
        {
            var sql1 = GetVisionBoardPicturesViewAsIQueryable();

            sql1 = sql1.Where(x => x.VisionBoardPictureId == visionBoardPictureId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<VisionBoardPicture>> GetAllVisionBoardPictures()
        {
            return appDBManager.SelectAllAsync<VisionBoardPicture>();
        }

        /*
        public Task<VisionBoardPicture> GetVisionBoardPictureByVisionBoardPictureCodeAsync(long userId, string visionBoardPictureCode)
        {
            return appDBManager.FirstOrDefaultAsync<VisionBoardPicture>(x => x.UserId == userId && x.VisionBoardPictureCode == visionBoardPictureCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateVisionBoardPictureCodeAsync(long userId, long visionBoardPictureId, string visionBoardPictureCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardPicture>().Where(x => x.UserId == userId && x.VisionBoardPictureId == visionBoardPictureId && x.IsDeleted == false).Set(x => x.VisionBoardPictureCode, visionBoardPictureCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<bool> UpdateVisionBoardPictureFileLocationAsync(long visionBoardPictureId, string fileLocation, string fileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardPicture>().Where(x => x.VisionBoardPictureId == visionBoardPictureId).Set(x => x.Location, fileLocation).Set(x => x.FileName, fileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<VisionBoardPicture> CreateVisionBoardPictureAsync(VisionBoardPicture visionBoardPicture, long? createdBy)
        {
            visionBoardPicture.VisionBoardPictureId = await appDBManager.InsertWithAuditAsync<VisionBoardPicture, long>(visionBoardPicture, createdBy);
            return visionBoardPicture;
        }

        public async Task<List<VisionBoardPicture>> CreateVisionBoardPicturesAsync(List<VisionBoardPicture> visionBoardPictures, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<VisionBoardPicture, long>(visionBoardPictures, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                visionBoardPictures[i].VisionBoardPictureId = lst[i];
            return visionBoardPictures;
        }

        public Task<bool> UpdateVisionBoardPictureAsync(VisionBoardPicture visionBoardPicture, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<VisionBoardPicture>(visionBoardPicture, modifiedBy);
        }

        public async Task<bool> MarkVisionBoardPictureAsNotDeletedByIdAsync(long visionBoardPictureId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardPicture>().Where(x => x.VisionBoardPictureId == visionBoardPictureId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkVisionBoardPictureAsDeletedByIdAsync(long visionBoardPictureId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardPicture>().Where(x => x.VisionBoardPictureId == visionBoardPictureId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteVisionBoardPictureByIdAsync(long visionBoardPictureId)
        {
            return await appDBManager.GetTable<VisionBoardPicture>().Where(x => x.VisionBoardPictureId == visionBoardPictureId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetVisionBoardPictureActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.VisionBoardPicture);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsVisionBoardPictureNameAlreadyExistsForOtherVisionBoardPictureAsync(long visionBoardPictureId, string fileName, long userId)
        {
            var sql = GetVisionBoardPicturesViewAsIQueryable().Where(x => x.IsDeleted == false && x.VisionBoardPictureId != visionBoardPictureId && x.FileName.ToLower().Trim() == fileName.ToLower().Trim() && x.UserId == userId);
            return sql.AnyAsync();
        }

        public Task<bool> IsVisionBoardPictureNameAlreadyExistsForOtherVisionBoardPictureAsync(string fileName, long userId)
        {
            var sql = GetVisionBoardPicturesViewAsIQueryable().Where(x => x.IsDeleted == false && x.FileName.ToLower().Trim() == fileName.ToLower().Trim() && x.UserId == userId);
            return sql.AnyAsync();
        }

        

        public Task<List<VisionBoardPictureView>> GetVisionBoardPictureViewByUserIdAsync(long userId)
        {
                return GetVisionBoardPicturesViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<VisionBoardPictureView>> GetVisionBoardPictureViewByUserIdAsync(VisionBoardPictureByUserIdSearchCriteria search)
        {
                return GetVisionBoardPicturesViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }

        public Task<List<VisionBoardPicture>> GetBareVisionBoardPictureByUserIdAsync(long userId)
        {
                return appDBManager.GetTable<VisionBoardPicture>().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<VisionBoardPicture>> GetBareVisionBoardPictureByUserIdAsync(VisionBoardPictureByUserIdSearchCriteria search)
        {
                return appDBManager.GetTable<VisionBoardPicture>().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }


        public Task<List<UserView>> GetUserViewByVisionBoardPictureIdAsync(long visionBoardPictureId)
        {
                var idsSql = appDBManager.GetTable<VisionBoardPicture>().Where(x => x.IsDeleted == false && x.VisionBoardPictureId == visionBoardPictureId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<UserView>> GetUserViewByVisionBoardPictureIdAsync(UserByVisionBoardPictureIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<VisionBoardPicture>().Where(x => x.IsDeleted == false && x.VisionBoardPictureId == search.VisionBoardPictureId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }

        public Task<List<User>> GetBareUserByVisionBoardPictureIdAsync(long visionBoardPictureId)
        {
                var idsSql = appDBManager.GetTable<VisionBoardPicture>().Where(x => x.IsDeleted == false && x.VisionBoardPictureId == visionBoardPictureId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<User>> GetBareUserByVisionBoardPictureIdAsync(UserByVisionBoardPictureIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<VisionBoardPicture>().Where(x => x.IsDeleted == false && x.VisionBoardPictureId == search.VisionBoardPictureId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }



        /*

        public async Task<bool> UpdateVisionBoardPicturePicturePathAsync(long visionBoardPictureId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardPicture>().Where(x => x.VisionBoardPictureId == visionBoardPictureId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearVisionBoardPictureProfilePathAsync(long visionBoardPictureId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardPicture>().Where(x => x.VisionBoardPictureId == visionBoardPictureId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableVisionBoardPictureByIdAsync(long visionBoardPictureId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardPicture>().Where(x => x.VisionBoardPictureId == visionBoardPictureId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableVisionBoardPictureByIdAsync(long visionBoardPictureId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardPicture>().Where(x => x.VisionBoardPictureId == visionBoardPictureId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateVisionBoardPictureWithXYZZYXAsync(long visionBoardPictureId, long xYZZYXId, long? createdBy)
        {
            var item = new VisionBoardPictureXYZZYXAssociationModel();
            item.VisionBoardPictureId = visionBoardPictureId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<VisionBoardPictureXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateVisionBoardPictureWithXYZZYXAsync(long visionBoardPictureId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<VisionBoardPictureXYZZYX>(x => x.VisionBoardPictureId == visionBoardPictureId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesVisionBoardPictureWithXYZZYXAssociationExistsAsync(long visionBoardPictureId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<VisionBoardPictureXYZZYX>(x => x.VisionBoardPictureId == visionBoardPictureId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<VisionBoardPictureView>> GetVisionBoardPicturesByXYZZYXAsync(long xYZZYXId)
        {
            var visionBoardPictureIds = GetVisionBoardPictureXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.VisionBoardPictureId);
            var visionBoardPictureSql = GetVisionBoardPicturesViewAsIQueryable().Where(x => visionBoardPictureIds.Contains(x.VisionBoardPictureId));
            return visionBoardPictureSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByVisionBoardPicturesAsync(long visionBoardPictureId)
        {
            var xYZZYXIds = GetVisionBoardPictureXYZZYXsViewAsIQueryable().Where(x => x.VisionBoardPictureId == visionBoardPicture).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<VisionBoardPictureXYZZYX> GetVisionBoardPictureXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<VisionBoardPictureXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetVisionBoardPictureXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.VisionBoardPictureXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region VisionBoardSession

        public IQueryable<VisionBoardSessionView> GetVisionBoardSessionsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<VisionBoardSession>()
                       join a in appDBManager.GetTable<User>() on item.UserId equals a.UserId

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new VisionBoardSessionView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       UserName = a.UserName,

                       VisionBoardSessionId = item.VisionBoardSessionId,
                       VisionBoardSessionDate = item.VisionBoardSessionDate,
                       VisionBoardSessionTimeSpentInSeconds = item.VisionBoardSessionTimeSpentInSeconds,
                       UserId = item.UserId,
                       AddedToFeed = item.AddedToFeed,
                       AddedToFeedDate = item.AddedToFeedDate,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<VisionBoardSessionView> GetVisionBoardSessionsInternalWithSearchCriteriaAsync(VisionBoardSessionSearchCriteria search)
        {
            var sql1 = GetVisionBoardSessionsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.VisionBoardSessionId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                );
            }

    /*if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
    {
        if (search.StartDate.Value <= search.EndDate.Value)
        {
            sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay() && x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
        }
    }
    else if (search.StartDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate >= search.StartDate.Value.StartOfDay());
    }
    else if (search.EndDate.IsValidDate())
    {
        sql1 = sql1.Where(x => x.OrderDate || x.DueDate <= search.EndDate.Value.EndOfDay());
    }*/

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

        public async Task<DbReturnListModel<VisionBoardSessionView>> GetVisionBoardSessionsInternalAsync(VisionBoardSessionSearchCriteria search)
        {
            var sql1 = GetVisionBoardSessionsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.VisionBoardSessionId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetVisionBoardSessionsViewAsIQueryable().FirstOrDefaultAsync(x => x.VisionBoardSessionId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<VisionBoardSessionView>> GetVisionBoardSessionsAsync(VisionBoardSessionSearchCriteria search)
        {
            return GetVisionBoardSessionsInternalAsync(search);
        }

        public Task<VisionBoardSession> GetVisionBoardSessionFromAnyByIdAsync(long visionBoardSessionId)
        {
            return appDBManager.FirstOrDefaultAsync<VisionBoardSession>(x => x.VisionBoardSessionId == visionBoardSessionId);
        }

        public Task<VisionBoardSessionView> GetVisionBoardSessionViewFromAnyByIdAsync(long visionBoardSessionId)
        {
            return GetVisionBoardSessionsViewAsIQueryable().FirstOrDefaultAsync(x => x.VisionBoardSessionId == visionBoardSessionId);
        }

        public Task<VisionBoardSession> GetVisionBoardSessionByUniqueIdentifierAsync(string visionBoardSessionId)
        {
            return appDBManager.FirstOrDefaultAsync<VisionBoardSession>(x => x.VisionBoardSessionId == SafeUtils.Int(visionBoardSessionId) && x.IsDeleted == false);
        }

        public Task<VisionBoardSession> GetVisionBoardSessionByIdAsync(long visionBoardSessionId)
        {
            return appDBManager.FirstOrDefaultAsync<VisionBoardSession>(x => x.VisionBoardSessionId == visionBoardSessionId && x.IsDeleted == false);
        }

        public Task<List<VisionBoardSession>> GetVisionBoardSessionByIdsAsync(List<long> visionBoardSessionIds)
        {
            return appDBManager.GetTable<VisionBoardSession>().Where(x => visionBoardSessionIds.Contains(x.VisionBoardSessionId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<VisionBoardSessionView> GetVisionBoardSessionViewByIdAsync(long visionBoardSessionId)
        {
            var sql1 = GetVisionBoardSessionsViewAsIQueryable();

            sql1 = sql1.Where(x => x.VisionBoardSessionId == visionBoardSessionId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<VisionBoardSession>> GetAllVisionBoardSessions()
        {
            return appDBManager.SelectAllAsync<VisionBoardSession>();
        }

        /*
        public Task<VisionBoardSession> GetVisionBoardSessionByVisionBoardSessionCodeAsync(long userId, string visionBoardSessionCode)
        {
            return appDBManager.FirstOrDefaultAsync<VisionBoardSession>(x => x.UserId == userId && x.VisionBoardSessionCode == visionBoardSessionCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateVisionBoardSessionCodeAsync(long userId, long visionBoardSessionId, string visionBoardSessionCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardSession>().Where(x => x.UserId == userId && x.VisionBoardSessionId == visionBoardSessionId && x.IsDeleted == false).Set(x => x.VisionBoardSessionCode, visionBoardSessionCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<VisionBoardSession> CreateVisionBoardSessionAsync(VisionBoardSession visionBoardSession, long? createdBy)
        {
            visionBoardSession.VisionBoardSessionId = await appDBManager.InsertWithAuditAsync<VisionBoardSession, long>(visionBoardSession, createdBy);
            return visionBoardSession;
        }

        public async Task<List<VisionBoardSession>> CreateVisionBoardSessionsAsync(List<VisionBoardSession> visionBoardSessions, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<VisionBoardSession, long>(visionBoardSessions, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                visionBoardSessions[i].VisionBoardSessionId = lst[i];
            return visionBoardSessions;
        }

        public Task<bool> UpdateVisionBoardSessionAsync(VisionBoardSession visionBoardSession, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<VisionBoardSession>(visionBoardSession, modifiedBy);
        }

        public async Task<bool> MarkVisionBoardSessionAsNotDeletedByIdAsync(long visionBoardSessionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardSession>().Where(x => x.VisionBoardSessionId == visionBoardSessionId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkVisionBoardSessionAsDeletedByIdAsync(long visionBoardSessionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardSession>().Where(x => x.VisionBoardSessionId == visionBoardSessionId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteVisionBoardSessionByIdAsync(long visionBoardSessionId)
        {
            return await appDBManager.GetTable<VisionBoardSession>().Where(x => x.VisionBoardSessionId == visionBoardSessionId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetVisionBoardSessionActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.VisionBoardSession);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<List<VisionBoardSessionView>> GetVisionBoardSessionViewByUserIdAsync(long userId)
        {
                return GetVisionBoardSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<VisionBoardSessionView>> GetVisionBoardSessionViewByUserIdAsync(VisionBoardSessionByUserIdSearchCriteria search)
        {
                return GetVisionBoardSessionsViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }

        public Task<List<VisionBoardSession>> GetBareVisionBoardSessionByUserIdAsync(long userId)
        {
                return appDBManager.GetTable<VisionBoardSession>().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<VisionBoardSession>> GetBareVisionBoardSessionByUserIdAsync(VisionBoardSessionByUserIdSearchCriteria search)
        {
                return appDBManager.GetTable<VisionBoardSession>().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }


        public Task<List<UserView>> GetUserViewByVisionBoardSessionIdAsync(long visionBoardSessionId)
        {
                var idsSql = appDBManager.GetTable<VisionBoardSession>().Where(x => x.IsDeleted == false && x.VisionBoardSessionId == visionBoardSessionId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<UserView>> GetUserViewByVisionBoardSessionIdAsync(UserByVisionBoardSessionIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<VisionBoardSession>().Where(x => x.IsDeleted == false && x.VisionBoardSessionId == search.VisionBoardSessionId).Select(x => x.UserId);
                return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }

        public Task<List<User>> GetBareUserByVisionBoardSessionIdAsync(long visionBoardSessionId)
        {
                var idsSql = appDBManager.GetTable<VisionBoardSession>().Where(x => x.IsDeleted == false && x.VisionBoardSessionId == visionBoardSessionId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<User>> GetBareUserByVisionBoardSessionIdAsync(UserByVisionBoardSessionIdSearchCriteria search)
        {
                var idsSql = appDBManager.GetTable<VisionBoardSession>().Where(x => x.IsDeleted == false && x.VisionBoardSessionId == search.VisionBoardSessionId).Select(x => x.UserId);
                return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }



        /*

        public async Task<bool> UpdateVisionBoardSessionPicturePathAsync(long visionBoardSessionId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardSession>().Where(x => x.VisionBoardSessionId == visionBoardSessionId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearVisionBoardSessionProfilePathAsync(long visionBoardSessionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardSession>().Where(x => x.VisionBoardSessionId == visionBoardSessionId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableVisionBoardSessionByIdAsync(long visionBoardSessionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardSession>().Where(x => x.VisionBoardSessionId == visionBoardSessionId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableVisionBoardSessionByIdAsync(long visionBoardSessionId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VisionBoardSession>().Where(x => x.VisionBoardSessionId == visionBoardSessionId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateVisionBoardSessionWithXYZZYXAsync(long visionBoardSessionId, long xYZZYXId, long? createdBy)
        {
            var item = new VisionBoardSessionXYZZYXAssociationModel();
            item.VisionBoardSessionId = visionBoardSessionId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<VisionBoardSessionXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateVisionBoardSessionWithXYZZYXAsync(long visionBoardSessionId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<VisionBoardSessionXYZZYX>(x => x.VisionBoardSessionId == visionBoardSessionId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesVisionBoardSessionWithXYZZYXAssociationExistsAsync(long visionBoardSessionId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<VisionBoardSessionXYZZYX>(x => x.VisionBoardSessionId == visionBoardSessionId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<VisionBoardSessionView>> GetVisionBoardSessionsByXYZZYXAsync(long xYZZYXId)
        {
            var visionBoardSessionIds = GetVisionBoardSessionXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.VisionBoardSessionId);
            var visionBoardSessionSql = GetVisionBoardSessionsViewAsIQueryable().Where(x => visionBoardSessionIds.Contains(x.VisionBoardSessionId));
            return visionBoardSessionSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByVisionBoardSessionsAsync(long visionBoardSessionId)
        {
            var xYZZYXIds = GetVisionBoardSessionXYZZYXsViewAsIQueryable().Where(x => x.VisionBoardSessionId == visionBoardSession).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<VisionBoardSessionXYZZYX> GetVisionBoardSessionXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<VisionBoardSessionXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetVisionBoardSessionXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.VisionBoardSessionXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region DailyJournal

        public IQueryable<DailyJournalView> GetDailyJournalsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DailyJournal>()
                   join a in appDBManager.GetTable<User>() on item.UserId equals a.UserId
                   join b in appDBManager.GetTable<EnumType>() on item.PredefinedMoodTypeId equals b.EnumTypeId

                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new DailyJournalView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       UserName = a.UserName,
                       PredefinedMoodTypeName = b.EnumTypeName,

                       DailyJournalId = item.DailyJournalId,
                       DailyJournalDate = item.DailyJournalDate,
                       UserId = item.UserId,
                       PredefinedMoodTypeId = item.PredefinedMoodTypeId,
                       Gratitude1 = item.Gratitude1,
                       Gratitude2 = item.Gratitude2,
                       Gratitude3 = item.Gratitude3,
                       Gratitude4 = item.Gratitude4,
                       Gratitude5 = item.Gratitude5,
                       YourReflection = item.YourReflection,
                       TommorowsIntentions = item.TommorowsIntentions,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,

                   };
        }

        public IQueryable<DailyJournalView> GetDailyJournalsInternalWithSearchCriteriaAsync(DailyJournalSearchCriteria search)
        {
            var sql1 = GetDailyJournalsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.DailyJournalId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Gratitude1.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Gratitude2.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Gratitude3.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Gratitude4.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Gratitude5.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.YourReflection.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.TommorowsIntentions.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
);
            }

            if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
            {
                if (search.StartDate.Value <= search.EndDate.Value)
                {
                    sql1 = sql1.Where(x => x.DailyJournalDate >= search.StartDate.Value.StartOfDay() && x.DailyJournalDate <= search.EndDate.Value.EndOfDay());
                }
            }
            else if (search.StartDate.IsValidDate())
            {
                sql1 = sql1.Where(x => x.DailyJournalDate >= search.StartDate.Value.StartOfDay());
            }
            else if (search.EndDate.IsValidDate())
            {
                sql1 = sql1.Where(x => x.DailyJournalDate <= search.EndDate.Value.EndOfDay());
            }

            if (search.UserId != null && search.UserId > 0)
                sql1 = sql1.Where(x => search.UserId == x.UserId);

            if (search.PredefinedMoodTypeIds != null && search.PredefinedMoodTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedMoodTypeIds.Contains(x.PredefinedMoodTypeId));

            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);

            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<DailyJournalView>> GetDailyJournalsInternalAsync(DailyJournalSearchCriteria search)
        {
            var sql1 = GetDailyJournalsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.DailyJournalId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetDailyJournalsViewAsIQueryable().FirstOrDefaultAsync(x => x.DailyJournalId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<DailyJournalView>> GetDailyJournalsAsync(DailyJournalSearchCriteria search)
        {
            return GetDailyJournalsInternalAsync(search);
        }

        public Task<DailyJournal> GetDailyJournalFromAnyByIdAsync(long dailyJournalId)
        {
            return appDBManager.FirstOrDefaultAsync<DailyJournal>(x => x.DailyJournalId == dailyJournalId);
        }

        public Task<DailyJournalView> GetDailyJournalViewFromAnyByIdAsync(long dailyJournalId)
        {
            return GetDailyJournalsViewAsIQueryable().FirstOrDefaultAsync(x => x.DailyJournalId == dailyJournalId);
        }

        public Task<DailyJournal> GetDailyJournalByUniqueIdentifierAsync(string dailyJournalId)
        {
            return appDBManager.FirstOrDefaultAsync<DailyJournal>(x => x.DailyJournalId == SafeUtils.Int(dailyJournalId) && x.IsDeleted == false);
        }

        public Task<DailyJournal> GetDailyJournalByIdAsync(long dailyJournalId)
        {
            return appDBManager.FirstOrDefaultAsync<DailyJournal>(x => x.DailyJournalId == dailyJournalId && x.IsDeleted == false);
        }

        public Task<List<DailyJournal>> GetDailyJournalByIdsAsync(List<long> dailyJournalIds)
        {
            return appDBManager.GetTable<DailyJournal>().Where(x => dailyJournalIds.Contains(x.DailyJournalId) && x.IsDeleted == false).ToListAsync();
        }

        public Task<DailyJournalView> GetDailyJournalViewByIdAsync(long dailyJournalId)
        {
            var sql1 = GetDailyJournalsViewAsIQueryable();

            sql1 = sql1.Where(x => x.DailyJournalId == dailyJournalId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<DailyJournal>> GetAllDailyJournals()
        {
            return appDBManager.SelectAllAsync<DailyJournal>();
        }

        /*
        public Task<DailyJournal> GetDailyJournalByDailyJournalCodeAsync(long companyId, string dailyJournalCode)
        {
            return appDBManager.FirstOrDefaultAsync<DailyJournal>(x => x.CompanyId == companyId && x.DailyJournalCode == dailyJournalCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateDailyJournalCodeAsync(long companyId, long dailyJournalId, string dailyJournalCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyJournal>().Where(x => x.CompanyId == companyId && x.DailyJournalId == dailyJournalId && x.IsDeleted == false).Set(x => x.DailyJournalCode, dailyJournalCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<DailyJournal> CreateDailyJournalAsync(DailyJournal dailyJournal, long? createdBy)
        {
            dailyJournal.DailyJournalId = await appDBManager.InsertWithAuditAsync<DailyJournal, long>(dailyJournal, createdBy);
            return dailyJournal;
        }

        public async Task<List<DailyJournal>> CreateDailyJournalsAsync(List<DailyJournal> dailyJournals, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<DailyJournal, long>(dailyJournals, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                dailyJournals[i].DailyJournalId = lst[i];
            return dailyJournals;
        }

        public Task<bool> UpdateDailyJournalAsync(DailyJournal dailyJournal, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<DailyJournal>(dailyJournal, modifiedBy);
        }

        public async Task<bool> MarkDailyJournalAsNotDeletedByIdAsync(long dailyJournalId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyJournal>().Where(x => x.DailyJournalId == dailyJournalId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkDailyJournalAsDeletedByIdAsync(long dailyJournalId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyJournal>().Where(x => x.DailyJournalId == dailyJournalId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteDailyJournalByIdAsync(long dailyJournalId)
        {
            return await appDBManager.GetTable<DailyJournal>().Where(x => x.DailyJournalId == dailyJournalId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetDailyJournalActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.DailyJournal);

            return GetActivitiesByExpressionAsync(srch, search);
        }


        public Task<List<DailyJournalView>> GetDailyJournalViewByUserIdAsync(long userId)
        {
            return GetDailyJournalsViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<DailyJournalView>> GetDailyJournalViewByUserIdAsync(DailyJournalByUserIdSearchCriteria search)
        {
            return GetDailyJournalsViewAsIQueryable().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }

        public Task<List<DailyJournal>> GetBareDailyJournalByUserIdAsync(long userId)
        {
            return appDBManager.GetTable<DailyJournal>().Where(x => x.IsDeleted == false && x.UserId == userId).ToListAsync();
        }

        public Task<DbReturnListModel<DailyJournal>> GetBareDailyJournalByUserIdAsync(DailyJournalByUserIdSearchCriteria search)
        {
            return appDBManager.GetTable<DailyJournal>().Where(x => x.IsDeleted == false && x.UserId == search.UserId).ReturnListModelResultAsync(search);
        }


        public Task<List<UserView>> GetUserViewByDailyJournalIdAsync(long dailyJournalId)
        {
            var idsSql = appDBManager.GetTable<DailyJournal>().Where(x => x.IsDeleted == false && x.DailyJournalId == dailyJournalId).Select(x => x.UserId);
            return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<UserView>> GetUserViewByDailyJournalIdAsync(UserByDailyJournalIdSearchCriteria search)
        {
            var idsSql = appDBManager.GetTable<DailyJournal>().Where(x => x.IsDeleted == false && x.DailyJournalId == search.DailyJournalId).Select(x => x.UserId);
            return companyRepository.GetUsersViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }

        public Task<List<User>> GetBareUserByDailyJournalIdAsync(long dailyJournalId)
        {
            var idsSql = appDBManager.GetTable<DailyJournal>().Where(x => x.IsDeleted == false && x.DailyJournalId == dailyJournalId).Select(x => x.UserId);
            return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ToListAsync();
        }

        public Task<DbReturnListModel<User>> GetBareUserByDailyJournalIdAsync(UserByDailyJournalIdSearchCriteria search)
        {
            var idsSql = appDBManager.GetTable<DailyJournal>().Where(x => x.IsDeleted == false && x.DailyJournalId == search.DailyJournalId).Select(x => x.UserId);
            return appDBManager.GetTable<User>().Where(x => x.IsDeleted == false && idsSql.Contains(x.UserId)).ReturnListModelResultAsync(search);
        }



        /*

        public async Task<bool> UpdateDailyJournalPicturePathAsync(long dailyJournalId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyJournal>().Where(x => x.DailyJournalId == dailyJournalId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearDailyJournalProfilePathAsync(long dailyJournalId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyJournal>().Where(x => x.DailyJournalId == dailyJournalId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableDailyJournalByIdAsync(long dailyJournalId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyJournal>().Where(x => x.DailyJournalId == dailyJournalId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableDailyJournalByIdAsync(long dailyJournalId, long? modifiedBy)
        {
            return await appDBManager.GetTable<DailyJournal>().Where(x => x.DailyJournalId == dailyJournalId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateDailyJournalWithXYZZYXAsync(long dailyJournalId, long xYZZYXId, long? createdBy)
        {
            var item = new DailyJournalXYZZYXAssociationModel();
            item.DailyJournalId = dailyJournalId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<DailyJournalXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateDailyJournalWithXYZZYXAsync(long dailyJournalId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<DailyJournalXYZZYX>(x => x.DailyJournalId == dailyJournalId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesDailyJournalWithXYZZYXAssociationExistsAsync(long dailyJournalId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<DailyJournalXYZZYX>(x => x.DailyJournalId == dailyJournalId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<DailyJournalView>> GetDailyJournalsByXYZZYXAsync(long xYZZYXId)
        {
            var dailyJournalIds = GetDailyJournalXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.DailyJournalId);
            var dailyJournalSql = GetDailyJournalsViewAsIQueryable().Where(x => dailyJournalIds.Contains(x.DailyJournalId));
            return dailyJournalSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByDailyJournalsAsync(long dailyJournalId)
        {
            var xYZZYXIds = GetDailyJournalXYZZYXsViewAsIQueryable().Where(x => x.DailyJournalId == dailyJournal).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<DailyJournalXYZZYX> GetDailyJournalXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<DailyJournalXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetDailyJournalXYZZYXActivitiesAsync(long companyId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.CompanyId == companyId && x.ResourceTypeId == (int)ResourceTypes.DailyJournalXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

    }
}