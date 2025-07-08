using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task MeditationFileTest_IntegrationTest()
        {
            #region MeditationFile
            //Tests for MeditationFile
            //Create Item Test

            var aMeditationFileModel = new MeditationFileCreateModel();
            var rMeditationFileModel = await http.CreateMeditationFile(aMeditationFileModel, userAdmin_Company_1);
            Assert.IsTrue(rMeditationFileModel.result.IsSuccess);

            aMeditationFileModel = new MeditationFileCreateModel();
            var r0MeditationFileModel = await http.CreateMeditationFile(aMeditationFileModel, userAdmin_Company_1);
            Assert.IsTrue(!r0MeditationFileModel.result.IsSuccess);

            aMeditationFileModel = new MeditationFileCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00MeditationFileModel = await http.CreateMeditationFile(aMeditationFileModel, userAdmin_Company_1);
            Assert.IsTrue(r00MeditationFileModel.result.IsSuccess);

            aMeditationFileModel = new MeditationFileCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01MeditationFileModel = await http.CreateMeditationFile(aMeditationFileModel, userAdmin_Company_2);
            Assert.IsTrue(!r01MeditationFileModel.result.IsSuccess);


            aMeditationFileModel = new MeditationFileCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01MeditationFileModel = await http.CreateMeditationFile(aMeditationFileModel, userAdmin_Company_2);
            Assert.IsTrue(r01MeditationFileModel.result.IsSuccess);

            //Get List Test
            var r2MeditationFileModel = await http.GetMeditationFiles(new MeditationFileSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2MeditationFileModel.result.IsSuccess);
            Assert.IsTrue(r2MeditationFileModel.result.Model.Any(x => x.MeditationFileId == rMeditationFileModel.result.Model.MeditationFileId));

            var r20MeditationFileModel = await http.GetMeditationFiles(new MeditationFileSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20MeditationFileModel.result.Model.Any(x => x.MeditationFileId == rMeditationFileModel.result.Model.MeditationFileId));

            //Get Item Test
            var r3MeditationFileModel = await http.GetMeditationFile(rMeditationFileModel.result.Model.MeditationFileId, userAdmin_Company_1);
            Assert.IsTrue(r3MeditationFileModel.result.IsSuccess);
            Assert.IsTrue(r3MeditationFileModel.result.Model.MeditationFileId == rMeditationFileModel.result.Model.MeditationFileId);

            var r30MeditationFileModel = await http.GetMeditationFile(rMeditationFileModel.result.Model.MeditationFileId, userAdmin_Company_2);
            Assert.IsTrue(!r30MeditationFileModel.result.IsSuccess);

            //Edit Item Test
            var r4MeditationFileModel = await http.UpdateMeditationFile(rMeditationFileModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4MeditationFileModel.result.IsSuccess);
            //Assert.IsTrue(r4MeditationFileModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40MeditationFileModel = await http.UpdateMeditationFile(rMeditationFileModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40MeditationFileModel.result.IsSuccess);
            #endregion

            /*
            #region MeditationFile Association with XYZZYX

            var tmeditationFileList0 = await http.GetMeditationFilesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tmeditationFileList0.result.IsSuccess);

            var tmeditationFileAssociationR = await http.AssociateMeditationFileWithXYZZYX(new MeditationFileXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, MeditationFileId = rMeditationFileModel.result.Model.MeditationFileId }, userAdmin_Company_1);
            Assert.IsTrue(tmeditationFileAssociationR.result.IsSuccess);

            tmeditationFileList0 = await http.GetMeditationFilesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tmeditationFileList0.result.IsSuccess);
            Assert.IsTrue(tmeditationFileList0.result.Model.Any(x => x.MeditationFileId == rMeditationFileModel.result.Model.MeditationFileId));

            var tmeditationFileUnAssociationR = await http.UnAssociateMeditationFileWithXYZZYX(new MeditationFileXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, MeditationFileId = rMeditationFileModel.result.Model.MeditationFileId }, userAdmin_Company_1);
            Assert.IsTrue(tmeditationFileUnAssociationR.result.IsSuccess);
            tmeditationFileList0 = await http.GetMeditationFilesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tmeditationFileList0.result.IsSuccess);
            Assert.IsTrue(!tmeditationFileList0.result.Model.Any(x => x.MeditationFileId == rMeditationFileModel.result.Model.MeditationFileId));

            #endregion
            */
        }
    }
}