#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    /*
    public class MessageToSendXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long MessageToSendId { get; set; }
    }
    */

    public class MessageToSendCreateModel : MessageToSendUpdateModel
    {
        protected long MessageToSendId { get { return base.MessageToSendId; } set { base.MessageToSendId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015