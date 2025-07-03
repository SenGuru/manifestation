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

public class LogSettingRepository
{
    private readonly string logDataBaseFilePath;
    private readonly string AppLogDatabasePath;
    private readonly string AppLogFileDataBasePath;

    public LogSettingRepository(ConfigurationService configuration)
    {
        this.logDataBaseFilePath = configuration.AppLogDatabasePath;
        this.AppLogDatabasePath = configuration.AppLogDatabasePath;
        this.AppLogFileDataBasePath = configuration.AppLogFileDataBasePath;
    }

    public Task SaveToFileDatabase(string filePath, AppLogDataModel lastLine, int lineCount)
    {
        using (var db = new LiteDatabase(LiteDbConnUtils.SharedConnection(AppLogFileDataBasePath)))
        {
            var fileData = new FileData { Path = filePath, LastLine = lastLine, LastLineCount = lineCount };
            var fileCollection = db.GetCollection<FileData>("FileDatabase");

            var item = fileCollection.FindOne(x => x.Path == filePath);

            if (item != null)
            {
                item.LastLine = lastLine;
                item.LastLineCount = lineCount;
                return Task.FromResult(fileCollection.Update(item));
            }

            return Task.FromResult(fileCollection.Insert(fileData));
        }
    }

    public int GetLastProcessedLine(string fileFullName)
    {
        using (var db = new LiteDatabase(LiteDbConnUtils.SharedConnection(AppLogFileDataBasePath)))
        {
            var fileCollection = db.GetCollection<FileData>("FileDatabase");
            var fileData = fileCollection.FindOne(x => x.Path == fileFullName);
            return fileData?.LastLineCount ?? 0;
        }
    }
}