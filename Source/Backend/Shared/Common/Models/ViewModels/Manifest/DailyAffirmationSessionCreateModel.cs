#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class DailyAffirmationSessionXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long DailyAffirmationSessionId { get; set; }
    }
    */

    public class DailyAffirmationSessionCreateModel : DailyAffirmationSessionUpdateModel
    {
        protected long DailyAffirmationSessionId { get { return base.DailyAffirmationSessionId; } set { base.DailyAffirmationSessionId = value; } }
        //protected List<DailyAffirmationSessionXYZZYXModel> ExistingDailyAffirmationSessionXYZZYXModels {get {return base.ExistingDailyAffirmationSessionXYZZYXModels;} set { base.ExistingDailyAffirmationSessionXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015