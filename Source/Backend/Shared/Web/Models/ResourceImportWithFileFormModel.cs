using Microsoft.AspNetCore.Http;

namespace Manifest.Web.Models
{
    public class ResourceImportWithFileFormModel
    {
        public IFormFile UploadedFile { get; set; }

        public long CompanyId { get; set; }
    }
}