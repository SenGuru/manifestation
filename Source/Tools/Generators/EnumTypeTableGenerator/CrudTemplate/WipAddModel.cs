#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace %AppName%.Common.Models.ViewModels
{
    //%ParentCommentBegin%
    public class EEntity123XYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long EEntity123Id { get; set; }
    }
    //%ParentCommentEnd%

    public class EEntity123CreateModel : EEntity123UpdateModel
    {
        protected long EEntity123Id { get { return base.EEntity123Id; } set { base.EEntity123Id = value; } }
        //protected List<EEntity123XYZZYXModel> ExistingEEntity123XYZZYXModels {get {return base.ExistingEEntity123XYZZYXModels;} set { base.ExistingEEntity123XYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015