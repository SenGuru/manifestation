#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class DefaultMeditationFileXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long DefaultMeditationFileId { get; set; }
    }
    */

    public class DefaultMeditationFileCreateModel : DefaultMeditationFileUpdateModel
    {
        protected long DefaultMeditationFileId { get { return base.DefaultMeditationFileId; } set { base.DefaultMeditationFileId = value; } }
        //protected List<DefaultMeditationFileXYZZYXModel> ExistingDefaultMeditationFileXYZZYXModels {get {return base.ExistingDefaultMeditationFileXYZZYXModels;} set { base.ExistingDefaultMeditationFileXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015