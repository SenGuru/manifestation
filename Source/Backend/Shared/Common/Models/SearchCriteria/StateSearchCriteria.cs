using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class StateSearchCriteria : BaseSearchCriteria
    {
        public long? CountryId { get; set; }

        public bool ShowDeleted { get; set; } = false;
    }
}