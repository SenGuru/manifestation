using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class GoalTrackerSearchCriteria : BaseSearchCriteria
    {
        //public DateTime? StartDate { get; set; }

        //public DateTime? EndDate { get; set; }

        public bool ShowDeleted { get; set; } = false;

        public List<long> PredefinedGoalPriorityTypeIds { get; set; }

        public List<long> PredefinedGoalStatusTypeIds { get; set; }

        public long? UserId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}