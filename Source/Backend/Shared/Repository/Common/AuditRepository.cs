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
using Framework.Infrastructure.Interfaces.DbAccess;
using Framework.Infrastructure.Interfaces.Helpers;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using LinqToDB;
using ObjectsComparator.Comparator.Helpers;

namespace Manifest.Repository.Common
{
    public class AuditRepository : BaseRepository
    {
        private readonly ICurrentUser currentUser;

        public AuditRepository(IAppDBManager appDBManager, ICurrentUser currentUser)
            : base(appDBManager)
        {
            this.currentUser = currentUser;
        }

        public async Task InsertAuditAsync(PredefinedAuditOperationTypes auditOperationType, string objectBeforeUpdate, string objectAfterUpdate, Guid? auditOperationGroupGuid = null, PredefinedResourceTypes? resourceType = null, string resourceName = null, long? resourceId = null, long? userId = null, string userName = null, string ipAddress = null, string description = null, string changeDiff = null, string browserName = null, string operatingSystemName = null, string deviceName = null, long? movieId = null, long? artistId = null, long? equipmentId = null, long? vendorId = null, long? technicianId = null, long? movieSceneId = null, long? budgetDivisionId = null, long? budgetCategoryId = null, long? movieShootDayId = null, long? propertyId = null, long? createdBy = null, long? modifiedBy = null)
        {
            var item = new AuditOperation
            {
                AuditDateTime = DateTime.UtcNow,
                PredefinedAuditOperationTypeId = (int)auditOperationType,
                PredefinedResourceTypeId = resourceType != null ? (int)resourceType : null,
                UserId = userId,
                IpAddress = ipAddress ?? currentUser.GetCurrentUserAddress(),
                Description = description,
                ChangeDiff = changeDiff,
                BrowserName = browserName,
                OperatingSystemName = operatingSystemName,
                DeviceName = deviceName,

                /*ApprovalId = ApprovalId,
                ApprovalCategoryId = ApprovalCategoryId,
                ApprovalCommentId = ApprovalCommentId,
                ApprovalPageId = ApprovalPageId,
                ApprovalPageFieldId = ApprovalPageFieldId,
                ApprovalUserId = ApprovalUserId,
                ControlId = ControlId,
                ControlCategoryId = ControlCategoryId,
                ControlItemId = ControlItemId,
                ControlItemCommentId = ControlItemCommentId,
                ControlItemUserId = ControlItemUserId,
                ControlPageId = ControlPageId,
                ControlPageFieldId = ControlPageFieldId,
                CustomFieldTemplateId = CustomFieldTemplateId,
                CustomFieldTemplatePageId = CustomFieldTemplatePageId,
                CustomFieldTemplatePageFieldId = CustomFieldTemplatePageFieldId,
                ProjectId = ProjectId,
                ProjectCategoryId = ProjectCategoryId,
                ProjectUserId = ProjectUserId,
                ReviewId = ReviewId,
                ReviewCategoryId = ReviewCategoryId,
                ReviewPageId = ReviewPageId,
                ReviewPageFieldId = ReviewPageFieldId,
                ReviewSessionId = ReviewSessionId,
                ReviewSessionCommentId = ReviewSessionCommentId,
                ReviewUserId = ReviewUserId,
                UserGroupId = UserGroupId,
                UserGroupUserId = UserGroupUserId,
                UserPermissionId = UserPermissionId,
                WorkflowId = WorkflowId,
                WorkflowSessionId = WorkflowSessionId,
                WorkflowStageId = WorkflowStageId,
                WorkflowStageSessionId = WorkflowStageSessionId,
                WorkflowTemplateId = WorkflowTemplateId,
                WorkflowTemplateCategoryId = WorkflowTemplateCategoryId,
                WorkflowTemplateStageId = WorkflowTemplateStageId,
                PredefinedResourceTypeId = PredefinedResourceTypeId,
                */
                ResourceId = resourceId,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = createdBy,
                ModifiedDate = DateTime.UtcNow,
                ModifiedByUserId = modifiedBy,
            };
            var auditOperationId = await appDBManager.InsertAsync<AuditOperation, long>(item);
            _ = await appDBManager.InsertAsync<AuditOperationDetail>(new AuditOperationDetail
            {
                AuditOperationId = auditOperationId,
                DifferenceJson = "",
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
            });
        }

