#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    /*
    public class EnumTypeXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long EnumTypeId { get; set; }
    }
    */

    public class EnumTypeCreateModel : EnumTypeUpdateModel
    {
        protected long EnumTypeId { get { return base.EnumTypeId; } set { base.EnumTypeId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015