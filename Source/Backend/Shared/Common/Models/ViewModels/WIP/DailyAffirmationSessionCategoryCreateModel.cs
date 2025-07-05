#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class DailyAffirmationSessionCategoryXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long DailyAffirmationSessionCategoryId { get; set; }
    }
    */

    public class DailyAffirmationSessionCategoryCreateModel : DailyAffirmationSessionCategoryUpdateModel
    {
        protected long DailyAffirmationSessionCategoryId { get { return base.DailyAffirmationSessionCategoryId; } set { base.DailyAffirmationSessionCategoryId = value; } }
        //protected List<DailyAffirmationSessionCategoryXYZZYXModel> ExistingDailyAffirmationSessionCategoryXYZZYXModels {get {return base.ExistingDailyAffirmationSessionCategoryXYZZYXModels;} set { base.ExistingDailyAffirmationSessionCategoryXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015