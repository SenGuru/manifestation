using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class DailyAffirmationTextSearchCriteria : BaseSearchCriteria
    {
    //public DateTime? StartDate { get; set; }

    //public DateTime? EndDate { get; set; }

        public bool ShowDeleted { get; set; } = false;

        
public long? CategoryId { get; set; }

        public long? UserId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }

    }

    

        public class DailyAffirmationTextByCategoryIdSearchCriteria : BaseSearchCriteria
        {
                public long CategoryId { get; set; }
        }


        public class CategoryByDailyAffirmationTextIdSearchCriteria : BaseSearchCriteria
        {
                public long DailyAffirmationTextId { get; set; }
        }


        public class DailyAffirmationTextByUserIdSearchCriteria : BaseSearchCriteria
        {
                public long UserId { get; set; }
        }


        public class UserByDailyAffirmationTextIdSearchCriteria : BaseSearchCriteria
        {
                public long DailyAffirmationTextId { get; set; }
        }


}