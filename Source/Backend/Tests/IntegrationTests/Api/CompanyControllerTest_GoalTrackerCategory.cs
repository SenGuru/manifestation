using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task GoalTrackerCategoryTest_IntegrationTest()
        {
            #region GoalTrackerCategory
            //Tests for GoalTrackerCategory
            //Create Item Test

            var aGoalTrackerCategoryModel = new GoalTrackerCategoryCreateModel();
            var rGoalTrackerCategoryModel = await http.CreateGoalTrackerCategory(aGoalTrackerCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(rGoalTrackerCategoryModel.result.IsSuccess);

            aGoalTrackerCategoryModel = new GoalTrackerCategoryCreateModel();
            var r0GoalTrackerCategoryModel = await http.CreateGoalTrackerCategory(aGoalTrackerCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(!r0GoalTrackerCategoryModel.result.IsSuccess);

            aGoalTrackerCategoryModel = new GoalTrackerCategoryCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00GoalTrackerCategoryModel = await http.CreateGoalTrackerCategory(aGoalTrackerCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(r00GoalTrackerCategoryModel.result.IsSuccess);

            aGoalTrackerCategoryModel = new GoalTrackerCategoryCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01GoalTrackerCategoryModel = await http.CreateGoalTrackerCategory(aGoalTrackerCategoryModel, userAdmin_Company_2);
            Assert.IsTrue(!r01GoalTrackerCategoryModel.result.IsSuccess);


            aGoalTrackerCategoryModel = new GoalTrackerCategoryCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01GoalTrackerCategoryModel = await http.CreateGoalTrackerCategory(aGoalTrackerCategoryModel, userAdmin_Company_2);
            Assert.IsTrue(r01GoalTrackerCategoryModel.result.IsSuccess);

            //Get List Test
            var r2GoalTrackerCategoryModel = await http.GetGoalTrackerCategorys(new GoalTrackerCategorySearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2GoalTrackerCategoryModel.result.IsSuccess);
            Assert.IsTrue(r2GoalTrackerCategoryModel.result.Model.Any(x => x.GoalTrackerCategoryId == rGoalTrackerCategoryModel.result.Model.GoalTrackerCategoryId));

            var r20GoalTrackerCategoryModel = await http.GetGoalTrackerCategorys(new GoalTrackerCategorySearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20GoalTrackerCategoryModel.result.Model.Any(x => x.GoalTrackerCategoryId == rGoalTrackerCategoryModel.result.Model.GoalTrackerCategoryId));

            //Get Item Test
            var r3GoalTrackerCategoryModel = await http.GetGoalTrackerCategory(rGoalTrackerCategoryModel.result.Model.GoalTrackerCategoryId, userAdmin_Company_1);
            Assert.IsTrue(r3GoalTrackerCategoryModel.result.IsSuccess);
            Assert.IsTrue(r3GoalTrackerCategoryModel.result.Model.GoalTrackerCategoryId == rGoalTrackerCategoryModel.result.Model.GoalTrackerCategoryId);

            var r30GoalTrackerCategoryModel = await http.GetGoalTrackerCategory(rGoalTrackerCategoryModel.result.Model.GoalTrackerCategoryId, userAdmin_Company_2);
            Assert.IsTrue(!r30GoalTrackerCategoryModel.result.IsSuccess);

            //Edit Item Test
            var r4GoalTrackerCategoryModel = await http.UpdateGoalTrackerCategory(rGoalTrackerCategoryModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4GoalTrackerCategoryModel.result.IsSuccess);
            //Assert.IsTrue(r4GoalTrackerCategoryModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40GoalTrackerCategoryModel = await http.UpdateGoalTrackerCategory(rGoalTrackerCategoryModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40GoalTrackerCategoryModel.result.IsSuccess);
            #endregion

            /*
            #region GoalTrackerCategory Association with XYZZYX

            var tgoalTrackerCategoryList0 = await http.GetGoalTrackerCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tgoalTrackerCategoryList0.result.IsSuccess);

            var tgoalTrackerCategoryAssociationR = await http.AssociateGoalTrackerCategoryWithXYZZYX(new GoalTrackerCategoryXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, GoalTrackerCategoryId = rGoalTrackerCategoryModel.result.Model.GoalTrackerCategoryId }, userAdmin_Company_1);
            Assert.IsTrue(tgoalTrackerCategoryAssociationR.result.IsSuccess);

            tgoalTrackerCategoryList0 = await http.GetGoalTrackerCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tgoalTrackerCategoryList0.result.IsSuccess);
            Assert.IsTrue(tgoalTrackerCategoryList0.result.Model.Any(x => x.GoalTrackerCategoryId == rGoalTrackerCategoryModel.result.Model.GoalTrackerCategoryId));

            var tgoalTrackerCategoryUnAssociationR = await http.UnAssociateGoalTrackerCategoryWithXYZZYX(new GoalTrackerCategoryXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, GoalTrackerCategoryId = rGoalTrackerCategoryModel.result.Model.GoalTrackerCategoryId }, userAdmin_Company_1);
            Assert.IsTrue(tgoalTrackerCategoryUnAssociationR.result.IsSuccess);
            tgoalTrackerCategoryList0 = await http.GetGoalTrackerCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tgoalTrackerCategoryList0.result.IsSuccess);
            Assert.IsTrue(!tgoalTrackerCategoryList0.result.Model.Any(x => x.GoalTrackerCategoryId == rGoalTrackerCategoryModel.result.Model.GoalTrackerCategoryId));

            #endregion
            */
        }
    }
}