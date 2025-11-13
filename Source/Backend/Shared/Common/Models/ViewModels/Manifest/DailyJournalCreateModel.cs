#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class DailyJournalXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long DailyJournalId { get; set; }
    }
    */

    public class DailyJournalCreateModel : DailyJournalUpdateModel
    {
        protected long DailyJournalId { get { return base.DailyJournalId; } set { base.DailyJournalId = value; } }
        //protected List<DailyJournalXYZZYXModel> ExistingDailyJournalXYZZYXModels {get {return base.ExistingDailyJournalXYZZYXModels;} set { base.ExistingDailyJournalXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015