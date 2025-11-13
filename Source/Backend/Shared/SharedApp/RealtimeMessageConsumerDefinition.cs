using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Scope;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Manifest.SharedApp
{
    public class RealtimeMessageConsumerDefinition : ConsumerDefinition<RealtimeMessageNotificationConsumer>
    {
        private readonly ILog ilog;
        private readonly IServiceScopeFactory scopeFactory;

        public RealtimeMessageConsumerDefinition(CustomServiceScopeFactory scopeFactory, ILog ilog)
        {
            this.ilog = ilog;
            EndpointName = "realtime-messaging-service";
            ConcurrentMessageLimit = 8;
            this.scopeFactory = scopeFactory;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<RealtimeMessageNotificationConsumer> consumerConfigurator, IRegistrationContext context)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                ilog.Info("Configuring Consumer....");
                // configure message retry with millisecond intervals
                endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));
                // use the outbox to prevent duplicate events from being published
                endpointConfigurator.UseInMemoryOutbox(context);
            }
        }
    }
}
