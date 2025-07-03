using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task VoiceNotificationServerTest_IntegrationTest()
        {
            #region VoiceNotificationServer
            //Tests for VoiceNotificationServer
            //Create Item Test

            var aVoiceNotificationServerModel = new VoiceNotificationServerCreateModel();
            var rVoiceNotificationServerModel = await http.CreateVoiceNotificationServer(aVoiceNotificationServerModel, userAdmin_Company_1);
            Assert.IsTrue(rVoiceNotificationServerModel.result.IsSuccess);

            aVoiceNotificationServerModel = new VoiceNotificationServerCreateModel();
            var r0VoiceNotificationServerModel = await http.CreateVoiceNotificationServer(aVoiceNotificationServerModel, userAdmin_Company_1);
            Assert.IsTrue(!r0VoiceNotificationServerModel.result.IsSuccess);

            aVoiceNotificationServerModel = new VoiceNotificationServerCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00VoiceNotificationServerModel = await http.CreateVoiceNotificationServer(aVoiceNotificationServerModel, userAdmin_Company_1);
            Assert.IsTrue(r00VoiceNotificationServerModel.result.IsSuccess);

            aVoiceNotificationServerModel = new VoiceNotificationServerCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01VoiceNotificationServerModel = await http.CreateVoiceNotificationServer(aVoiceNotificationServerModel, userAdmin_Company_2);
            Assert.IsTrue(!r01VoiceNotificationServerModel.result.IsSuccess);


            aVoiceNotificationServerModel = new VoiceNotificationServerCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01VoiceNotificationServerModel = await http.CreateVoiceNotificationServer(aVoiceNotificationServerModel, userAdmin_Company_2);
            Assert.IsTrue(r01VoiceNotificationServerModel.result.IsSuccess);

            //Get List Test
            var r2VoiceNotificationServerModel = await http.GetVoiceNotificationServers(new VoiceNotificationServerSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2VoiceNotificationServerModel.result.IsSuccess);
            Assert.IsTrue(r2VoiceNotificationServerModel.result.Model.Any(x => x.VoiceNotificationServerId == rVoiceNotificationServerModel.result.Model.VoiceNotificationServerId));

            var r20VoiceNotificationServerModel = await http.GetVoiceNotificationServers(new VoiceNotificationServerSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20VoiceNotificationServerModel.result.Model.Any(x => x.VoiceNotificationServerId == rVoiceNotificationServerModel.result.Model.VoiceNotificationServerId));

            //Get Item Test
            var r3VoiceNotificationServerModel = await http.GetVoiceNotificationServer(rVoiceNotificationServerModel.result.Model.VoiceNotificationServerId, userAdmin_Company_1);
            Assert.IsTrue(r3VoiceNotificationServerModel.result.IsSuccess);
            Assert.IsTrue(r3VoiceNotificationServerModel.result.Model.VoiceNotificationServerId == rVoiceNotificationServerModel.result.Model.VoiceNotificationServerId);

            var r30VoiceNotificationServerModel = await http.GetVoiceNotificationServer(rVoiceNotificationServerModel.result.Model.VoiceNotificationServerId, userAdmin_Company_2);
            Assert.IsTrue(!r30VoiceNotificationServerModel.result.IsSuccess);

            //Edit Item Test
            var r4VoiceNotificationServerModel = await http.UpdateVoiceNotificationServer(rVoiceNotificationServerModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4VoiceNotificationServerModel.result.IsSuccess);
            //Assert.IsTrue(r4VoiceNotificationServerModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40VoiceNotificationServerModel = await http.UpdateVoiceNotificationServer(rVoiceNotificationServerModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40VoiceNotificationServerModel.result.IsSuccess);
            #endregion

            /*
            #region VoiceNotificationServer Association with XYZZYX

            var tvoiceNotificationServerList0 = await http.GetVoiceNotificationServersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tvoiceNotificationServerList0.result.IsSuccess);

            var tvoiceNotificationServerAssociationR = await http.AssociateVoiceNotificationServerWithXYZZYX(new VoiceNotificationServerXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, VoiceNotificationServerId = rVoiceNotificationServerModel.result.Model.VoiceNotificationServerId }, userAdmin_Company_1);
            Assert.IsTrue(tvoiceNotificationServerAssociationR.result.IsSuccess);

            tvoiceNotificationServerList0 = await http.GetVoiceNotificationServersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tvoiceNotificationServerList0.result.IsSuccess);
            Assert.IsTrue(tvoiceNotificationServerList0.result.Model.Any(x => x.VoiceNotificationServerId == rVoiceNotificationServerModel.result.Model.VoiceNotificationServerId));

            var tvoiceNotificationServerUnAssociationR = await http.UnAssociateVoiceNotificationServerWithXYZZYX(new VoiceNotificationServerXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, VoiceNotificationServerId = rVoiceNotificationServerModel.result.Model.VoiceNotificationServerId }, userAdmin_Company_1);
            Assert.IsTrue(tvoiceNotificationServerUnAssociationR.result.IsSuccess);
            tvoiceNotificationServerList0 = await http.GetVoiceNotificationServersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tvoiceNotificationServerList0.result.IsSuccess);
            Assert.IsTrue(!tvoiceNotificationServerList0.result.Model.Any(x => x.VoiceNotificationServerId == rVoiceNotificationServerModel.result.Model.VoiceNotificationServerId));

            #endregion
            */
        }
    }
}