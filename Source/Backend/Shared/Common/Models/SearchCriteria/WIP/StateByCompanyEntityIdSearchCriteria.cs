using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class StateByCompanyEntityIdSearchCriteria : BaseSearchCriteria
    {
        public long CompanyEntityId { get; set; }
    }
}