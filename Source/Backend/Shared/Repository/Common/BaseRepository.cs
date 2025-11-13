using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Repository.DbAccess;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.DBView;
using Manifest.Common.Models.SearchCriteria;
using Framework.Data.Utils;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;

namespace Manifest.Repository.Common
{
    public class BaseRepository
    {
        protected readonly IAppDBManager appDBManager;

        public BaseRepository(IAppDBManager appDBManager)
        {
            this.appDBManager = appDBManager;
        }

        public IQueryable<EnumType> GetEnumQueryableOfPredefinedAuditOperationType()
        {
            return appDBManager.GetTable<EnumType>().Where(x => x.EnumGroupId == (int)EnumGroupTypes.PredefinedAuditOperationType);
        }

        public IQueryable<EnumType> GetEnumQueryableOfPredefinedUserType()
        {
            return appDBManager.GetTable<EnumType>().Where(x => x.EnumGroupId == (int)EnumGroupTypes.PredefinedUserType);
        }

        public IQueryable<EnumType> GetEnumQueryableOfPredefinedUserPermissionType()
        {
            return appDBManager.GetTable<EnumType>().Where(x => x.EnumGroupId == (int)EnumGroupTypes.PredefinedUserPermissionType);
        }

        public IQueryable<EnumType> GetEnumQueryableOfPredefinedResourceType()
        {
            return appDBManager.GetTable<EnumType>().Where(x => x.EnumGroupId == (int)EnumGroupTypes.PredefinedResourceType);
        }

        public Task<DbReturnListModel<ActivityView>> GetActivitiesByExpressionAsync(Expression<Func<ActivityView, bool>> predicate, ActivitySearchCriteria search)
        {
            var sql1 = from item in appDBManager.GetTable<AuditOperation>()
                       join at in GetEnumQueryableOfPredefinedAuditOperationType() on item.PredefinedAuditOperationTypeId equals at.EnumTypeId
                       join ob in appDBManager.GetTable<User>() on item.UserId equals ob.UserId into bTemp
                       from b in bTemp.DefaultIfEmpty()
                       join od in appDBManager.GetTable<User>() on item.UserId equals od.UserId into dTemp
                       from d in dTemp.DefaultIfEmpty()
                       join of in GetEnumQueryableOfPredefinedUserType() on d.PredefinedUserTypeId equals of.EnumTypeId into fTemp
                       from f in fTemp.DefaultIfEmpty()
                       join or in GetEnumQueryableOfPredefinedResourceType() on item.PredefinedResourceTypeId equals or.EnumTypeId into rTemp
                       from r in rTemp.DefaultIfEmpty()
                       join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                       from cu in cuTemp.DefaultIfEmpty()
                       join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                       from mu in muTemp.DefaultIfEmpty()
                       select new ActivityView
                       {
                           CreatedByUserFirstName = cu.FirstName,
                           CreatedByUserLastName = cu.LastName,
                           CreatedByUserEmail = cu.Email,
                           ModifiedByUserFirstName = mu.FirstName,
                           ModifiedByUserLastName = mu.LastName,
                           ModifiedByUserEmail = mu.Email,
                           PredefinedResourceTypeId = item.PredefinedResourceTypeId,
                           PredefinedAuditOperationTypeId = item.PredefinedAuditOperationTypeId,
                           PredefinedResourceTypeName = r.EnumTypeName,
                           ResourceId = item.ResourceId,
                           UserGroupId = item.UserGroupId,
                           UserGroupUserId = item.UserGroupUserId,
                           UserPermissionId = item.UserPermissionId,
                           //MovieName = e.Title,
                           ChangeDiff = item.ChangeDiff,
                           AuditOperationTypeName = at.EnumTypeName,
                           AuditOperationId = item.AuditOperationId,
                           AuditDateTime = item.AuditDateTime,
                           UserId = item.UserId,
                           UserFirstName = d.FirstName,
                           UserLastName = d.LastName,
                           UserTypeName = f.EnumTypeName,
                           IpAddress = item.IpAddress,
                           Description = item.Description,
                           IsDeleted = item.IsDeleted,
                           CreatedDate = item.CreatedDate,
                           CreatedByUserId = item.CreatedByUserId,
                           ModifiedDate = item.ModifiedDate,
                           ModifiedByUserId = item.ModifiedByUserId,
                           BrowserName = item.BrowserName,
                           DeviceName = item.DeviceName,
                           OperatingSystemName = item.OperatingSystemName,
                       };

            if (!search.PredefinedAuditOperationTypeIds.IsNullOrEmpty())
            {
                sql1 = sql1.Where(x => search.PredefinedAuditOperationTypeIds.Contains(x.PredefinedAuditOperationTypeId));
            }

            sql1 = sql1.Where(predicate);

            return sql1.ReturnListModelResultAsync(search);
        }
    }
}
