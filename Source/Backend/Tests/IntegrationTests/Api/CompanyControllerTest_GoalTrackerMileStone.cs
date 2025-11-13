using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task GoalTrackerMileStoneTest_IntegrationTest()
        {
            #region GoalTrackerMileStone
            //Tests for GoalTrackerMileStone
            //Create Item Test

            var aGoalTrackerMileStoneModel = new GoalTrackerMileStoneCreateModel();
            var rGoalTrackerMileStoneModel = await http.CreateGoalTrackerMileStone(aGoalTrackerMileStoneModel, userAdmin_Company_1);
            Assert.IsTrue(rGoalTrackerMileStoneModel.result.IsSuccess);

            aGoalTrackerMileStoneModel = new GoalTrackerMileStoneCreateModel();
            var r0GoalTrackerMileStoneModel = await http.CreateGoalTrackerMileStone(aGoalTrackerMileStoneModel, userAdmin_Company_1);
            Assert.IsTrue(!r0GoalTrackerMileStoneModel.result.IsSuccess);

            aGoalTrackerMileStoneModel = new GoalTrackerMileStoneCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00GoalTrackerMileStoneModel = await http.CreateGoalTrackerMileStone(aGoalTrackerMileStoneModel, userAdmin_Company_1);
            Assert.IsTrue(r00GoalTrackerMileStoneModel.result.IsSuccess);

            aGoalTrackerMileStoneModel = new GoalTrackerMileStoneCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01GoalTrackerMileStoneModel = await http.CreateGoalTrackerMileStone(aGoalTrackerMileStoneModel, userAdmin_Company_2);
            Assert.IsTrue(!r01GoalTrackerMileStoneModel.result.IsSuccess);


            aGoalTrackerMileStoneModel = new GoalTrackerMileStoneCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01GoalTrackerMileStoneModel = await http.CreateGoalTrackerMileStone(aGoalTrackerMileStoneModel, userAdmin_Company_2);
            Assert.IsTrue(r01GoalTrackerMileStoneModel.result.IsSuccess);

            //Get List Test
            var r2GoalTrackerMileStoneModel = await http.GetGoalTrackerMileStones(new GoalTrackerMileStoneSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2GoalTrackerMileStoneModel.result.IsSuccess);
            Assert.IsTrue(r2GoalTrackerMileStoneModel.result.Model.Any(x => x.GoalTrackerMileStoneId == rGoalTrackerMileStoneModel.result.Model.GoalTrackerMileStoneId));

            var r20GoalTrackerMileStoneModel = await http.GetGoalTrackerMileStones(new GoalTrackerMileStoneSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20GoalTrackerMileStoneModel.result.Model.Any(x => x.GoalTrackerMileStoneId == rGoalTrackerMileStoneModel.result.Model.GoalTrackerMileStoneId));

            //Get Item Test
            var r3GoalTrackerMileStoneModel = await http.GetGoalTrackerMileStone(rGoalTrackerMileStoneModel.result.Model.GoalTrackerMileStoneId, userAdmin_Company_1);
            Assert.IsTrue(r3GoalTrackerMileStoneModel.result.IsSuccess);
            Assert.IsTrue(r3GoalTrackerMileStoneModel.result.Model.GoalTrackerMileStoneId == rGoalTrackerMileStoneModel.result.Model.GoalTrackerMileStoneId);

            var r30GoalTrackerMileStoneModel = await http.GetGoalTrackerMileStone(rGoalTrackerMileStoneModel.result.Model.GoalTrackerMileStoneId, userAdmin_Company_2);
            Assert.IsTrue(!r30GoalTrackerMileStoneModel.result.IsSuccess);

            //Edit Item Test
            var r4GoalTrackerMileStoneModel = await http.UpdateGoalTrackerMileStone(rGoalTrackerMileStoneModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4GoalTrackerMileStoneModel.result.IsSuccess);
            //Assert.IsTrue(r4GoalTrackerMileStoneModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40GoalTrackerMileStoneModel = await http.UpdateGoalTrackerMileStone(rGoalTrackerMileStoneModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40GoalTrackerMileStoneModel.result.IsSuccess);
            #endregion

            /*
            #region GoalTrackerMileStone Association with XYZZYX

            var tgoalTrackerMileStoneList0 = await http.GetGoalTrackerMileStonesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tgoalTrackerMileStoneList0.result.IsSuccess);

            var tgoalTrackerMileStoneAssociationR = await http.AssociateGoalTrackerMileStoneWithXYZZYX(new GoalTrackerMileStoneXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, GoalTrackerMileStoneId = rGoalTrackerMileStoneModel.result.Model.GoalTrackerMileStoneId }, userAdmin_Company_1);
            Assert.IsTrue(tgoalTrackerMileStoneAssociationR.result.IsSuccess);

            tgoalTrackerMileStoneList0 = await http.GetGoalTrackerMileStonesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tgoalTrackerMileStoneList0.result.IsSuccess);
            Assert.IsTrue(tgoalTrackerMileStoneList0.result.Model.Any(x => x.GoalTrackerMileStoneId == rGoalTrackerMileStoneModel.result.Model.GoalTrackerMileStoneId));

            var tgoalTrackerMileStoneUnAssociationR = await http.UnAssociateGoalTrackerMileStoneWithXYZZYX(new GoalTrackerMileStoneXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, GoalTrackerMileStoneId = rGoalTrackerMileStoneModel.result.Model.GoalTrackerMileStoneId }, userAdmin_Company_1);
            Assert.IsTrue(tgoalTrackerMileStoneUnAssociationR.result.IsSuccess);
            tgoalTrackerMileStoneList0 = await http.GetGoalTrackerMileStonesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tgoalTrackerMileStoneList0.result.IsSuccess);
            Assert.IsTrue(!tgoalTrackerMileStoneList0.result.Model.Any(x => x.GoalTrackerMileStoneId == rGoalTrackerMileStoneModel.result.Model.GoalTrackerMileStoneId));

            #endregion
            */
        }
    }
}