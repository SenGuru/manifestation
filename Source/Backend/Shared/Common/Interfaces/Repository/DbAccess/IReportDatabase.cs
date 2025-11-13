using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Manifest.Common.Interfaces.Repository.DbAccess
{
    public interface IReportDatabase
    {
        public DbConnection Connection { get; set; }

        public DbConnection CreateDbConnection(string dbPath, string dbFilename);

        public string GetSqliteTableCreationSQLFromPoco(Type type);

        public Task<List<string>> InsertPocoAsync<T>(List<T> pocoObj);

        public Task<string> InsertPocoAsync<T>(T pocoObj);

        public Task<bool> ExecuteQueryAsync(string qyery);

        public Task<bool> ExecuteQueryAsync(List<string> listQyery);
    }
}
