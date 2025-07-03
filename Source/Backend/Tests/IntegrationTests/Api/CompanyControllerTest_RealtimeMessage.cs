using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task RealtimeMessageTest_IntegrationTest()
        {
            #region RealtimeMessage
            //Tests for RealtimeMessage
            //Create Item Test

            var aRealtimeMessageModel = new RealtimeMessageCreateModel();
            var rRealtimeMessageModel = await http.CreateRealtimeMessage(aRealtimeMessageModel, userAdmin_Company_1);
            Assert.IsTrue(rRealtimeMessageModel.result.IsSuccess);

            aRealtimeMessageModel = new RealtimeMessageCreateModel();
            var r0RealtimeMessageModel = await http.CreateRealtimeMessage(aRealtimeMessageModel, userAdmin_Company_1);
            Assert.IsTrue(!r0RealtimeMessageModel.result.IsSuccess);

            aRealtimeMessageModel = new RealtimeMessageCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00RealtimeMessageModel = await http.CreateRealtimeMessage(aRealtimeMessageModel, userAdmin_Company_1);
            Assert.IsTrue(r00RealtimeMessageModel.result.IsSuccess);

            aRealtimeMessageModel = new RealtimeMessageCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01RealtimeMessageModel = await http.CreateRealtimeMessage(aRealtimeMessageModel, userAdmin_Company_2);
            Assert.IsTrue(!r01RealtimeMessageModel.result.IsSuccess);


            aRealtimeMessageModel = new RealtimeMessageCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01RealtimeMessageModel = await http.CreateRealtimeMessage(aRealtimeMessageModel, userAdmin_Company_2);
            Assert.IsTrue(r01RealtimeMessageModel.result.IsSuccess);

            //Get List Test
            var r2RealtimeMessageModel = await http.GetRealtimeMessages(new RealtimeMessageSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2RealtimeMessageModel.result.IsSuccess);
            Assert.IsTrue(r2RealtimeMessageModel.result.Model.Any(x => x.RealtimeMessageId == rRealtimeMessageModel.result.Model.RealtimeMessageId));

            var r20RealtimeMessageModel = await http.GetRealtimeMessages(new RealtimeMessageSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20RealtimeMessageModel.result.Model.Any(x => x.RealtimeMessageId == rRealtimeMessageModel.result.Model.RealtimeMessageId));

            //Get Item Test
            var r3RealtimeMessageModel = await http.GetRealtimeMessage(rRealtimeMessageModel.result.Model.RealtimeMessageId, userAdmin_Company_1);
            Assert.IsTrue(r3RealtimeMessageModel.result.IsSuccess);
            Assert.IsTrue(r3RealtimeMessageModel.result.Model.RealtimeMessageId == rRealtimeMessageModel.result.Model.RealtimeMessageId);

            var r30RealtimeMessageModel = await http.GetRealtimeMessage(rRealtimeMessageModel.result.Model.RealtimeMessageId, userAdmin_Company_2);
            Assert.IsTrue(!r30RealtimeMessageModel.result.IsSuccess);

            //Edit Item Test
            var r4RealtimeMessageModel = await http.UpdateRealtimeMessage(rRealtimeMessageModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4RealtimeMessageModel.result.IsSuccess);
            //Assert.IsTrue(r4RealtimeMessageModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40RealtimeMessageModel = await http.UpdateRealtimeMessage(rRealtimeMessageModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40RealtimeMessageModel.result.IsSuccess);
            #endregion

            /*
            #region RealtimeMessage Association with XYZZYX

            var trealtimeMessageList0 = await http.GetRealtimeMessagesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(trealtimeMessageList0.result.IsSuccess);

            var trealtimeMessageAssociationR = await http.AssociateRealtimeMessageWithXYZZYX(new RealtimeMessageXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, RealtimeMessageId = rRealtimeMessageModel.result.Model.RealtimeMessageId }, userAdmin_Company_1);
            Assert.IsTrue(trealtimeMessageAssociationR.result.IsSuccess);

            trealtimeMessageList0 = await http.GetRealtimeMessagesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(trealtimeMessageList0.result.IsSuccess);
            Assert.IsTrue(trealtimeMessageList0.result.Model.Any(x => x.RealtimeMessageId == rRealtimeMessageModel.result.Model.RealtimeMessageId));

            var trealtimeMessageUnAssociationR = await http.UnAssociateRealtimeMessageWithXYZZYX(new RealtimeMessageXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, RealtimeMessageId = rRealtimeMessageModel.result.Model.RealtimeMessageId }, userAdmin_Company_1);
            Assert.IsTrue(trealtimeMessageUnAssociationR.result.IsSuccess);
            trealtimeMessageList0 = await http.GetRealtimeMessagesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(trealtimeMessageList0.result.IsSuccess);
            Assert.IsTrue(!trealtimeMessageList0.result.Model.Any(x => x.RealtimeMessageId == rRealtimeMessageModel.result.Model.RealtimeMessageId));

            #endregion
            */
        }
    }
}