using CsvHelper;
using CsvHelper.Configuration;
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

public class LogProcessorBackgroundService : BackgroundService
{
    private readonly ILogger<LogProcessorBackgroundService> logger;
    private FileSystemWatcher fileSystemWatcher;
    private ConcurrentDictionary<string, DateTime> watcherDictionary = new ConcurrentDictionary<string, DateTime>();
    private LogSettingRepository logSettingRepository;
    private LogDispatcherService logDispatcherService;

    private readonly string LogDirectory;
    private readonly int batchSize;

    public LogProcessorBackgroundService(ILogger<LogProcessorBackgroundService> logger, ConfigurationService configuration, LogDispatcherService logDispatcherService, LogSettingRepository logSettingRepository)
    {
        this.logger = logger;
        this.LogDirectory = configuration.LogDirectory;
        this.fileSystemWatcher = new FileSystemWatcher(LogDirectory);
        this.logDispatcherService = logDispatcherService;
        this.logSettingRepository = logSettingRepository;
        this.batchSize = configuration.LogBatchSize;
        if (this.batchSize < 100)
            this.batchSize = 100;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("App Log service started.");

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
            logger.LogError(ex, "An error occurred while processing app logs.");
        }
    }

    private void ConfigureFileWatcher()
    {
        logger.LogInformation("Configuring App File watcher instance.");
        FileSystemEventHandler onFileChangeOrCreate = (sender, e) =>
        {
            var fileName = e.FullPath;
            logger.LogInformation($"Inside AppLogs: {fileName}");
            if (!e.Name.EndsWith(".app.log"))
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
        logger.LogInformation("Processing existing log files (AppLogs).");
        try
        {
            foreach (var filePath in FileUtils.GetFileNamesRecursively(LogDirectory, goToOnlyThisfolder: "AppLogs", fileSearchPattern: "*.app.log"))
            {
                stoppingToken.ThrowIfCancellationRequested();

                await ProcessLogFile(filePath);
            }
        }
        catch (OperationCanceledException x1)
        {
            Console.WriteLine("Cancellation Error - " + x1.Message + " ST - " + x1.StackTrace);
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + " ST - " + ex.StackTrace);
        }
    }

    private async Task ProcessLogFile(string filePath)
    {
        var logDataList = new List<AppLogDataModel>();

        try
        {
            var fileInfo = new FileInfo(filePath);
            var lastProcessedLine = logSettingRepository.GetLastProcessedLine(fileInfo.FullName);
            using (var streamReader = new PositionableStreamReader(filePath, System.Text.Encoding.UTF8))
            {
                if (lastProcessedLine > 0)
                {
                    streamReader.Position = lastProcessedLine;
                }

                var currentLine = 0;
                AppLogDataModel logData = null;
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
                        var result = await logDispatcherService.CreateAppLogsAsync(logDataList);
                        if (result.IsSuccess)
                        {
                            await logSettingRepository.SaveToFileDatabase(fileInfo.FullName, logData, currentLine);
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
                    var result = await logDispatcherService.CreateAppLogsAsync(logDataList);
                    if (result.IsSuccess)
                        await logSettingRepository.SaveToFileDatabase(fileInfo.FullName, logData, currentLine);
                    else
                        logger.LogError($"An error occurred sending the app log file data : {filePath} + {result.ErrorHolder.AllErrorAsString()}");
                }

                logDataList.ClearAndReleaseList();
            }
        }
        catch (Exception ex)
        {
            logDataList.ClearAndReleaseList();
            logger.LogError(ex, $"An error occurred while processing the app log file: {filePath}");
        }
    }

    public static AppLogDataModel ParseLogLine(string logLine)
    {
        if (string.IsNullOrEmpty(logLine))
            return null;

        string[] fields = logLine.Split('\t');
        var logData = new AppLogDataModel();

        for (int i = 0; i < fields.Length; i++)
        {
            string field = fields[i].Trim();
            switch (i)
            {
                case 0:
                    logData.LogDate = DateTime.TryParse(field, out DateTime date) ? date : (DateTime?)null;
                    break;
                case 1:
                    logData.ActivityId = field;
                    break;
                case 2:
                    logData.MachineName = field;
                    break;
                case 3:
                    logData.UserIdentity = field;
                    break;
                case 4:
                    logData.AppName = field;
                    break;
                case 5:
                    logData.Severity = field;
                    break;
                case 6:
                    logData.ProcessId = field;
                    break;
                case 7:
                    logData.ThreadId = field;
                    break;
                case 8:
                    logData.CurrentFunction = field;
                    break;
                case 9:
                    logData.CurrentSourceFileName = field;
                    break;
                case 10:
                    logData.CurrentSourceLineNumber = field;
                    break;
                case 11:
                    logData.ElaspedTime = float.TryParse(field, out float time) ? time : (float?)null;
                    break;
                case 12:
                    logData.Result = field;
                    break;
                case 13:
                    logData.Message = field;
                    break;
                case 14:
                    logData.IPAddress = field;
                    break;
            }
        }

        return logData;
    }

    private async Task ProcessLogFile2(string filePath)
    {
        var logDataList = new List<AppLogDataModel>();

        try
        {
            var fileInfo = new FileInfo(filePath);
            var lastProcessedLine = logSettingRepository.GetLastProcessedLine(fileInfo.FullName);

            using (var streamReader = new StreamReader(filePath, new FileStreamOptions { Access = FileAccess.Read, Mode = FileMode.Open, Share = FileShare.ReadWrite }))
            using (var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                IgnoreBlankLines = true,
                Delimiter = "\t",
                HasHeaderRecord = false,
                BadDataFound = null,
                ShouldSkipRecord = args => args.Row.Parser.Record.All(string.IsNullOrWhiteSpace)
            }))
            {
                var currentLine = 0;
                AppLogDataModel logData = null;
                while (await csvReader.ReadAsync())
                {
                    currentLine++;

                    if (currentLine <= lastProcessedLine)
                    {
                        continue;
                    }

                    logData = csvReader.GetRecord<AppLogDataModel>();
                    logData.Id = Guid.NewGuid().ToString();
                    logDataList.Add(logData);

                    if (batchSize == logDataList.Count)
                    {
                        var result = await logDispatcherService.CreateAppLogsAsync(logDataList);
                        if (result.IsSuccess)
                        {
                            await logSettingRepository.SaveToFileDatabase(fileInfo.FullName, logData, currentLine);
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
                    var result = await logDispatcherService.CreateAppLogsAsync(logDataList);
                    if (result.IsSuccess)
                        await logSettingRepository.SaveToFileDatabase(fileInfo.FullName, logData, currentLine);
                    else
                        logger.LogError($"An error occurred sending the app log file data : {filePath} + {result.ErrorHolder.AllErrorAsString()}");
                }

                logDataList.ClearAndReleaseList();
            }
        }
        catch (Exception ex)
        {
            logDataList.ClearAndReleaseList();
            logger.LogError(ex, $"An error occurred while processing the app log file: {filePath}");
        }
    }
    public override void Dispose()
    {
        base.Dispose();
    }
}
