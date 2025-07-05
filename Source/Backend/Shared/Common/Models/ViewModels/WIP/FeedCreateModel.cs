#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class FeedXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long FeedId { get; set; }
    }
    */

    public class FeedCreateModel : FeedUpdateModel
    {
        protected long FeedId { get { return base.FeedId; } set { base.FeedId = value; } }
        //protected List<FeedXYZZYXModel> ExistingFeedXYZZYXModels {get {return base.ExistingFeedXYZZYXModels;} set { base.ExistingFeedXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015