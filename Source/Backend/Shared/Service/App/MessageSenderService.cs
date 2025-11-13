using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Interfaces.Service.Config;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.MessageTemplateParams;
using Manifest.Common.Models.Queue;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.ViewModels;
using Manifest.Repository.Common;
using Manifest.Service.ObjectMapping;
using Manifest.Service.Queue;
using Framework.Infrastructure.Interfaces.Services;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;

namespace Manifest.Service.App
{
    public class MessageSenderService : BaseAppService//, IMessageSenderService
    {
        private readonly IEmailService emailService;
        private readonly ICommonConfiguration commonConfiguration;
        private readonly MessageTemplateService messageTemplateService;
        private readonly ICurrentSessionService currentSessionService;
        private readonly SystemRepository systemRepository;
        private readonly QueueClientService queueClientService;

        public MessageSenderService(ILog log, ObjectMapperService objectMapperService, IEmailService emailService, ICommonConfiguration commonConfiguration, MessageTemplateService messageTemplateService, ICurrentSessionService currentSessionService, SystemRepository systemRepository, QueueClientService queueClientService)
            : base(log, objectMapperService)
        {
            this.emailService = emailService;
            this.commonConfiguration = commonConfiguration;
            this.messageTemplateService = messageTemplateService;
            this.currentSessionService = currentSessionService;
            this.systemRepository = systemRepository;
            this.queueClientService = queueClientService;
        }

        public Task<ReturnModel<bool>> SendSmsAsync(string toSms, MessageTemplateModel msgModel)
        {
            if (msgModel == null)
            {
                return Task.FromResult(ReturnModel<bool>.Error("Message Template is not valid"));
            }

            if (!msgModel.SmsBody.IsTrimmedStringNullOrEmpty())
            {
                // send sms ?;
            }

            return Task.FromResult(ReturnModel<bool>.Success(true));
        }

        public async Task<ReturnModel<bool>> SendEmailAsync(List<(string displayName, string email)> toEmails, PredefinedMessageTemplateTypes messageTemplateType, object param)
        {
            var msg = await messageTemplateService.FindEmailForTemplateAsync(messageTemplateType, null, param);
            if (!msg.IsSuccess)
            {
                log.Fatal($"Unable to find template for messageTemplateType = {messageTemplateType},  param = {param?.ToString()}");
                return ReturnModel<bool>.Error("Messaging Framework is not configured. Please call support for help.");
            }

            return await SendEmailAsync(toEmails, msg.Model);
        }

        public async Task<ReturnModel<bool>> SendEmailAsync(List<(string displayName, string email)> toEmailList, MessageTemplateModel msgModel)
        {
            var isAllOk = true;
            if (!toEmailList.IsNullOrEmpty())
            {
                foreach (var item in toEmailList)
                {
                    var sendResult = await SendEmailAsync(item.displayName, item.email, msgModel);
                    if (!sendResult.IsSuccess)
                    {
                        isAllOk = false;
                        log.ErrorAndReturnModel<bool>(sendResult);
                    }
                }
            }

            if (!isAllOk)
                return ReturnModel<bool>.Error("Not all emails are sent");
            return ReturnModel<bool>.Success(true);
        }

        public async Task<ReturnModel<bool>> SendEmailAsync(List<string> toEmailList, MessageTemplateModel msgModel)
        {
            var isAllOk = true;
            if (!toEmailList.IsNullOrEmpty())
            {
                foreach (var item in toEmailList)
                {
                    var sendResult = await SendEmailAsync(item, item, msgModel);
                    if (!sendResult.IsSuccess)
                    {
                        isAllOk = false;
                        log.ErrorAndReturnModel<bool>(sendResult);
                    }
                }
            }

            if (!isAllOk)
                return ReturnModel<bool>.Error("Not all emails are sent");
            return ReturnModel<bool>.Success(true);
        }

