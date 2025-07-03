using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task ScheduledJobNotificationEmailTest_IntegrationTest()
        {
            #region ScheduledJobNotificationEmail
            //Tests for ScheduledJobNotificationEmail
            //Create Item Test

            var aScheduledJobNotificationEmailModel = new ScheduledJobNotificationEmailCreateModel();
            var rScheduledJobNotificationEmailModel = await http.CreateScheduledJobNotificationEmail(aScheduledJobNotificationEmailModel, userAdmin_Company_1);
            Assert.IsTrue(rScheduledJobNotificationEmailModel.result.IsSuccess);

            aScheduledJobNotificationEmailModel = new ScheduledJobNotificationEmailCreateModel();
            var r0ScheduledJobNotificationEmailModel = await http.CreateScheduledJobNotificationEmail(aScheduledJobNotificationEmailModel, userAdmin_Company_1);
            Assert.IsTrue(!r0ScheduledJobNotificationEmailModel.result.IsSuccess);

            aScheduledJobNotificationEmailModel = new ScheduledJobNotificationEmailCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00ScheduledJobNotificationEmailModel = await http.CreateScheduledJobNotificationEmail(aScheduledJobNotificationEmailModel, userAdmin_Company_1);
            Assert.IsTrue(r00ScheduledJobNotificationEmailModel.result.IsSuccess);

            aScheduledJobNotificationEmailModel = new ScheduledJobNotificationEmailCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01ScheduledJobNotificationEmailModel = await http.CreateScheduledJobNotificationEmail(aScheduledJobNotificationEmailModel, userAdmin_Company_2);
            Assert.IsTrue(!r01ScheduledJobNotificationEmailModel.result.IsSuccess);


            aScheduledJobNotificationEmailModel = new ScheduledJobNotificationEmailCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01ScheduledJobNotificationEmailModel = await http.CreateScheduledJobNotificationEmail(aScheduledJobNotificationEmailModel, userAdmin_Company_2);
            Assert.IsTrue(r01ScheduledJobNotificationEmailModel.result.IsSuccess);

            //Get List Test
            var r2ScheduledJobNotificationEmailModel = await http.GetScheduledJobNotificationEmails(new ScheduledJobNotificationEmailSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2ScheduledJobNotificationEmailModel.result.IsSuccess);
            Assert.IsTrue(r2ScheduledJobNotificationEmailModel.result.Model.Any(x => x.ScheduledJobNotificationEmailId == rScheduledJobNotificationEmailModel.result.Model.ScheduledJobNotificationEmailId));

            var r20ScheduledJobNotificationEmailModel = await http.GetScheduledJobNotificationEmails(new ScheduledJobNotificationEmailSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20ScheduledJobNotificationEmailModel.result.Model.Any(x => x.ScheduledJobNotificationEmailId == rScheduledJobNotificationEmailModel.result.Model.ScheduledJobNotificationEmailId));

            //Get Item Test
            var r3ScheduledJobNotificationEmailModel = await http.GetScheduledJobNotificationEmail(rScheduledJobNotificationEmailModel.result.Model.ScheduledJobNotificationEmailId, userAdmin_Company_1);
            Assert.IsTrue(r3ScheduledJobNotificationEmailModel.result.IsSuccess);
            Assert.IsTrue(r3ScheduledJobNotificationEmailModel.result.Model.ScheduledJobNotificationEmailId == rScheduledJobNotificationEmailModel.result.Model.ScheduledJobNotificationEmailId);

            var r30ScheduledJobNotificationEmailModel = await http.GetScheduledJobNotificationEmail(rScheduledJobNotificationEmailModel.result.Model.ScheduledJobNotificationEmailId, userAdmin_Company_2);
            Assert.IsTrue(!r30ScheduledJobNotificationEmailModel.result.IsSuccess);

            //Edit Item Test
            var r4ScheduledJobNotificationEmailModel = await http.UpdateScheduledJobNotificationEmail(rScheduledJobNotificationEmailModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4ScheduledJobNotificationEmailModel.result.IsSuccess);
            //Assert.IsTrue(r4ScheduledJobNotificationEmailModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40ScheduledJobNotificationEmailModel = await http.UpdateScheduledJobNotificationEmail(rScheduledJobNotificationEmailModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40ScheduledJobNotificationEmailModel.result.IsSuccess);
            #endregion

            /*
            #region ScheduledJobNotificationEmail Association with XYZZYX

            var tscheduledJobNotificationEmailList0 = await http.GetScheduledJobNotificationEmailsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tscheduledJobNotificationEmailList0.result.IsSuccess);

            var tscheduledJobNotificationEmailAssociationR = await http.AssociateScheduledJobNotificationEmailWithXYZZYX(new ScheduledJobNotificationEmailXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ScheduledJobNotificationEmailId = rScheduledJobNotificationEmailModel.result.Model.ScheduledJobNotificationEmailId }, userAdmin_Company_1);
            Assert.IsTrue(tscheduledJobNotificationEmailAssociationR.result.IsSuccess);

            tscheduledJobNotificationEmailList0 = await http.GetScheduledJobNotificationEmailsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tscheduledJobNotificationEmailList0.result.IsSuccess);
            Assert.IsTrue(tscheduledJobNotificationEmailList0.result.Model.Any(x => x.ScheduledJobNotificationEmailId == rScheduledJobNotificationEmailModel.result.Model.ScheduledJobNotificationEmailId));

            var tscheduledJobNotificationEmailUnAssociationR = await http.UnAssociateScheduledJobNotificationEmailWithXYZZYX(new ScheduledJobNotificationEmailXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ScheduledJobNotificationEmailId = rScheduledJobNotificationEmailModel.result.Model.ScheduledJobNotificationEmailId }, userAdmin_Company_1);
            Assert.IsTrue(tscheduledJobNotificationEmailUnAssociationR.result.IsSuccess);
            tscheduledJobNotificationEmailList0 = await http.GetScheduledJobNotificationEmailsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tscheduledJobNotificationEmailList0.result.IsSuccess);
            Assert.IsTrue(!tscheduledJobNotificationEmailList0.result.Model.Any(x => x.ScheduledJobNotificationEmailId == rScheduledJobNotificationEmailModel.result.Model.ScheduledJobNotificationEmailId));

            #endregion
            */
        }
    }
}