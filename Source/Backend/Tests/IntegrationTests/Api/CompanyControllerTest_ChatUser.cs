using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task ChatUserTest_IntegrationTest()
        {
            #region ChatUser
            //Tests for ChatUser
            //Create Item Test

            var aChatUserModel = new ChatUserCreateModel();
            var rChatUserModel = await http.CreateChatUser(aChatUserModel, userAdmin_Company_1);
            Assert.IsTrue(rChatUserModel.result.IsSuccess);

            aChatUserModel = new ChatUserCreateModel();
            var r0ChatUserModel = await http.CreateChatUser(aChatUserModel, userAdmin_Company_1);
            Assert.IsTrue(!r0ChatUserModel.result.IsSuccess);

            aChatUserModel = new ChatUserCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00ChatUserModel = await http.CreateChatUser(aChatUserModel, userAdmin_Company_1);
            Assert.IsTrue(r00ChatUserModel.result.IsSuccess);

            aChatUserModel = new ChatUserCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01ChatUserModel = await http.CreateChatUser(aChatUserModel, userAdmin_Company_2);
            Assert.IsTrue(!r01ChatUserModel.result.IsSuccess);


            aChatUserModel = new ChatUserCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01ChatUserModel = await http.CreateChatUser(aChatUserModel, userAdmin_Company_2);
            Assert.IsTrue(r01ChatUserModel.result.IsSuccess);

            //Get List Test
            var r2ChatUserModel = await http.GetChatUsers(new ChatUserSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2ChatUserModel.result.IsSuccess);
            Assert.IsTrue(r2ChatUserModel.result.Model.Any(x => x.ChatUserId == rChatUserModel.result.Model.ChatUserId));

            var r20ChatUserModel = await http.GetChatUsers(new ChatUserSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20ChatUserModel.result.Model.Any(x => x.ChatUserId == rChatUserModel.result.Model.ChatUserId));

            //Get Item Test
            var r3ChatUserModel = await http.GetChatUser(rChatUserModel.result.Model.ChatUserId, userAdmin_Company_1);
            Assert.IsTrue(r3ChatUserModel.result.IsSuccess);
            Assert.IsTrue(r3ChatUserModel.result.Model.ChatUserId == rChatUserModel.result.Model.ChatUserId);

            var r30ChatUserModel = await http.GetChatUser(rChatUserModel.result.Model.ChatUserId, userAdmin_Company_2);
            Assert.IsTrue(!r30ChatUserModel.result.IsSuccess);

            //Edit Item Test
            var r4ChatUserModel = await http.UpdateChatUser(rChatUserModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4ChatUserModel.result.IsSuccess);
            //Assert.IsTrue(r4ChatUserModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40ChatUserModel = await http.UpdateChatUser(rChatUserModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40ChatUserModel.result.IsSuccess);
            #endregion

            /*
            #region ChatUser Association with XYZZYX

            var tchatUserList0 = await http.GetChatUsersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tchatUserList0.result.IsSuccess);

            var tchatUserAssociationR = await http.AssociateChatUserWithXYZZYX(new ChatUserXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ChatUserId = rChatUserModel.result.Model.ChatUserId }, userAdmin_Company_1);
            Assert.IsTrue(tchatUserAssociationR.result.IsSuccess);

            tchatUserList0 = await http.GetChatUsersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tchatUserList0.result.IsSuccess);
            Assert.IsTrue(tchatUserList0.result.Model.Any(x => x.ChatUserId == rChatUserModel.result.Model.ChatUserId));

            var tchatUserUnAssociationR = await http.UnAssociateChatUserWithXYZZYX(new ChatUserXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ChatUserId = rChatUserModel.result.Model.ChatUserId }, userAdmin_Company_1);
            Assert.IsTrue(tchatUserUnAssociationR.result.IsSuccess);
            tchatUserList0 = await http.GetChatUsersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tchatUserList0.result.IsSuccess);
            Assert.IsTrue(!tchatUserList0.result.Model.Any(x => x.ChatUserId == rChatUserModel.result.Model.ChatUserId));

            #endregion
            */
        }
    }
}