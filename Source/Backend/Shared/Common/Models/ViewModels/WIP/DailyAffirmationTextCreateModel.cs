#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class DailyAffirmationTextXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long DailyAffirmationTextId { get; set; }
    }
    */

    public class DailyAffirmationTextCreateModel : DailyAffirmationTextUpdateModel
    {
        protected long DailyAffirmationTextId { get { return base.DailyAffirmationTextId; } set { base.DailyAffirmationTextId = value; } }
        //protected List<DailyAffirmationTextXYZZYXModel> ExistingDailyAffirmationTextXYZZYXModels {get {return base.ExistingDailyAffirmationTextXYZZYXModels;} set { base.ExistingDailyAffirmationTextXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015