        public async Task<ReturnModel<bool>> SendEmailAsync(string toName, string toEmail, MessageTemplateModel msgModel)
        {
            bool useMailKit = true;
            if (msgModel == null)
            {
                return ReturnModel<bool>.Error("Msg Template Model is not Valid");
            }

            if (msgModel.EmailSubject.IsTrimmedStringNullOrEmpty() || msgModel.EmailBody.IsTrimmedStringNullOrEmpty())
            {
                return ReturnModel<bool>.Error("Msg Template Model does not contain either email subject and email body");
            }

            var tenantMailServer = await systemRepository.GetDefaultMailServerAsync();

            if (tenantMailServer != null)
            {
                var a = await emailService.SendAsync(
                    tenantMailServer.Host,
                    (int)tenantMailServer.Port,
                    tenantMailServer.UserName,
                    tenantMailServer.Password,
                    tenantMailServer.IsSecure ?? false,
                    tenantMailServer.SenderName,
                    tenantMailServer.SenderEmail,
                    toName,
                    toEmail,
                    null,
                    null,
                    msgModel.EmailSubject,
                    msgModel.EmailBody,
                    useMailKit: useMailKit);

                return a;
            }

            return await emailService.SendAsync(
                commonConfiguration.EmailServer,
                commonConfiguration.EmailServerPort,
                commonConfiguration.EmailServerUserName,
                commonConfiguration.EmailServerPassword,
                commonConfiguration.EmailUseSSL,
                commonConfiguration.AdminNameForEmail,
                commonConfiguration.AdminEmailForEmail,
                toName,
                toEmail,
                null,
                null,
                msgModel.EmailSubject,
                msgModel.EmailBody,
                useMailKit: useMailKit);
        }

        public Task<ReturnModel<bool>> SendMessageEmailAsync(string toEmail, MessageTemplateModel msgModel)
        {
            return SendEmailAsync(toEmail, toEmail, msgModel);
        }

        public Task<ReturnModel<bool>> SendMessageEmailAsync(List<string> toEmailList, MessageTemplateModel msgModel)
        {
            return SendEmailAsync(toEmailList, msgModel);
        }

        public async Task<ReturnModel<bool>> SendMessageToSendAsync(long messageToSendId, long realtimeMessageId)
        {
            try
            {
                var msgToSend = await systemRepository.GetMessageToSendByIdAsync(messageToSendId);
                if (msgToSend == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the Message with messageToSendId = {messageToSendId}");
                }

                if (msgToSend.PredefinedMessageToSendTypeId == (long)PredefinedMessageToSendTypes.Email)
                {
                    var msgTemplate = new MessageTemplateModel { EmailSubject = msgToSend.MessageSubject, EmailBody = msgToSend.MessageBody };
                    return await SendEmailAsync(msgToSend.MessageTo, msgToSend.MessageTo, msgTemplate);
                }
                else if (msgToSend.PredefinedMessageToSendTypeId == (long)PredefinedMessageToSendTypes.Sms)
                {
                    var msgTemplate = new MessageTemplateModel { SmsBody = msgToSend.MessageBody };
                    return await SendSmsAsync(msgToSend.MessageTo, msgTemplate);
                }
                else if (msgToSend.PredefinedMessageToSendTypeId == (long)PredefinedMessageToSendTypes.RealtimeMessage)
                {
                    //var msgTemplate = new CompanyMessageTemplateModel { WebNotificationSubject = msgToSend.MessageSubject, WebNotificationBody = msgToSend.MessageBody };
                    return ReturnModel<bool>.Success(true); //await SendRealtimeMsg(msgTemplate));
                }

                return log.ErrorAndReturnModel<bool>($"Message type is not supported = {messageToSendId}");
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when sending emails from the Messaget To Send Table", ex);
            }
        }

