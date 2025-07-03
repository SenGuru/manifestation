using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task WhatsAppNotificationServerTest_IntegrationTest()
        {
            #region WhatsAppNotificationServer
            //Tests for WhatsAppNotificationServer
            //Create Item Test

            var aWhatsAppNotificationServerModel = new WhatsAppNotificationServerCreateModel();
            var rWhatsAppNotificationServerModel = await http.CreateWhatsAppNotificationServer(aWhatsAppNotificationServerModel, userAdmin_Company_1);
            Assert.IsTrue(rWhatsAppNotificationServerModel.result.IsSuccess);

            aWhatsAppNotificationServerModel = new WhatsAppNotificationServerCreateModel();
            var r0WhatsAppNotificationServerModel = await http.CreateWhatsAppNotificationServer(aWhatsAppNotificationServerModel, userAdmin_Company_1);
            Assert.IsTrue(!r0WhatsAppNotificationServerModel.result.IsSuccess);

            aWhatsAppNotificationServerModel = new WhatsAppNotificationServerCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00WhatsAppNotificationServerModel = await http.CreateWhatsAppNotificationServer(aWhatsAppNotificationServerModel, userAdmin_Company_1);
            Assert.IsTrue(r00WhatsAppNotificationServerModel.result.IsSuccess);

            aWhatsAppNotificationServerModel = new WhatsAppNotificationServerCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01WhatsAppNotificationServerModel = await http.CreateWhatsAppNotificationServer(aWhatsAppNotificationServerModel, userAdmin_Company_2);
            Assert.IsTrue(!r01WhatsAppNotificationServerModel.result.IsSuccess);


            aWhatsAppNotificationServerModel = new WhatsAppNotificationServerCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01WhatsAppNotificationServerModel = await http.CreateWhatsAppNotificationServer(aWhatsAppNotificationServerModel, userAdmin_Company_2);
            Assert.IsTrue(r01WhatsAppNotificationServerModel.result.IsSuccess);

            //Get List Test
            var r2WhatsAppNotificationServerModel = await http.GetWhatsAppNotificationServers(new WhatsAppNotificationServerSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2WhatsAppNotificationServerModel.result.IsSuccess);
            Assert.IsTrue(r2WhatsAppNotificationServerModel.result.Model.Any(x => x.WhatsAppNotificationServerId == rWhatsAppNotificationServerModel.result.Model.WhatsAppNotificationServerId));

            var r20WhatsAppNotificationServerModel = await http.GetWhatsAppNotificationServers(new WhatsAppNotificationServerSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20WhatsAppNotificationServerModel.result.Model.Any(x => x.WhatsAppNotificationServerId == rWhatsAppNotificationServerModel.result.Model.WhatsAppNotificationServerId));

            //Get Item Test
            var r3WhatsAppNotificationServerModel = await http.GetWhatsAppNotificationServer(rWhatsAppNotificationServerModel.result.Model.WhatsAppNotificationServerId, userAdmin_Company_1);
            Assert.IsTrue(r3WhatsAppNotificationServerModel.result.IsSuccess);
            Assert.IsTrue(r3WhatsAppNotificationServerModel.result.Model.WhatsAppNotificationServerId == rWhatsAppNotificationServerModel.result.Model.WhatsAppNotificationServerId);

            var r30WhatsAppNotificationServerModel = await http.GetWhatsAppNotificationServer(rWhatsAppNotificationServerModel.result.Model.WhatsAppNotificationServerId, userAdmin_Company_2);
            Assert.IsTrue(!r30WhatsAppNotificationServerModel.result.IsSuccess);

            //Edit Item Test
            var r4WhatsAppNotificationServerModel = await http.UpdateWhatsAppNotificationServer(rWhatsAppNotificationServerModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4WhatsAppNotificationServerModel.result.IsSuccess);
            //Assert.IsTrue(r4WhatsAppNotificationServerModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40WhatsAppNotificationServerModel = await http.UpdateWhatsAppNotificationServer(rWhatsAppNotificationServerModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40WhatsAppNotificationServerModel.result.IsSuccess);
            #endregion

            /*
            #region WhatsAppNotificationServer Association with XYZZYX

            var twhatsAppNotificationServerList0 = await http.GetWhatsAppNotificationServersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(twhatsAppNotificationServerList0.result.IsSuccess);

            var twhatsAppNotificationServerAssociationR = await http.AssociateWhatsAppNotificationServerWithXYZZYX(new WhatsAppNotificationServerXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, WhatsAppNotificationServerId = rWhatsAppNotificationServerModel.result.Model.WhatsAppNotificationServerId }, userAdmin_Company_1);
            Assert.IsTrue(twhatsAppNotificationServerAssociationR.result.IsSuccess);

            twhatsAppNotificationServerList0 = await http.GetWhatsAppNotificationServersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(twhatsAppNotificationServerList0.result.IsSuccess);
            Assert.IsTrue(twhatsAppNotificationServerList0.result.Model.Any(x => x.WhatsAppNotificationServerId == rWhatsAppNotificationServerModel.result.Model.WhatsAppNotificationServerId));

            var twhatsAppNotificationServerUnAssociationR = await http.UnAssociateWhatsAppNotificationServerWithXYZZYX(new WhatsAppNotificationServerXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, WhatsAppNotificationServerId = rWhatsAppNotificationServerModel.result.Model.WhatsAppNotificationServerId }, userAdmin_Company_1);
            Assert.IsTrue(twhatsAppNotificationServerUnAssociationR.result.IsSuccess);
            twhatsAppNotificationServerList0 = await http.GetWhatsAppNotificationServersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(twhatsAppNotificationServerList0.result.IsSuccess);
            Assert.IsTrue(!twhatsAppNotificationServerList0.result.Model.Any(x => x.WhatsAppNotificationServerId == rWhatsAppNotificationServerModel.result.Model.WhatsAppNotificationServerId));

            #endregion
            */
        }
    }
}