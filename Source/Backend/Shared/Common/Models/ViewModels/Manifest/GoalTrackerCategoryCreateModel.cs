#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class GoalTrackerCategoryXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long GoalTrackerCategoryId { get; set; }
    }
    */

    public class GoalTrackerCategoryCreateModel : GoalTrackerCategoryUpdateModel
    {
        protected long GoalTrackerCategoryId { get { return base.GoalTrackerCategoryId; } set { base.GoalTrackerCategoryId = value; } }
        //protected List<GoalTrackerCategoryXYZZYXModel> ExistingGoalTrackerCategoryXYZZYXModels {get {return base.ExistingGoalTrackerCategoryXYZZYXModels;} set { base.ExistingGoalTrackerCategoryXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015