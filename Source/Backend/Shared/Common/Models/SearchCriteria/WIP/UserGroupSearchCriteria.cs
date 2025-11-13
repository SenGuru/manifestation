using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class UserGroupSearchCriteria : BaseSearchCriteria
    {
        public bool ShowDeleted { get; set; } = false;

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}