#pragma warning disable S2094 // Classes should not be empty
using System.Collections.Generic;
using Manifest.Common.Enums;

namespace Manifest.Common.Models.ViewModels
{
    public class PermissionRequirementModel
    {
        public PermissionRequirementModel(List<PredefinedUserPermissionTypes[]> requiredPermissions)
        {
            RequiredPermissions = requiredPermissions;
        }

        public List<PredefinedUserPermissionTypes[]> RequiredPermissions { get; private set; }
    }
}
#pragma warning restore S2094 // Classes should not be empty