using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task CategoryTest_IntegrationTest()
        {
            #region Category
            //Tests for Category
            //Create Item Test

            var aCategoryModel = new CategoryCreateModel();
            var rCategoryModel = await http.CreateCategory(aCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(rCategoryModel.result.IsSuccess);

            aCategoryModel = new CategoryCreateModel();
            var r0CategoryModel = await http.CreateCategory(aCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(!r0CategoryModel.result.IsSuccess);

            aCategoryModel = new CategoryCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00CategoryModel = await http.CreateCategory(aCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(r00CategoryModel.result.IsSuccess);

            aCategoryModel = new CategoryCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01CategoryModel = await http.CreateCategory(aCategoryModel, userAdmin_Company_2);
            Assert.IsTrue(!r01CategoryModel.result.IsSuccess);


            aCategoryModel = new CategoryCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01CategoryModel = await http.CreateCategory(aCategoryModel, userAdmin_Company_2);
            Assert.IsTrue(r01CategoryModel.result.IsSuccess);

            //Get List Test
            var r2CategoryModel = await http.GetCategorys(new CategorySearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2CategoryModel.result.IsSuccess);
            Assert.IsTrue(r2CategoryModel.result.Model.Any(x => x.CategoryId == rCategoryModel.result.Model.CategoryId));

            var r20CategoryModel = await http.GetCategorys(new CategorySearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20CategoryModel.result.Model.Any(x => x.CategoryId == rCategoryModel.result.Model.CategoryId));

            //Get Item Test
            var r3CategoryModel = await http.GetCategory(rCategoryModel.result.Model.CategoryId, userAdmin_Company_1);
            Assert.IsTrue(r3CategoryModel.result.IsSuccess);
            Assert.IsTrue(r3CategoryModel.result.Model.CategoryId == rCategoryModel.result.Model.CategoryId);

            var r30CategoryModel = await http.GetCategory(rCategoryModel.result.Model.CategoryId, userAdmin_Company_2);
            Assert.IsTrue(!r30CategoryModel.result.IsSuccess);

            //Edit Item Test
            var r4CategoryModel = await http.UpdateCategory(rCategoryModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4CategoryModel.result.IsSuccess);
            //Assert.IsTrue(r4CategoryModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40CategoryModel = await http.UpdateCategory(rCategoryModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40CategoryModel.result.IsSuccess);
            #endregion

            /*
            #region Category Association with XYZZYX

            var tcategoryList0 = await http.GetCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tcategoryList0.result.IsSuccess);

            var tcategoryAssociationR = await http.AssociateCategoryWithXYZZYX(new CategoryXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, CategoryId = rCategoryModel.result.Model.CategoryId }, userAdmin_Company_1);
            Assert.IsTrue(tcategoryAssociationR.result.IsSuccess);

            tcategoryList0 = await http.GetCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tcategoryList0.result.IsSuccess);
            Assert.IsTrue(tcategoryList0.result.Model.Any(x => x.CategoryId == rCategoryModel.result.Model.CategoryId));

            var tcategoryUnAssociationR = await http.UnAssociateCategoryWithXYZZYX(new CategoryXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, CategoryId = rCategoryModel.result.Model.CategoryId }, userAdmin_Company_1);
            Assert.IsTrue(tcategoryUnAssociationR.result.IsSuccess);
            tcategoryList0 = await http.GetCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tcategoryList0.result.IsSuccess);
            Assert.IsTrue(!tcategoryList0.result.Model.Any(x => x.CategoryId == rCategoryModel.result.Model.CategoryId));

            #endregion
            */
        }
    }
}