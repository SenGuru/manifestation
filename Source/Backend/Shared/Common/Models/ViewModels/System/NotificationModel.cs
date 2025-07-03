#pragma warning disable S2094 // Classes should not be empty
namespace Manifest.Common.Models.ViewModels
{
    public class NotificationModel
    {
        public long MovieShootDayResourceId { get; set; }

        public long CompanyId { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public long PredefinedPhoneCallNotificationStatusTypeId { get; set; }

        public long PredefinedEmailNotificationStatusTypeId { get; set; }

        public long PredefinedSmsNotificationStatusTypeId { get; set; }

        public long PredefinedPushNotificationStatusTypeId { get; set; }

        public long PredefinedWhatsAppNotificationStatusTypeId { get; set; }

        public bool IsNotificationStarted { get; set; }

        public bool IsNotificationCompleted { get; set; }
    }
}
#pragma warning restore S2094 // Classes should not be empty