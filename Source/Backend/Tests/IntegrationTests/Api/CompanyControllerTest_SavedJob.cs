using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task SavedJobTest_IntegrationTest()
        {
            #region SavedJob
            //Tests for SavedJob
            //Create Item Test

            var aSavedJobModel = new SavedJobCreateModel();
            var rSavedJobModel = await http.CreateSavedJob(aSavedJobModel, userAdmin_Company_1);
            Assert.IsTrue(rSavedJobModel.result.IsSuccess);

            aSavedJobModel = new SavedJobCreateModel();
            var r0SavedJobModel = await http.CreateSavedJob(aSavedJobModel, userAdmin_Company_1);
            Assert.IsTrue(!r0SavedJobModel.result.IsSuccess);

            aSavedJobModel = new SavedJobCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00SavedJobModel = await http.CreateSavedJob(aSavedJobModel, userAdmin_Company_1);
            Assert.IsTrue(r00SavedJobModel.result.IsSuccess);

            aSavedJobModel = new SavedJobCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01SavedJobModel = await http.CreateSavedJob(aSavedJobModel, userAdmin_Company_2);
            Assert.IsTrue(!r01SavedJobModel.result.IsSuccess);


            aSavedJobModel = new SavedJobCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01SavedJobModel = await http.CreateSavedJob(aSavedJobModel, userAdmin_Company_2);
            Assert.IsTrue(r01SavedJobModel.result.IsSuccess);

            //Get List Test
            var r2SavedJobModel = await http.GetSavedJobs(new SavedJobSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2SavedJobModel.result.IsSuccess);
            Assert.IsTrue(r2SavedJobModel.result.Model.Any(x => x.SavedJobId == rSavedJobModel.result.Model.SavedJobId));

            var r20SavedJobModel = await http.GetSavedJobs(new SavedJobSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20SavedJobModel.result.Model.Any(x => x.SavedJobId == rSavedJobModel.result.Model.SavedJobId));

            //Get Item Test
            var r3SavedJobModel = await http.GetSavedJob(rSavedJobModel.result.Model.SavedJobId, userAdmin_Company_1);
            Assert.IsTrue(r3SavedJobModel.result.IsSuccess);
            Assert.IsTrue(r3SavedJobModel.result.Model.SavedJobId == rSavedJobModel.result.Model.SavedJobId);

            var r30SavedJobModel = await http.GetSavedJob(rSavedJobModel.result.Model.SavedJobId, userAdmin_Company_2);
            Assert.IsTrue(!r30SavedJobModel.result.IsSuccess);

            //Edit Item Test
            var r4SavedJobModel = await http.UpdateSavedJob(rSavedJobModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4SavedJobModel.result.IsSuccess);
            //Assert.IsTrue(r4SavedJobModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40SavedJobModel = await http.UpdateSavedJob(rSavedJobModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40SavedJobModel.result.IsSuccess);
            #endregion

            /*
            #region SavedJob Association with XYZZYX

            var tsavedJobList0 = await http.GetSavedJobsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tsavedJobList0.result.IsSuccess);

            var tsavedJobAssociationR = await http.AssociateSavedJobWithXYZZYX(new SavedJobXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, SavedJobId = rSavedJobModel.result.Model.SavedJobId }, userAdmin_Company_1);
            Assert.IsTrue(tsavedJobAssociationR.result.IsSuccess);

            tsavedJobList0 = await http.GetSavedJobsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tsavedJobList0.result.IsSuccess);
            Assert.IsTrue(tsavedJobList0.result.Model.Any(x => x.SavedJobId == rSavedJobModel.result.Model.SavedJobId));

            var tsavedJobUnAssociationR = await http.UnAssociateSavedJobWithXYZZYX(new SavedJobXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, SavedJobId = rSavedJobModel.result.Model.SavedJobId }, userAdmin_Company_1);
            Assert.IsTrue(tsavedJobUnAssociationR.result.IsSuccess);
            tsavedJobList0 = await http.GetSavedJobsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tsavedJobList0.result.IsSuccess);
            Assert.IsTrue(!tsavedJobList0.result.Model.Any(x => x.SavedJobId == rSavedJobModel.result.Model.SavedJobId));

            #endregion
            */
        }
    }
}