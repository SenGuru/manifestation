using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure.Utils;

namespace Manifest.Service.Report
{
    public class IntermediateDBRepository
    {
        public async Task CreateReportDbAndItemsAsync(string dbPath, string dbFilename, List<Type> typeItems)
        {
            if (!Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
            }

            var path = FileUtils.Combine(dbPath, dbFilename);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            var connectionString = "Data source=" + path;
            foreach (var t in typeItems)
            {
                var tableSql = GetSqliteTableCreationSQLFromPoco(t);
                await ExecuteQueryAsync(connectionString, tableSql);
            }
        }

        public async Task CreateReportDbAndItemsAsync<TA, TB, TC, TD, TE, TF, TG, TH, TI>(string dbPath, string dbFilename, List<TA> aItems, List<TB> bItems, List<TC> cItems, List<TD> dItems, List<TE> eItems, List<TF> fItems, List<TG> gItems, List<TH> hItems, List<TI> iItems)
        {
            var typeItems = new List<Type> { typeof(TA), typeof(TB), typeof(TC), typeof(TD), typeof(TE), typeof(TF), typeof(TG), typeof(TH), typeof(TI) };
            await CreateReportDbAndItemsAsync(dbPath, dbFilename, typeItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, aItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, bItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, cItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, dItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, eItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, fItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, gItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, hItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, iItems);
        }

        public async Task CreateReportDbAndItemsAsync<TA, TB, TC, TD, TE, TF, TG, TH>(string dbPath, string dbFilename, List<TA> aItems, List<TB> bItems, List<TC> cItems, List<TD> dItems, List<TE> eItems, List<TF> fItems, List<TG> gItems, List<TH> hItems)
        {
            var typeItems = new List<Type> { typeof(TA), typeof(TB), typeof(TC), typeof(TD), typeof(TE), typeof(TF), typeof(TG), typeof(TH) };
            await CreateReportDbAndItemsAsync(dbPath, dbFilename, typeItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, aItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, bItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, cItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, dItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, eItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, fItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, gItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, hItems);
        }

        public async Task CreateReportDbAndItemsAsync<TA, TB, TC, TD, TE, TF, TG>(string dbPath, string dbFilename, List<TA> aItems, List<TB> bItems, List<TC> cItems, List<TD> dItems, List<TE> eItems, List<TF> fItems, List<TG> gItems)
        {
            var typeItems = new List<Type> { typeof(TA), typeof(TB), typeof(TC), typeof(TD), typeof(TE), typeof(TF), typeof(TG) };
            await CreateReportDbAndItemsAsync(dbPath, dbFilename, typeItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, aItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, bItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, cItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, dItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, eItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, fItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, gItems);
        }

        public async Task CreateReportDbAndItemsAsync<TA, TB, TC, TD, TE, TF>(string dbPath, string dbFilename, List<TA> aItems, List<TB> bItems, List<TC> cItems, List<TD> dItems, List<TE> eItems, List<TF> fItems)
        {
            var typeItems = new List<Type> { typeof(TA), typeof(TB), typeof(TC), typeof(TD), typeof(TE), typeof(TF) };
            await CreateReportDbAndItemsAsync(dbPath, dbFilename, typeItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, aItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, bItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, cItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, dItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, eItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, fItems);
        }

        public async Task CreateReportDbAndItemsAsync<TA, TB, TC, TD, TE>(string dbPath, string dbFilename, List<TA> aItems, List<TB> bItems, List<TC> cItems, List<TD> dItems, List<TE> eItems)
        {
            var typeItems = new List<Type> { typeof(TA), typeof(TB), typeof(TC), typeof(TD), typeof(TE) };
            await CreateReportDbAndItemsAsync(dbPath, dbFilename, typeItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, aItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, bItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, cItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, dItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, eItems);
        }

