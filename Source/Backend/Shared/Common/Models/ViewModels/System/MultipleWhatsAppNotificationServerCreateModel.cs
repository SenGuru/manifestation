#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class WhatsAppNotificationServerXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long WhatsAppNotificationServerId { get; set; }
    }
    */

    public class MultipleWhatsAppNotificationServerCreateModel
    {
        public List<WhatsAppNotificationServerCreateModel> NewWhatsAppNotificationServers { get; set; }
    }
}
#pragma warning restore CS0108, SA1502, S4015