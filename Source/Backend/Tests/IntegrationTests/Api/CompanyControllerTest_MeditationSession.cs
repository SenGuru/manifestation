using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task MeditationSessionTest_IntegrationTest()
        {
            #region MeditationSession
            //Tests for MeditationSession
            //Create Item Test

            var aMeditationSessionModel = new MeditationSessionCreateModel();
            var rMeditationSessionModel = await http.CreateMeditationSession(aMeditationSessionModel, userAdmin_Company_1);
            Assert.IsTrue(rMeditationSessionModel.result.IsSuccess);

            aMeditationSessionModel = new MeditationSessionCreateModel();
            var r0MeditationSessionModel = await http.CreateMeditationSession(aMeditationSessionModel, userAdmin_Company_1);
            Assert.IsTrue(!r0MeditationSessionModel.result.IsSuccess);

            aMeditationSessionModel = new MeditationSessionCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00MeditationSessionModel = await http.CreateMeditationSession(aMeditationSessionModel, userAdmin_Company_1);
            Assert.IsTrue(r00MeditationSessionModel.result.IsSuccess);

            aMeditationSessionModel = new MeditationSessionCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01MeditationSessionModel = await http.CreateMeditationSession(aMeditationSessionModel, userAdmin_Company_2);
            Assert.IsTrue(!r01MeditationSessionModel.result.IsSuccess);


            aMeditationSessionModel = new MeditationSessionCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01MeditationSessionModel = await http.CreateMeditationSession(aMeditationSessionModel, userAdmin_Company_2);
            Assert.IsTrue(r01MeditationSessionModel.result.IsSuccess);

            //Get List Test
            var r2MeditationSessionModel = await http.GetMeditationSessions(new MeditationSessionSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2MeditationSessionModel.result.IsSuccess);
            Assert.IsTrue(r2MeditationSessionModel.result.Model.Any(x => x.MeditationSessionId == rMeditationSessionModel.result.Model.MeditationSessionId));

            var r20MeditationSessionModel = await http.GetMeditationSessions(new MeditationSessionSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20MeditationSessionModel.result.Model.Any(x => x.MeditationSessionId == rMeditationSessionModel.result.Model.MeditationSessionId));

            //Get Item Test
            var r3MeditationSessionModel = await http.GetMeditationSession(rMeditationSessionModel.result.Model.MeditationSessionId, userAdmin_Company_1);
            Assert.IsTrue(r3MeditationSessionModel.result.IsSuccess);
            Assert.IsTrue(r3MeditationSessionModel.result.Model.MeditationSessionId == rMeditationSessionModel.result.Model.MeditationSessionId);

            var r30MeditationSessionModel = await http.GetMeditationSession(rMeditationSessionModel.result.Model.MeditationSessionId, userAdmin_Company_2);
            Assert.IsTrue(!r30MeditationSessionModel.result.IsSuccess);

            //Edit Item Test
            var r4MeditationSessionModel = await http.UpdateMeditationSession(rMeditationSessionModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4MeditationSessionModel.result.IsSuccess);
            //Assert.IsTrue(r4MeditationSessionModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40MeditationSessionModel = await http.UpdateMeditationSession(rMeditationSessionModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40MeditationSessionModel.result.IsSuccess);
            #endregion

            /*
            #region MeditationSession Association with XYZZYX

            var tmeditationSessionList0 = await http.GetMeditationSessionsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tmeditationSessionList0.result.IsSuccess);

            var tmeditationSessionAssociationR = await http.AssociateMeditationSessionWithXYZZYX(new MeditationSessionXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, MeditationSessionId = rMeditationSessionModel.result.Model.MeditationSessionId }, userAdmin_Company_1);
            Assert.IsTrue(tmeditationSessionAssociationR.result.IsSuccess);

            tmeditationSessionList0 = await http.GetMeditationSessionsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tmeditationSessionList0.result.IsSuccess);
            Assert.IsTrue(tmeditationSessionList0.result.Model.Any(x => x.MeditationSessionId == rMeditationSessionModel.result.Model.MeditationSessionId));

            var tmeditationSessionUnAssociationR = await http.UnAssociateMeditationSessionWithXYZZYX(new MeditationSessionXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, MeditationSessionId = rMeditationSessionModel.result.Model.MeditationSessionId }, userAdmin_Company_1);
            Assert.IsTrue(tmeditationSessionUnAssociationR.result.IsSuccess);
            tmeditationSessionList0 = await http.GetMeditationSessionsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tmeditationSessionList0.result.IsSuccess);
            Assert.IsTrue(!tmeditationSessionList0.result.Model.Any(x => x.MeditationSessionId == rMeditationSessionModel.result.Model.MeditationSessionId));

            #endregion
            */
        }
    }
}