#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    public class VoiceNotificationServerCreateModel : VoiceNotificationServerUpdateModel
    {
        protected long VoiceNotificationServerId { get { return base.VoiceNotificationServerId; } set { base.VoiceNotificationServerId = value; } }
        //protected List<VoiceNotificationServerXYZZYXModel> ExistingVoiceNotificationServerXYZZYXModels {get {return base.ExistingVoiceNotificationServerXYZZYXModels;} set { base.ExistingVoiceNotificationServerXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015