using Framework.Infrastructure.Models;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search.LogSearch;
using LogProcessor.Models;
using LogProcessor.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]/[action]")]
public class LogController : ControllerBase
{
    private readonly ILogger<LogController> logger;
    private readonly ILogRepository logRepository;
    private readonly IPerfLogRepository perfLogRepository;
    private readonly LogDispatcherService logDispatcherService;
    private readonly PerfLogService perfLogService;
    private readonly AppLogService appLogService;

    public LogController(ILogger<LogController> logger, ILogRepository logRepository, IPerfLogRepository perfLogRepository, LogDispatcherService logDispatcherService, PerfLogService perfLogService, AppLogService appLogService)
    {
        this.logger = logger;
        this.logRepository = logRepository;
        this.perfLogRepository = perfLogRepository;
        this.logDispatcherService = logDispatcherService;
        this.perfLogService = perfLogService;
        this.appLogService = appLogService;
    }

    [HttpGet]
    public ReturnListModel<AppLogDataModel> GetAppLogs([FromQuery] LogSearchCriteria logSearchCriteria)
    {
        return logRepository.GetAppLogs(logSearchCriteria);
    }

    [HttpGet]
    public ReturnListModel<PerfLogDataModel> GetPerfLogs([FromQuery] PerfLogSearchCriteria perfLogSearchCriteria)
    {
        return perfLogService.GetPerfLogs(perfLogSearchCriteria);
    }

    [HttpGet]
    public ReturnListModel<AppFunctionAggregateModel> GetAppFunctionAggregate([FromQuery] AppFunctionAggregateSearchCriteria searchCriteria)
    {
        return perfLogService.GetAppFunctionAggregate(searchCriteria);
    }

    [HttpGet]
    public ReturnModel<int> GetLastProcessedLineOfPerfLog([FromQuery] string fileFullName)
    {
        return perfLogService.GetLastProcessedLineOfPerfLog(fileFullName);
    }

    [HttpGet]
    public ReturnModel<int> GetLastProcessedLineOfAppLog([FromQuery] string fileFullName)
    {
        return appLogService.GetLastProcessedLineOfAppLog(fileFullName);
    }

    [HttpGet]
    public Task<ReturnListModel<string>> GetUniqueSeverityTypes()
    {
        return appLogService.GetUniqueSeverityTypes();
    }

    [HttpGet]
    public Task<ReturnListModel<string>> GetUniqueFunctionTypes()
    {
        return appLogService.GetUniqueFunctionTypes();
    }

    [HttpGet]
    public Task<ReturnListModel<string>> GetUniqueFileNames()
    {
        return appLogService.GetUniqueFileNames();
    }

    [HttpGet]
    public Task<ReturnListModel<string>> GetUniqueAppNames()
    {
        return appLogService.GetUniqueAppNames();
    }

    [HttpGet]
    public Task<ReturnListModel<string>> GetUniqueHttpVerbs()
    {
        return appLogService.GetUniqueHttpVerbs();
    }

    [HttpGet]
    public Task<ReturnListModel<string>> GetUniqueStatusCodes()
    {
        return appLogService.GetUniqueStatusCodes();
    }

    [HttpGet]
    public Task<ReturnListModel<string>> GetUniqueMachineNames()
    {
        return appLogService.GetUniqueMachineNames();
    }

    [HttpPost]
    public Task<ReturnModel<bool>> CreateAppLogs([FromBody] List<AppLogDataModel> appLogs)
    {
        return appLogService.CreateAppLogs(appLogs);
    }

    [HttpPost]
    public Task<ReturnModel<bool>> CreatePerfLogs([FromBody] List<PerfLogDataModel> perfLogs)
    {
        return perfLogService.CreatePerfLogs(perfLogs);
    }
}