#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class GoalTrackerMileStoneXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long GoalTrackerMileStoneId { get; set; }
    }
    */

    public class GoalTrackerMileStoneCreateModel : GoalTrackerMileStoneUpdateModel
    {
        protected long GoalTrackerMileStoneId { get { return base.GoalTrackerMileStoneId; } set { base.GoalTrackerMileStoneId = value; } }
        //protected List<GoalTrackerMileStoneXYZZYXModel> ExistingGoalTrackerMileStoneXYZZYXModels {get {return base.ExistingGoalTrackerMileStoneXYZZYXModels;} set { base.ExistingGoalTrackerMileStoneXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015