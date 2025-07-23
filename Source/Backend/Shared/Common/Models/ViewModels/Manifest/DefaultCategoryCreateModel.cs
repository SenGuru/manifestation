#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class DefaultCategoryXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long DefaultCategoryId { get; set; }
    }
    */

    public class DefaultCategoryCreateModel : DefaultCategoryUpdateModel
    {
        protected long DefaultCategoryId { get { return base.DefaultCategoryId; } set { base.DefaultCategoryId = value; } }
        //protected List<DefaultCategoryXYZZYXModel> ExistingDefaultCategoryXYZZYXModels {get {return base.ExistingDefaultCategoryXYZZYXModels;} set { base.ExistingDefaultCategoryXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015