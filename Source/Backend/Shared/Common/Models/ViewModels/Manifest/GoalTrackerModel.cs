#pragma warning disable S2094 // Classes should not be empty
using System.Collections.Generic;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.DBView;

namespace Manifest.Common.Models.ViewModels
{
    public class GoalTrackerCategoryWithCategoryModel : GoalTrackerCategoryWithCategoryView
    {

    }

    public class GoalTrackerCategoryWithCategoryView : GoalTrackerCategoryView
    {

    }

    public class GoalTrackerModel : GoalTrackerView
    {
        public List<GoalTrackerMileStoneModel> ExistingGoalTrackerMileStones { get; set; }

        public List<CategoryModel> ExistingCategorys { get; set; }
    }
}
#pragma warning restore S2094 // Classes should not be empty