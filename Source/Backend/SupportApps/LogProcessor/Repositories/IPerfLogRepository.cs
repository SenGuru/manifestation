using Framework.Infrastructure.Models;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search.LogSearch;
using LogProcessor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPerfLogRepository
{
    ReturnListModel<AppFunctionAggregateModel> GetAppFunctionAggregate(AppFunctionAggregateSearchCriteria searchCriteria);
    ReturnListModel<PerfLogDataModel> GetPerfLogs(PerfLogSearchCriteria perfLogSearchCriteria);
    Task SaveToLogDatabase(List<PerfLogDataModel> logDataList);
}