using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class DefaultDailyAffirmationTextSearchCriteria : BaseSearchCriteria
    {

        public long? UserId { get; set; }

        //public DateTime? StartDate { get; set; }

        //public DateTime? EndDate { get; set; }

        public bool ShowDeleted { get; set; } = false;

        
public long? DefaultCategoryId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }

    }

    

        public class DefaultDailyAffirmationTextByDefaultCategoryIdSearchCriteria : BaseSearchCriteria
        {
                public long DefaultCategoryId { get; set; }
        }


        public class DefaultCategoryByDefaultDailyAffirmationTextIdSearchCriteria : BaseSearchCriteria
        {
                public long DefaultDailyAffirmationTextId { get; set; }
        }


}