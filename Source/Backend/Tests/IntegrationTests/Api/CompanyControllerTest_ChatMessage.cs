using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task ChatMessageTest_IntegrationTest()
        {
            #region ChatMessage
            //Tests for ChatMessage
            //Create Item Test

            var aChatMessageModel = new ChatMessageCreateModel();
            var rChatMessageModel = await http.CreateChatMessage(aChatMessageModel, userAdmin_Company_1);
            Assert.IsTrue(rChatMessageModel.result.IsSuccess);

            aChatMessageModel = new ChatMessageCreateModel();
            var r0ChatMessageModel = await http.CreateChatMessage(aChatMessageModel, userAdmin_Company_1);
            Assert.IsTrue(!r0ChatMessageModel.result.IsSuccess);

            aChatMessageModel = new ChatMessageCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00ChatMessageModel = await http.CreateChatMessage(aChatMessageModel, userAdmin_Company_1);
            Assert.IsTrue(r00ChatMessageModel.result.IsSuccess);

            aChatMessageModel = new ChatMessageCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01ChatMessageModel = await http.CreateChatMessage(aChatMessageModel, userAdmin_Company_2);
            Assert.IsTrue(!r01ChatMessageModel.result.IsSuccess);


            aChatMessageModel = new ChatMessageCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01ChatMessageModel = await http.CreateChatMessage(aChatMessageModel, userAdmin_Company_2);
            Assert.IsTrue(r01ChatMessageModel.result.IsSuccess);

            //Get List Test
            var r2ChatMessageModel = await http.GetChatMessages(new ChatMessageSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2ChatMessageModel.result.IsSuccess);
            Assert.IsTrue(r2ChatMessageModel.result.Model.Any(x => x.ChatMessageId == rChatMessageModel.result.Model.ChatMessageId));

            var r20ChatMessageModel = await http.GetChatMessages(new ChatMessageSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20ChatMessageModel.result.Model.Any(x => x.ChatMessageId == rChatMessageModel.result.Model.ChatMessageId));

            //Get Item Test
            var r3ChatMessageModel = await http.GetChatMessage(rChatMessageModel.result.Model.ChatMessageId, userAdmin_Company_1);
            Assert.IsTrue(r3ChatMessageModel.result.IsSuccess);
            Assert.IsTrue(r3ChatMessageModel.result.Model.ChatMessageId == rChatMessageModel.result.Model.ChatMessageId);

            var r30ChatMessageModel = await http.GetChatMessage(rChatMessageModel.result.Model.ChatMessageId, userAdmin_Company_2);
            Assert.IsTrue(!r30ChatMessageModel.result.IsSuccess);

            //Edit Item Test
            var r4ChatMessageModel = await http.UpdateChatMessage(rChatMessageModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4ChatMessageModel.result.IsSuccess);
            //Assert.IsTrue(r4ChatMessageModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40ChatMessageModel = await http.UpdateChatMessage(rChatMessageModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40ChatMessageModel.result.IsSuccess);
            #endregion

            /*
            #region ChatMessage Association with XYZZYX

            var tchatMessageList0 = await http.GetChatMessagesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tchatMessageList0.result.IsSuccess);

            var tchatMessageAssociationR = await http.AssociateChatMessageWithXYZZYX(new ChatMessageXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ChatMessageId = rChatMessageModel.result.Model.ChatMessageId }, userAdmin_Company_1);
            Assert.IsTrue(tchatMessageAssociationR.result.IsSuccess);

            tchatMessageList0 = await http.GetChatMessagesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tchatMessageList0.result.IsSuccess);
            Assert.IsTrue(tchatMessageList0.result.Model.Any(x => x.ChatMessageId == rChatMessageModel.result.Model.ChatMessageId));

            var tchatMessageUnAssociationR = await http.UnAssociateChatMessageWithXYZZYX(new ChatMessageXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ChatMessageId = rChatMessageModel.result.Model.ChatMessageId }, userAdmin_Company_1);
            Assert.IsTrue(tchatMessageUnAssociationR.result.IsSuccess);
            tchatMessageList0 = await http.GetChatMessagesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tchatMessageList0.result.IsSuccess);
            Assert.IsTrue(!tchatMessageList0.result.Model.Any(x => x.ChatMessageId == rChatMessageModel.result.Model.ChatMessageId));

            #endregion
            */
        }
    }
}