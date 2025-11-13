using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task JobTest_IntegrationTest()
        {
            #region Job
            //Tests for Job
            //Create Item Test

            var aJobModel = new JobCreateModel();
            var rJobModel = await http.CreateJob(aJobModel, userAdmin_Company_1);
            Assert.IsTrue(rJobModel.result.IsSuccess);

            aJobModel = new JobCreateModel();
            var r0JobModel = await http.CreateJob(aJobModel, userAdmin_Company_1);
            Assert.IsTrue(!r0JobModel.result.IsSuccess);

            aJobModel = new JobCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00JobModel = await http.CreateJob(aJobModel, userAdmin_Company_1);
            Assert.IsTrue(r00JobModel.result.IsSuccess);

            aJobModel = new JobCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01JobModel = await http.CreateJob(aJobModel, userAdmin_Company_2);
            Assert.IsTrue(!r01JobModel.result.IsSuccess);


            aJobModel = new JobCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01JobModel = await http.CreateJob(aJobModel, userAdmin_Company_2);
            Assert.IsTrue(r01JobModel.result.IsSuccess);

            //Get List Test
            var r2JobModel = await http.GetJobs(new JobSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2JobModel.result.IsSuccess);
            Assert.IsTrue(r2JobModel.result.Model.Any(x => x.JobId == rJobModel.result.Model.JobId));

            var r20JobModel = await http.GetJobs(new JobSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20JobModel.result.Model.Any(x => x.JobId == rJobModel.result.Model.JobId));

            //Get Item Test
            var r3JobModel = await http.GetJob(rJobModel.result.Model.JobId, userAdmin_Company_1);
            Assert.IsTrue(r3JobModel.result.IsSuccess);
            Assert.IsTrue(r3JobModel.result.Model.JobId == rJobModel.result.Model.JobId);

            var r30JobModel = await http.GetJob(rJobModel.result.Model.JobId, userAdmin_Company_2);
            Assert.IsTrue(!r30JobModel.result.IsSuccess);

            //Edit Item Test
            var r4JobModel = await http.UpdateJob(rJobModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4JobModel.result.IsSuccess);
            //Assert.IsTrue(r4JobModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40JobModel = await http.UpdateJob(rJobModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40JobModel.result.IsSuccess);
            #endregion

            /*
            #region Job Association with XYZZYX

            var tjobList0 = await http.GetJobsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobList0.result.IsSuccess);

            var tjobAssociationR = await http.AssociateJobWithXYZZYX(new JobXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, JobId = rJobModel.result.Model.JobId }, userAdmin_Company_1);
            Assert.IsTrue(tjobAssociationR.result.IsSuccess);

            tjobList0 = await http.GetJobsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobList0.result.IsSuccess);
            Assert.IsTrue(tjobList0.result.Model.Any(x => x.JobId == rJobModel.result.Model.JobId));

            var tjobUnAssociationR = await http.UnAssociateJobWithXYZZYX(new JobXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, JobId = rJobModel.result.Model.JobId }, userAdmin_Company_1);
            Assert.IsTrue(tjobUnAssociationR.result.IsSuccess);
            tjobList0 = await http.GetJobsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobList0.result.IsSuccess);
            Assert.IsTrue(!tjobList0.result.Model.Any(x => x.JobId == rJobModel.result.Model.JobId));

            #endregion
            */
        }
    }
}