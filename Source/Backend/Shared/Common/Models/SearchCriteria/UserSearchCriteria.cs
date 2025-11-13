using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class UserSearchCriteria : BaseSearchCriteria
    {
        public bool? Active { get; set; }

        public List<long> PredefinedUserTypeIds { get; set; }

        public bool? ShowDeleted { get; set; }
    }
}