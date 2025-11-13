using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task DailyAffirmationSessionTest_IntegrationTest()
        {
            #region DailyAffirmationSession
            //Tests for DailyAffirmationSession
            //Create Item Test

            var aDailyAffirmationSessionModel = new DailyAffirmationSessionCreateModel();
            var rDailyAffirmationSessionModel = await http.CreateDailyAffirmationSession(aDailyAffirmationSessionModel, userAdmin_Company_1);
            Assert.IsTrue(rDailyAffirmationSessionModel.result.IsSuccess);

            aDailyAffirmationSessionModel = new DailyAffirmationSessionCreateModel();
            var r0DailyAffirmationSessionModel = await http.CreateDailyAffirmationSession(aDailyAffirmationSessionModel, userAdmin_Company_1);
            Assert.IsTrue(!r0DailyAffirmationSessionModel.result.IsSuccess);

            aDailyAffirmationSessionModel = new DailyAffirmationSessionCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00DailyAffirmationSessionModel = await http.CreateDailyAffirmationSession(aDailyAffirmationSessionModel, userAdmin_Company_1);
            Assert.IsTrue(r00DailyAffirmationSessionModel.result.IsSuccess);

            aDailyAffirmationSessionModel = new DailyAffirmationSessionCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01DailyAffirmationSessionModel = await http.CreateDailyAffirmationSession(aDailyAffirmationSessionModel, userAdmin_Company_2);
            Assert.IsTrue(!r01DailyAffirmationSessionModel.result.IsSuccess);


            aDailyAffirmationSessionModel = new DailyAffirmationSessionCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01DailyAffirmationSessionModel = await http.CreateDailyAffirmationSession(aDailyAffirmationSessionModel, userAdmin_Company_2);
            Assert.IsTrue(r01DailyAffirmationSessionModel.result.IsSuccess);

            //Get List Test
            var r2DailyAffirmationSessionModel = await http.GetDailyAffirmationSessions(new DailyAffirmationSessionSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2DailyAffirmationSessionModel.result.IsSuccess);
            Assert.IsTrue(r2DailyAffirmationSessionModel.result.Model.Any(x => x.DailyAffirmationSessionId == rDailyAffirmationSessionModel.result.Model.DailyAffirmationSessionId));

            var r20DailyAffirmationSessionModel = await http.GetDailyAffirmationSessions(new DailyAffirmationSessionSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20DailyAffirmationSessionModel.result.Model.Any(x => x.DailyAffirmationSessionId == rDailyAffirmationSessionModel.result.Model.DailyAffirmationSessionId));

            //Get Item Test
            var r3DailyAffirmationSessionModel = await http.GetDailyAffirmationSession(rDailyAffirmationSessionModel.result.Model.DailyAffirmationSessionId, userAdmin_Company_1);
            Assert.IsTrue(r3DailyAffirmationSessionModel.result.IsSuccess);
            Assert.IsTrue(r3DailyAffirmationSessionModel.result.Model.DailyAffirmationSessionId == rDailyAffirmationSessionModel.result.Model.DailyAffirmationSessionId);

            var r30DailyAffirmationSessionModel = await http.GetDailyAffirmationSession(rDailyAffirmationSessionModel.result.Model.DailyAffirmationSessionId, userAdmin_Company_2);
            Assert.IsTrue(!r30DailyAffirmationSessionModel.result.IsSuccess);

            //Edit Item Test
            var r4DailyAffirmationSessionModel = await http.UpdateDailyAffirmationSession(rDailyAffirmationSessionModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4DailyAffirmationSessionModel.result.IsSuccess);
            //Assert.IsTrue(r4DailyAffirmationSessionModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40DailyAffirmationSessionModel = await http.UpdateDailyAffirmationSession(rDailyAffirmationSessionModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40DailyAffirmationSessionModel.result.IsSuccess);
            #endregion

            /*
            #region DailyAffirmationSession Association with XYZZYX

            var tdailyAffirmationSessionList0 = await http.GetDailyAffirmationSessionsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdailyAffirmationSessionList0.result.IsSuccess);

            var tdailyAffirmationSessionAssociationR = await http.AssociateDailyAffirmationSessionWithXYZZYX(new DailyAffirmationSessionXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DailyAffirmationSessionId = rDailyAffirmationSessionModel.result.Model.DailyAffirmationSessionId }, userAdmin_Company_1);
            Assert.IsTrue(tdailyAffirmationSessionAssociationR.result.IsSuccess);

            tdailyAffirmationSessionList0 = await http.GetDailyAffirmationSessionsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdailyAffirmationSessionList0.result.IsSuccess);
            Assert.IsTrue(tdailyAffirmationSessionList0.result.Model.Any(x => x.DailyAffirmationSessionId == rDailyAffirmationSessionModel.result.Model.DailyAffirmationSessionId));

            var tdailyAffirmationSessionUnAssociationR = await http.UnAssociateDailyAffirmationSessionWithXYZZYX(new DailyAffirmationSessionXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DailyAffirmationSessionId = rDailyAffirmationSessionModel.result.Model.DailyAffirmationSessionId }, userAdmin_Company_1);
            Assert.IsTrue(tdailyAffirmationSessionUnAssociationR.result.IsSuccess);
            tdailyAffirmationSessionList0 = await http.GetDailyAffirmationSessionsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdailyAffirmationSessionList0.result.IsSuccess);
            Assert.IsTrue(!tdailyAffirmationSessionList0.result.Model.Any(x => x.DailyAffirmationSessionId == rDailyAffirmationSessionModel.result.Model.DailyAffirmationSessionId));

            #endregion
            */
        }
    }
}