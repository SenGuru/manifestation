using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task EnumGroupTest_IntegrationTest()
        {
            #region EnumGroup
            //Tests for EnumGroup
            //Create Item Test

            var aEnumGroupModel = new EnumGroupCreateModel();
            var rEnumGroupModel = await http.CreateEnumGroup(aEnumGroupModel, userAdmin_Company_1);
            Assert.IsTrue(rEnumGroupModel.result.IsSuccess);

            aEnumGroupModel = new EnumGroupCreateModel();
            var r0EnumGroupModel = await http.CreateEnumGroup(aEnumGroupModel, userAdmin_Company_1);
            Assert.IsTrue(!r0EnumGroupModel.result.IsSuccess);

            aEnumGroupModel = new EnumGroupCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00EnumGroupModel = await http.CreateEnumGroup(aEnumGroupModel, userAdmin_Company_1);
            Assert.IsTrue(r00EnumGroupModel.result.IsSuccess);

            aEnumGroupModel = new EnumGroupCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01EnumGroupModel = await http.CreateEnumGroup(aEnumGroupModel, userAdmin_Company_2);
            Assert.IsTrue(!r01EnumGroupModel.result.IsSuccess);


            aEnumGroupModel = new EnumGroupCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01EnumGroupModel = await http.CreateEnumGroup(aEnumGroupModel, userAdmin_Company_2);
            Assert.IsTrue(r01EnumGroupModel.result.IsSuccess);

            //Get List Test
            var r2EnumGroupModel = await http.GetEnumGroups(new EnumGroupSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2EnumGroupModel.result.IsSuccess);
            Assert.IsTrue(r2EnumGroupModel.result.Model.Any(x => x.EnumGroupId == rEnumGroupModel.result.Model.EnumGroupId));

            var r20EnumGroupModel = await http.GetEnumGroups(new EnumGroupSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20EnumGroupModel.result.Model.Any(x => x.EnumGroupId == rEnumGroupModel.result.Model.EnumGroupId));

            //Get Item Test
            var r3EnumGroupModel = await http.GetEnumGroup(rEnumGroupModel.result.Model.EnumGroupId, userAdmin_Company_1);
            Assert.IsTrue(r3EnumGroupModel.result.IsSuccess);
            Assert.IsTrue(r3EnumGroupModel.result.Model.EnumGroupId == rEnumGroupModel.result.Model.EnumGroupId);

            var r30EnumGroupModel = await http.GetEnumGroup(rEnumGroupModel.result.Model.EnumGroupId, userAdmin_Company_2);
            Assert.IsTrue(!r30EnumGroupModel.result.IsSuccess);

            //Edit Item Test
            var r4EnumGroupModel = await http.UpdateEnumGroup(rEnumGroupModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4EnumGroupModel.result.IsSuccess);
            //Assert.IsTrue(r4EnumGroupModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40EnumGroupModel = await http.UpdateEnumGroup(rEnumGroupModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40EnumGroupModel.result.IsSuccess);
            #endregion

            /*
            #region EnumGroup Association with XYZZYX

            var tenumGroupList0 = await http.GetEnumGroupsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tenumGroupList0.result.IsSuccess);

            var tenumGroupAssociationR = await http.AssociateEnumGroupWithXYZZYX(new EnumGroupXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, EnumGroupId = rEnumGroupModel.result.Model.EnumGroupId }, userAdmin_Company_1);
            Assert.IsTrue(tenumGroupAssociationR.result.IsSuccess);

            tenumGroupList0 = await http.GetEnumGroupsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tenumGroupList0.result.IsSuccess);
            Assert.IsTrue(tenumGroupList0.result.Model.Any(x => x.EnumGroupId == rEnumGroupModel.result.Model.EnumGroupId));

            var tenumGroupUnAssociationR = await http.UnAssociateEnumGroupWithXYZZYX(new EnumGroupXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, EnumGroupId = rEnumGroupModel.result.Model.EnumGroupId }, userAdmin_Company_1);
            Assert.IsTrue(tenumGroupUnAssociationR.result.IsSuccess);
            tenumGroupList0 = await http.GetEnumGroupsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tenumGroupList0.result.IsSuccess);
            Assert.IsTrue(!tenumGroupList0.result.Model.Any(x => x.EnumGroupId == rEnumGroupModel.result.Model.EnumGroupId));

            #endregion
            */
        }
    }
}