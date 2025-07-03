using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Repository.DbAccess;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.DBView;
using Manifest.Common.Models.DBView.System;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.SearchCriteria.System;
using Framework.Data.Utils;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using LinqToDB;
using static LinqToDB.Common.Configuration;

namespace Manifest.Repository.Common
{
    public partial class SystemRepository : BaseRepository
    {
        public SystemRepository(IAppDBManager appDBManager)
            : base(appDBManager)
        {
        }

        #region Country
        public Task<Country> GetCountryByIdAsync(long countryId)
        {
            return appDBManager.GetTable<Country>().FirstOrDefaultAsync(x => x.CountryId == countryId);
        }

        public Task<List<Country>> GetCountriesByIdsAsync(List<long> countryIds)
        {
            return appDBManager.GetTable<Country>().Where(x => countryIds.Contains(x.CountryId)).ToListAsync();
        }

        public async Task<long?> GetCountryIdFromNameAsync(string countryCode)
        {
            if (countryCode == null)
                countryCode = "";

            return (await appDBManager.FirstOrDefaultAsync<Country>(x => x.CountryName.ToLower() == countryCode.Trim().ToLower()))?.CountryId;
        }

        public Task<List<Country>> GetCountriesAsync(string keyword)
        {
            var sql = appDBManager.GetTable<Country>().AsQueryable();

            if (!keyword.IsTrimmedStringNullOrEmpty())
            {
                sql = sql.Where(x => x.CountryName.ToLower().Contains(keyword.ToLower().Trim()));
            }

            return sql.ToListAsync();
        }

        #endregion

        #region State

