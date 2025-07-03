using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class SavedJobSearchCriteria : BaseSearchCriteria
    {
        public bool ShowDeleted { get; set; } = false;

        public List<long> PredefinedJobTypeIds { get; set; }

        public List<long> PredefinedJobOrReportTypeIds { get; set; }

        public long? JobTemplateId { get; set; }

        public long? SubmittedByUserId { get; set; }

        public long? UserId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}