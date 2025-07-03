using Framework.TestUtilies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Manifest.Api.All;

namespace Manifest.Api.IntegrationTests
{
    [TestClass]
    public static class SetupAssemblyInitializer
    {
        static FrameworkTestEnvironment<Startup> testEnv;

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            testEnv = new FrameworkTestEnvironment<Startup>(useSingletonServer: true);
        }

        public static FrameworkTestEnvironment<Startup> GetTestEnv()
        {
            return testEnv;
        }
    }
}
