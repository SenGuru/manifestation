using System;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Models.Queue;
using Manifest.Common.Models.ViewModels;
using Manifest.Repository.Common;
using Manifest.Service.App;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Scope;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Manifest.SharedApp
{
    public class MessageSenderNotificationConsumer : IConsumer<MessageSenderMessageQueueModel>
    {
        private readonly ILog log;
        private readonly IServiceScopeFactory scopeFactory;

        public MessageSenderNotificationConsumer(ILog log, CustomServiceScopeFactory scopeFactory)
        {
            this.log = log;
            this.scopeFactory = scopeFactory;
        }

        public async Task Consume(ConsumeContext<MessageSenderMessageQueueModel> context)
        {
            if (context == null || context.Message == null || context.Message.MessageToSentId == 0)
                return;

            using (var scope = scopeFactory.CreateScope())
            {
                log.Trace($"Sending Message {context.Message?.MessageToSentId}");
                var msgSenderService = scope.ServiceProvider.GetRequiredService<MessageSenderService>();
                var systemRepository = scope.ServiceProvider.GetRequiredService<SystemRepository>();
                var messageNotificationService = scope.ServiceProvider.GetRequiredService<MessageNotificationService>();

                // await msgSenderService.SendMessageToSendAsync(context?.Message?.MessageToSentId ?? 0, context?.Message?.RealtimeMessageId ?? 0);
                var msgToSend = await systemRepository.GetMessageToSendByIdAsync(context.Message?.MessageToSentId ?? 0);
                if (msgToSend == null)
                {
                    return;
                }

                if (msgToSend.PredefinedMessageToSendTypeId == (long)PredefinedMessageToSendTypes.Email)
                {
                    var msgTemplate = new MessageTemplateModel { EmailSubject = msgToSend.MessageSubject, EmailBody = msgToSend.MessageBody };
                    await msgSenderService.SendEmailAsync(msgToSend.MessageTo, msgToSend.MessageTo, msgTemplate);
                }
                else if (msgToSend.PredefinedMessageToSendTypeId == (long)PredefinedMessageToSendTypes.Sms)
                {
                    var msgTemplate = new MessageTemplateModel { SmsBody = msgToSend.MessageBody };
                    await msgSenderService.SendSmsAsync(msgToSend.MessageTo, msgTemplate);
                }
                else if (msgToSend.PredefinedMessageToSendTypeId == (long)PredefinedMessageToSendTypes.RealtimeMessage)
                {
                    var rMsg = new RealtimeMessageCreateModel
                    {
                        PredefinedMessagePriorityTypeId = (int)PredefinedMessagePriorityTypes.Normal,
                        MessageDateTime = DateTime.UtcNow,
                        MessageSubject = msgToSend.MessageSubject,
                        MessageContent = msgToSend.MessageBody,
                        PredefinedRealtimeMessageStatusTypeId = (int)PredefinedRealtimeMessageStatusTypes.NotYetSent,
                        // if userID is zero, the below CreateRealtimeMessageAsync func gets the current user from session
                        UserId = 0,
                        // WHAT TO DO FOR THIS?
                        PredefinedNotificationMessageTypeId = (int)PredefinedNotificationMessageTypes.TaskAssigned
                    };

                    //creates a realtimemessage entry in db, publishes it to realtimeconsumer
                    await messageNotificationService.CreateRealtimeMessageAsync(rMsg);
                }
            }
        }
    }
}
