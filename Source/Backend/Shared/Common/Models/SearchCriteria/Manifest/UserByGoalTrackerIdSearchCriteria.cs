using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class UserByGoalTrackerIdSearchCriteria : BaseSearchCriteria
        {
                public long GoalTrackerId { get; set; }
        }


}