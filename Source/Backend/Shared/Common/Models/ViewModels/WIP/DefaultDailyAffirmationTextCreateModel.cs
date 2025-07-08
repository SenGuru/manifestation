#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class DefaultDailyAffirmationTextXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long DefaultDailyAffirmationTextId { get; set; }
    }
    */

    public class DefaultDailyAffirmationTextCreateModel : DefaultDailyAffirmationTextUpdateModel
    {
        protected long DefaultDailyAffirmationTextId { get { return base.DefaultDailyAffirmationTextId; } set { base.DefaultDailyAffirmationTextId = value; } }
        //protected List<DefaultDailyAffirmationTextXYZZYXModel> ExistingDefaultDailyAffirmationTextXYZZYXModels {get {return base.ExistingDefaultDailyAffirmationTextXYZZYXModels;} set { base.ExistingDefaultDailyAffirmationTextXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015