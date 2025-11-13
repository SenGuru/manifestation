#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    public class UserGroupCreateModel : UserGroupUpdateModel
    {
        protected long UserGroupId { get { return base.UserGroupId; } set { base.UserGroupId = value; } }
        //protected List<UserGroupXYZZYXModel> ExistingUserGroupXYZZYXModels {get {return base.ExistingUserGroupXYZZYXModels;} set { base.ExistingUserGroupXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015