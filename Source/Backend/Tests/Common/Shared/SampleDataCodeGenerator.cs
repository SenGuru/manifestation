using Manifest.Common.Enums;
using Framework.Infrastructure.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Tests.Common.Shared
{
    public class SampleDataCodeGenerator
    {
        public async static Task Main<T>(HttpService<T> http)
        {
            // Step 1: Login as super-admin
            var loginResult = await http.Login(new LoginModel { Username = "super-admin", Password = "!test1234!" });
            Assert.IsNotNull(loginResult);
            Assert.IsNotNull(loginResult.result);
            Assert.IsTrue(loginResult.result.IsSuccess);
            var token = loginResult.result.Model.Token;

            // Step 2: Create a new account
            var accountModel = new AccountCreateModel();
            accountModel.Email = "test@test.com";
            accountModel.FirstName = "TestFN";
            accountModel.LastName = "TestLN";
            accountModel.InitialPassword = "!test1234!";
            var createAccountResult = await http.CreateAccount(accountModel, token: token);
            Assert.IsNotNull(createAccountResult);
            Assert.IsNotNull(createAccountResult.result);
            Assert.IsTrue(createAccountResult.result.IsSuccess);
            var userId = createAccountResult.result.Model.UserId;

            // Step 3: Check if user has categories
            var categoriesResult = await http.GetCategoryByUserId(userId, token: token);

            Assert.IsNotNull(categoriesResult);
            Assert.IsTrue(categoriesResult.result.IsSuccess);
            Assert.IsTrue(categoriesResult.result.Model.Count > 0, "User should have at least one category");

            Assert.IsTrue(categoriesResult.result.Model.All(c => c.UserId == userId), "All categories should have the same UserId as the created user");

            var affirmationsResult = await http.GetDailyAffirmationTextByUserId(userId, token: token);
            Assert.IsNotNull(affirmationsResult);
            Assert.IsTrue(affirmationsResult.result.IsSuccess);
            Assert.IsTrue(affirmationsResult.result.Model.Count > 0, "User should have at least one affirmation text");

            Assert.IsTrue(affirmationsResult.result.Model.All(a => a.UserId == userId), "All Daily affimations should have the same UserId as the created user");
        }
    }
}
