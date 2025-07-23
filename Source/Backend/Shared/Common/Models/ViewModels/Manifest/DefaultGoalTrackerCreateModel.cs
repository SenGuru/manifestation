#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class DefaultGoalTrackerXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long DefaultGoalTrackerId { get; set; }
    }
    */

    public class DefaultGoalTrackerCreateModel : DefaultGoalTrackerUpdateModel
    {
        protected long DefaultGoalTrackerId { get { return base.DefaultGoalTrackerId; } set { base.DefaultGoalTrackerId = value; } }
        //protected List<DefaultGoalTrackerXYZZYXModel> ExistingDefaultGoalTrackerXYZZYXModels {get {return base.ExistingDefaultGoalTrackerXYZZYXModels;} set { base.ExistingDefaultGoalTrackerXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015