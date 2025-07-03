using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task ScheduledJobTest_IntegrationTest()
        {
            #region ScheduledJob
            //Tests for ScheduledJob
            //Create Item Test

            var aScheduledJobModel = new ScheduledJobCreateModel();
            var rScheduledJobModel = await http.CreateScheduledJob(aScheduledJobModel, userAdmin_Company_1);
            Assert.IsTrue(rScheduledJobModel.result.IsSuccess);

            aScheduledJobModel = new ScheduledJobCreateModel();
            var r0ScheduledJobModel = await http.CreateScheduledJob(aScheduledJobModel, userAdmin_Company_1);
            Assert.IsTrue(!r0ScheduledJobModel.result.IsSuccess);

            aScheduledJobModel = new ScheduledJobCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00ScheduledJobModel = await http.CreateScheduledJob(aScheduledJobModel, userAdmin_Company_1);
            Assert.IsTrue(r00ScheduledJobModel.result.IsSuccess);

            aScheduledJobModel = new ScheduledJobCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01ScheduledJobModel = await http.CreateScheduledJob(aScheduledJobModel, userAdmin_Company_2);
            Assert.IsTrue(!r01ScheduledJobModel.result.IsSuccess);


            aScheduledJobModel = new ScheduledJobCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01ScheduledJobModel = await http.CreateScheduledJob(aScheduledJobModel, userAdmin_Company_2);
            Assert.IsTrue(r01ScheduledJobModel.result.IsSuccess);

            //Get List Test
            var r2ScheduledJobModel = await http.GetScheduledJobs(new ScheduledJobSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2ScheduledJobModel.result.IsSuccess);
            Assert.IsTrue(r2ScheduledJobModel.result.Model.Any(x => x.ScheduledJobId == rScheduledJobModel.result.Model.ScheduledJobId));

            var r20ScheduledJobModel = await http.GetScheduledJobs(new ScheduledJobSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20ScheduledJobModel.result.Model.Any(x => x.ScheduledJobId == rScheduledJobModel.result.Model.ScheduledJobId));

            //Get Item Test
            var r3ScheduledJobModel = await http.GetScheduledJob(rScheduledJobModel.result.Model.ScheduledJobId, userAdmin_Company_1);
            Assert.IsTrue(r3ScheduledJobModel.result.IsSuccess);
            Assert.IsTrue(r3ScheduledJobModel.result.Model.ScheduledJobId == rScheduledJobModel.result.Model.ScheduledJobId);

            var r30ScheduledJobModel = await http.GetScheduledJob(rScheduledJobModel.result.Model.ScheduledJobId, userAdmin_Company_2);
            Assert.IsTrue(!r30ScheduledJobModel.result.IsSuccess);

            //Edit Item Test
            var r4ScheduledJobModel = await http.UpdateScheduledJob(rScheduledJobModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4ScheduledJobModel.result.IsSuccess);
            //Assert.IsTrue(r4ScheduledJobModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40ScheduledJobModel = await http.UpdateScheduledJob(rScheduledJobModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40ScheduledJobModel.result.IsSuccess);
            #endregion

            /*
            #region ScheduledJob Association with XYZZYX

            var tscheduledJobList0 = await http.GetScheduledJobsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tscheduledJobList0.result.IsSuccess);

            var tscheduledJobAssociationR = await http.AssociateScheduledJobWithXYZZYX(new ScheduledJobXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ScheduledJobId = rScheduledJobModel.result.Model.ScheduledJobId }, userAdmin_Company_1);
            Assert.IsTrue(tscheduledJobAssociationR.result.IsSuccess);

            tscheduledJobList0 = await http.GetScheduledJobsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tscheduledJobList0.result.IsSuccess);
            Assert.IsTrue(tscheduledJobList0.result.Model.Any(x => x.ScheduledJobId == rScheduledJobModel.result.Model.ScheduledJobId));

            var tscheduledJobUnAssociationR = await http.UnAssociateScheduledJobWithXYZZYX(new ScheduledJobXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ScheduledJobId = rScheduledJobModel.result.Model.ScheduledJobId }, userAdmin_Company_1);
            Assert.IsTrue(tscheduledJobUnAssociationR.result.IsSuccess);
            tscheduledJobList0 = await http.GetScheduledJobsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tscheduledJobList0.result.IsSuccess);
            Assert.IsTrue(!tscheduledJobList0.result.Model.Any(x => x.ScheduledJobId == rScheduledJobModel.result.Model.ScheduledJobId));

            #endregion
            */
        }
    }
}