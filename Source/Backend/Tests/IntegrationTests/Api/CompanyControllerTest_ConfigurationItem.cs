using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task ConfigurationItemTest_IntegrationTest()
        {
            #region ConfigurationItem
            //Tests for ConfigurationItem
            //Create Item Test

            var aConfigurationItemModel = new ConfigurationItemCreateModel();
            var rConfigurationItemModel = await http.CreateConfigurationItem(aConfigurationItemModel, userAdmin_Company_1);
            Assert.IsTrue(rConfigurationItemModel.result.IsSuccess);

            aConfigurationItemModel = new ConfigurationItemCreateModel();
            var r0ConfigurationItemModel = await http.CreateConfigurationItem(aConfigurationItemModel, userAdmin_Company_1);
            Assert.IsTrue(!r0ConfigurationItemModel.result.IsSuccess);

            aConfigurationItemModel = new ConfigurationItemCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00ConfigurationItemModel = await http.CreateConfigurationItem(aConfigurationItemModel, userAdmin_Company_1);
            Assert.IsTrue(r00ConfigurationItemModel.result.IsSuccess);

            aConfigurationItemModel = new ConfigurationItemCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01ConfigurationItemModel = await http.CreateConfigurationItem(aConfigurationItemModel, userAdmin_Company_2);
            Assert.IsTrue(!r01ConfigurationItemModel.result.IsSuccess);


            aConfigurationItemModel = new ConfigurationItemCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01ConfigurationItemModel = await http.CreateConfigurationItem(aConfigurationItemModel, userAdmin_Company_2);
            Assert.IsTrue(r01ConfigurationItemModel.result.IsSuccess);

            //Get List Test
            var r2ConfigurationItemModel = await http.GetConfigurationItems(new ConfigurationItemSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2ConfigurationItemModel.result.IsSuccess);
            Assert.IsTrue(r2ConfigurationItemModel.result.Model.Any(x => x.ConfigurationItemId == rConfigurationItemModel.result.Model.ConfigurationItemId));

            var r20ConfigurationItemModel = await http.GetConfigurationItems(new ConfigurationItemSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20ConfigurationItemModel.result.Model.Any(x => x.ConfigurationItemId == rConfigurationItemModel.result.Model.ConfigurationItemId));

            //Get Item Test
            var r3ConfigurationItemModel = await http.GetConfigurationItem(rConfigurationItemModel.result.Model.ConfigurationItemId, userAdmin_Company_1);
            Assert.IsTrue(r3ConfigurationItemModel.result.IsSuccess);
            Assert.IsTrue(r3ConfigurationItemModel.result.Model.ConfigurationItemId == rConfigurationItemModel.result.Model.ConfigurationItemId);

            var r30ConfigurationItemModel = await http.GetConfigurationItem(rConfigurationItemModel.result.Model.ConfigurationItemId, userAdmin_Company_2);
            Assert.IsTrue(!r30ConfigurationItemModel.result.IsSuccess);

            //Edit Item Test
            var r4ConfigurationItemModel = await http.UpdateConfigurationItem(rConfigurationItemModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4ConfigurationItemModel.result.IsSuccess);
            //Assert.IsTrue(r4ConfigurationItemModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40ConfigurationItemModel = await http.UpdateConfigurationItem(rConfigurationItemModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40ConfigurationItemModel.result.IsSuccess);
            #endregion

            /*
            #region ConfigurationItem Association with XYZZYX

            var tconfigurationItemList0 = await http.GetConfigurationItemsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tconfigurationItemList0.result.IsSuccess);

            var tconfigurationItemAssociationR = await http.AssociateConfigurationItemWithXYZZYX(new ConfigurationItemXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ConfigurationItemId = rConfigurationItemModel.result.Model.ConfigurationItemId }, userAdmin_Company_1);
            Assert.IsTrue(tconfigurationItemAssociationR.result.IsSuccess);

            tconfigurationItemList0 = await http.GetConfigurationItemsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tconfigurationItemList0.result.IsSuccess);
            Assert.IsTrue(tconfigurationItemList0.result.Model.Any(x => x.ConfigurationItemId == rConfigurationItemModel.result.Model.ConfigurationItemId));

            var tconfigurationItemUnAssociationR = await http.UnAssociateConfigurationItemWithXYZZYX(new ConfigurationItemXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, ConfigurationItemId = rConfigurationItemModel.result.Model.ConfigurationItemId }, userAdmin_Company_1);
            Assert.IsTrue(tconfigurationItemUnAssociationR.result.IsSuccess);
            tconfigurationItemList0 = await http.GetConfigurationItemsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tconfigurationItemList0.result.IsSuccess);
            Assert.IsTrue(!tconfigurationItemList0.result.Model.Any(x => x.ConfigurationItemId == rConfigurationItemModel.result.Model.ConfigurationItemId));

            #endregion
            */
        }
    }
}