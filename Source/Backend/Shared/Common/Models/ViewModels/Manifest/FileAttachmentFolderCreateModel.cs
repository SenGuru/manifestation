#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    public class FileAttachmentFolderCreateModel : FileAttachmentFolderUpdateModel
    {
        protected long FileAttachmentFolderId { get { return base.FileAttachmentFolderId; } set { base.FileAttachmentFolderId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015