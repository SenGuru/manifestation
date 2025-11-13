using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Models.ViewModels;
using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;

namespace Manifest.Service.App
{
    public partial class PermissionService : BaseAppService
    {
        private readonly Dictionary<(PredefinedUserPermissionActionTypes, PredefinedUserPermissionModuleTypes), PermissionRequirementModel> permissionMap;
        private readonly ICurrentSessionService currentSessionService;

        public PermissionService(
            ILog log,
            ObjectMapperService objectMapperService,
            ICurrentSessionService currentSessionService)
            : base(log, objectMapperService)
        {
            this.permissionMap = new Dictionary<(PredefinedUserPermissionActionTypes, PredefinedUserPermissionModuleTypes), PermissionRequirementModel>();
            this.currentSessionService = currentSessionService;

            this.PopulatePermissions();
        }

        public async Task<bool> IsUserSuperAdmin()
        {
            if (await currentSessionService.GetCurrentUserTypeIdAsync() == (int)PredefinedUserTypes.SuperAdmin)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsUserCompanyAdmin()
        {
            if (await currentSessionService.GetCurrentUserTypeIdAsync() == (int)PredefinedUserTypes.Manager)
            {
                return true;
            }

            return false;
        }

        public async Task<(bool resultValue, bool canAssign, string errorStr)> CanManageAppSetupAsync()
        {
            if (await currentSessionService.IsCurrentUserASuperAdminAsync())
            {
                return (true, false, "");
            }

            return (false, false, "");
        }

        public async Task<(bool resultValue, bool canAssign, string errorStr)> CanManageAppSetupAsync(long companyId)
        {
            if (await currentSessionService.IsCurrentUserASuperAdminAsync())
            {
                return (true, false, "");
            }

            return (false, false, "");
        }

        public async Task<(bool resultValue, bool canAssign, string errorStr)> HaveCompanyWithPermissionLevelAsync(long companyId, params PredefinedUserPermissionTypes[] permission)
        {
            if (await currentSessionService.IsCurrentUserASuperAdminAsync())
            {
                return (true, false, "");
            }

            if (permission == null || permission.Length == 0)
            {
                return (false, false, "Permission should be specified");
            }

            if (currentSessionService.DoesUserHavePermission(permission))
            {
                return (true, true, null);
            }

            var errorString = "You don't have the permission to View this resource";
            if (permission.FirstOrDefault() == PredefinedUserPermissionTypes.ConfigureCompany)
            {
                errorString = "You don't have the permission to Manage this resource";
            }

            return (false, false, errorString);
        }

        public async Task<(bool resultValue, bool canAssign, string errorStr)> HaveCompanyAndResourceWithPermissionLevelAsync(long companyId, long resourceId, params PredefinedUserPermissionTypes[] permission)
        {
            if (await currentSessionService.IsCurrentUserASuperAdminAsync())
            {
                return (true, false, "");
            }

            if (permission == null || permission.Length == 0)
            {
                return (false, false, "Permission should be specified");
            }

            if (currentSessionService.DoesUserHavePermission(permission))
            {
                return (true, true, "");
            }

            return (false, false, "You don't have the permission to do this action");
        }

        public async Task<(bool resultValue, List<long> permittedResourceIds, string errorStr)> GetPermittedResourceIds(long companyId, PredefinedUserPermissionActionTypes actionType, PredefinedUserPermissionModuleTypes moduleType)
        {
            var isSuperAdmin = await IsUserSuperAdmin();
            if (isSuperAdmin)
            {
                return (true, null, string.Empty);
            }

            var resourceIdsOfCurrentUser = await currentSessionService.GetCurrentUserResourceIdsAsync() ?? Enumerable.Empty<long>();

            var accessLevelStr = actionType == PredefinedUserPermissionActionTypes.View ? "View" : "Manage";

            if (!resourceIdsOfCurrentUser.Any())
            {
                return (false, null, $"User does not have permission to {accessLevelStr} {moduleType.ToString().SplitCamelCase()} resource");
            }

            var permittedResourceIds = new List<long>();

            foreach (var resourceId in resourceIdsOfCurrentUser)
            {
                var (hasPermission, _, _) = await HaveResourcePermissionsAsync(
                    resourceId,
                    0,
                    actionType,
                    moduleType);

                if (hasPermission)
                {
                    permittedResourceIds.Add(resourceId);
                }
            }

            if (!permittedResourceIds.Any())
            {
                return (false, null, $"User does not have permission to {accessLevelStr} {moduleType.ToString().SplitCamelCase()} resource");
            }

            return (true, permittedResourceIds, string.Empty);
        }

        public async Task<(bool resultValue, bool canAssign, string errorStr)> HaveResourcePermissionsAsync(long resourceId, long companyId, PredefinedUserPermissionActionTypes actionType, PredefinedUserPermissionModuleTypes moduleType)
        {
            if (await currentSessionService.IsCurrentUserASuperAdminAsync())
            {
                return (true, false, "");
            }

            if (resourceId <= 0)
            {
                return (false, false, "Invalid resource information");
            }

            var accessLevelStr = actionType == PredefinedUserPermissionActionTypes.View ? "viewing" : "managing";

            var permissions = GetPermissionRequirement(actionType, moduleType);
            var userPermissions = await currentSessionService.GetUserPermissionsByResourceIdAsync(resourceId);

            if (userPermissions == null || userPermissions.Count == 0)
            {
                return (false, false, $"You don't have the required permissions for {accessLevelStr} {moduleType.ToString().SplitCamelCase()}");
            }

            foreach (var permissionSet in permissions.RequiredPermissions)
            {
                bool hasAnyPermissionInSet = false;

                foreach (var permission in permissionSet)
                {
                    if (userPermissions.Select(x => x.PredefinedUserPermissionTypeId).Contains((int)permission))
                    {
                        hasAnyPermissionInSet = true;
                        break;
                    }
                }

                if (!hasAnyPermissionInSet)
                {
                    return (false, false, $"You don't have the required permissions for {accessLevelStr} {moduleType.ToString().SplitCamelCase()}");
                }
            }

            return (true, true, "");
        }

        public PermissionRequirementModel GetPermissionRequirement(PredefinedUserPermissionActionTypes action, PredefinedUserPermissionModuleTypes module)
        {
            if (!permissionMap.TryGetValue((action, module), out var requirement))
            {
                throw new NotSupportedException($"Permission mapping not found for {module}/{action}");
            }

            return requirement;
        }

        public void PopulatePermissions()
        {
            // Example permission mappings - customize based on application needs
            permissionMap[(PredefinedUserPermissionActionTypes.View, PredefinedUserPermissionModuleTypes.Company)] =
                new PermissionRequirementModel(new List<PredefinedUserPermissionTypes[]>
                {
                    new[] { PredefinedUserPermissionTypes.ViewCompany, PredefinedUserPermissionTypes.ConfigureCompany }
                });

            permissionMap[(PredefinedUserPermissionActionTypes.Manage, PredefinedUserPermissionModuleTypes.Company)] =
                new PermissionRequirementModel(new List<PredefinedUserPermissionTypes[]>
                {
                    new[] { PredefinedUserPermissionTypes.ConfigureCompany }
                });

            // Add other permission mappings as needed
        }

        public async Task<(bool resultValue, bool canAssign, string errorStr)> DoesBelongToCompanyAsync(long companyId)
        {
            if (await currentSessionService.IsCurrentUserASuperAdminAsync())
            {
                return (true, false, "");
            }

            return (true, true, "");
        }

        public async Task<(bool resultValue, bool canAssign, string errorStr)> CanModifyResource(long userId)
        {
            if (!await currentSessionService.IsCurrentUserACompanyAdminAsync())
            {
                if (userId != await currentSessionService.GetCurrentUserIdAsync())
                {
                    return (false, false, "This resource does not belong to you");
                }
            }
            
            return (true, !await currentSessionService.IsCurrentUserASuperAdminAsync(), null);
        }

    }
}