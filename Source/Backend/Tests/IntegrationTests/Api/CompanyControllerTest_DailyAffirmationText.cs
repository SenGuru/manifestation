using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task DailyAffirmationTextTest_IntegrationTest()
        {
            #region DailyAffirmationText
            //Tests for DailyAffirmationText
            //Create Item Test

            var aDailyAffirmationTextModel = new DailyAffirmationTextCreateModel();
            var rDailyAffirmationTextModel = await http.CreateDailyAffirmationText(aDailyAffirmationTextModel, userAdmin_Company_1);
            Assert.IsTrue(rDailyAffirmationTextModel.result.IsSuccess);

            aDailyAffirmationTextModel = new DailyAffirmationTextCreateModel();
            var r0DailyAffirmationTextModel = await http.CreateDailyAffirmationText(aDailyAffirmationTextModel, userAdmin_Company_1);
            Assert.IsTrue(!r0DailyAffirmationTextModel.result.IsSuccess);

            aDailyAffirmationTextModel = new DailyAffirmationTextCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00DailyAffirmationTextModel = await http.CreateDailyAffirmationText(aDailyAffirmationTextModel, userAdmin_Company_1);
            Assert.IsTrue(r00DailyAffirmationTextModel.result.IsSuccess);

            aDailyAffirmationTextModel = new DailyAffirmationTextCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01DailyAffirmationTextModel = await http.CreateDailyAffirmationText(aDailyAffirmationTextModel, userAdmin_Company_2);
            Assert.IsTrue(!r01DailyAffirmationTextModel.result.IsSuccess);


            aDailyAffirmationTextModel = new DailyAffirmationTextCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01DailyAffirmationTextModel = await http.CreateDailyAffirmationText(aDailyAffirmationTextModel, userAdmin_Company_2);
            Assert.IsTrue(r01DailyAffirmationTextModel.result.IsSuccess);

            //Get List Test
            var r2DailyAffirmationTextModel = await http.GetDailyAffirmationTexts(new DailyAffirmationTextSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2DailyAffirmationTextModel.result.IsSuccess);
            Assert.IsTrue(r2DailyAffirmationTextModel.result.Model.Any(x => x.DailyAffirmationTextId == rDailyAffirmationTextModel.result.Model.DailyAffirmationTextId));

            var r20DailyAffirmationTextModel = await http.GetDailyAffirmationTexts(new DailyAffirmationTextSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20DailyAffirmationTextModel.result.Model.Any(x => x.DailyAffirmationTextId == rDailyAffirmationTextModel.result.Model.DailyAffirmationTextId));

            //Get Item Test
            var r3DailyAffirmationTextModel = await http.GetDailyAffirmationText(rDailyAffirmationTextModel.result.Model.DailyAffirmationTextId, userAdmin_Company_1);
            Assert.IsTrue(r3DailyAffirmationTextModel.result.IsSuccess);
            Assert.IsTrue(r3DailyAffirmationTextModel.result.Model.DailyAffirmationTextId == rDailyAffirmationTextModel.result.Model.DailyAffirmationTextId);

            var r30DailyAffirmationTextModel = await http.GetDailyAffirmationText(rDailyAffirmationTextModel.result.Model.DailyAffirmationTextId, userAdmin_Company_2);
            Assert.IsTrue(!r30DailyAffirmationTextModel.result.IsSuccess);

            //Edit Item Test
            var r4DailyAffirmationTextModel = await http.UpdateDailyAffirmationText(rDailyAffirmationTextModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4DailyAffirmationTextModel.result.IsSuccess);
            //Assert.IsTrue(r4DailyAffirmationTextModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40DailyAffirmationTextModel = await http.UpdateDailyAffirmationText(rDailyAffirmationTextModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40DailyAffirmationTextModel.result.IsSuccess);
            #endregion

            /*
            #region DailyAffirmationText Association with XYZZYX

            var tdailyAffirmationTextList0 = await http.GetDailyAffirmationTextsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdailyAffirmationTextList0.result.IsSuccess);

            var tdailyAffirmationTextAssociationR = await http.AssociateDailyAffirmationTextWithXYZZYX(new DailyAffirmationTextXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DailyAffirmationTextId = rDailyAffirmationTextModel.result.Model.DailyAffirmationTextId }, userAdmin_Company_1);
            Assert.IsTrue(tdailyAffirmationTextAssociationR.result.IsSuccess);

            tdailyAffirmationTextList0 = await http.GetDailyAffirmationTextsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdailyAffirmationTextList0.result.IsSuccess);
            Assert.IsTrue(tdailyAffirmationTextList0.result.Model.Any(x => x.DailyAffirmationTextId == rDailyAffirmationTextModel.result.Model.DailyAffirmationTextId));

            var tdailyAffirmationTextUnAssociationR = await http.UnAssociateDailyAffirmationTextWithXYZZYX(new DailyAffirmationTextXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DailyAffirmationTextId = rDailyAffirmationTextModel.result.Model.DailyAffirmationTextId }, userAdmin_Company_1);
            Assert.IsTrue(tdailyAffirmationTextUnAssociationR.result.IsSuccess);
            tdailyAffirmationTextList0 = await http.GetDailyAffirmationTextsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdailyAffirmationTextList0.result.IsSuccess);
            Assert.IsTrue(!tdailyAffirmationTextList0.result.Model.Any(x => x.DailyAffirmationTextId == rDailyAffirmationTextModel.result.Model.DailyAffirmationTextId));

            #endregion
            */
        }
    }
}