#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    public class JobAttachmentCreateModel : JobAttachmentUpdateModel
    {
        protected long JobAttachmentId { get { return base.JobAttachmentId; } set { base.JobAttachmentId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015