        public async Task CreateReportDbAndItemsAsync<TA, TB, TC, TD>(string dbPath, string dbFilename, List<TA> aItems, List<TB> bItems, List<TC> cItems, List<TD> dItems)
        {
            var typeItems = new List<Type> { typeof(TA), typeof(TB), typeof(TC), typeof(TD) };
            await CreateReportDbAndItemsAsync(dbPath, dbFilename, typeItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, aItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, bItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, cItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, dItems);
        }

        public async Task CreateReportDbAndItemsAsync<TA, TB, TC>(string dbPath, string dbFilename, List<TA> aItems, List<TB> bItems, List<TC> cItems)
        {
            var typeItems = new List<Type> { typeof(TA), typeof(TB), typeof(TC) };
            await CreateReportDbAndItemsAsync(dbPath, dbFilename, typeItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, aItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, bItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, cItems);
        }

        public async Task CreateReportDbAndItemsAsync<TA, TB>(string dbPath, string dbFilename, List<TA> aItems, List<TB> bItems)
        {
            var typeItems = new List<Type> { typeof(TA), typeof(TB) };
            await CreateReportDbAndItemsAsync(dbPath, dbFilename, typeItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, aItems);
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, bItems);
        }

        public async Task CreateReportDbAndItemsAsync<TA>(string dbPath, string dbFilename, List<TA> aItems)
        {
            await CreateReportDbAndItemsAsync(dbPath, dbFilename, new List<Type> { typeof(TA) });
            await AddItemsToExistingReportDbAsync(dbPath, dbFilename, aItems);
        }

        public Task AddItemsToExistingReportDbAsync<T>(string dbPath, string dbFilename, List<T> dbItems)
        {
            var path = FileUtils.Combine(dbPath, dbFilename);
            var connectionString = "Data source=" + path;

            var insertSqls = GetInsertPocoSqls(dbItems);
            return ExecuteQueryAsync(connectionString, insertSqls);
        }

        protected string GetSqliteTableCreationSQLFromPoco(Type type)
        {
            try
            {
                var sql = new StringBuilder();
                int start = 0;
                sql.Append($"Create Table if not exists {type.Name}");
                foreach (var prp in type.GetProperties())
                {
                    if (start == 0)
                    {
                        start++;
                        sql.Append($" ( ");
                    }

                    if (prp.Name.Contains("Date"))
                        sql.Append($" {prp.Name} Date ,");
                    else if (prp.PropertyType == typeof(string))
                        sql.Append($" {prp.Name} TEXT ,");
                    else if (prp.PropertyType == typeof(long) || prp.PropertyType == typeof(int))
                        sql.Append($" {prp.Name} INTEGER ,");
                    else if (prp.PropertyType == typeof(long?) || prp.PropertyType == typeof(int?))
                        sql.Append($" {prp.Name} INTEGER NULL,");
                    else if (prp.PropertyType == typeof(double))
                        sql.Append($" {prp.Name} DOUBLE PRECISION ,");
                    else if (prp.PropertyType == typeof(double?))
                        sql.Append($" {prp.Name} DOUBLE PRECISION NULL ,");
                    else if (prp.PropertyType == typeof(bool))
                        sql.Append($" {prp.Name} BOOLEAN ,");
                    else if (prp.PropertyType == typeof(bool?))
                        sql.Append($" {prp.Name} BOOLEAN NULL,");
                    else if (prp.PropertyType == typeof(Guid))
                        sql.Append($" {prp.Name} TEXT ,");
                    else if (prp.PropertyType == typeof(Guid?))
                        sql.Append($" {prp.Name} TEXT NULL,");
                    else if (prp.PropertyType == typeof(byte[]))
                        sql.Append($" {prp.Name} BLOB ,");
                    else if (prp.PropertyType == typeof(DateTime) || prp.PropertyType == typeof(DateTimeOffset))
                        sql.Append($" {prp.Name} DATE ,");
                    else if (prp.PropertyType == typeof(DateTime?) || prp.PropertyType == typeof(DateTimeOffset?))
                        sql.Append($" {prp.Name} DATE NULL,");
                    else
                        throw new Exception($"{prp.Name} of type - {prp.PropertyType} is not supported");
                }

                string str = sql.ToString().TrimEnd().RemoveLastChar(',') + " ) ";
                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("table creation in sqlite exception ", ex);
            }
        }

