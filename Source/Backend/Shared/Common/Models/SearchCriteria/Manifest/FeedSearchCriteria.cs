using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class FeedSearchCriteria : BaseSearchCriteria
    {
    //public DateTime? StartDate { get; set; }

    //public DateTime? EndDate { get; set; }

        public bool ShowDeleted { get; set; } = false;

        
public long? UserId { get; set; }

        public long? DailyAffirmationSessionId { get; set; }

        public long? VisionBoardSessionId { get; set; }

        public long? MeditationSessionId { get; set; }

        public List<long> PredefinedMeditationTypeIds { get; set; }

        public long? GoalTrackerId { get; set; }

        public List<long> PredefinedGoalStatusTypeIds { get; set; }

        public long? GoalTrackerMileStoneId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }

    }

    

        public class FeedByUserIdSearchCriteria : BaseSearchCriteria
        {
                public long UserId { get; set; }
        }


        public class UserByFeedIdSearchCriteria : BaseSearchCriteria
        {
                public long FeedId { get; set; }
        }


        public class FeedByDailyAffirmationSessionIdSearchCriteria : BaseSearchCriteria
        {
                public long DailyAffirmationSessionId { get; set; }
        }


        public class DailyAffirmationSessionByFeedIdSearchCriteria : BaseSearchCriteria
        {
                public long FeedId { get; set; }
        }


        public class FeedByVisionBoardSessionIdSearchCriteria : BaseSearchCriteria
        {
                public long VisionBoardSessionId { get; set; }
        }


        public class VisionBoardSessionByFeedIdSearchCriteria : BaseSearchCriteria
        {
                public long FeedId { get; set; }
        }


        public class FeedByMeditationSessionIdSearchCriteria : BaseSearchCriteria
        {
                public long MeditationSessionId { get; set; }
        }


        public class MeditationSessionByFeedIdSearchCriteria : BaseSearchCriteria
        {
                public long FeedId { get; set; }
        }


        public class FeedByGoalTrackerIdSearchCriteria : BaseSearchCriteria
        {
                public long GoalTrackerId { get; set; }
        }


        public class GoalTrackerByFeedIdSearchCriteria : BaseSearchCriteria
        {
                public long FeedId { get; set; }
        }


        public class FeedByGoalTrackerMileStoneIdSearchCriteria : BaseSearchCriteria
        {
                public long GoalTrackerMileStoneId { get; set; }
        }


        public class GoalTrackerMileStoneByFeedIdSearchCriteria : BaseSearchCriteria
        {
                public long FeedId { get; set; }
        }


}