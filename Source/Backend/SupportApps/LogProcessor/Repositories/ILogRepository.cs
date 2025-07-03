using Framework.Infrastructure.Models;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search.LogSearch;
using LogProcessor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ILogRepository
{
    ReturnListModel<AppLogDataModel> GetAppLogs(LogSearchCriteria logSearchCriteria);
    Task<List<string>> GetUniqueFileNames();
    Task<List<string>> GetUniqueFunctionTypes();
    Task<List<string>> GetUniqueSeverityTypes();
    Task<List<string>> GetUniqueAppNames();
    Task<List<string>> GetUniqueHttpVerbs();
    Task<List<string>> GetUniqueStatusCodes();
    Task<List<string>> GetUniqueMachineNames();
    Task SaveToLogDatabase(List<AppLogDataModel> logDataList);
}