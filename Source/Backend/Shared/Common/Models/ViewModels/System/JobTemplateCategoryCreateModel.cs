#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    /*
    public class JobTemplateCategoryXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long JobTemplateCategoryId { get; set; }
    }
    */

    public class JobTemplateCategoryCreateModel : JobTemplateCategoryUpdateModel
    {
        protected long JobTemplateCategoryId { get { return base.JobTemplateCategoryId; } set { base.JobTemplateCategoryId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015