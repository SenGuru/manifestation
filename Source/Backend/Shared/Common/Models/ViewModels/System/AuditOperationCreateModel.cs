#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    /*
    public class AuditOperationXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long AuditOperationId { get; set; }
    }
    */

    public class AuditOperationCreateModel : AuditOperationUpdateModel
    {
        protected long AuditOperationId { get { return base.AuditOperationId; } set { base.AuditOperationId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015