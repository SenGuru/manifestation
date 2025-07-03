using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class UserGroupByCompanyEntityIdSearchCriteria : BaseSearchCriteria
    {
        public long CompanyEntityId { get; set; }
    }
}