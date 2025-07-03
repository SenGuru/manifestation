using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task ConfigurationGroupTest_IntegrationTest()
        {
            #region ConfigurationGroup
            //Tests for ConfigurationGroup
            //Create Item Test

            var aConfigurationGroupModel = new ConfigurationGroupCreateModel();
            var rConfigurationGroupModel = await http.CreateConfigurationGroup(aConfigurationGroupModel, userAdmin_Company_1);
            Assert.IsTrue(rConfigurationGroupModel.result.IsSuccess);

            aConfigurationGroupModel = new ConfigurationGroupCreateModel();
            var r0ConfigurationGroupModel = await http.CreateConfigurationGroup(aConfigurationGroupModel, userAdmin_Company_1);
            Assert.IsTrue(!r0ConfigurationGroupModel.result.IsSuccess);

            aConfigurationGroupModel = new ConfigurationGroupCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00ConfigurationGroupModel = await http.CreateConfigurationGroup(aConfigurationGroupModel, userAdmin_Company_1);
            Assert.IsTrue(r00ConfigurationGroupModel.result.IsSuccess);

            aConfigurationGroupModel = new ConfigurationGroupCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01ConfigurationGroupModel = await http.CreateConfigurationGroup(aConfigurationGroupModel, userAdmin_Company_2);
            Assert.IsTrue(!r01ConfigurationGroupModel.result.IsSuccess);


            aConfigurationGroupModel = new ConfigurationGroupCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01ConfigurationGroupModel = await http.CreateConfigurationGroup(aConfigurationGroupModel, userAdmin_Company_2);
            Assert.IsTrue(r01ConfigurationGroupModel.result.IsSuccess);

            //Get List Test
            var r2ConfigurationGroupModel = await http.GetConfigurationGroups(new ConfigurationGroupSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2ConfigurationGroupModel.result.IsSuccess);
            Assert.IsTrue(r2ConfigurationGroupModel.result.Model.Any(x => x.ConfigurationGroupId == rConfigurationGroupModel.result.Model.ConfigurationGroupId));

            var r20ConfigurationGroupModel = await http.GetConfigurationGroups(new ConfigurationGroupSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20ConfigurationGroupModel.result.Model.Any(x => x.ConfigurationGroupId == rConfigurationGroupModel.result.Model.ConfigurationGroupId));

            //Get Item Test
            var r3ConfigurationGroupModel = await http.GetConfigurationGroup(rConfigurationGroupModel.result.Model.ConfigurationGroupId, userAdmin_Company_1);
            Assert.IsTrue(r3ConfigurationGroupModel.result.IsSuccess);
            Assert.IsTrue(r3ConfigurationGroupModel.result.Model.ConfigurationGroupId == rConfigurationGroupModel.result.Model.ConfigurationGroupId);

            var r30ConfigurationGroupModel = await http.GetConfigurationGroup(rConfigurationGroupModel.result.Model.ConfigurationGroupId, userAdmin_Company_2);
            Assert.IsTrue(!r30ConfigurationGroupModel.result.IsSuccess);

            //Edit Item Test
            var r4ConfigurationGroupModel = await http.UpdateConfigurationGroup(rConfigurationGroupModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4ConfigurationGroupModel.result.IsSuccess);
            //Assert.IsTrue(r4ConfigurationGroupModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40ConfigurationGroupModel = await http.UpdateConfigurationGroup(rConfigurationGroupModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40ConfigurationGroupModel.result.IsSuccess);
            #endregion

            /*
            #region ConfigurationGroup Association with XYZZYX

            var tconfigurationGroupList0 = await http.GetConfigurationGroupsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tconfigurationGroupList0.result.IsSuccess);

            var tconfigurationGroupAssociationR = await http.AssociateConfigurationGroupWithXYZZYX(new ConfigurationGroupXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ConfigurationGroupId = rConfigurationGroupModel.result.Model.ConfigurationGroupId }, userAdmin_Company_1);
            Assert.IsTrue(tconfigurationGroupAssociationR.result.IsSuccess);

            tconfigurationGroupList0 = await http.GetConfigurationGroupsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tconfigurationGroupList0.result.IsSuccess);
            Assert.IsTrue(tconfigurationGroupList0.result.Model.Any(x => x.ConfigurationGroupId == rConfigurationGroupModel.result.Model.ConfigurationGroupId));

            var tconfigurationGroupUnAssociationR = await http.UnAssociateConfigurationGroupWithXYZZYX(new ConfigurationGroupXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ConfigurationGroupId = rConfigurationGroupModel.result.Model.ConfigurationGroupId }, userAdmin_Company_1);
            Assert.IsTrue(tconfigurationGroupUnAssociationR.result.IsSuccess);
            tconfigurationGroupList0 = await http.GetConfigurationGroupsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tconfigurationGroupList0.result.IsSuccess);
            Assert.IsTrue(!tconfigurationGroupList0.result.Model.Any(x => x.ConfigurationGroupId == rConfigurationGroupModel.result.Model.ConfigurationGroupId));

            #endregion
            */
        }
    }
}