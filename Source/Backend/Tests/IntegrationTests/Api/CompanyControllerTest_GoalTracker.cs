using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task GoalTrackerTest_IntegrationTest()
        {
            #region GoalTracker
            //Tests for GoalTracker
            //Create Item Test

            var aGoalTrackerModel = new GoalTrackerCreateModel();
            var rGoalTrackerModel = await http.CreateGoalTracker(aGoalTrackerModel, userAdmin_Company_1);
            Assert.IsTrue(rGoalTrackerModel.result.IsSuccess);

            aGoalTrackerModel = new GoalTrackerCreateModel();
            var r0GoalTrackerModel = await http.CreateGoalTracker(aGoalTrackerModel, userAdmin_Company_1);
            Assert.IsTrue(!r0GoalTrackerModel.result.IsSuccess);

            aGoalTrackerModel = new GoalTrackerCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00GoalTrackerModel = await http.CreateGoalTracker(aGoalTrackerModel, userAdmin_Company_1);
            Assert.IsTrue(r00GoalTrackerModel.result.IsSuccess);

            aGoalTrackerModel = new GoalTrackerCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01GoalTrackerModel = await http.CreateGoalTracker(aGoalTrackerModel, userAdmin_Company_2);
            Assert.IsTrue(!r01GoalTrackerModel.result.IsSuccess);


            aGoalTrackerModel = new GoalTrackerCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01GoalTrackerModel = await http.CreateGoalTracker(aGoalTrackerModel, userAdmin_Company_2);
            Assert.IsTrue(r01GoalTrackerModel.result.IsSuccess);

            //Get List Test
            var r2GoalTrackerModel = await http.GetGoalTrackers(new GoalTrackerSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2GoalTrackerModel.result.IsSuccess);
            Assert.IsTrue(r2GoalTrackerModel.result.Model.Any(x => x.GoalTrackerId == rGoalTrackerModel.result.Model.GoalTrackerId));

            var r20GoalTrackerModel = await http.GetGoalTrackers(new GoalTrackerSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20GoalTrackerModel.result.Model.Any(x => x.GoalTrackerId == rGoalTrackerModel.result.Model.GoalTrackerId));

            //Get Item Test
            var r3GoalTrackerModel = await http.GetGoalTracker(rGoalTrackerModel.result.Model.GoalTrackerId, userAdmin_Company_1);
            Assert.IsTrue(r3GoalTrackerModel.result.IsSuccess);
            Assert.IsTrue(r3GoalTrackerModel.result.Model.GoalTrackerId == rGoalTrackerModel.result.Model.GoalTrackerId);

            var r30GoalTrackerModel = await http.GetGoalTracker(rGoalTrackerModel.result.Model.GoalTrackerId, userAdmin_Company_2);
            Assert.IsTrue(!r30GoalTrackerModel.result.IsSuccess);

            //Edit Item Test
            var r4GoalTrackerModel = await http.UpdateGoalTracker(rGoalTrackerModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4GoalTrackerModel.result.IsSuccess);
            //Assert.IsTrue(r4GoalTrackerModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40GoalTrackerModel = await http.UpdateGoalTracker(rGoalTrackerModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40GoalTrackerModel.result.IsSuccess);
            #endregion

            /*
            #region GoalTracker Association with XYZZYX

            var tgoalTrackerList0 = await http.GetGoalTrackersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tgoalTrackerList0.result.IsSuccess);

            var tgoalTrackerAssociationR = await http.AssociateGoalTrackerWithXYZZYX(new GoalTrackerXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, GoalTrackerId = rGoalTrackerModel.result.Model.GoalTrackerId }, userAdmin_Company_1);
            Assert.IsTrue(tgoalTrackerAssociationR.result.IsSuccess);

            tgoalTrackerList0 = await http.GetGoalTrackersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tgoalTrackerList0.result.IsSuccess);
            Assert.IsTrue(tgoalTrackerList0.result.Model.Any(x => x.GoalTrackerId == rGoalTrackerModel.result.Model.GoalTrackerId));

            var tgoalTrackerUnAssociationR = await http.UnAssociateGoalTrackerWithXYZZYX(new GoalTrackerXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, GoalTrackerId = rGoalTrackerModel.result.Model.GoalTrackerId }, userAdmin_Company_1);
            Assert.IsTrue(tgoalTrackerUnAssociationR.result.IsSuccess);
            tgoalTrackerList0 = await http.GetGoalTrackersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tgoalTrackerList0.result.IsSuccess);
            Assert.IsTrue(!tgoalTrackerList0.result.Model.Any(x => x.GoalTrackerId == rGoalTrackerModel.result.Model.GoalTrackerId));

            #endregion
            */
        }
    }
}