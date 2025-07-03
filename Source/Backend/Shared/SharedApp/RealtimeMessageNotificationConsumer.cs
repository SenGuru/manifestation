using System;
using System.Threading.Tasks;
using Manifest.Common.Models.Queue;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Scope;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Manifest.SharedApp
{
    public class RealtimeMessageNotificationConsumer : IConsumer<RealtimeMessageQueueModel>
    {
        private readonly ILog log;
        private readonly IHubContext<RealtimeMessageNotificationHub> hubContext;
        private readonly IServiceScopeFactory scopeFactory;

        public RealtimeMessageNotificationConsumer(ILog log, IHubContext<RealtimeMessageNotificationHub> hubContext, CustomServiceScopeFactory scopeFactory)
        {
            this.log = log;
            this.hubContext = hubContext;
            this.scopeFactory = scopeFactory;
        }

        public async Task Consume(ConsumeContext<RealtimeMessageQueueModel> context)
        {
            if (context == null || context.Message == null || context.Message.UserName == null)
                return;

            using (var scope = scopeFactory.CreateScope())
            {
                var client = hubContext.Clients.Groups(context.Message.UserName);

                if (client == null)
                    return;

                try
                {
                    log.Trace($"Calling RecieveNotificationMessage to user {context.Message?.UserName}");
                    await client.SendAsync("RecieveNotificationMessage", context.Message);
                    log.Trace($"Notification message sent successfully to user {context.Message?.UserName}");
                }
                catch (Exception ex)
                {
                    log.Error($"Failed to send notification message to user {context.Message?.UserName}: {ex.Message}");
                    // Handle the failure (e.g., retry, log, etc.)
                }
            }
        }
    }
}
