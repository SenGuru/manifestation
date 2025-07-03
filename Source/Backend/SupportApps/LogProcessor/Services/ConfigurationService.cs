using System;
using System.IO;
using LiteDB;
using Microsoft.Extensions.Configuration;

public class ConfigurationService
{
    public bool UsePostgresDb { get; set; }
    public string PostgresDbConnectionString { get; set; }
    public string LogDirectory { get; set; }
    public string AppLogDatabasePath { get; set; }
    public string AppLogFileDataBasePath { get; set; }
    public string PerfLogDatabasePath { get; set; }
    public string PerfLogFileDataBasePath { get; set; }
    public string LogCentralizedServerUrl { get; set; }
    public int LogBatchSize { get; set; }

    public ConfigurationService()
    {
        var fileName = "log_app_config.json";
        var configFilePath = GetConfigFolderPath();
        var config = new ConfigurationBuilder()
        .SetBasePath(configFilePath)
        .AddJsonFile(fileName)
        .Build();

        Console.WriteLine($"Config Folder - {configFilePath}");
        Console.WriteLine($"Config File Name - {fileName}");

        PostgresDbConnectionString = config.GetValue<string>("PostgresDbConnectionString");
        UsePostgresDb = config.GetValue<bool>("UsePostgresDb",false);
        LogDirectory = GetSubstitutedPath(config.GetValue<string>("LogDirectory"), configFilePath);
        AppLogDatabasePath = GetSubstitutedPath(config.GetValue<string>("AppLogDatabasePath"), configFilePath);
        PerfLogDatabasePath = GetSubstitutedPath(config.GetValue<string>("PerfLogDatabasePath"), configFilePath);
        AppLogFileDataBasePath = GetSubstitutedPath(config.GetValue<string>("AppLogFileDataBasePath"), configFilePath);
        PerfLogFileDataBasePath = GetSubstitutedPath(config.GetValue<string>("PerfLogFileDataBasePath"), configFilePath);
        LogCentralizedServerUrl = config.GetValue<string>("LogCentralizedServerUrl");
        LogBatchSize = config.GetValue<int>("LogBatchSize");

        if (LogBatchSize < 100)
            LogBatchSize = 100;

        if (LogDirectory != null)
        {
            string directoryPath = Path.GetDirectoryName(LogDirectory);
            Console.WriteLine($"LogDirectory Folder - {directoryPath}");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        if (AppLogDatabasePath != null)
        {
            string directoryPath = Path.GetDirectoryName(AppLogDatabasePath);
            Console.WriteLine($"AppLogDatabasePath Folder - {directoryPath}");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        if (AppLogFileDataBasePath != null)
        {
            Console.WriteLine($"AppLogFileDataBasePath - {AppLogFileDataBasePath}");

            string directoryPath = Path.GetDirectoryName(AppLogFileDataBasePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        if (PerfLogDatabasePath != null)
        {
            Console.WriteLine($"PerfLogDatabasePath - {PerfLogDatabasePath}");

            string directoryPath = Path.GetDirectoryName(PerfLogDatabasePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        if (PerfLogFileDataBasePath != null)
        {
            Console.WriteLine($"PerfLogFileDataBasePath - {PerfLogFileDataBasePath}");

            string directoryPath = Path.GetDirectoryName(PerfLogFileDataBasePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            
        }
    }

    public string GetSubstitutedPath(string value, string configPath)
    {
        if (value == null)
            return null;

        if (configPath == null)
            return value;

        value = value.Replace("|ConfigPath|", configPath);

        return value;
    }

    public string GetConfigFolderPath()
    {
        var currentDirectory = Directory.GetCurrentDirectory();

        while (true)
        {
            var configFolderPath = Path.Combine(currentDirectory, "Configuration");
            if (Directory.Exists(configFolderPath))
                return configFolderPath;

            var parentDirectory = Directory.GetParent(currentDirectory);
            if (parentDirectory == null || parentDirectory.FullName == currentDirectory)
                throw new DirectoryNotFoundException("Config folder not found.");

            currentDirectory = parentDirectory.FullName;
        }
    }
}