        public async Task InsertAuditAsync<T>(PredefinedAuditOperationTypes auditOperationType, T objectBeforeUpdate, T objectAfterUpdate, Guid? auditOperationGroupGuid = null, PredefinedResourceTypes? resourceType = null, string resourceName = null, long? resourceId = null, long? userId = null, string userName = null, string ipAddress = null, string description = null, string changeDiff = null, string browserName = null, string operatingSystemName = null, string deviceName = null, long? movieId = null, long? artistId = null, long? equipmentId = null, long? vendorId = null, long? technicianId = null, long? movieSceneId = null, long? budgetDivisionId = null, long? budgetCategoryId = null, long? movieShootDayId = null, long? propertyId = null, long? createdBy = null, long? modifiedBy = null)
            where T : INewObject<T>
        {
            var jsonResultByCompareObjects = "";

            if (objectBeforeUpdate != null && objectAfterUpdate != null && objectBeforeUpdate is INewObject<T> && objectAfterUpdate is INewObject<T> && typeof(T) != typeof(string))
            {
                objectBeforeUpdate = objectBeforeUpdate.NewFromObject();
                objectAfterUpdate = objectAfterUpdate.NewFromObject();

                var diffObj = objectBeforeUpdate.DeeplyEquals(objectAfterUpdate);

                if (diffObj.IsNotNullOrEmpty())
                {
                    jsonResultByCompareObjects = JsonUtils.Serialize(diffObj);
                }
            }

            var item = new AuditOperation
            {
                AuditDateTime = DateTime.UtcNow,
                PredefinedAuditOperationTypeId = (int)auditOperationType,
                PredefinedResourceTypeId = resourceType != null ? (int)resourceType : null,
                UserId = userId,
                IpAddress = ipAddress ?? currentUser.GetCurrentUserAddress(),
                Description = description,
                ChangeDiff = changeDiff,
                BrowserName = browserName,
                OperatingSystemName = operatingSystemName,
                DeviceName = deviceName,
                /*ApprovalId = ApprovalId,
                ApprovalCategoryId = ApprovalCategoryId,
                ApprovalCommentId = ApprovalCommentId,
                ApprovalPageId = ApprovalPageId,
                ApprovalPageFieldId = ApprovalPageFieldId,
                ApprovalUserId = ApprovalUserId,
                ControlId = ControlId,
                ControlCategoryId = ControlCategoryId,
                ControlItemId = ControlItemId,
                ControlItemCommentId = ControlItemCommentId,
                ControlItemUserId = ControlItemUserId,
                ControlPageId = ControlPageId,
                ControlPageFieldId = ControlPageFieldId,
                CustomFieldTemplateId = CustomFieldTemplateId,
                CustomFieldTemplatePageId = CustomFieldTemplatePageId,
                CustomFieldTemplatePageFieldId = CustomFieldTemplatePageFieldId,
                ProjectId = ProjectId,
                ProjectCategoryId = ProjectCategoryId,
                ProjectUserId = ProjectUserId,
                ReviewId = ReviewId,
                ReviewCategoryId = ReviewCategoryId,
                ReviewPageId = ReviewPageId,
                ReviewPageFieldId = ReviewPageFieldId,
                ReviewSessionId = ReviewSessionId,
                ReviewSessionCommentId = ReviewSessionCommentId,
                ReviewUserId = ReviewUserId,
                UserGroupId = UserGroupId,
                UserGroupUserId = UserGroupUserId,
                UserPermissionId = UserPermissionId,
                WorkflowId = WorkflowId,
                WorkflowSessionId = WorkflowSessionId,
                WorkflowStageId = WorkflowStageId,
                WorkflowStageSessionId = WorkflowStageSessionId,
                WorkflowTemplateId = WorkflowTemplateId,
                WorkflowTemplateCategoryId = WorkflowTemplateCategoryId,
                WorkflowTemplateStageId = WorkflowTemplateStageId,
                PredefinedResourceTypeId = PredefinedResourceTypeId,
                */
                ResourceId = resourceId,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = createdBy,
                ModifiedDate = DateTime.UtcNow,
                ModifiedByUserId = modifiedBy,
            };
            var auditOperationId = await appDBManager.InsertAsync<AuditOperation, long>(item);
            _ = await appDBManager.InsertAsync<AuditOperationDetail>(new AuditOperationDetail
            {
                AuditOperationId = auditOperationId,
                DifferenceJson = jsonResultByCompareObjects ?? "",
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
            });
        }

