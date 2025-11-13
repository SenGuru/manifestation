using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class MessageToSendSearchCriteria : BaseSearchCriteria
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? IsSent { get; set; }

        public bool ShowDeleted { get; set; } = false;

        public List<long> PredefinedMessageToSendTypeIds { get; set; }

        public long? RealtimeMessageId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}