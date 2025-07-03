using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models;
using LogProcessor.Models;
using Microsoft.Extensions.Logging;
using Framework.Infrastructure.Models.Search.LogSearch;
using LiteDB;

namespace LogProcessor.Services;

public class AppLogService
{
    private readonly ILogger<AppLogService> logger;
    private readonly ILogRepository logRepository;
    private readonly LogSettingRepository logSettingRepository;
    private readonly LogDispatcherService logDispatcherService;

    public AppLogService(ILogger<AppLogService> logger, ILogRepository logRepository, LogDispatcherService logDispatcherService, LogSettingRepository logSettingRepository)
    {
        this.logger = logger;
        this.logRepository = logRepository;
        this.logDispatcherService = logDispatcherService;
        this.logSettingRepository = logSettingRepository;
    }

    public ReturnListModel<AppLogDataModel> GetAppLogs(LogSearchCriteria logSearchCriteria)
    {
        try
        {
            return logRepository.GetAppLogs(logSearchCriteria);
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error when getting app logs - {ex}");
            return ReturnListModel<AppLogDataModel>.Error(ex);
        }
    }

    public ReturnModel<int> GetLastProcessedLineOfAppLog(string fileFullName)
    {
        try
        {
            if (string.IsNullOrEmpty(fileFullName))
                return ReturnModel<int>.Error("Full file name is not valid");

            return ReturnModel<int>.Success(logSettingRepository.GetLastProcessedLine(fileFullName));
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error when getting last processed line of a app log file - {ex}");
            return ReturnModel<int>.Error(ex);
        }
    }

    public async Task<ReturnListModel<string>> GetUniqueSeverityTypes()
    {
        try
        {
            return ReturnListModel<string>.Success(await logRepository.GetUniqueSeverityTypes());
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error when getting severity types - {ex}");
            return ReturnListModel<string>.Error(ex);
        }
    }

    public async Task<ReturnListModel<string>> GetUniqueFunctionTypes()
    {
        try
        {
            return ReturnListModel<string>.Success(await logRepository.GetUniqueFunctionTypes());
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error when getting function types - {ex}");
            return ReturnListModel<string>.Error(ex);
        }
    }

    public async Task<ReturnListModel<string>> GetUniqueFileNames()
    {
        try
        {
            return ReturnListModel<string>.Success(await logRepository.GetUniqueFileNames());
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error when getting unique file names - {ex}");
            return ReturnListModel<string>.Error(ex);
        }
    }

    public async Task<ReturnListModel<string>> GetUniqueAppNames()
    {
        try
        {
            return ReturnListModel<string>.Success(await logRepository.GetUniqueAppNames());
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error when getting unique file names - {ex}");
            return ReturnListModel<string>.Error(ex);
        }
    }

    public async Task<ReturnListModel<string>> GetUniqueHttpVerbs()
    {
        try
        {
            return ReturnListModel<string>.Success(await logRepository.GetUniqueHttpVerbs());
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error when getting unique file names - {ex}");
            return ReturnListModel<string>.Error(ex);
        }
    }

    public async Task<ReturnListModel<string>> GetUniqueStatusCodes()
    {
        try
        {
            return ReturnListModel<string>.Success(await logRepository.GetUniqueStatusCodes());
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error when getting unique file names - {ex}");
            return ReturnListModel<string>.Error(ex);
        }
    }

    public async Task<ReturnListModel<string>> GetUniqueMachineNames()
    {
        try
        {
            return ReturnListModel<string>.Success(await logRepository.GetUniqueMachineNames());
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error when getting unique file names - {ex}");
            return ReturnListModel<string>.Error(ex);
        }
    }

    public async Task<ReturnModel<bool>> CreateAppLogs(List<AppLogDataModel> appLogs)
    {
        try
        {
            if (appLogs == null)
                return ReturnModel<bool>.Error("App logs is empty");

            await logRepository.SaveToLogDatabase(appLogs);
            return ReturnModel<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error when creating app logs - {ex}");
            return ReturnModel<bool>.Error(ex);
        }
    }

    public async Task<ReturnModel<bool>> CreateAppLogFileReadInfoEntry(AppLogFileReadInfoModel appLogFileReadInfoModel)
    {
        try
        {
            await logSettingRepository.SaveToFileDatabase(appLogFileReadInfoModel.filePath, appLogFileReadInfoModel.LastLine, appLogFileReadInfoModel.LineCount);
            return ReturnModel<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error when creating app log file read info - {ex}");
            return ReturnModel<bool>.Error(ex);
        }
    }
}
