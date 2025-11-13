using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task VisionBoardPictureTest_IntegrationTest()
        {
            #region VisionBoardPicture
            //Tests for VisionBoardPicture
            //Create Item Test

            var aVisionBoardPictureModel = new VisionBoardPictureCreateWithFileFormModel();
            var rVisionBoardPictureModel = await http.CreateVisionBoardPicture(aVisionBoardPictureModel, userAdmin_Company_1);
            Assert.IsTrue(rVisionBoardPictureModel.result.IsSuccess);

            aVisionBoardPictureModel = new VisionBoardPictureCreateWithFileFormModel();
            var r0VisionBoardPictureModel = await http.CreateVisionBoardPicture(aVisionBoardPictureModel, userAdmin_Company_1);
            Assert.IsTrue(!r0VisionBoardPictureModel.result.IsSuccess);

            aVisionBoardPictureModel = new VisionBoardPictureCreateWithFileFormModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00VisionBoardPictureModel = await http.CreateVisionBoardPicture(aVisionBoardPictureModel, userAdmin_Company_1);
            Assert.IsTrue(r00VisionBoardPictureModel.result.IsSuccess);

            aVisionBoardPictureModel = new VisionBoardPictureCreateWithFileFormModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01VisionBoardPictureModel = await http.CreateVisionBoardPicture(aVisionBoardPictureModel, userAdmin_Company_2);
            Assert.IsTrue(!r01VisionBoardPictureModel.result.IsSuccess);


            aVisionBoardPictureModel = new VisionBoardPictureCreateWithFileFormModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01VisionBoardPictureModel = await http.CreateVisionBoardPicture(aVisionBoardPictureModel, userAdmin_Company_2);
            Assert.IsTrue(r01VisionBoardPictureModel.result.IsSuccess);

            //Get List Test
            var r2VisionBoardPictureModel = await http.GetVisionBoardPictures(new VisionBoardPictureSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2VisionBoardPictureModel.result.IsSuccess);
            Assert.IsTrue(r2VisionBoardPictureModel.result.Model.Any(x => x.VisionBoardPictureId == rVisionBoardPictureModel.result.Model.VisionBoardPictureId));

            var r20VisionBoardPictureModel = await http.GetVisionBoardPictures(new VisionBoardPictureSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20VisionBoardPictureModel.result.Model.Any(x => x.VisionBoardPictureId == rVisionBoardPictureModel.result.Model.VisionBoardPictureId));

            //Get Item Test
            var r3VisionBoardPictureModel = await http.GetVisionBoardPicture(rVisionBoardPictureModel.result.Model.VisionBoardPictureId, userAdmin_Company_1);
            Assert.IsTrue(r3VisionBoardPictureModel.result.IsSuccess);
            Assert.IsTrue(r3VisionBoardPictureModel.result.Model.VisionBoardPictureId == rVisionBoardPictureModel.result.Model.VisionBoardPictureId);

            var r30VisionBoardPictureModel = await http.GetVisionBoardPicture(rVisionBoardPictureModel.result.Model.VisionBoardPictureId, userAdmin_Company_2);
            Assert.IsTrue(!r30VisionBoardPictureModel.result.IsSuccess);

            //Edit Item Test
            var r4VisionBoardPictureModel = await http.UpdateVisionBoardPicture(rVisionBoardPictureModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4VisionBoardPictureModel.result.IsSuccess);
            //Assert.IsTrue(r4VisionBoardPictureModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40VisionBoardPictureModel = await http.UpdateVisionBoardPicture(rVisionBoardPictureModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40VisionBoardPictureModel.result.IsSuccess);
            #endregion

            /*
            #region VisionBoardPicture Association with XYZZYX

            var tvisionBoardPictureList0 = await http.GetVisionBoardPicturesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tvisionBoardPictureList0.result.IsSuccess);

            var tvisionBoardPictureAssociationR = await http.AssociateVisionBoardPictureWithXYZZYX(new VisionBoardPictureXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, VisionBoardPictureId = rVisionBoardPictureModel.result.Model.VisionBoardPictureId }, userAdmin_Company_1);
            Assert.IsTrue(tvisionBoardPictureAssociationR.result.IsSuccess);

            tvisionBoardPictureList0 = await http.GetVisionBoardPicturesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tvisionBoardPictureList0.result.IsSuccess);
            Assert.IsTrue(tvisionBoardPictureList0.result.Model.Any(x => x.VisionBoardPictureId == rVisionBoardPictureModel.result.Model.VisionBoardPictureId));

            var tvisionBoardPictureUnAssociationR = await http.UnAssociateVisionBoardPictureWithXYZZYX(new VisionBoardPictureXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, VisionBoardPictureId = rVisionBoardPictureModel.result.Model.VisionBoardPictureId }, userAdmin_Company_1);
            Assert.IsTrue(tvisionBoardPictureUnAssociationR.result.IsSuccess);
            tvisionBoardPictureList0 = await http.GetVisionBoardPicturesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tvisionBoardPictureList0.result.IsSuccess);
            Assert.IsTrue(!tvisionBoardPictureList0.result.Model.Any(x => x.VisionBoardPictureId == rVisionBoardPictureModel.result.Model.VisionBoardPictureId));

            #endregion
            */
        }
    }
}