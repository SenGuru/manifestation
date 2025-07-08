#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class MeditationFileXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long MeditationFileId { get; set; }
    }
    */

    public class MeditationFileCreateModel : MeditationFileUpdateModel
    {
        protected long MeditationFileId { get { return base.MeditationFileId; } set { base.MeditationFileId = value; } }
        //protected List<MeditationFileXYZZYXModel> ExistingMeditationFileXYZZYXModels {get {return base.ExistingMeditationFileXYZZYXModels;} set { base.ExistingMeditationFileXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015