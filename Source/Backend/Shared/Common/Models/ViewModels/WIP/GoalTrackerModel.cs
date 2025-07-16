#pragma warning disable S2094 // Classes should not be empty
using Manifest.Common.Models.DBView;
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    public class GoalTrackerModel : GoalTrackerView
    {
        public List<GoalTrackerMileStoneModel> ExistingGoalTrackerMileStones { get; set; }

        public List<CategoryModel> ExistingCategorys { get; set; }
    }
}
#pragma warning restore S2094 // Classes should not be empty