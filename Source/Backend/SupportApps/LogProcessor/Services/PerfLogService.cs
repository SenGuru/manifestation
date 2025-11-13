using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Framework.Infrastructure.Models.Search.LogSearch;
using System;
using Framework.Infrastructure.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogProcessor.Models;
using System.Linq;

namespace LogProcessor.Services;

public class PerfLogService
{
    private readonly ILogger<PerfLogService> logger;
    private readonly IPerfLogRepository perfLogRepository;
    private readonly PerfLogSettingRepository perfLogSettingRepository;
    private readonly LogDispatcherService logDispatcherService;

    public PerfLogService(ILogger<PerfLogService> logger, IPerfLogRepository perfLogRepository, LogDispatcherService logDispatcherService, PerfLogSettingRepository perfLogSettingRepository)
    {
        this.logger = logger;
        this.perfLogRepository = perfLogRepository;
        this.logDispatcherService = logDispatcherService;
        this.perfLogSettingRepository = perfLogSettingRepository;
    }

    public ReturnListModel<PerfLogDataModel> GetPerfLogs(PerfLogSearchCriteria perfLogSearchCriteria)
    {
        try
        {
            var perfLogs =  perfLogRepository.GetPerfLogs(perfLogSearchCriteria);
            perfLogs.Model.Where(x => x.ElapsedTimeInMs != null).ForEach(perfLog => perfLog.ElapsedTimeInSeconds = perfLog.ElapsedTimeInMs / 1000);
            return perfLogs;
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error when getting perf logs - {ex}");
            return ReturnListModel<PerfLogDataModel>.Error(ex);
        }
    }

    public ReturnListModel<AppFunctionAggregateModel> GetAppFunctionAggregate(AppFunctionAggregateSearchCriteria searchCriteria)
    {
        try
        {
            return perfLogRepository.GetAppFunctionAggregate(searchCriteria);
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error when getting app function aggregate - {ex}");
            return ReturnListModel<AppFunctionAggregateModel>.Error(ex);
        }
    }

    public ReturnModel<int> GetLastProcessedLineOfPerfLog(string fileFullName)
    {
        try
        {
            if (fileFullName.IsNullOrEmpty())
                return ReturnModel<int>.Error("Full file name is not valid");

            return ReturnModel<int>.Success(perfLogSettingRepository.GetLastProcessedLine(fileFullName));
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error when getting last processed line of a perf log file - {ex}");
            return ReturnModel<int>.Error(ex);
        }
    }

    public async Task<ReturnModel<bool>> CreatePerfLogs(List<PerfLogDataModel> perfLogs)
    {
        try
        {
            if (perfLogs.IsNullOrEmpty())
                return ReturnModel<bool>.Error("Perf logs is empty");

            await perfLogRepository.SaveToLogDatabase(perfLogs);
            return ReturnModel<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error when creating perf logs - {ex}");
            return ReturnModel<bool>.Error(ex);
        }
    }

    public async Task<ReturnModel<bool>> CreatePerfLogFileReadInfoEntry(PerfLogFileReadInfoModel perfLogFileReadInfoModel)
    {
        try
        {
            await perfLogSettingRepository.SaveToFileDatabase(perfLogFileReadInfoModel.filePath, perfLogFileReadInfoModel.LastLine, perfLogFileReadInfoModel.LineCount);
            return ReturnModel<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error when creating perf log file read info - {ex}");
            return ReturnModel<bool>.Error(ex);
        }
    }
}
