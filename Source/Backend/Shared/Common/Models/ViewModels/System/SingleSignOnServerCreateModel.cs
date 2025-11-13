#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    /*
    public class SingleSignOnServerXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long SingleSignOnServerId { get; set; }
    }
    */

    public class SingleSignOnServerCreateModel : SingleSignOnServerUpdateModel
    {
        protected long SingleSignOnServerId { get { return base.SingleSignOnServerId; } set { base.SingleSignOnServerId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015