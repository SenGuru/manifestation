using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task VisionBoardSessionTest_IntegrationTest()
        {
            #region VisionBoardSession
            //Tests for VisionBoardSession
            //Create Item Test

            var aVisionBoardSessionModel = new VisionBoardSessionCreateModel();
            var rVisionBoardSessionModel = await http.CreateVisionBoardSession(aVisionBoardSessionModel, userAdmin_Company_1);
            Assert.IsTrue(rVisionBoardSessionModel.result.IsSuccess);

            aVisionBoardSessionModel = new VisionBoardSessionCreateModel();
            var r0VisionBoardSessionModel = await http.CreateVisionBoardSession(aVisionBoardSessionModel, userAdmin_Company_1);
            Assert.IsTrue(!r0VisionBoardSessionModel.result.IsSuccess);

            aVisionBoardSessionModel = new VisionBoardSessionCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00VisionBoardSessionModel = await http.CreateVisionBoardSession(aVisionBoardSessionModel, userAdmin_Company_1);
            Assert.IsTrue(r00VisionBoardSessionModel.result.IsSuccess);

            aVisionBoardSessionModel = new VisionBoardSessionCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01VisionBoardSessionModel = await http.CreateVisionBoardSession(aVisionBoardSessionModel, userAdmin_Company_2);
            Assert.IsTrue(!r01VisionBoardSessionModel.result.IsSuccess);


            aVisionBoardSessionModel = new VisionBoardSessionCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01VisionBoardSessionModel = await http.CreateVisionBoardSession(aVisionBoardSessionModel, userAdmin_Company_2);
            Assert.IsTrue(r01VisionBoardSessionModel.result.IsSuccess);

            //Get List Test
            var r2VisionBoardSessionModel = await http.GetVisionBoardSessions(new VisionBoardSessionSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2VisionBoardSessionModel.result.IsSuccess);
            Assert.IsTrue(r2VisionBoardSessionModel.result.Model.Any(x => x.VisionBoardSessionId == rVisionBoardSessionModel.result.Model.VisionBoardSessionId));

            var r20VisionBoardSessionModel = await http.GetVisionBoardSessions(new VisionBoardSessionSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20VisionBoardSessionModel.result.Model.Any(x => x.VisionBoardSessionId == rVisionBoardSessionModel.result.Model.VisionBoardSessionId));

            //Get Item Test
            var r3VisionBoardSessionModel = await http.GetVisionBoardSession(rVisionBoardSessionModel.result.Model.VisionBoardSessionId, userAdmin_Company_1);
            Assert.IsTrue(r3VisionBoardSessionModel.result.IsSuccess);
            Assert.IsTrue(r3VisionBoardSessionModel.result.Model.VisionBoardSessionId == rVisionBoardSessionModel.result.Model.VisionBoardSessionId);

            var r30VisionBoardSessionModel = await http.GetVisionBoardSession(rVisionBoardSessionModel.result.Model.VisionBoardSessionId, userAdmin_Company_2);
            Assert.IsTrue(!r30VisionBoardSessionModel.result.IsSuccess);

            //Edit Item Test
            var r4VisionBoardSessionModel = await http.UpdateVisionBoardSession(rVisionBoardSessionModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4VisionBoardSessionModel.result.IsSuccess);
            //Assert.IsTrue(r4VisionBoardSessionModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40VisionBoardSessionModel = await http.UpdateVisionBoardSession(rVisionBoardSessionModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40VisionBoardSessionModel.result.IsSuccess);
            #endregion

            /*
            #region VisionBoardSession Association with XYZZYX

            var tvisionBoardSessionList0 = await http.GetVisionBoardSessionsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tvisionBoardSessionList0.result.IsSuccess);

            var tvisionBoardSessionAssociationR = await http.AssociateVisionBoardSessionWithXYZZYX(new VisionBoardSessionXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, VisionBoardSessionId = rVisionBoardSessionModel.result.Model.VisionBoardSessionId }, userAdmin_Company_1);
            Assert.IsTrue(tvisionBoardSessionAssociationR.result.IsSuccess);

            tvisionBoardSessionList0 = await http.GetVisionBoardSessionsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tvisionBoardSessionList0.result.IsSuccess);
            Assert.IsTrue(tvisionBoardSessionList0.result.Model.Any(x => x.VisionBoardSessionId == rVisionBoardSessionModel.result.Model.VisionBoardSessionId));

            var tvisionBoardSessionUnAssociationR = await http.UnAssociateVisionBoardSessionWithXYZZYX(new VisionBoardSessionXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, VisionBoardSessionId = rVisionBoardSessionModel.result.Model.VisionBoardSessionId }, userAdmin_Company_1);
            Assert.IsTrue(tvisionBoardSessionUnAssociationR.result.IsSuccess);
            tvisionBoardSessionList0 = await http.GetVisionBoardSessionsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tvisionBoardSessionList0.result.IsSuccess);
            Assert.IsTrue(!tvisionBoardSessionList0.result.Model.Any(x => x.VisionBoardSessionId == rVisionBoardSessionModel.result.Model.VisionBoardSessionId));

            #endregion
            */
        }
    }
}