using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task DefaultGoalTrackerTest_IntegrationTest()
        {
            #region DefaultGoalTracker
            //Tests for DefaultGoalTracker
            //Create Item Test

            var aDefaultGoalTrackerModel = new DefaultGoalTrackerCreateModel();
            var rDefaultGoalTrackerModel = await http.CreateDefaultGoalTracker(aDefaultGoalTrackerModel, userAdmin_Company_1);
            Assert.IsTrue(rDefaultGoalTrackerModel.result.IsSuccess);

            aDefaultGoalTrackerModel = new DefaultGoalTrackerCreateModel();
            var r0DefaultGoalTrackerModel = await http.CreateDefaultGoalTracker(aDefaultGoalTrackerModel, userAdmin_Company_1);
            Assert.IsTrue(!r0DefaultGoalTrackerModel.result.IsSuccess);

            aDefaultGoalTrackerModel = new DefaultGoalTrackerCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00DefaultGoalTrackerModel = await http.CreateDefaultGoalTracker(aDefaultGoalTrackerModel, userAdmin_Company_1);
            Assert.IsTrue(r00DefaultGoalTrackerModel.result.IsSuccess);

            aDefaultGoalTrackerModel = new DefaultGoalTrackerCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01DefaultGoalTrackerModel = await http.CreateDefaultGoalTracker(aDefaultGoalTrackerModel, userAdmin_Company_2);
            Assert.IsTrue(!r01DefaultGoalTrackerModel.result.IsSuccess);


            aDefaultGoalTrackerModel = new DefaultGoalTrackerCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01DefaultGoalTrackerModel = await http.CreateDefaultGoalTracker(aDefaultGoalTrackerModel, userAdmin_Company_2);
            Assert.IsTrue(r01DefaultGoalTrackerModel.result.IsSuccess);

            //Get List Test
            var r2DefaultGoalTrackerModel = await http.GetDefaultGoalTrackers(new DefaultGoalTrackerSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2DefaultGoalTrackerModel.result.IsSuccess);
            Assert.IsTrue(r2DefaultGoalTrackerModel.result.Model.Any(x => x.DefaultGoalTrackerId == rDefaultGoalTrackerModel.result.Model.DefaultGoalTrackerId));

            var r20DefaultGoalTrackerModel = await http.GetDefaultGoalTrackers(new DefaultGoalTrackerSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20DefaultGoalTrackerModel.result.Model.Any(x => x.DefaultGoalTrackerId == rDefaultGoalTrackerModel.result.Model.DefaultGoalTrackerId));

            //Get Item Test
            var r3DefaultGoalTrackerModel = await http.GetDefaultGoalTracker(rDefaultGoalTrackerModel.result.Model.DefaultGoalTrackerId, userAdmin_Company_1);
            Assert.IsTrue(r3DefaultGoalTrackerModel.result.IsSuccess);
            Assert.IsTrue(r3DefaultGoalTrackerModel.result.Model.DefaultGoalTrackerId == rDefaultGoalTrackerModel.result.Model.DefaultGoalTrackerId);

            var r30DefaultGoalTrackerModel = await http.GetDefaultGoalTracker(rDefaultGoalTrackerModel.result.Model.DefaultGoalTrackerId, userAdmin_Company_2);
            Assert.IsTrue(!r30DefaultGoalTrackerModel.result.IsSuccess);

            //Edit Item Test
            var r4DefaultGoalTrackerModel = await http.UpdateDefaultGoalTracker(rDefaultGoalTrackerModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4DefaultGoalTrackerModel.result.IsSuccess);
            //Assert.IsTrue(r4DefaultGoalTrackerModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40DefaultGoalTrackerModel = await http.UpdateDefaultGoalTracker(rDefaultGoalTrackerModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40DefaultGoalTrackerModel.result.IsSuccess);
            #endregion

            /*
            #region DefaultGoalTracker Association with XYZZYX

            var tdefaultGoalTrackerList0 = await http.GetDefaultGoalTrackersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultGoalTrackerList0.result.IsSuccess);

            var tdefaultGoalTrackerAssociationR = await http.AssociateDefaultGoalTrackerWithXYZZYX(new DefaultGoalTrackerXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DefaultGoalTrackerId = rDefaultGoalTrackerModel.result.Model.DefaultGoalTrackerId }, userAdmin_Company_1);
            Assert.IsTrue(tdefaultGoalTrackerAssociationR.result.IsSuccess);

            tdefaultGoalTrackerList0 = await http.GetDefaultGoalTrackersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultGoalTrackerList0.result.IsSuccess);
            Assert.IsTrue(tdefaultGoalTrackerList0.result.Model.Any(x => x.DefaultGoalTrackerId == rDefaultGoalTrackerModel.result.Model.DefaultGoalTrackerId));

            var tdefaultGoalTrackerUnAssociationR = await http.UnAssociateDefaultGoalTrackerWithXYZZYX(new DefaultGoalTrackerXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DefaultGoalTrackerId = rDefaultGoalTrackerModel.result.Model.DefaultGoalTrackerId }, userAdmin_Company_1);
            Assert.IsTrue(tdefaultGoalTrackerUnAssociationR.result.IsSuccess);
            tdefaultGoalTrackerList0 = await http.GetDefaultGoalTrackersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultGoalTrackerList0.result.IsSuccess);
            Assert.IsTrue(!tdefaultGoalTrackerList0.result.Model.Any(x => x.DefaultGoalTrackerId == rDefaultGoalTrackerModel.result.Model.DefaultGoalTrackerId));

            #endregion
            */
        }
    }
}