        protected List<string> GetInsertPocoSqls<T>(List<T> pocoObj)
        {
            try
            {
                var lstQuery = new List<string>();
                if (pocoObj != null && pocoObj.Count > 0)
                {
                    foreach (var item in pocoObj)
                    {
                        lstQuery.Add(GetSingleInsertPocoSQL(item));
                    }
                }

                return lstQuery;
            }
            catch (Exception ex)
            {
                throw new Exception("table creation in sqlite exception ", ex);
            }
        }

        protected string GetSingleInsertPocoSQL<T>(T pocoObj)
        {
            try
            {
                string result = null;
                StringBuilder query = null;
                var sql = new StringBuilder();
                if (pocoObj != null)
                {
                    var t = typeof(T);
                    sql.Append($"INSERT INTO {t.Name} ( ");
                    foreach (var prp in t.GetProperties())
                    {
                        if (!prp.PropertyType.Name.Contains("Collection"))
                            sql.Append($"{prp.Name},");
                    }

                    string strinsert = sql.ToString().TrimEnd().RemoveLastChar(',') + " ) VALUES (";
                    query = new StringBuilder();
                    query.Append(strinsert);
                    foreach (var prp in pocoObj.GetType().GetProperties())
                    {
                        var propValue = prp.GetValue(pocoObj);
                        if (propValue == null)
                            continue;

                        if (prp.Name.Contains("Date") || prp.PropertyType == typeof(DateTime) || prp.PropertyType == typeof(DateTime?))
                            query.Append($"\'{propValue.ToString()}\' ,");
                        else if (prp.PropertyType == typeof(string))
                            query.Append($"\'{propValue}\' ,");
                        else if (prp.PropertyType == typeof(long) || prp.PropertyType == typeof(long?))
                            query.Append($"{propValue} ,");
                        else if (prp.PropertyType == typeof(int) || prp.PropertyType == typeof(int?))
                            query.Append($"{propValue} ,");
                        else if (prp.PropertyType == typeof(double) || prp.PropertyType == typeof(double?))
                            query.Append($"{propValue} ,");
                        else if (prp.PropertyType == typeof(bool) || prp.PropertyType == typeof(bool?))
                            query.Append($"{propValue} ,");
                        else if (prp.PropertyType == typeof(Guid) || prp.PropertyType == typeof(Guid?))
                            query.Append($"\'{propValue}\' ,");
                        else
                            throw new Exception($"{prp.Name} of type - {prp.PropertyType} is not supported");
                    }

                    result = query.ToString().TrimEnd().RemoveLastChar(',') + " ) ";
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("table creation in sqlite exception ", ex);
            }
        }

        protected Task<bool> ExecuteQueryAsync(string connectionString, string qry)
        {
            return ExecuteQueryAsync(connectionString, new List<string> { qry });
        }

        protected async Task<bool> ExecuteQueryAsync(string connectionString, List<string> qryList)
        {
            bool result = false;
            using (DbConnection cn = new SQLiteConnection(connectionString))
            {
                await cn.OpenAsync();
                using (DbCommand cmd = new SQLiteCommand())
                {
                    using (var transaction = await cn.BeginTransactionAsync())
                    {
                        cmd.Connection = cn;
                        cmd.Transaction = transaction;
                        foreach (var query in qryList)
                        {
#pragma warning disable CA2100
                            cmd.CommandText = query;
#pragma warning restore CA2100
                            await cmd.ExecuteNonQueryAsync();
                        }

                        await transaction.CommitAsync();
                        result = true;
                    }
                }

                await cn.CloseAsync();
            }

            return result;
        }
    }
}