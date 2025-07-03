using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task CompanyStorageConfigurationTest_IntegrationTest()
        {
            #region CompanyStorageConfiguration
            //Tests for CompanyStorageConfiguration
            //Create Item Test

            var aCompanyStorageConfigurationModel = new CompanyStorageConfigurationCreateModel();
            var rCompanyStorageConfigurationModel = await http.CreateCompanyStorageConfiguration(aCompanyStorageConfigurationModel, userAdmin_Company_1);
            Assert.IsTrue(rCompanyStorageConfigurationModel.result.IsSuccess);

            aCompanyStorageConfigurationModel = new CompanyStorageConfigurationCreateModel();
            var r0CompanyStorageConfigurationModel = await http.CreateCompanyStorageConfiguration(aCompanyStorageConfigurationModel, userAdmin_Company_1);
            Assert.IsTrue(!r0CompanyStorageConfigurationModel.result.IsSuccess);

            aCompanyStorageConfigurationModel = new CompanyStorageConfigurationCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00CompanyStorageConfigurationModel = await http.CreateCompanyStorageConfiguration(aCompanyStorageConfigurationModel, userAdmin_Company_1);
            Assert.IsTrue(r00CompanyStorageConfigurationModel.result.IsSuccess);

            aCompanyStorageConfigurationModel = new CompanyStorageConfigurationCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01CompanyStorageConfigurationModel = await http.CreateCompanyStorageConfiguration(aCompanyStorageConfigurationModel, userAdmin_Company_2);
            Assert.IsTrue(!r01CompanyStorageConfigurationModel.result.IsSuccess);


            aCompanyStorageConfigurationModel = new CompanyStorageConfigurationCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01CompanyStorageConfigurationModel = await http.CreateCompanyStorageConfiguration(aCompanyStorageConfigurationModel, userAdmin_Company_2);
            Assert.IsTrue(r01CompanyStorageConfigurationModel.result.IsSuccess);

            //Get Item Test
            var r3CompanyStorageConfigurationModel = await http.GetCompanyStorageConfiguration(rCompanyStorageConfigurationModel.result.Model.CompanyStorageConfigurationId, userAdmin_Company_1);
            Assert.IsTrue(r3CompanyStorageConfigurationModel.result.IsSuccess);
            Assert.IsTrue(r3CompanyStorageConfigurationModel.result.Model.CompanyStorageConfigurationId == rCompanyStorageConfigurationModel.result.Model.CompanyStorageConfigurationId);

            var r30CompanyStorageConfigurationModel = await http.GetCompanyStorageConfiguration(rCompanyStorageConfigurationModel.result.Model.CompanyStorageConfigurationId, userAdmin_Company_2);
            Assert.IsTrue(!r30CompanyStorageConfigurationModel.result.IsSuccess);

            //Edit Item Test
            var r4CompanyStorageConfigurationModel = await http.UpdateCompanyStorageConfiguration(rCompanyStorageConfigurationModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4CompanyStorageConfigurationModel.result.IsSuccess);
            //Assert.IsTrue(r4CompanyStorageConfigurationModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40CompanyStorageConfigurationModel = await http.UpdateCompanyStorageConfiguration(rCompanyStorageConfigurationModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40CompanyStorageConfigurationModel.result.IsSuccess);
            #endregion

            /*
            #region CompanyStorageConfiguration Association with XYZZYX

            var tcompanyStorageConfigurationList0 = await http.GetCompanyStorageConfigurationsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tcompanyStorageConfigurationList0.result.IsSuccess);

            var tcompanyStorageConfigurationAssociationR = await http.AssociateCompanyStorageConfigurationWithXYZZYX(new CompanyStorageConfigurationXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, CompanyStorageConfigurationId = rCompanyStorageConfigurationModel.result.Model.CompanyStorageConfigurationId }, userAdmin_Company_1);
            Assert.IsTrue(tcompanyStorageConfigurationAssociationR.result.IsSuccess);

            tcompanyStorageConfigurationList0 = await http.GetCompanyStorageConfigurationsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tcompanyStorageConfigurationList0.result.IsSuccess);
            Assert.IsTrue(tcompanyStorageConfigurationList0.result.Model.Any(x => x.CompanyStorageConfigurationId == rCompanyStorageConfigurationModel.result.Model.CompanyStorageConfigurationId));

            var tcompanyStorageConfigurationUnAssociationR = await http.UnAssociateCompanyStorageConfigurationWithXYZZYX(new CompanyStorageConfigurationXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, CompanyStorageConfigurationId = rCompanyStorageConfigurationModel.result.Model.CompanyStorageConfigurationId }, userAdmin_Company_1);
            Assert.IsTrue(tcompanyStorageConfigurationUnAssociationR.result.IsSuccess);
            tcompanyStorageConfigurationList0 = await http.GetCompanyStorageConfigurationsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tcompanyStorageConfigurationList0.result.IsSuccess);
            Assert.IsTrue(!tcompanyStorageConfigurationList0.result.Model.Any(x => x.CompanyStorageConfigurationId == rCompanyStorageConfigurationModel.result.Model.CompanyStorageConfigurationId));

            #endregion
            */
        }
    }
}