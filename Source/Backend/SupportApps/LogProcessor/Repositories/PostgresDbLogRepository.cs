using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Data.Utils;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search.LogSearch;
using Framework.Infrastructure.Utils;
using LiteDB;
using LogProcessor.Models;
using Marten;
using Npgsql;

public class PostgresDbLogRepository : ILogRepository
{
    private readonly string PostgresDbConnectionString;


    public PostgresDbLogRepository(ConfigurationService configuration)
    {
        this.PostgresDbConnectionString = configuration.PostgresDbConnectionString;
    }

    public DocumentStore GetConnection()
    {
        EnsureDatabaseExists();
        return DocumentStore.For(opts =>
        {
            opts.Connection(PostgresDbConnectionString);
            opts.Schema.For<AppLogDataModel>().FullTextIndex();
            opts.Schema.For<AppNameModel>().FullTextIndex();
            opts.Schema.For<FileNameModel>().FullTextIndex();
            opts.Schema.For<FunctionTypeModel>().FullTextIndex();
            opts.Schema.For<HttpVerbModel>().FullTextIndex();
            opts.Schema.For<MachineNameModel>().FullTextIndex();
            opts.Schema.For<SeverityTypeModel>().FullTextIndex();
            opts.Schema.For<StatusCodeModel>().FullTextIndex();
        });
    }

