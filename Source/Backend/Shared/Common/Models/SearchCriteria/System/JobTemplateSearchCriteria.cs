using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class JobTemplateSearchCriteria : BaseSearchCriteria
    {
        public bool ShowDeleted { get; set; } = false;

        public List<long> PredefinedJobTypeIds { get; set; }

        public List<long> PredefinedJobOrReportTypeIds { get; set; }

        public long? JobTemplateCategoryId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}