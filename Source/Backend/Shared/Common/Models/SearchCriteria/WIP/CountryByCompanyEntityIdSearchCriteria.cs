using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class CountryByCompanyEntityIdSearchCriteria : BaseSearchCriteria
    {
        public long CompanyEntityId { get; set; }
    }
}