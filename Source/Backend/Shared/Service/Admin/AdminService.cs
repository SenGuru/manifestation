using System.Threading.Tasks;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Models.SearchCriteria.External;
using Manifest.Common.Models.ViewModels.External.Logging;
using Manifest.Service.App;
using Manifest.Service.ObjectMapping;
using Manifest.Service.Utils;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;

namespace Manifest.Service.Admin
{
    public class AdminService : BaseAppService
    {
        private readonly ICurrentSessionService currentSessionService;
        private readonly LogServerClientService logServerClientService;

        public AdminService(ILog log, ObjectMapperService objectMapperService, ICurrentSessionService currentSessionService, LogServerClientService logServerClientService)
            : base(log, objectMapperService)
        {
            this.currentSessionService = currentSessionService;
            this.logServerClientService = logServerClientService;
        }

        public async Task<ReturnListModel<AppLogModel>> GetAppLogsAsync(AppLogSearchCriteria search)
        {
            if (!await currentSessionService.IsCurrentUserASuperAdminAsync())
                ReturnListModel<AppLogModel>.Error("You need to be a super admin to access this function");

            return await logServerClientService.GetAppLogsAsync(search);
        }

        public async Task<ReturnListModel<string>> GetLogUniqueSeverityTypesAsync()
        {
            if (!await currentSessionService.IsCurrentUserASuperAdminAsync())
                ReturnListModel<string>.Error("You need to be a super admin to access this function");

            return await logServerClientService.GetUniqueSeverityTypesAsync();
        }

        public async Task<ReturnListModel<string>> GetLogUniqueFunctionTypesAsync()
        {
            if (!await currentSessionService.IsCurrentUserASuperAdminAsync())
                ReturnListModel<string>.Error("You need to be a super admin to access this function");

            return await logServerClientService.GetUniqueFunctionTypesAsync();
        }

        public async Task<ReturnListModel<string>> GetLogUniqueFileNamesAsync()
        {
            if (!await currentSessionService.IsCurrentUserASuperAdminAsync())
                ReturnListModel<string>.Error("You need be to a super admin to access this function");

            return await logServerClientService.GetUniqueFileNamesAsync();
        }

        public async Task<ReturnListModel<PerfLogModel>> GetPerfLogsAsync(PerfLogSearchCriteria search)
        {
            if (!await currentSessionService.IsCurrentUserASuperAdminAsync())
                ReturnListModel<PerfLogModel>.Error("You need to be a super admin to access this function");

            return await logServerClientService.GetPerfLogsAsync(search);
        }

        public async Task<ReturnListModel<PerfLogModel>> GetAppFunctionAggregateLogsAsync(AppFunctionAggregateSearchCriteria search)
        {
            if (!await currentSessionService.IsCurrentUserASuperAdminAsync())
                ReturnListModel<PerfLogModel>.Error("You need to be a super admin to access this function");

            return await logServerClientService.GetAppFunctionAggregateAsync(search);
        }
    }
}
