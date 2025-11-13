using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task AuditOperationDetailTest_IntegrationTest()
        {
            #region AuditOperationDetail
            //Tests for AuditOperationDetail
            //Create Item Test

            var aAuditOperationDetailModel = new AuditOperationDetailCreateModel();
            var rAuditOperationDetailModel = await http.CreateAuditOperationDetail(aAuditOperationDetailModel, userAdmin_Company_1);
            Assert.IsTrue(rAuditOperationDetailModel.result.IsSuccess);

            aAuditOperationDetailModel = new AuditOperationDetailCreateModel();
            var r0AuditOperationDetailModel = await http.CreateAuditOperationDetail(aAuditOperationDetailModel, userAdmin_Company_1);
            Assert.IsTrue(!r0AuditOperationDetailModel.result.IsSuccess);

            aAuditOperationDetailModel = new AuditOperationDetailCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00AuditOperationDetailModel = await http.CreateAuditOperationDetail(aAuditOperationDetailModel, userAdmin_Company_1);
            Assert.IsTrue(r00AuditOperationDetailModel.result.IsSuccess);

            aAuditOperationDetailModel = new AuditOperationDetailCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01AuditOperationDetailModel = await http.CreateAuditOperationDetail(aAuditOperationDetailModel, userAdmin_Company_2);
            Assert.IsTrue(!r01AuditOperationDetailModel.result.IsSuccess);


            aAuditOperationDetailModel = new AuditOperationDetailCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01AuditOperationDetailModel = await http.CreateAuditOperationDetail(aAuditOperationDetailModel, userAdmin_Company_2);
            Assert.IsTrue(r01AuditOperationDetailModel.result.IsSuccess);

            //Get List Test
            var r2AuditOperationDetailModel = await http.GetAuditOperationDetails(new AuditOperationDetailSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2AuditOperationDetailModel.result.IsSuccess);
            Assert.IsTrue(r2AuditOperationDetailModel.result.Model.Any(x => x.AuditOperationDetailId == rAuditOperationDetailModel.result.Model.AuditOperationDetailId));

            var r20AuditOperationDetailModel = await http.GetAuditOperationDetails(new AuditOperationDetailSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20AuditOperationDetailModel.result.Model.Any(x => x.AuditOperationDetailId == rAuditOperationDetailModel.result.Model.AuditOperationDetailId));

            //Get Item Test
            var r3AuditOperationDetailModel = await http.GetAuditOperationDetail(rAuditOperationDetailModel.result.Model.AuditOperationDetailId, userAdmin_Company_1);
            Assert.IsTrue(r3AuditOperationDetailModel.result.IsSuccess);
            Assert.IsTrue(r3AuditOperationDetailModel.result.Model.AuditOperationDetailId == rAuditOperationDetailModel.result.Model.AuditOperationDetailId);

            var r30AuditOperationDetailModel = await http.GetAuditOperationDetail(rAuditOperationDetailModel.result.Model.AuditOperationDetailId, userAdmin_Company_2);
            Assert.IsTrue(!r30AuditOperationDetailModel.result.IsSuccess);

            //Edit Item Test
            var r4AuditOperationDetailModel = await http.UpdateAuditOperationDetail(rAuditOperationDetailModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4AuditOperationDetailModel.result.IsSuccess);
            //Assert.IsTrue(r4AuditOperationDetailModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40AuditOperationDetailModel = await http.UpdateAuditOperationDetail(rAuditOperationDetailModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40AuditOperationDetailModel.result.IsSuccess);
            #endregion

            /*
            #region AuditOperationDetail Association with XYZZYX

            var tauditOperationDetailList0 = await http.GetAuditOperationDetailsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tauditOperationDetailList0.result.IsSuccess);

            var tauditOperationDetailAssociationR = await http.AssociateAuditOperationDetailWithXYZZYX(new AuditOperationDetailXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, AuditOperationDetailId = rAuditOperationDetailModel.result.Model.AuditOperationDetailId }, userAdmin_Company_1);
            Assert.IsTrue(tauditOperationDetailAssociationR.result.IsSuccess);

            tauditOperationDetailList0 = await http.GetAuditOperationDetailsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tauditOperationDetailList0.result.IsSuccess);
            Assert.IsTrue(tauditOperationDetailList0.result.Model.Any(x => x.AuditOperationDetailId == rAuditOperationDetailModel.result.Model.AuditOperationDetailId));

            var tauditOperationDetailUnAssociationR = await http.UnAssociateAuditOperationDetailWithXYZZYX(new AuditOperationDetailXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, AuditOperationDetailId = rAuditOperationDetailModel.result.Model.AuditOperationDetailId }, userAdmin_Company_1);
            Assert.IsTrue(tauditOperationDetailUnAssociationR.result.IsSuccess);
            tauditOperationDetailList0 = await http.GetAuditOperationDetailsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tauditOperationDetailList0.result.IsSuccess);
            Assert.IsTrue(!tauditOperationDetailList0.result.Model.Any(x => x.AuditOperationDetailId == rAuditOperationDetailModel.result.Model.AuditOperationDetailId));

            #endregion
            */
        }
    }
}