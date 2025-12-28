using Microsoft.AspNetCore.Http;

namespace Manifest.Web.Models
{
    public class VisionBoardPictureThumbnailWithFileFormModel
    {
        public long VisionBoardPictureId { get; set; }

        public IFormFile UploadedFile { get; set; }
    }
}