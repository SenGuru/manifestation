#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    public class ScheduledJobHistoryCreateModel : ScheduledJobHistoryUpdateModel
    {
        protected long ScheduledJobHistoryId { get { return base.ScheduledJobHistoryId; } set { base.ScheduledJobHistoryId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015