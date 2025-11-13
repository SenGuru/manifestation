using Microsoft.AspNetCore.Http;

namespace Manifest.Web.Models
{
    public class UserPictureWithFileFormModel
    {
        public long UserId { get; set; }

        public IFormFile UploadedFile { get; set; }
    }
}
