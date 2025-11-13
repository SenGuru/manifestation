using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Framework.Data.Utils;
using Framework.Infrastructure.Models;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search.LogSearch;
using Framework.Infrastructure.Utils;
using JasperFx.Core;
using LiteDB;
using LogProcessor.Models;
using Marten;
using Npgsql;

public class PostgresDbPerfLogRepository : IPerfLogRepository
{
    private readonly string perfLogDataBaseFilePath;
    private readonly string postgresDbConnectionString;

    public PostgresDbPerfLogRepository(ConfigurationService configuration)
    {
        this.perfLogDataBaseFilePath = configuration.PerfLogDatabasePath;
        this.postgresDbConnectionString = configuration.PostgresDbConnectionString;
    }

    public DocumentStore GetConnection()
    {
        EnsureDatabaseExists();
        return DocumentStore.For(opts =>
        {
            opts.Connection(postgresDbConnectionString);
            opts.Schema.For<PerfLogDataModel>().FullTextIndex();
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
        var builder = new NpgsqlConnectionStringBuilder(postgresDbConnectionString);
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

    public ReturnListModel<PerfLogDataModel> GetPerfLogs(PerfLogSearchCriteria perfLogSearchCriteria)
    {
        using (var store = GetConnection())
        {
            using (var logDb = store.LightweightSession())
            {
                if (perfLogSearchCriteria == null)
                {
                    perfLogSearchCriteria = new PerfLogSearchCriteria();
                }

                var qry = logDb.Query<PerfLogDataModel>().AsQueryable();

                if (!perfLogSearchCriteria.Keyword.IsTrimmedStringNullOrEmpty())
                {
                    qry = qry.Where(x =>
                        x.MachineName.Trim().ToUpper().Contains(perfLogSearchCriteria.Keyword.Trim().ToUpper())
                        || x.AppName.Trim().ToUpper().Contains(perfLogSearchCriteria.Keyword.Trim().ToUpper())
                        || x.AppFunction.Trim().ToUpper().Contains(perfLogSearchCriteria.Keyword.Trim().ToUpper())
                        || x.AppModule.Trim().ToUpper().Contains(perfLogSearchCriteria.Keyword.Trim().ToUpper()));
                }

                if (perfLogSearchCriteria.LogDate != null)
                {
                    qry = qry.Where(item => item.LogDate == perfLogSearchCriteria.LogDate.Value);
                }

                if (perfLogSearchCriteria.ActivityIds.IsNotNullOrEmpty())
                {
                    qry = qry.Where(item => item.ActivityId != null && perfLogSearchCriteria.ActivityIds.Select(x => x.Trim().ToLower()).Contains(item.ActivityId.Trim().ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(perfLogSearchCriteria.MachineName))
                {
                    qry = qry.Where(item => item.MachineName != null && item.MachineName.Trim().ToLower() == perfLogSearchCriteria.MachineName.Trim().ToLower());
                }

                if (!string.IsNullOrWhiteSpace(perfLogSearchCriteria.UserIdentity))
                {
                    qry = qry.Where(item => item.UserIdentity != null && item.UserIdentity.Trim().ToLower() == perfLogSearchCriteria.UserIdentity.Trim().ToLower());
                }

                if (!string.IsNullOrWhiteSpace(perfLogSearchCriteria.AppName))
                {
                    qry = qry.Where(item => item.AppName != null && item.AppName.Trim().ToLower() == perfLogSearchCriteria.AppName.Trim().ToLower());
                }

                if (!string.IsNullOrWhiteSpace(perfLogSearchCriteria.AppModule))
                {
                    qry = qry.Where(item => item.AppModule != null && item.AppModule.Trim().ToLower() == perfLogSearchCriteria.AppModule.Trim().ToLower());
                }

                if (perfLogSearchCriteria.AppFunctions.IsNotNullOrEmpty())
                {
                    qry = qry.Where(item => item.AppFunction != null && perfLogSearchCriteria.AppFunctions.Select(x => x.Trim().ToLower()).Contains(item.AppFunction.Trim().ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(perfLogSearchCriteria.HttpVerb))
                {
                    qry = qry.Where(item => item.HttpVerb != null && item.HttpVerb.Trim().ToLower() == perfLogSearchCriteria.HttpVerb.Trim().ToLower());
                }

                if (!string.IsNullOrWhiteSpace(perfLogSearchCriteria.StatusCode))
                {
                    qry = qry.Where(item => item.StatusCode != null && item.StatusCode.Trim().ToLower() == perfLogSearchCriteria.StatusCode.Trim().ToLower());
                }

                if (!string.IsNullOrWhiteSpace(perfLogSearchCriteria.IPAddress))
                {
                    qry = qry.Where(item => item.IPAddress != null && item.IPAddress.Trim().ToLower() == perfLogSearchCriteria.IPAddress.Trim().ToLower());
                }

                if (perfLogSearchCriteria.StartDate.IsValidDate() && perfLogSearchCriteria.EndDate.IsValidDate())
                {
                    if (perfLogSearchCriteria.StartDate.Value <= perfLogSearchCriteria.EndDate.Value)
                    {
                        qry = qry.Where(x => x.LogDate >= perfLogSearchCriteria.StartDate.Value.StartOfDay() && x.LogDate <= perfLogSearchCriteria.EndDate.Value.EndOfDay());
                    }
                }
                else if (perfLogSearchCriteria.StartDate.IsValidDate())
                {
                    qry = qry.Where(x => x.LogDate >= perfLogSearchCriteria.StartDate.Value.StartOfDay());
                }
                else if (perfLogSearchCriteria.EndDate.IsValidDate())
                {
                    qry = qry.Where(x => x.LogDate <= perfLogSearchCriteria.EndDate.Value.EndOfDay());
                }

                if (perfLogSearchCriteria.LesserThanMS != null)
                {
                    qry = qry.Where(item => item.ElapsedTimeInMs <= perfLogSearchCriteria.LesserThanMS);
                }

                if (perfLogSearchCriteria.GreaterThanMS != null)
                {
                    qry = qry.Where(item => item.ElapsedTimeInMs >= perfLogSearchCriteria.GreaterThanMS);
                }

                if (perfLogSearchCriteria.EqualToMs != null)
                {
                    qry = qry.Where(item => item.ElapsedTimeInMs == perfLogSearchCriteria.EqualToMs);
                }

                var r = qry.ReturnListModelResultAsync(perfLogSearchCriteria).Result;

                return ReturnListModel<PerfLogDataModel>.Success(r.Result, r.TotalRows);
            }
        }
    }

    public ReturnListModel<AppFunctionAggregateModel> GetAppFunctionAggregate(AppFunctionAggregateSearchCriteria searchCriteria)
    {
        using (var logDb = new LiteDatabase(perfLogDataBaseFilePath))
        {
            var logCollection = logDb.GetCollection<PerfLogDataModel>("LogData");

            var allData = logCollection.FindAll();

            var groupedData = allData.GroupBy(x => x.AppFunction);

            var aggregatedData = new List<AppFunctionAggregateModel>();

            foreach (var group in groupedData)
            {
                var appFunction = group.Key;
                var elapsedTimes = group.Select(x => x.ElapsedTimeInMs / 1000).OrderBy(x => x).ToList();

                var median = CalculateMedian(elapsedTimes);

                var mean = elapsedTimes.Sum() / elapsedTimes.Count;

                var valueCounts = group.Count();

                aggregatedData.Add(new AppFunctionAggregateModel
                {
                    AppFunction = appFunction,
                    MedianElapsedTimeSec = median,
                    MeanElapsedTimeSec = mean,
                    AppFunctionCount = valueCounts
                });
            }

            var aggQ = aggregatedData.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchCriteria.AppFunction))
            {
                aggQ = aggQ.Where(item => item.AppFunction.Trim().ToLower() == searchCriteria.AppFunction.Trim().ToLower());
            }

            if (searchCriteria.LesserThanMedianTimeElapsed != null)
            {
                aggQ = aggQ.Where(item => item.MedianElapsedTimeSec <= searchCriteria.LesserThanMedianTimeElapsed);
            }

            if (searchCriteria.GreaterThanMedianTimeElapsed != null)
            {
                aggQ = aggQ.Where(item => item.MedianElapsedTimeSec >= searchCriteria.GreaterThanMeanTimeElapsed);
            }

            if (searchCriteria.LesserThanMeanTimeElapsed != null)
            {
                aggQ = aggQ.Where(item => item.MeanElapsedTimeSec <= searchCriteria.LesserThanMeanTimeElapsed);
            }

            if (searchCriteria.GreaterThanMeanTimeElapsed != null)
            {
                aggQ = aggQ.Where(item => item.MeanElapsedTimeSec >= searchCriteria.GreaterThanMeanTimeElapsed);
            }

            if (searchCriteria.LesserThanAppFunctionCount != null)
            {
                aggQ = aggQ.Where(item => item.AppFunctionCount <= searchCriteria.LesserThanAppFunctionCount);
            }

            if (searchCriteria.GreaterThanAppFunctionCount != null)
            {
                aggQ = aggQ.Where(item => item.AppFunctionCount >= searchCriteria.GreaterThanAppFunctionCount);
            }

            var count = aggQ.AsQueryable().Count();
            aggQ = aggQ.ApplySortingOrderBy(searchCriteria.SortBy, searchCriteria.SortAscending);
            aggQ = aggQ.ApplyPaging(searchCriteria.Page, searchCriteria.PageSize);

            return ReturnListModel<AppFunctionAggregateModel>.Success(aggQ.ToList(), count);
        }
    }

    public static float? CalculateMedian(List<float?> values)
    {
        int count = values.Count;
        if (count == 0)
            return 0;

        values.Sort();

        if (count % 2 == 0)
        {
            int middle = count / 2;
            var a = values[middle - 1];
            var b = values[middle];
            var c = (a + b) / 2;
            return (values[middle - 1] + values[middle]) / 2;
        }
        else
        {
            return values[count / 2];
        }
    }

    public async Task SaveToLogDatabase(List<PerfLogDataModel> logDataList)
    {
        using (var store = GetConnection())
        {
            using (var db = store.LightweightSession())
            {
                var appNameData = logDataList.Where(x => !x.AppName.IsTrimmedStringNullOrEmpty()).Select(x => x.AppName).Distinct().SelectToList(x => new AppNameModel { Id = x });
                var statusCodeData = logDataList.Where(x => !x.StatusCode.IsTrimmedStringNullOrEmpty()).Select(x => x.StatusCode).Distinct().SelectToList(x => new StatusCodeModel { Id = x });
                var fileNameTypeData = logDataList.Where(x => !x.AppFunction.IsTrimmedStringNullOrEmpty()).Select(x => x.AppFunction).Distinct().SelectToList(x => new FileNameModel { Id = x });
                var httpVerbData = logDataList.Where(x => !x.HttpVerb.IsTrimmedStringNullOrEmpty()).Select(x => x.HttpVerb).Distinct().SelectToList(x => new HttpVerbModel { Id = x });
                var machineNameData = logDataList.Where(x => !x.MachineName.IsTrimmedStringNullOrEmpty()).Select(x => x.MachineName).Distinct().SelectToList(x => new MachineNameModel { Id = x });

                if (appNameData.Count > 0)
                {
                    db.StoreObjects(appNameData);
                }

                if (statusCodeData.Count > 0)
                {
                    db.StoreObjects(statusCodeData);
                }

                if (fileNameTypeData.Count > 0)
                {
                    db.StoreObjects(fileNameTypeData);
                }

                if (httpVerbData.Count > 0)
                {
                    db.StoreObjects(httpVerbData);
                }

                if (machineNameData.Count > 0)
                {
                    db.StoreObjects(machineNameData);
                }

                if (logDataList.Count > 0)
                {
                    db.StoreObjects(logDataList);
                }

                try
                {
                    await db.SaveChangesAsync();
                }
                finally
                {
                    appNameData.ClearAndReleaseList();
                    statusCodeData.ClearAndReleaseList();
                    fileNameTypeData.ClearAndReleaseList();
                    httpVerbData.ClearAndReleaseList();
                    machineNameData.ClearAndReleaseList();
                    logDataList.ClearAndReleaseList();
                }
            }
        }
    }
}