        public async Task<ReturnModel<bool>> SaveSmsAsync(string toSms, PredefinedMessageTemplateTypes messageTemplateType, object param)
        {
            var msg = await messageTemplateService.FindEmailForTemplateAsync(messageTemplateType, null, param);
            if (!msg.IsSuccess)
            {
                log.Fatal($"Unable to find template for messageTemplateType = {messageTemplateType}, param = {param?.ToString()}");
                return ReturnModel<bool>.Error(msg);
            }

            if (msg.Model == null)
            {
                return log.ErrorAndReturnModel<bool>("Template is not available for this message. ");
            }

            if (!msg.Model.SmsBody.IsTrimmedStringNullOrEmpty())
            {
                var item = await CreateMessageToSendAsync(new MessageToSend
                {
                    MessageFrom = commonConfiguration.AdminEmailForEmail,
                    MessageTo = toSms,
                    MessageSubject = "",
                    MessageBody = msg.Model.SmsBody,
                    PredefinedMessageToSendTypeId = (long)PredefinedMessageToSendTypes.Sms,
                    RetryCount = 3,
                    IsDeleted = false,
                });

                if (!item.IsSuccess)
                {
                    return ReturnModel<bool>.Error(item);
                }

                await queueClientService.SubmitMessageAsync(new MessageSenderMessageQueueModel { MessageToSentId = item.Model.MessageToSendId });
            }

            return ReturnModel<bool>.Success(true);
        }

        public async Task<ReturnModel<bool>> SaveRealtimeMessageAsync(PredefinedMessageTemplateTypes messageTemplateType, object param, bool isInternal)
        {
            var msg = await messageTemplateService.FindEmailForTemplateAsync(messageTemplateType, null, param);
            if (!msg.IsSuccess)
            {
                log.Fatal($"Unable to find template for messageTemplateType = {messageTemplateType}, param = {param?.ToString()}");
                return ReturnModel<bool>.Error(msg);
            }

            if (msg.Model == null)
            {
                return log.ErrorAndReturnModel<bool>("Template is not available for this message. ");
            }

            if (!(msg.Model.WebNotificationSubject.IsTrimmedStringNullOrEmpty() || msg.Model.WebNotificationBody.IsTrimmedStringNullOrEmpty()))
            {
                var currentUserName = await currentSessionService.GetCurrentUserNameAsStringAsync();

                var item = await CreateMessageToSendAsync(new MessageToSend
                {
                    MessageFrom = commonConfiguration.AdminEmailForEmail,
                    MessageSubject = msg.Model.WebNotificationSubject,
                    MessageBody = msg.Model.WebNotificationBody,
                    RetryCount = 3,
                    MessageTo = currentUserName,
                    PredefinedMessageToSendTypeId = (long)PredefinedMessageToSendTypes.RealtimeMessage,
                    IsDeleted = false,
                });

                if (!item.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(item);
                }

                await queueClientService.SubmitMessageAsync(new MessageSenderMessageQueueModel { MessageToSentId = item.Model.MessageToSendId });
            }

            return ReturnModel<bool>.Success(true);
        }

        public async Task<ReturnModel<bool>> SaveEmailAsync(List<(string displayName, string email)> toEmails, PredefinedMessageTemplateTypes messageTemplateType, object param, bool isInternal)
        {
            var msg = await messageTemplateService.FindEmailForTemplateAsync(messageTemplateType, 0, param);
            if (!msg.IsSuccess)
            {
                log.Fatal($"Unable to find template for messageTemplateType = {messageTemplateType}, param = {param?.ToString()}");
                return log.ErrorAndReturnModel<bool>(msg);
            }

            var saveEmail = await SaveEmailAsync(toEmails, msg.Model, isInternal);
            if (!saveEmail.IsSuccess)
            {
                return log.ErrorAndReturnModel<bool>(saveEmail);
            }

            return ReturnModel<bool>.Success(true);
        }

