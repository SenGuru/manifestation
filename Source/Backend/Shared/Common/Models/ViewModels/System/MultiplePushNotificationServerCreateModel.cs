#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class PushNotificationServerXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long PushNotificationServerId { get; set; }
    }
    */

    public class MultiplePushNotificationServerCreateModel
    {
        public List<PushNotificationServerCreateModel> NewPushNotificationServers { get; set; }
    }
}
#pragma warning restore CS0108, SA1502, S4015