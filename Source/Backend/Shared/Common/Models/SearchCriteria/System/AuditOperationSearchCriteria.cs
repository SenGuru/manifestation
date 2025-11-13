using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class AuditOperationSearchCriteria : BaseSearchCriteria
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public bool ShowDeleted { get; set; } = false;

        public List<long> PredefinedAuditOperationTypeIds { get; set; }

        public long? UserId { get; set; }

        public long? MovieId { get; set; }

        public List<long> PredefinedResourceTypeIds { get; set; }

        public List<long> UserIds { get; set; }

        public string IpAddress { get; set; }

        public long? ResourceId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}