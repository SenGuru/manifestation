#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    /*
    public class SavedJobXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long SavedJobId { get; set; }
    }
    */

    public class SavedJobCreateModel : SavedJobUpdateModel
    {
        protected long SavedJobId { get { return base.SavedJobId; } set { base.SavedJobId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015