        public async Task<ReturnModel<bool>> SaveEmailAsync(List<(string displayName, string email)> toEmailList, MessageTemplateModel msgModel, bool isInternal)
        {
            var isAllOk = true;
            if (!toEmailList.IsNullOrEmpty())
            {
                foreach (var item in toEmailList)
                {
                    var saveEmail = await SaveEmailAsync(item.displayName, item.email, msgModel, isInternal);
                    if (!saveEmail.IsSuccess)
                    {
                        isAllOk = false;
                        log.ErrorAndReturnModel<bool>(saveEmail);
                    }
                }
            }

            if (!isAllOk)
                return ReturnModel<bool>.Error("Not all emails are sent");
            return ReturnModel<bool>.Success(true);
        }

        public async Task<ReturnModel<bool>> SaveEmailAsync(List<string> toEmailList, MessageTemplateModel msgModel, bool isInternal)
        {
            var isAllOk = true;
            if (!toEmailList.IsNullOrEmpty())
            {
                foreach (var item in toEmailList)
                {
                    var saveEmail = await SaveEmailAsync(item, item, msgModel, isInternal);
                    if (!saveEmail.IsSuccess)
                    {
                        isAllOk = false;
                        log.ErrorAndReturnModel<bool>(saveEmail);
                    }
                }
            }

            if (!isAllOk)
                return ReturnModel<bool>.Error("Not all emails are sent");
            return ReturnModel<bool>.Success(true);
        }

        public async Task<ReturnModel<bool>> SaveEmailAsync(string toName, string toEmail, MessageTemplateModel msgModel, bool isInternal)
        {
            if (msgModel == null)
            {
                return log.ErrorAndReturnModel<bool>("Message template is not valid");
            }

            bool canSend = false;

            if (isInternal && (msgModel.SendEmailForInternalUser ?? false))
            {
                canSend = true;
            }

            if (!isInternal && (msgModel.SendEmailForExternalUser ?? false))
            {
                canSend = true;
            }

            if (!canSend)
            {
                return ReturnModel<bool>.Success(true);
            }

            if (msgModel.EmailSubject.IsTrimmedStringNullOrEmpty() || msgModel.EmailBody.IsTrimmedStringNullOrEmpty())
            {
                return ReturnModel<bool>.Error("Msg Template Model does not contain either email subject and email body");
            }

            var tenantMailServer = await systemRepository.GetDefaultMailServerAsync();

            ReturnModel<MessageToSend> item;
            if (tenantMailServer != null)
            {
                item = await CreateMessageToSendAsync(new MessageToSend
                {
                    MessageFrom = tenantMailServer.SenderEmail,
                    MessageFromName = tenantMailServer.SenderName,
                    MessageTo = toEmail,
                    MessageToName = toName,
                    MessageSubject = msgModel.EmailSubject,
                    MessageBody = msgModel.EmailBody,
                    PredefinedMessageToSendTypeId = (long)PredefinedMessageToSendTypes.Email,
                    RetryCount = 3,
                    IsDeleted = false,
                });
            }
            else
            {
                item = await CreateMessageToSendAsync(new MessageToSend
                {
                    MessageFrom = commonConfiguration.AdminEmailForEmail,
                    MessageFromName = commonConfiguration.AdminNameForEmail,
                    MessageTo = toEmail,
                    MessageToName = toName,
                    MessageSubject = msgModel.EmailSubject,
                    MessageBody = msgModel.EmailBody,
                    PredefinedMessageToSendTypeId = (long)PredefinedMessageToSendTypes.Email,
                    RetryCount = 3,
                    IsDeleted = false,
                });
            }

            if (!item.IsSuccess)
            {
                return log.ErrorAndReturnModel<bool>(item);
            }

            await queueClientService.SubmitMessageAsync(new MessageSenderMessageQueueModel { MessageToSentId = item.Model.MessageToSendId });

            return ReturnModel<bool>.Success(true);
        }

