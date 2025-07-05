#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class VisionBoardSessionPictureXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long VisionBoardSessionPictureId { get; set; }
    }
    */

    public class VisionBoardSessionPictureCreateModel : VisionBoardSessionPictureUpdateModel
    {
        protected long VisionBoardSessionPictureId { get { return base.VisionBoardSessionPictureId; } set { base.VisionBoardSessionPictureId = value; } }
        //protected List<VisionBoardSessionPictureXYZZYXModel> ExistingVisionBoardSessionPictureXYZZYXModels {get {return base.ExistingVisionBoardSessionPictureXYZZYXModels;} set { base.ExistingVisionBoardSessionPictureXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015