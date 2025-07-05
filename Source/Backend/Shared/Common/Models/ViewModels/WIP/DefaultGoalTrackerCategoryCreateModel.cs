#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class DefaultGoalTrackerCategoryXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long DefaultGoalTrackerCategoryId { get; set; }
    }
    */

    public class DefaultGoalTrackerCategoryCreateModel : DefaultGoalTrackerCategoryUpdateModel
    {
        protected long DefaultGoalTrackerCategoryId { get { return base.DefaultGoalTrackerCategoryId; } set { base.DefaultGoalTrackerCategoryId = value; } }
        //protected List<DefaultGoalTrackerCategoryXYZZYXModel> ExistingDefaultGoalTrackerCategoryXYZZYXModels {get {return base.ExistingDefaultGoalTrackerCategoryXYZZYXModels;} set { base.ExistingDefaultGoalTrackerCategoryXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015