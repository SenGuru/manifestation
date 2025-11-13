using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task DefaultDailyAffirmationTextTest_IntegrationTest()
        {
            #region DefaultDailyAffirmationText
            //Tests for DefaultDailyAffirmationText
            //Create Item Test

            var aDefaultDailyAffirmationTextModel = new DefaultDailyAffirmationTextCreateModel();
            var rDefaultDailyAffirmationTextModel = await http.CreateDefaultDailyAffirmationText(aDefaultDailyAffirmationTextModel, userAdmin_Company_1);
            Assert.IsTrue(rDefaultDailyAffirmationTextModel.result.IsSuccess);

            aDefaultDailyAffirmationTextModel = new DefaultDailyAffirmationTextCreateModel();
            var r0DefaultDailyAffirmationTextModel = await http.CreateDefaultDailyAffirmationText(aDefaultDailyAffirmationTextModel, userAdmin_Company_1);
            Assert.IsTrue(!r0DefaultDailyAffirmationTextModel.result.IsSuccess);

            aDefaultDailyAffirmationTextModel = new DefaultDailyAffirmationTextCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00DefaultDailyAffirmationTextModel = await http.CreateDefaultDailyAffirmationText(aDefaultDailyAffirmationTextModel, userAdmin_Company_1);
            Assert.IsTrue(r00DefaultDailyAffirmationTextModel.result.IsSuccess);

            aDefaultDailyAffirmationTextModel = new DefaultDailyAffirmationTextCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01DefaultDailyAffirmationTextModel = await http.CreateDefaultDailyAffirmationText(aDefaultDailyAffirmationTextModel, userAdmin_Company_2);
            Assert.IsTrue(!r01DefaultDailyAffirmationTextModel.result.IsSuccess);


            aDefaultDailyAffirmationTextModel = new DefaultDailyAffirmationTextCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01DefaultDailyAffirmationTextModel = await http.CreateDefaultDailyAffirmationText(aDefaultDailyAffirmationTextModel, userAdmin_Company_2);
            Assert.IsTrue(r01DefaultDailyAffirmationTextModel.result.IsSuccess);

            //Get List Test
            var r2DefaultDailyAffirmationTextModel = await http.GetDefaultDailyAffirmationTexts(new DefaultDailyAffirmationTextSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2DefaultDailyAffirmationTextModel.result.IsSuccess);
            Assert.IsTrue(r2DefaultDailyAffirmationTextModel.result.Model.Any(x => x.DefaultDailyAffirmationTextId == rDefaultDailyAffirmationTextModel.result.Model.DefaultDailyAffirmationTextId));

            var r20DefaultDailyAffirmationTextModel = await http.GetDefaultDailyAffirmationTexts(new DefaultDailyAffirmationTextSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20DefaultDailyAffirmationTextModel.result.Model.Any(x => x.DefaultDailyAffirmationTextId == rDefaultDailyAffirmationTextModel.result.Model.DefaultDailyAffirmationTextId));

            //Get Item Test
            var r3DefaultDailyAffirmationTextModel = await http.GetDefaultDailyAffirmationText(rDefaultDailyAffirmationTextModel.result.Model.DefaultDailyAffirmationTextId, userAdmin_Company_1);
            Assert.IsTrue(r3DefaultDailyAffirmationTextModel.result.IsSuccess);
            Assert.IsTrue(r3DefaultDailyAffirmationTextModel.result.Model.DefaultDailyAffirmationTextId == rDefaultDailyAffirmationTextModel.result.Model.DefaultDailyAffirmationTextId);

            var r30DefaultDailyAffirmationTextModel = await http.GetDefaultDailyAffirmationText(rDefaultDailyAffirmationTextModel.result.Model.DefaultDailyAffirmationTextId, userAdmin_Company_2);
            Assert.IsTrue(!r30DefaultDailyAffirmationTextModel.result.IsSuccess);

            //Edit Item Test
            var r4DefaultDailyAffirmationTextModel = await http.UpdateDefaultDailyAffirmationText(rDefaultDailyAffirmationTextModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4DefaultDailyAffirmationTextModel.result.IsSuccess);
            //Assert.IsTrue(r4DefaultDailyAffirmationTextModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40DefaultDailyAffirmationTextModel = await http.UpdateDefaultDailyAffirmationText(rDefaultDailyAffirmationTextModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40DefaultDailyAffirmationTextModel.result.IsSuccess);
            #endregion

            /*
            #region DefaultDailyAffirmationText Association with XYZZYX

            var tdefaultDailyAffirmationTextList0 = await http.GetDefaultDailyAffirmationTextsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultDailyAffirmationTextList0.result.IsSuccess);

            var tdefaultDailyAffirmationTextAssociationR = await http.AssociateDefaultDailyAffirmationTextWithXYZZYX(new DefaultDailyAffirmationTextXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DefaultDailyAffirmationTextId = rDefaultDailyAffirmationTextModel.result.Model.DefaultDailyAffirmationTextId }, userAdmin_Company_1);
            Assert.IsTrue(tdefaultDailyAffirmationTextAssociationR.result.IsSuccess);

            tdefaultDailyAffirmationTextList0 = await http.GetDefaultDailyAffirmationTextsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultDailyAffirmationTextList0.result.IsSuccess);
            Assert.IsTrue(tdefaultDailyAffirmationTextList0.result.Model.Any(x => x.DefaultDailyAffirmationTextId == rDefaultDailyAffirmationTextModel.result.Model.DefaultDailyAffirmationTextId));

            var tdefaultDailyAffirmationTextUnAssociationR = await http.UnAssociateDefaultDailyAffirmationTextWithXYZZYX(new DefaultDailyAffirmationTextXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DefaultDailyAffirmationTextId = rDefaultDailyAffirmationTextModel.result.Model.DefaultDailyAffirmationTextId }, userAdmin_Company_1);
            Assert.IsTrue(tdefaultDailyAffirmationTextUnAssociationR.result.IsSuccess);
            tdefaultDailyAffirmationTextList0 = await http.GetDefaultDailyAffirmationTextsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultDailyAffirmationTextList0.result.IsSuccess);
            Assert.IsTrue(!tdefaultDailyAffirmationTextList0.result.Model.Any(x => x.DefaultDailyAffirmationTextId == rDefaultDailyAffirmationTextModel.result.Model.DefaultDailyAffirmationTextId));

            #endregion
            */
        }
    }
}