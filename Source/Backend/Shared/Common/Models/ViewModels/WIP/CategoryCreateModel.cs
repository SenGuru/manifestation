#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class CategoryXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long CategoryId { get; set; }
    }
    */

    public class CategoryCreateModel : CategoryUpdateModel
    {
        protected long CategoryId { get { return base.CategoryId; } set { base.CategoryId = value; } }
        //protected List<CategoryXYZZYXModel> ExistingCategoryXYZZYXModels {get {return base.ExistingCategoryXYZZYXModels;} set { base.ExistingCategoryXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015