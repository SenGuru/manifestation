#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    /*
    public class JobNotificationEmailXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long JobNotificationEmailId { get; set; }
    }
    */

    public class JobNotificationEmailCreateModel : JobNotificationEmailUpdateModel
    {
        protected long JobNotificationEmailId { get { return base.JobNotificationEmailId; } set { base.JobNotificationEmailId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015