using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task EnumTypeTest_IntegrationTest()
        {
            #region EnumType
            //Tests for EnumType
            //Create Item Test

            var aEnumTypeModel = new EnumTypeCreateModel();
            var rEnumTypeModel = await http.CreateEnumType(aEnumTypeModel, userAdmin_Company_1);
            Assert.IsTrue(rEnumTypeModel.result.IsSuccess);

            aEnumTypeModel = new EnumTypeCreateModel();
            var r0EnumTypeModel = await http.CreateEnumType(aEnumTypeModel, userAdmin_Company_1);
            Assert.IsTrue(!r0EnumTypeModel.result.IsSuccess);

            aEnumTypeModel = new EnumTypeCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00EnumTypeModel = await http.CreateEnumType(aEnumTypeModel, userAdmin_Company_1);
            Assert.IsTrue(r00EnumTypeModel.result.IsSuccess);

            aEnumTypeModel = new EnumTypeCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01EnumTypeModel = await http.CreateEnumType(aEnumTypeModel, userAdmin_Company_2);
            Assert.IsTrue(!r01EnumTypeModel.result.IsSuccess);


            aEnumTypeModel = new EnumTypeCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01EnumTypeModel = await http.CreateEnumType(aEnumTypeModel, userAdmin_Company_2);
            Assert.IsTrue(r01EnumTypeModel.result.IsSuccess);

            //Get List Test
            var r2EnumTypeModel = await http.GetEnumTypes(new EnumTypeSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2EnumTypeModel.result.IsSuccess);
            Assert.IsTrue(r2EnumTypeModel.result.Model.Any(x => x.EnumTypeId == rEnumTypeModel.result.Model.EnumTypeId));

            var r20EnumTypeModel = await http.GetEnumTypes(new EnumTypeSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20EnumTypeModel.result.Model.Any(x => x.EnumTypeId == rEnumTypeModel.result.Model.EnumTypeId));

            //Get Item Test
            var r3EnumTypeModel = await http.GetEnumType(rEnumTypeModel.result.Model.EnumTypeId, userAdmin_Company_1);
            Assert.IsTrue(r3EnumTypeModel.result.IsSuccess);
            Assert.IsTrue(r3EnumTypeModel.result.Model.EnumTypeId == rEnumTypeModel.result.Model.EnumTypeId);

            var r30EnumTypeModel = await http.GetEnumType(rEnumTypeModel.result.Model.EnumTypeId, userAdmin_Company_2);
            Assert.IsTrue(!r30EnumTypeModel.result.IsSuccess);

            //Edit Item Test
            var r4EnumTypeModel = await http.UpdateEnumType(rEnumTypeModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4EnumTypeModel.result.IsSuccess);
            //Assert.IsTrue(r4EnumTypeModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40EnumTypeModel = await http.UpdateEnumType(rEnumTypeModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40EnumTypeModel.result.IsSuccess);
            #endregion

            /*
            #region EnumType Association with XYZZYX

            var tenumTypeList0 = await http.GetEnumTypesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tenumTypeList0.result.IsSuccess);

            var tenumTypeAssociationR = await http.AssociateEnumTypeWithXYZZYX(new EnumTypeXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, EnumTypeId = rEnumTypeModel.result.Model.EnumTypeId }, userAdmin_Company_1);
            Assert.IsTrue(tenumTypeAssociationR.result.IsSuccess);

            tenumTypeList0 = await http.GetEnumTypesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tenumTypeList0.result.IsSuccess);
            Assert.IsTrue(tenumTypeList0.result.Model.Any(x => x.EnumTypeId == rEnumTypeModel.result.Model.EnumTypeId));

            var tenumTypeUnAssociationR = await http.UnAssociateEnumTypeWithXYZZYX(new EnumTypeXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, EnumTypeId = rEnumTypeModel.result.Model.EnumTypeId }, userAdmin_Company_1);
            Assert.IsTrue(tenumTypeUnAssociationR.result.IsSuccess);
            tenumTypeList0 = await http.GetEnumTypesByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tenumTypeList0.result.IsSuccess);
            Assert.IsTrue(!tenumTypeList0.result.Model.Any(x => x.EnumTypeId == rEnumTypeModel.result.Model.EnumTypeId));

            #endregion
            */
        }
    }
}