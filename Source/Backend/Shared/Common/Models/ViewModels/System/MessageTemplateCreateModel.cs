#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    /*
    public class MessageTemplateXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long MessageTemplateId { get; set; }
    }
    */

    public class MessageTemplateCreateModel : MessageTemplateUpdateModel
    {
        protected long MessageTemplateId { get { return base.MessageTemplateId; } set { base.MessageTemplateId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015