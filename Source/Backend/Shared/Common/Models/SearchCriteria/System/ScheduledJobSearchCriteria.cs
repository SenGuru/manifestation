using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class ScheduledJobSearchCriteria : BaseSearchCriteria
    {
        public bool ShowDeleted { get; set; } = false;

        public long? JobTemplateId { get; set; }

        public List<long> PredefinedJobOrReportTypeIds { get; set; }

        public long? SubmittedByUserId { get; set; }

        public long? RunAsUserId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}