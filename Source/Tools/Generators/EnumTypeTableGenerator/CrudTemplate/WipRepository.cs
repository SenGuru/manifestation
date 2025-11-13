        #region EEntity123

        public IQueryable<EEntity123View> GetEEntity123sViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<EEntity123>()
                       //%RepoJoin%
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new EEntity123View
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,

                       //%RepoAssignment%
                   };
        }

        public IQueryable<EEntity123View> GetEEntity123sInternalWithSearchCriteriaAsync(EEntity123SearchCriteria search)
        {
            var sql1 = GetEEntity123sViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.EEntity123Id.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                //%RepoWhereCheck%);
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

    //%RepoConditionalWhere%
    if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<EEntity123View>> GetEEntity123sInternalAsync(EEntity123SearchCriteria search)
        {
            var sql1 = GetEEntity123sInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.EEntity123Id == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetEEntity123sViewAsIQueryable().FirstOrDefaultAsync(x => x.EEntity123Id == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<EEntity123View>> GetEEntity123sAsync(EEntity123SearchCriteria search)
        {
            return GetEEntity123sInternalAsync(search);
        }

        public Task<EEntity123> GetEEntity123FromAnyByIdAsync(long eentity123Id)
        {
            return appDBManager.FirstOrDefaultAsync<EEntity123>(x => x.EEntity123Id == eentity123Id);
        }

        public Task<EEntity123View> GetEEntity123ViewFromAnyByIdAsync(long eentity123Id)
        {
            return GetEEntity123sViewAsIQueryable().FirstOrDefaultAsync(x => x.EEntity123Id == eentity123Id);
        }

        public Task<EEntity123> GetEEntity123ByUniqueIdentifierAsync(string eentity123Id)
        {
            return appDBManager.FirstOrDefaultAsync<EEntity123>(x => x.EEntity123Id == SafeUtils.Int(eentity123Id) && x.IsDeleted == false);
        }

        public Task<EEntity123> GetEEntity123ByIdAsync(long eentity123Id)
        {
            return appDBManager.FirstOrDefaultAsync<EEntity123>(x => x.EEntity123Id == eentity123Id && x.IsDeleted == false);
        }

        public Task<List<EEntity123>> GetEEntity123ByIdsAsync(List<long> eentity123Ids)
        {
            return appDBManager.GetTable<EEntity123>().Where(x => eentity123Ids.Contains(x.EEntity123Id) && x.IsDeleted == false).ToListAsync();
        }

        public Task<EEntity123View> GetEEntity123ViewByIdAsync(long eentity123Id)
        {
            var sql1 = GetEEntity123sViewAsIQueryable();

            sql1 = sql1.Where(x => x.EEntity123Id == eentity123Id && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<EEntity123>> GetAllEEntity123s()
        {
            return appDBManager.SelectAllAsync<EEntity123>();
        }

        /*
        public Task<EEntity123> GetEEntity123ByEEntity123CodeAsync(long companyId, string eentity123Code)
        {
            return appDBManager.FirstOrDefaultAsync<EEntity123>(x => x.CompanyId == companyId && x.EEntity123Code == eentity123Code && x.IsDeleted == false);
        }

        public async Task<bool> UpdateEEntity123CodeAsync(long companyId, long eentity123Id, string eentity123Code, long? modifiedBy)
        {
            return await appDBManager.GetTable<EEntity123>().Where(x => x.CompanyId == companyId && x.EEntity123Id == eentity123Id && x.IsDeleted == false).Set(x => x.EEntity123Code, eentity123Code).UpdateWithAuditAsync(modifiedBy) > 0;
        }
        */

        public async Task<EEntity123> CreateEEntity123Async(EEntity123 eentity123, long? createdBy)
        {
            eentity123.EEntity123Id = await appDBManager.InsertWithAuditAsync<EEntity123, long>(eentity123, createdBy);
            return eentity123;
        }

        public async Task<List<EEntity123>> CreateEEntity123sAsync(List<EEntity123> eentity123s, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<EEntity123, long>(eentity123s, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                eentity123s[i].EEntity123Id = lst[i];
            return eentity123s;
        }

        public Task<bool> UpdateEEntity123Async(EEntity123 eentity123, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<EEntity123>(eentity123, modifiedBy);
        }

        public async Task<bool> MarkEEntity123AsNotDeletedByIdAsync(long eentity123Id, long? modifiedBy)
        {
            return await appDBManager.GetTable<EEntity123>().Where(x => x.EEntity123Id == eentity123Id).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkEEntity123AsDeletedByIdAsync(long eentity123Id, long? modifiedBy)
        {
            return await appDBManager.GetTable<EEntity123>().Where(x => x.EEntity123Id == eentity123Id).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteEEntity123ByIdAsync(long eentity123Id)
        {
            return await appDBManager.GetTable<EEntity123>().Where(x => x.EEntity123Id == eentity123Id).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetEEntity123ActivitiesAsync(long companyId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.CompanyId == companyId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.EEntity123);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> IsEEntity123NameAlreadyExistsForOtherEEntity123Async(long eentity123Id, string eentity123Name, long companyId)
        {
            var sql = GetEEntity123sViewAsIQueryable().Where(x => x.IsDeleted == false && x.EEntity123Id != eentity123Id && x.EEntity123Name.ToLower().Trim() == eentity123Name.ToLower().Trim() && x.CompanyId == companyId);
            return sql.AnyAsync();
        }

        public Task<bool> IsEEntity123NameAlreadyExistsForOtherEEntity123Async(string eentity123Name, long companyId)
        {
            var sql = GetEEntity123sViewAsIQueryable().Where(x => x.IsDeleted == false && x.EEntity123Name.ToLower().Trim() == eentity123Name.ToLower().Trim() && x.CompanyId == companyId);
            return sql.AnyAsync();
        }

        //%RepoFkeyBasedFunctions%

        //%ParentCommentBegin%

        public async Task<bool> UpdateEEntity123PicturePathAsync(long eentity123Id, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<EEntity123>().Where(x => x.EEntity123Id == eentity123Id && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearEEntity123ProfilePathAsync(long eentity123Id, long? modifiedBy)
        {
            return await appDBManager.GetTable<EEntity123>().Where(x => x.EEntity123Id == eentity123Id && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableEEntity123ByIdAsync(long eentity123Id, long? modifiedBy)
        {
            return await appDBManager.GetTable<EEntity123>().Where(x => x.EEntity123Id == eentity123Id).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableEEntity123ByIdAsync(long eentity123Id, long? modifiedBy)
        {
            return await appDBManager.GetTable<EEntity123>().Where(x => x.EEntity123Id == eentity123Id).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateEEntity123WithXYZZYXAsync(long eentity123Id, long xyzzyxId, long? createdBy)
        {
            var item = new EEntity123XYZZYXAssociationModel();
            item.EEntity123Id = eentity123Id;
            item.XYZZYXId = xyzzyxId;

            return (await appDBManager.InsertWithAuditAsync<EEntity123XYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateEEntity123WithXYZZYXAsync(long eentity123Id, long xyzzyxId)
        {
            return await appDBManager.DeleteAsync<EEntity123XYZZYX>(x => x.EEntity123Id == eentity123Id && x.XYZZYXId == xyzzyxId) > 0;
        }

        public Task<bool> DoesEEntity123WithXYZZYXAssociationExistsAsync(long eentity123Id, long xyzzyxId)
        {
            return appDBManager.ExistsAsync<EEntity123XYZZYX>(x => x.EEntity123Id == eentity123Id && x.XYZZYXId == xyzzyxId);
        }

        public Task<List<EEntity123View>> GetEEntity123sByXYZZYXAsync(long xyzzyxId)
        {
            var eentity123Ids = GetEEntity123XYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xyzzyxId).Select(x => x.EEntity123Id);
            var eentity123Sql = GetEEntity123sViewAsIQueryable().Where(x => eentity123Ids.Contains(x.EEntity123Id));
            return eentity123Sql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByEEntity123sAsync(long eentity123Id)
        {
            var xyzzyxIds = GetEEntity123XYZZYXsViewAsIQueryable().Where(x => x.EEntity123Id == eentity123).Select(x => x.XYZZYXId);
            var xyzzyxSql = GetXYZZYXsViewAsIQueryable().Where(x => xyzzyxIds.Contains(x.XYZZYXId));
            return xyzzyxSql.ToListAsync();
        }

        public IQueryable<EEntity123XYZZYX> GetEEntity123XYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<EEntity123XYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetEEntity123XYZZYXActivitiesAsync(long companyId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.CompanyId == companyId && x.ResourceTypeId == (int)ResourceTypes.EEntity123XYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        //%ParentCommentEnd%
        #endregion
