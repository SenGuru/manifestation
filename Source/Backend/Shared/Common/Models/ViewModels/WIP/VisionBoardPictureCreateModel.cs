#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class VisionBoardPictureXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long VisionBoardPictureId { get; set; }
    }
    */

    public class VisionBoardPictureCreateModel : VisionBoardPictureUpdateModel
    {
        protected long VisionBoardPictureId { get { return base.VisionBoardPictureId; } set { base.VisionBoardPictureId = value; } }
        //protected List<VisionBoardPictureXYZZYXModel> ExistingVisionBoardPictureXYZZYXModels {get {return base.ExistingVisionBoardPictureXYZZYXModels;} set { base.ExistingVisionBoardPictureXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015