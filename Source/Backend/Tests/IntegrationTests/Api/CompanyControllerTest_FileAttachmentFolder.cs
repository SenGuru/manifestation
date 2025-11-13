using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task FileAttachmentFolderTest_IntegrationTest()
        {
            #region FileAttachmentFolder
            //Tests for FileAttachmentFolder
            //Create Item Test

            var aFileAttachmentFolderModel = new FileAttachmentFolderCreateModel();
            var rFileAttachmentFolderModel = await http.CreateFileAttachmentFolder(aFileAttachmentFolderModel, userAdmin_Company_1);
            Assert.IsTrue(rFileAttachmentFolderModel.result.IsSuccess);

            aFileAttachmentFolderModel = new FileAttachmentFolderCreateModel();
            var r0FileAttachmentFolderModel = await http.CreateFileAttachmentFolder(aFileAttachmentFolderModel, userAdmin_Company_1);
            Assert.IsTrue(!r0FileAttachmentFolderModel.result.IsSuccess);

            aFileAttachmentFolderModel = new FileAttachmentFolderCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00FileAttachmentFolderModel = await http.CreateFileAttachmentFolder(aFileAttachmentFolderModel, userAdmin_Company_1);
            Assert.IsTrue(r00FileAttachmentFolderModel.result.IsSuccess);

            aFileAttachmentFolderModel = new FileAttachmentFolderCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01FileAttachmentFolderModel = await http.CreateFileAttachmentFolder(aFileAttachmentFolderModel, userAdmin_Company_2);
            Assert.IsTrue(!r01FileAttachmentFolderModel.result.IsSuccess);


            aFileAttachmentFolderModel = new FileAttachmentFolderCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01FileAttachmentFolderModel = await http.CreateFileAttachmentFolder(aFileAttachmentFolderModel, userAdmin_Company_2);
            Assert.IsTrue(r01FileAttachmentFolderModel.result.IsSuccess);

            //Get List Test
            var r2FileAttachmentFolderModel = await http.GetFileAttachmentFolders(new FileAttachmentFolderSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2FileAttachmentFolderModel.result.IsSuccess);
            Assert.IsTrue(r2FileAttachmentFolderModel.result.Model.Any(x => x.FileAttachmentFolderId == rFileAttachmentFolderModel.result.Model.FileAttachmentFolderId));

            var r20FileAttachmentFolderModel = await http.GetFileAttachmentFolders(new FileAttachmentFolderSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20FileAttachmentFolderModel.result.Model.Any(x => x.FileAttachmentFolderId == rFileAttachmentFolderModel.result.Model.FileAttachmentFolderId));

            //Get Item Test
            var r3FileAttachmentFolderModel = await http.GetFileAttachmentFolder(rFileAttachmentFolderModel.result.Model.FileAttachmentFolderId, userAdmin_Company_1);
            Assert.IsTrue(r3FileAttachmentFolderModel.result.IsSuccess);
            Assert.IsTrue(r3FileAttachmentFolderModel.result.Model.FileAttachmentFolderId == rFileAttachmentFolderModel.result.Model.FileAttachmentFolderId);

            var r30FileAttachmentFolderModel = await http.GetFileAttachmentFolder(rFileAttachmentFolderModel.result.Model.FileAttachmentFolderId, userAdmin_Company_2);
            Assert.IsTrue(!r30FileAttachmentFolderModel.result.IsSuccess);

            //Edit Item Test
            var r4FileAttachmentFolderModel = await http.UpdateFileAttachmentFolder(rFileAttachmentFolderModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4FileAttachmentFolderModel.result.IsSuccess);
            //Assert.IsTrue(r4FileAttachmentFolderModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40FileAttachmentFolderModel = await http.UpdateFileAttachmentFolder(rFileAttachmentFolderModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40FileAttachmentFolderModel.result.IsSuccess);
            #endregion

            /*
            #region FileAttachmentFolder Association with XYZZYX

            var tfileAttachmentFolderList0 = await http.GetFileAttachmentFoldersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tfileAttachmentFolderList0.result.IsSuccess);

            var tfileAttachmentFolderAssociationR = await http.AssociateFileAttachmentFolderWithXYZZYX(new FileAttachmentFolderXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, FileAttachmentFolderId = rFileAttachmentFolderModel.result.Model.FileAttachmentFolderId }, userAdmin_Company_1);
            Assert.IsTrue(tfileAttachmentFolderAssociationR.result.IsSuccess);

            tfileAttachmentFolderList0 = await http.GetFileAttachmentFoldersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tfileAttachmentFolderList0.result.IsSuccess);
            Assert.IsTrue(tfileAttachmentFolderList0.result.Model.Any(x => x.FileAttachmentFolderId == rFileAttachmentFolderModel.result.Model.FileAttachmentFolderId));

            var tfileAttachmentFolderUnAssociationR = await http.UnAssociateFileAttachmentFolderWithXYZZYX(new FileAttachmentFolderXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, FileAttachmentFolderId = rFileAttachmentFolderModel.result.Model.FileAttachmentFolderId }, userAdmin_Company_1);
            Assert.IsTrue(tfileAttachmentFolderUnAssociationR.result.IsSuccess);
            tfileAttachmentFolderList0 = await http.GetFileAttachmentFoldersByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tfileAttachmentFolderList0.result.IsSuccess);
            Assert.IsTrue(!tfileAttachmentFolderList0.result.Model.Any(x => x.FileAttachmentFolderId == rFileAttachmentFolderModel.result.Model.FileAttachmentFolderId));

            #endregion
            */
        }
    }
}