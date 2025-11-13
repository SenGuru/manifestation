using System.Reflection;
using Framework.Data.DbAccess;
using Framework.Data.Migrations;
using Framework.Infrastructure.Logging;

namespace Manifest.Repository.Migration
{
    public class AppDBMigration : DBMigration
    {
        public AppDBMigration(ILog log, IDBInfo dbInfo)
            : base(log, dbInfo)
        {
        }

        public override Assembly GetMigrationAssembly()
        {
            return this.GetType().Assembly;
        }
    }
}
