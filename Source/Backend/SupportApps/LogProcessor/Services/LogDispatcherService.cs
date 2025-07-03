using AspNetCore.Http.Extensions;
using Framework.Infrastructure.Models;
using Framework.Infrastructure.Models.Result;
using LogProcessor.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LogProcessor.Services;

public class LogDispatcherService
{
    private readonly ILogger<LogDispatcherService> logger;
    private readonly string logCentralizedServerUrl;

    public LogDispatcherService(ConfigurationService configuration, ILogger<LogDispatcherService> logger)
    {
        this.logger = logger;
        this.logCentralizedServerUrl = configuration.LogCentralizedServerUrl;
    }

    public async Task<ReturnModel<bool>> CreateAppLogsAsync(List<AppLogDataModel> appLogs)
    {
        using (HttpClient httpClient = new())
        {
            try
            {
                var response = await httpClient.PostAndReadAsJsonAsync<ReturnModel<bool>, List<AppLogDataModel>>(logCentralizedServerUrl + "api/log/CreateAppLogs", appLogs);

                if (!response.msg.IsSuccessStatusCode)
                {
                    logger.LogError($"Error getting the App Logs - Status Code - " + response.msg.StatusCode.ToString());
                    return ReturnModel<bool>.Error(response.msg.StatusCode.ToString());
                }

                if (!response.result.IsSuccess)
                {
                    logger.LogError($"Error getting the App Logs - " + response.result.ErrorHolder.AllErrorAsString());
                    return ReturnModel<bool>.Error(response.result.ErrorHolder.AllErrorAsString());
                }

                return ReturnModel<bool>.Success(response.result.Model);
            }
            catch (Exception ex)
            {
                logger.LogError($"Unable to get App Logs, Message - {ex.Message} - ST - {ex.StackTrace}");
                return ReturnModel<bool>.Error("Unable to get App Logs");
            }
        }
    }

    public async Task<ReturnModel<bool>> CreatePerfLogsAsync(List<PerfLogDataModel> perfLogs)
    {
        using (HttpClient httpClient = new())
        {
            try
            {
                var response = await httpClient.PostAndReadAsJsonAsync<ReturnModel<bool>, List<PerfLogDataModel>>(logCentralizedServerUrl + "api/log/CreatePerfLogs", perfLogs);

                if (!response.msg.IsSuccessStatusCode)
                {
                    logger.LogError($"Error getting the Perf Logs - Status Code - " + response.msg.StatusCode.ToString());
                    return ReturnModel<bool>.Error(response.msg.StatusCode.ToString());
                }

                if (!response.result.IsSuccess)
                {
                    logger.LogError($"Error getting the Perf Logs - " + response.result.ErrorHolder.AllErrorAsString());
                    return ReturnModel<bool>.Error(response.result.ErrorHolder.AllErrorAsString());
                }

                return ReturnModel<bool>.Success(response.result.Model);
            }
            catch (Exception ex)
            {
                logger.LogError($"Unable to get Perf Logs, Message - {ex.Message} - ST - {ex.StackTrace}");
                return ReturnModel<bool>.Error("Unable to get Perf Logs");
            }
        }
    }
}
