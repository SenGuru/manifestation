using Manifest.Common.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Manifest.Web.Models
{
    public class UserCreateWithFileFormModel : UserCreateModel
    {
        public IFormFile UploadedFile { get; set; }
    }
}
