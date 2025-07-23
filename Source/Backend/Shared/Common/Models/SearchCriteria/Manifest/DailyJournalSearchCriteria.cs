using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class DailyJournalSearchCriteria : BaseSearchCriteria
    {
    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

        public bool ShowDeleted { get; set; } = false;

        
public long? UserId { get; set; }

        public List<long> PredefinedMoodTypeIds { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }

    }

    

        public class DailyJournalByUserIdSearchCriteria : BaseSearchCriteria
        {
                public long UserId { get; set; }
        }


        public class UserByDailyJournalIdSearchCriteria : BaseSearchCriteria
        {
                public long DailyJournalId { get; set; }
        }


}