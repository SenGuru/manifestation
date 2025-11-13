using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria.System
{
    public class AuditOperationDetailSearchCriteria : BaseSearchCriteria
    {
        public bool ShowDeleted { get; set; } = false;

        public long? AuditOperationId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}