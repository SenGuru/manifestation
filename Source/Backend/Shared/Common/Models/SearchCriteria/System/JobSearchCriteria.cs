using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class JobSearchCriteria : BaseSearchCriteria
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool ShowDeleted { get; set; } = false;

        public List<long> PredefinedJobTypeIds { get; set; }

        public long? JobTemplateId { get; set; }

        public List<long> PredefinedJobOrReportTypeIds { get; set; }

        public List<long> PredefinedJobStatusTypeIds { get; set; }

        public long? SubmittedByUserId { get; set; }

        public long? ExecutionProcessId { get; set; }

        public long? ExecutionThreadId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}