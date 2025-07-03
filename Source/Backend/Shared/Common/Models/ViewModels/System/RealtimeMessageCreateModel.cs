#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    /*
    public class RealtimeMessageXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long RealtimeMessageId { get; set; }
    }
    */

    public class RealtimeMessageCreateModel : RealtimeMessageUpdateModel
    {
        protected long RealtimeMessageId { get { return base.RealtimeMessageId; } set { base.RealtimeMessageId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015