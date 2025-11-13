using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class SavedJobNotificationEmailSearchCriteria : BaseSearchCriteria
    {
        public bool ShowDeleted { get; set; } = false;

        public long? SavedJobId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}