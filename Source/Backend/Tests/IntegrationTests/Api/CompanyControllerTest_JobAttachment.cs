using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task JobAttachmentTest_IntegrationTest()
        {
            #region JobAttachment
            //Tests for JobAttachment
            //Create Item Test

            var aJobAttachmentModel = new JobAttachmentCreateModel();
            var rJobAttachmentModel = await http.CreateJobAttachment(aJobAttachmentModel, userAdmin_Company_1);
            Assert.IsTrue(rJobAttachmentModel.result.IsSuccess);

            aJobAttachmentModel = new JobAttachmentCreateModel();
            var r0JobAttachmentModel = await http.CreateJobAttachment(aJobAttachmentModel, userAdmin_Company_1);
            Assert.IsTrue(!r0JobAttachmentModel.result.IsSuccess);

            aJobAttachmentModel = new JobAttachmentCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00JobAttachmentModel = await http.CreateJobAttachment(aJobAttachmentModel, userAdmin_Company_1);
            Assert.IsTrue(r00JobAttachmentModel.result.IsSuccess);

            aJobAttachmentModel = new JobAttachmentCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01JobAttachmentModel = await http.CreateJobAttachment(aJobAttachmentModel, userAdmin_Company_2);
            Assert.IsTrue(!r01JobAttachmentModel.result.IsSuccess);


            aJobAttachmentModel = new JobAttachmentCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01JobAttachmentModel = await http.CreateJobAttachment(aJobAttachmentModel, userAdmin_Company_2);
            Assert.IsTrue(r01JobAttachmentModel.result.IsSuccess);

            //Get List Test
            var r2JobAttachmentModel = await http.GetJobAttachments(new JobAttachmentSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2JobAttachmentModel.result.IsSuccess);
            Assert.IsTrue(r2JobAttachmentModel.result.Model.Any(x => x.JobAttachmentId == rJobAttachmentModel.result.Model.JobAttachmentId));

            var r20JobAttachmentModel = await http.GetJobAttachments(new JobAttachmentSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20JobAttachmentModel.result.Model.Any(x => x.JobAttachmentId == rJobAttachmentModel.result.Model.JobAttachmentId));

            //Get Item Test
            var r3JobAttachmentModel = await http.GetJobAttachment(rJobAttachmentModel.result.Model.JobAttachmentId, userAdmin_Company_1);
            Assert.IsTrue(r3JobAttachmentModel.result.IsSuccess);
            Assert.IsTrue(r3JobAttachmentModel.result.Model.JobAttachmentId == rJobAttachmentModel.result.Model.JobAttachmentId);

            var r30JobAttachmentModel = await http.GetJobAttachment(rJobAttachmentModel.result.Model.JobAttachmentId, userAdmin_Company_2);
            Assert.IsTrue(!r30JobAttachmentModel.result.IsSuccess);

            //Edit Item Test
            var r4JobAttachmentModel = await http.UpdateJobAttachment(rJobAttachmentModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4JobAttachmentModel.result.IsSuccess);
            //Assert.IsTrue(r4JobAttachmentModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40JobAttachmentModel = await http.UpdateJobAttachment(rJobAttachmentModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40JobAttachmentModel.result.IsSuccess);
            #endregion

            /*
            #region JobAttachment Association with XYZZYX

            var tjobAttachmentList0 = await http.GetJobAttachmentsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobAttachmentList0.result.IsSuccess);

            var tjobAttachmentAssociationR = await http.AssociateJobAttachmentWithXYZZYX(new JobAttachmentXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, JobAttachmentId = rJobAttachmentModel.result.Model.JobAttachmentId }, userAdmin_Company_1);
            Assert.IsTrue(tjobAttachmentAssociationR.result.IsSuccess);

            tjobAttachmentList0 = await http.GetJobAttachmentsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobAttachmentList0.result.IsSuccess);
            Assert.IsTrue(tjobAttachmentList0.result.Model.Any(x => x.JobAttachmentId == rJobAttachmentModel.result.Model.JobAttachmentId));

            var tjobAttachmentUnAssociationR = await http.UnAssociateJobAttachmentWithXYZZYX(new JobAttachmentXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, JobAttachmentId = rJobAttachmentModel.result.Model.JobAttachmentId }, userAdmin_Company_1);
            Assert.IsTrue(tjobAttachmentUnAssociationR.result.IsSuccess);
            tjobAttachmentList0 = await http.GetJobAttachmentsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobAttachmentList0.result.IsSuccess);
            Assert.IsTrue(!tjobAttachmentList0.result.Model.Any(x => x.JobAttachmentId == rJobAttachmentModel.result.Model.JobAttachmentId));

            #endregion
            */
        }
    }
}