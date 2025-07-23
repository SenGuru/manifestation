#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class GoalTrackerXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long GoalTrackerId { get; set; }
    }
    */

    public class GoalTrackerCreateModel : GoalTrackerUpdateModel
    {
        protected long GoalTrackerId { get { return base.GoalTrackerId; } set { base.GoalTrackerId = value; } }
        //protected List<GoalTrackerXYZZYXModel> ExistingGoalTrackerXYZZYXModels {get {return base.ExistingGoalTrackerXYZZYXModels;} set { base.ExistingGoalTrackerXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015