using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task PushNotificationServerTest_IntegrationTest()
        {
            #region PushNotificationServer
            //Tests for PushNotificationServer
            //Create Item Test

            var aPushNotificationServerModel = new PushNotificationServerCreateModel();
            var rPushNotificationServerModel = await http.CreatePushNotificationServer(aPushNotificationServerModel, userAdmin_Company_1);
            Assert.IsTrue(rPushNotificationServerModel.result.IsSuccess);

            aPushNotificationServerModel = new PushNotificationServerCreateModel();
            var r0PushNotificationServerModel = await http.CreatePushNotificationServer(aPushNotificationServerModel, userAdmin_Company_1);
            Assert.IsTrue(!r0PushNotificationServerModel.result.IsSuccess);

            aPushNotificationServerModel = new PushNotificationServerCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00PushNotificationServerModel = await http.CreatePushNotificationServer(aPushNotificationServerModel, userAdmin_Company_1);
            Assert.IsTrue(r00PushNotificationServerModel.result.IsSuccess);

            aPushNotificationServerModel = new PushNotificationServerCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01PushNotificationServerModel = await http.CreatePushNotificationServer(aPushNotificationServerModel, userAdmin_Company_2);
            Assert.IsTrue(!r01PushNotificationServerModel.result.IsSuccess);


            aPushNotificationServerModel = new PushNotificationServerCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01PushNotificationServerModel = await http.CreatePushNotificationServer(aPushNotificationServerModel, userAdmin_Company_2);
            Assert.IsTrue(r01PushNotificationServerModel.result.IsSuccess);

            //Get List Test
            var r2PushNotificationServerModel = await http.GetPushNotificationServers(new PushNotificationServerSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2PushNotificationServerModel.result.IsSuccess);
            Assert.IsTrue(r2PushNotificationServerModel.result.Model.Any(x => x.PushNotificationServerId == rPushNotificationServerModel.result.Model.PushNotificationServerId));

            var r20PushNotificationServerModel = await http.GetPushNotificationServers(new PushNotificationServerSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20PushNotificationServerModel.result.Model.Any(x => x.PushNotificationServerId == rPushNotificationServerModel.result.Model.PushNotificationServerId));

            //Get Item Test
            var r3PushNotificationServerModel = await http.GetPushNotificationServer(rPushNotificationServerModel.result.Model.PushNotificationServerId, userAdmin_Company_1);
            Assert.IsTrue(r3PushNotificationServerModel.result.IsSuccess);
            Assert.IsTrue(r3PushNotificationServerModel.result.Model.PushNotificationServerId == rPushNotificationServerModel.result.Model.PushNotificationServerId);

            var r30PushNotificationServerModel = await http.GetPushNotificationServer(rPushNotificationServerModel.result.Model.PushNotificationServerId, userAdmin_Company_2);
            Assert.IsTrue(!r30PushNotificationServerModel.result.IsSuccess);

            //Edit Item Test
            var r4PushNotificationServerModel = await http.UpdatePushNotificationServer(rPushNotificationServerModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4PushNotificationServerModel.result.IsSuccess);
            //Assert.IsTrue(r4PushNotificationServerModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40PushNotificationServerModel = await http.UpdatePushNotificationServer(rPushNotificationServerModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40PushNotificationServerModel.result.IsSuccess);
            #endregion

            /*
            #region PushNotificationServer Association with XYZZYX

            var tpushNotificationConfigurationList0 = await http.GetPushNotificationServersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tpushNotificationConfigurationList0.result.IsSuccess);

            var tpushNotificationConfigurationAssociationR = await http.AssociatePushNotificationServerWithXYZZYX(new PushNotificationServerXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, PushNotificationServerId = rPushNotificationServerModel.result.Model.PushNotificationServerId }, userAdmin_Company_1);
            Assert.IsTrue(tpushNotificationConfigurationAssociationR.result.IsSuccess);

            tpushNotificationConfigurationList0 = await http.GetPushNotificationServersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tpushNotificationConfigurationList0.result.IsSuccess);
            Assert.IsTrue(tpushNotificationConfigurationList0.result.Model.Any(x => x.PushNotificationServerId == rPushNotificationServerModel.result.Model.PushNotificationServerId));

            var tpushNotificationConfigurationUnAssociationR = await http.UnAssociatePushNotificationServerWithXYZZYX(new PushNotificationServerXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, PushNotificationServerId = rPushNotificationServerModel.result.Model.PushNotificationServerId }, userAdmin_Company_1);
            Assert.IsTrue(tpushNotificationConfigurationUnAssociationR.result.IsSuccess);
            tpushNotificationConfigurationList0 = await http.GetPushNotificationServersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tpushNotificationConfigurationList0.result.IsSuccess);
            Assert.IsTrue(!tpushNotificationConfigurationList0.result.Model.Any(x => x.PushNotificationServerId == rPushNotificationServerModel.result.Model.PushNotificationServerId));

            #endregion
            */
        }
    }
}