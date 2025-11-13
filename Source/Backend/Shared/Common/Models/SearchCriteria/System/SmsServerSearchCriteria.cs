using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class SmsServerSearchCriteria : BaseSearchCriteria
    {
        public List<long> PredefinedSmsAndVoiceServerTypeIds { get; set; }

        public bool ShowDeleted { get; set; } = false;

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}