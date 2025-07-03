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

public class LiteDbLogRepository : ILogRepository
{
    private readonly string logDataBaseFilePath;
    private readonly string AppLogDatabasePath;
    private readonly string AppLogFileDataBasePath;

    public LiteDbLogRepository(ConfigurationService configuration)
    {
        this.logDataBaseFilePath = configuration.AppLogDatabasePath;
        this.AppLogDatabasePath = configuration.AppLogDatabasePath;
        this.AppLogFileDataBasePath = configuration.AppLogFileDataBasePath;
    }

    public ReturnListModel<AppLogDataModel> GetAppLogs(LogSearchCriteria logSearchCriteria)
    {
        using (var logDb = new LiteDatabase(logDataBaseFilePath))
        {
            if (logSearchCriteria == null)
            {
                logSearchCriteria = new LogSearchCriteria();
            }

            var logCollection = logDb.GetCollection<AppLogDataModel>("LogData");
            var qry = logCollection.Query();

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
                var processIds = logSearchCriteria.ProcessIds.Where(s => int.TryParse(s, out int result)).Select(int.Parse).ToList();
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

            /*
            var properties = logSearchCriteria.GetType().GetProperties();

            Type derivedType = typeof(LogSearchCriteria);

            properties = derivedType.GetProperties()
                .Where(p => p.DeclaringType == derivedType)
                .ToArray();

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(logSearchCriteria);

                if (propertyValue != null)
                {
                    var propertyName = property.Name.Trim().ToLower();
                    var filter = CreateFilter<LogData>(property.Name, propertyValue);
                    qry = qry.Where(filter);
                }
            }

            */

            var count = qry.Where(item => 1 == 1).LongCount();
            qry = qry.ApplySortingOrderBy(logSearchCriteria.SortBy, logSearchCriteria.SortAscending);
            var resultQry = qry.ApplyPaging(logSearchCriteria.Page, logSearchCriteria.PageSize);
            var result = ReturnListModel<AppLogDataModel>.Success(resultQry.ToList(), count);
            return result;
        }
    }

    public Task SaveToLogDatabase(List<AppLogDataModel> logDataList)
    {
        var doesFileExist = File.Exists(AppLogDatabasePath);

        using (var db = new LiteDatabase(LiteDbConnUtils.SharedConnection(AppLogDatabasePath)))
        {
            var logCollection = db.GetCollection<AppLogDataModel>("LogData");
            var severityTypeCollection = db.GetCollection<TypeCollectionModel>("SeverityType");
            var functionTypeCollection = db.GetCollection<TypeCollectionModel>("FunctionType");
            var fileNameCollection = db.GetCollection<TypeCollectionModel>("FileName");

            var severityTypeData = logDataList.Select(x => x.Severity).Distinct().Where(x => !severityTypeCollection.Exists(a => a.Id == x)).ToList();
            var functionTypeData = logDataList.Select(x => x.CurrentFunction).Distinct().Where(x => !functionTypeCollection.Exists(a => a.Id == x)).ToList();
            var fileNameTypeData = logDataList.Select(x => x.CurrentSourceFileName).Distinct().Where(x => !fileNameCollection.Exists(a => a.Id == x)).ToList();

            var result = logCollection.InsertBulk(logDataList);

            severityTypeCollection.InsertBulk(severityTypeData.Select(x => new TypeCollectionModel { Id = x }));
            functionTypeCollection.InsertBulk(functionTypeData.Select(x => new TypeCollectionModel { Id = x }));
            fileNameCollection.InsertBulk(fileNameTypeData.Select(x => new TypeCollectionModel { Id = x }));


            if (!doesFileExist)
            {
                foreach (var prop in typeof(AppLogDataModel).GetProperties())
                {
                    if (prop.Name == nameof(AppLogDataModel.Message))
                        continue;

                    logCollection.EnsureIndex(prop.Name);
                }
            }

            return Task.FromResult(result);
        }
    }

    public Task<List<string>> GetTypedTypes<T>()
    where T : TypeCollectionModel
    {
        using (var db = new LiteDatabase(LiteDbConnUtils.SharedConnection(logDataBaseFilePath)))
        {
            var items = db.GetCollection<T>(nameof(T));
            return Task.FromResult(items.Query().Select(x => x.Id).ToList());
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