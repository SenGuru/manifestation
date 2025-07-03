using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class JobTemplateParameterSearchCriteria : BaseSearchCriteria
    {
        public bool ShowDeleted { get; set; } = false;

        public long? JobTemplateId { get; set; }

        public List<long> PredefinedFieldControlTypeIds { get; set; }

        public List<long> PredefinedFieldTypeIds { get; set; }

        public List<long> PredefinedFieldPopulatorTypeIds { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}