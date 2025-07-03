#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    /*
    public class ConfigurationItemXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long ConfigurationItemId { get; set; }
    }
    */

    public class ConfigurationItemCreateModel : ConfigurationItemUpdateModel
    {
        protected long ConfigurationItemId { get { return base.ConfigurationItemId; } set { base.ConfigurationItemId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015