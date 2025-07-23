#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class MeditationSessionXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long MeditationSessionId { get; set; }
    }
    */

    public class MeditationSessionCreateModel : MeditationSessionUpdateModel
    {
        protected long MeditationSessionId { get { return base.MeditationSessionId; } set { base.MeditationSessionId = value; } }
        //protected List<MeditationSessionXYZZYXModel> ExistingMeditationSessionXYZZYXModels {get {return base.ExistingMeditationSessionXYZZYXModels;} set { base.ExistingMeditationSessionXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015