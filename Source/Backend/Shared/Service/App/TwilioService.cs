using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.Config;
using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace Manifest.Service.App
{
    public class TwilioService : BaseAppService
    {
        private readonly string baseUrl;

        public TwilioService(ILog log, ICommonConfiguration commonConfiguration, ObjectMapperService objectMapperService)
            : base(log, objectMapperService)
        {
            this.baseUrl = commonConfiguration.APIServerUrl;
        }

        public CallResource InitiateCall(string phoneNumber, string twilioAccountSid, string twilioAccountAuthToken, string twilioAccountPhoneNumber)
        {
            TwilioClient.Init(twilioAccountSid, twilioAccountAuthToken);

            var url = new Uri($"{baseUrl}Twilio/HandleQuestion");

            var call = CallResource.Create(
                to: new Twilio.Types.PhoneNumber(phoneNumber),
                from: new Twilio.Types.PhoneNumber(twilioAccountPhoneNumber),
                url: url);
                //statusCallback: new Uri($"{baseUrl}Twilio/HandleResponse"),

            return call;
        }

        public async Task<string> InitiateAutomatedCallAsync(string toNumber, long movieShootDayResourceId, long movieShootDayResourceTypeId, string question, string twilioAccountSid, string twilioAccountAuthToken, string twilioAccountPhoneNumber)
        {
            TwilioClient.Init(twilioAccountSid, twilioAccountAuthToken);

            var call = await CallResource.CreateAsync(
                to: new Twilio.Types.PhoneNumber(toNumber),
                from: new Twilio.Types.PhoneNumber(twilioAccountPhoneNumber),
                url: new Uri($"{baseUrl}Twilio/GenerateCallTwiML?msdrid={movieShootDayResourceId}&msdrtid={movieShootDayResourceTypeId}&question={Uri.EscapeDataString(question)}&ntid={(long)PredefinedNotificationTypes.VoiceNotification}"),
                statusCallback: new Uri($"{baseUrl}Twilio/HandleStatus?msdrid={movieShootDayResourceId}&msdrtid={movieShootDayResourceTypeId}&ntid={(long)PredefinedNotificationTypes.VoiceNotification}"),
                statusCallbackEvent: new List<string> { "initiated", "ringing", "answered", "canceled", "completed" },
                statusCallbackMethod: Twilio.Http.HttpMethod.Post,
                method: Twilio.Http.HttpMethod.Post);

            return call.Sid;
        }

        public async Task<string> InitiateAutomatedMessageAsync(string toNumber, long movieShootDayResourceId, long movieShootDayResourceTypeId, string message, string twilioAccountSid, string twilioAccountAuthToken, string twilioAccountPhoneNumber)
        {
            TwilioClient.Init(twilioAccountSid, twilioAccountAuthToken);

            var messageRes = await MessageResource.CreateAsync(
                body: $"Hello from C# Twilio POC!\n{message}",
                to: new Twilio.Types.PhoneNumber(toNumber),
                from: new Twilio.Types.PhoneNumber(twilioAccountPhoneNumber),
                statusCallback: new Uri($"{baseUrl}Twilio/HandleStatus?msdrid={movieShootDayResourceId}&msdrtid={movieShootDayResourceTypeId}&ntid={(long)PredefinedNotificationTypes.SMSNotification}"));

            return messageRes.Sid;
        }

        public VoiceResponse GenerateCallTwiML(long movieShootDayResourceId, long movieShootDayResourceTypeId, long notificationTypeId, string question)
        {
            var response = new VoiceResponse();
            response.Say("Welcome to our automated call system.");
            response.Say(question);
            response.Say("Press any key to confirm");
            var gather = new Gather(
                input: new List<Gather.InputEnum> { Gather.InputEnum.Dtmf },
                numDigits: 1,
                timeout: 10,
                action: new Uri($"{baseUrl}Twilio/HandleGather?msdrid={movieShootDayResourceId}&msdrtid={movieShootDayResourceTypeId}&ntid={notificationTypeId}"),
                method: Twilio.Http.HttpMethod.Post);

            response.Append(gather);
            response.Redirect(new Uri($"{baseUrl}Twilio/HandleGather?msdrid={movieShootDayResourceId}&msdrtid={movieShootDayResourceTypeId}&ntid={notificationTypeId}"));
            //response.Hangup();

            return response;
        }

        public VoiceResponse HandleQuestion()
        {
            var response = new VoiceResponse();
            response.Say("Welcome to our automated call system.");
            response.Say("Please answer the following question with yes or no.");
            response.Say("Do you enjoy programming?");
            response.Gather(
                numDigits: 1,
                action: new Uri($"{baseUrl}Twilio/HandleResponse"),
                method: Twilio.Http.HttpMethod.Get);

            return response;
        }

        public VoiceResponse HandleResponse(string digits)
        {
            var response = new VoiceResponse();

            string answer = digits switch
            {
                "1" => "Yes",
                "2" => "No",
                _ => "Invalid"
            };

            if (answer != "Invalid")
            {
                response.Say($"You answered {answer}. Thank you for your response.");
            }
            else
            {
                response.Say("Invalid response. Please try again.");
                response.Redirect(new Uri($"{baseUrl}call/question"));
            }

            return response;
        }

        public Task<VoiceResponse> ProcessGatherResultAsync(string digits, long movieShootDayResourceId, long movieShootDayResourceTypeId, long notificationTypeId)
        {
            //long notificationStatusId = 0;
            if (notificationTypeId == (long)PredefinedNotificationTypes.VoiceNotification)
            {
                if (digits.IsNullOrEmpty())
                {
                    log.Error("Invalid Digits");
                    //notificationStatusId = (long)PredefinedPhoneCallNotificationStatusTypes.Declined;
                }
            }

            //else if (notificationTypeId == (long)PredefinedNotificationTypes.SMSNotification)
            //{
            //    if (digits.IsNullOrEmpty())
            //    {
            //        log.Error("Invalid Digits");
            //        //notificationStatusId = (long)PredefinedSMSNotificationStatusTypes.Declined;
            //    }
            //}

            //else if (notificationTypeId == (long)PredefinedNotificationTypes.EmailNotification)
            //{
            //    if (digits.IsNullOrEmpty())
            //    {
            //        log.Error("Invalid Digits");
            //        notificationStatusId = (long)PredefinedEmailNotificationStatusTypes.Declined;
            //    }
            //}
            //else if (notificationTypeId == (long)PredefinedNotificationTypes.PushNotification)
            //{
            //    if (digits.IsNullOrEmpty())
            //    {
            //        log.Error("Invalid Digits");
            //        notificationStatusId = (long)PredefinedPushNotificationStatusTypes.Declined;
            //    }
            //}
            //else if (notificationTypeId == (long)PredefinedWhatsAppNotificationStatusTypes.Sent)
            //{
            //    if (digits.IsNullOrEmpty())
            //    {
            //        log.Error("Invalid Digits");
            //        //notificationStatusId = (long)PredefinedWhatsAppNotificationStatusTypes.Declined;
            //    }
            //}

            /*bool result;
            switch (movieShootDayResourceTypeId)
            {
                default:
                    log.Error("Invalid Movie Shoot Day Resource Type");
                    result = false;
                    break;
            }

            var response = new VoiceResponse();
            if (result)
            {
                response.Say("Thank you for confirming your availability. We look forward to seeing you on set.");
            }
            else
            {
                response.Say("There was an error confirming your availability. Please try again later.");
            }
            */

            var response = new VoiceResponse();
            return Task<VoiceResponse>.FromResult(response);
        }

        public Task<bool> ProcessTelecomStatusAsync(string callStatus, string smsStatus, long movieShootDayResourceId, long movieShootDayResourceTypeId, long notificationTypeId)
        {
            if (movieShootDayResourceId == 0 || movieShootDayResourceTypeId == 0 || notificationTypeId == 0)
            {
                log.Error("Invalid Movie Shoot Day Resource Id or Type Id");
                return Task<bool>.FromResult(false);
            }

            /*
            long notificationStatusId = 0;
            if (notificationTypeId == (long)PredefinedNotificationTypes.VoiceNotification)
            {
                if (StringConstants.NegativeTelecomStatusCallBackNames.Contains(callStatus.ToLower()))
                {
                    notificationStatusId = (long)PredefinedPhoneCallNotificationStatusTypes.Declined;
                }
                else
                {
                    notificationStatusId = (long)PredefinedPhoneCallNotificationStatusTypes.Confirmed;
                }
            }
            else if (notificationTypeId == (long)PredefinedNotificationTypes.SMSNotification)
            {
                if (StringConstants.NegativeTelecomStatusCallBackNames.Contains(smsStatus.ToLower()))
                {
                    notificationStatusId = (long)PredefinedSMSNotificationStatusTypes.Declined;
                }
                else
                {
                    notificationStatusId = (long)PredefinedSMSNotificationStatusTypes.Sent;
                }
            }
            //else if (notificationTypeId == (long)PredefinedNotificationTypes.EmailNotification)
            //{
            //    if (StringConstants.NegativeTelecomStatusCallBackNames.Contains(callStatus.ToLower()))
            //    {
            //        notificationStatusId = (long)PredefinedEmailNotificationStatusTypes.Declined;
            //    }
            //    else
            //    {
            //        notificationStatusId = (long)PredefinedEmailNotificationStatusTypes.Sent;
            //    }
            //}
            //else if (notificationTypeId == (long)PredefinedNotificationTypes.PushNotification)
            //{
            //    if (StringConstants.NegativeTelecomStatusCallBackNames.Contains(callStatus.ToLower()))
            //    {
            //        notificationStatusId = (long)PredefinedPushNotificationStatusTypes.Declined;
            //    }
            //    else
            //    {
            //        notificationStatusId = (long)PredefinedPushNotificationStatusTypes.Sent;
            //    }
            //}
            else if (notificationTypeId == (long)PredefinedNotificationTypes.WhatsAppNotification)
            {
                if (StringConstants.NegativeTelecomStatusCallBackNames.Contains(callStatus.ToLower()))
                {
                    notificationStatusId = (long)PredefinedWhatsAppNotificationStatusTypes.Declined;
                }
                else
                {
                    notificationStatusId = (long)PredefinedWhatsAppNotificationStatusTypes.Sent;
                }
            }
            */

            bool result;

            switch (movieShootDayResourceTypeId)
            {
                default:
                    log.Error("Invalid Movie Shoot Day Resource Type");
                    result = false;
                    break;
            }

            return Task<bool>.FromResult(result);
        }
    }
}