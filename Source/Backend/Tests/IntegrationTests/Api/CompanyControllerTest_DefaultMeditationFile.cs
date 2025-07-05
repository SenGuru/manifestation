using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task DefaultMeditationFileTest_IntegrationTest()
        {
            #region DefaultMeditationFile
            //Tests for DefaultMeditationFile
            //Create Item Test

            var aDefaultMeditationFileModel = new DefaultMeditationFileCreateModel();
            var rDefaultMeditationFileModel = await http.CreateDefaultMeditationFile(aDefaultMeditationFileModel, userAdmin_Company_1);
            Assert.IsTrue(rDefaultMeditationFileModel.result.IsSuccess);

            aDefaultMeditationFileModel = new DefaultMeditationFileCreateModel();
            var r0DefaultMeditationFileModel = await http.CreateDefaultMeditationFile(aDefaultMeditationFileModel, userAdmin_Company_1);
            Assert.IsTrue(!r0DefaultMeditationFileModel.result.IsSuccess);

            aDefaultMeditationFileModel = new DefaultMeditationFileCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00DefaultMeditationFileModel = await http.CreateDefaultMeditationFile(aDefaultMeditationFileModel, userAdmin_Company_1);
            Assert.IsTrue(r00DefaultMeditationFileModel.result.IsSuccess);

            aDefaultMeditationFileModel = new DefaultMeditationFileCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01DefaultMeditationFileModel = await http.CreateDefaultMeditationFile(aDefaultMeditationFileModel, userAdmin_Company_2);
            Assert.IsTrue(!r01DefaultMeditationFileModel.result.IsSuccess);


            aDefaultMeditationFileModel = new DefaultMeditationFileCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01DefaultMeditationFileModel = await http.CreateDefaultMeditationFile(aDefaultMeditationFileModel, userAdmin_Company_2);
            Assert.IsTrue(r01DefaultMeditationFileModel.result.IsSuccess);

            //Get List Test
            var r2DefaultMeditationFileModel = await http.GetDefaultMeditationFiles(new DefaultMeditationFileSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2DefaultMeditationFileModel.result.IsSuccess);
            Assert.IsTrue(r2DefaultMeditationFileModel.result.Model.Any(x => x.DefaultMeditationFileId == rDefaultMeditationFileModel.result.Model.DefaultMeditationFileId));

            var r20DefaultMeditationFileModel = await http.GetDefaultMeditationFiles(new DefaultMeditationFileSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20DefaultMeditationFileModel.result.Model.Any(x => x.DefaultMeditationFileId == rDefaultMeditationFileModel.result.Model.DefaultMeditationFileId));

            //Get Item Test
            var r3DefaultMeditationFileModel = await http.GetDefaultMeditationFile(rDefaultMeditationFileModel.result.Model.DefaultMeditationFileId, userAdmin_Company_1);
            Assert.IsTrue(r3DefaultMeditationFileModel.result.IsSuccess);
            Assert.IsTrue(r3DefaultMeditationFileModel.result.Model.DefaultMeditationFileId == rDefaultMeditationFileModel.result.Model.DefaultMeditationFileId);

            var r30DefaultMeditationFileModel = await http.GetDefaultMeditationFile(rDefaultMeditationFileModel.result.Model.DefaultMeditationFileId, userAdmin_Company_2);
            Assert.IsTrue(!r30DefaultMeditationFileModel.result.IsSuccess);

            //Edit Item Test
            var r4DefaultMeditationFileModel = await http.UpdateDefaultMeditationFile(rDefaultMeditationFileModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4DefaultMeditationFileModel.result.IsSuccess);
            //Assert.IsTrue(r4DefaultMeditationFileModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40DefaultMeditationFileModel = await http.UpdateDefaultMeditationFile(rDefaultMeditationFileModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40DefaultMeditationFileModel.result.IsSuccess);
            #endregion

            /*
            #region DefaultMeditationFile Association with XYZZYX

            var tdefaultMeditationFileList0 = await http.GetDefaultMeditationFilesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultMeditationFileList0.result.IsSuccess);

            var tdefaultMeditationFileAssociationR = await http.AssociateDefaultMeditationFileWithXYZZYX(new DefaultMeditationFileXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DefaultMeditationFileId = rDefaultMeditationFileModel.result.Model.DefaultMeditationFileId }, userAdmin_Company_1);
            Assert.IsTrue(tdefaultMeditationFileAssociationR.result.IsSuccess);

            tdefaultMeditationFileList0 = await http.GetDefaultMeditationFilesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultMeditationFileList0.result.IsSuccess);
            Assert.IsTrue(tdefaultMeditationFileList0.result.Model.Any(x => x.DefaultMeditationFileId == rDefaultMeditationFileModel.result.Model.DefaultMeditationFileId));

            var tdefaultMeditationFileUnAssociationR = await http.UnAssociateDefaultMeditationFileWithXYZZYX(new DefaultMeditationFileXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DefaultMeditationFileId = rDefaultMeditationFileModel.result.Model.DefaultMeditationFileId }, userAdmin_Company_1);
            Assert.IsTrue(tdefaultMeditationFileUnAssociationR.result.IsSuccess);
            tdefaultMeditationFileList0 = await http.GetDefaultMeditationFilesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultMeditationFileList0.result.IsSuccess);
            Assert.IsTrue(!tdefaultMeditationFileList0.result.Model.Any(x => x.DefaultMeditationFileId == rDefaultMeditationFileModel.result.Model.DefaultMeditationFileId));

            #endregion
            */
        }
    }
}