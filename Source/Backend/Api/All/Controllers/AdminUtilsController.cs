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
using Swashbuckle.AspNetCore.Annotations;

namespace Manifest.Api.All.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public partial class AdminUtilsController : Controller
    {
        private readonly AdminService adminService;

        public AdminUtilsController(AdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "AdminUtils" })]
        public Task<ReturnListModel<AppLogModel>> GetAppLogs([FromQuery] AppLogSearchCriteria search)
        {
            return adminService.GetAppLogsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "AdminUtils" })]
        public Task<ReturnListModel<string>> GetLogUniqueSeverityTypes()
        {
            return adminService.GetLogUniqueSeverityTypesAsync();
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "AdminUtils" })]
        public Task<ReturnListModel<string>> GetLogUniqueFunctionTypes()
        {
            return adminService.GetLogUniqueFunctionTypesAsync();
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "AdminUtils" })]
        public Task<ReturnListModel<string>> GetLogUniqueFileNames()
        {
            return adminService.GetLogUniqueFunctionTypesAsync();
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "AdminUtils" })]
        public Task<ReturnListModel<PerfLogModel>> GetPerfLogs([FromQuery] PerfLogSearchCriteria search)
        {
            return adminService.GetPerfLogsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "AdminUtils" })]
        public Task<ReturnListModel<PerfLogModel>> GetAppFunctionAggregateLogs([FromQuery] AppFunctionAggregateSearchCriteria search)
        {
            return adminService.GetAppFunctionAggregateLogsAsync(search);
        }
    }
}
