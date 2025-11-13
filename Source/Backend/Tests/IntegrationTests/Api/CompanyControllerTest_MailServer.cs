using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task MailServerTest_IntegrationTest()
        {
            #region MailServer
            //Tests for MailServer
            //Create Item Test

            var aMailServerModel = new MailServerCreateModel();
            var rMailServerModel = await http.CreateMailServer(aMailServerModel, userAdmin_Company_1);
            Assert.IsTrue(rMailServerModel.result.IsSuccess);

            aMailServerModel = new MailServerCreateModel();
            var r0MailServerModel = await http.CreateMailServer(aMailServerModel, userAdmin_Company_1);
            Assert.IsTrue(!r0MailServerModel.result.IsSuccess);

            aMailServerModel = new MailServerCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00MailServerModel = await http.CreateMailServer(aMailServerModel, userAdmin_Company_1);
            Assert.IsTrue(r00MailServerModel.result.IsSuccess);

            aMailServerModel = new MailServerCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01MailServerModel = await http.CreateMailServer(aMailServerModel, userAdmin_Company_2);
            Assert.IsTrue(!r01MailServerModel.result.IsSuccess);


            aMailServerModel = new MailServerCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01MailServerModel = await http.CreateMailServer(aMailServerModel, userAdmin_Company_2);
            Assert.IsTrue(r01MailServerModel.result.IsSuccess);

            //Get List Test
            var r2MailServerModel = await http.GetMailServers(new MailServerSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2MailServerModel.result.IsSuccess);
            Assert.IsTrue(r2MailServerModel.result.Model.Any(x => x.MailServerId == rMailServerModel.result.Model.MailServerId));

            var r20MailServerModel = await http.GetMailServers(new MailServerSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20MailServerModel.result.Model.Any(x => x.MailServerId == rMailServerModel.result.Model.MailServerId));

            //Get Item Test
            var r3MailServerModel = await http.GetMailServer(rMailServerModel.result.Model.MailServerId, userAdmin_Company_1);
            Assert.IsTrue(r3MailServerModel.result.IsSuccess);
            Assert.IsTrue(r3MailServerModel.result.Model.MailServerId == rMailServerModel.result.Model.MailServerId);

            var r30MailServerModel = await http.GetMailServer(rMailServerModel.result.Model.MailServerId, userAdmin_Company_2);
            Assert.IsTrue(!r30MailServerModel.result.IsSuccess);

            //Edit Item Test
            var r4MailServerModel = await http.UpdateMailServer(rMailServerModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4MailServerModel.result.IsSuccess);
            //Assert.IsTrue(r4MailServerModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40MailServerModel = await http.UpdateMailServer(rMailServerModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40MailServerModel.result.IsSuccess);
            #endregion

            /*
            #region MailServer Association with XYZZYX

            var tmailServerList0 = await http.GetMailServersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tmailServerList0.result.IsSuccess);

            var tmailServerAssociationR = await http.AssociateMailServerWithXYZZYX(new MailServerXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, MailServerId = rMailServerModel.result.Model.MailServerId }, userAdmin_Company_1);
            Assert.IsTrue(tmailServerAssociationR.result.IsSuccess);

            tmailServerList0 = await http.GetMailServersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tmailServerList0.result.IsSuccess);
            Assert.IsTrue(tmailServerList0.result.Model.Any(x => x.MailServerId == rMailServerModel.result.Model.MailServerId));

            var tmailServerUnAssociationR = await http.UnAssociateMailServerWithXYZZYX(new MailServerXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, MailServerId = rMailServerModel.result.Model.MailServerId }, userAdmin_Company_1);
            Assert.IsTrue(tmailServerUnAssociationR.result.IsSuccess);
            tmailServerList0 = await http.GetMailServersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tmailServerList0.result.IsSuccess);
            Assert.IsTrue(!tmailServerList0.result.Model.Any(x => x.MailServerId == rMailServerModel.result.Model.MailServerId));

            #endregion
            */
        }
    }
}