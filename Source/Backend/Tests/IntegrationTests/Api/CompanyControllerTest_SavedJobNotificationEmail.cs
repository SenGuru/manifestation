using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task SavedJobNotificationEmailTest_IntegrationTest()
        {
            #region SavedJobNotificationEmail
            //Tests for SavedJobNotificationEmail
            //Create Item Test

            var aSavedJobNotificationEmailModel = new SavedJobNotificationEmailCreateModel();
            var rSavedJobNotificationEmailModel = await http.CreateSavedJobNotificationEmail(aSavedJobNotificationEmailModel, userAdmin_Company_1);
            Assert.IsTrue(rSavedJobNotificationEmailModel.result.IsSuccess);

            aSavedJobNotificationEmailModel = new SavedJobNotificationEmailCreateModel();
            var r0SavedJobNotificationEmailModel = await http.CreateSavedJobNotificationEmail(aSavedJobNotificationEmailModel, userAdmin_Company_1);
            Assert.IsTrue(!r0SavedJobNotificationEmailModel.result.IsSuccess);

            aSavedJobNotificationEmailModel = new SavedJobNotificationEmailCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00SavedJobNotificationEmailModel = await http.CreateSavedJobNotificationEmail(aSavedJobNotificationEmailModel, userAdmin_Company_1);
            Assert.IsTrue(r00SavedJobNotificationEmailModel.result.IsSuccess);

            aSavedJobNotificationEmailModel = new SavedJobNotificationEmailCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01SavedJobNotificationEmailModel = await http.CreateSavedJobNotificationEmail(aSavedJobNotificationEmailModel, userAdmin_Company_2);
            Assert.IsTrue(!r01SavedJobNotificationEmailModel.result.IsSuccess);


            aSavedJobNotificationEmailModel = new SavedJobNotificationEmailCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01SavedJobNotificationEmailModel = await http.CreateSavedJobNotificationEmail(aSavedJobNotificationEmailModel, userAdmin_Company_2);
            Assert.IsTrue(r01SavedJobNotificationEmailModel.result.IsSuccess);

            //Get List Test
            var r2SavedJobNotificationEmailModel = await http.GetSavedJobNotificationEmails(new SavedJobNotificationEmailSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2SavedJobNotificationEmailModel.result.IsSuccess);
            Assert.IsTrue(r2SavedJobNotificationEmailModel.result.Model.Any(x => x.SavedJobNotificationEmailId == rSavedJobNotificationEmailModel.result.Model.SavedJobNotificationEmailId));

            var r20SavedJobNotificationEmailModel = await http.GetSavedJobNotificationEmails(new SavedJobNotificationEmailSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20SavedJobNotificationEmailModel.result.Model.Any(x => x.SavedJobNotificationEmailId == rSavedJobNotificationEmailModel.result.Model.SavedJobNotificationEmailId));

            //Get Item Test
            var r3SavedJobNotificationEmailModel = await http.GetSavedJobNotificationEmail(rSavedJobNotificationEmailModel.result.Model.SavedJobNotificationEmailId, userAdmin_Company_1);
            Assert.IsTrue(r3SavedJobNotificationEmailModel.result.IsSuccess);
            Assert.IsTrue(r3SavedJobNotificationEmailModel.result.Model.SavedJobNotificationEmailId == rSavedJobNotificationEmailModel.result.Model.SavedJobNotificationEmailId);

            var r30SavedJobNotificationEmailModel = await http.GetSavedJobNotificationEmail(rSavedJobNotificationEmailModel.result.Model.SavedJobNotificationEmailId, userAdmin_Company_2);
            Assert.IsTrue(!r30SavedJobNotificationEmailModel.result.IsSuccess);

            //Edit Item Test
            var r4SavedJobNotificationEmailModel = await http.UpdateSavedJobNotificationEmail(rSavedJobNotificationEmailModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4SavedJobNotificationEmailModel.result.IsSuccess);
            //Assert.IsTrue(r4SavedJobNotificationEmailModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40SavedJobNotificationEmailModel = await http.UpdateSavedJobNotificationEmail(rSavedJobNotificationEmailModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40SavedJobNotificationEmailModel.result.IsSuccess);
            #endregion

            /*
            #region SavedJobNotificationEmail Association with XYZZYX

            var tsavedJobNotificationEmailList0 = await http.GetSavedJobNotificationEmailsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tsavedJobNotificationEmailList0.result.IsSuccess);

            var tsavedJobNotificationEmailAssociationR = await http.AssociateSavedJobNotificationEmailWithXYZZYX(new SavedJobNotificationEmailXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, SavedJobNotificationEmailId = rSavedJobNotificationEmailModel.result.Model.SavedJobNotificationEmailId }, userAdmin_Company_1);
            Assert.IsTrue(tsavedJobNotificationEmailAssociationR.result.IsSuccess);

            tsavedJobNotificationEmailList0 = await http.GetSavedJobNotificationEmailsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tsavedJobNotificationEmailList0.result.IsSuccess);
            Assert.IsTrue(tsavedJobNotificationEmailList0.result.Model.Any(x => x.SavedJobNotificationEmailId == rSavedJobNotificationEmailModel.result.Model.SavedJobNotificationEmailId));

            var tsavedJobNotificationEmailUnAssociationR = await http.UnAssociateSavedJobNotificationEmailWithXYZZYX(new SavedJobNotificationEmailXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, SavedJobNotificationEmailId = rSavedJobNotificationEmailModel.result.Model.SavedJobNotificationEmailId }, userAdmin_Company_1);
            Assert.IsTrue(tsavedJobNotificationEmailUnAssociationR.result.IsSuccess);
            tsavedJobNotificationEmailList0 = await http.GetSavedJobNotificationEmailsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tsavedJobNotificationEmailList0.result.IsSuccess);
            Assert.IsTrue(!tsavedJobNotificationEmailList0.result.Model.Any(x => x.SavedJobNotificationEmailId == rSavedJobNotificationEmailModel.result.Model.SavedJobNotificationEmailId));

            #endregion
            */
        }
    }
}