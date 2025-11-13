using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task AuditOperationTest_IntegrationTest()
        {
            #region AuditOperation
            //Tests for AuditOperation
            //Create Item Test

            var aAuditOperationModel = new AuditOperationCreateModel();
            var rAuditOperationModel = await http.CreateAuditOperation(aAuditOperationModel, userAdmin_Company_1);
            Assert.IsTrue(rAuditOperationModel.result.IsSuccess);

            aAuditOperationModel = new AuditOperationCreateModel();
            var r0AuditOperationModel = await http.CreateAuditOperation(aAuditOperationModel, userAdmin_Company_1);
            Assert.IsTrue(!r0AuditOperationModel.result.IsSuccess);

            aAuditOperationModel = new AuditOperationCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00AuditOperationModel = await http.CreateAuditOperation(aAuditOperationModel, userAdmin_Company_1);
            Assert.IsTrue(r00AuditOperationModel.result.IsSuccess);

            aAuditOperationModel = new AuditOperationCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01AuditOperationModel = await http.CreateAuditOperation(aAuditOperationModel, userAdmin_Company_2);
            Assert.IsTrue(!r01AuditOperationModel.result.IsSuccess);


            aAuditOperationModel = new AuditOperationCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01AuditOperationModel = await http.CreateAuditOperation(aAuditOperationModel, userAdmin_Company_2);
            Assert.IsTrue(r01AuditOperationModel.result.IsSuccess);

            //Get List Test
            var r2AuditOperationModel = await http.GetAuditOperations(new AuditOperationSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2AuditOperationModel.result.IsSuccess);
            Assert.IsTrue(r2AuditOperationModel.result.Model.Any(x => x.AuditOperationId == rAuditOperationModel.result.Model.AuditOperationId));

            var r20AuditOperationModel = await http.GetAuditOperations(new AuditOperationSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20AuditOperationModel.result.Model.Any(x => x.AuditOperationId == rAuditOperationModel.result.Model.AuditOperationId));

            //Get Item Test
            var r3AuditOperationModel = await http.GetAuditOperation(rAuditOperationModel.result.Model.AuditOperationId, userAdmin_Company_1);
            Assert.IsTrue(r3AuditOperationModel.result.IsSuccess);
            Assert.IsTrue(r3AuditOperationModel.result.Model.AuditOperationId == rAuditOperationModel.result.Model.AuditOperationId);

            var r30AuditOperationModel = await http.GetAuditOperation(rAuditOperationModel.result.Model.AuditOperationId, userAdmin_Company_2);
            Assert.IsTrue(!r30AuditOperationModel.result.IsSuccess);

            //Edit Item Test
            var r4AuditOperationModel = await http.UpdateAuditOperation(rAuditOperationModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4AuditOperationModel.result.IsSuccess);
            //Assert.IsTrue(r4AuditOperationModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40AuditOperationModel = await http.UpdateAuditOperation(rAuditOperationModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40AuditOperationModel.result.IsSuccess);
            #endregion

            /*
            #region AuditOperation Association with XYZZYX

            var tauditOperationList0 = await http.GetAuditOperationsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tauditOperationList0.result.IsSuccess);

            var tauditOperationAssociationR = await http.AssociateAuditOperationWithXYZZYX(new AuditOperationXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, AuditOperationId = rAuditOperationModel.result.Model.AuditOperationId }, userAdmin_Company_1);
            Assert.IsTrue(tauditOperationAssociationR.result.IsSuccess);

            tauditOperationList0 = await http.GetAuditOperationsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tauditOperationList0.result.IsSuccess);
            Assert.IsTrue(tauditOperationList0.result.Model.Any(x => x.AuditOperationId == rAuditOperationModel.result.Model.AuditOperationId));

            var tauditOperationUnAssociationR = await http.UnAssociateAuditOperationWithXYZZYX(new AuditOperationXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, AuditOperationId = rAuditOperationModel.result.Model.AuditOperationId }, userAdmin_Company_1);
            Assert.IsTrue(tauditOperationUnAssociationR.result.IsSuccess);
            tauditOperationList0 = await http.GetAuditOperationsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tauditOperationList0.result.IsSuccess);
            Assert.IsTrue(!tauditOperationList0.result.Model.Any(x => x.AuditOperationId == rAuditOperationModel.result.Model.AuditOperationId));

            #endregion
            */
        }
    }
}