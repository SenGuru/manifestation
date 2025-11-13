using System.Threading.Tasks;
using Manifest.Common.Models.Queue;
using Manifest.Service.App;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Scope;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Manifest.SharedApp
{
    public class JobServerNotificationConsumer : IConsumer<JobServerMessageQueueModel>
    {
        private readonly ILog log;
        private readonly IServiceScopeFactory scopeFactory;

        public JobServerNotificationConsumer(ILog log, CustomServiceScopeFactory scopeFactory)
        {
            this.log = log;
            this.scopeFactory = scopeFactory;
        }

        public async Task Consume(ConsumeContext<JobServerMessageQueueModel> context)
        {
            if (context == null || context.Message == null || context.Message.JobId == 0)
                return;

            using (var scope = scopeFactory.CreateScope())
            {
                log.Trace($"Running Job {context.Message?.JobId}");

                var repo = scope.ServiceProvider.GetRequiredService<ReportService>();

                var result = await repo.RunJobAsync(context.Message?.JobId ?? 0);

                if (!result.IsSuccess)
                    log.Error(result.ErrorHolder);
            }
        }
    }
}
