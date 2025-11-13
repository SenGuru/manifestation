using System.Threading;
using System.Threading.Tasks;
using Manifest.Service.Manager;
using Microsoft.Extensions.Hosting;

namespace Manifest.SharedApp
{
    public class ScheduledJobInitializerService : BackgroundService
    {
        private readonly ScheduledJobManager jobManager;

        public ScheduledJobInitializerService(ScheduledJobManager jobManager)
        {
            this.jobManager = jobManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await jobManager.InitializeJobsAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
