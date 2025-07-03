using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task FileAttachmentTest_IntegrationTest()
        {
            #region FileAttachment
            //Tests for FileAttachment
            //Create Item Test

            var aFileAttachmentModel = new FileAttachmentCreateWithFileFormModel();
            var rFileAttachmentModel = await http.CreateFileAttachment(aFileAttachmentModel, userAdmin_Company_1);
            Assert.IsTrue(rFileAttachmentModel.result.IsSuccess);

            aFileAttachmentModel = new FileAttachmentCreateWithFileFormModel();
            var r0FileAttachmentModel = await http.CreateFileAttachment(aFileAttachmentModel, userAdmin_Company_1);
            Assert.IsTrue(!r0FileAttachmentModel.result.IsSuccess);

            aFileAttachmentModel = new FileAttachmentCreateWithFileFormModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00FileAttachmentModel = await http.CreateFileAttachment(aFileAttachmentModel, userAdmin_Company_1);
            Assert.IsTrue(r00FileAttachmentModel.result.IsSuccess);

            aFileAttachmentModel = new FileAttachmentCreateWithFileFormModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01FileAttachmentModel = await http.CreateFileAttachment(aFileAttachmentModel, userAdmin_Company_2);
            Assert.IsTrue(!r01FileAttachmentModel.result.IsSuccess);


            aFileAttachmentModel = new FileAttachmentCreateWithFileFormModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01FileAttachmentModel = await http.CreateFileAttachment(aFileAttachmentModel, userAdmin_Company_2);
            Assert.IsTrue(r01FileAttachmentModel.result.IsSuccess);

            //Get List Test
            var r2FileAttachmentModel = await http.GetFileAttachments(new FileAttachmentSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2FileAttachmentModel.result.IsSuccess);
            Assert.IsTrue(r2FileAttachmentModel.result.Model.Any(x => x.FileAttachmentId == rFileAttachmentModel.result.Model.FileAttachmentId));

            var r20FileAttachmentModel = await http.GetFileAttachments(new FileAttachmentSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20FileAttachmentModel.result.Model.Any(x => x.FileAttachmentId == rFileAttachmentModel.result.Model.FileAttachmentId));

            //Get Item Test
            var r3FileAttachmentModel = await http.GetFileAttachment(rFileAttachmentModel.result.Model.FileAttachmentId, userAdmin_Company_1);
            Assert.IsTrue(r3FileAttachmentModel.result.IsSuccess);
            Assert.IsTrue(r3FileAttachmentModel.result.Model.FileAttachmentId == rFileAttachmentModel.result.Model.FileAttachmentId);

            var r30FileAttachmentModel = await http.GetFileAttachment(rFileAttachmentModel.result.Model.FileAttachmentId, userAdmin_Company_2);
            Assert.IsTrue(!r30FileAttachmentModel.result.IsSuccess);

            //Edit Item Test
            var r4FileAttachmentModel = await http.UpdateFileAttachment(rFileAttachmentModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4FileAttachmentModel.result.IsSuccess);
            //Assert.IsTrue(r4FileAttachmentModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40FileAttachmentModel = await http.UpdateFileAttachment(rFileAttachmentModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40FileAttachmentModel.result.IsSuccess);
            #endregion

            /*
            #region FileAttachment Association with XYZZYX

            var tfileAttachmentList0 = await http.GetFileAttachmentsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tfileAttachmentList0.result.IsSuccess);

            var tfileAttachmentAssociationR = await http.AssociateFileAttachmentWithXYZZYX(new FileAttachmentXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, FileAttachmentId = rFileAttachmentModel.result.Model.FileAttachmentId }, userAdmin_Company_1);
            Assert.IsTrue(tfileAttachmentAssociationR.result.IsSuccess);

            tfileAttachmentList0 = await http.GetFileAttachmentsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tfileAttachmentList0.result.IsSuccess);
            Assert.IsTrue(tfileAttachmentList0.result.Model.Any(x => x.FileAttachmentId == rFileAttachmentModel.result.Model.FileAttachmentId));

            var tfileAttachmentUnAssociationR = await http.UnAssociateFileAttachmentWithXYZZYX(new FileAttachmentXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, FileAttachmentId = rFileAttachmentModel.result.Model.FileAttachmentId }, userAdmin_Company_1);
            Assert.IsTrue(tfileAttachmentUnAssociationR.result.IsSuccess);
            tfileAttachmentList0 = await http.GetFileAttachmentsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tfileAttachmentList0.result.IsSuccess);
            Assert.IsTrue(!tfileAttachmentList0.result.Model.Any(x => x.FileAttachmentId == rFileAttachmentModel.result.Model.FileAttachmentId));

            #endregion
            */
        }
    }
}