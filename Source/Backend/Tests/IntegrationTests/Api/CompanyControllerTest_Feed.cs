using System.Linq;
using System.Threading.Tasks;
using Manifest.Tests.Common;
using LinqToDB.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Api.IntegrationTests
{
    public partial class AllIntegrationTest
    {
        public async Task FeedTest_IntegrationTest()
        {
            #region Feed
            //Tests for Feed
            //Create Item Test

            var aFeedModel = new FeedCreateModel();
            var rFeedModel = await http.CreateFeed(aFeedModel, userAdmin_Company_1);
            Assert.IsTrue(rFeedModel.result.IsSuccess);

            aFeedModel = new FeedCreateModel();
            var r0FeedModel = await http.CreateFeed(aFeedModel, userAdmin_Company_1);
            Assert.IsTrue(!r0FeedModel.result.IsSuccess);

            aFeedModel = new FeedCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1.1");
            var r00FeedModel = await http.CreateFeed(aFeedModel, userAdmin_Company_1);
            Assert.IsTrue(r00FeedModel.result.IsSuccess);

            aFeedModel = new FeedCreateModel();// rXYZZYXModel.result.Model.XYZZYXId, "Item 1");
            var r01FeedModel = await http.CreateFeed(aFeedModel, userAdmin_Company_2);
            Assert.IsTrue(!r01FeedModel.result.IsSuccess);


            aFeedModel = new FeedCreateModel();// r01XYZZYXModel.result.Model.XYZZYXId, "Item 1");
            r01FeedModel = await http.CreateFeed(aFeedModel, userAdmin_Company_2);
            Assert.IsTrue(r01FeedModel.result.IsSuccess);

            //Get List Test
            var r2FeedModel = await http.GetFeeds(new FeedSearchCriteria(), userAdmin_Company_1);
            Assert.IsTrue(r2FeedModel.result.IsSuccess);
            Assert.IsTrue(r2FeedModel.result.Model.Any(x => x.FeedId == rFeedModel.result.Model.FeedId));

            var r20FeedModel = await http.GetFeeds(new FeedSearchCriteria(), userAdmin_Company_2);
            Assert.IsTrue(!r20FeedModel.result.Model.Any(x => x.FeedId == rFeedModel.result.Model.FeedId));

            //Get Item Test
            var r3FeedModel = await http.GetFeed(rFeedModel.result.Model.FeedId, userAdmin_Company_1);
            Assert.IsTrue(r3FeedModel.result.IsSuccess);
            Assert.IsTrue(r3FeedModel.result.Model.FeedId == rFeedModel.result.Model.FeedId);

            var r30FeedModel = await http.GetFeed(rFeedModel.result.Model.FeedId, userAdmin_Company_2);
            Assert.IsTrue(!r30FeedModel.result.IsSuccess);

            //Edit Item Test
            var r4FeedModel = await http.UpdateFeed(rFeedModel.result.Model.GetAsUpdateModel(), userAdmin_Company_1);
            Assert.IsTrue(r4FeedModel.result.IsSuccess);
            //Assert.IsTrue(r4FeedModel.result.Model.ModifiedDate > r.result.Model.ModifiedDate);

            var r40FeedModel = await http.UpdateFeed(rFeedModel.result.Model.GetAsUpdateModel(), userAdmin_Company_2);
            Assert.IsTrue(!r40FeedModel.result.IsSuccess);
            #endregion

            /*
            #region Feed Association with XYZZYX

            var tfeedList0 = await http.GetFeedsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tfeedList0.result.IsSuccess);

            var tfeedAssociationR = await http.AssociateFeedWithXYZZYX(new FeedXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, FeedId = rFeedModel.result.Model.FeedId }, userAdmin_Company_1);
            Assert.IsTrue(tfeedAssociationR.result.IsSuccess);

            tfeedList0 = await http.GetFeedsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tfeedList0.result.IsSuccess);
            Assert.IsTrue(tfeedList0.result.Model.Any(x => x.FeedId == rFeedModel.result.Model.FeedId));

            var tfeedUnAssociationR = await http.UnAssociateFeedWithXYZZYX(new FeedXYZZYXAssociationModel { XYZZYXId = rXYZZYXModel.result.Model.XYZZYXId, FeedId = rFeedModel.result.Model.FeedId }, userAdmin_Company_1);
            Assert.IsTrue(tfeedUnAssociationR.result.IsSuccess);
            tfeedList0 = await http.GetFeedsByXYZZYX(rXYZZYXModel.result.Model.XYZZYXId, userAdmin_Company_1);
            Assert.IsTrue(tfeedList0.result.IsSuccess);
            Assert.IsTrue(!tfeedList0.result.Model.Any(x => x.FeedId == rFeedModel.result.Model.FeedId));

            #endregion
            */
        }
    }
}