        public IQueryable<StateView> GetStatesViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<State>()
                   join a in appDBManager.GetTable<Country>() on item.CountryId equals a.CountryId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new StateView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       CountryName = a.CountryName,
                       StateId = item.StateId,
                       StateName = item.StateName,
                       StateCode = item.StateCode,
                       CountryId = item.CountryId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public async Task<DbReturnListModel<StateView>> GetStatesInternalAsync(StateSearchCriteria search)
        {
            var sql1 = GetStatesViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.StateName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.StateCode.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.CountryId != null && search.CountryId > 0)
                sql1 = sql1.Where(x => search.CountryId == x.CountryId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            if (search.SortBy.IsTrimmedStringNullOrEmpty())
            {
                search.SortBy = State.COLUMN_StateName;
                search.SortAscending = true;
            }

            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.StateId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetStatesViewAsIQueryable().FirstOrDefaultAsync(x => x.StateId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<StateView>> GetStatesAsync(StateSearchCriteria search)
        {
            return GetStatesInternalAsync(search);
        }

        public Task<State> GetStateFromAnyByIdAsync(long stateId)
        {
            return appDBManager.FirstOrDefaultAsync<State>(x => x.StateId == stateId);
        }

        public async Task<long?> GetStateIdFromCodeAsync(string stateCode)
        {
            if (stateCode == null)
                stateCode = "";

            return (await appDBManager.FirstOrDefaultAsync<State>(x => x.StateCode.ToLower() == stateCode.Trim().ToLower()))?.StateId;
        }

        public async Task<long?> GetStateIdFromCodeOrNameAsync(string stateCode)
        {
            if (stateCode == null)
                stateCode = "";

            return (await appDBManager.FirstOrDefaultAsync<State>(x => x.StateName.ToLower() == stateCode.Trim().ToLower() && x.StateCode.ToLower() == stateCode.Trim().ToLower()))?.StateId;
        }

        public Task<StateView> GetStateViewFromAnyByIdAsync(long stateId)
        {
            return GetStatesViewAsIQueryable().FirstOrDefaultAsync(x => x.StateId == stateId);
        }

        public Task<StateView> GetStateViewByStateNameAndCountryIdAsync(string stateName, long countryId)
        {
            return GetStatesViewAsIQueryable().FirstOrDefaultAsync(x => x.StateName.ToLower().Trim() == stateName.ToLower().Trim() && x.CountryId == countryId && x.IsDeleted == false);
        }

        public Task<State> GetStateByIdAsync(long stateId)
        {
            return appDBManager.FirstOrDefaultAsync<State>(x => x.StateId == stateId && x.IsDeleted == false);
        }

        public Task<State> GetStateByUniqueIdentifierAsync(string stateId)
        {
            return appDBManager.FirstOrDefaultAsync<State>(x => x.StateId == stateId.Int() && x.IsDeleted == false);
        }

        public Task<StateView> GetStateViewByIdAsync(long stateId)
        {
            var sql1 = GetStatesViewAsIQueryable();

            sql1 = sql1.Where(x => x.StateId == stateId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<State>> GetAllStates()
        {
            return appDBManager.SelectAllAsync<State>();
        }

        public async Task<State> CreateStateAsync(State state, long? createdBy)
        {
            state.StateId = await appDBManager.InsertWithAuditAsync<State, long>(state, createdBy);
            return state;
        }

        public async Task<List<State>> CreateStatesAsync(List<State> states, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<State, long>(states, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                states[i].StateId = lst[i];
            return states;
        }

        public Task<bool> UpdateStateAsync(State state, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<State>(state, modifiedBy);
        }

        public async Task<bool> MarkStateAsNotDeletedByIdAsync(long stateId, long? modifiedBy)
        {
            return await appDBManager.GetTable<State>().Where(x => x.StateId == stateId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkStateAsDeletedByIdAsync(long stateId, long? modifiedBy)
        {
            return await appDBManager.GetTable<State>().Where(x => x.StateId == stateId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteStateByIdAsync(long stateId)
        {
            return await appDBManager.GetTable<State>().Where(x => x.StateId == stateId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetStateActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.State);
            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsStateNameAlreadyExistsForOtherStateAsync(long stateId, string stateName, long? countryId)
        {
            var sql = GetStatesViewAsIQueryable().Where(x => x.IsDeleted == false && x.StateId != stateId && x.StateName.ToLower().Trim() == stateName.ToLower().Trim());
            if (countryId != null)
                sql = sql.Where(x => x.CountryId == countryId.Value);
            return sql.AnyAsync();
        }

        public Task<bool> IsStateNameAlreadyExistsForOtherStateAsync(string stateName, long? countryId)
        {
            var sql = GetStatesViewAsIQueryable().Where(x => x.IsDeleted == false && x.StateName.ToLower().Trim() == stateName.ToLower().Trim());
            if (countryId != null)
                sql = sql.Where(x => x.CountryId == countryId.Value);
            return sql.AnyAsync();
        }
        #endregion

        #region AuditOperation

        public IQueryable<AuditOperationView> GetAuditOperationsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<AuditOperation>()
                   join a in appDBManager.GetTable<EnumType>() on item.PredefinedAuditOperationTypeId equals a.EnumTypeId
                   join ob in appDBManager.GetTable<User>() on item.UserId equals ob.UserId into bTemp
                   from b in bTemp.DefaultIfEmpty()
                   join oe in appDBManager.GetTable<EnumType>() on item.PredefinedResourceTypeId equals oe.EnumTypeId into eTemp
                   from e in eTemp.DefaultIfEmpty()
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new AuditOperationView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       PredefinedAuditOperationTypeName = a.EnumTypeName,
                       UserName = b.UserName,
                       PredefinedResourceTypeName = e.EnumTypeName,
                       AuditOperationId = item.AuditOperationId,
                       AuditDateTime = item.AuditDateTime,
                       PredefinedAuditOperationTypeId = item.PredefinedAuditOperationTypeId,
                       ChangeDiff = item.ChangeDiff,
                       UserId = item.UserId,
                       BrowserName = item.BrowserName,
                       DeviceName = item.DeviceName,
                       OperatingSystemName = item.OperatingSystemName,
                       UserGroupId = item.UserGroupId,
                       UserGroupUserId = item.UserGroupUserId,
                       UserPermissionId = item.UserPermissionId,
                       PredefinedResourceTypeId = item.PredefinedResourceTypeId,
                       ResourceId = item.ResourceId,
                       IpAddress = item.IpAddress,
                       Description = item.Description,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
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
            if (search.UserId != null && search.UserId > 0)
                sql1 = sql1.Where(x => search.UserId == x.UserId);
            if (search.PredefinedResourceTypeIds != null && search.PredefinedResourceTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedResourceTypeIds.Contains(x.PredefinedResourceTypeId ?? 0));
            if (search.ResourceId != null && search.ResourceId > 0)
                sql1 = sql1.Where(x => search.ResourceId == x.ResourceId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

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

        public Task<AuditOperation> GetAuditOperationFromAnyByIdAsync(long auditOperationId)
        {
            return appDBManager.FirstOrDefaultAsync<AuditOperation>(x => x.AuditOperationId == auditOperationId);
        }

        public Task<AuditOperationView> GetAuditOperationViewFromAnyByIdAsync(long auditOperationId)
        {
            return GetAuditOperationsViewAsIQueryable().FirstOrDefaultAsync(x => x.AuditOperationId == auditOperationId);
        }

        public Task<AuditOperation> GetAuditOperationByIdAsync(long auditOperationId)
        {
            return appDBManager.FirstOrDefaultAsync<AuditOperation>(x => x.AuditOperationId == auditOperationId && x.IsDeleted == false);
        }

        public Task<AuditOperation> GetAuditOperationByUniqueIdentifierAsync(string auditOperationId)
        {
            return appDBManager.FirstOrDefaultAsync<AuditOperation>(x => x.AuditOperationId == auditOperationId.Int() && x.IsDeleted == false);
        }

        public Task<AuditOperationView> GetAuditOperationViewByIdAsync(long auditOperationId)
        {
            var sql1 = GetAuditOperationsViewAsIQueryable();

            sql1 = sql1.Where(x => x.AuditOperationId == auditOperationId && x.IsDeleted == false);

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

        public async Task<List<AuditOperation>> CreateAuditOperationsAsync(List<AuditOperation> auditOperations, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<AuditOperation, long>(auditOperations, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                auditOperations[i].AuditOperationId = lst[i];
            return auditOperations;
        }

        public Task<bool> UpdateAuditOperationAsync(AuditOperation auditOperation, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<AuditOperation>(auditOperation, modifiedBy);
        }

        public async Task<bool> MarkAuditOperationAsNotDeletedByIdAsync(long auditOperationId, long? modifiedBy)
        {
            return await appDBManager.GetTable<AuditOperation>().Where(x => x.AuditOperationId == auditOperationId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkAuditOperationAsDeletedByIdAsync(long auditOperationId, long? modifiedBy)
        {
            return await appDBManager.GetTable<AuditOperation>().Where(x => x.AuditOperationId == auditOperationId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteAuditOperationByIdAsync(long auditOperationId)
        {
            return await appDBManager.GetTable<AuditOperation>().Where(x => x.AuditOperationId == auditOperationId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetAuditOperationActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.AuditOperation);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        /*

        public async Task<bool> EnableAuditOperationByIdAsync(long auditOperationId, long? modifiedBy)
        {
            return await appDBManager.GetTable<AuditOperation>().Where(x => x.AuditOperationId == auditOperationId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableAuditOperationByIdAsync(long auditOperationId, long? modifiedBy)
        {
            return await appDBManager.GetTable<AuditOperation>().Where(x => x.AuditOperationId == auditOperationId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateAuditOperationWithXYZZYXAsync(long auditOperationId, long xYZZYXId, long? createdBy)
        {
            var item = new AuditOperationXYZZYXAssociationModel();
            item.AuditOperationId = auditOperationId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<AuditOperationXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateAuditOperationWithXYZZYXAsync(long auditOperationId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<AuditOperationXYZZYX>(x => x.AuditOperationId == auditOperationId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesAuditOperationWithXYZZYXAssociationExistsAsync(long auditOperationId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<AuditOperationXYZZYX>(x => x.AuditOperationId == auditOperationId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<AuditOperationView>> GetAuditOperationsByXYZZYXAsync(long xYZZYXId)
        {
            var auditOperationIds = GetAuditOperationXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.AuditOperationId);
            var auditOperationSql = GetAuditOperationsViewAsIQueryable().Where(x => auditOperationIds.Contains(x.AuditOperationId));
            return auditOperationSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByAuditOperationsAsync(long auditOperationId)
        {
            var xYZZYXIds = GetAuditOperationXYZZYXsViewAsIQueryable().Where(x => x.AuditOperationId == auditOperation).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<AuditOperationXYZZYX> GetAuditOperationXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<AuditOperationXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetAuditOperationXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.AuditOperationXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region AuditOperationDetail

        public IQueryable<AuditOperationDetailView> GetAuditOperationDetailsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<AuditOperationDetail>()
                   join a in appDBManager.GetTable<AuditOperation>() on item.AuditOperationId equals a.AuditOperationId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new AuditOperationDetailView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       AuditOperationDetailId = item.AuditOperationDetailId,
                       AuditOperationId = item.AuditOperationId,
                       DifferenceJson = item.DifferenceJson,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<AuditOperationDetailView> GetAuditOperationDetailsInternalWithSearchCriteriaAsync(AuditOperationDetailSearchCriteria search)
        {
            var sql1 = GetAuditOperationDetailsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x => x.AuditOperationDetailId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper());
            }

            if (search.AuditOperationId != null && search.AuditOperationId > 0)
                sql1 = sql1.Where(x => search.AuditOperationId == x.AuditOperationId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<AuditOperationDetailView>> GetAuditOperationDetailsInternalAsync(AuditOperationDetailSearchCriteria search)
        {
            var sql1 = GetAuditOperationDetailsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.AuditOperationDetailId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetAuditOperationDetailsViewAsIQueryable().FirstOrDefaultAsync(x => x.AuditOperationDetailId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<AuditOperationDetailView>> GetAuditOperationDetailsAsync(AuditOperationDetailSearchCriteria search)
        {
            return GetAuditOperationDetailsInternalAsync(search);
        }

        public Task<AuditOperationDetail> GetAuditOperationDetailFromAnyByIdAsync(long auditOperationDetailId)
        {
            return appDBManager.FirstOrDefaultAsync<AuditOperationDetail>(x => x.AuditOperationDetailId == auditOperationDetailId);
        }

        public Task<AuditOperationDetailView> GetAuditOperationDetailViewFromAnyByIdAsync(long auditOperationDetailId)
        {
            return GetAuditOperationDetailsViewAsIQueryable().FirstOrDefaultAsync(x => x.AuditOperationDetailId == auditOperationDetailId);
        }

        public Task<AuditOperationDetail> GetAuditOperationDetailByUniqueIdentifierAsync(string auditOperationDetailId)
        {
            return appDBManager.FirstOrDefaultAsync<AuditOperationDetail>(x => x.AuditOperationDetailId == SafeUtils.Int(auditOperationDetailId) && x.IsDeleted == false);
        }

        public Task<AuditOperationDetail> GetAuditOperationDetailByIdAsync(long auditOperationDetailId)
        {
            return appDBManager.FirstOrDefaultAsync<AuditOperationDetail>(x => x.AuditOperationDetailId == auditOperationDetailId && x.IsDeleted == false);
        }

        public Task<AuditOperationDetailView> GetAuditOperationDetailViewByIdAsync(long auditOperationDetailId)
        {
            var sql1 = GetAuditOperationDetailsViewAsIQueryable();

            sql1 = sql1.Where(x => x.AuditOperationDetailId == auditOperationDetailId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<AuditOperationDetailView> GetAuditOperationDetailViewByAuditOperationIdAsync(long auditOperationId)
        {
            return GetAuditOperationDetailsViewAsIQueryable().FirstOrDefaultAsync(x => x.AuditOperationId == auditOperationId && x.IsDeleted == false);
        }

        public Task<List<AuditOperationDetail>> GetAllAuditOperationDetails()
        {
            return appDBManager.SelectAllAsync<AuditOperationDetail>();
        }

        public async Task<AuditOperationDetail> CreateAuditOperationDetailAsync(AuditOperationDetail auditOperationDetail, long? createdBy)
        {
            auditOperationDetail.AuditOperationDetailId = await appDBManager.InsertWithAuditAsync<AuditOperationDetail, long>(auditOperationDetail, createdBy);
            return auditOperationDetail;
        }

        public async Task<List<AuditOperationDetail>> CreateAuditOperationDetailsAsync(List<AuditOperationDetail> auditOperationDetails, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<AuditOperationDetail, long>(auditOperationDetails, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                auditOperationDetails[i].AuditOperationDetailId = lst[i];
            return auditOperationDetails;
        }

        public Task<bool> UpdateAuditOperationDetailAsync(AuditOperationDetail auditOperationDetail, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<AuditOperationDetail>(auditOperationDetail, modifiedBy);
        }

        public async Task<bool> MarkAuditOperationDetailAsNotDeletedByIdAsync(long auditOperationDetailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<AuditOperationDetail>().Where(x => x.AuditOperationDetailId == auditOperationDetailId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkAuditOperationDetailAsDeletedByIdAsync(long auditOperationDetailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<AuditOperationDetail>().Where(x => x.AuditOperationDetailId == auditOperationDetailId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteAuditOperationDetailByIdAsync(long auditOperationDetailId)
        {
            return await appDBManager.GetTable<AuditOperationDetail>().Where(x => x.AuditOperationDetailId == auditOperationDetailId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetAuditOperationDetailActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.AuditOperationDetail);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        /*

        public async Task<bool> EnableAuditOperationDetailByIdAsync(long auditOperationDetailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<AuditOperationDetail>().Where(x => x.AuditOperationDetailId == auditOperationDetailId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableAuditOperationDetailByIdAsync(long auditOperationDetailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<AuditOperationDetail>().Where(x => x.AuditOperationDetailId == auditOperationDetailId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateAuditOperationDetailWithXYZZYXAsync(long auditOperationDetailId, long xYZZYXId, long? createdBy)
        {
            var item = new AuditOperationDetailXYZZYXAssociationModel();
            item.AuditOperationDetailId = auditOperationDetailId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<AuditOperationDetailXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateAuditOperationDetailWithXYZZYXAsync(long auditOperationDetailId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<AuditOperationDetailXYZZYX>(x => x.AuditOperationDetailId == auditOperationDetailId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesAuditOperationDetailWithXYZZYXAssociationExistsAsync(long auditOperationDetailId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<AuditOperationDetailXYZZYX>(x => x.AuditOperationDetailId == auditOperationDetailId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<AuditOperationDetailView>> GetAuditOperationDetailsByXYZZYXAsync(long xYZZYXId)
        {
            var auditOperationDetailIds = GetAuditOperationDetailXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.AuditOperationDetailId);
            var auditOperationDetailSql = GetAuditOperationDetailsViewAsIQueryable().Where(x => auditOperationDetailIds.Contains(x.AuditOperationDetailId));
            return auditOperationDetailSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByAuditOperationDetailsAsync(long auditOperationDetailId)
        {
            var xYZZYXIds = GetAuditOperationDetailXYZZYXsViewAsIQueryable().Where(x => x.AuditOperationDetailId == auditOperationDetail).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<AuditOperationDetailXYZZYX> GetAuditOperationDetailXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<AuditOperationDetailXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetAuditOperationDetailXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.AuditOperationDetailXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region ConfigurationGroup

        public IQueryable<ConfigurationGroupView> GetConfigurationGroupsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ConfigurationGroup>()
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new ConfigurationGroupView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       ConfigurationGroupId = item.ConfigurationGroupId,
                       GroupName = item.GroupName,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<ConfigurationGroupView> GetConfigurationGroupsInternalWithSearchCriteriaAsync(ConfigurationGroupSearchCriteria search)
        {
            var sql1 = GetConfigurationGroupsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.ConfigurationGroupId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.GroupName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<ConfigurationGroupView>> GetConfigurationGroupsInternalAsync(ConfigurationGroupSearchCriteria search)
        {
            var sql1 = GetConfigurationGroupsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.ConfigurationGroupId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetConfigurationGroupsViewAsIQueryable().FirstOrDefaultAsync(x => x.ConfigurationGroupId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<ConfigurationGroupView>> GetConfigurationGroupsAsync(ConfigurationGroupSearchCriteria search)
        {
            return GetConfigurationGroupsInternalAsync(search);
        }

        public Task<ConfigurationGroup> GetConfigurationGroupFromAnyByIdAsync(long configurationGroupId)
        {
            return appDBManager.FirstOrDefaultAsync<ConfigurationGroup>(x => x.ConfigurationGroupId == configurationGroupId);
        }

        public Task<ConfigurationGroupView> GetConfigurationGroupViewFromAnyByIdAsync(long configurationGroupId)
        {
            return GetConfigurationGroupsViewAsIQueryable().FirstOrDefaultAsync(x => x.ConfigurationGroupId == configurationGroupId);
        }

        public Task<ConfigurationGroup> GetConfigurationGroupByIdAsync(long configurationGroupId)
        {
            return appDBManager.FirstOrDefaultAsync<ConfigurationGroup>(x => x.ConfigurationGroupId == configurationGroupId && x.IsDeleted == false);
        }

        public Task<ConfigurationGroup> GetConfigurationGroupByUniqueIdentifierAsync(string configurationGroupId)
        {
            return appDBManager.FirstOrDefaultAsync<ConfigurationGroup>(x => x.ConfigurationGroupId == configurationGroupId.Int() && x.IsDeleted == false);
        }

        public Task<ConfigurationGroupView> GetConfigurationGroupViewByIdAsync(long configurationGroupId)
        {
            var sql1 = GetConfigurationGroupsViewAsIQueryable();

            sql1 = sql1.Where(x => x.ConfigurationGroupId == configurationGroupId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<ConfigurationGroup>> GetAllConfigurationGroups()
        {
            return appDBManager.SelectAllAsync<ConfigurationGroup>();
        }

        public async Task<ConfigurationGroup> CreateConfigurationGroupAsync(ConfigurationGroup configurationGroup, long? createdBy)
        {
            configurationGroup.ConfigurationGroupId = await appDBManager.InsertWithAuditAsync<ConfigurationGroup, long>(configurationGroup, createdBy);
            return configurationGroup;
        }

        public async Task<List<ConfigurationGroup>> CreateConfigurationGroupsAsync(List<ConfigurationGroup> configurationGroups, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<ConfigurationGroup, long>(configurationGroups, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                configurationGroups[i].ConfigurationGroupId = lst[i];
            return configurationGroups;
        }

        public Task<bool> UpdateConfigurationGroupAsync(ConfigurationGroup configurationGroup, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<ConfigurationGroup>(configurationGroup, modifiedBy);
        }

        public async Task<bool> MarkConfigurationGroupAsNotDeletedByIdAsync(long configurationGroupId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ConfigurationGroup>().Where(x => x.ConfigurationGroupId == configurationGroupId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkConfigurationGroupAsDeletedByIdAsync(long configurationGroupId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ConfigurationGroup>().Where(x => x.ConfigurationGroupId == configurationGroupId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteConfigurationGroupByIdAsync(long configurationGroupId)
        {
            return await appDBManager.GetTable<ConfigurationGroup>().Where(x => x.ConfigurationGroupId == configurationGroupId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetConfigurationGroupActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.ConfigurationGroup);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        /*
        public async Task<bool> EnableConfigurationGroupByIdAsync(long configurationGroupId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ConfigurationGroup>().Where(x => x.ConfigurationGroupId == configurationGroupId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableConfigurationGroupByIdAsync(long configurationGroupId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ConfigurationGroup>().Where(x => x.ConfigurationGroupId == configurationGroupId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateConfigurationGroupWithXYZZYXAsync(long configurationGroupId, long xYZZYXId, long? createdBy)
        {
            var item = new ConfigurationGroupXYZZYXAssociationModel();
            item.ConfigurationGroupId = configurationGroupId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<ConfigurationGroupXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateConfigurationGroupWithXYZZYXAsync(long configurationGroupId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<ConfigurationGroupXYZZYX>(x => x.ConfigurationGroupId == configurationGroupId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesConfigurationGroupWithXYZZYXAssociationExistsAsync(long configurationGroupId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<ConfigurationGroupXYZZYX>(x => x.ConfigurationGroupId == configurationGroupId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<ConfigurationGroupView>> GetConfigurationGroupsByXYZZYXAsync(long xYZZYXId)
        {
            var configurationGroupIds = GetConfigurationGroupXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.ConfigurationGroupId);
            var configurationGroupSql = GetConfigurationGroupsViewAsIQueryable().Where(x => configurationGroupIds.Contains(x.ConfigurationGroupId));
            return configurationGroupSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByConfigurationGroupsAsync(long configurationGroupId)
        {
            var xYZZYXIds = GetConfigurationGroupXYZZYXsViewAsIQueryable().Where(x => x.ConfigurationGroupId == configurationGroup).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<ConfigurationGroupXYZZYX> GetConfigurationGroupXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ConfigurationGroupXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetConfigurationGroupXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.ConfigurationGroupXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region ConfigurationItem

        public IQueryable<ConfigurationItemView> GetConfigurationItemsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ConfigurationItem>()
                   join a in appDBManager.GetTable<ConfigurationGroup>() on item.ConfigurationGroupId equals a.ConfigurationGroupId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new ConfigurationItemView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       ConfigurationItemId = item.ConfigurationItemId,
                       ConfigurationGroupId = item.ConfigurationGroupId,
                       Key = item.Key,
                       Value = item.Value,
                       DefaultValue = item.DefaultValue,
                       Required = item.Required,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<ConfigurationItemView> GetConfigurationItemsInternalWithSearchCriteriaAsync(ConfigurationItemSearchCriteria search)
        {
            var sql1 = GetConfigurationItemsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.ConfigurationItemId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Key.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Value.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.DefaultValue.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.ConfigurationGroupId != null && search.ConfigurationGroupId > 0)
                sql1 = sql1.Where(x => search.ConfigurationGroupId == x.ConfigurationGroupId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<ConfigurationItemView>> GetConfigurationItemsInternalAsync(ConfigurationItemSearchCriteria search)
        {
            var sql1 = GetConfigurationItemsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.ConfigurationItemId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetConfigurationItemsViewAsIQueryable().FirstOrDefaultAsync(x => x.ConfigurationItemId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<ConfigurationItemView>> GetConfigurationItemsAsync(ConfigurationItemSearchCriteria search)
        {
            return GetConfigurationItemsInternalAsync(search);
        }

        public Task<ConfigurationItem> GetConfigurationItemFromAnyByIdAsync(long configurationItemId)
        {
            return appDBManager.FirstOrDefaultAsync<ConfigurationItem>(x => x.ConfigurationItemId == configurationItemId);
        }

        public Task<ConfigurationItemView> GetConfigurationItemViewFromAnyByIdAsync(long configurationItemId)
        {
            return GetConfigurationItemsViewAsIQueryable().FirstOrDefaultAsync(x => x.ConfigurationItemId == configurationItemId);
        }

        public Task<ConfigurationItem> GetConfigurationItemByIdAsync(long configurationItemId)
        {
            return appDBManager.FirstOrDefaultAsync<ConfigurationItem>(x => x.ConfigurationItemId == configurationItemId && x.IsDeleted == false);
        }

        public Task<ConfigurationItem> GetConfigurationItemByUniqueIdentifierAsync(string configurationItemId)
        {
            return appDBManager.FirstOrDefaultAsync<ConfigurationItem>(x => x.ConfigurationItemId == configurationItemId.Int() && x.IsDeleted == false);
        }

        public Task<ConfigurationItemView> GetConfigurationItemViewByIdAsync(long configurationItemId)
        {
            var sql1 = GetConfigurationItemsViewAsIQueryable();

            sql1 = sql1.Where(x => x.ConfigurationItemId == configurationItemId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<ConfigurationItem>> GetAllConfigurationItems()
        {
            return appDBManager.SelectAllAsync<ConfigurationItem>();
        }

        public async Task<ConfigurationItem> CreateConfigurationItemAsync(ConfigurationItem configurationItem, long? createdBy)
        {
            configurationItem.ConfigurationItemId = await appDBManager.InsertWithAuditAsync<ConfigurationItem, long>(configurationItem, createdBy);
            return configurationItem;
        }

        public async Task<List<ConfigurationItem>> CreateConfigurationItemsAsync(List<ConfigurationItem> configurationItems, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<ConfigurationItem, long>(configurationItems, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                configurationItems[i].ConfigurationItemId = lst[i];
            return configurationItems;
        }

        public Task<bool> UpdateConfigurationItemAsync(ConfigurationItem configurationItem, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<ConfigurationItem>(configurationItem, modifiedBy);
        }

        public async Task<bool> MarkConfigurationItemAsNotDeletedByIdAsync(long configurationItemId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ConfigurationItem>().Where(x => x.ConfigurationItemId == configurationItemId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkConfigurationItemAsDeletedByIdAsync(long configurationItemId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ConfigurationItem>().Where(x => x.ConfigurationItemId == configurationItemId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteConfigurationItemByIdAsync(long configurationItemId)
        {
            return await appDBManager.GetTable<ConfigurationItem>().Where(x => x.ConfigurationItemId == configurationItemId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetConfigurationItemActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.ConfigurationItem);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        /*

        public async Task<bool> EnableConfigurationItemByIdAsync(long configurationItemId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ConfigurationItem>().Where(x => x.ConfigurationItemId == configurationItemId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableConfigurationItemByIdAsync(long configurationItemId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ConfigurationItem>().Where(x => x.ConfigurationItemId == configurationItemId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateConfigurationItemWithXYZZYXAsync(long configurationItemId, long xYZZYXId, long? createdBy)
        {
            var item = new ConfigurationItemXYZZYXAssociationModel();
            item.ConfigurationItemId = configurationItemId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<ConfigurationItemXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateConfigurationItemWithXYZZYXAsync(long configurationItemId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<ConfigurationItemXYZZYX>(x => x.ConfigurationItemId == configurationItemId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesConfigurationItemWithXYZZYXAssociationExistsAsync(long configurationItemId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<ConfigurationItemXYZZYX>(x => x.ConfigurationItemId == configurationItemId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<ConfigurationItemView>> GetConfigurationItemsByXYZZYXAsync(long xYZZYXId)
        {
            var configurationItemIds = GetConfigurationItemXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.ConfigurationItemId);
            var configurationItemSql = GetConfigurationItemsViewAsIQueryable().Where(x => configurationItemIds.Contains(x.ConfigurationItemId));
            return configurationItemSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByConfigurationItemsAsync(long configurationItemId)
        {
            var xYZZYXIds = GetConfigurationItemXYZZYXsViewAsIQueryable().Where(x => x.ConfigurationItemId == configurationItem).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<ConfigurationItemXYZZYX> GetConfigurationItemXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ConfigurationItemXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetConfigurationItemXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.ConfigurationItemXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region EnumGroup

        public IQueryable<EnumGroupView> GetEnumGroupsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<EnumGroup>()
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new EnumGroupView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       EnumGroupId = item.EnumGroupId,
                       EnumGroupName = item.EnumGroupName,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<EnumGroupView> GetEnumGroupsInternalWithSearchCriteriaAsync(EnumGroupSearchCriteria search)
        {
            var sql1 = GetEnumGroupsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.EnumGroupId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.EnumGroupName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<EnumGroupView>> GetEnumGroupsInternalAsync(EnumGroupSearchCriteria search)
        {
            var sql1 = GetEnumGroupsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.EnumGroupId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetEnumGroupsViewAsIQueryable().FirstOrDefaultAsync(x => x.EnumGroupId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<EnumGroupView>> GetEnumGroupsAsync(EnumGroupSearchCriteria search)
        {
            return GetEnumGroupsInternalAsync(search);
        }

        public Task<EnumGroup> GetEnumGroupFromAnyByIdAsync(long enumGroupId)
        {
            return appDBManager.FirstOrDefaultAsync<EnumGroup>(x => x.EnumGroupId == enumGroupId);
        }

        public Task<EnumGroupView> GetEnumGroupViewFromAnyByIdAsync(long enumGroupId)
        {
            return GetEnumGroupsViewAsIQueryable().FirstOrDefaultAsync(x => x.EnumGroupId == enumGroupId);
        }

        public Task<EnumGroup> GetEnumGroupByIdAsync(long enumGroupId)
        {
            return appDBManager.FirstOrDefaultAsync<EnumGroup>(x => x.EnumGroupId == enumGroupId && x.IsDeleted == false);
        }

        public Task<EnumGroup> GetEnumGroupByUniqueIdentifierAsync(string enumGroupId)
        {
            return appDBManager.FirstOrDefaultAsync<EnumGroup>(x => x.EnumGroupId == enumGroupId.Int() && x.IsDeleted == false);
        }

        public Task<EnumGroupView> GetEnumGroupViewByIdAsync(long enumGroupId)
        {
            var sql1 = GetEnumGroupsViewAsIQueryable();

            sql1 = sql1.Where(x => x.EnumGroupId == enumGroupId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<EnumGroup>> GetAllEnumGroups()
        {
            return appDBManager.SelectAllAsync<EnumGroup>();
        }

        public async Task<EnumGroup> CreateEnumGroupAsync(EnumGroup enumGroup, long? createdBy)
        {
            enumGroup.EnumGroupId = await appDBManager.InsertWithAuditAsync<EnumGroup, long>(enumGroup, createdBy);
            return enumGroup;
        }

        public async Task<List<EnumGroup>> CreateEnumGroupsAsync(List<EnumGroup> enumGroups, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<EnumGroup, long>(enumGroups, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                enumGroups[i].EnumGroupId = lst[i];
            return enumGroups;
        }

        public Task<bool> UpdateEnumGroupAsync(EnumGroup enumGroup, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<EnumGroup>(enumGroup, modifiedBy);
        }

        public async Task<bool> MarkEnumGroupAsNotDeletedByIdAsync(long enumGroupId, long? modifiedBy)
        {
            return await appDBManager.GetTable<EnumGroup>().Where(x => x.EnumGroupId == enumGroupId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkEnumGroupAsDeletedByIdAsync(long enumGroupId, long? modifiedBy)
        {
            return await appDBManager.GetTable<EnumGroup>().Where(x => x.EnumGroupId == enumGroupId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteEnumGroupByIdAsync(long enumGroupId)
        {
            return await appDBManager.GetTable<EnumGroup>().Where(x => x.EnumGroupId == enumGroupId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetEnumGroupActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.EnumGroup);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        /*

        public async Task<bool> EnableEnumGroupByIdAsync(long enumGroupId, long? modifiedBy)
        {
            return await appDBManager.GetTable<EnumGroup>().Where(x => x.EnumGroupId == enumGroupId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableEnumGroupByIdAsync(long enumGroupId, long? modifiedBy)
        {
            return await appDBManager.GetTable<EnumGroup>().Where(x => x.EnumGroupId == enumGroupId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateEnumGroupWithXYZZYXAsync(long enumGroupId, long xYZZYXId, long? createdBy)
        {
            var item = new EnumGroupXYZZYXAssociationModel();
            item.EnumGroupId = enumGroupId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<EnumGroupXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateEnumGroupWithXYZZYXAsync(long enumGroupId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<EnumGroupXYZZYX>(x => x.EnumGroupId == enumGroupId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesEnumGroupWithXYZZYXAssociationExistsAsync(long enumGroupId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<EnumGroupXYZZYX>(x => x.EnumGroupId == enumGroupId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<EnumGroupView>> GetEnumGroupsByXYZZYXAsync(long xYZZYXId)
        {
            var enumGroupIds = GetEnumGroupXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.EnumGroupId);
            var enumGroupSql = GetEnumGroupsViewAsIQueryable().Where(x => enumGroupIds.Contains(x.EnumGroupId));
            return enumGroupSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByEnumGroupsAsync(long enumGroupId)
        {
            var xYZZYXIds = GetEnumGroupXYZZYXsViewAsIQueryable().Where(x => x.EnumGroupId == enumGroup).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<EnumGroupXYZZYX> GetEnumGroupXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<EnumGroupXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetEnumGroupXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.EnumGroupXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region EnumType

        public IQueryable<EnumTypeView> GetEnumTypesViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<EnumType>()
                   join a in appDBManager.GetTable<EnumGroup>() on item.EnumGroupId equals a.EnumGroupId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new EnumTypeView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       EnumGroupName = a.EnumGroupName,

                       EnumTypeId = item.EnumTypeId,
                       InternalValue = item.InternalValue,
                       EnumGroupId = item.EnumGroupId,
                       EnumTypeName = item.EnumTypeName,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<EnumTypeView> GetEnumTypesInternalWithSearchCriteriaAsync(EnumTypeSearchCriteria search)
        {
            var sql1 = GetEnumTypesViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.EnumTypeId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.EnumTypeName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.EnumGroupId != null && search.EnumGroupId > 0)
                sql1 = sql1.Where(x => search.EnumGroupId == x.EnumGroupId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<EnumTypeView>> GetEnumTypesInternalAsync(EnumTypeSearchCriteria search)
        {
            var sql1 = GetEnumTypesInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.EnumTypeId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetEnumTypesViewAsIQueryable().FirstOrDefaultAsync(x => x.EnumTypeId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<EnumTypeView>> GetEnumTypesAsync(EnumTypeSearchCriteria search)
        {
            return GetEnumTypesInternalAsync(search);
        }

        public Task<EnumType> GetEnumTypeFromAnyByIdAsync(long enumTypeId)
        {
            return appDBManager.FirstOrDefaultAsync<EnumType>(x => x.EnumTypeId == enumTypeId);
        }

        public Task<EnumTypeView> GetEnumTypeViewFromAnyByIdAsync(long enumTypeId)
        {
            return GetEnumTypesViewAsIQueryable().FirstOrDefaultAsync(x => x.EnumTypeId == enumTypeId);
        }

        public Task<EnumType> GetEnumTypeByIdAsync(long enumTypeId)
        {
            return appDBManager.FirstOrDefaultAsync<EnumType>(x => x.EnumTypeId == enumTypeId && x.IsDeleted == false);
        }

        public Task<EnumType> GetEnumTypeByUniqueIdentifierAsync(string enumTypeId)
        {
            return appDBManager.FirstOrDefaultAsync<EnumType>(x => x.EnumTypeId == enumTypeId.Int() && x.IsDeleted == false);
        }

        public Task<EnumTypeView> GetEnumTypeViewByIdAsync(long enumTypeId)
        {
            var sql1 = GetEnumTypesViewAsIQueryable();

            sql1 = sql1.Where(x => x.EnumTypeId == enumTypeId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<EnumType>> GetAllEnumTypes()
        {
            return appDBManager.SelectAllAsync<EnumType>();
        }

        public async Task<EnumType> CreateEnumTypeAsync(EnumType enumType, long? createdBy)
        {
            enumType.EnumTypeId = await appDBManager.InsertWithAuditAsync<EnumType, long>(enumType, createdBy);
            return enumType;
        }

        public async Task<List<EnumType>> CreateEnumTypesAsync(List<EnumType> enumTypes, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<EnumType, long>(enumTypes, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                enumTypes[i].EnumTypeId = lst[i];
            return enumTypes;
        }

        public Task<bool> UpdateEnumTypeAsync(EnumType enumType, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<EnumType>(enumType, modifiedBy);
        }

        public async Task<bool> MarkEnumTypeAsNotDeletedByIdAsync(long enumTypeId, long? modifiedBy)
        {
            return await appDBManager.GetTable<EnumType>().Where(x => x.EnumTypeId == enumTypeId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkEnumTypeAsDeletedByIdAsync(long enumTypeId, long? modifiedBy)
        {
            return await appDBManager.GetTable<EnumType>().Where(x => x.EnumTypeId == enumTypeId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteEnumTypeByIdAsync(long enumTypeId)
        {
            return await appDBManager.GetTable<EnumType>().Where(x => x.EnumTypeId == enumTypeId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetEnumTypeActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.EnumType);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsEnumTypeNameAlreadyExistsForOtherEnumTypeAsync(long enumTypeId, string enumTypeName)
        {
            var sql = GetEnumTypesViewAsIQueryable().Where(x => x.IsDeleted == false && x.EnumTypeId != enumTypeId && x.EnumTypeName.ToLower().Trim() == enumTypeName.ToLower().Trim());
            return sql.AnyAsync();
        }

        public Task<bool> IsEnumTypeNameAlreadyExistsForOtherEnumTypeAsync(string enumTypeName)
        {
            var sql = GetEnumTypesViewAsIQueryable().Where(x => x.IsDeleted == false && x.EnumTypeName.ToLower().Trim() == enumTypeName.ToLower().Trim());
            return sql.AnyAsync();
        }

        /*

        public async Task<bool> EnableEnumTypeByIdAsync(long enumTypeId, long? modifiedBy)
        {
            return await appDBManager.GetTable<EnumType>().Where(x => x.EnumTypeId == enumTypeId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableEnumTypeByIdAsync(long enumTypeId, long? modifiedBy)
        {
            return await appDBManager.GetTable<EnumType>().Where(x => x.EnumTypeId == enumTypeId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateEnumTypeWithXYZZYXAsync(long enumTypeId, long xYZZYXId, long? createdBy)
        {
            var item = new EnumTypeXYZZYXAssociationModel();
            item.EnumTypeId = enumTypeId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<EnumTypeXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateEnumTypeWithXYZZYXAsync(long enumTypeId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<EnumTypeXYZZYX>(x => x.EnumTypeId == enumTypeId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesEnumTypeWithXYZZYXAssociationExistsAsync(long enumTypeId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<EnumTypeXYZZYX>(x => x.EnumTypeId == enumTypeId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<EnumTypeView>> GetEnumTypesByXYZZYXAsync(long xYZZYXId)
        {
            var enumTypeIds = GetEnumTypeXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.EnumTypeId);
            var enumTypeSql = GetEnumTypesViewAsIQueryable().Where(x => enumTypeIds.Contains(x.EnumTypeId));
            return enumTypeSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByEnumTypesAsync(long enumTypeId)
        {
            var xYZZYXIds = GetEnumTypeXYZZYXsViewAsIQueryable().Where(x => x.EnumTypeId == enumType).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<EnumTypeXYZZYX> GetEnumTypeXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<EnumTypeXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetEnumTypeXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.EnumTypeXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region Job

        public IQueryable<JobView> GetJobsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<Job>()
                   join a in appDBManager.GetTable<EnumType>() on item.PredefinedJobTypeId equals a.EnumTypeId
                   join b in appDBManager.GetTable<JobTemplate>() on item.JobTemplateId equals b.JobTemplateId
                   join od in appDBManager.GetTable<EnumType>() on item.PredefinedJobOrReportTypeId equals od.EnumTypeId into dTemp
                   from d in dTemp.DefaultIfEmpty()
                   join e in appDBManager.GetTable<EnumType>() on item.PredefinedJobStatusTypeId equals e.EnumTypeId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new JobView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       PredefinedJobTypeName = a.EnumTypeName,
                       PredefinedJobOrReportTypeName = d.EnumTypeName,
                       PredefinedJobStatusTypeName = e.EnumTypeName,

                       JobId = item.JobId,
                       PredefinedJobTypeId = item.PredefinedJobTypeId,
                       JobTemplateId = item.JobTemplateId,
                       PredefinedJobOrReportTypeId = item.PredefinedJobOrReportTypeId,
                       CompletedDate = item.CompletedDate,
                       SubmittedDate = item.SubmittedDate,
                       Message = item.Message,
                       PredefinedJobStatusTypeId = item.PredefinedJobStatusTypeId,
                       SubmittedByUserId = item.SubmittedByUserId,
                       ExecutableFolder = item.ExecutableFolder,
                       ExecutableName = item.ExecutableName,
                       ExecutableParameter = item.ExecutableParameter,
                       SerializedJobParameter = item.SerializedJobParameter,
                       ReportTemplateFile = item.ReportTemplateFile,
                       ExportDoc = item.ExportDoc,
                       ExportXls = item.ExportXls,
                       ExportPpt = item.ExportPpt,
                       ExportCsv = item.ExportCsv,
                       ExportXml = item.ExportXml,
                       ExportPdf = item.ExportPdf,
                       ExportCustom = item.ExportCustom,
                       CustomExportName = item.CustomExportName,
                       PickupDate = item.PickupDate,
                       JobName = item.JobName,
                       IncludeDataSourceFile = item.IncludeDataSourceFile,
                       ExecutionAppName = item.ExecutionAppName,
                       ExecutionAppId = item.ExecutionAppId,
                       ExecutionHostName = item.ExecutionHostName,
                       ExecutionAppCheckPort = item.ExecutionAppCheckPort,
                       ExecutionProcessId = item.ExecutionProcessId,
                       ExecutionThreadId = item.ExecutionThreadId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<JobView> GetJobsInternalWithSearchCriteriaAsync(JobSearchCriteria search)
        {
            var sql1 = GetJobsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.JobId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Message.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutableFolder.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutableName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutableParameter.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.SerializedJobParameter.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ReportTemplateFile.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.CustomExportName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.JobName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutionAppName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutionAppId.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutionHostName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.PredefinedJobTypeIds != null && search.PredefinedJobTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedJobTypeIds.Contains(x.PredefinedJobTypeId ?? 0));
            if (search.JobTemplateId != null && search.JobTemplateId > 0)
                sql1 = sql1.Where(x => search.JobTemplateId == x.JobTemplateId);
            if (search.PredefinedJobOrReportTypeIds != null && search.PredefinedJobOrReportTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedJobOrReportTypeIds.Contains(x.PredefinedJobOrReportTypeId ?? 0));
            if (search.PredefinedJobStatusTypeIds != null && search.PredefinedJobStatusTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedJobStatusTypeIds.Contains(x.PredefinedJobStatusTypeId));
            if (search.SubmittedByUserId != null && search.SubmittedByUserId > 0)
                sql1 = sql1.Where(x => search.SubmittedByUserId == x.SubmittedByUserId);
            if (search.ExecutionProcessId != null && search.ExecutionProcessId > 0)
                sql1 = sql1.Where(x => search.ExecutionProcessId == x.ExecutionProcessId);
            if (search.ExecutionThreadId != null && search.ExecutionThreadId > 0)
                sql1 = sql1.Where(x => search.ExecutionThreadId == x.ExecutionThreadId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<JobView>> GetJobsInternalAsync(JobSearchCriteria search)
        {
            var sql1 = GetJobsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.JobId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetJobsViewAsIQueryable().FirstOrDefaultAsync(x => x.JobId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<JobView>> GetJobsAsync(JobSearchCriteria search)
        {
            return GetJobsInternalAsync(search);
        }

        public Task<Job> GetJobFromAnyByIdAsync(long jobId)
        {
            return appDBManager.FirstOrDefaultAsync<Job>(x => x.JobId == jobId);
        }

        public Task<JobView> GetJobViewFromAnyByIdAsync(long jobId)
        {
            return GetJobsViewAsIQueryable().FirstOrDefaultAsync(x => x.JobId == jobId);
        }

        public Task<Job> GetJobByIdAsync(long jobId)
        {
            return appDBManager.FirstOrDefaultAsync<Job>(x => x.JobId == jobId && x.IsDeleted == false);
        }

        public Task<Job> GetJobByUniqueIdentifierAsync(string jobId)
        {
            return appDBManager.FirstOrDefaultAsync<Job>(x => x.JobId == jobId.Int() && x.IsDeleted == false);
        }

        public Task<JobView> GetJobViewByIdAsync(long jobId)
        {
            var sql1 = GetJobsViewAsIQueryable();

            sql1 = sql1.Where(x => x.JobId == jobId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<Job>> GetAllJobs()
        {
            return appDBManager.SelectAllAsync<Job>();
        }

        public Task<long> GetJobCountAsync()
        {
            return appDBManager.CountAsync<Job>(x => x.IsDeleted == false);
        }

        public Task<long> GetJobSuccessCountAsync()
        {
            return appDBManager.CountAsync<Job>(x => x.PredefinedJobStatusTypeId == (long)PredefinedJobStatusTypes.Completed && x.IsDeleted == false);
        }

        public Task<long> GetJobErrorCountAsync()
        {
            return appDBManager.CountAsync<Job>(x => x.PredefinedJobStatusTypeId == (long)PredefinedJobStatusTypes.Error && x.IsDeleted == false);
        }

        public Task<long> GetRunningJobCountAsync()
        {
            return appDBManager.CountAsync<Job>(x => x.PredefinedJobStatusTypeId == (long)PredefinedJobStatusTypes.Running && x.IsDeleted == false);
        }

        public async Task<Job> CreateJobAsync(Job job, long? createdBy)
        {
            job.JobId = await appDBManager.InsertWithAuditAsync<Job, long>(job, createdBy);
            return job;
        }

        public async Task<List<Job>> CreateJobsAsync(List<Job> jobs, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<Job, long>(jobs, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                jobs[i].JobId = lst[i];
            return jobs;
        }

        public Task<bool> UpdateJobAsync(Job job, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<Job>(job, modifiedBy);
        }

        public async Task<bool> MarkJobAsNotDeletedByIdAsync(long jobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Job>().Where(x => x.JobId == jobId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkJobAsDeletedByIdAsync(long jobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Job>().Where(x => x.JobId == jobId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteJobByIdAsync(long jobId)
        {
            return await appDBManager.GetTable<Job>().Where(x => x.JobId == jobId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.Job);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        /*

        public async Task<bool> EnableJobByIdAsync(long jobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Job>().Where(x => x.JobId == jobId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableJobByIdAsync(long jobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Job>().Where(x => x.JobId == jobId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateJobWithXYZZYXAsync(long jobId, long xYZZYXId, long? createdBy)
        {
            var item = new JobXYZZYXAssociationModel();
            item.JobId = jobId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<JobXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateJobWithXYZZYXAsync(long jobId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<JobXYZZYX>(x => x.JobId == jobId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesJobWithXYZZYXAssociationExistsAsync(long jobId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<JobXYZZYX>(x => x.JobId == jobId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<JobView>> GetJobsByXYZZYXAsync(long xYZZYXId)
        {
            var jobIds = GetJobXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.JobId);
            var jobSql = GetJobsViewAsIQueryable().Where(x => jobIds.Contains(x.JobId));
            return jobSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByJobsAsync(long jobId)
        {
            var xYZZYXIds = GetJobXYZZYXsViewAsIQueryable().Where(x => x.JobId == job).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<JobXYZZYX> GetJobXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<JobXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.JobXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region JobAttachment

        public IQueryable<JobAttachmentView> GetJobAttachmentsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<JobAttachment>()
                   join a in appDBManager.GetTable<Job>() on item.JobId equals a.JobId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new JobAttachmentView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       JobName = a.JobName,

                       JobAttachmentId = item.JobAttachmentId,
                       JobId = item.JobId,
                       FileName = item.FileName,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                       StoredFilePath = item.StoredFilePath,
                   };
        }

        public IQueryable<JobAttachmentView> GetJobAttachmentsInternalWithSearchCriteriaAsync(JobAttachmentSearchCriteria search)
        {
            var sql1 = GetJobAttachmentsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.JobAttachmentId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.FileName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.StoredFilePath.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.JobId != null && search.JobId > 0)
                sql1 = sql1.Where(x => search.JobId == x.JobId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<JobAttachmentView>> GetJobAttachmentsInternalAsync(JobAttachmentSearchCriteria search)
        {
            var sql1 = GetJobAttachmentsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.JobAttachmentId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetJobAttachmentsViewAsIQueryable().FirstOrDefaultAsync(x => x.JobAttachmentId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<JobAttachmentView>> GetJobAttachmentsAsync(JobAttachmentSearchCriteria search)
        {
            return GetJobAttachmentsInternalAsync(search);
        }

        public Task<JobAttachment> GetJobAttachmentFromAnyByIdAsync(long jobAttachmentId)
        {
            return appDBManager.FirstOrDefaultAsync<JobAttachment>(x => x.JobAttachmentId == jobAttachmentId);
        }

        public Task<JobAttachmentView> GetJobAttachmentViewFromAnyByIdAsync(long jobAttachmentId)
        {
            return GetJobAttachmentsViewAsIQueryable().FirstOrDefaultAsync(x => x.JobAttachmentId == jobAttachmentId);
        }

        public Task<JobAttachment> GetJobAttachmentByIdAsync(long jobAttachmentId)
        {
            return appDBManager.FirstOrDefaultAsync<JobAttachment>(x => x.JobAttachmentId == jobAttachmentId && x.IsDeleted == false);
        }

        public Task<JobAttachment> GetJobAttachmentByUniqueIdentifierAsync(string jobAttachmentId)
        {
            return appDBManager.FirstOrDefaultAsync<JobAttachment>(x => x.JobAttachmentId == jobAttachmentId.Int() && x.IsDeleted == false);
        }

        public Task<JobAttachmentView> GetJobAttachmentViewByIdAsync(long jobAttachmentId)
        {
            var sql1 = GetJobAttachmentsViewAsIQueryable();

            sql1 = sql1.Where(x => x.JobAttachmentId == jobAttachmentId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<JobAttachment>> GetAllJobAttachments()
        {
            return appDBManager.SelectAllAsync<JobAttachment>();
        }

        public async Task<JobAttachment> CreateJobAttachmentAsync(JobAttachment jobAttachment, long? createdBy)
        {
            jobAttachment.JobAttachmentId = await appDBManager.InsertWithAuditAsync<JobAttachment, long>(jobAttachment, createdBy);
            return jobAttachment;
        }

        public async Task<List<JobAttachment>> CreateJobAttachmentsAsync(List<JobAttachment> jobAttachments, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<JobAttachment, long>(jobAttachments, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                jobAttachments[i].JobAttachmentId = lst[i];
            return jobAttachments;
        }

        public Task<bool> UpdateJobAttachmentAsync(JobAttachment jobAttachment, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<JobAttachment>(jobAttachment, modifiedBy);
        }

        public async Task<bool> MarkJobAttachmentAsNotDeletedByIdAsync(long jobAttachmentId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobAttachment>().Where(x => x.JobAttachmentId == jobAttachmentId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkJobAttachmentAsDeletedByIdAsync(long jobAttachmentId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobAttachment>().Where(x => x.JobAttachmentId == jobAttachmentId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteJobAttachmentByIdAsync(long jobAttachmentId)
        {
            return await appDBManager.GetTable<JobAttachment>().Where(x => x.JobAttachmentId == jobAttachmentId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobAttachmentActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.JobAttachment);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        /*
        public async Task<bool> EnableJobAttachmentByIdAsync(long jobAttachmentId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobAttachment>().Where(x => x.JobAttachmentId == jobAttachmentId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableJobAttachmentByIdAsync(long jobAttachmentId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobAttachment>().Where(x => x.JobAttachmentId == jobAttachmentId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateJobAttachmentWithXYZZYXAsync(long jobAttachmentId, long xYZZYXId, long? createdBy)
        {
            var item = new JobAttachmentXYZZYXAssociationModel();
            item.JobAttachmentId = jobAttachmentId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<JobAttachmentXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateJobAttachmentWithXYZZYXAsync(long jobAttachmentId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<JobAttachmentXYZZYX>(x => x.JobAttachmentId == jobAttachmentId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesJobAttachmentWithXYZZYXAssociationExistsAsync(long jobAttachmentId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<JobAttachmentXYZZYX>(x => x.JobAttachmentId == jobAttachmentId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<JobAttachmentView>> GetJobAttachmentsByXYZZYXAsync(long xYZZYXId)
        {
            var jobAttachmentIds = GetJobAttachmentXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.JobAttachmentId);
            var jobAttachmentSql = GetJobAttachmentsViewAsIQueryable().Where(x => jobAttachmentIds.Contains(x.JobAttachmentId));
            return jobAttachmentSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByJobAttachmentsAsync(long jobAttachmentId)
        {
            var xYZZYXIds = GetJobAttachmentXYZZYXsViewAsIQueryable().Where(x => x.JobAttachmentId == jobAttachment).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<JobAttachmentXYZZYX> GetJobAttachmentXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<JobAttachmentXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobAttachmentXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.JobAttachmentXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region JobNotificationEmail

        public IQueryable<JobNotificationEmailView> GetJobNotificationEmailsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<JobNotificationEmail>()
                   join a in appDBManager.GetTable<Job>() on item.JobId equals a.JobId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new JobNotificationEmailView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       JobName = a.JobName,

                       JobNotificationEmailId = item.JobNotificationEmailId,
                       JobId = item.JobId,
                       Email = item.Email,
                       SendStatus = item.SendStatus,
                       Message = item.Message,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<JobNotificationEmailView> GetJobNotificationEmailsInternalWithSearchCriteriaAsync(JobNotificationEmailSearchCriteria search)
        {
            var sql1 = GetJobNotificationEmailsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.JobNotificationEmailId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Email.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Message.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.JobId != null && search.JobId > 0)
                sql1 = sql1.Where(x => search.JobId == x.JobId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<JobNotificationEmailView>> GetJobNotificationEmailsInternalAsync(JobNotificationEmailSearchCriteria search)
        {
            var sql1 = GetJobNotificationEmailsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.JobNotificationEmailId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetJobNotificationEmailsViewAsIQueryable().FirstOrDefaultAsync(x => x.JobNotificationEmailId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<JobNotificationEmailView>> GetJobNotificationEmailsAsync(JobNotificationEmailSearchCriteria search)
        {
            return GetJobNotificationEmailsInternalAsync(search);
        }

        public Task<JobNotificationEmail> GetJobNotificationEmailFromAnyByIdAsync(long jobNotificationEmailId)
        {
            return appDBManager.FirstOrDefaultAsync<JobNotificationEmail>(x => x.JobNotificationEmailId == jobNotificationEmailId);
        }

        public Task<JobNotificationEmailView> GetJobNotificationEmailViewFromAnyByIdAsync(long jobNotificationEmailId)
        {
            return GetJobNotificationEmailsViewAsIQueryable().FirstOrDefaultAsync(x => x.JobNotificationEmailId == jobNotificationEmailId);
        }

        public Task<JobNotificationEmail> GetJobNotificationEmailByIdAsync(long jobNotificationEmailId)
        {
            return appDBManager.FirstOrDefaultAsync<JobNotificationEmail>(x => x.JobNotificationEmailId == jobNotificationEmailId && x.IsDeleted == false);
        }

        public Task<JobNotificationEmail> GetJobNotificationEmailByUniqueIdentifierAsync(string jobNotificationEmailId)
        {
            return appDBManager.FirstOrDefaultAsync<JobNotificationEmail>(x => x.JobNotificationEmailId == jobNotificationEmailId.Int() && x.IsDeleted == false);
        }

        public Task<JobNotificationEmailView> GetJobNotificationEmailViewByIdAsync(long jobNotificationEmailId)
        {
            var sql1 = GetJobNotificationEmailsViewAsIQueryable();

            sql1 = sql1.Where(x => x.JobNotificationEmailId == jobNotificationEmailId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<JobNotificationEmail>> GetAllJobNotificationEmails()
        {
            return appDBManager.SelectAllAsync<JobNotificationEmail>();
        }

        public async Task<JobNotificationEmail> CreateJobNotificationEmailAsync(JobNotificationEmail jobNotificationEmail, long? createdBy)
        {
            jobNotificationEmail.JobNotificationEmailId = await appDBManager.InsertWithAuditAsync<JobNotificationEmail, long>(jobNotificationEmail, createdBy);
            return jobNotificationEmail;
        }

        public async Task<List<JobNotificationEmail>> CreateJobNotificationEmailsAsync(List<JobNotificationEmail> jobNotificationEmails, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<JobNotificationEmail, long>(jobNotificationEmails, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                jobNotificationEmails[i].JobNotificationEmailId = lst[i];
            return jobNotificationEmails;
        }

        public Task<bool> UpdateJobNotificationEmailAsync(JobNotificationEmail jobNotificationEmail, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<JobNotificationEmail>(jobNotificationEmail, modifiedBy);
        }

        public async Task<bool> MarkJobNotificationEmailAsNotDeletedByIdAsync(long jobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobNotificationEmail>().Where(x => x.JobNotificationEmailId == jobNotificationEmailId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkJobNotificationEmailAsDeletedByIdAsync(long jobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobNotificationEmail>().Where(x => x.JobNotificationEmailId == jobNotificationEmailId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteJobNotificationEmailByIdAsync(long jobNotificationEmailId)
        {
            return await appDBManager.GetTable<JobNotificationEmail>().Where(x => x.JobNotificationEmailId == jobNotificationEmailId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobNotificationEmailActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.JobNotificationEmail);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        /*
        public async Task<bool> EnableJobNotificationEmailByIdAsync(long jobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobNotificationEmail>().Where(x => x.JobNotificationEmailId == jobNotificationEmailId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableJobNotificationEmailByIdAsync(long jobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobNotificationEmail>().Where(x => x.JobNotificationEmailId == jobNotificationEmailId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateJobNotificationEmailWithXYZZYXAsync(long jobNotificationEmailId, long xYZZYXId, long? createdBy)
        {
            var item = new JobNotificationEmailXYZZYXAssociationModel();
            item.JobNotificationEmailId = jobNotificationEmailId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<JobNotificationEmailXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateJobNotificationEmailWithXYZZYXAsync(long jobNotificationEmailId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<JobNotificationEmailXYZZYX>(x => x.JobNotificationEmailId == jobNotificationEmailId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesJobNotificationEmailWithXYZZYXAssociationExistsAsync(long jobNotificationEmailId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<JobNotificationEmailXYZZYX>(x => x.JobNotificationEmailId == jobNotificationEmailId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<JobNotificationEmailView>> GetJobNotificationEmailsByXYZZYXAsync(long xYZZYXId)
        {
            var jobNotificationEmailIds = GetJobNotificationEmailXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.JobNotificationEmailId);
            var jobNotificationEmailSql = GetJobNotificationEmailsViewAsIQueryable().Where(x => jobNotificationEmailIds.Contains(x.JobNotificationEmailId));
            return jobNotificationEmailSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByJobNotificationEmailsAsync(long jobNotificationEmailId)
        {
            var xYZZYXIds = GetJobNotificationEmailXYZZYXsViewAsIQueryable().Where(x => x.JobNotificationEmailId == jobNotificationEmail).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<JobNotificationEmailXYZZYX> GetJobNotificationEmailXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<JobNotificationEmailXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobNotificationEmailXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.JobNotificationEmailXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region JobTemplate

        public IQueryable<JobTemplateView> GetJobTemplatesViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<JobTemplate>()
                   join a in appDBManager.GetTable<EnumType>() on item.PredefinedJobTypeId equals a.EnumTypeId
                   join ob in appDBManager.GetTable<EnumType>() on item.PredefinedJobOrReportTypeId equals ob.EnumTypeId into bTemp
                   from b in bTemp.DefaultIfEmpty()
                   join c in appDBManager.GetTable<JobTemplateCategory>() on item.JobTemplateCategoryId equals c.JobTemplateCategoryId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new JobTemplateView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       PredefinedJobTypeName = a.EnumTypeName,
                       PredefinedJobOrReportTypeName = b.EnumTypeName,
                       JobTemplateId = item.JobTemplateId,
                       PredefinedJobTypeId = item.PredefinedJobTypeId,
                       Description = item.Description,
                       PredefinedJobOrReportTypeId = item.PredefinedJobOrReportTypeId,
                       JobTemplateCategoryId = item.JobTemplateCategoryId,
                       ExecutableFolder = item.ExecutableFolder,
                       ExecutableName = item.ExecutableName,
                       ExecutableParameter = item.ExecutableParameter,
                       CanExportDoc = item.CanExportDoc,
                       CanExportXls = item.CanExportXls,
                       CanExportPpt = item.CanExportPpt,
                       CanExportCsv = item.CanExportCsv,
                       CanExportXml = item.CanExportXml,
                       CanExportPdf = item.CanExportPdf,
                       CanExportCustom = item.CanExportCustom,
                       CustomExportName = item.CustomExportName,
                       TemplateName = item.TemplateName,
                       CanExportDocDefaultValue = item.CanExportDocDefaultValue,
                       CanExportXlsDefaultValue = item.CanExportXlsDefaultValue,
                       CanExportPptDefaultValue = item.CanExportPptDefaultValue,
                       CanExportCsvDefaultValue = item.CanExportCsvDefaultValue,
                       CanExportXmlDefaultValue = item.CanExportXmlDefaultValue,
                       CanExportPdfDefaultValue = item.CanExportPdfDefaultValue,
                       CanExportCustomDefaultValue = item.CanExportCustomDefaultValue,
                       TaskIdentifierName = item.TaskIdentifierName,
                       ReportTemplateFile = item.ReportTemplateFile,
                       DefaultOutputFileName = item.DefaultOutputFileName,
                       IncludeDataSourceFile = item.IncludeDataSourceFile,
                       IncludeDataSourceFileDefaultValue = item.IncludeDataSourceFileDefaultValue,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<JobTemplateView> GetJobTemplatesInternalWithSearchCriteriaAsync(JobTemplateSearchCriteria search)
        {
            var sql1 = GetJobTemplatesViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.JobTemplateId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Description.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutableFolder.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutableName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutableParameter.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.CustomExportName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.TemplateName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.TaskIdentifierName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ReportTemplateFile.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.DefaultOutputFileName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.PredefinedJobTypeIds != null && search.PredefinedJobTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedJobTypeIds.Contains(x.PredefinedJobTypeId));
            if (search.PredefinedJobOrReportTypeIds != null && search.PredefinedJobOrReportTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedJobOrReportTypeIds.Contains(x.PredefinedJobOrReportTypeId ?? 0));
            if (search.JobTemplateCategoryId != null && search.JobTemplateCategoryId > 0)
                sql1 = sql1.Where(x => search.JobTemplateCategoryId == x.JobTemplateCategoryId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<JobTemplateView>> GetJobTemplatesInternalAsync(JobTemplateSearchCriteria search)
        {
            var sql1 = GetJobTemplatesInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.JobTemplateId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetJobTemplatesViewAsIQueryable().FirstOrDefaultAsync(x => x.JobTemplateId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<JobTemplateView>> GetJobTemplatesAsync(JobTemplateSearchCriteria search)
        {
            return GetJobTemplatesInternalAsync(search);
        }

        public Task<JobTemplate> GetJobTemplateFromAnyByIdAsync(long jobTemplateId)
        {
            return appDBManager.FirstOrDefaultAsync<JobTemplate>(x => x.JobTemplateId == jobTemplateId);
        }

        public Task<JobTemplateView> GetJobTemplateViewFromAnyByIdAsync(long jobTemplateId)
        {
            return GetJobTemplatesViewAsIQueryable().FirstOrDefaultAsync(x => x.JobTemplateId == jobTemplateId);
        }

        public Task<JobTemplate> GetJobTemplateByIdAsync(long jobTemplateId)
        {
            return appDBManager.FirstOrDefaultAsync<JobTemplate>(x => x.JobTemplateId == jobTemplateId && x.IsDeleted == false);
        }

        public Task<JobTemplate> GetJobTemplateByUniqueIdentifierAsync(string jobTemplateId)
        {
            return appDBManager.FirstOrDefaultAsync<JobTemplate>(x => x.JobTemplateId == jobTemplateId.Int() && x.IsDeleted == false);
        }

        public Task<JobTemplate> GetJobTemplateByPredefinedJobTypeIdAsync(long predefinedJobTypeId)
        {
            return appDBManager.FirstOrDefaultAsync<JobTemplate>(x => x.PredefinedJobTypeId == predefinedJobTypeId && x.IsDeleted == false);
        }

        public Task<JobTemplateView> GetJobTemplateViewByIdAsync(long jobTemplateId)
        {
            var sql1 = GetJobTemplatesViewAsIQueryable();

            sql1 = sql1.Where(x => x.JobTemplateId == jobTemplateId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<JobTemplate>> GetAllJobTemplates()
        {
            return appDBManager.SelectAllAsync<JobTemplate>();
        }

        public async Task<JobTemplate> CreateJobTemplateAsync(JobTemplate jobTemplate, long? createdBy)
        {
            jobTemplate.JobTemplateId = await appDBManager.InsertWithAuditAsync<JobTemplate, long>(jobTemplate, createdBy);
            return jobTemplate;
        }

        public async Task<List<JobTemplate>> CreateJobTemplatesAsync(List<JobTemplate> jobTemplates, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<JobTemplate, long>(jobTemplates, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                jobTemplates[i].JobTemplateId = lst[i];
            return jobTemplates;
        }

        public Task<bool> UpdateJobTemplateAsync(JobTemplate jobTemplate, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<JobTemplate>(jobTemplate, modifiedBy);
        }

        public async Task<bool> MarkJobTemplateAsNotDeletedByIdAsync(long jobTemplateId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplate>().Where(x => x.JobTemplateId == jobTemplateId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkJobTemplateAsDeletedByIdAsync(long jobTemplateId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplate>().Where(x => x.JobTemplateId == jobTemplateId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteJobTemplateByIdAsync(long jobTemplateId)
        {
            return await appDBManager.GetTable<JobTemplate>().Where(x => x.JobTemplateId == jobTemplateId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobTemplateActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.JobTemplate);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        /*
        public async Task<bool> EnableJobTemplateByIdAsync(long jobTemplateId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplate>().Where(x => x.JobTemplateId == jobTemplateId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableJobTemplateByIdAsync(long jobTemplateId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplate>().Where(x => x.JobTemplateId == jobTemplateId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateJobTemplateWithXYZZYXAsync(long jobTemplateId, long xYZZYXId, long? createdBy)
        {
            var item = new JobTemplateXYZZYXAssociationModel();
            item.JobTemplateId = jobTemplateId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<JobTemplateXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateJobTemplateWithXYZZYXAsync(long jobTemplateId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<JobTemplateXYZZYX>(x => x.JobTemplateId == jobTemplateId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesJobTemplateWithXYZZYXAssociationExistsAsync(long jobTemplateId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<JobTemplateXYZZYX>(x => x.JobTemplateId == jobTemplateId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<JobTemplateView>> GetJobTemplatesByXYZZYXAsync(long xYZZYXId)
        {
            var jobTemplateIds = GetJobTemplateXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.JobTemplateId);
            var jobTemplateSql = GetJobTemplatesViewAsIQueryable().Where(x => jobTemplateIds.Contains(x.JobTemplateId));
            return jobTemplateSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByJobTemplatesAsync(long jobTemplateId)
        {
            var xYZZYXIds = GetJobTemplateXYZZYXsViewAsIQueryable().Where(x => x.JobTemplateId == jobTemplate).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<JobTemplateXYZZYX> GetJobTemplateXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<JobTemplateXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobTemplateXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.JobTemplateXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region JobTemplateCategory

        public IQueryable<JobTemplateCategoryView> GetJobTemplateCategorysViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<JobTemplateCategory>()
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new JobTemplateCategoryView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       JobTemplateCategoryId = item.JobTemplateCategoryId,
                       Name = item.Name,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<JobTemplateCategoryView> GetJobTemplateCategorysInternalWithSearchCriteriaAsync(JobTemplateCategorySearchCriteria search)
        {
            var sql1 = GetJobTemplateCategorysViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.JobTemplateCategoryId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Name.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<JobTemplateCategoryView>> GetJobTemplateCategorysInternalAsync(JobTemplateCategorySearchCriteria search)
        {
            var sql1 = GetJobTemplateCategorysInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.JobTemplateCategoryId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetJobTemplateCategorysViewAsIQueryable().FirstOrDefaultAsync(x => x.JobTemplateCategoryId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<JobTemplateCategoryView>> GetJobTemplateCategorysAsync(JobTemplateCategorySearchCriteria search)
        {
            return GetJobTemplateCategorysInternalAsync(search);
        }

        public Task<JobTemplateCategory> GetJobTemplateCategoryFromAnyByIdAsync(long jobTemplateCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<JobTemplateCategory>(x => x.JobTemplateCategoryId == jobTemplateCategoryId);
        }

        public Task<JobTemplateCategoryView> GetJobTemplateCategoryViewFromAnyByIdAsync(long jobTemplateCategoryId)
        {
            return GetJobTemplateCategorysViewAsIQueryable().FirstOrDefaultAsync(x => x.JobTemplateCategoryId == jobTemplateCategoryId);
        }

        public Task<JobTemplateCategory> GetJobTemplateCategoryByIdAsync(long jobTemplateCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<JobTemplateCategory>(x => x.JobTemplateCategoryId == jobTemplateCategoryId && x.IsDeleted == false);
        }

        public Task<JobTemplateCategory> GetJobTemplateCategoryByUniqueIdentifierAsync(string jobTemplateCategoryId)
        {
            return appDBManager.FirstOrDefaultAsync<JobTemplateCategory>(x => x.JobTemplateCategoryId == jobTemplateCategoryId.Int() && x.IsDeleted == false);
        }

        public Task<JobTemplateCategoryView> GetJobTemplateCategoryViewByIdAsync(long jobTemplateCategoryId)
        {
            var sql1 = GetJobTemplateCategorysViewAsIQueryable();

            sql1 = sql1.Where(x => x.JobTemplateCategoryId == jobTemplateCategoryId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<JobTemplateCategory>> GetAllJobTemplateCategorys()
        {
            return appDBManager.SelectAllAsync<JobTemplateCategory>();
        }

        public async Task<JobTemplateCategory> CreateJobTemplateCategoryAsync(JobTemplateCategory jobTemplateCategory, long? createdBy)
        {
            jobTemplateCategory.JobTemplateCategoryId = await appDBManager.InsertWithAuditAsync<JobTemplateCategory, long>(jobTemplateCategory, createdBy);
            return jobTemplateCategory;
        }

        public async Task<List<JobTemplateCategory>> CreateJobTemplateCategorysAsync(List<JobTemplateCategory> jobTemplateCategorys, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<JobTemplateCategory, long>(jobTemplateCategorys, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                jobTemplateCategorys[i].JobTemplateCategoryId = lst[i];
            return jobTemplateCategorys;
        }

        public Task<bool> UpdateJobTemplateCategoryAsync(JobTemplateCategory jobTemplateCategory, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<JobTemplateCategory>(jobTemplateCategory, modifiedBy);
        }

        public async Task<bool> MarkJobTemplateCategoryAsNotDeletedByIdAsync(long jobTemplateCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplateCategory>().Where(x => x.JobTemplateCategoryId == jobTemplateCategoryId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkJobTemplateCategoryAsDeletedByIdAsync(long jobTemplateCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplateCategory>().Where(x => x.JobTemplateCategoryId == jobTemplateCategoryId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteJobTemplateCategoryByIdAsync(long jobTemplateCategoryId)
        {
            return await appDBManager.GetTable<JobTemplateCategory>().Where(x => x.JobTemplateCategoryId == jobTemplateCategoryId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobTemplateCategoryActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.JobTemplateCategory);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        /*
        public async Task<bool> EnableJobTemplateCategoryByIdAsync(long jobTemplateCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplateCategory>().Where(x => x.JobTemplateCategoryId == jobTemplateCategoryId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableJobTemplateCategoryByIdAsync(long jobTemplateCategoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplateCategory>().Where(x => x.JobTemplateCategoryId == jobTemplateCategoryId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateJobTemplateCategoryWithXYZZYXAsync(long jobTemplateCategoryId, long xYZZYXId, long? createdBy)
        {
            var item = new JobTemplateCategoryXYZZYXAssociationModel();
            item.JobTemplateCategoryId = jobTemplateCategoryId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<JobTemplateCategoryXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateJobTemplateCategoryWithXYZZYXAsync(long jobTemplateCategoryId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<JobTemplateCategoryXYZZYX>(x => x.JobTemplateCategoryId == jobTemplateCategoryId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesJobTemplateCategoryWithXYZZYXAssociationExistsAsync(long jobTemplateCategoryId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<JobTemplateCategoryXYZZYX>(x => x.JobTemplateCategoryId == jobTemplateCategoryId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<JobTemplateCategoryView>> GetJobTemplateCategorysByXYZZYXAsync(long xYZZYXId)
        {
            var jobTemplateCategoryIds = GetJobTemplateCategoryXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.JobTemplateCategoryId);
            var jobTemplateCategorySql = GetJobTemplateCategorysViewAsIQueryable().Where(x => jobTemplateCategoryIds.Contains(x.JobTemplateCategoryId));
            return jobTemplateCategorySql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByJobTemplateCategorysAsync(long jobTemplateCategoryId)
        {
            var xYZZYXIds = GetJobTemplateCategoryXYZZYXsViewAsIQueryable().Where(x => x.JobTemplateCategoryId == jobTemplateCategory).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<JobTemplateCategoryXYZZYX> GetJobTemplateCategoryXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<JobTemplateCategoryXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobTemplateCategoryXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.JobTemplateCategoryXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region JobTemplateParameter

        public IQueryable<JobTemplateParameterView> GetJobTemplateParametersViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<JobTemplateParameter>()
                   join a in appDBManager.GetTable<JobTemplate>() on item.JobTemplateId equals a.JobTemplateId
                   join b in appDBManager.GetTable<EnumType>() on item.PredefinedFieldControlTypeId equals b.EnumTypeId
                   join c in appDBManager.GetTable<EnumType>() on item.PredefinedFieldTypeId equals c.EnumTypeId
                   join d in appDBManager.GetTable<EnumType>() on item.PredefinedFieldPopulatorTypeId equals d.EnumTypeId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new JobTemplateParameterView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       PredefinedFieldControlTypeName = b.EnumTypeName,
                       PredefinedFieldTypeName = c.EnumTypeName,
                       PredefinedFieldPopulatorTypeName = c.EnumTypeName,

                       JobTemplateParameterId = item.JobTemplateParameterId,
                       JobTemplateId = item.JobTemplateId,
                       Name = item.Name,
                       Value = item.Value,
                       DefaultValue = item.DefaultValue,
                       PredefinedFieldControlTypeId = item.PredefinedFieldControlTypeId,
                       PredefinedFieldTypeId = item.PredefinedFieldTypeId,
                       PredefinedFieldPopulatorTypeId = item.PredefinedFieldPopulatorTypeId,
                       Caption = item.Caption,
                       PopulatorSource = item.PopulatorSource,
                       Size = item.Size,
                       Empty = item.Empty,
                       Position = item.Position,
                       IsVisible = item.IsVisible,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<JobTemplateParameterView> GetJobTemplateParametersInternalWithSearchCriteriaAsync(JobTemplateParameterSearchCriteria search)
        {
            var sql1 = GetJobTemplateParametersViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.JobTemplateParameterId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Name.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Value.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.DefaultValue.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Caption.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.PopulatorSource.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.JobTemplateId != null && search.JobTemplateId > 0)
                sql1 = sql1.Where(x => search.JobTemplateId == x.JobTemplateId);
            if (search.PredefinedFieldControlTypeIds != null && search.PredefinedFieldControlTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedFieldControlTypeIds.Contains(x.PredefinedFieldControlTypeId));
            if (search.PredefinedFieldTypeIds != null && search.PredefinedFieldTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedFieldTypeIds.Contains(x.PredefinedFieldTypeId));
            if (search.PredefinedFieldPopulatorTypeIds != null && search.PredefinedFieldPopulatorTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedFieldPopulatorTypeIds.Contains(x.PredefinedFieldPopulatorTypeId));
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<JobTemplateParameterView>> GetJobTemplateParametersInternalAsync(JobTemplateParameterSearchCriteria search)
        {
            var sql1 = GetJobTemplateParametersInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.JobTemplateParameterId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetJobTemplateParametersViewAsIQueryable().FirstOrDefaultAsync(x => x.JobTemplateParameterId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<JobTemplateParameterView>> GetJobTemplateParametersAsync(JobTemplateParameterSearchCriteria search)
        {
            return GetJobTemplateParametersInternalAsync(search);
        }

        public Task<JobTemplateParameter> GetJobTemplateParameterFromAnyByIdAsync(long jobTemplateParameterId)
        {
            return appDBManager.FirstOrDefaultAsync<JobTemplateParameter>(x => x.JobTemplateParameterId == jobTemplateParameterId);
        }

        public Task<JobTemplateParameterView> GetJobTemplateParameterViewFromAnyByIdAsync(long jobTemplateParameterId)
        {
            return GetJobTemplateParametersViewAsIQueryable().FirstOrDefaultAsync(x => x.JobTemplateParameterId == jobTemplateParameterId);
        }

        public Task<JobTemplateParameter> GetJobTemplateParameterByIdAsync(long jobTemplateParameterId)
        {
            return appDBManager.FirstOrDefaultAsync<JobTemplateParameter>(x => x.JobTemplateParameterId == jobTemplateParameterId && x.IsDeleted == false);
        }

        public Task<JobTemplateParameter> GetJobTemplateParameterByUniqueIdentifierAsync(string jobTemplateParameterId)
        {
            return appDBManager.FirstOrDefaultAsync<JobTemplateParameter>(x => x.JobTemplateParameterId == jobTemplateParameterId.Int() && x.IsDeleted == false);
        }

        public Task<JobTemplateParameterView> GetJobTemplateParameterViewByIdAsync(long jobTemplateParameterId)
        {
            var sql1 = GetJobTemplateParametersViewAsIQueryable();

            sql1 = sql1.Where(x => x.JobTemplateParameterId == jobTemplateParameterId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<JobTemplateParameter>> GetAllJobTemplateParameters()
        {
            return appDBManager.SelectAllAsync<JobTemplateParameter>();
        }

        public async Task<JobTemplateParameter> CreateJobTemplateParameterAsync(JobTemplateParameter jobTemplateParameter, long? createdBy)
        {
            jobTemplateParameter.JobTemplateParameterId = await appDBManager.InsertWithAuditAsync<JobTemplateParameter, long>(jobTemplateParameter, createdBy);
            return jobTemplateParameter;
        }

        public async Task<List<JobTemplateParameter>> CreateJobTemplateParametersAsync(List<JobTemplateParameter> jobTemplateParameters, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<JobTemplateParameter, long>(jobTemplateParameters, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                jobTemplateParameters[i].JobTemplateParameterId = lst[i];
            return jobTemplateParameters;
        }

        public Task<bool> UpdateJobTemplateParameterAsync(JobTemplateParameter jobTemplateParameter, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<JobTemplateParameter>(jobTemplateParameter, modifiedBy);
        }

        public async Task<bool> MarkJobTemplateParameterAsNotDeletedByIdAsync(long jobTemplateParameterId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplateParameter>().Where(x => x.JobTemplateParameterId == jobTemplateParameterId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkJobTemplateParameterAsDeletedByIdAsync(long jobTemplateParameterId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplateParameter>().Where(x => x.JobTemplateParameterId == jobTemplateParameterId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteJobTemplateParameterByIdAsync(long jobTemplateParameterId)
        {
            return await appDBManager.GetTable<JobTemplateParameter>().Where(x => x.JobTemplateParameterId == jobTemplateParameterId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobTemplateParameterActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.JobTemplateParameter);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        /*

        public async Task<bool> EnableJobTemplateParameterByIdAsync(long jobTemplateParameterId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplateParameter>().Where(x => x.JobTemplateParameterId == jobTemplateParameterId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableJobTemplateParameterByIdAsync(long jobTemplateParameterId, long? modifiedBy)
        {
            return await appDBManager.GetTable<JobTemplateParameter>().Where(x => x.JobTemplateParameterId == jobTemplateParameterId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateJobTemplateParameterWithXYZZYXAsync(long jobTemplateParameterId, long xYZZYXId, long? createdBy)
        {
            var item = new JobTemplateParameterXYZZYXAssociationModel();
            item.JobTemplateParameterId = jobTemplateParameterId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<JobTemplateParameterXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateJobTemplateParameterWithXYZZYXAsync(long jobTemplateParameterId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<JobTemplateParameterXYZZYX>(x => x.JobTemplateParameterId == jobTemplateParameterId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesJobTemplateParameterWithXYZZYXAssociationExistsAsync(long jobTemplateParameterId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<JobTemplateParameterXYZZYX>(x => x.JobTemplateParameterId == jobTemplateParameterId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<JobTemplateParameterView>> GetJobTemplateParametersByXYZZYXAsync(long xYZZYXId)
        {
            var jobTemplateParameterIds = GetJobTemplateParameterXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.JobTemplateParameterId);
            var jobTemplateParameterSql = GetJobTemplateParametersViewAsIQueryable().Where(x => jobTemplateParameterIds.Contains(x.JobTemplateParameterId));
            return jobTemplateParameterSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByJobTemplateParametersAsync(long jobTemplateParameterId)
        {
            var xYZZYXIds = GetJobTemplateParameterXYZZYXsViewAsIQueryable().Where(x => x.JobTemplateParameterId == jobTemplateParameter).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<JobTemplateParameterXYZZYX> GetJobTemplateParameterXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<JobTemplateParameterXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetJobTemplateParameterXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.JobTemplateParameterXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region SavedJob

        public IQueryable<SavedJobView> GetSavedJobsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<SavedJob>()
                   join a in appDBManager.GetTable<EnumType>() on item.PredefinedJobTypeId equals a.EnumTypeId
                   join ob in appDBManager.GetTable<EnumType>() on item.PredefinedJobOrReportTypeId equals ob.EnumTypeId into bTemp
                   from b in bTemp.DefaultIfEmpty()
                   join c in appDBManager.GetTable<JobTemplate>() on item.JobTemplateId equals c.JobTemplateId
                   join oe in appDBManager.GetTable<User>() on item.UserId equals oe.UserId into eTemp
                   from e in eTemp.DefaultIfEmpty()
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new SavedJobView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       PredefinedJobTypeName = a.EnumTypeName,
                       PredefinedJobOrReportTypeName = b.EnumTypeName,
                       UserName = e.UserName,
                       SavedJobId = item.SavedJobId,
                       PredefinedJobTypeId = item.PredefinedJobTypeId,
                       PredefinedJobOrReportTypeId = item.PredefinedJobOrReportTypeId,
                       JobTemplateId = item.JobTemplateId,
                       CompletedDate = item.CompletedDate,
                       SubmittedDate = item.SubmittedDate,
                       SubmittedByUserId = item.SubmittedByUserId,
                       ExecutableFolder = item.ExecutableFolder,
                       ExecutableName = item.ExecutableName,
                       ExecutableParameter = item.ExecutableParameter,
                       SerializedJobParameter = item.SerializedJobParameter,
                       ReportTemplateFile = item.ReportTemplateFile,
                       ExportDoc = item.ExportDoc,
                       ExportXls = item.ExportXls,
                       ExportPpt = item.ExportPpt,
                       ExportCsv = item.ExportCsv,
                       ExportXml = item.ExportXml,
                       ExportPdf = item.ExportPdf,
                       ExportCustom = item.ExportCustom,
                       CustomExportName = item.CustomExportName,
                       PickupDate = item.PickupDate,
                       JobName = item.JobName,
                       UserId = item.UserId,
                       IncludeDataSourceFile = item.IncludeDataSourceFile,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<SavedJobView> GetSavedJobsInternalWithSearchCriteriaAsync(SavedJobSearchCriteria search)
        {
            var sql1 = GetSavedJobsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.SavedJobId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.ExecutableFolder.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutableName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutableParameter.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.SerializedJobParameter.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ReportTemplateFile.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.CustomExportName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.JobName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.PredefinedJobTypeIds != null && search.PredefinedJobTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedJobTypeIds.Contains(x.PredefinedJobTypeId));
            if (search.PredefinedJobOrReportTypeIds != null && search.PredefinedJobOrReportTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedJobOrReportTypeIds.Contains(x.PredefinedJobOrReportTypeId ?? 0));
            if (search.JobTemplateId != null && search.JobTemplateId > 0)
                sql1 = sql1.Where(x => search.JobTemplateId == x.JobTemplateId);
            if (search.SubmittedByUserId != null && search.SubmittedByUserId > 0)
                sql1 = sql1.Where(x => search.SubmittedByUserId == x.SubmittedByUserId);
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

        public async Task<DbReturnListModel<SavedJobView>> GetSavedJobsInternalAsync(SavedJobSearchCriteria search)
        {
            var sql1 = GetSavedJobsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.SavedJobId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetSavedJobsViewAsIQueryable().FirstOrDefaultAsync(x => x.SavedJobId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<SavedJobView>> GetSavedJobsAsync(SavedJobSearchCriteria search)
        {
            return GetSavedJobsInternalAsync(search);
        }

        public Task<SavedJob> GetSavedJobFromAnyByIdAsync(long savedJobId)
        {
            return appDBManager.FirstOrDefaultAsync<SavedJob>(x => x.SavedJobId == savedJobId);
        }

        public Task<SavedJobView> GetSavedJobViewFromAnyByIdAsync(long savedJobId)
        {
            return GetSavedJobsViewAsIQueryable().FirstOrDefaultAsync(x => x.SavedJobId == savedJobId);
        }

        public Task<SavedJob> GetSavedJobByIdAsync(long savedJobId)
        {
            return appDBManager.FirstOrDefaultAsync<SavedJob>(x => x.SavedJobId == savedJobId && x.IsDeleted == false);
        }

        public Task<SavedJob> GetSavedJobByUniqueIdentifierAsync(string savedJobId)
        {
            return appDBManager.FirstOrDefaultAsync<SavedJob>(x => x.SavedJobId == savedJobId.Int() && x.IsDeleted == false);
        }

        public Task<SavedJobView> GetSavedJobViewByIdAsync(long savedJobId)
        {
            var sql1 = GetSavedJobsViewAsIQueryable();

            sql1 = sql1.Where(x => x.SavedJobId == savedJobId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<SavedJob>> GetAllSavedJobs()
        {
            return appDBManager.SelectAllAsync<SavedJob>();
        }

        public async Task<SavedJob> CreateSavedJobAsync(SavedJob savedJob, long? createdBy)
        {
            savedJob.SavedJobId = await appDBManager.InsertWithAuditAsync<SavedJob, long>(savedJob, createdBy);
            return savedJob;
        }

        public async Task<List<SavedJob>> CreateSavedJobsAsync(List<SavedJob> savedJobs, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<SavedJob, long>(savedJobs, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                savedJobs[i].SavedJobId = lst[i];
            return savedJobs;
        }

        public Task<bool> UpdateSavedJobAsync(SavedJob savedJob, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<SavedJob>(savedJob, modifiedBy);
        }

        public async Task<bool> MarkSavedJobAsNotDeletedByIdAsync(long savedJobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SavedJob>().Where(x => x.SavedJobId == savedJobId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkSavedJobAsDeletedByIdAsync(long savedJobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SavedJob>().Where(x => x.SavedJobId == savedJobId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteSavedJobByIdAsync(long savedJobId)
        {
            return await appDBManager.GetTable<SavedJob>().Where(x => x.SavedJobId == savedJobId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetSavedJobActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.SavedJob);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        /*

        public async Task<bool> EnableSavedJobByIdAsync(long savedJobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SavedJob>().Where(x => x.SavedJobId == savedJobId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableSavedJobByIdAsync(long savedJobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SavedJob>().Where(x => x.SavedJobId == savedJobId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateSavedJobWithXYZZYXAsync(long savedJobId, long xYZZYXId, long? createdBy)
        {
            var item = new SavedJobXYZZYXAssociationModel();
            item.SavedJobId = savedJobId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<SavedJobXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateSavedJobWithXYZZYXAsync(long savedJobId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<SavedJobXYZZYX>(x => x.SavedJobId == savedJobId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesSavedJobWithXYZZYXAssociationExistsAsync(long savedJobId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<SavedJobXYZZYX>(x => x.SavedJobId == savedJobId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<SavedJobView>> GetSavedJobsByXYZZYXAsync(long xYZZYXId)
        {
            var savedJobIds = GetSavedJobXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.SavedJobId);
            var savedJobSql = GetSavedJobsViewAsIQueryable().Where(x => savedJobIds.Contains(x.SavedJobId));
            return savedJobSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsBySavedJobsAsync(long savedJobId)
        {
            var xYZZYXIds = GetSavedJobXYZZYXsViewAsIQueryable().Where(x => x.SavedJobId == savedJob).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<SavedJobXYZZYX> GetSavedJobXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<SavedJobXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetSavedJobXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.SavedJobXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region ScheduledJob

        public IQueryable<ScheduledJobView> GetScheduledJobsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ScheduledJob>()
                   join a in appDBManager.GetTable<JobTemplate>() on item.JobTemplateId equals a.JobTemplateId
                   join ob in appDBManager.GetTable<EnumType>() on item.PredefinedJobOrReportTypeId equals ob.EnumTypeId into bTemp
                   from b in bTemp.DefaultIfEmpty()
                   join oj in appDBManager.GetTable<EnumType>() on item.PredefinedJobTypeId equals oj.EnumTypeId into jTemp
                   from j in jTemp.DefaultIfEmpty()
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new ScheduledJobView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       EnumTypeName = b.EnumTypeName,
                       ScheduledJobId = item.ScheduledJobId,
                       PredefinedJobTypeId = item.PredefinedJobTypeId,
                       PredefinedJobTypeName = j.EnumTypeName,
                       JobTemplateId = item.JobTemplateId,
                       PredefinedJobOrReportTypeId = item.PredefinedJobOrReportTypeId,
                       SubmittedByUserId = item.SubmittedByUserId,
                       ExecutableFolder = item.ExecutableFolder,
                       ExecutableName = item.ExecutableName,
                       ExecutableParameter = item.ExecutableParameter,
                       SerializedJobParameter = item.SerializedJobParameter,
                       ReportTemplateFile = item.ReportTemplateFile,
                       ExportDoc = item.ExportDoc,
                       ExportXls = item.ExportXls,
                       ExportPpt = item.ExportPpt,
                       ExportCsv = item.ExportCsv,
                       ExportXml = item.ExportXml,
                       ExportPdf = item.ExportPdf,
                       ExportCustom = item.ExportCustom,
                       CustomExportName = item.CustomExportName,
                       PickupDate = item.PickupDate,
                       JobName = item.JobName,
                       RunAsUserId = item.RunAsUserId,
                       StartDate = item.StartDate,
                       EndDate = item.EndDate,
                       StartTime = item.StartTime,
                       Recurring = item.Recurring,
                       RunMonday = item.RunMonday,
                       RunTuesday = item.RunTuesday,
                       RunWednesday = item.RunWednesday,
                       RunThursday = item.RunThursday,
                       RunFriday = item.RunFriday,
                       RunSaturday = item.RunSaturday,
                       RunSunday = item.RunSunday,
                       Active = item.Active,
                       IsMonthly = item.IsMonthly,
                       RunMonthlyDay = item.RunMonthlyDay,
                       RunMonthlyTime = item.RunMonthlyTime,
                       IsHourly = item.IsHourly,
                       IsDaily = item.IsDaily,
                       RunDailyTime = item.RunDailyTime,
                       IsWeekly = item.IsWeekly,
                       RunHourlyMinute = item.RunHourlyMinute,
                       IncludeDataSourceFile = item.IncludeDataSourceFile,
                       RunOnWeekNumber = item.RunOnWeekNumber,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<ScheduledJobView> GetScheduledJobsInternalWithSearchCriteriaAsync(ScheduledJobSearchCriteria search)
        {
            var sql1 = GetScheduledJobsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.ScheduledJobId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.ExecutableFolder.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutableName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ExecutableParameter.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.SerializedJobParameter.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ReportTemplateFile.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.CustomExportName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.JobName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.JobTemplateId != null && search.JobTemplateId > 0)
                sql1 = sql1.Where(x => search.JobTemplateId == x.JobTemplateId);
            if (search.PredefinedJobOrReportTypeIds != null && search.PredefinedJobOrReportTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedJobOrReportTypeIds.Contains(x.PredefinedJobOrReportTypeId ?? 0));
            if (search.SubmittedByUserId != null && search.SubmittedByUserId > 0)
                sql1 = sql1.Where(x => search.SubmittedByUserId == x.SubmittedByUserId);
            if (search.RunAsUserId != null && search.RunAsUserId > 0)
                sql1 = sql1.Where(x => search.RunAsUserId == x.RunAsUserId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<ScheduledJobView>> GetScheduledJobsInternalAsync(ScheduledJobSearchCriteria search)
        {
            var sql1 = GetScheduledJobsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.ScheduledJobId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetScheduledJobsViewAsIQueryable().FirstOrDefaultAsync(x => x.ScheduledJobId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<ScheduledJobView>> GetScheduledJobsAsync(ScheduledJobSearchCriteria search)
        {
            return GetScheduledJobsInternalAsync(search);
        }

        public Task<ScheduledJob> GetScheduledJobFromAnyByIdAsync(long scheduledJobId)
        {
            return appDBManager.FirstOrDefaultAsync<ScheduledJob>(x => x.ScheduledJobId == scheduledJobId);
        }

        public Task<ScheduledJobView> GetScheduledJobViewFromAnyByIdAsync(long scheduledJobId)
        {
            return GetScheduledJobsViewAsIQueryable().FirstOrDefaultAsync(x => x.ScheduledJobId == scheduledJobId);
        }

        public Task<ScheduledJob> GetScheduledJobByIdAsync(long scheduledJobId)
        {
            return appDBManager.FirstOrDefaultAsync<ScheduledJob>(x => x.ScheduledJobId == scheduledJobId && x.IsDeleted == false);
        }

        public Task<ScheduledJob> GetScheduledJobByUniqueIdentifierAsync(string scheduledJobId)
        {
            return appDBManager.FirstOrDefaultAsync<ScheduledJob>(x => x.ScheduledJobId == scheduledJobId.Int() && x.IsDeleted == false);
        }

        public Task<ScheduledJobView> GetScheduledJobViewByIdAsync(long scheduledJobId)
        {
            var sql1 = GetScheduledJobsViewAsIQueryable();

            sql1 = sql1.Where(x => x.ScheduledJobId == scheduledJobId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<ScheduledJob>> GetAllScheduledJobs()
        {
            return appDBManager.SelectAllAsync<ScheduledJob>();
        }

        public async Task<ScheduledJob> CreateScheduledJobAsync(ScheduledJob scheduledJob, long? createdBy)
        {
            scheduledJob.ScheduledJobId = await appDBManager.InsertWithAuditAsync<ScheduledJob, long>(scheduledJob, createdBy);
            return scheduledJob;
        }

        public async Task<List<ScheduledJob>> CreateScheduledJobsAsync(List<ScheduledJob> scheduledJobs, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<ScheduledJob, long>(scheduledJobs, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                scheduledJobs[i].ScheduledJobId = lst[i];
            return scheduledJobs;
        }

        public Task<bool> UpdateScheduledJobAsync(ScheduledJob scheduledJob, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<ScheduledJob>(scheduledJob, modifiedBy);
        }

        public async Task<bool> MarkScheduledJobAsNotDeletedByIdAsync(long scheduledJobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJob>().Where(x => x.ScheduledJobId == scheduledJobId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkScheduledJobAsDeletedByIdAsync(long scheduledJobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJob>().Where(x => x.ScheduledJobId == scheduledJobId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteScheduledJobByIdAsync(long scheduledJobId)
        {
            return await appDBManager.GetTable<ScheduledJob>().Where(x => x.ScheduledJobId == scheduledJobId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetScheduledJobActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.ScheduledJob);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsScheduledJobExistsForPickUpTimeAsync(long pickUpTime)
        {
            return appDBManager.ExistsAsync<ScheduledJob>(x => x.PickupTime == pickUpTime && x.IsDeleted == false);
        }

        /*

        public async Task<bool> EnableScheduledJobByIdAsync(long scheduledJobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJob>().Where(x => x.ScheduledJobId == scheduledJobId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableScheduledJobByIdAsync(long scheduledJobId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJob>().Where(x => x.ScheduledJobId == scheduledJobId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateScheduledJobWithXYZZYXAsync(long scheduledJobId, long xYZZYXId, long? createdBy)
        {
            var item = new ScheduledJobXYZZYXAssociationModel();
            item.ScheduledJobId = scheduledJobId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<ScheduledJobXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateScheduledJobWithXYZZYXAsync(long scheduledJobId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<ScheduledJobXYZZYX>(x => x.ScheduledJobId == scheduledJobId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesScheduledJobWithXYZZYXAssociationExistsAsync(long scheduledJobId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<ScheduledJobXYZZYX>(x => x.ScheduledJobId == scheduledJobId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<ScheduledJobView>> GetScheduledJobsByXYZZYXAsync(long xYZZYXId)
        {
            var scheduledJobIds = GetScheduledJobXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.ScheduledJobId);
            var scheduledJobSql = GetScheduledJobsViewAsIQueryable().Where(x => scheduledJobIds.Contains(x.ScheduledJobId));
            return scheduledJobSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByScheduledJobsAsync(long scheduledJobId)
        {
            var xYZZYXIds = GetScheduledJobXYZZYXsViewAsIQueryable().Where(x => x.ScheduledJobId == scheduledJob).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<ScheduledJobXYZZYX> GetScheduledJobXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ScheduledJobXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetScheduledJobXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.ScheduledJobXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region SavedJobNotificationEmail

        public IQueryable<SavedJobNotificationEmailView> GetSavedJobNotificationEmailsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<SavedJobNotificationEmail>()
                   join a in appDBManager.GetTable<SavedJob>() on item.SavedJobId equals a.SavedJobId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new SavedJobNotificationEmailView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       SavedJobNotificationEmailId = item.SavedJobNotificationEmailId,
                       SavedJobId = item.SavedJobId,
                       Email = item.Email,
                       SendStatus = item.SendStatus,
                       Message = item.Message,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<SavedJobNotificationEmailView> GetSavedJobNotificationEmailsInternalWithSearchCriteriaAsync(SavedJobNotificationEmailSearchCriteria search)
        {
            var sql1 = GetSavedJobNotificationEmailsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.SavedJobNotificationEmailId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Email.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Message.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.SavedJobId != null && search.SavedJobId > 0)
                sql1 = sql1.Where(x => search.SavedJobId == x.SavedJobId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<SavedJobNotificationEmailView>> GetSavedJobNotificationEmailsInternalAsync(SavedJobNotificationEmailSearchCriteria search)
        {
            var sql1 = GetSavedJobNotificationEmailsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.SavedJobNotificationEmailId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetSavedJobNotificationEmailsViewAsIQueryable().FirstOrDefaultAsync(x => x.SavedJobNotificationEmailId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<SavedJobNotificationEmailView>> GetSavedJobNotificationEmailsAsync(SavedJobNotificationEmailSearchCriteria search)
        {
            return GetSavedJobNotificationEmailsInternalAsync(search);
        }

        public Task<SavedJobNotificationEmail> GetSavedJobNotificationEmailFromAnyByIdAsync(long savedJobNotificationEmailId)
        {
            return appDBManager.FirstOrDefaultAsync<SavedJobNotificationEmail>(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId);
        }

        public Task<SavedJobNotificationEmailView> GetSavedJobNotificationEmailViewFromAnyByIdAsync(long savedJobNotificationEmailId)
        {
            return GetSavedJobNotificationEmailsViewAsIQueryable().FirstOrDefaultAsync(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId);
        }

        public Task<SavedJobNotificationEmail> GetSavedJobNotificationEmailByIdAsync(long savedJobNotificationEmailId)
        {
            return appDBManager.FirstOrDefaultAsync<SavedJobNotificationEmail>(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId && x.IsDeleted == false);
        }

        public Task<SavedJobNotificationEmail> GetSavedJobNotificationEmailByUniqueIdentifierAsync(string savedJobNotificationEmailId)
        {
            return appDBManager.FirstOrDefaultAsync<SavedJobNotificationEmail>(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId.Int() && x.IsDeleted == false);
        }

        public Task<SavedJobNotificationEmailView> GetSavedJobNotificationEmailViewByIdAsync(long savedJobNotificationEmailId)
        {
            var sql1 = GetSavedJobNotificationEmailsViewAsIQueryable();

            sql1 = sql1.Where(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<SavedJobNotificationEmail>> GetAllSavedJobNotificationEmails()
        {
            return appDBManager.SelectAllAsync<SavedJobNotificationEmail>();
        }

        public async Task<SavedJobNotificationEmail> CreateSavedJobNotificationEmailAsync(SavedJobNotificationEmail savedJobNotificationEmail, long? createdBy)
        {
            savedJobNotificationEmail.SavedJobNotificationEmailId = await appDBManager.InsertWithAuditAsync<SavedJobNotificationEmail, long>(savedJobNotificationEmail, createdBy);
            return savedJobNotificationEmail;
        }

        public async Task<List<SavedJobNotificationEmail>> CreateSavedJobNotificationEmailsAsync(List<SavedJobNotificationEmail> savedJobNotificationEmails, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<SavedJobNotificationEmail, long>(savedJobNotificationEmails, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                savedJobNotificationEmails[i].SavedJobNotificationEmailId = lst[i];
            return savedJobNotificationEmails;
        }

        public Task<bool> UpdateSavedJobNotificationEmailAsync(SavedJobNotificationEmail savedJobNotificationEmail, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<SavedJobNotificationEmail>(savedJobNotificationEmail, modifiedBy);
        }

        public async Task<bool> MarkSavedJobNotificationEmailAsNotDeletedByIdAsync(long savedJobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SavedJobNotificationEmail>().Where(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkSavedJobNotificationEmailAsDeletedByIdAsync(long savedJobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SavedJobNotificationEmail>().Where(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteSavedJobNotificationEmailByIdAsync(long savedJobNotificationEmailId)
        {
            return await appDBManager.GetTable<SavedJobNotificationEmail>().Where(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetSavedJobNotificationEmailActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.SavedJobNotificationEmail);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        /*

        public async Task<bool> EnableSavedJobNotificationEmailByIdAsync(long savedJobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SavedJobNotificationEmail>().Where(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableSavedJobNotificationEmailByIdAsync(long savedJobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SavedJobNotificationEmail>().Where(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateSavedJobNotificationEmailWithXYZZYXAsync(long savedJobNotificationEmailId, long xYZZYXId, long? createdBy)
        {
            var item = new SavedJobNotificationEmailXYZZYXAssociationModel();
            item.SavedJobNotificationEmailId = savedJobNotificationEmailId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<SavedJobNotificationEmailXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateSavedJobNotificationEmailWithXYZZYXAsync(long savedJobNotificationEmailId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<SavedJobNotificationEmailXYZZYX>(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesSavedJobNotificationEmailWithXYZZYXAssociationExistsAsync(long savedJobNotificationEmailId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<SavedJobNotificationEmailXYZZYX>(x => x.SavedJobNotificationEmailId == savedJobNotificationEmailId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<SavedJobNotificationEmailView>> GetSavedJobNotificationEmailsByXYZZYXAsync(long xYZZYXId)
        {
            var savedJobNotificationEmailIds = GetSavedJobNotificationEmailXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.SavedJobNotificationEmailId);
            var savedJobNotificationEmailSql = GetSavedJobNotificationEmailsViewAsIQueryable().Where(x => savedJobNotificationEmailIds.Contains(x.SavedJobNotificationEmailId));
            return savedJobNotificationEmailSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsBySavedJobNotificationEmailsAsync(long savedJobNotificationEmailId)
        {
            var xYZZYXIds = GetSavedJobNotificationEmailXYZZYXsViewAsIQueryable().Where(x => x.SavedJobNotificationEmailId == savedJobNotificationEmail).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<SavedJobNotificationEmailXYZZYX> GetSavedJobNotificationEmailXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<SavedJobNotificationEmailXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetSavedJobNotificationEmailXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.SavedJobNotificationEmailXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region ScheduledJobHistory

        public IQueryable<ScheduledJobHistoryView> GetScheduledJobHistorysViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ScheduledJobHistory>()
                   join a in appDBManager.GetTable<ScheduledJob>() on item.ScheduledJobId equals a.ScheduledJobId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new ScheduledJobHistoryView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       ScheduledJobHistoryId = item.ScheduledJobHistoryId,
                       ScheduledJobId = item.ScheduledJobId,
                       PickupDate = item.PickupDate,
                       PickupMachineName = item.PickupMachineName,
                       PickupProcessId = item.PickupProcessId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<ScheduledJobHistoryView> GetScheduledJobHistorysInternalWithSearchCriteriaAsync(ScheduledJobHistorySearchCriteria search)
        {
            var sql1 = GetScheduledJobHistorysViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.ScheduledJobHistoryId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.PickupMachineName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.ScheduledJobId != null && search.ScheduledJobId > 0)
                sql1 = sql1.Where(x => search.ScheduledJobId == x.ScheduledJobId);
            if (search.PickupProcessId != null && search.PickupProcessId > 0)
                sql1 = sql1.Where(x => search.PickupProcessId == x.PickupProcessId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<ScheduledJobHistoryView>> GetScheduledJobHistorysInternalAsync(ScheduledJobHistorySearchCriteria search)
        {
            var sql1 = GetScheduledJobHistorysInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.ScheduledJobHistoryId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetScheduledJobHistorysViewAsIQueryable().FirstOrDefaultAsync(x => x.ScheduledJobHistoryId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<ScheduledJobHistoryView>> GetScheduledJobHistorysAsync(ScheduledJobHistorySearchCriteria search)
        {
            return GetScheduledJobHistorysInternalAsync(search);
        }

        public Task<ScheduledJobHistory> GetScheduledJobHistoryFromAnyByIdAsync(long scheduledJobHistoryId)
        {
            return appDBManager.FirstOrDefaultAsync<ScheduledJobHistory>(x => x.ScheduledJobHistoryId == scheduledJobHistoryId);
        }

        public Task<ScheduledJobHistoryView> GetScheduledJobHistoryViewFromAnyByIdAsync(long scheduledJobHistoryId)
        {
            return GetScheduledJobHistorysViewAsIQueryable().FirstOrDefaultAsync(x => x.ScheduledJobHistoryId == scheduledJobHistoryId);
        }

        public Task<ScheduledJobHistory> GetScheduledJobHistoryByIdAsync(long scheduledJobHistoryId)
        {
            return appDBManager.FirstOrDefaultAsync<ScheduledJobHistory>(x => x.ScheduledJobHistoryId == scheduledJobHistoryId && x.IsDeleted == false);
        }

        public Task<ScheduledJobHistory> GetScheduledJobHistoryByUniqueIdentifierAsync(string scheduledJobHistoryId)
        {
            return appDBManager.FirstOrDefaultAsync<ScheduledJobHistory>(x => x.ScheduledJobHistoryId == scheduledJobHistoryId.Int() && x.IsDeleted == false);
        }

        public Task<ScheduledJobHistoryView> GetScheduledJobHistoryViewByIdAsync(long scheduledJobHistoryId)
        {
            var sql1 = GetScheduledJobHistorysViewAsIQueryable();

            sql1 = sql1.Where(x => x.ScheduledJobHistoryId == scheduledJobHistoryId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<ScheduledJobHistory>> GetAllScheduledJobHistorys()
        {
            return appDBManager.SelectAllAsync<ScheduledJobHistory>();
        }

        public async Task<ScheduledJobHistory> CreateScheduledJobHistoryAsync(ScheduledJobHistory scheduledJobHistory, long? createdBy)
        {
            scheduledJobHistory.ScheduledJobHistoryId = await appDBManager.InsertWithAuditAsync<ScheduledJobHistory, long>(scheduledJobHistory, createdBy);
            return scheduledJobHistory;
        }

        public async Task<List<ScheduledJobHistory>> CreateScheduledJobHistorysAsync(List<ScheduledJobHistory> scheduledJobHistorys, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<ScheduledJobHistory, long>(scheduledJobHistorys, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                scheduledJobHistorys[i].ScheduledJobHistoryId = lst[i];
            return scheduledJobHistorys;
        }

        public Task<bool> UpdateScheduledJobHistoryAsync(ScheduledJobHistory scheduledJobHistory, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<ScheduledJobHistory>(scheduledJobHistory, modifiedBy);
        }

        public async Task<bool> MarkScheduledJobHistoryAsNotDeletedByIdAsync(long scheduledJobHistoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJobHistory>().Where(x => x.ScheduledJobHistoryId == scheduledJobHistoryId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkScheduledJobHistoryAsDeletedByIdAsync(long scheduledJobHistoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJobHistory>().Where(x => x.ScheduledJobHistoryId == scheduledJobHistoryId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteScheduledJobHistoryByIdAsync(long scheduledJobHistoryId)
        {
            return await appDBManager.GetTable<ScheduledJobHistory>().Where(x => x.ScheduledJobHistoryId == scheduledJobHistoryId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetScheduledJobHistoryActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.ScheduledJobHistory);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        /*
        public async Task<bool> EnableScheduledJobHistoryByIdAsync(long scheduledJobHistoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJobHistory>().Where(x => x.ScheduledJobHistoryId == scheduledJobHistoryId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableScheduledJobHistoryByIdAsync(long scheduledJobHistoryId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJobHistory>().Where(x => x.ScheduledJobHistoryId == scheduledJobHistoryId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateScheduledJobHistoryWithXYZZYXAsync(long scheduledJobHistoryId, long xYZZYXId, long? createdBy)
        {
            var item = new ScheduledJobHistoryXYZZYXAssociationModel();
            item.ScheduledJobHistoryId = scheduledJobHistoryId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<ScheduledJobHistoryXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateScheduledJobHistoryWithXYZZYXAsync(long scheduledJobHistoryId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<ScheduledJobHistoryXYZZYX>(x => x.ScheduledJobHistoryId == scheduledJobHistoryId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesScheduledJobHistoryWithXYZZYXAssociationExistsAsync(long scheduledJobHistoryId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<ScheduledJobHistoryXYZZYX>(x => x.ScheduledJobHistoryId == scheduledJobHistoryId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<ScheduledJobHistoryView>> GetScheduledJobHistorysByXYZZYXAsync(long xYZZYXId)
        {
            var scheduledJobHistoryIds = GetScheduledJobHistoryXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.ScheduledJobHistoryId);
            var scheduledJobHistorySql = GetScheduledJobHistorysViewAsIQueryable().Where(x => scheduledJobHistoryIds.Contains(x.ScheduledJobHistoryId));
            return scheduledJobHistorySql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByScheduledJobHistorysAsync(long scheduledJobHistoryId)
        {
            var xYZZYXIds = GetScheduledJobHistoryXYZZYXsViewAsIQueryable().Where(x => x.ScheduledJobHistoryId == scheduledJobHistory).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<ScheduledJobHistoryXYZZYX> GetScheduledJobHistoryXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ScheduledJobHistoryXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetScheduledJobHistoryXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.ScheduledJobHistoryXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region ScheduledJobNotificationEmail

        public IQueryable<ScheduledJobNotificationEmailView> GetScheduledJobNotificationEmailsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ScheduledJobNotificationEmail>()
                   join a in appDBManager.GetTable<ScheduledJob>() on item.ScheduledJobId equals a.ScheduledJobId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new ScheduledJobNotificationEmailView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       ScheduledJobNotificationEmailId = item.ScheduledJobNotificationEmailId,
                       ScheduledJobId = item.ScheduledJobId,
                       Email = item.Email,
                       SendStatus = item.SendStatus,
                       Message = item.Message,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<ScheduledJobNotificationEmailView> GetScheduledJobNotificationEmailsInternalWithSearchCriteriaAsync(ScheduledJobNotificationEmailSearchCriteria search)
        {
            var sql1 = GetScheduledJobNotificationEmailsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.ScheduledJobNotificationEmailId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Email.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.Message.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.ScheduledJobId != null && search.ScheduledJobId > 0)
                sql1 = sql1.Where(x => search.ScheduledJobId == x.ScheduledJobId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<ScheduledJobNotificationEmailView>> GetScheduledJobNotificationEmailsInternalAsync(ScheduledJobNotificationEmailSearchCriteria search)
        {
            var sql1 = GetScheduledJobNotificationEmailsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.ScheduledJobNotificationEmailId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetScheduledJobNotificationEmailsViewAsIQueryable().FirstOrDefaultAsync(x => x.ScheduledJobNotificationEmailId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<ScheduledJobNotificationEmailView>> GetScheduledJobNotificationEmailsAsync(ScheduledJobNotificationEmailSearchCriteria search)
        {
            return GetScheduledJobNotificationEmailsInternalAsync(search);
        }

        public Task<ScheduledJobNotificationEmail> GetScheduledJobNotificationEmailFromAnyByIdAsync(long scheduledJobNotificationEmailId)
        {
            return appDBManager.FirstOrDefaultAsync<ScheduledJobNotificationEmail>(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId);
        }

        public Task<ScheduledJobNotificationEmailView> GetScheduledJobNotificationEmailViewFromAnyByIdAsync(long scheduledJobNotificationEmailId)
        {
            return GetScheduledJobNotificationEmailsViewAsIQueryable().FirstOrDefaultAsync(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId);
        }

        public Task<ScheduledJobNotificationEmail> GetScheduledJobNotificationEmailByIdAsync(long scheduledJobNotificationEmailId)
        {
            return appDBManager.FirstOrDefaultAsync<ScheduledJobNotificationEmail>(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId && x.IsDeleted == false);
        }

        public Task<ScheduledJobNotificationEmail> GetScheduledJobNotificationEmailByUniqueIdentifierAsync(string scheduledJobNotificationEmailId)
        {
            return appDBManager.FirstOrDefaultAsync<ScheduledJobNotificationEmail>(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId.Int() && x.IsDeleted == false);
        }

        public Task<ScheduledJobNotificationEmailView> GetScheduledJobNotificationEmailViewByIdAsync(long scheduledJobNotificationEmailId)
        {
            var sql1 = GetScheduledJobNotificationEmailsViewAsIQueryable();

            sql1 = sql1.Where(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<ScheduledJobNotificationEmail>> GetAllScheduledJobNotificationEmails()
        {
            return appDBManager.SelectAllAsync<ScheduledJobNotificationEmail>();
        }

        public async Task<ScheduledJobNotificationEmail> CreateScheduledJobNotificationEmailAsync(ScheduledJobNotificationEmail scheduledJobNotificationEmail, long? createdBy)
        {
            scheduledJobNotificationEmail.ScheduledJobNotificationEmailId = await appDBManager.InsertWithAuditAsync<ScheduledJobNotificationEmail, long>(scheduledJobNotificationEmail, createdBy);
            return scheduledJobNotificationEmail;
        }

        public async Task<List<ScheduledJobNotificationEmail>> CreateScheduledJobNotificationEmailsAsync(List<ScheduledJobNotificationEmail> scheduledJobNotificationEmails, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<ScheduledJobNotificationEmail, long>(scheduledJobNotificationEmails, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                scheduledJobNotificationEmails[i].ScheduledJobNotificationEmailId = lst[i];
            return scheduledJobNotificationEmails;
        }

        public Task<bool> UpdateScheduledJobNotificationEmailAsync(ScheduledJobNotificationEmail scheduledJobNotificationEmail, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<ScheduledJobNotificationEmail>(scheduledJobNotificationEmail, modifiedBy);
        }

        public async Task<bool> MarkScheduledJobNotificationEmailAsNotDeletedByIdAsync(long scheduledJobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJobNotificationEmail>().Where(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkScheduledJobNotificationEmailAsDeletedByIdAsync(long scheduledJobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJobNotificationEmail>().Where(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteScheduledJobNotificationEmailByIdAsync(long scheduledJobNotificationEmailId)
        {
            return await appDBManager.GetTable<ScheduledJobNotificationEmail>().Where(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetScheduledJobNotificationEmailActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.ScheduledJobNotificationEmail);
            return GetActivitiesByExpressionAsync(srch, search);
        }

        /*

        public async Task<bool> EnableScheduledJobNotificationEmailByIdAsync(long scheduledJobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJobNotificationEmail>().Where(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableScheduledJobNotificationEmailByIdAsync(long scheduledJobNotificationEmailId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ScheduledJobNotificationEmail>().Where(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateScheduledJobNotificationEmailWithXYZZYXAsync(long scheduledJobNotificationEmailId, long xYZZYXId, long? createdBy)
        {
            var item = new ScheduledJobNotificationEmailXYZZYXAssociationModel();
            item.ScheduledJobNotificationEmailId = scheduledJobNotificationEmailId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<ScheduledJobNotificationEmailXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateScheduledJobNotificationEmailWithXYZZYXAsync(long scheduledJobNotificationEmailId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<ScheduledJobNotificationEmailXYZZYX>(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesScheduledJobNotificationEmailWithXYZZYXAssociationExistsAsync(long scheduledJobNotificationEmailId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<ScheduledJobNotificationEmailXYZZYX>(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmailId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<ScheduledJobNotificationEmailView>> GetScheduledJobNotificationEmailsByXYZZYXAsync(long xYZZYXId)
        {
            var scheduledJobNotificationEmailIds = GetScheduledJobNotificationEmailXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.ScheduledJobNotificationEmailId);
            var scheduledJobNotificationEmailSql = GetScheduledJobNotificationEmailsViewAsIQueryable().Where(x => scheduledJobNotificationEmailIds.Contains(x.ScheduledJobNotificationEmailId));
            return scheduledJobNotificationEmailSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByScheduledJobNotificationEmailsAsync(long scheduledJobNotificationEmailId)
        {
            var xYZZYXIds = GetScheduledJobNotificationEmailXYZZYXsViewAsIQueryable().Where(x => x.ScheduledJobNotificationEmailId == scheduledJobNotificationEmail).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<ScheduledJobNotificationEmailXYZZYX> GetScheduledJobNotificationEmailXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ScheduledJobNotificationEmailXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetScheduledJobNotificationEmailXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.ScheduledJobNotificationEmailXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region AppExecutionVersion

        public Task<List<AppExecutionVersion>> GetAppExecutionVersionAsync()
        {
            return appDBManager.GetTable<AppExecutionVersion>().ToListAsync();
        }

        public async Task<AppExecutionVersion> CreateAppExecutionVersionAsync(AppExecutionVersion appExecutionVersion, long? createdBy)
        {
            appExecutionVersion.AppExecutionVersionId = await appDBManager.InsertWithAuditAsync<AppExecutionVersion, long>(appExecutionVersion, createdBy);
            return appExecutionVersion;
        }
        #endregion

        #region Message Template
        public Task<List<MessageTemplate>> GetAllMessageTemplatesAsync()
        {
            return appDBManager.GetTable<MessageTemplate>().ToListAsync();
        }

        public async Task SaveMessageTemplateAsync(MessageTemplate templateDetail, string userName)
        {
            if (templateDetail.MessageTemplateId == 0)
            {
                await appDBManager.InsertWithAuditAsync<MessageTemplate>(templateDetail, userName);
            }
            else
            {
                await appDBManager.UpdateWithAuditAsync(templateDetail, userName);
            }
        }

        public async Task<bool> DeleteMessageTemplateAsync(long messageTemplateId)
        {
            return await appDBManager.GetTable<MessageTemplate>()
                        .Where(x => x.MessageTemplateId == messageTemplateId)
                        .DeleteAsync() > 0;
        }

        public Task<bool> MarkRealtimeMessagesAsReadAsync(List<long> realtimeMessageIds, long userId, long? modifiedBy)
        {
            return MarkRealtimeMessagesAsStatusAsync(realtimeMessageIds, userId, PredefinedRealtimeMessageStatusTypes.Viewed, modifiedBy);
        }

        public async Task<bool> MarkRealtimeMessagesAsStatusAsync(List<long> realtimeMessageIds, long userId, PredefinedRealtimeMessageStatusTypes status, long? modifiedBy)
        {
            return await appDBManager.GetTable<RealtimeMessage>().Where(x => x.UserId == userId && realtimeMessageIds.Contains(x.RealtimeMessageId)).Set(x => x.PredefinedRealtimeMessageStatusTypeId, (int)status).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public Task<MessageTemplate> GetMessageTemplateByTypeAsync(long templateType)
        {
            return appDBManager.FirstOrDefaultAsync<MessageTemplate>(x => x.PredefinedMessageTemplateTypeId == templateType);
        }

        public Task<bool> IsMessageTemplateTypeAlreadyPresentAsync(long templateType)
        {
            return appDBManager.ExistsAsync<MessageTemplate>(x => x.PredefinedMessageTemplateTypeId == templateType);
        }
        #endregion

        #region MessageTemplate

        public IQueryable<MessageTemplateView> GetMessageTemplatesViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<MessageTemplate>()
                   join a in appDBManager.GetTable<EnumType>() on item.PredefinedMessageTemplateTypeId equals a.EnumTypeId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new MessageTemplateView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       PredefinedMessageTemplateTypeName = a.EnumTypeName,
                       MessageTemplateId = item.MessageTemplateId,
                       PredefinedMessageTemplateTypeId = item.PredefinedMessageTemplateTypeId,
                       EmailSubject = item.EmailSubject,
                       EmailBody = item.EmailBody,
                       SmsBody = item.SmsBody,
                       AppNotificationSubject = item.AppNotificationSubject,
                       AppNotificationBody = item.AppNotificationBody,
                       WebNotificationSubject = item.WebNotificationSubject,
                       WebNotificationBody = item.WebNotificationBody,
                       SendEmailForInternalUser = item.SendEmailForInternalUser,
                       SendSmsForInternalUser = item.SendSmsForInternalUser,
                       SendAppNotificationForInternalUser = item.SendAppNotificationForInternalUser,
                       SendEmailForExternalUser = item.SendEmailForExternalUser,
                       SendSmsForExternalUser = item.SendSmsForExternalUser,
                       SendAppNotificationForExternalUser = item.SendAppNotificationForExternalUser,
                       AdditionalRecipients = item.AdditionalRecipients,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<MessageTemplateView> GetMessageTemplatesInternalWithSearchCriteriaAsync(MessageTemplateSearchCriteria search)
        {
            var sql1 = GetMessageTemplatesViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.MessageTemplateId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.EmailSubject.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.EmailBody.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.SmsBody.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.AppNotificationSubject.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.AppNotificationBody.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.WebNotificationSubject.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.WebNotificationBody.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.AdditionalRecipients.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.PredefinedMessageTemplateTypeIds != null && search.PredefinedMessageTemplateTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedMessageTemplateTypeIds.Contains(x.PredefinedMessageTemplateTypeId));
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<MessageTemplateView>> GetMessageTemplatesInternalAsync(MessageTemplateSearchCriteria search)
        {
            var sql1 = GetMessageTemplatesInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.MessageTemplateId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetMessageTemplatesViewAsIQueryable().FirstOrDefaultAsync(x => x.MessageTemplateId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<MessageTemplateView>> GetMessageTemplatesAsync(MessageTemplateSearchCriteria search)
        {
            return GetMessageTemplatesInternalAsync(search);
        }

        public Task<MessageTemplate> GetMessageTemplateFromAnyByIdAsync(long messageTemplateId)
        {
            return appDBManager.FirstOrDefaultAsync<MessageTemplate>(x => x.MessageTemplateId == messageTemplateId);
        }

        public Task<MessageTemplateView> GetMessageTemplateViewFromAnyByIdAsync(long messageTemplateId)
        {
            return GetMessageTemplatesViewAsIQueryable().FirstOrDefaultAsync(x => x.MessageTemplateId == messageTemplateId);
        }

        public Task<MessageTemplate> GetMessageTemplateByUniqueIdentifierAsync(string messageTemplateId)
        {
            return appDBManager.FirstOrDefaultAsync<MessageTemplate>(x => x.MessageTemplateId == SafeUtils.Int(messageTemplateId) && x.IsDeleted == false);
        }

        public Task<MessageTemplate> GetMessageTemplateByIdAsync(long messageTemplateId)
        {
            return appDBManager.FirstOrDefaultAsync<MessageTemplate>(x => x.MessageTemplateId == messageTemplateId && x.IsDeleted == false);
        }

        public Task<MessageTemplateView> GetMessageTemplateViewByIdAsync(long messageTemplateId)
        {
            var sql1 = GetMessageTemplatesViewAsIQueryable();

            sql1 = sql1.Where(x => x.MessageTemplateId == messageTemplateId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<MessageTemplate>> GetAllMessageTemplates()
        {
            return appDBManager.SelectAllAsync<MessageTemplate>();
        }

        public async Task<MessageTemplate> CreateMessageTemplateAsync(MessageTemplate messageTemplate, long? createdBy)
        {
            messageTemplate.MessageTemplateId = await appDBManager.InsertWithAuditAsync<MessageTemplate, long>(messageTemplate, createdBy);
            return messageTemplate;
        }

        public async Task CreateMessageTemplateAsync(MessageTemplate item, string userName)
        {
            _ = await appDBManager.InsertWithAuditAsync<MessageTemplate>(item, userName);
        }

        public async Task<List<MessageTemplate>> CreateMessageTemplatesAsync(List<MessageTemplate> messageTemplates, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<MessageTemplate, long>(messageTemplates, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                messageTemplates[i].MessageTemplateId = lst[i];
            return messageTemplates;
        }

        public Task<bool> UpdateMessageTemplateAsync(MessageTemplate messageTemplate, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<MessageTemplate>(messageTemplate, modifiedBy);
        }

        public Task UpdateMessageTemplateAsync(MessageTemplate item, string userName)
        {
            return appDBManager.UpdateWithAuditAsync<MessageTemplate>(item, userName);
        }

        public async Task<bool> MarkMessageTemplateAsNotDeletedByIdAsync(long messageTemplateId, long? modifiedBy)
        {
            return await appDBManager.GetTable<MessageTemplate>().Where(x => x.MessageTemplateId == messageTemplateId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkMessageTemplateAsDeletedByIdAsync(long messageTemplateId, long? modifiedBy)
        {
            return await appDBManager.GetTable<MessageTemplate>().Where(x => x.MessageTemplateId == messageTemplateId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteMessageTemplateByIdAsync(long messageTemplateId)
        {
            return await appDBManager.GetTable<MessageTemplate>().Where(x => x.MessageTemplateId == messageTemplateId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetMessageTemplateActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.MessageTemplate);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsMessageTemplateNameAlreadyExistsForOtherMessageTemplateAsync(long messageTemplateId, long predefinedMessageTemplateTypeId)
        {
            var sql = GetMessageTemplatesViewAsIQueryable().Where(x => x.IsDeleted == false && x.MessageTemplateId != messageTemplateId && x.PredefinedMessageTemplateTypeId == predefinedMessageTemplateTypeId);
            return sql.AnyAsync();
        }

        public Task<bool> IsMessageTemplateNameAlreadyExistsForOtherMessageTemplateAsync(long predefinedMessageTemplateTypeId)
        {
            var sql = GetMessageTemplatesViewAsIQueryable().Where(x => x.IsDeleted == false && x.PredefinedMessageTemplateTypeId == predefinedMessageTemplateTypeId);
            return sql.AnyAsync();
        }

        /*

        public async Task<bool> EnableMessageTemplateByIdAsync(long messageTemplateId, long? modifiedBy)
        {
            return await appDBManager.GetTable<MessageTemplate>().Where(x => x.MessageTemplateId == messageTemplateId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableMessageTemplateByIdAsync(long messageTemplateId, long? modifiedBy)
        {
            return await appDBManager.GetTable<MessageTemplate>().Where(x => x.MessageTemplateId == messageTemplateId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateMessageTemplateWithXYZZYXAsync(long messageTemplateId, long xYZZYXId, long? createdBy)
        {
            var item = new MessageTemplateXYZZYXAssociationModel();
            item.MessageTemplateId = messageTemplateId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<MessageTemplateXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateMessageTemplateWithXYZZYXAsync(long messageTemplateId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<MessageTemplateXYZZYX>(x => x.MessageTemplateId == messageTemplateId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesMessageTemplateWithXYZZYXAssociationExistsAsync(long messageTemplateId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<MessageTemplateXYZZYX>(x => x.MessageTemplateId == messageTemplateId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<MessageTemplateView>> GetMessageTemplatesByXYZZYXAsync(long xYZZYXId)
        {
            var messageTemplateIds = GetMessageTemplateXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.MessageTemplateId);
            var messageTemplateSql = GetMessageTemplatesViewAsIQueryable().Where(x => messageTemplateIds.Contains(x.MessageTemplateId));
            return messageTemplateSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByMessageTemplatesAsync(long messageTemplateId)
        {
            var xYZZYXIds = GetMessageTemplateXYZZYXsViewAsIQueryable().Where(x => x.MessageTemplateId == messageTemplate).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<MessageTemplateXYZZYX> GetMessageTemplateXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<MessageTemplateXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetMessageTemplateXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.MessageTemplateXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region Realtime Message

        public async Task<DbReturnListModel<RealtimeMessageView>> GetNotShownRealtimeMessagesAsync(long userId, RealtimeMessageSearchCriteria search)
        {
            var sql = GetRealtimeMessagesViewAsIQueryable().Where(x => x.UserId == userId);
            if (search.PredefinedRealtimeMessageStatusTypeIds.IsNotNullOrEmpty())
            {
                sql = sql.Where(x => search.PredefinedRealtimeMessageStatusTypeIds.Contains(x.PredefinedRealtimeMessageStatusTypeId));
            }

            if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
            {
                if (search.StartDate.Value <= search.EndDate.Value)
                {
                    sql = sql.AsQueryable().Where(x => x.MessageDateTime >= search.StartDate.Value.StartOfDay() && x.MessageDateTime <= search.EndDate.Value.EndOfDay());
                }
            }
            else if (search.StartDate.IsValidDate())
            {
                sql = sql.AsQueryable().Where(x => x.MessageDateTime >= search.StartDate.Value.StartOfDay());
            }
            else if (search.EndDate.IsValidDate())
            {
                sql = sql.AsQueryable().Where(x => x.MessageDateTime <= search.EndDate.Value.EndOfDay());
            }

            var result = await sql.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.RealtimeMessageId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetRealtimeMessagesViewAsIQueryable().FirstOrDefaultAsync(x => x.RealtimeMessageId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public IQueryable<RealtimeMessageView> GetRealtimeMessagesViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<RealtimeMessage>()
                   join a in appDBManager.GetTable<EnumType>() on item.PredefinedRealtimeMessageStatusTypeId equals a.EnumTypeId
                   join ob in appDBManager.GetTable<EnumType>() on item.PredefinedResourceTypeId equals ob.EnumTypeId into bTemp
                   from b in bTemp.DefaultIfEmpty()
                   join c in appDBManager.GetTable<User>() on item.UserId equals c.UserId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new RealtimeMessageView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       PredefinedMessagePriorityTypeId = item.PredefinedMessagePriorityTypeId,
                       IsDeleted = item.IsDeleted,
                       PredefinedNotificationMessageTypeId = item.PredefinedNotificationMessageTypeId,
                       PredefinedRealtimeMessageStatusTypeName = a.EnumTypeName,
                       PredefinedResourceTypeName = b.EnumTypeName,
                       UserName = c.UserName,
                       RealtimeMessageId = item.RealtimeMessageId,
                       MessageDateTime = item.MessageDateTime,
                       MessageSubject = item.MessageSubject,
                       MessageContent = item.MessageContent,
                       MessageSentDateTime = item.MessageSentDateTime,
                       MessageViewedDateTime = item.MessageViewedDateTime,
                       PredefinedRealtimeMessageStatusTypeId = item.PredefinedRealtimeMessageStatusTypeId,
                       ResourceName = item.ResourceName,
                       PredefinedResourceTypeId = item.PredefinedResourceTypeId,
                       ResourceId = item.ResourceId,
                       ErrorMessage = item.ErrorMessage,
                       ErrorOccurredOn = item.ErrorOccurredOn,
                       UserId = item.UserId,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId
                   };
        }

        public async Task<DbReturnListModel<RealtimeMessageView>> GetRealtimeMessagesInternalAsync(RealtimeMessageSearchCriteria search)
        {
            var sql1 = GetRealtimeMessagesViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.RealtimeMessageId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.MessageSubject.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.MessageContent.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ResourceName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ErrorMessage.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.PredefinedRealtimeMessageStatusTypeIds != null && search.PredefinedRealtimeMessageStatusTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedRealtimeMessageStatusTypeIds.Contains(x.PredefinedRealtimeMessageStatusTypeId));

            if (search.ResourceId != null && search.ResourceId > 0)
                sql1 = sql1.Where(x => search.ResourceId == x.ResourceId);

            if (search.UserId != null && search.UserId > 0)
                sql1 = sql1.Where(x => search.UserId == x.UserId);

            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.RealtimeMessageId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetRealtimeMessagesViewAsIQueryable().FirstOrDefaultAsync(x => x.RealtimeMessageId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<RealtimeMessageView>> GetRealtimeMessagesAsync(RealtimeMessageSearchCriteria search)
        {
            return GetRealtimeMessagesInternalAsync(search);
        }

        public Task<RealtimeMessage> GetRealtimeMessageFromAnyByIdAsync(long realtimeMessageId)
        {
            return appDBManager.FirstOrDefaultAsync<RealtimeMessage>(x => x.RealtimeMessageId == realtimeMessageId);
        }

        public Task<RealtimeMessageView> GetRealtimeMessageViewFromAnyByIdAsync(long realtimeMessageId)
        {
            return GetRealtimeMessagesViewAsIQueryable().FirstOrDefaultAsync(x => x.RealtimeMessageId == realtimeMessageId);
        }

        public Task<RealtimeMessage> GetRealtimeMessageByIdAsync(long realtimeMessageId)
        {
            return appDBManager.FirstOrDefaultAsync<RealtimeMessage>(x => x.RealtimeMessageId == realtimeMessageId);
        }

        public Task<RealtimeMessageView> GetRealtimeMessageViewByIdAsync(long realtimeMessageId)
        {
            var sql1 = GetRealtimeMessagesViewAsIQueryable();

            sql1 = sql1.Where(x => x.RealtimeMessageId == realtimeMessageId);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<RealtimeMessage>> GetAllRealtimeMessages()
        {
            return appDBManager.SelectAllAsync<RealtimeMessage>();
        }

        public async Task<RealtimeMessage> CreateRealtimeMessageAsync(RealtimeMessage realtimeMessage, long? createdBy)
        {
            realtimeMessage.RealtimeMessageId = await appDBManager.InsertWithAuditAsync<RealtimeMessage, long>(realtimeMessage, createdBy);
            return realtimeMessage;
        }

        public async Task<List<RealtimeMessage>> CreateRealtimeMessagesAsync(List<RealtimeMessage> realtimeMessages, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<RealtimeMessage, long>(realtimeMessages, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                realtimeMessages[i].RealtimeMessageId = lst[i];
            return realtimeMessages;
        }

        public Task<bool> UpdateRealtimeMessageAsync(RealtimeMessage realtimeMessage, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<RealtimeMessage>(realtimeMessage, modifiedBy);
        }

        public async Task<bool> HardDeleteRealtimeMessageByIdAsync(long realtimeMessageId)
        {
            return await appDBManager.DeleteAsync<RealtimeMessage>(x => x.RealtimeMessageId == realtimeMessageId) > 0;
        }
        #endregion

        #region MessageToSend
        public async Task<MessageToSend> CreateMessageToSendAsync(MessageToSend messageToSend, long? createdBy)
        {
            messageToSend.MessageToSendId = await appDBManager.InsertWithAuditAsync<MessageToSend, long>(messageToSend, createdBy);
            return messageToSend;
        }

        public Task<MessageToSend> GetMessageToSendByIdAsync(long messageToSendById)
        {
            return appDBManager.GetTable<MessageToSend>().FirstOrDefaultAsync(x => x.MessageToSendId == messageToSendById);
        }

        public Task<long> GetMessageToSendCountAsync()
        {
            return appDBManager.CountAsync<MessageToSend>(x => x.IsDeleted == false);
        }

        public Task<long> GetMessageToSendErrorCountAsync()
        {
            return appDBManager.CountAsync<MessageToSend>(x => x.IsSentError == true && x.IsDeleted == false);
        }

        public Task<long> GetMessageToSendSuccessCountAsync()
        {
            return appDBManager.CountAsync<MessageToSend>(x => x.Sent == true && x.IsSentError == false && x.IsDeleted == false);
        }

        public async Task<DbReturnListModel<MessageToSend>> GetMessagesToSendAsync(MessageToSendSearchCriteria search)
        {
            var sql1 = appDBManager.GetTable<MessageToSend>().AsQueryable();
            if (search.IsSent != null)
                sql1 = sql1.Where(x => x.Sent == search.IsSent.Value);
            if (search.PredefinedMessageToSendTypeIds != null && search.PredefinedMessageToSendTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedMessageToSendTypeIds.Contains(x.PredefinedMessageToSendTypeId));

            if (search.StartDate.IsValidDate() && search.EndDate.IsValidDate())
            {
                if (search.StartDate.Value <= search.EndDate.Value)
                {
                    sql1 = sql1.AsQueryable().Where(x => x.CreatedDate >= search.StartDate.Value.StartOfDay() && x.CreatedDate <= search.EndDate.Value.EndOfDay());
                }
            }
            else if (search.StartDate.IsValidDate())
            {
                sql1 = sql1.AsQueryable().Where(x => x.CreatedDate >= search.StartDate.Value.StartOfDay());
            }
            else if (search.EndDate.IsValidDate())
            {
                sql1 = sql1.AsQueryable().Where(x => x.CreatedDate <= search.EndDate.Value.EndOfDay());
            }

            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.MessageToSendId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await appDBManager.GetTable<MessageToSend>().FirstOrDefaultAsync(x => x.MessageToSendId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }
        #endregion

        #region MailServer
        public Task<MailServer> GetMailServerByCompanyIdAsync(long userId)
        {
            return appDBManager.GetTable<MailServer>().FirstOrDefaultAsync();
        }

        public Task<MailServer> GetMailServerByIdAsync(long mailServerId)
        {
            return appDBManager.FirstOrDefaultAsync<MailServer>(x => x.MailServerId == mailServerId);
        }

        public Task<MailServer> GetMailServerByMailServerNameAsync(string mailServerName)
        {
            return appDBManager.FirstOrDefaultAsync<MailServer>(x => x.MailServerName == mailServerName);
        }

        public Task<MailServer> GetDefaultMailServerAsync()
        {
            return appDBManager.FirstOrDefaultAsync<MailServer>(x => x.IsDefault == true && x.IsDeleted == false);
        }

        public Task<MailServer> GetMailServerByMailServerCodeAsync(string mailServerCode)
        {
            return appDBManager.FirstOrDefaultAsync<MailServer>(x => x.MailServerCode == mailServerCode);
        }

        public async Task<bool> UpdateMailServerCodeAsync(long mailserverId, string mailserverCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<MailServer>().Where(x => x.MailServerId == mailserverId && x.IsDeleted == false).Set(x => x.MailServerCode, mailserverCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<MailServer> CreateMailServerAsync(MailServer mailServer, long? createdBy)
        {
            mailServer.MailServerId = await appDBManager.InsertWithAuditAsync<MailServer, long>(mailServer, createdBy);
            return mailServer;
        }

        public async Task<DbReturnListModel<MailServerView>> GetMailServersAsync(MailServerSearchCriteria search)
        {
            var sql = from item in appDBManager.GetTable<MailServer>()
                      join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                      from cu in cuTemp.DefaultIfEmpty()
                      join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                      from mu in muTemp.DefaultIfEmpty()
                      select new MailServerView
                      {
                          CreatedByUserFirstName = cu.FirstName,
                          CreatedByUserLastName = cu.LastName,
                          CreatedByUserEmail = cu.Email,
                          ModifiedByUserFirstName = mu.FirstName,
                          ModifiedByUserLastName = mu.LastName,
                          ModifiedByUserEmail = mu.Email,
                          MailServerId = item.MailServerId,
                          MailServerName = item.MailServerName,
                          Host = item.Host,
                          Port = item.Port,
                          SenderName = item.SenderName,
                          SenderEmail = item.SenderEmail,
                          UserName = item.UserName,
                          Password = item.Password,
                          IsSecure = item.IsSecure,
                          MailServerCode = item.MailServerCode,
                          IsDefault = item.IsDefault,
                          IsDeleted = item.IsDeleted,
                          CreatedDate = item.CreatedDate,
                          CreatedByUserId = item.CreatedByUserId,
                          ModifiedDate = item.ModifiedDate,
                          ModifiedByUserId = item.ModifiedByUserId,
                      };

            if (search.ShowDeleted == false)
                sql = sql.Where(x => x.IsDeleted == false);

            var sql2 = sql.AsQueryable();
            var result = await sql.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.MailServerId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await sql2.FirstOrDefaultAsync(x => x.MailServerId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<bool> UpdateMailServerAsync(MailServer mailServer, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<MailServer>(mailServer, modifiedBy);
        }

        public async Task<bool> MarkMailServerAsNotDeletedByIdAsync(long mailServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<MailServer>().Where(x => x.MailServerId == mailServerId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkMailServerAsDeletedByIdAsync(long mailServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<MailServer>().Where(x => x.MailServerId == mailServerId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteMailServerByIdAsync(long mailServerId)
        {
            return await appDBManager.GetTable<MailServer>().Where(x => x.MailServerId == mailServerId).DeleteAsync() > 0;
        }

        public Task<bool> IsMailServerAlreadyExistsByMailServerNameAsync(long mailServerId, string mailServerName)
        {
            var sql = appDBManager.GetTable<MailServer>().Where(x => x.MailServerId != mailServerId && x.IsDeleted == false && x.MailServerName == mailServerName);
            return sql.AnyAsync();
        }

        #endregion

        #region SmsServer
        public Task<SmsServer> GetSmsServerByCompanyIdAsync(long userId)
        {
            return appDBManager.GetTable<SmsServer>().FirstOrDefaultAsync();
        }

        public Task<SmsServer> GetSmsServerByIdAsync(long smsServerId)
        {
            return appDBManager.FirstOrDefaultAsync<SmsServer>(x => x.SmsServerId == smsServerId);
        }

        public Task<SmsServer> GetSmsServerBySmsServerNameAsync(string smsServerName)
        {
            return appDBManager.FirstOrDefaultAsync<SmsServer>(x => x.SmsServerName == smsServerName);
        }

        public Task<SmsServer> GetDefaultSmsServerAsync()
        {
            return appDBManager.FirstOrDefaultAsync<SmsServer>(x => x.IsDefault == true && x.IsDeleted == false);
        }

        public Task<SmsServer> GetSmsServerBySmsServerCodeAsync(string smsServerCode)
        {
            return appDBManager.FirstOrDefaultAsync<SmsServer>(x => x.SmsServerCode == smsServerCode);
        }

        public async Task<bool> UpdateSmsServerCodeAsync(long smsserverId, string smsserverCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<SmsServer>().Where(x => x.SmsServerId == smsserverId && x.IsDeleted == false).Set(x => x.SmsServerCode, smsserverCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<SmsServer> CreateSmsServerAsync(SmsServer smsServer, long? createdBy)
        {
            smsServer.SmsServerId = await appDBManager.InsertWithAuditAsync<SmsServer, long>(smsServer, createdBy);
            return smsServer;
        }

        public async Task<DbReturnListModel<SmsServerView>> GetSmsServersAsync(SmsServerSearchCriteria search)
        {
            var sql = from item in appDBManager.GetTable<SmsServer>()
                      join st in appDBManager.GetTable<EnumType>() on item.PredefinedSmsAndVoiceServerTypeId equals st.EnumTypeId
                      join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                      from cu in cuTemp.DefaultIfEmpty()
                      join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                      from mu in muTemp.DefaultIfEmpty()
                      select new SmsServerView
                      {
                          PredefinedSmsAndVoiceServerTypeName = st.EnumTypeName,
                          CreatedByUserFirstName = cu.FirstName,
                          CreatedByUserLastName = cu.LastName,
                          CreatedByUserEmail = cu.Email,
                          ModifiedByUserFirstName = mu.FirstName,
                          ModifiedByUserLastName = mu.LastName,
                          ModifiedByUserEmail = mu.Email,
                          SmsServerId = item.SmsServerId,
                          SmsServerName = item.SmsServerName,
                          Host = item.Host,
                          Port = item.Port,
                          SenderName = item.SenderName,
                          SenderEmail = item.SenderEmail,
                          UserName = item.UserName,
                          Password = item.Password,
                          IsSecure = item.IsSecure,
                          SmsServerCode = item.SmsServerCode,
                          PredefinedSmsAndVoiceServerTypeId = item.PredefinedSmsAndVoiceServerTypeId,
                          TwilioAccountAuthToken = item.TwilioAccountAuthToken,
                          TwilioAccountSid = item.TwilioAccountSid,
                          TwilioAccountPhoneNumber = item.TwilioAccountPhoneNumber,
                          IsDefault = item.IsDefault,
                          IsDeleted = item.IsDeleted,
                          CreatedDate = item.CreatedDate,
                          CreatedByUserId = item.CreatedByUserId,
                          ModifiedDate = item.ModifiedDate,
                          ModifiedByUserId = item.ModifiedByUserId,
                      };

            if (search.ShowDeleted == false)
                sql = sql.Where(x => x.IsDeleted == false);

            if (!search.PredefinedSmsAndVoiceServerTypeIds.IsNullOrEmpty())
                sql = sql.Where(x => search.PredefinedSmsAndVoiceServerTypeIds.Contains(x.PredefinedSmsAndVoiceServerTypeId));

            var sql2 = sql.AsQueryable();
            var result = await sql.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.SmsServerId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await sql2.FirstOrDefaultAsync(x => x.SmsServerId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<bool> UpdateSmsServerAsync(SmsServer smsServer, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<SmsServer>(smsServer, modifiedBy);
        }

        public async Task<bool> MarkSmsServerAsNotDeletedByIdAsync(long smsServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SmsServer>().Where(x => x.SmsServerId == smsServerId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkSmsServerAsDeletedByIdAsync(long smsServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SmsServer>().Where(x => x.SmsServerId == smsServerId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteSmsServerByIdAsync(long smsServerId)
        {
            return await appDBManager.GetTable<SmsServer>().Where(x => x.SmsServerId == smsServerId).DeleteAsync() > 0;
        }

        public Task<bool> IsSmsServerAlreadyExistsBySmsServerNameAsync(long smsServerId, string smsServerName)
        {
            var sql = appDBManager.GetTable<SmsServer>().Where(x => x.IsDeleted == false && x.SmsServerId != smsServerId && x.SmsServerName == smsServerName);
            return sql.AnyAsync();
        }

        #endregion

        #region CompanyStorageConfiguration

        public Task<CompanyStorageConfiguration> GetCompanyStorageConfigurationByCompanyIdAsync()
        {
            return appDBManager.GetTable<CompanyStorageConfiguration>().FirstOrDefaultAsync();
        }

        public IQueryable<CompanyStorageConfigurationView> GetCompanyStorageConfigurationsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<CompanyStorageConfiguration>()
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new CompanyStorageConfigurationView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       CompanyStorageConfigurationId = item.CompanyStorageConfigurationId,
                       CompanyStorageConfigurationGuid = item.CompanyStorageConfigurationGuid,
                       ProfilePictureFileStorageConnectionString = item.ProfilePictureFileStorageConnectionString,
                       ReportServerFileStorageConnectionString = item.ReportServerFileStorageConnectionString,
                       FileAttachmentFileStorageConnectionString = item.FileAttachmentFileStorageConnectionString,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<CompanyStorageConfigurationView> GetCompanyStorageConfigurationsInternalWithSearchCriteriaAsync(CompanyStorageConfigurationSearchCriteria search)
        {
            var sql1 = GetCompanyStorageConfigurationsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.CompanyStorageConfigurationId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.ProfilePictureFileStorageConnectionString.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ReportServerFileStorageConnectionString.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<CompanyStorageConfigurationView>> GetCompanyStorageConfigurationsInternalAsync(CompanyStorageConfigurationSearchCriteria search)
        {
            var sql1 = GetCompanyStorageConfigurationsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.CompanyStorageConfigurationId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetCompanyStorageConfigurationsViewAsIQueryable().FirstOrDefaultAsync(x => x.CompanyStorageConfigurationId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<CompanyStorageConfigurationView>> GetCompanyStorageConfigurationsAsync(CompanyStorageConfigurationSearchCriteria search)
        {
            return GetCompanyStorageConfigurationsInternalAsync(search);
        }

        public Task<CompanyStorageConfiguration> GetCompanyStorageConfigurationFromAnyByIdAsync(long companyStorageConfigurationId)
        {
            return appDBManager.FirstOrDefaultAsync<CompanyStorageConfiguration>(x => x.CompanyStorageConfigurationId == companyStorageConfigurationId);
        }

        public Task<CompanyStorageConfigurationView> GetCompanyStorageConfigurationViewFromAnyByIdAsync(long companyStorageConfigurationId)
        {
            return GetCompanyStorageConfigurationsViewAsIQueryable().FirstOrDefaultAsync(x => x.CompanyStorageConfigurationId == companyStorageConfigurationId);
        }

        public Task<CompanyStorageConfiguration> GetCompanyStorageConfigurationByUniqueIdentifierAsync(string companyStorageConfigurationId)
        {
            return appDBManager.FirstOrDefaultAsync<CompanyStorageConfiguration>(x => x.CompanyStorageConfigurationId == SafeUtils.Int(companyStorageConfigurationId) && x.IsDeleted == false);
        }

        public Task<CompanyStorageConfiguration> GetCompanyStorageConfigurationByIdAsync(long companyStorageConfigurationId)
        {
            return appDBManager.FirstOrDefaultAsync<CompanyStorageConfiguration>(x => x.CompanyStorageConfigurationId == companyStorageConfigurationId && x.IsDeleted == false);
        }

        public Task<CompanyStorageConfigurationView> GetCompanyStorageConfigurationViewByIdAsync(long companyStorageConfigurationId)
        {
            var sql1 = GetCompanyStorageConfigurationsViewAsIQueryable();

            sql1 = sql1.Where(x => x.CompanyStorageConfigurationId == companyStorageConfigurationId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<CompanyStorageConfiguration>> GetAllCompanyStorageConfigurations()
        {
            return appDBManager.SelectAllAsync<CompanyStorageConfiguration>();
        }

        public async Task<CompanyStorageConfiguration> CreateCompanyStorageConfigurationAsync(CompanyStorageConfiguration companyStorageConfiguration, long? createdBy)
        {
            companyStorageConfiguration.CompanyStorageConfigurationId = await appDBManager.InsertWithAuditAsync<CompanyStorageConfiguration, long>(companyStorageConfiguration, createdBy);
            return companyStorageConfiguration;
        }

        public async Task<List<CompanyStorageConfiguration>> CreateCompanyStorageConfigurationsAsync(List<CompanyStorageConfiguration> companyStorageConfigurations, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<CompanyStorageConfiguration, long>(companyStorageConfigurations, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                companyStorageConfigurations[i].CompanyStorageConfigurationId = lst[i];
            return companyStorageConfigurations;
        }

        public Task<bool> UpdateCompanyStorageConfigurationAsync(CompanyStorageConfiguration companyStorageConfiguration, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<CompanyStorageConfiguration>(companyStorageConfiguration, modifiedBy);
        }

        public async Task<bool> MarkCompanyStorageConfigurationAsNotDeletedByIdAsync(long companyStorageConfigurationId, long? modifiedBy)
        {
            return await appDBManager.GetTable<CompanyStorageConfiguration>().Where(x => x.CompanyStorageConfigurationId == companyStorageConfigurationId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkCompanyStorageConfigurationAsDeletedByIdAsync(long companyStorageConfigurationId, long? modifiedBy)
        {
            return await appDBManager.GetTable<CompanyStorageConfiguration>().Where(x => x.CompanyStorageConfigurationId == companyStorageConfigurationId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteCompanyStorageConfigurationByIdAsync(long companyStorageConfigurationId)
        {
            return await appDBManager.GetTable<CompanyStorageConfiguration>().Where(x => x.CompanyStorageConfigurationId == companyStorageConfigurationId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetCompanyStorageConfigurationActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.CompanyStorageConfiguration);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        /*

        public async Task<bool> EnableCompanyStorageConfigurationByIdAsync(long companyStorageConfigurationId, long? modifiedBy)
        {
            return await appDBManager.GetTable<CompanyStorageConfiguration>().Where(x => x.CompanyStorageConfigurationId == companyStorageConfigurationId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableCompanyStorageConfigurationByIdAsync(long companyStorageConfigurationId, long? modifiedBy)
        {
            return await appDBManager.GetTable<CompanyStorageConfiguration>().Where(x => x.CompanyStorageConfigurationId == companyStorageConfigurationId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateCompanyStorageConfigurationWithXYZZYXAsync(long companyStorageConfigurationId, long xYZZYXId, long? createdBy)
        {
            var item = new CompanyStorageConfigurationXYZZYXAssociationModel();
            item.CompanyStorageConfigurationId = companyStorageConfigurationId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<CompanyStorageConfigurationXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateCompanyStorageConfigurationWithXYZZYXAsync(long companyStorageConfigurationId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<CompanyStorageConfigurationXYZZYX>(x => x.CompanyStorageConfigurationId == companyStorageConfigurationId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesCompanyStorageConfigurationWithXYZZYXAssociationExistsAsync(long companyStorageConfigurationId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<CompanyStorageConfigurationXYZZYX>(x => x.CompanyStorageConfigurationId == companyStorageConfigurationId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<CompanyStorageConfigurationView>> GetCompanyStorageConfigurationsByXYZZYXAsync(long xYZZYXId)
        {
            var companyStorageConfigurationIds = GetCompanyStorageConfigurationXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.CompanyStorageConfigurationId);
            var companyStorageConfigurationSql = GetCompanyStorageConfigurationsViewAsIQueryable().Where(x => companyStorageConfigurationIds.Contains(x.CompanyStorageConfigurationId));
            return companyStorageConfigurationSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByCompanyStorageConfigurationsAsync(long companyStorageConfigurationId)
        {
            var xYZZYXIds = GetCompanyStorageConfigurationXYZZYXsViewAsIQueryable().Where(x => x.CompanyStorageConfigurationId == companyStorageConfiguration).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<CompanyStorageConfigurationXYZZYX> GetCompanyStorageConfigurationXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<CompanyStorageConfigurationXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetCompanyStorageConfigurationXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.CompanyStorageConfigurationXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region SingleSignOnServer

        public IQueryable<SingleSignOnServerView> GetSingleSignOnServersViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<SingleSignOnServer>()
                   join a in appDBManager.GetTable<EnumType>() on item.PredefinedSingleSignOnServerTypeId equals a.EnumTypeId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new SingleSignOnServerView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       SingleSignOnServerName = item.SingleSignOnServerName,
                       SingleSignOnServerTypeName = a.EnumTypeName,
                       SingleSignOnServerId = item.SingleSignOnServerId,
                       SingleSignOnServerCode = item.SingleSignOnServerCode,
                       PredefinedSingleSignOnServerTypeId = item.PredefinedSingleSignOnServerTypeId,
                       ForAllCompany = item.ForAllCompany,
                       ClientTenantId = item.ClientTenantId,
                       Host = item.Host,
                       ClientId = item.ClientId,
                       ClientSecret = item.ClientSecret,
                       IsDefault = item.IsDefault,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<SingleSignOnServerView> GetSingleSignOnServersInternalWithSearchCriteriaAsync(SingleSignOnServerSearchCriteria search)
        {
            var sql1 = GetSingleSignOnServersViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.SingleSignOnServerId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.Host.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ClientId.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ClientSecret.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.PredefinedSingleSignOnServerTypeIds.IsNotNullOrEmpty())
                sql1 = sql1.Where(x => search.PredefinedSingleSignOnServerTypeIds.Contains(x.PredefinedSingleSignOnServerTypeId));

            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);

            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<SingleSignOnServerView>> GetSingleSignOnServersInternalAsync(SingleSignOnServerSearchCriteria search)
        {
            var sql1 = GetSingleSignOnServersInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.SingleSignOnServerId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetSingleSignOnServersViewAsIQueryable().FirstOrDefaultAsync(x => x.SingleSignOnServerId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<SingleSignOnServerView>> GetSingleSignOnServersAsync(SingleSignOnServerSearchCriteria search)
        {
            return GetSingleSignOnServersInternalAsync(search);
        }

        public Task<SingleSignOnServer> GetSingleSignOnServerFromAnyByIdAsync(long singleSignOnServerId)
        {
            return appDBManager.FirstOrDefaultAsync<SingleSignOnServer>(x => x.SingleSignOnServerId == singleSignOnServerId);
        }

        public Task<SingleSignOnServerView> GetSingleSignOnServerViewFromAnyByIdAsync(long singleSignOnServerId)
        {
            return GetSingleSignOnServersViewAsIQueryable().FirstOrDefaultAsync(x => x.SingleSignOnServerId == singleSignOnServerId);
        }

        public Task<SingleSignOnServer> GetSingleSignOnServerByUniqueIdentifierAsync(string singleSignOnServerId)
        {
            return appDBManager.FirstOrDefaultAsync<SingleSignOnServer>(x => x.SingleSignOnServerId == SafeUtils.Int(singleSignOnServerId) && x.IsDeleted == false);
        }

        public Task<SingleSignOnServer> GetSingleSignOnServerByIdAsync(long singleSignOnServerId)
        {
            return appDBManager.FirstOrDefaultAsync<SingleSignOnServer>(x => x.SingleSignOnServerId == singleSignOnServerId && x.IsDeleted == false);
        }

        public Task<SingleSignOnServerView> GetSingleSignOnServerViewByIdAsync(long singleSignOnServerId)
        {
            var sql1 = GetSingleSignOnServersViewAsIQueryable();

            sql1 = sql1.Where(x => x.SingleSignOnServerId == singleSignOnServerId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<SingleSignOnServer>> GetAllSingleSignOnServers()
        {
            return appDBManager.SelectAllAsync<SingleSignOnServer>();
        }

        public Task<SingleSignOnServer> GetDefaultSingleSignOnServerAsync(long predefinedSingleSignOnServerId)
        {
            return appDBManager.FirstOrDefaultAsync<SingleSignOnServer>(x => x.IsDefault == true && x.PredefinedSingleSignOnServerTypeId == predefinedSingleSignOnServerId && x.IsDeleted == false);
        }

        public Task<SingleSignOnServer> GetSingleSignOnServerBySingleSignOnServerCodeAsync(string singleSignOnServerCode)
        {
            return appDBManager.FirstOrDefaultAsync<SingleSignOnServer>(x => x.SingleSignOnServerCode == singleSignOnServerCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateSingleSignOnServerCodeAsync(long singleSignOnServerId, string singleSignOnServerCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<SingleSignOnServer>().Where(x => x.SingleSignOnServerId == singleSignOnServerId && x.IsDeleted == false).Set(x => x.SingleSignOnServerCode, singleSignOnServerCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ResetAllDefaultSingleSignOnServerByTypeAsync(long predefinedSingleSignOnServerTypeId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SingleSignOnServer>().Where(x => x.PredefinedSingleSignOnServerTypeId == predefinedSingleSignOnServerTypeId && x.IsDeleted == false).Set(x => x.IsDefault, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> SetSingleSignOnServerAsDefaultAsync(long singleSignOnServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SingleSignOnServer>().Where(x => x.SingleSignOnServerId == singleSignOnServerId && x.IsDeleted == false).Set(x => x.IsDefault, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<SingleSignOnServer> CreateSingleSignOnServerAsync(SingleSignOnServer singleSignOnServer, long? createdBy)
        {
            singleSignOnServer.SingleSignOnServerId = await appDBManager.InsertWithAuditAsync<SingleSignOnServer, long>(singleSignOnServer, createdBy);
            return singleSignOnServer;
        }

        public async Task<List<SingleSignOnServer>> CreateSingleSignOnServersAsync(List<SingleSignOnServer> singleSignOnServers, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<SingleSignOnServer, long>(singleSignOnServers, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                singleSignOnServers[i].SingleSignOnServerId = lst[i];
            return singleSignOnServers;
        }

        public Task<bool> UpdateSingleSignOnServerAsync(SingleSignOnServer singleSignOnServer, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<SingleSignOnServer>(singleSignOnServer, modifiedBy);
        }

        public async Task<bool> MarkSingleSignOnServerAsNotDeletedByIdAsync(long singleSignOnServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SingleSignOnServer>().Where(x => x.SingleSignOnServerId == singleSignOnServerId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkSingleSignOnServerAsDeletedByIdAsync(long singleSignOnServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SingleSignOnServer>().Where(x => x.SingleSignOnServerId == singleSignOnServerId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteSingleSignOnServerByIdAsync(long singleSignOnServerId)
        {
            return await appDBManager.GetTable<SingleSignOnServer>().Where(x => x.SingleSignOnServerId == singleSignOnServerId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetSingleSignOnServerActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.SingleSignOnServer);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsSingleSignOnServerNameAlreadyExistsForOtherSingleSignOnServerAsync(long singleSignOnServerId, string singleSignOnServerName)
        {
            var sql = GetSingleSignOnServersViewAsIQueryable().Where(x => x.IsDeleted == false && x.SingleSignOnServerId != singleSignOnServerId && x.SingleSignOnServerName.ToLower().Trim() == singleSignOnServerName.ToLower().Trim());
            return sql.AnyAsync();
        }

        public Task<bool> IsSingleSignOnServerNameAlreadyExistsForOtherSingleSignOnServerAsync(string singleSignOnServerName)
        {
            var sql = GetSingleSignOnServersViewAsIQueryable().Where(x => x.IsDeleted == false && x.SingleSignOnServerName.ToLower().Trim() == singleSignOnServerName.ToLower().Trim());
            return sql.AnyAsync();
        }

        /*

        public async Task<bool> EnableSingleSignOnServerByIdAsync(long singleSignOnServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SingleSignOnServer>().Where(x => x.SingleSignOnServerId == singleSignOnServerId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableSingleSignOnServerByIdAsync(long singleSignOnServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<SingleSignOnServer>().Where(x => x.SingleSignOnServerId == singleSignOnServerId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateSingleSignOnServerWithXYZZYXAsync(long singleSignOnServerId, long xYZZYXId, long? createdBy)
        {
            var item = new SingleSignOnServerXYZZYXAssociationModel();
            item.SingleSignOnServerId = singleSignOnServerId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<SingleSignOnServerXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateSingleSignOnServerWithXYZZYXAsync(long singleSignOnServerId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<SingleSignOnServerXYZZYX>(x => x.SingleSignOnServerId == singleSignOnServerId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesSingleSignOnServerWithXYZZYXAssociationExistsAsync(long singleSignOnServerId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<SingleSignOnServerXYZZYX>(x => x.SingleSignOnServerId == singleSignOnServerId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<SingleSignOnServerView>> GetSingleSignOnServersByXYZZYXAsync(long xYZZYXId)
        {
            var singleSignOnServerIds = GetSingleSignOnServerXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.SingleSignOnServerId);
            var singleSignOnServerSql = GetSingleSignOnServersViewAsIQueryable().Where(x => singleSignOnServerIds.Contains(x.SingleSignOnServerId));
            return singleSignOnServerSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsBySingleSignOnServersAsync(long singleSignOnServerId)
        {
            var xYZZYXIds = GetSingleSignOnServerXYZZYXsViewAsIQueryable().Where(x => x.SingleSignOnServerId == singleSignOnServer).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<SingleSignOnServerXYZZYX> GetSingleSignOnServerXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<SingleSignOnServerXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetSingleSignOnServerXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.SingleSignOnServerXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region PushNotificationServer

        public IQueryable<PushNotificationServerView> GetPushNotificationServersViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<PushNotificationServer>()
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new PushNotificationServerView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       PushNotificationServerName = item.PushNotificationServerName,
                       PushNotificationServerCode = item.PushNotificationServerCode,
                       PushNotificationServerId = item.PushNotificationServerId,
                       PredefinedPushNotificationServerTypeId = item.PredefinedPushNotificationServerTypeId,
                       ProjectId = item.ProjectId,
                       ApiKey = item.ApiKey,
                       ApiSecret = item.ApiSecret,
                       AppleTeamId = item.AppleTeamId,
                       AppleKeyId = item.AppleKeyId,
                       AppleAuthKey = item.AppleAuthKey,
                       IsEnabled = item.IsEnabled,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<PushNotificationServerView> GetPushNotificationServersInternalWithSearchCriteriaAsync(PushNotificationServerSearchCriteria search)
        {
            var sql1 = GetPushNotificationServersViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.ProjectId.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ApiKey.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ApiSecret.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.AppleTeamId.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.AppleKeyId.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.AppleAuthKey.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<PushNotificationServerView>> GetPushNotificationServersInternalAsync(PushNotificationServerSearchCriteria search)
        {
            var sql1 = GetPushNotificationServersInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.PushNotificationServerId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetPushNotificationServersViewAsIQueryable().FirstOrDefaultAsync(x => x.PushNotificationServerId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<PushNotificationServerView>> GetPushNotificationServersAsync(PushNotificationServerSearchCriteria search)
        {
            return GetPushNotificationServersInternalAsync(search);
        }

        public Task<PushNotificationServer> GetPushNotificationServerFromAnyByIdAsync(long pushNotificationConfigurationId)
        {
            return appDBManager.FirstOrDefaultAsync<PushNotificationServer>(x => x.PushNotificationServerId == pushNotificationConfigurationId);
        }

        public Task<PushNotificationServerView> GetPushNotificationServerViewFromAnyByIdAsync(long pushNotificationConfigurationId)
        {
            return GetPushNotificationServersViewAsIQueryable().FirstOrDefaultAsync(x => x.PushNotificationServerId == pushNotificationConfigurationId);
        }

        public Task<PushNotificationServer> GetPushNotificationServerByUniqueIdentifierAsync(string pushNotificationConfigurationId)
        {
            return appDBManager.FirstOrDefaultAsync<PushNotificationServer>(x => x.PushNotificationServerId == SafeUtils.Int(pushNotificationConfigurationId) && x.IsDeleted == false);
        }

        public Task<PushNotificationServer> GetPushNotificationServerByIdAsync(long pushNotificationConfigurationId)
        {
            return appDBManager.FirstOrDefaultAsync<PushNotificationServer>(x => x.PushNotificationServerId == pushNotificationConfigurationId && x.IsDeleted == false);
        }

        public Task<PushNotificationServerView> GetPushNotificationServerViewByIdAsync(long pushNotificationConfigurationId)
        {
            var sql1 = GetPushNotificationServersViewAsIQueryable();

            sql1 = sql1.Where(x => x.PushNotificationServerId == pushNotificationConfigurationId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<PushNotificationServer>> GetAllPushNotificationServers()
        {
            return appDBManager.SelectAllAsync<PushNotificationServer>();
        }

        public async Task<PushNotificationServer> CreatePushNotificationServerAsync(PushNotificationServer pushNotificationConfiguration, long? createdBy)
        {
            pushNotificationConfiguration.PushNotificationServerId = await appDBManager.InsertWithAuditAsync<PushNotificationServer, long>(pushNotificationConfiguration, createdBy);
            return pushNotificationConfiguration;
        }

        public async Task<List<PushNotificationServer>> CreatePushNotificationServersAsync(List<PushNotificationServer> pushNotificationConfigurations, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<PushNotificationServer, long>(pushNotificationConfigurations, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                pushNotificationConfigurations[i].PushNotificationServerId = lst[i];
            return pushNotificationConfigurations;
        }

        public Task<bool> UpdatePushNotificationServerAsync(PushNotificationServer pushNotificationConfiguration, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<PushNotificationServer>(pushNotificationConfiguration, modifiedBy);
        }

        public Task<PushNotificationServer> GetPushNotificationServerByPushNotificationServerCodeAsync(string pushNotificationServerCode)
        {
            return appDBManager.FirstOrDefaultAsync<PushNotificationServer>(x => x.PushNotificationServerCode == pushNotificationServerCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdatePushNotificationServerCodeAsync(long pushNotificationServerId, string pushNotificationServerCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<PushNotificationServer>().Where(x => x.PushNotificationServerId == pushNotificationServerId && x.IsDeleted == false).Set(x => x.PushNotificationServerCode, pushNotificationServerCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkPushNotificationServerAsNotDeletedByIdAsync(long pushNotificationConfigurationId, long? modifiedBy)
        {
            return await appDBManager.GetTable<PushNotificationServer>().Where(x => x.PushNotificationServerId == pushNotificationConfigurationId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkPushNotificationServerAsDeletedByIdAsync(long pushNotificationConfigurationId, long? modifiedBy)
        {
            return await appDBManager.GetTable<PushNotificationServer>().Where(x => x.PushNotificationServerId == pushNotificationConfigurationId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeletePushNotificationServerByIdAsync(long pushNotificationConfigurationId)
        {
            return await appDBManager.GetTable<PushNotificationServer>().Where(x => x.PushNotificationServerId == pushNotificationConfigurationId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetPushNotificationServerActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.PushNotificationServer);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsPushNotificationServerNameAlreadyExistsForOtherPushNotificationServerAsync(long pushNotificationConfigurationId, string pushNotificationConfigurationName)
        {
            var sql = GetPushNotificationServersViewAsIQueryable().Where(x => x.IsDeleted == false && x.PushNotificationServerId != pushNotificationConfigurationId && x.PushNotificationServerName.ToLower().Trim() == pushNotificationConfigurationName.ToLower().Trim());
            return sql.AnyAsync();
        }

        public Task<bool> IsPushNotificationServerNameAlreadyExistsForOtherPushNotificationServerAsync(string pushNotificationConfigurationName)
        {
            var sql = GetPushNotificationServersViewAsIQueryable().Where(x => x.IsDeleted == false && x.PushNotificationServerName.ToLower().Trim() == pushNotificationConfigurationName.ToLower().Trim());
            return sql.AnyAsync();
        }

        /*

        public async Task<bool> UpdatePushNotificationServerPicturePathAsync(long pushNotificationConfigurationId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<PushNotificationServer>().Where(x => x.PushNotificationServerId == pushNotificationConfigurationId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearPushNotificationServerProfilePathAsync(long pushNotificationConfigurationId, long? modifiedBy)
        {
            return await appDBManager.GetTable<PushNotificationServer>().Where(x => x.PushNotificationServerId == pushNotificationConfigurationId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnablePushNotificationServerByIdAsync(long pushNotificationConfigurationId, long? modifiedBy)
        {
            return await appDBManager.GetTable<PushNotificationServer>().Where(x => x.PushNotificationServerId == pushNotificationConfigurationId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisablePushNotificationServerByIdAsync(long pushNotificationConfigurationId, long? modifiedBy)
        {
            return await appDBManager.GetTable<PushNotificationServer>().Where(x => x.PushNotificationServerId == pushNotificationConfigurationId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociatePushNotificationServerWithXYZZYXAsync(long pushNotificationConfigurationId, long xYZZYXId, long? createdBy)
        {
            var item = new PushNotificationServerXYZZYXAssociationModel();
            item.PushNotificationServerId = pushNotificationConfigurationId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<PushNotificationServerXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociatePushNotificationServerWithXYZZYXAsync(long pushNotificationConfigurationId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<PushNotificationServerXYZZYX>(x => x.PushNotificationServerId == pushNotificationConfigurationId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesPushNotificationServerWithXYZZYXAssociationExistsAsync(long pushNotificationConfigurationId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<PushNotificationServerXYZZYX>(x => x.PushNotificationServerId == pushNotificationConfigurationId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<PushNotificationServerView>> GetPushNotificationServersByXYZZYXAsync(long xYZZYXId)
        {
            var pushNotificationConfigurationIds = GetPushNotificationServerXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.PushNotificationServerId);
            var pushNotificationConfigurationSql = GetPushNotificationServersViewAsIQueryable().Where(x => pushNotificationConfigurationIds.Contains(x.PushNotificationServerId));
            return pushNotificationConfigurationSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByPushNotificationServersAsync(long pushNotificationConfigurationId)
        {
            var xYZZYXIds = GetPushNotificationServerXYZZYXsViewAsIQueryable().Where(x => x.PushNotificationServerId == pushNotificationConfiguration).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<PushNotificationServerXYZZYX> GetPushNotificationServerXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<PushNotificationServerXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetPushNotificationServerXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.PushNotificationServerXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region VoiceNotificationServer
        public IQueryable<VoiceNotificationServerView> GetVoiceNotificationServersViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<VoiceNotificationServer>()
                   join a in appDBManager.GetTable<EnumType>() on item.PredefinedSmsAndVoiceServerTypeId equals a.EnumTypeId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new VoiceNotificationServerView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       EnumTypeName = a.EnumTypeName,

                       VoiceNotificationServerName = item.VoiceNotificationServerName,
                       VoiceNotificationServerCode = item.VoiceNotificationServerCode,
                       VoiceNotificationServerId = item.VoiceNotificationServerId,
                       PredefinedSmsAndVoiceServerTypeId = item.PredefinedSmsAndVoiceServerTypeId,
                       TwilioAccountSid = item.TwilioAccountSid,
                       TwilioAccountAuthToken = item.TwilioAccountAuthToken,
                       TwilioAccountPhoneNumber = item.TwilioAccountPhoneNumber,
                       IsEnabled = item.IsEnabled,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<VoiceNotificationServerView> GetVoiceNotificationServersInternalWithSearchCriteriaAsync(VoiceNotificationServerSearchCriteria search)
        {
            var sql1 = GetVoiceNotificationServersViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.TwilioAccountSid.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.TwilioAccountAuthToken.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.TwilioAccountPhoneNumber.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.PredefinedSmsAndVoiceServerTypeIds != null && search.PredefinedSmsAndVoiceServerTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedSmsAndVoiceServerTypeIds.Contains(x.PredefinedSmsAndVoiceServerTypeId));
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<VoiceNotificationServerView>> GetVoiceNotificationServersInternalAsync(VoiceNotificationServerSearchCriteria search)
        {
            var sql1 = GetVoiceNotificationServersInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.VoiceNotificationServerId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetVoiceNotificationServersViewAsIQueryable().FirstOrDefaultAsync(x => x.VoiceNotificationServerId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<VoiceNotificationServerView>> GetVoiceNotificationServersAsync(VoiceNotificationServerSearchCriteria search)
        {
            return GetVoiceNotificationServersInternalAsync(search);
        }

        public Task<VoiceNotificationServer> GetVoiceNotificationServerFromAnyByIdAsync(long voiceNotificationServerId)
        {
            return appDBManager.FirstOrDefaultAsync<VoiceNotificationServer>(x => x.VoiceNotificationServerId == voiceNotificationServerId);
        }

        public Task<VoiceNotificationServerView> GetVoiceNotificationServerViewFromAnyByIdAsync(long voiceNotificationServerId)
        {
            return GetVoiceNotificationServersViewAsIQueryable().FirstOrDefaultAsync(x => x.VoiceNotificationServerId == voiceNotificationServerId);
        }

        public Task<VoiceNotificationServer> GetVoiceNotificationServerByUniqueIdentifierAsync(string voiceNotificationServerId)
        {
            return appDBManager.FirstOrDefaultAsync<VoiceNotificationServer>(x => x.VoiceNotificationServerId == SafeUtils.Int(voiceNotificationServerId) && x.IsDeleted == false);
        }

        public Task<VoiceNotificationServer> GetVoiceNotificationServerByIdAsync(long voiceNotificationServerId)
        {
            return appDBManager.FirstOrDefaultAsync<VoiceNotificationServer>(x => x.VoiceNotificationServerId == voiceNotificationServerId && x.IsDeleted == false);
        }


        public Task<VoiceNotificationServer> GetDefaultVoiceNotificationServerAsync()
        {
            return appDBManager.FirstOrDefaultAsync<VoiceNotificationServer>(x => x.IsDeleted == false);
        }


        public Task<VoiceNotificationServerView> GetVoiceNotificationServerViewByIdAsync(long voiceNotificationServerId)
        {
            var sql1 = GetVoiceNotificationServersViewAsIQueryable();

            sql1 = sql1.Where(x => x.VoiceNotificationServerId == voiceNotificationServerId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<VoiceNotificationServer>> GetAllVoiceNotificationServers()
        {
            return appDBManager.SelectAllAsync<VoiceNotificationServer>();
        }

        public Task<VoiceNotificationServer> GetVoiceNotificationServerByVoiceNotificationServerCodeAsync(string voiceNotificationServerCode)
        {
            return appDBManager.FirstOrDefaultAsync<VoiceNotificationServer>(x => x.VoiceNotificationServerCode == voiceNotificationServerCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateVoiceNotificationServerCodeAsync(long voiceNotificationServerId, string voiceNotificationServerCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<VoiceNotificationServer>().Where(x => x.VoiceNotificationServerId == voiceNotificationServerId && x.IsDeleted == false).Set(x => x.VoiceNotificationServerCode, voiceNotificationServerCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<VoiceNotificationServer> CreateVoiceNotificationServerAsync(VoiceNotificationServer voiceNotificationServer, long? createdBy)
        {
            voiceNotificationServer.VoiceNotificationServerId = await appDBManager.InsertWithAuditAsync<VoiceNotificationServer, long>(voiceNotificationServer, createdBy);
            return voiceNotificationServer;
        }

        public async Task<List<VoiceNotificationServer>> CreateVoiceNotificationServersAsync(List<VoiceNotificationServer> voiceNotificationServers, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<VoiceNotificationServer, long>(voiceNotificationServers, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                voiceNotificationServers[i].VoiceNotificationServerId = lst[i];
            return voiceNotificationServers;
        }

        public Task<bool> UpdateVoiceNotificationServerAsync(VoiceNotificationServer voiceNotificationServer, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<VoiceNotificationServer>(voiceNotificationServer, modifiedBy);
        }

        public async Task<bool> MarkVoiceNotificationServerAsNotDeletedByIdAsync(long voiceNotificationServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VoiceNotificationServer>().Where(x => x.VoiceNotificationServerId == voiceNotificationServerId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkVoiceNotificationServerAsDeletedByIdAsync(long voiceNotificationServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VoiceNotificationServer>().Where(x => x.VoiceNotificationServerId == voiceNotificationServerId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteVoiceNotificationServerByIdAsync(long voiceNotificationServerId)
        {
            return await appDBManager.GetTable<VoiceNotificationServer>().Where(x => x.VoiceNotificationServerId == voiceNotificationServerId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetVoiceNotificationServerActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.VoiceNotificationServer);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsVoiceNotificationServerNameAlreadyExistsForOtherVoiceNotificationServerAsync(long voiceNotificationServerId, string voiceNotificationServerName)
        {
            var sql = GetVoiceNotificationServersViewAsIQueryable().Where(x => x.IsDeleted == false && x.VoiceNotificationServerId != voiceNotificationServerId && x.VoiceNotificationServerName.ToLower().Trim() == voiceNotificationServerName.ToLower().Trim());
            return sql.AnyAsync();
        }

        public Task<bool> IsVoiceNotificationServerNameAlreadyExistsForOtherVoiceNotificationServerAsync(string voiceNotificationServerName)
        {
            var sql = GetVoiceNotificationServersViewAsIQueryable().Where(x => x.IsDeleted == false && x.VoiceNotificationServerName.ToLower().Trim() == voiceNotificationServerName.ToLower().Trim());
            return sql.AnyAsync();
        }

        /*

        public async Task<bool> UpdateVoiceNotificationServerPicturePathAsync(long voiceNotificationServerId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<VoiceNotificationServer>().Where(x => x.VoiceNotificationServerId == voiceNotificationServerId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearVoiceNotificationServerProfilePathAsync(long voiceNotificationServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VoiceNotificationServer>().Where(x => x.VoiceNotificationServerId == voiceNotificationServerId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableVoiceNotificationServerByIdAsync(long voiceNotificationServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VoiceNotificationServer>().Where(x => x.VoiceNotificationServerId == voiceNotificationServerId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableVoiceNotificationServerByIdAsync(long voiceNotificationServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<VoiceNotificationServer>().Where(x => x.VoiceNotificationServerId == voiceNotificationServerId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateVoiceNotificationServerWithXYZZYXAsync(long voiceNotificationServerId, long xYZZYXId, long? createdBy)
        {
            var item = new VoiceNotificationServerXYZZYXAssociationModel();
            item.VoiceNotificationServerId = voiceNotificationServerId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<VoiceNotificationServerXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateVoiceNotificationServerWithXYZZYXAsync(long voiceNotificationServerId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<VoiceNotificationServerXYZZYX>(x => x.VoiceNotificationServerId == voiceNotificationServerId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesVoiceNotificationServerWithXYZZYXAssociationExistsAsync(long voiceNotificationServerId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<VoiceNotificationServerXYZZYX>(x => x.VoiceNotificationServerId == voiceNotificationServerId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<VoiceNotificationServerView>> GetVoiceNotificationServersByXYZZYXAsync(long xYZZYXId)
        {
            var voiceNotificationServerIds = GetVoiceNotificationServerXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.VoiceNotificationServerId);
            var voiceNotificationServerSql = GetVoiceNotificationServersViewAsIQueryable().Where(x => voiceNotificationServerIds.Contains(x.VoiceNotificationServerId));
            return voiceNotificationServerSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByVoiceNotificationServersAsync(long voiceNotificationServerId)
        {
            var xYZZYXIds = GetVoiceNotificationServerXYZZYXsViewAsIQueryable().Where(x => x.VoiceNotificationServerId == voiceNotificationServer).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<VoiceNotificationServerXYZZYX> GetVoiceNotificationServerXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<VoiceNotificationServerXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetVoiceNotificationServerXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.VoiceNotificationServerXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region WhatsAppNotificationServer

        public IQueryable<WhatsAppNotificationServerView> GetWhatsAppNotificationServersViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<WhatsAppNotificationServer>()
                   join a in appDBManager.GetTable<EnumType>() on item.PredefinedSmsAndVoiceServerTypeId equals a.EnumTypeId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new WhatsAppNotificationServerView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       EnumTypeName = a.EnumTypeName,

                       WhatsAppNotificationServerId = item.WhatsAppNotificationServerId,
                       WhatsAppNotificationServerCode = item.WhatsAppNotificationServerCode,
                       WhatsAppNotificationServerName = item.WhatsAppNotificationServerName,
                       PredefinedSmsAndVoiceServerTypeId = item.PredefinedSmsAndVoiceServerTypeId,
                       AccessToken = item.AccessToken,
                       ApiKey = item.ApiKey,
                       ApiSecret = item.ApiSecret,
                       VersionNumber = item.VersionNumber,
                       PhoneNumberId = item.PhoneNumberId,
                       BusinessAccountId = item.BusinessAccountId,
                       SenderPhoneNumber = item.SenderPhoneNumber,
                       IsEnabled = item.IsEnabled,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<WhatsAppNotificationServerView> GetWhatsAppNotificationServersInternalWithSearchCriteriaAsync(WhatsAppNotificationServerSearchCriteria search)
        {
            var sql1 = GetWhatsAppNotificationServersViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.WhatsAppNotificationServerCode.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.WhatsAppNotificationServerName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ApiKey.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.ApiSecret.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.PhoneNumberId.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.BusinessAccountId.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())
                                || x.SenderPhoneNumber.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.PredefinedSmsAndVoiceServerTypeIds != null && search.PredefinedSmsAndVoiceServerTypeIds.Count > 0)
                sql1 = sql1.Where(x => search.PredefinedSmsAndVoiceServerTypeIds.Contains(x.PredefinedSmsAndVoiceServerTypeId));
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<WhatsAppNotificationServerView>> GetWhatsAppNotificationServersInternalAsync(WhatsAppNotificationServerSearchCriteria search)
        {
            var sql1 = GetWhatsAppNotificationServersInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.WhatsAppNotificationServerId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetWhatsAppNotificationServersViewAsIQueryable().FirstOrDefaultAsync(x => x.WhatsAppNotificationServerId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<WhatsAppNotificationServerView>> GetWhatsAppNotificationServersAsync(WhatsAppNotificationServerSearchCriteria search)
        {
            return GetWhatsAppNotificationServersInternalAsync(search);
        }

        public Task<WhatsAppNotificationServer> GetWhatsAppNotificationServerFromAnyByIdAsync(long whatsAppNotificationServerId)
        {
            return appDBManager.FirstOrDefaultAsync<WhatsAppNotificationServer>(x => x.WhatsAppNotificationServerId == whatsAppNotificationServerId);
        }

        public Task<WhatsAppNotificationServerView> GetWhatsAppNotificationServerViewFromAnyByIdAsync(long whatsAppNotificationServerId)
        {
            return GetWhatsAppNotificationServersViewAsIQueryable().FirstOrDefaultAsync(x => x.WhatsAppNotificationServerId == whatsAppNotificationServerId);
        }

        public Task<WhatsAppNotificationServer> GetWhatsAppNotificationServerByUniqueIdentifierAsync(string whatsAppNotificationServerId)
        {
            return appDBManager.FirstOrDefaultAsync<WhatsAppNotificationServer>(x => x.WhatsAppNotificationServerId == SafeUtils.Int(whatsAppNotificationServerId) && x.IsDeleted == false);
        }

        public Task<WhatsAppNotificationServer> GetWhatsAppNotificationServerByIdAsync(long whatsAppNotificationServerId)
        {
            return appDBManager.FirstOrDefaultAsync<WhatsAppNotificationServer>(x => x.WhatsAppNotificationServerId == whatsAppNotificationServerId && x.IsDeleted == false);
        }

        public Task<WhatsAppNotificationServer> GetDefaultWhatsAppNotificationServerAsync()
        {
            return appDBManager.FirstOrDefaultAsync<WhatsAppNotificationServer>(x => x.IsDeleted == false);
        }

        public Task<WhatsAppNotificationServerView> GetWhatsAppNotificationServerViewByIdAsync(long whatsAppNotificationServerId)
        {
            var sql1 = GetWhatsAppNotificationServersViewAsIQueryable();

            sql1 = sql1.Where(x => x.WhatsAppNotificationServerId == whatsAppNotificationServerId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<WhatsAppNotificationServer>> GetAllWhatsAppNotificationServers()
        {
            return appDBManager.SelectAllAsync<WhatsAppNotificationServer>();
        }

        public Task<WhatsAppNotificationServer> GetWhatsAppNotificationServerByWhatsAppNotificationServerCodeAsync(string whatsAppNotificationServerCode)
        {
            return appDBManager.FirstOrDefaultAsync<WhatsAppNotificationServer>(x => x.WhatsAppNotificationServerCode == whatsAppNotificationServerCode && x.IsDeleted == false);
        }

        public async Task<bool> UpdateWhatsAppNotificationServerCodeAsync(long whatsAppNotificationServerId, string whatsAppNotificationServerCode, long? modifiedBy)
        {
            return await appDBManager.GetTable<WhatsAppNotificationServer>().Where(x => x.WhatsAppNotificationServerId == whatsAppNotificationServerId && x.IsDeleted == false).Set(x => x.WhatsAppNotificationServerCode, whatsAppNotificationServerCode).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<WhatsAppNotificationServer> CreateWhatsAppNotificationServerAsync(WhatsAppNotificationServer whatsAppNotificationServer, long? createdBy)
        {
            whatsAppNotificationServer.WhatsAppNotificationServerId = await appDBManager.InsertWithAuditAsync<WhatsAppNotificationServer, long>(whatsAppNotificationServer, createdBy);
            return whatsAppNotificationServer;
        }

        public async Task<List<WhatsAppNotificationServer>> CreateWhatsAppNotificationServersAsync(List<WhatsAppNotificationServer> whatsAppNotificationServers, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<WhatsAppNotificationServer, long>(whatsAppNotificationServers, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                whatsAppNotificationServers[i].WhatsAppNotificationServerId = lst[i];
            return whatsAppNotificationServers;
        }

        public Task<bool> UpdateWhatsAppNotificationServerAsync(WhatsAppNotificationServer whatsAppNotificationServer, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<WhatsAppNotificationServer>(whatsAppNotificationServer, modifiedBy);
        }

        public async Task<bool> MarkWhatsAppNotificationServerAsNotDeletedByIdAsync(long whatsAppNotificationServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<WhatsAppNotificationServer>().Where(x => x.WhatsAppNotificationServerId == whatsAppNotificationServerId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkWhatsAppNotificationServerAsDeletedByIdAsync(long whatsAppNotificationServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<WhatsAppNotificationServer>().Where(x => x.WhatsAppNotificationServerId == whatsAppNotificationServerId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteWhatsAppNotificationServerByIdAsync(long whatsAppNotificationServerId)
        {
            return await appDBManager.GetTable<WhatsAppNotificationServer>().Where(x => x.WhatsAppNotificationServerId == whatsAppNotificationServerId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetWhatsAppNotificationServerActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.WhatsAppNotificationServer);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsWhatsAppNotificationServerNameAlreadyExistsForOtherWhatsAppNotificationServerAsync(long whatsAppNotificationServerId, string whatsAppNotificationServerName)
        {
            var sql = GetWhatsAppNotificationServersViewAsIQueryable().Where(x => x.IsDeleted == false && x.WhatsAppNotificationServerId != whatsAppNotificationServerId && x.WhatsAppNotificationServerName.ToLower().Trim() == whatsAppNotificationServerName.ToLower().Trim());
            return sql.AnyAsync();
        }

        public Task<bool> IsWhatsAppNotificationServerNameAlreadyExistsForOtherWhatsAppNotificationServerAsync(string whatsAppNotificationServerName)
        {
            var sql = GetWhatsAppNotificationServersViewAsIQueryable().Where(x => x.IsDeleted == false && x.WhatsAppNotificationServerName.ToLower().Trim() == whatsAppNotificationServerName.ToLower().Trim());
            return sql.AnyAsync();
        }

        /*

        public async Task<bool> UpdateWhatsAppNotificationServerPicturePathAsync(long whatsAppNotificationServerId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<WhatsAppNotificationServer>().Where(x => x.WhatsAppNotificationServerId == whatsAppNotificationServerId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearWhatsAppNotificationServerProfilePathAsync(long whatsAppNotificationServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<WhatsAppNotificationServer>().Where(x => x.WhatsAppNotificationServerId == whatsAppNotificationServerId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableWhatsAppNotificationServerByIdAsync(long whatsAppNotificationServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<WhatsAppNotificationServer>().Where(x => x.WhatsAppNotificationServerId == whatsAppNotificationServerId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableWhatsAppNotificationServerByIdAsync(long whatsAppNotificationServerId, long? modifiedBy)
        {
            return await appDBManager.GetTable<WhatsAppNotificationServer>().Where(x => x.WhatsAppNotificationServerId == whatsAppNotificationServerId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateWhatsAppNotificationServerWithXYZZYXAsync(long whatsAppNotificationServerId, long xYZZYXId, long? createdBy)
        {
            var item = new WhatsAppNotificationServerXYZZYXAssociationModel();
            item.WhatsAppNotificationServerId = whatsAppNotificationServerId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<WhatsAppNotificationServerXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateWhatsAppNotificationServerWithXYZZYXAsync(long whatsAppNotificationServerId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<WhatsAppNotificationServerXYZZYX>(x => x.WhatsAppNotificationServerId == whatsAppNotificationServerId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesWhatsAppNotificationServerWithXYZZYXAssociationExistsAsync(long whatsAppNotificationServerId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<WhatsAppNotificationServerXYZZYX>(x => x.WhatsAppNotificationServerId == whatsAppNotificationServerId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<WhatsAppNotificationServerView>> GetWhatsAppNotificationServersByXYZZYXAsync(long xYZZYXId)
        {
            var whatsAppNotificationServerIds = GetWhatsAppNotificationServerXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.WhatsAppNotificationServerId);
            var whatsAppNotificationServerSql = GetWhatsAppNotificationServersViewAsIQueryable().Where(x => whatsAppNotificationServerIds.Contains(x.WhatsAppNotificationServerId));
            return whatsAppNotificationServerSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByWhatsAppNotificationServersAsync(long whatsAppNotificationServerId)
        {
            var xYZZYXIds = GetWhatsAppNotificationServerXYZZYXsViewAsIQueryable().Where(x => x.WhatsAppNotificationServerId == whatsAppNotificationServer).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<WhatsAppNotificationServerXYZZYX> GetWhatsAppNotificationServerXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<WhatsAppNotificationServerXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetWhatsAppNotificationServerXYZZYXActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.ResourceTypeId == (int)ResourceTypes.WhatsAppNotificationServerXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region AppExecutionVersion
        public Task<List<AppExecutionVersion>> GetAllAppExecutionVersions()
        {
            return appDBManager.SelectAllAsync<AppExecutionVersion>();
        }

        public async Task<bool> UpdateAppExecutionVersionAsync(long appExecutionVersion, long? modifiedBy)
        {
            return await appDBManager.GetTable<AppExecutionVersion>().Set(x => x.Version, appExecutionVersion).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        #endregion
    }
}