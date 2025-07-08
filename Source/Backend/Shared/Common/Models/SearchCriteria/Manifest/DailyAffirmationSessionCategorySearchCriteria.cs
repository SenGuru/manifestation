using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class DailyAffirmationSessionCategorySearchCriteria : BaseSearchCriteria
    {
    //public DateTime? StartDate { get; set; }

    //public DateTime? EndDate { get; set; }

        public bool ShowDeleted { get; set; } = false;

        
public long? CategoryId { get; set; }

        public long? DailyAffirmationSessionId { get; set; }

        public long? UserId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }

    }

    

        public class DailyAffirmationSessionCategoryByCategoryIdSearchCriteria : BaseSearchCriteria
        {
                public long CategoryId { get; set; }
        }


        public class CategoryByDailyAffirmationSessionCategoryIdSearchCriteria : BaseSearchCriteria
        {
                public long DailyAffirmationSessionCategoryId { get; set; }
        }


        public class DailyAffirmationSessionCategoryByDailyAffirmationSessionIdSearchCriteria : BaseSearchCriteria
        {
                public long DailyAffirmationSessionId { get; set; }
        }


        public class DailyAffirmationSessionByDailyAffirmationSessionCategoryIdSearchCriteria : BaseSearchCriteria
        {
                public long DailyAffirmationSessionCategoryId { get; set; }
        }


        public class DailyAffirmationSessionCategoryByUserIdSearchCriteria : BaseSearchCriteria
        {
                public long UserId { get; set; }
        }


        public class UserByDailyAffirmationSessionCategoryIdSearchCriteria : BaseSearchCriteria
        {
                public long DailyAffirmationSessionCategoryId { get; set; }
        }


}