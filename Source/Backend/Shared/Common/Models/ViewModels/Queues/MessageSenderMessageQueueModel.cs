namespace Manifest.Common.Models.Queue
{
    public class MessageSenderMessageQueueModel
    {
        public long MessageToSentId { get; set; }

        public long? RealtimeMessageId { get; set; }

        public bool? IsSms { get; set; }

        public bool? IsEmail { get; set; }

        public bool? IsRealtime { get; set; }
    }
}
