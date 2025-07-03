using Manifest.Api.All;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.TestUtilies;
using System;
using Manifest.Tests.Common;
using netDumbster.smtp;

namespace Manifest.Api.IntegrationTests
{
    public class BaseControllerTest : System.IDisposable
    {
        protected readonly FrameworkTestEnvironment<Startup> testEnv;
        protected string token = "";
        protected bool isTestInitilaized = false;
        protected readonly HttpService<Startup> http;
        private bool isDisposed;
        protected SimpleSmtpServer server;

        public BaseControllerTest()
        {
            this.testEnv = SetupAssemblyInitializer.GetTestEnv();
            http = new HttpService<Startup>(null, testEnv);
            this.server = SimpleSmtpServer.Start(8080);
        }

        protected (string, long) GetToken(string userName, string password)
        {
            var login = new LoginModel
            {
                Username = userName,
                Password = password
            };

            var result = http.Login(login);
            var r = result.Result;

            Assert.IsTrue(r.result.IsSuccess);
            return (r.result.Model.Token, r.result.Model.UserId);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                if (testEnv != null)
                {
                    testEnv.Dispose();
                }

                if (http != null)
                {
                    http.Dispose();
                }

                if (server != null)
                {
                    server.Stop();
                    server.Dispose();
                }
            }
            isDisposed = true;
        }
    }
}
