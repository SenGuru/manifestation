#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    /*
    public class EnumGroupXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long EnumGroupId { get; set; }
    }
    */

    public class EnumGroupCreateModel : EnumGroupUpdateModel
    {
        protected long EnumGroupId { get { return base.EnumGroupId; } set { base.EnumGroupId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015