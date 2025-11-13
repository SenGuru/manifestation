using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class DefaultGoalTrackerCategorySearchCriteria : BaseSearchCriteria
    {
    //public DateTime? StartDate { get; set; }

    //public DateTime? EndDate { get; set; }

        public bool ShowDeleted { get; set; } = false;

        
public long? DefaultCategoryId { get; set; }

        public long? DefaultGoalTrackerId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }

    }

    

        public class DefaultGoalTrackerCategoryByDefaultCategoryIdSearchCriteria : BaseSearchCriteria
        {
                public long DefaultCategoryId { get; set; }
        }


        public class DefaultCategoryByDefaultGoalTrackerCategoryIdSearchCriteria : BaseSearchCriteria
        {
                public long DefaultGoalTrackerCategoryId { get; set; }
        }


        public class DefaultGoalTrackerCategoryByDefaultGoalTrackerIdSearchCriteria : BaseSearchCriteria
        {
                public long DefaultGoalTrackerId { get; set; }
        }


        public class DefaultGoalTrackerByDefaultGoalTrackerCategoryIdSearchCriteria : BaseSearchCriteria
        {
                public long DefaultGoalTrackerCategoryId { get; set; }
        }


}