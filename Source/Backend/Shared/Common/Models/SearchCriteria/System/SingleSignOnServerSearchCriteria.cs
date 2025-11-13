using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class SingleSignOnServerSearchCriteria : BaseSearchCriteria
    {
        public bool ShowDeleted { get; set; } = false;

        public List<long> PredefinedSingleSignOnServerTypeIds { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}