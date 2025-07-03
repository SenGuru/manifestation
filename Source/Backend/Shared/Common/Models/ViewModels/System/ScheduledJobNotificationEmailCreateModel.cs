#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    /*
    public class ScheduledJobNotificationEmailXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long ScheduledJobNotificationEmailId { get; set; }
    }
    */

    public class ScheduledJobNotificationEmailCreateModel : ScheduledJobNotificationEmailUpdateModel
    {
        protected long ScheduledJobNotificationEmailId { get { return base.ScheduledJobNotificationEmailId; } set { base.ScheduledJobNotificationEmailId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015