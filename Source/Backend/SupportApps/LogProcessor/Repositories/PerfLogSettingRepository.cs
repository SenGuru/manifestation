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

public class PerfLogSettingRepository
{
    private readonly string perfLogDataBaseFilePath;
    private readonly string PerfLogDatabasePath;
    private readonly string PerfLogFileDataBasePath;

    public PerfLogSettingRepository(ConfigurationService configuration)
    {
        this.perfLogDataBaseFilePath = configuration.PerfLogDatabasePath;
        this.PerfLogDatabasePath = configuration.PerfLogDatabasePath;
        this.PerfLogFileDataBasePath = configuration.PerfLogFileDataBasePath;
    }

    public Task SaveToFileDatabase(string filePath, PerfLogDataModel lastLine, int lineCount)
    {
        using (var db = new LiteDatabase(LiteDbConnUtils.SharedConnection(PerfLogFileDataBasePath)))
        {
            var fileData = new PerfFileData { Path = filePath, LastLine = lastLine, LastLineCount = lineCount };
            var fileCollection = db.GetCollection<PerfFileData>("FileDatabase");
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
        using (var db = new LiteDatabase(LiteDbConnUtils.SharedConnection(PerfLogFileDataBasePath)))
        {
            var fileCollection = db.GetCollection<PerfFileData>("FileDatabase");
            var fileData = fileCollection.FindOne(x => x.Path == fileFullName);
            return fileData?.LastLineCount ?? 0;
        }
    }
}
