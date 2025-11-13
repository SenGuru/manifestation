using System;
using Framework.Infrastructure.DI;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;
using Manifest.DI;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Manifest.Service.App;
using System.Diagnostics;
using System.Linq;
using Framework.Data.Migrations;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Threading.Tasks;
using SecurityAssessment.Service.DataUpdate;

namespace Manifest.Api.All
{
    public static class Program
    {
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(o =>
                {
                    o.AllowSynchronousIO = true;
                    var appServices = o.ApplicationServices;
                    o.ConfigureHttpsDefaults(h =>
                    {
                        h.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                        h.UseLettuceEncrypt(appServices);

                    });
                })
                .UseIIS()
                .UseIISIntegration()
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = true; //context.HostingEnvironment.IsDevelopment();
                    options.ValidateOnBuild = true;
                })
                .UseStartup<Startup>();

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Starting the App....");

            bool isMigration = args.Contains("--migrate");
            bool isDataUpdate = args.Contains("--data-update");

            if (isMigration)
            {

                Console.WriteLine("DB Migration in progress...");
                DISetup.Environment = "Testing";
                var migration = DISetup.ServiceProvider.GetService<IDBMigration>();

                if (!migration.IsMigrationUptoDate(migration.GetMigrationAssembly()))
                {
                    migration.MigrateToLatestVersion(migration.GetMigrationAssembly());
                }
                Console.WriteLine("DB Migration Completed.");

                return;
            }

            if (isDataUpdate)
            {
                Console.WriteLine("Data Update in progress...");
                DISetup.Environment = "Testing";
                var updater = DISetup.ServiceProvider.GetService<DataUpdateService>();

                await updater.RunUpdaterAsync();

                Console.WriteLine("Data Update Completed.");

                return;
            }

            Console.WriteLine("Main starting ...");
            var hostBuilder = CreateWebHostBuilder(args).UseKestrel().UseIIS().UseIISIntegration();

            hostBuilder = hostBuilder.UseUrls($"http://0.0.0.0:{ConfigUtils.GetEnvironmentVariableOrDefault(args, "API_PORT", 9009)}", $"https://0.0.0.0:{ConfigUtils.GetEnvironmentVariableOrDefault(args, "SSL_API_PORT", 9010)}");

            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var log = DISetup.ServiceProvider.GetService<ILog>();
            log.Info($" baseDir = {baseDir}");

            hostBuilder = hostBuilder.UseContentRoot(baseDir);

            var host = hostBuilder.Build();

            bool isService = args.Contains("--service");

            if (isService)
            {
                host.RunAsService();
            }
            else
            {
                await host.RunAsync();
            }
        }
    }
}
