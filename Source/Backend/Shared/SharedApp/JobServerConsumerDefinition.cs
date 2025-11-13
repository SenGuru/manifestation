using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Scope;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Manifest.SharedApp
{
    public class JobServerConsumerDefinition : ConsumerDefinition<JobServerNotificationConsumer>
    {
        private readonly ILog log;
        private readonly IServiceScopeFactory scopeFactory;

        public JobServerConsumerDefinition(ILog log, CustomServiceScopeFactory scopeFactory)
        {
            this.log = log;
            EndpointName = "job-server-service";
            ConcurrentMessageLimit = 4;
            this.scopeFactory = scopeFactory;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<JobServerNotificationConsumer> consumerConfigurator, IRegistrationContext context)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                log.Info("Configuring Job Server Consumer....");
                // configure message retry with millisecond intervals
                endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 500, 1000, 2000, 5000, 10000));
                // use the outbox to prevent duplicate events from being published
                endpointConfigurator.UseInMemoryOutbox(context);
            }
        }
    }
}
