using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task ChatTest_IntegrationTest()
        {
            #region Chat
            //Tests for Chat
            //Create Item Test

            var aChatModel = new ChatCreateModel();
            var rChatModel = await http.CreateChat(aChatModel, userAdmin_Company_1);
            Assert.IsTrue(rChatModel.result.IsSuccess);

            aChatModel = new ChatCreateModel();
            var r0ChatModel = await http.CreateChat(aChatModel, userAdmin_Company_1);
            Assert.IsTrue(!r0ChatModel.result.IsSuccess);

            aChatModel = new ChatCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00ChatModel = await http.CreateChat(aChatModel, userAdmin_Company_1);
            Assert.IsTrue(r00ChatModel.result.IsSuccess);

            aChatModel = new ChatCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01ChatModel = await http.CreateChat(aChatModel, userAdmin_Company_2);
            Assert.IsTrue(!r01ChatModel.result.IsSuccess);


            aChatModel = new ChatCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01ChatModel = await http.CreateChat(aChatModel, userAdmin_Company_2);
            Assert.IsTrue(r01ChatModel.result.IsSuccess);

            //Get List Test
            var r2ChatModel = await http.GetChats(new ChatSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2ChatModel.result.IsSuccess);
            Assert.IsTrue(r2ChatModel.result.Model.Any(x => x.ChatId == rChatModel.result.Model.ChatId));

            var r20ChatModel = await http.GetChats(new ChatSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20ChatModel.result.Model.Any(x => x.ChatId == rChatModel.result.Model.ChatId));

            //Get Item Test
            var r3ChatModel = await http.GetChat(rChatModel.result.Model.ChatId, userAdmin_Company_1);
            Assert.IsTrue(r3ChatModel.result.IsSuccess);
            Assert.IsTrue(r3ChatModel.result.Model.ChatId == rChatModel.result.Model.ChatId);

            var r30ChatModel = await http.GetChat(rChatModel.result.Model.ChatId, userAdmin_Company_2);
            Assert.IsTrue(!r30ChatModel.result.IsSuccess);

            //Edit Item Test
            var r4ChatModel = await http.UpdateChat(rChatModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4ChatModel.result.IsSuccess);
            //Assert.IsTrue(r4ChatModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40ChatModel = await http.UpdateChat(rChatModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40ChatModel.result.IsSuccess);
            #endregion

            /*
            #region Chat Association with XYZZYX

            var tchatList0 = await http.GetChatsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tchatList0.result.IsSuccess);

            var tchatAssociationR = await http.AssociateChatWithXYZZYX(new ChatXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ChatId = rChatModel.result.Model.ChatId }, userAdmin_Company_1);
            Assert.IsTrue(tchatAssociationR.result.IsSuccess);

            tchatList0 = await http.GetChatsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tchatList0.result.IsSuccess);
            Assert.IsTrue(tchatList0.result.Model.Any(x => x.ChatId == rChatModel.result.Model.ChatId));

            var tchatUnAssociationR = await http.UnAssociateChatWithXYZZYX(new ChatXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ChatId = rChatModel.result.Model.ChatId }, userAdmin_Company_1);
            Assert.IsTrue(tchatUnAssociationR.result.IsSuccess);
            tchatList0 = await http.GetChatsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tchatList0.result.IsSuccess);
            Assert.IsTrue(!tchatList0.result.Model.Any(x => x.ChatId == rChatModel.result.Model.ChatId));

            #endregion
            */
        }
    }
}