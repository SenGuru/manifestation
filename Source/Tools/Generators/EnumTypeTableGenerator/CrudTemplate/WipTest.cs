using System.Linq;
using System.Threading.Tasks;
using %AppName%.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace %AppName%.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task EEntity123Test_IntegrationTest()
        {
            #region EEntity123
            //Tests for EEntity123
            //Create Item Test

            var aEEntity123Model = new EEntity123CreateModel();
            var rEEntity123Model = await http.CreateEEntity123(aEEntity123Model, userAdmin_Company_1);
            Assert.IsTrue(rEEntity123Model.result.IsSuccess);

            aEEntity123Model = new EEntity123CreateModel();
            var r0EEntity123Model = await http.CreateEEntity123(aEEntity123Model, userAdmin_Company_1);
            Assert.IsTrue(!r0EEntity123Model.result.IsSuccess);

            aEEntity123Model = new EEntity123CreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00EEntity123Model = await http.CreateEEntity123(aEEntity123Model, userAdmin_Company_1);
            Assert.IsTrue(r00EEntity123Model.result.IsSuccess);

            aEEntity123Model = new EEntity123CreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01EEntity123Model = await http.CreateEEntity123(aEEntity123Model, userAdmin_Company_2);
            Assert.IsTrue(!r01EEntity123Model.result.IsSuccess);


            aEEntity123Model = new EEntity123CreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01EEntity123Model = await http.CreateEEntity123(aEEntity123Model, userAdmin_Company_2);
            Assert.IsTrue(r01EEntity123Model.result.IsSuccess);

            //Get List Test
            var r2EEntity123Model = await http.GetEEntity123s(new EEntity123SearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2EEntity123Model.result.IsSuccess);
            Assert.IsTrue(r2EEntity123Model.result.Model.Any(x => x.EEntity123Id == rEEntity123Model.result.Model.EEntity123Id));

            var r20EEntity123Model = await http.GetEEntity123s(new EEntity123SearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20EEntity123Model.result.Model.Any(x => x.EEntity123Id == rEEntity123Model.result.Model.EEntity123Id));

            //Get Item Test
            var r3EEntity123Model = await http.GetEEntity123(rEEntity123Model.result.Model.EEntity123Id, userAdmin_Company_1);
            Assert.IsTrue(r3EEntity123Model.result.IsSuccess);
            Assert.IsTrue(r3EEntity123Model.result.Model.EEntity123Id == rEEntity123Model.result.Model.EEntity123Id);

            var r30EEntity123Model = await http.GetEEntity123(rEEntity123Model.result.Model.EEntity123Id, userAdmin_Company_2);
            Assert.IsTrue(!r30EEntity123Model.result.IsSuccess);

            //Edit Item Test
            var r4EEntity123Model = await http.UpdateEEntity123(rEEntity123Model.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4EEntity123Model.result.IsSuccess);
            //Assert.IsTrue(r4EEntity123Model.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40EEntity123Model = await http.UpdateEEntity123(rEEntity123Model.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40EEntity123Model.result.IsSuccess);
            #endregion

            //%ParentCommentBegin%
            #region EEntity123 Association with XYZZYX

            var teentity123List0 = await http.GetEEntity123sByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(teentity123List0.result.IsSuccess);

            var teentity123AssociationR = await http.AssociateEEntity123WithXYZZYX(new EEntity123XYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, EEntity123Id = rEEntity123Model.result.Model.EEntity123Id }, userAdmin_Company_1);
            Assert.IsTrue(teentity123AssociationR.result.IsSuccess);

            teentity123List0 = await http.GetEEntity123sByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(teentity123List0.result.IsSuccess);
            Assert.IsTrue(teentity123List0.result.Model.Any(x => x.EEntity123Id == rEEntity123Model.result.Model.EEntity123Id));

            var teentity123UnAssociationR = await http.UnAssociateEEntity123WithXYZZYX(new EEntity123XYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, EEntity123Id = rEEntity123Model.result.Model.EEntity123Id }, userAdmin_Company_1);
            Assert.IsTrue(teentity123UnAssociationR.result.IsSuccess);
            teentity123List0 = await http.GetEEntity123sByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(teentity123List0.result.IsSuccess);
            Assert.IsTrue(!teentity123List0.result.Model.Any(x => x.EEntity123Id == rEEntity123Model.result.Model.EEntity123Id));

            #endregion
            //%ParentCommentEnd%
        }
    }
}