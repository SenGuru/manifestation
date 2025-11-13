using System;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.Http.Extensions;
using Manifest.Common.Interfaces.Service.Config;
using Manifest.Common.Models.SearchCriteria.External;
using Manifest.Common.Models.ViewModels.External.Logging;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;

namespace Manifest.Service.Utils
{
    public class LogServerClientService
    {
        protected readonly ILog log;
        private readonly string url;

        public LogServerClientService(ILog log, ICommonConfiguration commonConfiguration)
        {
            this.log = log;
            this.url = commonConfiguration.LocalLogServerUrl;
        }

        public async Task<ReturnListModel<string>> GetUniqueSeverityTypesAsync()
        {
            using (HttpClient httpClient = new ())
            {
                try
                {
                    var response = await httpClient.GetAndReadAsJsonAsync<ReturnListModel<string>>(url + "api/Log/GetUniqueSeverityTypes");

                    if (!response.msg.IsSuccessStatusCode)
                    {
                        return log.ErrorAndReturnListModel<string>($"Error getting the Severity Types", (int)response.msg.StatusCode);
                    }

                    if (response.result == null)
                    {
                        return log.ErrorAndReturnListModel<string>($"Unable to get Severity Types", (int)response.msg.StatusCode);
                    }

                    return ReturnListModel<string>.Success(response.result.Model, response.result.TotalRecords);
                }
                catch (Exception ex)
                {
                    return log.ErrorAndReturnListModel<string>($"Unable to get Severity Types", ex);
                }
            }
        }

        public async Task<ReturnListModel<string>> GetUniqueFunctionTypesAsync()
        {
            using (HttpClient httpClient = new ())
            {
                try
                {
                    var response = await httpClient.GetAndReadAsJsonAsync<ReturnListModel<string>>(url + "api/Log/GetUniqueFunctionTypes");

                    if (!response.msg.IsSuccessStatusCode)
                    {
                        return log.ErrorAndReturnListModel<string>($"Error getting the Function Types", (int)response.msg.StatusCode);
                    }

                    if (response.result == null)
                    {
                        return log.ErrorAndReturnListModel<string>($"Unable to get Function Types", (int)response.msg.StatusCode);
                    }

                    return ReturnListModel<string>.Success(response.result.Model, response.result.TotalRecords);
                }
                catch (Exception ex)
                {
                    return log.ErrorAndReturnListModel<string>($"Unable to get Function Types", ex);
                }
            }
        }

        public async Task<ReturnListModel<string>> GetUniqueFileNamesAsync()
        {
            using (HttpClient httpClient = new ())
            {
                try
                {
                    var response = await httpClient.GetAndReadAsJsonAsync<ReturnListModel<string>>(url + "api/Log/GetUniqueFileNames");

                    if (!response.msg.IsSuccessStatusCode)
                    {
                        return log.ErrorAndReturnListModel<string>($"Error getting the File Name", (int)response.msg.StatusCode);
                    }

                    if (response.result == null)
                    {
                        return log.ErrorAndReturnListModel<string>($"Unable to get File Name", (int)response.msg.StatusCode);
                    }

                    return ReturnListModel<string>.Success(response.result.Model, response.result.TotalRecords);
                }
                catch (Exception ex)
                {
                    return log.ErrorAndReturnListModel<string>($"Unable to get unique file names", ex);
                }
            }
        }

        public async Task<ReturnListModel<AppLogModel>> GetAppLogsAsync(AppLogSearchCriteria search)
        {
            using (HttpClient httpClient = new ())
            {
                try
                {
                    var response = await httpClient.GetAndReadAsJsonAsync<ReturnListModel<AppLogModel>>(HttpClientUtils.GetQueryString(search, url + "api/Log/GetAppLogs"));

                    if (!response.msg.IsSuccessStatusCode)
                    {
                        return log.ErrorAndReturnListModel<AppLogModel>($"Error getting the App Logs", (int)response.msg.StatusCode);
                    }

                    if (response.result == null)
                    {
                        return log.ErrorAndReturnListModel<AppLogModel>($"Unable to get App Log list", (int)response.msg.StatusCode);
                    }

                    return ReturnListModel<AppLogModel>.Success(response.result.Model, response.result.TotalRecords);
                }
                catch (Exception ex)
                {
                    return log.ErrorAndReturnListModel<AppLogModel>($"Unable to get App Logs", ex);
                }
            }
        }

        public async Task<ReturnListModel<PerfLogModel>> GetPerfLogsAsync(PerfLogSearchCriteria search)
        {
            using (HttpClient httpClient = new ())
            {
                try
                {
                    var response = await httpClient.GetAndReadAsJsonAsync<ReturnListModel<PerfLogModel>>(HttpClientUtils.GetQueryString(search, url + "api/log/GetPerfLogs"));

                    if (!response.msg.IsSuccessStatusCode)
                    {
                        return log.ErrorAndReturnListModel<PerfLogModel>($"Error getting the Perf Logs", (int)response.msg.StatusCode);
                    }

                    if (response.result == null)
                    {
                        return log.ErrorAndReturnListModel<PerfLogModel>($"Unable to get Perf Log list", (int)response.msg.StatusCode);
                    }

                    return ReturnListModel<PerfLogModel>.Success(response.result.Model, response.result.TotalRecords);
                }
                catch (Exception ex)
                {
                    return log.ErrorAndReturnListModel<PerfLogModel>($"Unable to get Perf Logs", ex);
                }
            }
        }

        public async Task<ReturnListModel<PerfLogModel>> GetAppFunctionAggregateAsync(AppFunctionAggregateSearchCriteria search)
        {
            using (HttpClient httpClient = new ())
            {
                try
                {
                    var response = await httpClient.GetAndReadAsJsonAsync<ReturnListModel<PerfLogModel>>(HttpClientUtils.GetQueryString(search, url + "api/log/AppFunctionAggregate"));

                    if (!response.msg.IsSuccessStatusCode)
                    {
                        return log.ErrorAndReturnListModel<PerfLogModel>($"Error getting the AppFunctionAggregate Logs", (int)response.msg.StatusCode);
                    }

                    if (response.result == null)
                    {
                        return log.ErrorAndReturnListModel<PerfLogModel>($"Unable to get AppFunctionAggregate Log list", (int)response.msg.StatusCode);
                    }

                    return ReturnListModel<PerfLogModel>.Success(response.result.Model);
                }
                catch (Exception ex)
                {
                    return log.ErrorAndReturnListModel<PerfLogModel>($"Unable to get AppFunctionAggregate Logs", ex);
                }
            }
        }
    }
}