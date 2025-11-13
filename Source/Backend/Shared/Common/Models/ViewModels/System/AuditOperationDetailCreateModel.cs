#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels.System
{
    /*
    public class AuditOperationDetailXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long AuditOperationDetailId { get; set; }
    }
    */

    public class AuditOperationDetailCreateModel : AuditOperationDetailUpdateModel
    {
        protected long AuditOperationDetailId { get { return base.AuditOperationDetailId; } set { base.AuditOperationDetailId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015