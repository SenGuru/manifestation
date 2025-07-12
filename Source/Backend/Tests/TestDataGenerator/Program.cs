using Manifest.Tests.Common;
using Manifest.Tests.Common.Shared;
using Framework.Infrastructure.Utils;
using Framework.TestUtilies;

namespace Manifest.Tests.TestDataGenerator
{
    public static class Program
    {
        public async static Task Main(string[] args)
        {
            Console.WriteLine("Starting the data creation process...");

            var filePath = "";
            var importFromFile = false;
            var companyCode = "";
            var companyName = "";
            if (args.IsParamAvailable("--company-name"))
            {
                companyName = args.GetParamValueAsString("--company-name");
            }
            else
            {
                companyName = "Madras Talkies";
            }

            if (args.IsParamAvailable("--import-from-file"))
            {
                importFromFile = args.GetParamValueAs<bool>("--import-from-file", false);
                if (importFromFile)
                {
                    if (args.IsParamAvailable("--company-code"))
                    {
                        companyCode = args.GetParamValueAsString("--company-code");
                    }

                    filePath = FileUtils.GetApplicationExeDirectory() + "/../../../../Data/" + "ImportCallSheetDataFile.json";
                    if (args.IsParamAvailable("--filepath"))
                    {
                        var val = args.GetParamValueAsString("--filepath");
                        if (File.Exists(val))
                        {
                            filePath = val;
                        }
                    }
                    else if (args.IsParamAvailable("--company-name"))
                    {
                        filePath = FileUtils.GetApplicationExeDirectory() + "/../../../../Data/" + companyName.RemoveWhitespace() + ".json";
                    }

                    Console.WriteLine("Company Name: " + companyName);

                    Console.WriteLine("File path: " + filePath);
                }
            }

            var server = "http://localhost:9009";

            if (args.IsParamAvailable("--server"))
            {
                server = args.GetParamValueAs<string>("--server", "http://localhost:9009");

                if (server.IsNullOrEmpty())
                    server = "http://localhost:9009";
            }
            Console.WriteLine($"Server - {server}");

            var env = new FrameworkTestEnvironment<string>(false, true);
            var http = new HttpService<string>(server, env);
            await SampleDataCodeGenerator.Main(http);
        }

    }
}