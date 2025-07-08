using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task DailyAffirmationSessionCategoryTest_IntegrationTest()
        {
            #region DailyAffirmationSessionCategory
            //Tests for DailyAffirmationSessionCategory
            //Create Item Test

            var aDailyAffirmationSessionCategoryModel = new DailyAffirmationSessionCategoryCreateModel();
            var rDailyAffirmationSessionCategoryModel = await http.CreateDailyAffirmationSessionCategory(aDailyAffirmationSessionCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(rDailyAffirmationSessionCategoryModel.result.IsSuccess);

            aDailyAffirmationSessionCategoryModel = new DailyAffirmationSessionCategoryCreateModel();
            var r0DailyAffirmationSessionCategoryModel = await http.CreateDailyAffirmationSessionCategory(aDailyAffirmationSessionCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(!r0DailyAffirmationSessionCategoryModel.result.IsSuccess);

            aDailyAffirmationSessionCategoryModel = new DailyAffirmationSessionCategoryCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00DailyAffirmationSessionCategoryModel = await http.CreateDailyAffirmationSessionCategory(aDailyAffirmationSessionCategoryModel, userAdmin_Company_1);
            Assert.IsTrue(r00DailyAffirmationSessionCategoryModel.result.IsSuccess);

            aDailyAffirmationSessionCategoryModel = new DailyAffirmationSessionCategoryCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01DailyAffirmationSessionCategoryModel = await http.CreateDailyAffirmationSessionCategory(aDailyAffirmationSessionCategoryModel, userAdmin_Company_2);
            Assert.IsTrue(!r01DailyAffirmationSessionCategoryModel.result.IsSuccess);


            aDailyAffirmationSessionCategoryModel = new DailyAffirmationSessionCategoryCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01DailyAffirmationSessionCategoryModel = await http.CreateDailyAffirmationSessionCategory(aDailyAffirmationSessionCategoryModel, userAdmin_Company_2);
            Assert.IsTrue(r01DailyAffirmationSessionCategoryModel.result.IsSuccess);

            //Get List Test
            var r2DailyAffirmationSessionCategoryModel = await http.GetDailyAffirmationSessionCategorys(new DailyAffirmationSessionCategorySearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2DailyAffirmationSessionCategoryModel.result.IsSuccess);
            Assert.IsTrue(r2DailyAffirmationSessionCategoryModel.result.Model.Any(x => x.DailyAffirmationSessionCategoryId == rDailyAffirmationSessionCategoryModel.result.Model.DailyAffirmationSessionCategoryId));

            var r20DailyAffirmationSessionCategoryModel = await http.GetDailyAffirmationSessionCategorys(new DailyAffirmationSessionCategorySearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20DailyAffirmationSessionCategoryModel.result.Model.Any(x => x.DailyAffirmationSessionCategoryId == rDailyAffirmationSessionCategoryModel.result.Model.DailyAffirmationSessionCategoryId));

            //Get Item Test
            var r3DailyAffirmationSessionCategoryModel = await http.GetDailyAffirmationSessionCategory(rDailyAffirmationSessionCategoryModel.result.Model.DailyAffirmationSessionCategoryId, userAdmin_Company_1);
            Assert.IsTrue(r3DailyAffirmationSessionCategoryModel.result.IsSuccess);
            Assert.IsTrue(r3DailyAffirmationSessionCategoryModel.result.Model.DailyAffirmationSessionCategoryId == rDailyAffirmationSessionCategoryModel.result.Model.DailyAffirmationSessionCategoryId);

            var r30DailyAffirmationSessionCategoryModel = await http.GetDailyAffirmationSessionCategory(rDailyAffirmationSessionCategoryModel.result.Model.DailyAffirmationSessionCategoryId, userAdmin_Company_2);
            Assert.IsTrue(!r30DailyAffirmationSessionCategoryModel.result.IsSuccess);

            //Edit Item Test
            var r4DailyAffirmationSessionCategoryModel = await http.UpdateDailyAffirmationSessionCategory(rDailyAffirmationSessionCategoryModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4DailyAffirmationSessionCategoryModel.result.IsSuccess);
            //Assert.IsTrue(r4DailyAffirmationSessionCategoryModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40DailyAffirmationSessionCategoryModel = await http.UpdateDailyAffirmationSessionCategory(rDailyAffirmationSessionCategoryModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40DailyAffirmationSessionCategoryModel.result.IsSuccess);
            #endregion

            /*
            #region DailyAffirmationSessionCategory Association with XYZZYX

            var tdailyAffirmationSessionCategoryList0 = await http.GetDailyAffirmationSessionCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdailyAffirmationSessionCategoryList0.result.IsSuccess);

            var tdailyAffirmationSessionCategoryAssociationR = await http.AssociateDailyAffirmationSessionCategoryWithXYZZYX(new DailyAffirmationSessionCategoryXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DailyAffirmationSessionCategoryId = rDailyAffirmationSessionCategoryModel.result.Model.DailyAffirmationSessionCategoryId }, userAdmin_Company_1);
            Assert.IsTrue(tdailyAffirmationSessionCategoryAssociationR.result.IsSuccess);

            tdailyAffirmationSessionCategoryList0 = await http.GetDailyAffirmationSessionCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdailyAffirmationSessionCategoryList0.result.IsSuccess);
            Assert.IsTrue(tdailyAffirmationSessionCategoryList0.result.Model.Any(x => x.DailyAffirmationSessionCategoryId == rDailyAffirmationSessionCategoryModel.result.Model.DailyAffirmationSessionCategoryId));

            var tdailyAffirmationSessionCategoryUnAssociationR = await http.UnAssociateDailyAffirmationSessionCategoryWithXYZZYX(new DailyAffirmationSessionCategoryXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DailyAffirmationSessionCategoryId = rDailyAffirmationSessionCategoryModel.result.Model.DailyAffirmationSessionCategoryId }, userAdmin_Company_1);
            Assert.IsTrue(tdailyAffirmationSessionCategoryUnAssociationR.result.IsSuccess);
            tdailyAffirmationSessionCategoryList0 = await http.GetDailyAffirmationSessionCategorysByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdailyAffirmationSessionCategoryList0.result.IsSuccess);
            Assert.IsTrue(!tdailyAffirmationSessionCategoryList0.result.Model.Any(x => x.DailyAffirmationSessionCategoryId == rDailyAffirmationSessionCategoryModel.result.Model.DailyAffirmationSessionCategoryId));

            #endregion
            */
        }
    }
}