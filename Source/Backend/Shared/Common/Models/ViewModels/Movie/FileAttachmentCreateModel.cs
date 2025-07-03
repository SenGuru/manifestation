#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    public class FileAttachmentCreateModel : FileAttachmentUpdateModel
    {
        protected long FileAttachmentId { get { return base.FileAttachmentId; } set { base.FileAttachmentId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015