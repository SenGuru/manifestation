using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task VisionBoardSessionPictureTest_IntegrationTest()
        {
            #region VisionBoardSessionPicture
            //Tests for VisionBoardSessionPicture
            //Create Item Test

            var aVisionBoardSessionPictureModel = new VisionBoardSessionPictureCreateModel();
            var rVisionBoardSessionPictureModel = await http.CreateVisionBoardSessionPicture(aVisionBoardSessionPictureModel, userAdmin_Company_1);
            Assert.IsTrue(rVisionBoardSessionPictureModel.result.IsSuccess);

            aVisionBoardSessionPictureModel = new VisionBoardSessionPictureCreateModel();
            var r0VisionBoardSessionPictureModel = await http.CreateVisionBoardSessionPicture(aVisionBoardSessionPictureModel, userAdmin_Company_1);
            Assert.IsTrue(!r0VisionBoardSessionPictureModel.result.IsSuccess);

            aVisionBoardSessionPictureModel = new VisionBoardSessionPictureCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00VisionBoardSessionPictureModel = await http.CreateVisionBoardSessionPicture(aVisionBoardSessionPictureModel, userAdmin_Company_1);
            Assert.IsTrue(r00VisionBoardSessionPictureModel.result.IsSuccess);

            aVisionBoardSessionPictureModel = new VisionBoardSessionPictureCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01VisionBoardSessionPictureModel = await http.CreateVisionBoardSessionPicture(aVisionBoardSessionPictureModel, userAdmin_Company_2);
            Assert.IsTrue(!r01VisionBoardSessionPictureModel.result.IsSuccess);


            aVisionBoardSessionPictureModel = new VisionBoardSessionPictureCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01VisionBoardSessionPictureModel = await http.CreateVisionBoardSessionPicture(aVisionBoardSessionPictureModel, userAdmin_Company_2);
            Assert.IsTrue(r01VisionBoardSessionPictureModel.result.IsSuccess);

            //Get List Test
            var r2VisionBoardSessionPictureModel = await http.GetVisionBoardSessionPictures(new VisionBoardSessionPictureSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2VisionBoardSessionPictureModel.result.IsSuccess);
            Assert.IsTrue(r2VisionBoardSessionPictureModel.result.Model.Any(x => x.VisionBoardSessionPictureId == rVisionBoardSessionPictureModel.result.Model.VisionBoardSessionPictureId));

            var r20VisionBoardSessionPictureModel = await http.GetVisionBoardSessionPictures(new VisionBoardSessionPictureSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20VisionBoardSessionPictureModel.result.Model.Any(x => x.VisionBoardSessionPictureId == rVisionBoardSessionPictureModel.result.Model.VisionBoardSessionPictureId));

            //Get Item Test
            var r3VisionBoardSessionPictureModel = await http.GetVisionBoardSessionPicture(rVisionBoardSessionPictureModel.result.Model.VisionBoardSessionPictureId, userAdmin_Company_1);
            Assert.IsTrue(r3VisionBoardSessionPictureModel.result.IsSuccess);
            Assert.IsTrue(r3VisionBoardSessionPictureModel.result.Model.VisionBoardSessionPictureId == rVisionBoardSessionPictureModel.result.Model.VisionBoardSessionPictureId);

            var r30VisionBoardSessionPictureModel = await http.GetVisionBoardSessionPicture(rVisionBoardSessionPictureModel.result.Model.VisionBoardSessionPictureId, userAdmin_Company_2);
            Assert.IsTrue(!r30VisionBoardSessionPictureModel.result.IsSuccess);

            //Edit Item Test
            var r4VisionBoardSessionPictureModel = await http.UpdateVisionBoardSessionPicture(rVisionBoardSessionPictureModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4VisionBoardSessionPictureModel.result.IsSuccess);
            //Assert.IsTrue(r4VisionBoardSessionPictureModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40VisionBoardSessionPictureModel = await http.UpdateVisionBoardSessionPicture(rVisionBoardSessionPictureModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40VisionBoardSessionPictureModel.result.IsSuccess);
            #endregion

            /*
            #region VisionBoardSessionPicture Association with XYZZYX

            var tvisionBoardSessionPictureList0 = await http.GetVisionBoardSessionPicturesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tvisionBoardSessionPictureList0.result.IsSuccess);

            var tvisionBoardSessionPictureAssociationR = await http.AssociateVisionBoardSessionPictureWithXYZZYX(new VisionBoardSessionPictureXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, VisionBoardSessionPictureId = rVisionBoardSessionPictureModel.result.Model.VisionBoardSessionPictureId }, userAdmin_Company_1);
            Assert.IsTrue(tvisionBoardSessionPictureAssociationR.result.IsSuccess);

            tvisionBoardSessionPictureList0 = await http.GetVisionBoardSessionPicturesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tvisionBoardSessionPictureList0.result.IsSuccess);
            Assert.IsTrue(tvisionBoardSessionPictureList0.result.Model.Any(x => x.VisionBoardSessionPictureId == rVisionBoardSessionPictureModel.result.Model.VisionBoardSessionPictureId));

            var tvisionBoardSessionPictureUnAssociationR = await http.UnAssociateVisionBoardSessionPictureWithXYZZYX(new VisionBoardSessionPictureXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, VisionBoardSessionPictureId = rVisionBoardSessionPictureModel.result.Model.VisionBoardSessionPictureId }, userAdmin_Company_1);
            Assert.IsTrue(tvisionBoardSessionPictureUnAssociationR.result.IsSuccess);
            tvisionBoardSessionPictureList0 = await http.GetVisionBoardSessionPicturesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tvisionBoardSessionPictureList0.result.IsSuccess);
            Assert.IsTrue(!tvisionBoardSessionPictureList0.result.Model.Any(x => x.VisionBoardSessionPictureId == rVisionBoardSessionPictureModel.result.Model.VisionBoardSessionPictureId));

            #endregion
            */
        }
    }
}