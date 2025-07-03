/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using DatabaseSchemaReader.DataSchema;
using Framework.Utilities.PocoGenerator.Utilities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Framework.Utilities.PocoGenerator
{
    public class Config
    {
        private const string MSSQLDBTYPE = "sqlserver";
        private const string MYSQLDBTYPE = "mysql";
        private const string SQLITE3DBTYPE = "sqlite3";

        private const string PARAMETERCONFIG = "--config";

        private const string CANWRITETOFLATFILECONFIG = "--canWriteToFlatFile";
        private const string WRITETOFLATFILELOCATIONCONFIG = "--writeToFlatFileLocation";

        private const string SECTIONCONNECTIONSTRING = "connectionString";
        private const string SECTIONNAMESPACE = "nameSpace";
        private const string SECTIONDBTYPE = "dbType";
        private const string SECTIONCODEFILENAME = "codeFileName";
        private const string SECTIONTEMPLATEFILENAME = "templateFileName";
        private const string SECTIONGENERATE = "generate";
        private const string SECTIONIGNOREDTABLES = "ignoredTableNames";
        private const string SECTIONCOLUMNSTOJSONIGNORE = "columnsToJsonIgnore";
        private const string SECTIONIGNOREDTABLESFORSEQ = "ignoredTablesForUpdateSequence";
        private const string SECTIONINBUILTSCHEMA = "inbuiltSchema";
        private const string SECTIONONLYGENTABLES = "selectedTablesToGenerate";
        private const string SECTIONONLYGENOPTION = "onlyGenerateSelectedTables";

        private const string VARIABLECONFIGFILEFOLDER = "{ConfigFolder}";
        private const string VARIABLEEXEFOLDER = "{EXEFolder}";

        private readonly string[] args;

        public Config(string[] args)
        {
            this.args = args;
        }

        public bool CanWriteToFlatFile { get; private set; }

        public string WriteToFlatFileLocation { get; private set; }

        public string ConfigFileLocation { get; private set; }

        public string DbType { get; set; } = string.Empty;

        public string ConnectionString { get; set; } = string.Empty;

        public string Namespace { get; private set; } = string.Empty;

        public string ClassPrefix { get; } = string.Empty;

        public string ClassSuffix { get; } = string.Empty;

        public string SchemaName { get; } = null;

        public bool IncludeViews { get; } = false;

        public List<string> ColumnsToJsonIgnore { get; } = new List<string>();

        public List<string> IgnoredTableNames { get; } = new List<string>();

        public List<string> IgnoredTablesForUpdateSequence { get; } = new List<string>();

        public List<string> InbuiltSchema { get; } = new List<string>();

        public List<TemplateAndCodeFile> InputOutputFiles { get; } = new List<TemplateAndCodeFile>();

        public bool GenerateOnlyForSelectedTables { get; protected set; } = false;

        public List<string> SelectedTablesToGenerate { get; } = new List<string>();

        public SqlType ServerType
        {
#pragma warning disable S2372 , CA1065 // Exceptions should not be thrown from property getters
            get
            {
                if (DbType == null)
                {
                    throw new Exception("DbType not provided.");
                }

                var dbType = DbType.Trim().ToLower();

                switch (dbType)
                {
                    case MSSQLDBTYPE:
                        return SqlType.SqlServer;

                    case MYSQLDBTYPE:
                        return SqlType.MySql;

                    case SQLITE3DBTYPE:
                        return SqlType.SQLite;

                    default:
                        throw new Exception($"DbType {DbType} is not valid.");
                }
            }
#pragma warning restore S2372 , CA1065 // Exceptions should not be thrown from property getters

        }

        public void Load()
        {
            if (!args.IsParamAvailable(PARAMETERCONFIG))
            {
                throw new Exception("-config configuration option is not specified");
            }

            if (!args.IsParamValueAvailable(PARAMETERCONFIG))
            {
                throw new Exception("Configuration file is not specified");
            }

            if (!args.IsParamAvailable(CANWRITETOFLATFILECONFIG))
            {
                throw new Exception($"-{CANWRITETOFLATFILECONFIG} configuration option is not specified");
            }

            CanWriteToFlatFile = args.GetParamValueAs<bool>(CANWRITETOFLATFILECONFIG,false);

            if (CanWriteToFlatFile)
            {
                if (!args.IsParamAvailable(WRITETOFLATFILELOCATIONCONFIG))
                {
                    throw new Exception($"-{WRITETOFLATFILELOCATIONCONFIG} configuration option is not specified");
                }

                WriteToFlatFileLocation = args.GetParamValueAs<string>(WRITETOFLATFILELOCATIONCONFIG, "");

                Console.WriteLine($"WriteToFlatFileLocation = {WriteToFlatFileLocation}");

                var flatFileLocation = Path.GetDirectoryName(WriteToFlatFileLocation);

                Console.WriteLine($"Directory of WriteToFlatFileLocation = {flatFileLocation}");

                if (flatFileLocation == null || flatFileLocation.Trim() == "")
                {
                    throw new Exception($"-{WRITETOFLATFILELOCATIONCONFIG} file is not valid");
                }

                if (!Directory.Exists(flatFileLocation))
                {
                    Directory.CreateDirectory(flatFileLocation);
                }
            }

            ConfigFileLocation = args.GetParamValueAsString(PARAMETERCONFIG);
            // Convert the relative paths to absolute path
            ConfigFileLocation = Path.GetFullPath(ConfigFileLocation);

            if (!File.Exists(ConfigFileLocation))
            {
                throw new Exception($"Configuration file is not specified from {ConfigFileLocation}");
            }

            IConfigurationRoot configuration;
            try
            {
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile(ConfigFileLocation);
                configuration = builder.Build();
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to load the Configuration file from {ConfigFileLocation}", ex);
            }

            ConnectionString = SettingsValue(configuration, SECTIONCONNECTIONSTRING);
            DbType = SettingsValue(configuration, SECTIONDBTYPE);
            Namespace = SettingsValue(configuration, SECTIONNAMESPACE);

            var section = configuration.GetSection(SECTIONGENERATE);
            foreach (var child in section.GetChildren())
            {
                var item = new TemplateAndCodeFile()
                {
                    TemplateFile = SettingsValue(child, SECTIONTEMPLATEFILENAME),
                    CodeFile = SettingsValue(child, SECTIONCODEFILENAME),
                };

                if (item.TemplateFile == null || item.TemplateFile.Trim() == string.Empty)
                {
                    throw new Exception("Template File is not specified in the configuration");
                }

                if (!File.Exists(item.TemplateFile))
                {
                    throw new Exception("Template File specified in the configuration does not exists");
                }

                if (item.CodeFile == null || item.CodeFile.Trim() == string.Empty)
                {
                    throw new Exception("Code File is not specified in the configuration");
                }

                var codeFileDir = Path.GetDirectoryName(item.CodeFile);
                if (!Directory.Exists(codeFileDir))
                {
                    try
                    {
                        Directory.CreateDirectory(codeFileDir);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Unable to create Code File directory {codeFileDir}", ex);
                    }
                }

                InputOutputFiles.Add(item);
            }

            section = configuration.GetSection(SECTIONIGNOREDTABLES);
            foreach (var child in section.GetChildren())
            {
                if (child == null || child.Value == null)
                    continue;

                IgnoredTableNames.Add(child.Value.Trim().ToUpper());
            }

            section = configuration.GetSection(SECTIONIGNOREDTABLESFORSEQ);
            foreach (var child in section.GetChildren())
            {
                if (child == null || child.Value == null)
                    continue;

                IgnoredTablesForUpdateSequence.Add(child.Value.Trim().ToUpper());
            }

            section = configuration.GetSection(SECTIONINBUILTSCHEMA);
            foreach (var child in section.GetChildren())
            {
                if (child == null || child.Value == null)
                    continue;

                InbuiltSchema.Add(child.Value.Trim().ToUpper());
            }

            GenerateOnlyForSelectedTables = SettingsValueAsBool(configuration, SECTIONONLYGENOPTION);
            if (GenerateOnlyForSelectedTables)
            {
                section = configuration.GetSection(SECTIONONLYGENTABLES);
                foreach (var child in section.GetChildren())
                {
                    if (child == null || child.Value == null)
                        continue;

                    SelectedTablesToGenerate.Add(child.Value.Trim().ToUpper());
                }
            }

            section = configuration.GetSection(SECTIONCOLUMNSTOJSONIGNORE);
            foreach (var child in section.GetChildren())
            {
                if (child == null || child.Value == null)
                    continue;

                ColumnsToJsonIgnore.Add(child.Value.Trim().ToUpper());
            }
        }

        public DbConnection GetConnection()
        {
            if (DbType == null)
            {
                throw new Exception("DbType not provided.");
            }

            var dbType = DbType.Trim().ToLower();

            switch (dbType)
            {
                case MSSQLDBTYPE:
                    {
                        return new SqlConnection(ConnectionString);
                    }

                case MYSQLDBTYPE:
                    {
                        return new MySqlConnection(ConnectionString);
                    }

                case SQLITE3DBTYPE:
                    {
                        throw new NotImplementedException();
                    }

                default:
                    throw new Exception($"DbType {DbType} is not valid.");
            }
        }

        private string SubstituteVariables(string str)
        {
            if (str == null)
            {
                return str;
            }

            str = str.Replace(VARIABLECONFIGFILEFOLDER, Path.GetDirectoryName(ConfigFileLocation));
            return str.Replace(VARIABLEEXEFOLDER, Path.GetDirectoryName(Directory.GetCurrentDirectory()));
        }

        private string SettingsValue(IConfiguration configuration, string sectionString, bool substituteVariables = true)
        {
            var section = configuration.GetSection(sectionString);
            if (section == null)
            {
                throw new Exception($"Unable find the {sectionString} settings in config file");
            }

            if (section.Value == null || section.Value.Trim().Length == 0)
            {
                throw new Exception($"Value for {sectionString} settings is not set in config file");
            }

            return substituteVariables ? SubstituteVariables(section.Value) : section.Value;
        }

        private bool SettingsValueAsBool(IConfiguration configuration, string sectionString, bool substituteVariables = true)
        {
            var str = SettingsValue(configuration, sectionString, substituteVariables);
            if (str == null)
            {
                return false;
            }

            str = str.Trim().ToLower();

            return str == "yes" || str == "y" || str == "true" || str == "1";
        }
    }
}
