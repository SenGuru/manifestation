using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task DefaultGoalTrackerCategoryTest_IntegrationTest()
        {
            #region DefaultGoalTrackerCategory
            //Tests for DefaultGoalTrackerCategory
            //Create Item Test

            var aDefaultGoalTrackerCategoryModel = new DefaultGoalTrackerCategoryCreateModel();
            var rDefaultGoalTrackerCategoryModel = await http.CreateDefaultGoalTrackerCategory(aDefaultGoalTrackerCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(rDefaultGoalTrackerCategoryModel.result.IsSuccess);

            aDefaultGoalTrackerCategoryModel = new DefaultGoalTrackerCategoryCreateModel();
            var r0DefaultGoalTrackerCategoryModel = await http.CreateDefaultGoalTrackerCategory(aDefaultGoalTrackerCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(!r0DefaultGoalTrackerCategoryModel.result.IsSuccess);

            aDefaultGoalTrackerCategoryModel = new DefaultGoalTrackerCategoryCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00DefaultGoalTrackerCategoryModel = await http.CreateDefaultGoalTrackerCategory(aDefaultGoalTrackerCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(r00DefaultGoalTrackerCategoryModel.result.IsSuccess);

            aDefaultGoalTrackerCategoryModel = new DefaultGoalTrackerCategoryCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01DefaultGoalTrackerCategoryModel = await http.CreateDefaultGoalTrackerCategory(aDefaultGoalTrackerCategoryModel, userAdmin_Company_2);
            Assert.IsTrue(!r01DefaultGoalTrackerCategoryModel.result.IsSuccess);


            aDefaultGoalTrackerCategoryModel = new DefaultGoalTrackerCategoryCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01DefaultGoalTrackerCategoryModel = await http.CreateDefaultGoalTrackerCategory(aDefaultGoalTrackerCategoryModel, userAdmin_Company_2);
            Assert.IsTrue(r01DefaultGoalTrackerCategoryModel.result.IsSuccess);

            //Get List Test
            var r2DefaultGoalTrackerCategoryModel = await http.GetDefaultGoalTrackerCategorys(new DefaultGoalTrackerCategorySearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2DefaultGoalTrackerCategoryModel.result.IsSuccess);
            Assert.IsTrue(r2DefaultGoalTrackerCategoryModel.result.Model.Any(x => x.DefaultGoalTrackerCategoryId == rDefaultGoalTrackerCategoryModel.result.Model.DefaultGoalTrackerCategoryId));

            var r20DefaultGoalTrackerCategoryModel = await http.GetDefaultGoalTrackerCategorys(new DefaultGoalTrackerCategorySearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20DefaultGoalTrackerCategoryModel.result.Model.Any(x => x.DefaultGoalTrackerCategoryId == rDefaultGoalTrackerCategoryModel.result.Model.DefaultGoalTrackerCategoryId));

            //Get Item Test
            var r3DefaultGoalTrackerCategoryModel = await http.GetDefaultGoalTrackerCategory(rDefaultGoalTrackerCategoryModel.result.Model.DefaultGoalTrackerCategoryId, userAdmin_Company_1);
            Assert.IsTrue(r3DefaultGoalTrackerCategoryModel.result.IsSuccess);
            Assert.IsTrue(r3DefaultGoalTrackerCategoryModel.result.Model.DefaultGoalTrackerCategoryId == rDefaultGoalTrackerCategoryModel.result.Model.DefaultGoalTrackerCategoryId);

            var r30DefaultGoalTrackerCategoryModel = await http.GetDefaultGoalTrackerCategory(rDefaultGoalTrackerCategoryModel.result.Model.DefaultGoalTrackerCategoryId, userAdmin_Company_2);
            Assert.IsTrue(!r30DefaultGoalTrackerCategoryModel.result.IsSuccess);

            //Edit Item Test
            var r4DefaultGoalTrackerCategoryModel = await http.UpdateDefaultGoalTrackerCategory(rDefaultGoalTrackerCategoryModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4DefaultGoalTrackerCategoryModel.result.IsSuccess);
            //Assert.IsTrue(r4DefaultGoalTrackerCategoryModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40DefaultGoalTrackerCategoryModel = await http.UpdateDefaultGoalTrackerCategory(rDefaultGoalTrackerCategoryModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40DefaultGoalTrackerCategoryModel.result.IsSuccess);
            #endregion

            /*
            #region DefaultGoalTrackerCategory Association with XYZZYX

            var tdefaultGoalTrackerCategoryList0 = await http.GetDefaultGoalTrackerCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultGoalTrackerCategoryList0.result.IsSuccess);

            var tdefaultGoalTrackerCategoryAssociationR = await http.AssociateDefaultGoalTrackerCategoryWithXYZZYX(new DefaultGoalTrackerCategoryXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DefaultGoalTrackerCategoryId = rDefaultGoalTrackerCategoryModel.result.Model.DefaultGoalTrackerCategoryId }, userAdmin_Company_1);
            Assert.IsTrue(tdefaultGoalTrackerCategoryAssociationR.result.IsSuccess);

            tdefaultGoalTrackerCategoryList0 = await http.GetDefaultGoalTrackerCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultGoalTrackerCategoryList0.result.IsSuccess);
            Assert.IsTrue(tdefaultGoalTrackerCategoryList0.result.Model.Any(x => x.DefaultGoalTrackerCategoryId == rDefaultGoalTrackerCategoryModel.result.Model.DefaultGoalTrackerCategoryId));

            var tdefaultGoalTrackerCategoryUnAssociationR = await http.UnAssociateDefaultGoalTrackerCategoryWithXYZZYX(new DefaultGoalTrackerCategoryXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DefaultGoalTrackerCategoryId = rDefaultGoalTrackerCategoryModel.result.Model.DefaultGoalTrackerCategoryId }, userAdmin_Company_1);
            Assert.IsTrue(tdefaultGoalTrackerCategoryUnAssociationR.result.IsSuccess);
            tdefaultGoalTrackerCategoryList0 = await http.GetDefaultGoalTrackerCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultGoalTrackerCategoryList0.result.IsSuccess);
            Assert.IsTrue(!tdefaultGoalTrackerCategoryList0.result.Model.Any(x => x.DefaultGoalTrackerCategoryId == rDefaultGoalTrackerCategoryModel.result.Model.DefaultGoalTrackerCategoryId));

            #endregion
            */
        }
    }
}