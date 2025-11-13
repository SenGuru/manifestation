using System.Threading.Tasks;
using Framework.Infrastructure.Models.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.SearchCriteria.External;
using Manifest.Common.Models.ViewModels;
using Manifest.Common.Models.ViewModels.External.Logging;
using Manifest.Service.Admin;
using Manifest.Service.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace Manifest.Api.All.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public partial class BackendTestController : Controller
    {
        private readonly BackendTestService backendTestService;

        public BackendTestController(BackendTestService backendTestService)
        {
            this.backendTestService = backendTestService;
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "BackendTest" })]
        public async Task<IActionResult> ExportMovieSceneArtistsInJsonLFormat()
        {
            var fs = await backendTestService.ExportMovieSceneArtistsInJsonLFormat();
            return new FileStreamResult(fs.FileData, fs.ContentType)
            {
                FileDownloadName = fs.FileName
            };
        }
    }
}