        public Task<ReturnModel<bool>> SaveMessageEmailAsync(string toEmail, MessageTemplateModel msgModel, bool isInternal)
        {
            return SaveEmailAsync(toEmail, toEmail, msgModel, isInternal);
        }

        public Task<ReturnModel<bool>> SaveMessageEmailAsync(List<string> toEmailList, MessageTemplateModel msgModel, bool isInternal)
        {
            return SaveEmailAsync(toEmailList, msgModel, isInternal);
        }

        public async Task<ReturnModel<bool>> SaveEmailFromMessageToSendAsync(long messageToSendId, bool isInternal)
        {
            try
            {
                var msgToSend = await systemRepository.GetMessageToSendByIdAsync(messageToSendId);
                if (msgToSend == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the Message with messageToSendId = {messageToSendId}");
                }

                var msgTemplate = new MessageTemplateModel { EmailSubject = msgToSend.MessageSubject, EmailBody = msgToSend.MessageBody };

                return await SaveEmailAsync(msgToSend.MessageTo, msgToSend.MessageTo, msgTemplate, isInternal);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when sending emails from the Messaget To Send Table", ex);
            }
        }

        public async Task<ReturnModel<bool>> SaveMessageToSendAsync(object param, PredefinedMessageTemplateTypes messageTemplateType, List<(string displayName, string email)> sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null, bool isInternal = true)
        {
            if (sendToEmail.IsNotNullOrEmpty())
            {
                return await SaveEmailAsync(sendToEmail, messageTemplateType, param, isInternal);
            }

            if (sendRealTimeMessage != null && sendRealTimeMessage == true)
            {
                return await SaveRealtimeMessageAsync(messageTemplateType, param, isInternal);
            }

            if (sendToSmsNumber.IsNotNullOrEmpty())
            {
                return await SaveSmsAsync(sendToSmsNumber, messageTemplateType, param);
            }

            log.Error($"No valid option specified to sending the message - type - {messageTemplateType}");
            return ReturnModel<bool>.Success(true);
        }

