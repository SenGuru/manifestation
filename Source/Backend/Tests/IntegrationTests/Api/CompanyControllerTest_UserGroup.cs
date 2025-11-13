using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task UserGroupTest_IntegrationTest()
        {
            #region UserGroup
            //Tests for UserGroup
            //Create Item Test

            var aUserGroupModel = new UserGroupCreateModel();
            var rUserGroupModel = await http.CreateUserGroup(aUserGroupModel, userAdmin_Company_1);
            Assert.IsTrue(rUserGroupModel.result.IsSuccess);

            aUserGroupModel = new UserGroupCreateModel();
            var r0UserGroupModel = await http.CreateUserGroup(aUserGroupModel, userAdmin_Company_1);
            Assert.IsTrue(!r0UserGroupModel.result.IsSuccess);

            aUserGroupModel = new UserGroupCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00UserGroupModel = await http.CreateUserGroup(aUserGroupModel, userAdmin_Company_1);
            Assert.IsTrue(r00UserGroupModel.result.IsSuccess);

            aUserGroupModel = new UserGroupCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01UserGroupModel = await http.CreateUserGroup(aUserGroupModel, userAdmin_Company_2);
            Assert.IsTrue(!r01UserGroupModel.result.IsSuccess);


            aUserGroupModel = new UserGroupCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01UserGroupModel = await http.CreateUserGroup(aUserGroupModel, userAdmin_Company_2);
            Assert.IsTrue(r01UserGroupModel.result.IsSuccess);

            //Get List Test
            var r2UserGroupModel = await http.GetUserGroups(new UserGroupSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2UserGroupModel.result.IsSuccess);
            Assert.IsTrue(r2UserGroupModel.result.Model.Any(x => x.UserGroupId == rUserGroupModel.result.Model.UserGroupId));

            var r20UserGroupModel = await http.GetUserGroups(new UserGroupSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20UserGroupModel.result.Model.Any(x => x.UserGroupId == rUserGroupModel.result.Model.UserGroupId));

            //Get Item Test
            var r3UserGroupModel = await http.GetUserGroup(rUserGroupModel.result.Model.UserGroupId, userAdmin_Company_1);
            Assert.IsTrue(r3UserGroupModel.result.IsSuccess);
            Assert.IsTrue(r3UserGroupModel.result.Model.UserGroupId == rUserGroupModel.result.Model.UserGroupId);

            var r30UserGroupModel = await http.GetUserGroup(rUserGroupModel.result.Model.UserGroupId, userAdmin_Company_2);
            Assert.IsTrue(!r30UserGroupModel.result.IsSuccess);

            //Edit Item Test
            var r4UserGroupModel = await http.UpdateUserGroup(rUserGroupModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4UserGroupModel.result.IsSuccess);
            //Assert.IsTrue(r4UserGroupModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40UserGroupModel = await http.UpdateUserGroup(rUserGroupModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40UserGroupModel.result.IsSuccess);
            #endregion

            /*
            #region UserGroup Association with XYZZYX

            var tuserGroupList0 = await http.GetUserGroupsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tuserGroupList0.result.IsSuccess);

            var tuserGroupAssociationR = await http.AssociateUserGroupWithXYZZYX(new UserGroupXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, UserGroupId = rUserGroupModel.result.Model.UserGroupId }, userAdmin_Company_1);
            Assert.IsTrue(tuserGroupAssociationR.result.IsSuccess);

            tuserGroupList0 = await http.GetUserGroupsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tuserGroupList0.result.IsSuccess);
            Assert.IsTrue(tuserGroupList0.result.Model.Any(x => x.UserGroupId == rUserGroupModel.result.Model.UserGroupId));

            var tuserGroupUnAssociationR = await http.UnAssociateUserGroupWithXYZZYX(new UserGroupXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, UserGroupId = rUserGroupModel.result.Model.UserGroupId }, userAdmin_Company_1);
            Assert.IsTrue(tuserGroupUnAssociationR.result.IsSuccess);
            tuserGroupList0 = await http.GetUserGroupsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tuserGroupList0.result.IsSuccess);
            Assert.IsTrue(!tuserGroupList0.result.Model.Any(x => x.UserGroupId == rUserGroupModel.result.Model.UserGroupId));

            #endregion
            */
        }
    }
}