    private void EnsureDatabaseExists()
    {
        var builder = new NpgsqlConnectionStringBuilder(PostgresDbConnectionString);
        var databaseName = builder.Database;
        builder.Database = "postgres"; // Connect to the default 'postgres' database

        using (var connection = new NpgsqlConnection(builder.ToString()))
        {
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = $"SELECT 1 FROM pg_database WHERE datname = '{databaseName}'";
                var result = cmd.ExecuteScalar();

                if (result == null)
                {
                    // Database doesn't exist, so create it
                    cmd.CommandText = $"CREATE DATABASE {databaseName}";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public ReturnListModel<AppLogDataModel> GetAppLogs(LogSearchCriteria logSearchCriteria)
    {
        using (var store = GetConnection())
        {
            using (var logDb = store.LightweightSession())
            {
                if (logSearchCriteria == null)
                {
                    logSearchCriteria = new LogSearchCriteria();
                }

                var qry = logDb.Query<AppLogDataModel>().AsQueryable();

                if (!logSearchCriteria.Keyword.IsTrimmedStringNullOrEmpty())
                {
                    qry = qry.Where(x =>
                        x.MachineName.Trim().ToUpper().Contains(logSearchCriteria.Keyword.Trim().ToUpper())
                        || x.CurrentFunction.Trim().ToUpper().Contains(logSearchCriteria.Keyword.Trim().ToUpper())
                        || x.AppName.Trim().ToUpper().Contains(logSearchCriteria.Keyword.Trim().ToUpper())
                        || x.Message.Trim().ToUpper().Contains(logSearchCriteria.Keyword.Trim().ToUpper())
                        || x.CurrentSourceFileName.Trim().ToUpper().Contains(logSearchCriteria.Keyword.Trim().ToUpper()));
                }

                if (logSearchCriteria.ActivityIds.IsNotNullOrEmpty())
                {
                    qry = qry.Where(item => item.ActivityId != null && logSearchCriteria.ActivityIds.Select(x => x.Trim().ToLower()).Contains(item.ActivityId.Trim().ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(logSearchCriteria.MachineName))
                {
                    qry = qry.Where(item => item.MachineName != null && item.MachineName.Trim().ToLower() == logSearchCriteria.MachineName.Trim().ToLower());
                }

                if (!string.IsNullOrWhiteSpace(logSearchCriteria.UserIdentity))
                {
                    qry = qry.Where(item => item.UserIdentity != null && item.UserIdentity.Trim().ToLower() == logSearchCriteria.UserIdentity.Trim().ToLower());
                }

                if (!string.IsNullOrWhiteSpace(logSearchCriteria.AppName))
                {
                    qry = qry.Where(item => item.AppName != null && item.AppName.Trim().ToLower() == logSearchCriteria.AppName.Trim().ToLower());
                }

                if (logSearchCriteria.SeverityLevels.IsNotNullOrEmpty())
                {
                    qry = qry.Where(item => item.Severity != null && logSearchCriteria.SeverityLevels.Select(x => x.Trim().ToLower()).Contains(item.Severity.Trim().ToLower()));
                }

                if (logSearchCriteria.ProcessIds.IsNotNullOrEmpty())
                {
                    //var processIds = logSearchCriteria.ProcessIds.Where(s => int.TryParse(s, out int result)).Select(int.Parse).ToList();
                    qry = qry.Where(item => item.ProcessId != null && logSearchCriteria.ProcessIds.Contains(item.ProcessId));
                }

                if (logSearchCriteria.ThreadIds.IsNotNullOrEmpty())
                {
                    //var threadIds = logSearchCriteria.ThreadIds.Where(s => int.TryParse(s, out int result)).Select(int.Parse).ToList();
                    qry = qry.Where(item => item.ThreadId != null && logSearchCriteria.ThreadIds.Contains(item.ThreadId));
                }

                if (logSearchCriteria.CurrentFunctions.IsNotNullOrEmpty())
                {
                    qry = qry.Where(item => item.CurrentFunction != null && logSearchCriteria.CurrentFunctions.Select(x => x.Trim().ToLower()).Contains(item.CurrentFunction.Trim().ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(logSearchCriteria.Result))
                {
                    qry = qry.Where(item => item.Result != null && item.Result.Trim().ToLower() == logSearchCriteria.Result.Trim().ToLower());
                }

                if (!string.IsNullOrWhiteSpace(logSearchCriteria.Message))
                {
                    qry = qry.Where(item => item.Message != null && item.Message.Trim().ToLower() == logSearchCriteria.Message.Trim().ToLower());
                }

                if (!string.IsNullOrWhiteSpace(logSearchCriteria.IPAddress))
                {
                    qry = qry.Where(item => item.IPAddress != null && item.IPAddress.Trim().ToLower() == logSearchCriteria.IPAddress.Trim().ToLower());
                }

                if (logSearchCriteria.ElaspedTime != null)
                {
                    qry = qry.Where(item => item.ElaspedTime != null && item.ElaspedTime == logSearchCriteria.ElaspedTime.Value);
                }

                if (logSearchCriteria.LogDate != null)
                {
                    qry = qry.Where(item => item.LogDate != null && item.LogDate.Value.Date == logSearchCriteria.LogDate.Value.Date);
                }

                if (logSearchCriteria.StartDate.IsValidDate() && logSearchCriteria.EndDate.IsValidDate())
                {
                    if (logSearchCriteria.StartDate.Value <= logSearchCriteria.EndDate.Value)
                    {
                        qry = qry.Where(x => x.LogDate >= logSearchCriteria.StartDate.Value.StartOfDay() && x.LogDate <= logSearchCriteria.EndDate.Value.EndOfDay());
                    }
                }
                else if (logSearchCriteria.StartDate.IsValidDate())
                {
                    qry = qry.Where(x => x.LogDate >= logSearchCriteria.StartDate.Value.StartOfDay());
                }
                else if (logSearchCriteria.EndDate.IsValidDate())
                {
                    qry = qry.Where(x => x.LogDate <= logSearchCriteria.EndDate.Value.EndOfDay());
                }

                var r = qry.ReturnListModelResultAsync(logSearchCriteria).Result;
                return ReturnListModel<AppLogDataModel>.Success(r.Result, r.TotalRows);
            }
        }
    }

    public async Task SaveToLogDatabase(List<AppLogDataModel> logDataList)
    {
        using (var store = GetConnection())
        {
            using (var db = store.LightweightSession())
            {
                var severityTypeData = logDataList.Where(x => !x.Severity.IsTrimmedStringNullOrEmpty()).Select(x => x.Severity).Distinct().SelectToList(x => new SeverityTypeModel { Id = x });
                var functionTypeData = logDataList.Where(x => !x.CurrentFunction.IsTrimmedStringNullOrEmpty()).Select(x => x.CurrentFunction).Distinct().SelectToList(x => new FunctionTypeModel { Id = x });
                var fileNameTypeData = logDataList.Where(x => !x.CurrentSourceFileName.IsTrimmedStringNullOrEmpty()).Select(x => x.CurrentSourceFileName).Distinct().SelectToList(x => new FileNameModel { Id = x });
                var machineNameData = logDataList.Where(x => !x.MachineName.IsTrimmedStringNullOrEmpty()).Select(x => x.MachineName).Distinct().SelectToList(x => new MachineNameModel { Id = x });
                var appNameData = logDataList.Where(x => !x.AppName.IsTrimmedStringNullOrEmpty()).Select(x => x.AppName).Distinct().SelectToList(x => new AppNameModel { Id = x });

                if (appNameData.Count > 0)
                    db.StoreObjects(appNameData);

                if (severityTypeData.Count > 0)
                    db.StoreObjects(severityTypeData);
                
                if (functionTypeData.Count > 0)
                    db.StoreObjects(functionTypeData);
                
                if (fileNameTypeData.Count > 0)
                    db.StoreObjects(fileNameTypeData);
                
                if (machineNameData.Count > 0)
                    db.StoreObjects(machineNameData);

                if (logDataList.Count > 0)
                    db.StoreObjects(logDataList);

                try
                {
                    await db.SaveChangesAsync();
                }
                finally
                {
                    appNameData.ClearAndReleaseList();
                    severityTypeData.ClearAndReleaseList();
                    functionTypeData.ClearAndReleaseList();
                    fileNameTypeData.ClearAndReleaseList();
                    machineNameData.ClearAndReleaseList();
                    logDataList.ClearAndReleaseList();
                }
            }
        }
    }

    public async Task<List<string>> GetTypedTypes<T>()
        where T : TypeCollectionModel
    {
        using (var store = GetConnection())
        {
            using (var logDb = store.LightweightSession())
            {
                var qry = logDb.Query<T>().AsQueryable();
                return (await qry.ToListAsync()).Select(x => x.Id).ToList();
            }
        }
    }

    public Task<List<string>> GetUniqueSeverityTypes()
    {
        return GetTypedTypes<SeverityTypeModel>();
    }

    public Task<List<string>> GetUniqueFunctionTypes()
    {
        return GetTypedTypes<FunctionTypeModel>();
    }

    public Task<List<string>> GetUniqueFileNames()
    {
        return GetTypedTypes<FileNameModel>();
    }

    public Task<List<string>> GetUniqueAppNames()
    {
        return GetTypedTypes<AppNameModel>();
    }
    public Task<List<string>> GetUniqueHttpVerbs()
    {
        return GetTypedTypes<HttpVerbModel>();
    }
    public Task<List<string>> GetUniqueStatusCodes()
    {
        return GetTypedTypes<StatusCodeModel>();
    }
    public Task<List<string>> GetUniqueMachineNames()
    {
        return GetTypedTypes<MachineNameModel>();
    }
}