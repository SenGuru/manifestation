using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task ScheduledJobHistoryTest_IntegrationTest()
        {
            #region ScheduledJobHistory
            //Tests for ScheduledJobHistory
            //Create Item Test

            var aScheduledJobHistoryModel = new ScheduledJobHistoryCreateModel();
            var rScheduledJobHistoryModel = await http.CreateScheduledJobHistory(aScheduledJobHistoryModel, userAdmin_Company_1);
            Assert.IsTrue(rScheduledJobHistoryModel.result.IsSuccess);

            aScheduledJobHistoryModel = new ScheduledJobHistoryCreateModel();
            var r0ScheduledJobHistoryModel = await http.CreateScheduledJobHistory(aScheduledJobHistoryModel, userAdmin_Company_1);
            Assert.IsTrue(!r0ScheduledJobHistoryModel.result.IsSuccess);

            aScheduledJobHistoryModel = new ScheduledJobHistoryCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00ScheduledJobHistoryModel = await http.CreateScheduledJobHistory(aScheduledJobHistoryModel, userAdmin_Company_1);
            Assert.IsTrue(r00ScheduledJobHistoryModel.result.IsSuccess);

            aScheduledJobHistoryModel = new ScheduledJobHistoryCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01ScheduledJobHistoryModel = await http.CreateScheduledJobHistory(aScheduledJobHistoryModel, userAdmin_Company_2);
            Assert.IsTrue(!r01ScheduledJobHistoryModel.result.IsSuccess);


            aScheduledJobHistoryModel = new ScheduledJobHistoryCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01ScheduledJobHistoryModel = await http.CreateScheduledJobHistory(aScheduledJobHistoryModel, userAdmin_Company_2);
            Assert.IsTrue(r01ScheduledJobHistoryModel.result.IsSuccess);

            //Get List Test
            var r2ScheduledJobHistoryModel = await http.GetScheduledJobHistorys(new ScheduledJobHistorySearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2ScheduledJobHistoryModel.result.IsSuccess);
            Assert.IsTrue(r2ScheduledJobHistoryModel.result.Model.Any(x => x.ScheduledJobHistoryId == rScheduledJobHistoryModel.result.Model.ScheduledJobHistoryId));

            var r20ScheduledJobHistoryModel = await http.GetScheduledJobHistorys(new ScheduledJobHistorySearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20ScheduledJobHistoryModel.result.Model.Any(x => x.ScheduledJobHistoryId == rScheduledJobHistoryModel.result.Model.ScheduledJobHistoryId));

            //Get Item Test
            var r3ScheduledJobHistoryModel = await http.GetScheduledJobHistory(rScheduledJobHistoryModel.result.Model.ScheduledJobHistoryId, userAdmin_Company_1);
            Assert.IsTrue(r3ScheduledJobHistoryModel.result.IsSuccess);
            Assert.IsTrue(r3ScheduledJobHistoryModel.result.Model.ScheduledJobHistoryId == rScheduledJobHistoryModel.result.Model.ScheduledJobHistoryId);

            var r30ScheduledJobHistoryModel = await http.GetScheduledJobHistory(rScheduledJobHistoryModel.result.Model.ScheduledJobHistoryId, userAdmin_Company_2);
            Assert.IsTrue(!r30ScheduledJobHistoryModel.result.IsSuccess);

            //Edit Item Test
            var r4ScheduledJobHistoryModel = await http.UpdateScheduledJobHistory(rScheduledJobHistoryModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4ScheduledJobHistoryModel.result.IsSuccess);
            //Assert.IsTrue(r4ScheduledJobHistoryModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40ScheduledJobHistoryModel = await http.UpdateScheduledJobHistory(rScheduledJobHistoryModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40ScheduledJobHistoryModel.result.IsSuccess);
            #endregion

            /*
            #region ScheduledJobHistory Association with XYZZYX

            var tscheduledJobHistoryList0 = await http.GetScheduledJobHistorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tscheduledJobHistoryList0.result.IsSuccess);

            var tscheduledJobHistoryAssociationR = await http.AssociateScheduledJobHistoryWithXYZZYX(new ScheduledJobHistoryXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ScheduledJobHistoryId = rScheduledJobHistoryModel.result.Model.ScheduledJobHistoryId }, userAdmin_Company_1);
            Assert.IsTrue(tscheduledJobHistoryAssociationR.result.IsSuccess);

            tscheduledJobHistoryList0 = await http.GetScheduledJobHistorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tscheduledJobHistoryList0.result.IsSuccess);
            Assert.IsTrue(tscheduledJobHistoryList0.result.Model.Any(x => x.ScheduledJobHistoryId == rScheduledJobHistoryModel.result.Model.ScheduledJobHistoryId));

            var tscheduledJobHistoryUnAssociationR = await http.UnAssociateScheduledJobHistoryWithXYZZYX(new ScheduledJobHistoryXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ScheduledJobHistoryId = rScheduledJobHistoryModel.result.Model.ScheduledJobHistoryId }, userAdmin_Company_1);
            Assert.IsTrue(tscheduledJobHistoryUnAssociationR.result.IsSuccess);
            tscheduledJobHistoryList0 = await http.GetScheduledJobHistorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tscheduledJobHistoryList0.result.IsSuccess);
            Assert.IsTrue(!tscheduledJobHistoryList0.result.Model.Any(x => x.ScheduledJobHistoryId == rScheduledJobHistoryModel.result.Model.ScheduledJobHistoryId));

            #endregion
            */
        }
    }
}