using Manifest.Common.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Manifest.Web.Models
{
    public class FileAttachmentCreateWithFileFormModel : FileAttachmentCreateModel
    {
        public IFormFile UploadedFile { get; set; }
    }
}
