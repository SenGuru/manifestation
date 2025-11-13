using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task ChatMessageStatusTest_IntegrationTest()
        {
            #region ChatMessageStatus
            //Tests for ChatMessageStatus
            //Create Item Test

            var aChatMessageStatusModel = new ChatMessageStatusCreateModel();
            var rChatMessageStatusModel = await http.CreateChatMessageStatus(aChatMessageStatusModel, userAdmin_Company_1);
            Assert.IsTrue(rChatMessageStatusModel.result.IsSuccess);

            aChatMessageStatusModel = new ChatMessageStatusCreateModel();
            var r0ChatMessageStatusModel = await http.CreateChatMessageStatus(aChatMessageStatusModel, userAdmin_Company_1);
            Assert.IsTrue(!r0ChatMessageStatusModel.result.IsSuccess);

            aChatMessageStatusModel = new ChatMessageStatusCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00ChatMessageStatusModel = await http.CreateChatMessageStatus(aChatMessageStatusModel, userAdmin_Company_1);
            Assert.IsTrue(r00ChatMessageStatusModel.result.IsSuccess);

            aChatMessageStatusModel = new ChatMessageStatusCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01ChatMessageStatusModel = await http.CreateChatMessageStatus(aChatMessageStatusModel, userAdmin_Company_2);
            Assert.IsTrue(!r01ChatMessageStatusModel.result.IsSuccess);


            aChatMessageStatusModel = new ChatMessageStatusCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01ChatMessageStatusModel = await http.CreateChatMessageStatus(aChatMessageStatusModel, userAdmin_Company_2);
            Assert.IsTrue(r01ChatMessageStatusModel.result.IsSuccess);

            //Get List Test
            var r2ChatMessageStatusModel = await http.GetChatMessageStatuss(new ChatMessageStatusSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2ChatMessageStatusModel.result.IsSuccess);
            Assert.IsTrue(r2ChatMessageStatusModel.result.Model.Any(x => x.ChatMessageStatusId == rChatMessageStatusModel.result.Model.ChatMessageStatusId));

            var r20ChatMessageStatusModel = await http.GetChatMessageStatuss(new ChatMessageStatusSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20ChatMessageStatusModel.result.Model.Any(x => x.ChatMessageStatusId == rChatMessageStatusModel.result.Model.ChatMessageStatusId));

            //Get Item Test
            var r3ChatMessageStatusModel = await http.GetChatMessageStatus(rChatMessageStatusModel.result.Model.ChatMessageStatusId, userAdmin_Company_1);
            Assert.IsTrue(r3ChatMessageStatusModel.result.IsSuccess);
            Assert.IsTrue(r3ChatMessageStatusModel.result.Model.ChatMessageStatusId == rChatMessageStatusModel.result.Model.ChatMessageStatusId);

            var r30ChatMessageStatusModel = await http.GetChatMessageStatus(rChatMessageStatusModel.result.Model.ChatMessageStatusId, userAdmin_Company_2);
            Assert.IsTrue(!r30ChatMessageStatusModel.result.IsSuccess);

            //Edit Item Test
            var r4ChatMessageStatusModel = await http.UpdateChatMessageStatus(rChatMessageStatusModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4ChatMessageStatusModel.result.IsSuccess);
            //Assert.IsTrue(r4ChatMessageStatusModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40ChatMessageStatusModel = await http.UpdateChatMessageStatus(rChatMessageStatusModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40ChatMessageStatusModel.result.IsSuccess);
            #endregion

            /*
            #region ChatMessageStatus Association with XYZZYX

            var tchatMessageStatusList0 = await http.GetChatMessageStatussByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tchatMessageStatusList0.result.IsSuccess);

            var tchatMessageStatusAssociationR = await http.AssociateChatMessageStatusWithXYZZYX(new ChatMessageStatusXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ChatMessageStatusId = rChatMessageStatusModel.result.Model.ChatMessageStatusId }, userAdmin_Company_1);
            Assert.IsTrue(tchatMessageStatusAssociationR.result.IsSuccess);

            tchatMessageStatusList0 = await http.GetChatMessageStatussByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tchatMessageStatusList0.result.IsSuccess);
            Assert.IsTrue(tchatMessageStatusList0.result.Model.Any(x => x.ChatMessageStatusId == rChatMessageStatusModel.result.Model.ChatMessageStatusId));

            var tchatMessageStatusUnAssociationR = await http.UnAssociateChatMessageStatusWithXYZZYX(new ChatMessageStatusXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ChatMessageStatusId = rChatMessageStatusModel.result.Model.ChatMessageStatusId }, userAdmin_Company_1);
            Assert.IsTrue(tchatMessageStatusUnAssociationR.result.IsSuccess);
            tchatMessageStatusList0 = await http.GetChatMessageStatussByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tchatMessageStatusList0.result.IsSuccess);
            Assert.IsTrue(!tchatMessageStatusList0.result.Model.Any(x => x.ChatMessageStatusId == rChatMessageStatusModel.result.Model.ChatMessageStatusId));

            #endregion
            */
        }
    }
}