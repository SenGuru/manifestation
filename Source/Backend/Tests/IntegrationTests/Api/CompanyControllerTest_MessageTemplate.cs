using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task MessageTemplateTest_IntegrationTest()
        {
            #region MessageTemplate
            //Tests for MessageTemplate
            //Create Item Test

            var aMessageTemplateModel = new MessageTemplateCreateModel();
            var rMessageTemplateModel = await http.CreateMessageTemplate(aMessageTemplateModel, userAdmin_Company_1);
            Assert.IsTrue(rMessageTemplateModel.result.IsSuccess);

            aMessageTemplateModel = new MessageTemplateCreateModel();
            var r0MessageTemplateModel = await http.CreateMessageTemplate(aMessageTemplateModel, userAdmin_Company_1);
            Assert.IsTrue(!r0MessageTemplateModel.result.IsSuccess);

            aMessageTemplateModel = new MessageTemplateCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00MessageTemplateModel = await http.CreateMessageTemplate(aMessageTemplateModel, userAdmin_Company_1);
            Assert.IsTrue(r00MessageTemplateModel.result.IsSuccess);

            aMessageTemplateModel = new MessageTemplateCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01MessageTemplateModel = await http.CreateMessageTemplate(aMessageTemplateModel, userAdmin_Company_2);
            Assert.IsTrue(!r01MessageTemplateModel.result.IsSuccess);


            aMessageTemplateModel = new MessageTemplateCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01MessageTemplateModel = await http.CreateMessageTemplate(aMessageTemplateModel, userAdmin_Company_2);
            Assert.IsTrue(r01MessageTemplateModel.result.IsSuccess);

            //Get List Test
            var r2MessageTemplateModel = await http.GetMessageTemplates(new MessageTemplateSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2MessageTemplateModel.result.IsSuccess);
            Assert.IsTrue(r2MessageTemplateModel.result.Model.Any(x => x.MessageTemplateId == rMessageTemplateModel.result.Model.MessageTemplateId));

            var r20MessageTemplateModel = await http.GetMessageTemplates(new MessageTemplateSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20MessageTemplateModel.result.Model.Any(x => x.MessageTemplateId == rMessageTemplateModel.result.Model.MessageTemplateId));

            //Get Item Test
            var r3MessageTemplateModel = await http.GetMessageTemplate(rMessageTemplateModel.result.Model.MessageTemplateId, userAdmin_Company_1);
            Assert.IsTrue(r3MessageTemplateModel.result.IsSuccess);
            Assert.IsTrue(r3MessageTemplateModel.result.Model.MessageTemplateId == rMessageTemplateModel.result.Model.MessageTemplateId);

            var r30MessageTemplateModel = await http.GetMessageTemplate(rMessageTemplateModel.result.Model.MessageTemplateId, userAdmin_Company_2);
            Assert.IsTrue(!r30MessageTemplateModel.result.IsSuccess);

            //Edit Item Test
            var r4MessageTemplateModel = await http.UpdateMessageTemplate(rMessageTemplateModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4MessageTemplateModel.result.IsSuccess);
            //Assert.IsTrue(r4MessageTemplateModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40MessageTemplateModel = await http.UpdateMessageTemplate(rMessageTemplateModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40MessageTemplateModel.result.IsSuccess);
            #endregion

            /*
            #region MessageTemplate Association with XYZZYX

            var tmessageTemplateList0 = await http.GetMessageTemplatesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tmessageTemplateList0.result.IsSuccess);

            var tmessageTemplateAssociationR = await http.AssociateMessageTemplateWithXYZZYX(new MessageTemplateXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, MessageTemplateId = rMessageTemplateModel.result.Model.MessageTemplateId }, userAdmin_Company_1);
            Assert.IsTrue(tmessageTemplateAssociationR.result.IsSuccess);

            tmessageTemplateList0 = await http.GetMessageTemplatesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tmessageTemplateList0.result.IsSuccess);
            Assert.IsTrue(tmessageTemplateList0.result.Model.Any(x => x.MessageTemplateId == rMessageTemplateModel.result.Model.MessageTemplateId));

            var tmessageTemplateUnAssociationR = await http.UnAssociateMessageTemplateWithXYZZYX(new MessageTemplateXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, MessageTemplateId = rMessageTemplateModel.result.Model.MessageTemplateId }, userAdmin_Company_1);
            Assert.IsTrue(tmessageTemplateUnAssociationR.result.IsSuccess);
            tmessageTemplateList0 = await http.GetMessageTemplatesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tmessageTemplateList0.result.IsSuccess);
            Assert.IsTrue(!tmessageTemplateList0.result.Model.Any(x => x.MessageTemplateId == rMessageTemplateModel.result.Model.MessageTemplateId));

            #endregion
            */
        }
    }
}