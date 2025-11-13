using Manifest.Common.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Manifest.Web.Models
{
    public class VisionBoardPictureUpdateWithFileFormModel : VisionBoardPictureUpdateModel
    {
        [Required]
        public IFormFile UploadedFile { get; set; }
    }
}
