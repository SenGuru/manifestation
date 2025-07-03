using Manifest.Common.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Manifest.Web.Models
{
    public class UserUpdateWithFileFormModel : UserUpdateModel
    {
        public IFormFile UploadedFile { get; set; }
    }
}
