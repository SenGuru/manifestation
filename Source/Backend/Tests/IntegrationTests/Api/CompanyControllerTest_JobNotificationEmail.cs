using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task JobNotificationEmailTest_IntegrationTest()
        {
            #region JobNotificationEmail
            //Tests for JobNotificationEmail
            //Create Item Test

            var aJobNotificationEmailModel = new JobNotificationEmailCreateModel();
            var rJobNotificationEmailModel = await http.CreateJobNotificationEmail(aJobNotificationEmailModel, userAdmin_Company_1);
            Assert.IsTrue(rJobNotificationEmailModel.result.IsSuccess);

            aJobNotificationEmailModel = new JobNotificationEmailCreateModel();
            var r0JobNotificationEmailModel = await http.CreateJobNotificationEmail(aJobNotificationEmailModel, userAdmin_Company_1);
            Assert.IsTrue(!r0JobNotificationEmailModel.result.IsSuccess);

            aJobNotificationEmailModel = new JobNotificationEmailCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00JobNotificationEmailModel = await http.CreateJobNotificationEmail(aJobNotificationEmailModel, userAdmin_Company_1);
            Assert.IsTrue(r00JobNotificationEmailModel.result.IsSuccess);

            aJobNotificationEmailModel = new JobNotificationEmailCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01JobNotificationEmailModel = await http.CreateJobNotificationEmail(aJobNotificationEmailModel, userAdmin_Company_2);
            Assert.IsTrue(!r01JobNotificationEmailModel.result.IsSuccess);


            aJobNotificationEmailModel = new JobNotificationEmailCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01JobNotificationEmailModel = await http.CreateJobNotificationEmail(aJobNotificationEmailModel, userAdmin_Company_2);
            Assert.IsTrue(r01JobNotificationEmailModel.result.IsSuccess);

            //Get List Test
            var r2JobNotificationEmailModel = await http.GetJobNotificationEmails(new JobNotificationEmailSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2JobNotificationEmailModel.result.IsSuccess);
            Assert.IsTrue(r2JobNotificationEmailModel.result.Model.Any(x => x.JobNotificationEmailId == rJobNotificationEmailModel.result.Model.JobNotificationEmailId));

            var r20JobNotificationEmailModel = await http.GetJobNotificationEmails(new JobNotificationEmailSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20JobNotificationEmailModel.result.Model.Any(x => x.JobNotificationEmailId == rJobNotificationEmailModel.result.Model.JobNotificationEmailId));

            //Get Item Test
            var r3JobNotificationEmailModel = await http.GetJobNotificationEmail(rJobNotificationEmailModel.result.Model.JobNotificationEmailId, userAdmin_Company_1);
            Assert.IsTrue(r3JobNotificationEmailModel.result.IsSuccess);
            Assert.IsTrue(r3JobNotificationEmailModel.result.Model.JobNotificationEmailId == rJobNotificationEmailModel.result.Model.JobNotificationEmailId);

            var r30JobNotificationEmailModel = await http.GetJobNotificationEmail(rJobNotificationEmailModel.result.Model.JobNotificationEmailId, userAdmin_Company_2);
            Assert.IsTrue(!r30JobNotificationEmailModel.result.IsSuccess);

            //Edit Item Test
            var r4JobNotificationEmailModel = await http.UpdateJobNotificationEmail(rJobNotificationEmailModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4JobNotificationEmailModel.result.IsSuccess);
            //Assert.IsTrue(r4JobNotificationEmailModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40JobNotificationEmailModel = await http.UpdateJobNotificationEmail(rJobNotificationEmailModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40JobNotificationEmailModel.result.IsSuccess);
            #endregion

            /*
            #region JobNotificationEmail Association with XYZZYX

            var tjobNotificationEmailList0 = await http.GetJobNotificationEmailsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobNotificationEmailList0.result.IsSuccess);

            var tjobNotificationEmailAssociationR = await http.AssociateJobNotificationEmailWithXYZZYX(new JobNotificationEmailXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, JobNotificationEmailId = rJobNotificationEmailModel.result.Model.JobNotificationEmailId }, userAdmin_Company_1);
            Assert.IsTrue(tjobNotificationEmailAssociationR.result.IsSuccess);

            tjobNotificationEmailList0 = await http.GetJobNotificationEmailsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobNotificationEmailList0.result.IsSuccess);
            Assert.IsTrue(tjobNotificationEmailList0.result.Model.Any(x => x.JobNotificationEmailId == rJobNotificationEmailModel.result.Model.JobNotificationEmailId));

            var tjobNotificationEmailUnAssociationR = await http.UnAssociateJobNotificationEmailWithXYZZYX(new JobNotificationEmailXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, JobNotificationEmailId = rJobNotificationEmailModel.result.Model.JobNotificationEmailId }, userAdmin_Company_1);
            Assert.IsTrue(tjobNotificationEmailUnAssociationR.result.IsSuccess);
            tjobNotificationEmailList0 = await http.GetJobNotificationEmailsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobNotificationEmailList0.result.IsSuccess);
            Assert.IsTrue(!tjobNotificationEmailList0.result.Model.Any(x => x.JobNotificationEmailId == rJobNotificationEmailModel.result.Model.JobNotificationEmailId));

            #endregion
            */
        }
    }
}