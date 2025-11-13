#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    public class MultipleGoalTrackerMileStoneCreateModel
    {
        public List<GoalTrackerMileStoneCreateModel> NewGoalTrackerMileStones { get; set; }
    }
}
#pragma warning restore CS0108, SA1502, S4015