        #region AuditOperation
        public IQueryable<AuditOperationView> GetAuditOperationsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<AuditOperation>()
                   join a in GetEnumQueryableOfPredefinedAuditOperationType() on item.PredefinedAuditOperationTypeId equals a.EnumTypeId
                   join od in appDBManager.GetTable<User>() on item.UserId equals od.UserId into dTemp
                   from d in dTemp.DefaultIfEmpty()
                   select new AuditOperationView
                   {
                       PredefinedAuditOperationTypeName = a.EnumTypeName,
                       UserName = d.UserName,
                       AuditOperationId = item.AuditOperationId,
                       AuditDateTime = item.AuditDateTime,
                       PredefinedAuditOperationTypeId = item.PredefinedAuditOperationTypeId,
                       UserId = item.UserId,
                       CreatedByUserFirstName = d.FirstName,
                       CreatedByUserLastName = d.LastName,
                       IpAddress = item.IpAddress,
                       Description = item.Description,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                       FirstName = d.FirstName,
                       LastName = d.LastName,
                       ChangeDiff = item.ChangeDiff,
                       IsDeleted = item.IsDeleted,
                       PredefinedResourceTypeId = item.PredefinedResourceTypeId,
                       ResourceId = item.ResourceId,
                       BrowserName = item.BrowserName,
                       OperatingSystemName = item.OperatingSystemName,
                       DeviceName = item.DeviceName,
                       UserGroupId = item.UserGroupId,
                       UserGroupUserId = item.UserGroupUserId,
                       UserPermissionId = item.UserPermissionId,
                   };
        }

        public IQueryable<AuditOperationView> GetAuditOperationsInternalWithSearchCriteriaAsync(AuditOperationSearchCriteria search)
        {
            var sql1 = GetAuditOperationsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.AuditOperationId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.IpAddress.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Description.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.PredefinedAuditOperationTypeIds != null && search.PredefinedAuditOperationTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedAuditOperationTypeIds.Contains(x.PredefinedAuditOperationTypeId));

            if (search.UserIds != null && search.UserIds.Count > 0)
                sql1 = sql1.Where(x => search.UserIds.Contains((long)x.UserId));

            if (!search.IpAddress.IsTrimmedStringNullOrEmpty())
                sql1 = sql1.Where(x => search.IpAddress.Contains(x.IpAddress));

            if (search.SortBy.IsTrimmedStringNullOrEmpty())
            {
                search.SortBy = "CreatedDate";
                search.SortAscending = false;
            }

            return sql1;
        }

        public async Task<DbReturnListModel<AuditOperationView>> GetAuditOperationsInternalAsync(AuditOperationSearchCriteria search)
        {
            var sql1 = GetAuditOperationsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.AuditOperationId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetAuditOperationsViewAsIQueryable().FirstOrDefaultAsync(x => x.AuditOperationId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<AuditOperationView>> GetAuditOperationsAsync(AuditOperationSearchCriteria search)
        {
            return GetAuditOperationsInternalAsync(search);
        }

        public Task<AuditOperation> GetAuditOperationByUniqueIdentifierAsync(string auditOperationId)
        {
            return appDBManager.FirstOrDefaultAsync<AuditOperation>(x => x.AuditOperationId == auditOperationId.Int());
        }

        public Task<AuditOperation> GetAuditOperationByIdAsync(long auditOperationId)
        {
            return appDBManager.FirstOrDefaultAsync<AuditOperation>(x => x.AuditOperationId == auditOperationId);
        }

        public Task<AuditOperationView> GetAuditOperationViewByIdAsync(long auditOperationId)
        {
            var sql1 = GetAuditOperationsViewAsIQueryable();

            sql1 = sql1.Where(x => x.AuditOperationId == auditOperationId);
            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<AuditOperation>> GetAllAuditOperations()
        {
            return appDBManager.SelectAllAsync<AuditOperation>();
        }

        public async Task<AuditOperation> CreateAuditOperationAsync(AuditOperation auditOperation, long? createdBy)
        {
            auditOperation.AuditOperationId = await appDBManager.InsertWithAuditAsync<AuditOperation, long>(auditOperation, createdBy);
            return auditOperation;
        }

        public async Task<List<AuditOperation>> CreateAuditOperationsAsync(List<AuditOperation> auditOperations, string createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<AuditOperation, long>(auditOperations, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                auditOperations[i].AuditOperationId = lst[i];
            return auditOperations;
        }

        public Task<bool> UpdateAuditOperationAsync(AuditOperation auditOperation, string modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<AuditOperation>(auditOperation, modifiedBy);
        }

        public async Task<bool> HardDeleteAuditOperationByIdAsync(long auditOperationId)
        {
            return await appDBManager.GetTable<AuditOperation>().Where(x => x.AuditOperationId == auditOperationId).DeleteAsync() > 0;
        }

        public Task<int> GetLossPreventionCountAsync(AuditOperationSearchCriteria search)
        {
            var sql1 = GetAuditOperationsViewAsIQueryable();

            if (search.FromDate != null && search.ToDate != null)
            {
                sql1 = sql1.Where(x => x.AuditDateTime >= search.FromDate.Value.StartOfDay() && x.AuditDateTime <= search.ToDate.Value.EndOfDay());
            }

            sql1 = sql1.Where(x => search.PredefinedAuditOperationTypeIds.Contains(x.PredefinedAuditOperationTypeId));

            return sql1.Select(x => x.AuditOperationId).CountAsync();
        }
        #endregion AuditOperation
    }
}
