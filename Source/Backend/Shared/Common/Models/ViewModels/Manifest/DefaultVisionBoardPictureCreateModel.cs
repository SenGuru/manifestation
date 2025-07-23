#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class DefaultVisionBoardPictureXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long DefaultVisionBoardPictureId { get; set; }
    }
    */

    public class DefaultVisionBoardPictureCreateModel : DefaultVisionBoardPictureUpdateModel
    {
        protected long DefaultVisionBoardPictureId { get { return base.DefaultVisionBoardPictureId; } set { base.DefaultVisionBoardPictureId = value; } }
        //protected List<DefaultVisionBoardPictureXYZZYXModel> ExistingDefaultVisionBoardPictureXYZZYXModels {get {return base.ExistingDefaultVisionBoardPictureXYZZYXModels;} set { base.ExistingDefaultVisionBoardPictureXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015