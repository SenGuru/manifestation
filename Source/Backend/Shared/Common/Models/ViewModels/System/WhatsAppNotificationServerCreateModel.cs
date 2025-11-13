#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    public class WhatsAppNotificationServerCreateModel : WhatsAppNotificationServerUpdateModel
    {
        protected long WhatsAppNotificationServerId { get { return base.WhatsAppNotificationServerId; } set { base.WhatsAppNotificationServerId = value; } }
        //protected List<WhatsAppNotificationServerXYZZYXModel> ExistingWhatsAppNotificationServerXYZZYXModels {get {return base.ExistingWhatsAppNotificationServerXYZZYXModels;} set { base.ExistingWhatsAppNotificationServerXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015