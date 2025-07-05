using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task DefaultVisionBoardPictureTest_IntegrationTest()
        {
            #region DefaultVisionBoardPicture
            //Tests for DefaultVisionBoardPicture
            //Create Item Test

            var aDefaultVisionBoardPictureModel = new DefaultVisionBoardPictureCreateModel();
            var rDefaultVisionBoardPictureModel = await http.CreateDefaultVisionBoardPicture(aDefaultVisionBoardPictureModel, userAdmin_Company_1);
            Assert.IsTrue(rDefaultVisionBoardPictureModel.result.IsSuccess);

            aDefaultVisionBoardPictureModel = new DefaultVisionBoardPictureCreateModel();
            var r0DefaultVisionBoardPictureModel = await http.CreateDefaultVisionBoardPicture(aDefaultVisionBoardPictureModel, userAdmin_Company_1);
            Assert.IsTrue(!r0DefaultVisionBoardPictureModel.result.IsSuccess);

            aDefaultVisionBoardPictureModel = new DefaultVisionBoardPictureCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00DefaultVisionBoardPictureModel = await http.CreateDefaultVisionBoardPicture(aDefaultVisionBoardPictureModel, userAdmin_Company_1);
            Assert.IsTrue(r00DefaultVisionBoardPictureModel.result.IsSuccess);

            aDefaultVisionBoardPictureModel = new DefaultVisionBoardPictureCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01DefaultVisionBoardPictureModel = await http.CreateDefaultVisionBoardPicture(aDefaultVisionBoardPictureModel, userAdmin_Company_2);
            Assert.IsTrue(!r01DefaultVisionBoardPictureModel.result.IsSuccess);


            aDefaultVisionBoardPictureModel = new DefaultVisionBoardPictureCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01DefaultVisionBoardPictureModel = await http.CreateDefaultVisionBoardPicture(aDefaultVisionBoardPictureModel, userAdmin_Company_2);
            Assert.IsTrue(r01DefaultVisionBoardPictureModel.result.IsSuccess);

            //Get List Test
            var r2DefaultVisionBoardPictureModel = await http.GetDefaultVisionBoardPictures(new DefaultVisionBoardPictureSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2DefaultVisionBoardPictureModel.result.IsSuccess);
            Assert.IsTrue(r2DefaultVisionBoardPictureModel.result.Model.Any(x => x.DefaultVisionBoardPictureId == rDefaultVisionBoardPictureModel.result.Model.DefaultVisionBoardPictureId));

            var r20DefaultVisionBoardPictureModel = await http.GetDefaultVisionBoardPictures(new DefaultVisionBoardPictureSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20DefaultVisionBoardPictureModel.result.Model.Any(x => x.DefaultVisionBoardPictureId == rDefaultVisionBoardPictureModel.result.Model.DefaultVisionBoardPictureId));

            //Get Item Test
            var r3DefaultVisionBoardPictureModel = await http.GetDefaultVisionBoardPicture(rDefaultVisionBoardPictureModel.result.Model.DefaultVisionBoardPictureId, userAdmin_Company_1);
            Assert.IsTrue(r3DefaultVisionBoardPictureModel.result.IsSuccess);
            Assert.IsTrue(r3DefaultVisionBoardPictureModel.result.Model.DefaultVisionBoardPictureId == rDefaultVisionBoardPictureModel.result.Model.DefaultVisionBoardPictureId);

            var r30DefaultVisionBoardPictureModel = await http.GetDefaultVisionBoardPicture(rDefaultVisionBoardPictureModel.result.Model.DefaultVisionBoardPictureId, userAdmin_Company_2);
            Assert.IsTrue(!r30DefaultVisionBoardPictureModel.result.IsSuccess);

            //Edit Item Test
            var r4DefaultVisionBoardPictureModel = await http.UpdateDefaultVisionBoardPicture(rDefaultVisionBoardPictureModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4DefaultVisionBoardPictureModel.result.IsSuccess);
            //Assert.IsTrue(r4DefaultVisionBoardPictureModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40DefaultVisionBoardPictureModel = await http.UpdateDefaultVisionBoardPicture(rDefaultVisionBoardPictureModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40DefaultVisionBoardPictureModel.result.IsSuccess);
            #endregion

            /*
            #region DefaultVisionBoardPicture Association with XYZZYX

            var tdefaultVisionBoardPictureList0 = await http.GetDefaultVisionBoardPicturesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultVisionBoardPictureList0.result.IsSuccess);

            var tdefaultVisionBoardPictureAssociationR = await http.AssociateDefaultVisionBoardPictureWithXYZZYX(new DefaultVisionBoardPictureXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DefaultVisionBoardPictureId = rDefaultVisionBoardPictureModel.result.Model.DefaultVisionBoardPictureId }, userAdmin_Company_1);
            Assert.IsTrue(tdefaultVisionBoardPictureAssociationR.result.IsSuccess);

            tdefaultVisionBoardPictureList0 = await http.GetDefaultVisionBoardPicturesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultVisionBoardPictureList0.result.IsSuccess);
            Assert.IsTrue(tdefaultVisionBoardPictureList0.result.Model.Any(x => x.DefaultVisionBoardPictureId == rDefaultVisionBoardPictureModel.result.Model.DefaultVisionBoardPictureId));

            var tdefaultVisionBoardPictureUnAssociationR = await http.UnAssociateDefaultVisionBoardPictureWithXYZZYX(new DefaultVisionBoardPictureXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DefaultVisionBoardPictureId = rDefaultVisionBoardPictureModel.result.Model.DefaultVisionBoardPictureId }, userAdmin_Company_1);
            Assert.IsTrue(tdefaultVisionBoardPictureUnAssociationR.result.IsSuccess);
            tdefaultVisionBoardPictureList0 = await http.GetDefaultVisionBoardPicturesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdefaultVisionBoardPictureList0.result.IsSuccess);
            Assert.IsTrue(!tdefaultVisionBoardPictureList0.result.Model.Any(x => x.DefaultVisionBoardPictureId == rDefaultVisionBoardPictureModel.result.Model.DefaultVisionBoardPictureId));

            #endregion
            */
        }
    }
}