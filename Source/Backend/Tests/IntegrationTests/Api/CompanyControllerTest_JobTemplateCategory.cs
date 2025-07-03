using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task JobTemplateCategoryTest_IntegrationTest()
        {
            #region JobTemplateCategory
            //Tests for JobTemplateCategory
            //Create Item Test

            var aJobTemplateCategoryModel = new JobTemplateCategoryCreateModel();
            var rJobTemplateCategoryModel = await http.CreateJobTemplateCategory(aJobTemplateCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(rJobTemplateCategoryModel.result.IsSuccess);

            aJobTemplateCategoryModel = new JobTemplateCategoryCreateModel();
            var r0JobTemplateCategoryModel = await http.CreateJobTemplateCategory(aJobTemplateCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(!r0JobTemplateCategoryModel.result.IsSuccess);

            aJobTemplateCategoryModel = new JobTemplateCategoryCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00JobTemplateCategoryModel = await http.CreateJobTemplateCategory(aJobTemplateCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(r00JobTemplateCategoryModel.result.IsSuccess);

            aJobTemplateCategoryModel = new JobTemplateCategoryCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01JobTemplateCategoryModel = await http.CreateJobTemplateCategory(aJobTemplateCategoryModel, userAdmin_Company_2);
            Assert.IsTrue(!r01JobTemplateCategoryModel.result.IsSuccess);


            aJobTemplateCategoryModel = new JobTemplateCategoryCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01JobTemplateCategoryModel = await http.CreateJobTemplateCategory(aJobTemplateCategoryModel, userAdmin_Company_2);
            Assert.IsTrue(r01JobTemplateCategoryModel.result.IsSuccess);

            //Get List Test
            var r2JobTemplateCategoryModel = await http.GetJobTemplateCategorys(new JobTemplateCategorySearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2JobTemplateCategoryModel.result.IsSuccess);
            Assert.IsTrue(r2JobTemplateCategoryModel.result.Model.Any(x => x.JobTemplateCategoryId == rJobTemplateCategoryModel.result.Model.JobTemplateCategoryId));

            var r20JobTemplateCategoryModel = await http.GetJobTemplateCategorys(new JobTemplateCategorySearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20JobTemplateCategoryModel.result.Model.Any(x => x.JobTemplateCategoryId == rJobTemplateCategoryModel.result.Model.JobTemplateCategoryId));

            //Get Item Test
            var r3JobTemplateCategoryModel = await http.GetJobTemplateCategory(rJobTemplateCategoryModel.result.Model.JobTemplateCategoryId, userAdmin_Company_1);
            Assert.IsTrue(r3JobTemplateCategoryModel.result.IsSuccess);
            Assert.IsTrue(r3JobTemplateCategoryModel.result.Model.JobTemplateCategoryId == rJobTemplateCategoryModel.result.Model.JobTemplateCategoryId);

            var r30JobTemplateCategoryModel = await http.GetJobTemplateCategory(rJobTemplateCategoryModel.result.Model.JobTemplateCategoryId, userAdmin_Company_2);
            Assert.IsTrue(!r30JobTemplateCategoryModel.result.IsSuccess);

            //Edit Item Test
            var r4JobTemplateCategoryModel = await http.UpdateJobTemplateCategory(rJobTemplateCategoryModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4JobTemplateCategoryModel.result.IsSuccess);
            //Assert.IsTrue(r4JobTemplateCategoryModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40JobTemplateCategoryModel = await http.UpdateJobTemplateCategory(rJobTemplateCategoryModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40JobTemplateCategoryModel.result.IsSuccess);
            #endregion

            /*
            #region JobTemplateCategory Association with XYZZYX

            var tjobTemplateCategoryList0 = await http.GetJobTemplateCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobTemplateCategoryList0.result.IsSuccess);

            var tjobTemplateCategoryAssociationR = await http.AssociateJobTemplateCategoryWithXYZZYX(new JobTemplateCategoryXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, JobTemplateCategoryId = rJobTemplateCategoryModel.result.Model.JobTemplateCategoryId }, userAdmin_Company_1);
            Assert.IsTrue(tjobTemplateCategoryAssociationR.result.IsSuccess);

            tjobTemplateCategoryList0 = await http.GetJobTemplateCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobTemplateCategoryList0.result.IsSuccess);
            Assert.IsTrue(tjobTemplateCategoryList0.result.Model.Any(x => x.JobTemplateCategoryId == rJobTemplateCategoryModel.result.Model.JobTemplateCategoryId));

            var tjobTemplateCategoryUnAssociationR = await http.UnAssociateJobTemplateCategoryWithXYZZYX(new JobTemplateCategoryXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, JobTemplateCategoryId = rJobTemplateCategoryModel.result.Model.JobTemplateCategoryId }, userAdmin_Company_1);
            Assert.IsTrue(tjobTemplateCategoryUnAssociationR.result.IsSuccess);
            tjobTemplateCategoryList0 = await http.GetJobTemplateCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tjobTemplateCategoryList0.result.IsSuccess);
            Assert.IsTrue(!tjobTemplateCategoryList0.result.Model.Any(x => x.JobTemplateCategoryId == rJobTemplateCategoryModel.result.Model.JobTemplateCategoryId));

            #endregion
            */
        }
    }
}