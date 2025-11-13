#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    public class PushNotificationServerCreateModel : PushNotificationServerUpdateModel
    {
        protected long PushNotificationServerId { get { return base.PushNotificationServerId; } set { base.PushNotificationServerId = value; } }
        //protected List<PushNotificationServerXYZZYXModel> ExistingPushNotificationServerXYZZYXModels {get {return base.ExistingPushNotificationServerXYZZYXModels;} set { base.ExistingPushNotificationServerXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015