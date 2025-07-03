using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task JobTemplateParameterTest_IntegrationTest()
        {
            #region JobTemplateParameter
            //Tests for JobTemplateParameter
            //Create Item Test

            var aJobTemplateParameterModel = new JobTemplateParameterCreateModel();
            var rJobTemplateParameterModel = await http.CreateJobTemplateParameter(aJobTemplateParameterModel, userAdmin_Company_1);
            Assert.IsTrue(rJobTemplateParameterModel.result.IsSuccess);

            aJobTemplateParameterModel = new JobTemplateParameterCreateModel();
            var r0JobTemplateParameterModel = await http.CreateJobTemplateParameter(aJobTemplateParameterModel, userAdmin_Company_1);
            Assert.IsTrue(!r0JobTemplateParameterModel.result.IsSuccess);

            aJobTemplateParameterModel = new JobTemplateParameterCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00JobTemplateParameterModel = await http.CreateJobTemplateParameter(aJobTemplateParameterModel, userAdmin_Company_1);
            Assert.IsTrue(r00JobTemplateParameterModel.result.IsSuccess);

            aJobTemplateParameterModel = new JobTemplateParameterCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01JobTemplateParameterModel = await http.CreateJobTemplateParameter(aJobTemplateParameterModel, userAdmin_Company_2);
            Assert.IsTrue(!r01JobTemplateParameterModel.result.IsSuccess);


            aJobTemplateParameterModel = new JobTemplateParameterCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01JobTemplateParameterModel = await http.CreateJobTemplateParameter(aJobTemplateParameterModel, userAdmin_Company_2);
            Assert.IsTrue(r01JobTemplateParameterModel.result.IsSuccess);

            //Get List Test
            var r2JobTemplateParameterModel = await http.GetJobTemplateParameters(new JobTemplateParameterSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2JobTemplateParameterModel.result.IsSuccess);
            Assert.IsTrue(r2JobTemplateParameterModel.result.Model.Any(x => x.JobTemplateParameterId == rJobTemplateParameterModel.result.Model.JobTemplateParameterId));

            var r20JobTemplateParameterModel = await http.GetJobTemplateParameters(new JobTemplateParameterSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20JobTemplateParameterModel.result.Model.Any(x => x.JobTemplateParameterId == rJobTemplateParameterModel.result.Model.JobTemplateParameterId));

            //Get Item Test
            var r3JobTemplateParameterModel = await http.GetJobTemplateParameter(rJobTemplateParameterModel.result.Model.JobTemplateParameterId, userAdmin_Company_1);
            Assert.IsTrue(r3JobTemplateParameterModel.result.IsSuccess);
            Assert.IsTrue(r3JobTemplateParameterModel.result.Model.JobTemplateParameterId == rJobTemplateParameterModel.result.Model.JobTemplateParameterId);

            var r30JobTemplateParameterModel = await http.GetJobTemplateParameter(rJobTemplateParameterModel.result.Model.JobTemplateParameterId, userAdmin_Company_2);
            Assert.IsTrue(!r30JobTemplateParameterModel.result.IsSuccess);

            //Edit Item Test
            var r4JobTemplateParameterModel = await http.UpdateJobTemplateParameter(rJobTemplateParameterModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4JobTemplateParameterModel.result.IsSuccess);
            //Assert.IsTrue(r4JobTemplateParameterModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40JobTemplateParameterModel = await http.UpdateJobTemplateParameter(rJobTemplateParameterModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40JobTemplateParameterModel.result.IsSuccess);
            #endregion

            /*
            #region JobTemplateParameter Association with XYZZYX

            var tjobTemplateParameterList0 = await http.GetJobTemplateParametersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobTemplateParameterList0.result.IsSuccess);

            var tjobTemplateParameterAssociationR = await http.AssociateJobTemplateParameterWithXYZZYX(new JobTemplateParameterXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, JobTemplateParameterId = rJobTemplateParameterModel.result.Model.JobTemplateParameterId }, userAdmin_Company_1);
            Assert.IsTrue(tjobTemplateParameterAssociationR.result.IsSuccess);

            tjobTemplateParameterList0 = await http.GetJobTemplateParametersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobTemplateParameterList0.result.IsSuccess);
            Assert.IsTrue(tjobTemplateParameterList0.result.Model.Any(x => x.JobTemplateParameterId == rJobTemplateParameterModel.result.Model.JobTemplateParameterId));

            var tjobTemplateParameterUnAssociationR = await http.UnAssociateJobTemplateParameterWithXYZZYX(new JobTemplateParameterXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, JobTemplateParameterId = rJobTemplateParameterModel.result.Model.JobTemplateParameterId }, userAdmin_Company_1);
            Assert.IsTrue(tjobTemplateParameterUnAssociationR.result.IsSuccess);
            tjobTemplateParameterList0 = await http.GetJobTemplateParametersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobTemplateParameterList0.result.IsSuccess);
            Assert.IsTrue(!tjobTemplateParameterList0.result.Model.Any(x => x.JobTemplateParameterId == rJobTemplateParameterModel.result.Model.JobTemplateParameterId));

            #endregion
            */
        }
    }
}