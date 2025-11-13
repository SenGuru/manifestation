using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class GoalTrackerMileStoneSearchCriteria : BaseSearchCriteria
    {
    //public DateTime? StartDate { get; set; }

    //public DateTime? EndDate { get; set; }

        public bool ShowDeleted { get; set; } = false;

        
public long? GoalTrackerId { get; set; }

        public List<long> PredefinedGoalStatusTypeIds { get; set; }

        public long? UserId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }

    }

    

        public class GoalTrackerMileStoneByGoalTrackerIdSearchCriteria : BaseSearchCriteria
        {
                public long GoalTrackerId { get; set; }
        }


        public class GoalTrackerByGoalTrackerMileStoneIdSearchCriteria : BaseSearchCriteria
        {
                public long GoalTrackerMileStoneId { get; set; }
        }


        public class GoalTrackerMileStoneByUserIdSearchCriteria : BaseSearchCriteria
        {
                public long UserId { get; set; }
        }


        public class UserByGoalTrackerMileStoneIdSearchCriteria : BaseSearchCriteria
        {
                public long GoalTrackerMileStoneId { get; set; }
        }


}