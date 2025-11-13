using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task DailyJournalTest_IntegrationTest()
        {
            #region DailyJournal
            //Tests for DailyJournal
            //Create Item Test

            var aDailyJournalModel = new DailyJournalCreateModel();
            var rDailyJournalModel = await http.CreateDailyJournal(aDailyJournalModel, userAdmin_Company_1);
            Assert.IsTrue(rDailyJournalModel.result.IsSuccess);

            aDailyJournalModel = new DailyJournalCreateModel();
            var r0DailyJournalModel = await http.CreateDailyJournal(aDailyJournalModel, userAdmin_Company_1);
            Assert.IsTrue(!r0DailyJournalModel.result.IsSuccess);

            aDailyJournalModel = new DailyJournalCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00DailyJournalModel = await http.CreateDailyJournal(aDailyJournalModel, userAdmin_Company_1);
            Assert.IsTrue(r00DailyJournalModel.result.IsSuccess);

            aDailyJournalModel = new DailyJournalCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01DailyJournalModel = await http.CreateDailyJournal(aDailyJournalModel, userAdmin_Company_2);
            Assert.IsTrue(!r01DailyJournalModel.result.IsSuccess);


            aDailyJournalModel = new DailyJournalCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01DailyJournalModel = await http.CreateDailyJournal(aDailyJournalModel, userAdmin_Company_2);
            Assert.IsTrue(r01DailyJournalModel.result.IsSuccess);

            //Get List Test
            var r2DailyJournalModel = await http.GetDailyJournals(new DailyJournalSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2DailyJournalModel.result.IsSuccess);
            Assert.IsTrue(r2DailyJournalModel.result.Model.Any(x => x.DailyJournalId == rDailyJournalModel.result.Model.DailyJournalId));

            var r20DailyJournalModel = await http.GetDailyJournals(new DailyJournalSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20DailyJournalModel.result.Model.Any(x => x.DailyJournalId == rDailyJournalModel.result.Model.DailyJournalId));

            //Get Item Test
            var r3DailyJournalModel = await http.GetDailyJournal(rDailyJournalModel.result.Model.DailyJournalId, userAdmin_Company_1);
            Assert.IsTrue(r3DailyJournalModel.result.IsSuccess);
            Assert.IsTrue(r3DailyJournalModel.result.Model.DailyJournalId == rDailyJournalModel.result.Model.DailyJournalId);

            var r30DailyJournalModel = await http.GetDailyJournal(rDailyJournalModel.result.Model.DailyJournalId, userAdmin_Company_2);
            Assert.IsTrue(!r30DailyJournalModel.result.IsSuccess);

            //Edit Item Test
            var r4DailyJournalModel = await http.UpdateDailyJournal(rDailyJournalModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4DailyJournalModel.result.IsSuccess);
            //Assert.IsTrue(r4DailyJournalModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40DailyJournalModel = await http.UpdateDailyJournal(rDailyJournalModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40DailyJournalModel.result.IsSuccess);
            #endregion

            /*
            #region DailyJournal Association with XYZZYX

            var tdailyJournalList0 = await http.GetDailyJournalsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdailyJournalList0.result.IsSuccess);

            var tdailyJournalAssociationR = await http.AssociateDailyJournalWithXYZZYX(new DailyJournalXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DailyJournalId = rDailyJournalModel.result.Model.DailyJournalId }, userAdmin_Company_1);
            Assert.IsTrue(tdailyJournalAssociationR.result.IsSuccess);

            tdailyJournalList0 = await http.GetDailyJournalsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdailyJournalList0.result.IsSuccess);
            Assert.IsTrue(tdailyJournalList0.result.Model.Any(x => x.DailyJournalId == rDailyJournalModel.result.Model.DailyJournalId));

            var tdailyJournalUnAssociationR = await http.UnAssociateDailyJournalWithXYZZYX(new DailyJournalXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, DailyJournalId = rDailyJournalModel.result.Model.DailyJournalId }, userAdmin_Company_1);
            Assert.IsTrue(tdailyJournalUnAssociationR.result.IsSuccess);
            tdailyJournalList0 = await http.GetDailyJournalsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tdailyJournalList0.result.IsSuccess);
            Assert.IsTrue(!tdailyJournalList0.result.Model.Any(x => x.DailyJournalId == rDailyJournalModel.result.Model.DailyJournalId));

            #endregion
            */
        }
    }
}