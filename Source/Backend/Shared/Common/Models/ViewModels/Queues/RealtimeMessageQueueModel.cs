using Manifest.Common.Models.ViewModels;

namespace Manifest.Common.Models.Queue
{
    public class RealtimeMessageQueueModel : RealtimeMessageModel
    {
        public string SentToUserName { get; set; }
    }
}
