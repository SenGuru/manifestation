using Framework.Infrastructure.Exceptions;
using Framework.Infrastructure.Interfaces.DbAccess;
using Framework.Infrastructure.Interfaces.Helpers;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using %AppName%.Common.Enums;
using %AppName%.Common.Models.DB;
using %AppName%.Common.Models.DBView;
using %AppName%.Common.Models.ViewModels;
using %AppName%.Common.Models.SearchCriteria;
using %AppName%.Repository.Common;
using %AppName%.Service.App;
using %AppName%.Service.ObjectMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.AccessControl;

namespace %AppName%.Service.App
{
    public partial class PermissionService
    {
        #region EEntity123

        public async Task<(bool, bool, string)> CanCreateEEntity123Async()
        {
            if (!await DoesUserHavePermissionCurrentRole(UserPermissionTypes.ManageContract, UserPermissionTypes.ConfigureSystem, UserPermissionTypes.ConfigureCompany))
                return (false, false, "No permission to create this resource");

            if (!(await currentSessionService.GetCurrentUserCurrentRoleAsync() == (int)UserRoleTypes.SuperAdmin))
            {
                return (true, true, "");
            }
            else
            {
                return (true, false, "");
            }
        }

        public async Task<(bool, bool, string)> GetCanManageEEntity123PermissionParamsAsync(long? companyId = null)
        {
            return await GetCanManageEEntity123PermissionParamsInternalAsync(companyId, UserPermissionTypes.ManageContract, UserPermissionTypes.ConfigureSystem, UserPermissionTypes.ConfigureCompany);
        }

        public async Task<(bool, bool, string)> GetCanViewEEntity123PermissionParamsAsync(long? companyId = null)
        {
            return await GetCanManageEEntity123PermissionParamsInternalAsync(companyId, UserPermissionTypes.ViewContract, UserPermissionTypes.ManageContract, UserPermissionTypes.ConfigureSystem, UserPermissionTypes.ConfigureCompany);
        }

        private async Task<(bool, bool, string)> GetCanManageEEntity123PermissionParamsInternalAsync(long? companyId, params UserPermissionTypes[] permissions)
        {
            var (resultValue, canAssign, errorStr, ignoreFurtherCheck) = await GetCompanyParamsValidationAsync(companyId);

            if (resultValue == false)
                return (resultValue, canAssign, errorStr);

            if (ignoreFurtherCheck)
                return (resultValue, canAssign, errorStr);

            if (!await DoesUserHavePermissionCurrentRole(permissions))
                return (false, false, "No permission to access this resource");

            return (resultValue, canAssign, errorStr);
        }
        #endregion

    }
}
