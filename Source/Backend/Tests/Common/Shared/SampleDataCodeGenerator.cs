using Manifest.Common.Enums;
using Framework.Infrastructure.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Manifest.Tests.Common.Shared
{
    public class SampleDataCodeGenerator
    {
        public async static Task Main<T>(HttpService<T> http, bool importFromFile = false, string filePath = null, string companyCode = null, string companyName = null)
        {
            Console.WriteLine("Starting the data creation process...");

            var loginResult = await http.Login(new LoginModel { Username = "super-admin", Password = "!test1234!" });
            Assert.IsNotNull(loginResult);
            Assert.IsNotNull(loginResult.result);
            Assert.IsTrue(loginResult.result.IsSuccess);
            var token = loginResult.result.Model.Token;

            var state = await http.GetStates(new StateSearchCriteria { Keyword = "Tamil Nadu" }, token: token);
            Assert.IsTrue(state.result.IsSuccess);
            Assert.IsTrue(state.result.Model.Count > 0);

            var mail = await http.GetMailServers(new MailServerSearchCriteria { Page = 1, PageSize = int.MaxValue }, token: token);
            Assert.IsTrue(mail.result.IsSuccess);
            Assert.IsTrue(mail.result.Model.Count > 0);

            var singleSignOnServer = await http.GetSingleSignOnServers(new SingleSignOnServerSearchCriteria { Page = 1, PageSize = int.MaxValue }, token: token);
            Assert.IsTrue(singleSignOnServer.result.IsSuccess);
            Assert.IsTrue(singleSignOnServer.result.Model.Count > 0);

            if (importFromFile)
            {
                if (filePath.IsNullOrEmpty())
                {
                    Assert.Fail("File Path is not specified in the Import From File Option");
                }

                await ImportDataWithFileAsync(http, token, filePath, companyCode ?? "", companyName ?? "", state.result.Model[0], mail.result.Model[0], singleSignOnServer.result.Model[0]);
            }
        }

        static async Task ImportDataWithFileAsync<T>(HttpService<T> http, string token, string filePath, string companyCode, string companyName, StateModel state, MailServerModel mail, SingleSignOnServerModel singleSignOnServer)
        {
            Assert.IsTrue(File.Exists(filePath));
            using (var uFs = new Framework.Infrastructure.Models.FileInput.FileStreamModel(filePath))
            {
                Console.WriteLine($"Company Code {companyCode}");
            }
        }
    }
}
