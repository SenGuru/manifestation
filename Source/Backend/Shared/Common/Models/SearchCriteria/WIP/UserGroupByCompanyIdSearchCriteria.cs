using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class UserGroupByCompanyIdSearchCriteria : BaseSearchCriteria
    {
        public long CompanyId { get; set; }
    }
}