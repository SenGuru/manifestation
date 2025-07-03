using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task JobTemplateTest_IntegrationTest()
        {
            #region JobTemplate
            //Tests for JobTemplate
            //Create Item Test

            var aJobTemplateModel = new JobTemplateCreateModel();
            var rJobTemplateModel = await http.CreateJobTemplate(aJobTemplateModel, userAdmin_Company_1);
            Assert.IsTrue(rJobTemplateModel.result.IsSuccess);

            aJobTemplateModel = new JobTemplateCreateModel();
            var r0JobTemplateModel = await http.CreateJobTemplate(aJobTemplateModel, userAdmin_Company_1);
            Assert.IsTrue(!r0JobTemplateModel.result.IsSuccess);

            aJobTemplateModel = new JobTemplateCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00JobTemplateModel = await http.CreateJobTemplate(aJobTemplateModel, userAdmin_Company_1);
            Assert.IsTrue(r00JobTemplateModel.result.IsSuccess);

            aJobTemplateModel = new JobTemplateCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01JobTemplateModel = await http.CreateJobTemplate(aJobTemplateModel, userAdmin_Company_2);
            Assert.IsTrue(!r01JobTemplateModel.result.IsSuccess);


            aJobTemplateModel = new JobTemplateCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01JobTemplateModel = await http.CreateJobTemplate(aJobTemplateModel, userAdmin_Company_2);
            Assert.IsTrue(r01JobTemplateModel.result.IsSuccess);

            //Get List Test
            var r2JobTemplateModel = await http.GetJobTemplates(new JobTemplateSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2JobTemplateModel.result.IsSuccess);
            Assert.IsTrue(r2JobTemplateModel.result.Model.Any(x => x.JobTemplateId == rJobTemplateModel.result.Model.JobTemplateId));

            var r20JobTemplateModel = await http.GetJobTemplates(new JobTemplateSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20JobTemplateModel.result.Model.Any(x => x.JobTemplateId == rJobTemplateModel.result.Model.JobTemplateId));

            //Get Item Test
            var r3JobTemplateModel = await http.GetJobTemplate(rJobTemplateModel.result.Model.JobTemplateId, userAdmin_Company_1);
            Assert.IsTrue(r3JobTemplateModel.result.IsSuccess);
            Assert.IsTrue(r3JobTemplateModel.result.Model.JobTemplateId == rJobTemplateModel.result.Model.JobTemplateId);

            var r30JobTemplateModel = await http.GetJobTemplate(rJobTemplateModel.result.Model.JobTemplateId, userAdmin_Company_2);
            Assert.IsTrue(!r30JobTemplateModel.result.IsSuccess);

            //Edit Item Test
            var r4JobTemplateModel = await http.UpdateJobTemplate(rJobTemplateModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4JobTemplateModel.result.IsSuccess);
            //Assert.IsTrue(r4JobTemplateModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40JobTemplateModel = await http.UpdateJobTemplate(rJobTemplateModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40JobTemplateModel.result.IsSuccess);
            #endregion

            /*
            #region JobTemplate Association with XYZZYX

            var tjobTemplateList0 = await http.GetJobTemplatesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobTemplateList0.result.IsSuccess);

            var tjobTemplateAssociationR = await http.AssociateJobTemplateWithXYZZYX(new JobTemplateXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, JobTemplateId = rJobTemplateModel.result.Model.JobTemplateId }, userAdmin_Company_1);
            Assert.IsTrue(tjobTemplateAssociationR.result.IsSuccess);

            tjobTemplateList0 = await http.GetJobTemplatesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobTemplateList0.result.IsSuccess);
            Assert.IsTrue(tjobTemplateList0.result.Model.Any(x => x.JobTemplateId == rJobTemplateModel.result.Model.JobTemplateId));

            var tjobTemplateUnAssociationR = await http.UnAssociateJobTemplateWithXYZZYX(new JobTemplateXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, JobTemplateId = rJobTemplateModel.result.Model.JobTemplateId }, userAdmin_Company_1);
            Assert.IsTrue(tjobTemplateUnAssociationR.result.IsSuccess);
            tjobTemplateList0 = await http.GetJobTemplatesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobTemplateList0.result.IsSuccess);
            Assert.IsTrue(!tjobTemplateList0.result.Model.Any(x => x.JobTemplateId == rJobTemplateModel.result.Model.JobTemplateId));

            #endregion
            */
        }
    }
}