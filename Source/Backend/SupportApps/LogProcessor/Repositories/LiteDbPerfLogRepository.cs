using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Framework.Infrastructure.Models;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search.LogSearch;
using Framework.Infrastructure.Utils;
using LiteDB;
using LogProcessor.Models;
using LogProcessor.Utils;

public class LiteDbPerfLogRepository : IPerfLogRepository
{
    private readonly string perfLogDataBaseFilePath;
    private readonly string PerfLogDatabasePath;
    private readonly string PerfLogFileDataBasePath;

    public LiteDbPerfLogRepository(ConfigurationService configuration)
    {
        this.perfLogDataBaseFilePath = configuration.PerfLogDatabasePath;
        this.PerfLogDatabasePath = configuration.PerfLogDatabasePath;
        this.PerfLogFileDataBasePath = configuration.PerfLogFileDataBasePath;
    }

    public ReturnListModel<PerfLogDataModel> GetPerfLogs(PerfLogSearchCriteria perfLogSearchCriteria)
    {
        using (var logDb = new LiteDatabase(perfLogDataBaseFilePath))
        {
            if (perfLogSearchCriteria == null)
            {
                perfLogSearchCriteria = new PerfLogSearchCriteria();
            }

            var logCollection = logDb.GetCollection<PerfLogDataModel>("LogData");
            var qry = logCollection.Query();

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

            var count = qry.Where(item => 1 == 1).LongCount();
            qry = qry.ApplySortingOrderBy(perfLogSearchCriteria.SortBy, perfLogSearchCriteria.SortAscending);
            var resultQry = qry.ApplyPaging(perfLogSearchCriteria.Page, perfLogSearchCriteria.PageSize);

            return ReturnListModel<PerfLogDataModel>.Success(resultQry.ToList(), count);
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

    public Task SaveToLogDatabase(List<PerfLogDataModel> logDataList)
    {
        var doesFileExist = File.Exists(PerfLogDatabasePath);

        using (var db = new LiteDatabase(LiteDbConnUtils.SharedConnection(PerfLogDatabasePath)))
        {
            var logCollection = db.GetCollection<PerfLogDataModel>("LogData");
            var result = logCollection.InsertBulk(logDataList);
            if (!doesFileExist)
            {
                foreach (var prop in typeof(PerfLogDataModel).GetProperties())
                {
                    logCollection.EnsureIndex(prop.Name);
                }
            }

            return Task.FromResult(result);
        }
    }
}