#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
using Framework.Data.Migrations;
using Manifest.DI;
using Microsoft.Extensions.DependencyInjection;

namespace DBMigrator
{
    public static class Program
    {
        public static int Main()
        {
            var serviceProvider = new ServiceCollection()
                .AddApplicationDI()
                .BuildServiceProvider();

            var migration = serviceProvider.GetRequiredService<IDBMigration>();

            if (!migration.IsMigrationUptoDate())
            {
                migration.MigrateToLatestVersion();
            }

            return 0;
        }
    }
}
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'