using Manifest.Tests.Common.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Manifest.Api.IntegrationTests
{
    [TestClass]
    public partial class AllIntegrationTest : BaseControllerTest
    {
        private readonly string superAdminToken;
        private string userAdmin_Company_1;
        //private string user_Company_1;
        private string userAdmin_Company_2;
        //private string user_Company_2;

        public AllIntegrationTest() : base()
        {
            (superAdminToken, _) = GetToken("super-admin", "!test1234!");
        }

        [TestMethod]
        public async Task DataGenTest()
        {
            await SampleDataCodeGenerator.Main(http);
        }

        [TestMethod]
        public async Task EndToEndIntegrationTest()
        {
            Assert.IsNotNull(superAdminToken);

            (userAdmin_Company_1, _) = GetToken("marketing", "!test1234!");
            //(user_Company_1, _) = GetToken("sales", "!test1234!");
            (userAdmin_Company_2, _) = GetToken("marketing", "!test1234!");
            //(user_Company_2, _) = GetToken("sales", "!test1234!");

            Assert.IsNotNull(userAdmin_Company_1);
            //Assert.IsNotNull(user_Company_1);
            Assert.IsNotNull(userAdmin_Company_2);
            //Assert.IsNotNull(user_Company_2);

            await AuditOperationTest_IntegrationTest();

            await UserPermissionTest_IntegrationTest();

        }
    }
}