        public Task<ReturnModel<bool>> ChangedPasswordAsync(ChangedPasswordMessageTemplateParam param, string sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return ChangedPasswordAsync(param, sendToEmail != null ? new List<(string displayName, string email)> { (sendToEmail, sendToEmail) } : null, sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> ChangedPasswordAsync(ChangedPasswordMessageTemplateParam param, List<string> sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return ChangedPasswordAsync(param, sendToEmail?.SelectToList(x => (x, x)), sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> ChangedPasswordAsync(ChangedPasswordMessageTemplateParam param, List<(string displayName, string email)> sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return SaveMessageToSendAsync(param, PredefinedMessageTemplateTypes.ChangedPassword, sendToEmail, sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> MemberOtpAsync(MemberOtpMessageTemplateParam param, string sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return MemberOtpAsync(param, sendToEmail != null ? new List<(string displayName, string email)> { (sendToEmail, sendToEmail) } : null, sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> MemberOtpAsync(MemberOtpMessageTemplateParam param, List<string> sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return MemberOtpAsync(param, sendToEmail?.SelectToList(x => (x, x)), sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> MemberOtpAsync(MemberOtpMessageTemplateParam param, List<(string displayName, string email)> sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return SaveMessageToSendAsync(param, PredefinedMessageTemplateTypes.MemberOtp, sendToEmail, sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> ForgotPasswordAsync(ForgotPasswordMessageTemplateParam param, string sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return ForgotPasswordAsync(param, sendToEmail != null ? new List<(string displayName, string email)> { (sendToEmail, sendToEmail) } : null, sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> ForgotPasswordAsync(ForgotPasswordMessageTemplateParam param, List<string> sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return ForgotPasswordAsync(param, sendToEmail?.SelectToList(x => (x, x)), sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> ForgotPasswordAsync(ForgotPasswordMessageTemplateParam param, List<(string displayName, string email)> sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return SaveMessageToSendAsync(param, PredefinedMessageTemplateTypes.ForgotPassword, sendToEmail, sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> ReportErrorAsync(ReportErrorMessageTemplateParam param, string sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return ReportErrorAsync(param, sendToEmail != null ? new List<(string displayName, string email)> { (sendToEmail, sendToEmail) } : null, sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> ReportErrorAsync(ReportErrorMessageTemplateParam param, List<string> sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return ReportErrorAsync(param, sendToEmail?.SelectToList(x => (x, x)), sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> ReportErrorAsync(ReportErrorMessageTemplateParam param, List<(string displayName, string email)> sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return SaveMessageToSendAsync(param, PredefinedMessageTemplateTypes.ReportError, sendToEmail, sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> ReportSuccessAsync(ReportSuccessMessageTemplateParam param, string sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return ReportSuccessAsync(param, sendToEmail != null ? new List<(string displayName, string email)> { (sendToEmail, sendToEmail) } : null, sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> ReportSuccessAsync(ReportSuccessMessageTemplateParam param, List<string> sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return ReportSuccessAsync(param, sendToEmail?.SelectToList(x => (x, x)), sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> ReportSuccessAsync(ReportSuccessMessageTemplateParam param, List<(string displayName, string email)> sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return SaveMessageToSendAsync(param, PredefinedMessageTemplateTypes.ReportSuccess, sendToEmail, sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> UserCreationAsync(UserCreationMessageTemplateParam param, string sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return UserCreationAsync(param, sendToEmail != null ? new List<(string displayName, string email)> { (sendToEmail, sendToEmail) } : null, sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> UserCreationAsync(UserCreationMessageTemplateParam param, List<string> sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return UserCreationAsync(param, sendToEmail?.SelectToList(x => (x, x)), sendRealTimeMessage, sendToSmsNumber);
        }

        public Task<ReturnModel<bool>> UserCreationAsync(UserCreationMessageTemplateParam param, List<(string displayName, string email)> sendToEmail = null, bool? sendRealTimeMessage = null, string sendToSmsNumber = null)
        {
            return SaveMessageToSendAsync(param, PredefinedMessageTemplateTypes.UserCreation, sendToEmail, sendRealTimeMessage, sendToSmsNumber);
        }

        #region MessageToSend
        public async Task<ReturnModel<MessageToSend>> CreateMessageToSendAsync(MessageToSend messageToSend)
        {
            try
            {
                return ReturnModel<MessageToSend>.Success(await systemRepository.CreateMessageToSendAsync(messageToSend, await currentSessionService.GetCurrentUserIdAsync()));
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<MessageToSend>("Unexpected error when Creating Message To Send", ex);
            }
        }

        public Task<ReturnModel<MessageToSend>> GetMessageToSendByIdForReadAsync(long messageToSendById)
        {
            return GetMessageToSendByIdInternalAsync(forWrite: false, messageToSendById);
        }

        public Task<ReturnModel<MessageToSend>> GetMessageToSendByIdForWriteAsync(long messageToSendById)
        {
            return GetMessageToSendByIdInternalAsync(forWrite: true, messageToSendById);
        }

        public async Task<ReturnModel<MessageToSend>> GetMessageToSendByIdInternalAsync(bool forWrite, long messageToSendById)
        {
            try
            {
                var msg = await systemRepository.GetMessageToSendByIdAsync(messageToSendById);
                return ReturnModel<MessageToSend>.Success(msg);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<MessageToSend>("Unexpected error when getting the Message To Send", ex);
            }
        }

        public async Task<ReturnListModel<MessageToSend>> GetMessagesToSendAsync(MessageToSendSearchCriteria search)
        {
            try
            {
                return ReturnListModel<MessageToSend>.Success((await systemRepository.GetMessagesToSendAsync(search)).Result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<MessageToSend>("Unexpected error when getting Message To Send List", ex);
            }
        }
        #endregion
    }
}
