#pragma warning disable CS0108, SA1502, S4015
namespace Manifest.Common.Models.ViewModels
{
    /*
    public class UserPermissionXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long UserPermissionId { get; set; }
    }
    */

    public class UserPermissionCreateModel : UserPermissionUpdateModel
    {
        protected long UserPermissionId { get { return base.UserPermissionId; } set { base.UserPermissionId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015