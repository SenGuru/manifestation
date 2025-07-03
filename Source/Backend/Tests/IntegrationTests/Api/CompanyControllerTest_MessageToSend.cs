using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task MessageToSendTest_IntegrationTest()
        {
            #region MessageToSend
            //Tests for MessageToSend
            //Create Item Test

            var aMessageToSendModel = new MessageToSendCreateModel();
            var rMessageToSendModel = await http.CreateMessageToSend(aMessageToSendModel, userAdmin_Company_1);
            Assert.IsTrue(rMessageToSendModel.result.IsSuccess);

            aMessageToSendModel = new MessageToSendCreateModel();
            var r0MessageToSendModel = await http.CreateMessageToSend(aMessageToSendModel, userAdmin_Company_1);
            Assert.IsTrue(!r0MessageToSendModel.result.IsSuccess);

            aMessageToSendModel = new MessageToSendCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00MessageToSendModel = await http.CreateMessageToSend(aMessageToSendModel, userAdmin_Company_1);
            Assert.IsTrue(r00MessageToSendModel.result.IsSuccess);

            aMessageToSendModel = new MessageToSendCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01MessageToSendModel = await http.CreateMessageToSend(aMessageToSendModel, userAdmin_Company_2);
            Assert.IsTrue(!r01MessageToSendModel.result.IsSuccess);


            aMessageToSendModel = new MessageToSendCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01MessageToSendModel = await http.CreateMessageToSend(aMessageToSendModel, userAdmin_Company_2);
            Assert.IsTrue(r01MessageToSendModel.result.IsSuccess);

            //Get List Test
            var r2MessageToSendModel = await http.GetMessageToSends(new MessageToSendSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2MessageToSendModel.result.IsSuccess);
            Assert.IsTrue(r2MessageToSendModel.result.Model.Any(x => x.MessageToSendId == rMessageToSendModel.result.Model.MessageToSendId));

            var r20MessageToSendModel = await http.GetMessageToSends(new MessageToSendSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20MessageToSendModel.result.Model.Any(x => x.MessageToSendId == rMessageToSendModel.result.Model.MessageToSendId));

            //Get Item Test
            var r3MessageToSendModel = await http.GetMessageToSend(rMessageToSendModel.result.Model.MessageToSendId, userAdmin_Company_1);
            Assert.IsTrue(r3MessageToSendModel.result.IsSuccess);
            Assert.IsTrue(r3MessageToSendModel.result.Model.MessageToSendId == rMessageToSendModel.result.Model.MessageToSendId);

            var r30MessageToSendModel = await http.GetMessageToSend(rMessageToSendModel.result.Model.MessageToSendId, userAdmin_Company_2);
            Assert.IsTrue(!r30MessageToSendModel.result.IsSuccess);

            #endregion

            /*
            #region MessageToSend Association with XYZZYX

            var tmessageToSendList0 = await http.GetMessageToSendsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tmessageToSendList0.result.IsSuccess);

            var tmessageToSendAssociationR = await http.AssociateMessageToSendWithXYZZYX(new MessageToSendXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, MessageToSendId = rMessageToSendModel.result.Model.MessageToSendId }, userAdmin_Company_1);
            Assert.IsTrue(tmessageToSendAssociationR.result.IsSuccess);

            tmessageToSendList0 = await http.GetMessageToSendsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tmessageToSendList0.result.IsSuccess);
            Assert.IsTrue(tmessageToSendList0.result.Model.Any(x => x.MessageToSendId == rMessageToSendModel.result.Model.MessageToSendId));

            var tmessageToSendUnAssociationR = await http.UnAssociateMessageToSendWithXYZZYX(new MessageToSendXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, MessageToSendId = rMessageToSendModel.result.Model.MessageToSendId }, userAdmin_Company_1);
            Assert.IsTrue(tmessageToSendUnAssociationR.result.IsSuccess);
            tmessageToSendList0 = await http.GetMessageToSendsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tmessageToSendList0.result.IsSuccess);
            Assert.IsTrue(!tmessageToSendList0.result.Model.Any(x => x.MessageToSendId == rMessageToSendModel.result.Model.MessageToSendId));

            #endregion
            */
        }
    }
}