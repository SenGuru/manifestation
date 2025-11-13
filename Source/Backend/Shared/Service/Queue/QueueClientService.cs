using System.Threading.Tasks;
using Manifest.Common.Models.Queue;
using MassTransit;

namespace Manifest.Service.Queue
{
    public class QueueClientService
    {
        private readonly IBusControl busControl;

        public QueueClientService(IBusControl busControl)
        {
            this.busControl = busControl;
        }

        public Task SubmitMessageAsync<T>(T t)
            where T : class
        {
            return busControl.Publish<T>(t);
        }

        public Task SubmitMessageAsync(MessageSenderMessageQueueModel t)
        {
            return busControl.Publish(t);
        }
    }
}
