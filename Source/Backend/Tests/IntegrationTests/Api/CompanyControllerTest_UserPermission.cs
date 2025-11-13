using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task UserPermissionTest_IntegrationTest()
        {
            #region UserPermission
            //Tests for UserPermission
            //Create Item Test

            var aUserPermissionModel = new UserPermissionCreateModel();
            var rUserPermissionModel = await http.CreateUserPermission(aUserPermissionModel, userAdmin_Company_1);
            Assert.IsTrue(rUserPermissionModel.result.IsSuccess);

            aUserPermissionModel = new UserPermissionCreateModel();
            var r0UserPermissionModel = await http.CreateUserPermission(aUserPermissionModel, userAdmin_Company_1);
            Assert.IsTrue(!r0UserPermissionModel.result.IsSuccess);

            aUserPermissionModel = new UserPermissionCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00UserPermissionModel = await http.CreateUserPermission(aUserPermissionModel, userAdmin_Company_1);
            Assert.IsTrue(r00UserPermissionModel.result.IsSuccess);

            aUserPermissionModel = new UserPermissionCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01UserPermissionModel = await http.CreateUserPermission(aUserPermissionModel, userAdmin_Company_2);
            Assert.IsTrue(!r01UserPermissionModel.result.IsSuccess);


            aUserPermissionModel = new UserPermissionCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01UserPermissionModel = await http.CreateUserPermission(aUserPermissionModel, userAdmin_Company_2);
            Assert.IsTrue(r01UserPermissionModel.result.IsSuccess);

            //Get List Test
            var r2UserPermissionModel = await http.GetUserPermissions(new UserPermissionSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2UserPermissionModel.result.IsSuccess);
            Assert.IsTrue(r2UserPermissionModel.result.Model.Any(x => x.UserPermissionId == rUserPermissionModel.result.Model.UserPermissionId));

            var r20UserPermissionModel = await http.GetUserPermissions(new UserPermissionSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20UserPermissionModel.result.Model.Any(x => x.UserPermissionId == rUserPermissionModel.result.Model.UserPermissionId));

            //Get Item Test
            var r3UserPermissionModel = await http.GetUserPermission(rUserPermissionModel.result.Model.UserPermissionId, userAdmin_Company_1);
            Assert.IsTrue(r3UserPermissionModel.result.IsSuccess);
            Assert.IsTrue(r3UserPermissionModel.result.Model.UserPermissionId == rUserPermissionModel.result.Model.UserPermissionId);

            var r30UserPermissionModel = await http.GetUserPermission(rUserPermissionModel.result.Model.UserPermissionId, userAdmin_Company_2);
            Assert.IsTrue(!r30UserPermissionModel.result.IsSuccess);

            //Edit Item Test
            var r4UserPermissionModel = await http.UpdateUserPermission(rUserPermissionModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4UserPermissionModel.result.IsSuccess);
            //Assert.IsTrue(r4UserPermissionModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40UserPermissionModel = await http.UpdateUserPermission(rUserPermissionModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40UserPermissionModel.result.IsSuccess);
            #endregion

            /*
            #region UserPermission Association with XYZZYX

            var tuserPermissionList0 = await http.GetUserPermissionsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tuserPermissionList0.result.IsSuccess);

            var tuserPermissionAssociationR = await http.AssociateUserPermissionWithXYZZYX(new UserPermissionXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, UserPermissionId = rUserPermissionModel.result.Model.UserPermissionId }, userAdmin_Company_1);
            Assert.IsTrue(tuserPermissionAssociationR.result.IsSuccess);

            tuserPermissionList0 = await http.GetUserPermissionsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tuserPermissionList0.result.IsSuccess);
            Assert.IsTrue(tuserPermissionList0.result.Model.Any(x => x.UserPermissionId == rUserPermissionModel.result.Model.UserPermissionId));

            var tuserPermissionUnAssociationR = await http.UnAssociateUserPermissionWithXYZZYX(new UserPermissionXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, UserPermissionId = rUserPermissionModel.result.Model.UserPermissionId }, userAdmin_Company_1);
            Assert.IsTrue(tuserPermissionUnAssociationR.result.IsSuccess);
            tuserPermissionList0 = await http.GetUserPermissionsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tuserPermissionList0.result.IsSuccess);
            Assert.IsTrue(!tuserPermissionList0.result.Model.Any(x => x.UserPermissionId == rUserPermissionModel.result.Model.UserPermissionId));

            #endregion
            */
        }
    }
}