using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class ActivitySearchCriteria : BaseSearchCriteria
    {
        public List<long> PredefinedAuditOperationTypeIds { get; set; }
    }
}