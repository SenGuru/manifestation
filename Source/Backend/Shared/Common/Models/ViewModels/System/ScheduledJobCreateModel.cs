#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    /*
    public class ScheduledJobXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long ScheduledJobId { get; set; }
    }
    */

    public class ScheduledJobCreateModel : ScheduledJobUpdateModel
    {
        protected long ScheduledJobId { get { return base.ScheduledJobId; } set { base.ScheduledJobId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015