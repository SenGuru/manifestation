using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task DefaultCategoryTest_IntegrationTest()
        {
            #region DefaultCategory
            //Tests for DefaultCategory
            //Create Item Test

            var aDefaultCategoryModel = new DefaultCategoryCreateModel();
            var rDefaultCategoryModel = await http.CreateDefaultCategory(aDefaultCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(rDefaultCategoryModel.result.IsSuccess);

            aDefaultCategoryModel = new DefaultCategoryCreateModel();
            var r0DefaultCategoryModel = await http.CreateDefaultCategory(aDefaultCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(!r0DefaultCategoryModel.result.IsSuccess);

            aDefaultCategoryModel = new DefaultCategoryCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00DefaultCategoryModel = await http.CreateDefaultCategory(aDefaultCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(r00DefaultCategoryModel.result.IsSuccess);

            aDefaultCategoryModel = new DefaultCategoryCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01DefaultCategoryModel = await http.CreateDefaultCategory(aDefaultCategoryModel, userAdmin_Company_2);
            Assert.IsTrue(!r01DefaultCategoryModel.result.IsSuccess);


            aDefaultCategoryModel = new DefaultCategoryCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01DefaultCategoryModel = await http.CreateDefaultCategory(aDefaultCategoryModel, userAdmin_Company_2);
            Assert.IsTrue(r01DefaultCategoryModel.result.IsSuccess);

            //Get List Test
            var r2DefaultCategoryModel = await http.GetDefaultCategorys(new DefaultCategorySearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2DefaultCategoryModel.result.IsSuccess);
            Assert.IsTrue(r2DefaultCategoryModel.result.Model.Any(x => x.DefaultCategoryId == rDefaultCategoryModel.result.Model.DefaultCategoryId));

            var r20DefaultCategoryModel = await http.GetDefaultCategorys(new DefaultCategorySearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20DefaultCategoryModel.result.Model.Any(x => x.DefaultCategoryId == rDefaultCategoryModel.result.Model.DefaultCategoryId));

            //Get Item Test
            var r3DefaultCategoryModel = await http.GetDefaultCategory(rDefaultCategoryModel.result.Model.DefaultCategoryId, userAdmin_Company_1);
            Assert.IsTrue(r3DefaultCategoryModel.result.IsSuccess);
            Assert.IsTrue(r3DefaultCategoryModel.result.Model.DefaultCategoryId == rDefaultCategoryModel.result.Model.DefaultCategoryId);

            var r30DefaultCategoryModel = await http.GetDefaultCategory(rDefaultCategoryModel.result.Model.DefaultCategoryId, userAdmin_Company_2);
            Assert.IsTrue(!r30DefaultCategoryModel.result.IsSuccess);

            //Edit Item Test
            var r4DefaultCategoryModel = await http.UpdateDefaultCategory(rDefaultCategoryModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4DefaultCategoryModel.result.IsSuccess);
            //Assert.IsTrue(r4DefaultCategoryModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40DefaultCategoryModel = await http.UpdateDefaultCategory(rDefaultCategoryModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40DefaultCategoryModel.result.IsSuccess);
            #endregion

            /*
            #region DefaultCategory Association with XYZZYX

            var tdefaultCategoryList0 = await http.GetDefaultCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultCategoryList0.result.IsSuccess);

            var tdefaultCategoryAssociationR = await http.AssociateDefaultCategoryWithXYZZYX(new DefaultCategoryXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DefaultCategoryId = rDefaultCategoryModel.result.Model.DefaultCategoryId }, userAdmin_Company_1);
            Assert.IsTrue(tdefaultCategoryAssociationR.result.IsSuccess);

            tdefaultCategoryList0 = await http.GetDefaultCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultCategoryList0.result.IsSuccess);
            Assert.IsTrue(tdefaultCategoryList0.result.Model.Any(x => x.DefaultCategoryId == rDefaultCategoryModel.result.Model.DefaultCategoryId));

            var tdefaultCategoryUnAssociationR = await http.UnAssociateDefaultCategoryWithXYZZYX(new DefaultCategoryXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DefaultCategoryId = rDefaultCategoryModel.result.Model.DefaultCategoryId }, userAdmin_Company_1);
            Assert.IsTrue(tdefaultCategoryUnAssociationR.result.IsSuccess);
            tdefaultCategoryList0 = await http.GetDefaultCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultCategoryList0.result.IsSuccess);
            Assert.IsTrue(!tdefaultCategoryList0.result.Model.Any(x => x.DefaultCategoryId == rDefaultCategoryModel.result.Model.DefaultCategoryId));

            #endregion
            */
        }
    }
}