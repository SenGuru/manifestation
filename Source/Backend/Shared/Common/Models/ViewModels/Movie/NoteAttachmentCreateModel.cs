#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    public class NoteAttachmentCreateModel : NoteAttachmentUpdateModel
    {
        protected long NoteAttachmentId { get { return base.NoteAttachmentId; } set { base.NoteAttachmentId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015