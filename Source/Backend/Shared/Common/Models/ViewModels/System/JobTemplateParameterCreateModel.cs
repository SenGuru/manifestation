#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    /*
    public class JobTemplateParameterXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long JobTemplateParameterId { get; set; }
    }
    */

    public class JobTemplateParameterCreateModel : JobTemplateParameterUpdateModel
    {
        protected long JobTemplateParameterId { get { return base.JobTemplateParameterId; } set { base.JobTemplateParameterId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015