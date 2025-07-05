#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class VisionBoardSessionXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long VisionBoardSessionId { get; set; }
    }
    */

    public class VisionBoardSessionCreateModel : VisionBoardSessionUpdateModel
    {
        protected long VisionBoardSessionId { get { return base.VisionBoardSessionId; } set { base.VisionBoardSessionId = value; } }
        //protected List<VisionBoardSessionXYZZYXModel> ExistingVisionBoardSessionXYZZYXModels {get {return base.ExistingVisionBoardSessionXYZZYXModels;} set { base.ExistingVisionBoardSessionXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015