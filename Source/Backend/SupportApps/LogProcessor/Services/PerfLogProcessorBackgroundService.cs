using CsvHelper;
using CsvHelper.Configuration;
using Framework.Infrastructure.Models;
using Framework.Infrastructure.Utils;
using LiteDB;
using LogProcessor.Models;
using LogProcessor.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class PerfLogProcessorBackgroundService : BackgroundService
{
    private readonly ILogger<PerfLogProcessorBackgroundService> logger;
    private FileSystemWatcher fileSystemWatcher;
    private ConcurrentDictionary<string, DateTime> watcherDictionary = new ConcurrentDictionary<string, DateTime>();
    private IPerfLogRepository perfLogRepository;
    private PerfLogSettingRepository perfLogSettingRepository;
    private LogDispatcherService logDispatcherService;
    private readonly string LogDirectory;
    private readonly string PerfLogDatabasePath;
    private readonly string PerfLogFileDataBasePath;
    private readonly int batchSize;

    public PerfLogProcessorBackgroundService(ILogger<PerfLogProcessorBackgroundService> logger, ConfigurationService configuration, IPerfLogRepository perfLogRepository, LogDispatcherService logDispatcherService, PerfLogSettingRepository perfLogSettingRepository)
    {
        this.logger = logger;
        this.LogDirectory = configuration.LogDirectory;
        this.PerfLogDatabasePath = configuration.PerfLogDatabasePath;
        this.PerfLogFileDataBasePath = configuration.PerfLogFileDataBasePath;
        this.fileSystemWatcher = new FileSystemWatcher(LogDirectory);
        this.perfLogRepository = perfLogRepository;
        this.logDispatcherService = logDispatcherService;
        this.perfLogSettingRepository = perfLogSettingRepository;
        this.batchSize = configuration.LogBatchSize;
        if (this.batchSize < 100)
            this.batchSize = 100;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Perf Log service started.");

        try
        {
            await ProcessExistingFiles(stoppingToken);
            ConfigureFileWatcher();

            while (!stoppingToken.IsCancellationRequested)
            {
                var keys = watcherDictionary.Keys.ToList();
                foreach (var key in keys)
                {
                    logger.LogDebug($"Processing change in {key}");
                    if (!watcherDictionary.TryGetValue(key, out DateTime value))
                    {
                        continue;
                    }

                    await ProcessLogFile(key);

                    if (!watcherDictionary.TryGetValue(key, out DateTime anotherValue))
                    {
                        continue;
                    }

                    if (anotherValue == value)
                    {
                        watcherDictionary.Remove(key, out DateTime newValue);
                        await Task.Delay(1000);
                        continue;
                    }

                    await Task.Delay(500);
                    await ProcessLogFile(key);
                    await Task.Delay(1000);
                }

                await Task.Delay(5000);
            }

            fileSystemWatcher.EnableRaisingEvents = false;
            fileSystemWatcher.Dispose();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing perf logs.");
        }
    }

    private void ConfigureFileWatcher()
    {
        logger.LogInformation("Configuring Perf File watcher instance.");
        FileSystemEventHandler onFileChangeOrCreate = (sender, e) =>
        {
            var fileName = e.FullPath;
            logger.LogInformation($"Inside PerfLogs: {fileName}");
            if (!e.Name.EndsWith(".perf.log"))
                return;

            DateTime value = DateTime.Now;
            if (!watcherDictionary.TryAdd(fileName, value))
            {
                watcherDictionary.TryUpdate(fileName, value, value);
            }
        };

        fileSystemWatcher.Changed += onFileChangeOrCreate;
        fileSystemWatcher.Created += onFileChangeOrCreate;

        fileSystemWatcher.IncludeSubdirectories = true;
        fileSystemWatcher.EnableRaisingEvents = true;
    }

    private async Task ProcessExistingFiles(CancellationToken stoppingToken)
    {
        logger.LogInformation("Processing existing log files (PerfLogs).");

        foreach (var filePath in FileUtils.GetFileNamesRecursively(LogDirectory, goToOnlyThisfolder: "PerfLogs", fileSearchPattern: "*.perf.log"))
        {
            stoppingToken.ThrowIfCancellationRequested();

            await ProcessLogFile(filePath);
        }
    }

    private async Task ProcessLogFile(string filePath)
    {
        var logDataList = new List<PerfLogDataModel>();

        try
        {
            var fileInfo = new FileInfo(filePath);
            var lastProcessedLine = perfLogSettingRepository.GetLastProcessedLine(fileInfo.FullName);
            using (var streamReader = new PositionableStreamReader(filePath, System.Text.Encoding.UTF8))
            {
                if (lastProcessedLine > 0)
                {
                    streamReader.Position = lastProcessedLine;
                }

                var currentLine = 0;
                PerfLogDataModel logData = null;
                while (true)
                {
                    var logLine = await streamReader.ReadLineAsync();
                    if (logLine.IsNullOrEmpty())
                        break;

                    currentLine++;

                    if (currentLine <= lastProcessedLine)
                    {
                        continue;
                    }

                    logData = ParseLogLine(logLine);
                    if (logData == null)
                        continue;

                    logData.Id = Guid.NewGuid().ToString();
                    logDataList.Add(logData);

                    if (batchSize == logDataList.Count)
                    {
                        var result = await logDispatcherService.CreatePerfLogsAsync(logDataList);
                        if (result.IsSuccess)
                        {
                            await perfLogSettingRepository.SaveToFileDatabase(fileInfo.FullName, logData, currentLine);
                            logDataList.ClearAndReleaseList();
                        }
                        else
                        {
                            logger.LogError($"An error occurred sending the app log file data : {filePath} + {result.ErrorHolder.AllErrorAsString()}");
                            break;
                        }
                    }
                }

                if (logDataList.Any())
                {
                    var result = await logDispatcherService.CreatePerfLogsAsync(logDataList);
                    if (result.IsSuccess)
                        await perfLogSettingRepository.SaveToFileDatabase(fileInfo.FullName, logData, currentLine);
                    else
                        logger.LogError($"An error occurred sending the perf log file data : {filePath} + {result.ErrorHolder.AllErrorAsString()}");
                }

                logDataList.ClearAndReleaseList();
            }
        }
        catch (Exception ex)
        {
            logDataList.ClearAndReleaseList();
            logger.LogError(ex, $"An error occurred while processing the perf log file: {filePath}");
        }
    }

    public static PerfLogDataModel ParseLogLine(string logLine)
    {
        if (string.IsNullOrEmpty(logLine))
            return null;

        string[] fields = logLine.Split('\t');
        var perfLogData = new PerfLogDataModel();

        for (int i = 0; i < fields.Length; i++)
        {
            string field = fields[i].Trim();
            switch (i)
            {
                case 0:
                    perfLogData.LogDate = DateTime.TryParse(field, out DateTime date) ? date : (DateTime?)null;
                    break;
                case 1:
                    perfLogData.ActivityId = field;
                    break;
                case 2:
                    perfLogData.MachineName = field;
                    break;
                case 3:
                    perfLogData.UserIdentity = field;
                    break;
                case 4:
                    perfLogData.AppName = field;
                    break;
                case 5:
                    perfLogData.AppModule = field;
                    break;
                case 6:
                    perfLogData.AppFunction = field;
                    break;
                case 7:
                    perfLogData.HttpVerb = field;
                    break;
                case 8:
                    perfLogData.StatusCode = field;
                    break;
                case 9:
                    perfLogData.StartTime = field;
                    break;
                case 10:
                    perfLogData.EndTime = field;
                    break;
                case 11:
                    perfLogData.ElapsedTimeInMs = float.TryParse(field, out float timeInMs) ? timeInMs : (float?)null;
                    break;
                case 12:
                    perfLogData.IPAddress = field;
                    break;
            }
        }

        if (perfLogData.ElapsedTimeInMs.HasValue)
        {
            perfLogData.ElapsedTimeInSeconds = perfLogData.ElapsedTimeInMs.Value / 1000f;
        }

        return perfLogData;
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
