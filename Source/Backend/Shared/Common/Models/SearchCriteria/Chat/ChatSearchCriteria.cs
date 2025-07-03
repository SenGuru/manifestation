using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class ChatSearchCriteria : BaseSearchCriteria
    {
        public bool ShowDeleted { get; set; } = false;

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}