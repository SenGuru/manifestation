#pragma warning disable CS0108,SA1502,S4015, SA1025
using System;
using %AppName%.Common.Models.DB;

namespace %AppName%.Common.Models.ViewModels
{
    public class EEntity123UpdateModel : EEntity123
    {
        //public List<EEntity123XYZZYXCreateModel> NewEEntity123XYZZYXModels { get; set; }
        //public List<EEntity123XYZZYXUpdateModel> ExistingEEntity123XYZZYXModels { get; set; }
        //protected long CompanyId { get { return base.CompanyId; } set { base.CompanyId = value; } }

        protected bool IsDeleted { get { return base.IsDeleted; } set { base.IsDeleted = value; } }

        protected DateTime CreatedDate { get { return base.CreatedDate; } set { base.CreatedDate = value; } }

        protected long? CreatedByUserId { get { return base.CreatedByUserId; } set { base.CreatedByUserId = value; } }

        protected DateTime ModifiedDate { get { return base.ModifiedDate; } set { base.ModifiedDate = value; } }

        protected long? ModifiedByUserId { get { return base.ModifiedByUserId; } set { base.ModifiedByUserId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015, SA1025