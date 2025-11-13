using System;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.Http.Extensions;
using Manifest.Common.Interfaces.Service.Config;
using Manifest.Common.Models.ViewModels;
using Manifest.Repository.Common;
using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;

namespace Manifest.Service.App
{
    public class WhatsAppService : BaseAppService
    {
        private readonly string baseUrl;
        private readonly SystemRepository systemRepository;

        public WhatsAppService(ILog log, ICommonConfiguration commonConfiguration, SystemRepository systemRepository, ObjectMapperService objectMapperService)
            : base(log, objectMapperService)
        {
            this.baseUrl = commonConfiguration.WhatsAppGraphAPIServerUrl;
            this.systemRepository = systemRepository;
        }

        public async Task<ReturnModel<WhatsAppAuthModel>> AuthenticateAsync(string apiKey, string apiSecret, string versionNumber)
        {
            using (HttpClient httpClient = new ())
            {
                try
                {
                    //var payload = new WhatsAppAuthPayloadModel
                    //{
                    //    ClientId = apiKey,
                    //    ClientSecret = apiSecret,
                    //    GrantType = "client_credentials"
                    //};

                    //var stringifiedPayload = JsonUtils.Serialize(payload);

                    var response = await httpClient.GetAndReadAsJsonAsync<WhatsAppAuthModel>(baseUrl + $"{versionNumber}/oauth/access_token?client_id={apiKey}&client_secret={apiSecret}&grant_type=client_credentials");

                    if (!response.msg.IsSuccessStatusCode)
                    {
                        log.Error($"Error getting the App Logs - Status Code - " + response.msg.StatusCode.ToString());
                        return ReturnModel<WhatsAppAuthModel>.Error(response.msg.StatusCode.ToString());
                    }

                    //if (!response.result.IsSuccess)
                    //{
                    //    log.Error($"Error getting the App Logs - " + response.result.ErrorHolder.AllErrorAsString());
                    //    return WhatsAppAuthModel.Error(response.result.ErrorHolder.AllErrorAsString());
                    //}

                    return ReturnModel<WhatsAppAuthModel>.Success(response.result);
                }
                catch (Exception ex)
                {
                    log.Error($"Unable to get App Logs, Message - {ex.Message} - ST - {ex.StackTrace}");
                    return ReturnModel<WhatsAppAuthModel>.Error("Unable to get App Logs");
                }
            }
        }

        public async Task<ReturnModel<bool>> SendMessageAsync(long whatsAppNotificationServerId, WhatsAppMessageModel data)
        {
            using (HttpClient httpClient = new ())
            {
                try
                {
                    var waNotificationServer = await systemRepository.GetWhatsAppNotificationServerByIdAsync(whatsAppNotificationServerId);
                    if (waNotificationServer == null)
                    {
                        return ReturnModel<bool>.Error("Unable to get whatsApp server configurations");
                    }

                    var whatsAppAuthModel = await AuthenticateAsync(waNotificationServer.ApiKey, waNotificationServer.ApiSecret, waNotificationServer.VersionNumber);
                    if (!whatsAppAuthModel.IsSuccess)
                    {
                        return ReturnModel<bool>.Error("Unable to authenticate");
                    }

                    var header = new WhatsAppHeaderModel
                    {
                        Authorization = whatsAppAuthModel.Model.AccessToken,
                    };

                    var response = await httpClient.PostAndReadAsJsonAsync<ReturnModel<bool>, WhatsAppMessageModel>(baseUrl + $"{waNotificationServer.VersionNumber}/{waNotificationServer.PhoneNumberId}/messages", data, header);

                    if (!response.msg.IsSuccessStatusCode)
                    {
                        log.Error($"Error sending message - Status Code - " + response.msg.StatusCode.ToString());
                        return ReturnModel<bool>.Error(response.msg.StatusCode.ToString());
                    }

                    if (!response.result.IsSuccess)
                    {
                        log.Error($"Error sending message - " + response.result.ErrorHolder.AllErrorAsString());
                        return ReturnModel<bool>.Error(response.result.ErrorHolder.AllErrorAsString());
                    }

                    return ReturnModel<bool>.Success(response.result.Model);
                }
                catch (Exception ex)
                {
                    log.Error($"Error sending message, Message - {ex.Message} - ST - {ex.StackTrace}");
                    return ReturnModel<bool>.Error("Error sending message");
                }
            }
        }
    }
}