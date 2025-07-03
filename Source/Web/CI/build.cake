#addin nuget:?package=Cake.FileHelpers&version=6.1.3
#addin nuget:?package=ReportGenerator.Core&version=5.1.21
#addin nuget:?package=Cake.Coverlet&version=3.0.4
#tool nuget:?package=GitVersion.CommandLine&version=5.12.0
#addin nuget:?package=Npgsql&version=6.0.9
#addin nuget:?package=Cake.Npm&version=3.0.0


//////////////////////////////////////////////////////////////////////
// CONSTANTS
//////////////////////////////////////////////////////////////////////
const string appNameStr = "Manifest";
const string dotnetVerStr = "net8.0";
const string customCommandStr = "Command";
const string commandStr = "command";

//////////////////////////////////////////////////////////////////////
// TASK CONSTANTS
//////////////////////////////////////////////////////////////////////
const string generatePoco = "generate-poco";
const string generateFlatFile = "generate-flat-file";
const string generateMigration = "generate-migration";
const string generateMigrationFromFlatFile = "generate-migration-from-flat-file";
const string generateCode = "generate-code";
const string initDevEnvironment = "init-dev-env";
const string deleteCreateDb = "delete-create-db";
const string deleteCreateDbAndGenc = "delete-create-db-genc";
const string runMigration = "run-migration";
const string installDbServer = "install-db-server";
const string installChoco = "installChoco";
const string buildCodeGenerator = "build-code-generator";

const string gitMergeWithoutCommit = "git-merge-without-commit";

const string generateCrud = "generate-crud";
const string buildGenerateCrud = "build-generate-crud";

const string buildDbMigrator = "build-db-migrator";

const string generateReact = "generate-react";
const string buildGenerateReact = "build-generate-react";

const string generateAngular = "generate-angular";
const string buildGenerateAngular = "build-generate-angular";

const string generateUI = "generate-ui";
const string buildGenerateUI = "build-generate-ui";

const string generateCredList = "generate-cred-list";
const string buildGenerateCredList = "build-generate-cred-list";

const string generateAllTableMig = "generate-all-table-mig";
const string generateSelectedTableMig = "generate-selected-table-mig";
const string generateTableMig = "generate-table-mig";
const string generateColumnMig = "generate-column-mig";

const string generateData = "generate-data";
const string generateDataForPS = "generate-data-for-ps";

const string gitStats = "git-stats";

const string generateTestApiModel = "generate-test-api-model";
const string buildGenerateTestApiModel = "build-test-generate-api-model";

const string generateWordApiModel = "generate-word-api-model";
const string buildGenerateWordApiModel = "build-generate-word-api-model";

const string generateEnumMigration = "generate-enum-migration";
const string buildGenerateEnumMigration = "build-generate-enum-migration";

const string generateAllModels = "generate-all-models";
const string generateAllAPiModels = "generate-all-api-models";

const string localPgMigrate = "local-pg-migrate";
const string checkEnumWithProd = "check-enum-with-prod";
const string generateUiPill = "generate-ui-pill";
const string generateUiInputBlock = "generate-ui-input-block";

const string clean = "clean";
const string build = "build";
const string test = "test";
const string run = "run";
const string watchbuild = "watch-build";
const string watchtest = "watch-test";

const string coverage = "coverage";
const string debugBuild = "debug-build";
const string crudTableName = "table";
const string crudTableNames = "tables";
const string crudColumnName = "column";
const string enumTableName = "enum";
const string uiPropertyName = "property";
const string uiPropertyType = "property-type";
const string branchName = "branch";
const string runWeb = "runWeb";
const string installWeb = "installWeb";
const string newColumn = "new-column";
const string newFunction = "new-function";

const string paramHttpMethod = "http-method";
const string paramReturnType = "return-type"; 
const string paramItemOrList = "item-or-list"; 
const string paramFunctionName = "function-name"; 
const string paramParameterType = "parameter-type";

// make a copy of OpConfig.json and replace "default" with "default.lsql" and "default.lpg" with "default"
const string opConfigPath = $"../Configuration/{appNameStr}Config.json";
const string opConfigBackupPath = $"../Configuration/{appNameStr}Config_backup.json";
const string pgSqlPassword = "MySasiGuru1994";

//////////////////////////////////////////////////////////////////////
// INTERMEDIATE TASK CONSTANTS
//////////////////////////////////////////////////////////////////////

const string buildPocoGenerator = "build-poco-generator";


//////////////////////////////////////////////////////////////////////
// PATH VARIABLES
//////////////////////////////////////////////////////////////////////
const string artifactsPathStr = "../Build/artifacts";
const string sourcePathStr = "../Source";

const string dbMigratorProjectPath = "../Source/Backend/Task/DBMigrator/";
const string dbMigratorProjectFile = "../Source/Backend/Task/DBMigrator/DBMigrator.csproj";
const string dbUtilProjectFile = "../Source/Backend/Task/DBRelatedUtil/DBRelatedUtil.csproj";

var dbMigratorExe = dbMigratorProjectPath + $"/bin/Debug/{dotnetVerStr}/DBMigrator.exe";

const string solutionPath = "../Source/Solutions/Manifest.All.sln";

const string webAppPath = "../Source/Web/Web";

const string pocoGeneratorPathStr = "../Source/Backend/Framework/Utilities/PocoGenerator/";
const string pocoScriptPathStr = "../Source/Backend/Shared/Common/Models/DB/PocoTemplate/";
const string pocoGeneratorProjectFileStr = "../Source/Backend/Framework/Utilities/PocoGenerator/PocoGenerator.csproj";
var pocoGenDir = MakeAbsolute(Directory(pocoGeneratorPathStr));
var pocoGenExe = pocoGenDir + $"/bin/Release/{dotnetVerStr}/PocoGenerator.exe";
var pocoScriptPath = pocoScriptPathStr + "/poco_generator_settings.json";

const string migrationScriptPathStr = "../Source/Backend/Shared/Repository/Migration/AppDBMigrationSteps/InitMigrationTemplate";
var migrationScriptPath = migrationScriptPathStr + "/migration_generator_settings.json";
var webAppCodePathStr = "../Source/Backend/Api/All/";
var webAppCodeProjectFileStr = "../Source/Backend/Api/All/Manifest.App.Api.csproj";
var webAppCodePublishPathStr = webAppCodePathStr + "bin\\Release\\publish";

var testDataProjectFileStr = "../Source/Backend/Tests/TestDataGenerator/Manifest.Tests.TestDataGenerator.csproj";

var logAppCodePathStr = "../Source/Backend/SupportApps/LogProcessor/";
var logAppCodeProjectFileStr = "../Source/Backend/SupportApps/LogProcessor/LogProcessor.csproj";
var logAppCodePublishPathStr = logAppCodePathStr + "bin\\Release\\publish";

var nppesAppCodePathStr = "../Source/Backend/SupportApps/NpiServer/";
var nppesAppCodeProjectFileStr = "../Source/Backend/SupportApps/NpiServer/NpiServer.csproj";
var nppesAppCodePublishPathStr = nppesAppCodePathStr + "bin\\Release\\publish";

var migrationAppProjectPathStr = "../Source/Backend/Task/DBMigrator/";
var crudGenExe = $"../Source/Tools/Generators/EnumTypeTableGenerator/bin/Debug/{dotnetVerStr}/EnumTypeTableGenerator.exe";
var crudGenExeWorkingDir = "../Source/Tools/Generators/EnumTypeTableGenerator/";
var crudGenProjectFileStr = "../Source/Tools/Generators/EnumTypeTableGenerator/EnumTypeTableGenerator.csproj";
crudGenProjectFileStr = MakeAbsolute(File(crudGenProjectFileStr)).FullPath;

//////////////////////////////////////////////////////////////////////
// APP TARGET
//////////////////////////////////////////////////////////////////////

Task(buildPocoGenerator)
.Does(() =>
{
    var appCodePathStr = pocoGenDir+ "/PocoGenerator.csproj";

    Warning($"Building Poco Generator App ...");
    MSBuild(appCodePathStr, new MSBuildSettings {
        Verbosity = Verbosity.Minimal,
        Configuration = "Release",
        PlatformTarget = PlatformTarget.MSIL,
        Restore = true
    });
});


Task(buildCodeGenerator)
.Does(() =>
{
    var appCodePathStr = crudGenExeWorkingDir+ "/EnumTypeTableGenerator.csproj";

    Warning($"Building Crud Generator App ...");
    MSBuild(appCodePathStr, new MSBuildSettings {
        Verbosity = Verbosity.Minimal,
        Configuration = "Debug",
        PlatformTarget = PlatformTarget.MSIL,
        Restore = true
    });
});

Task(buildDbMigrator)
.Does(() =>
{
    Warning($"Building Db Migrator App ...");
    MSBuild(dbMigratorProjectFile, new MSBuildSettings {
        Verbosity = Verbosity.Minimal,
        Configuration = "Debug",
        PlatformTarget = PlatformTarget.MSIL,
        Restore = true
    });
});

void BackupConfigFile()
{
    if (System.IO.File.Exists(opConfigBackupPath))
    {
            //Revert the previously backed up file
            Console.WriteLine("Backup file found, so reverting it..");
            if (System.IO.File.Exists(opConfigBackupPath))
            {
                System.IO.File.Delete(opConfigPath);
                System.IO.File.Move(opConfigBackupPath, opConfigPath);
            }
    }
}

void RevertConfigFile()
{
try
        {
            if (System.IO.File.Exists(opConfigBackupPath))
            {
                System.IO.File.Delete(opConfigPath);
                System.IO.File.Move(opConfigBackupPath, opConfigPath);
            }
            else
            {
                throw new Exception($"Backup file {opConfigBackupPath} does not exist.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error restoring backup file {opConfigPath}: {ex.Message}", ex);
        }
}

const string toLocalSQLString = "\"default.lsql\":";
const string toLocalPGString = "\"default.lpg\":";
const string toQAPGString = "\"default.qa\":";

void ChangeSettingInConfig(string fromString)
{
    try
    {
            System.IO.File.Copy(opConfigPath, opConfigBackupPath, true);
            var opConfigText = System.IO.File.ReadAllText(opConfigPath);
            opConfigText = opConfigText.Replace("\"default\":", toLocalSQLString)
                                       .Replace(fromString, "\"default\":");
            System.IO.File.WriteAllText(opConfigPath, opConfigText);
    }
    catch (Exception ex)
    {
        throw new Exception($"Error backing up or modifying file {opConfigPath}: {ex.Message}", ex);
    }
}


Task(localPgMigrate)
    .Does(() =>
{

    BackupConfigFile();

    if (System.IO.File.Exists(opConfigPath))
    {
        ChangeSettingInConfig(toLocalPGString);
        // delete the "public" schema from PostgreSQL by running a command with the password passed in as an argument
        try
        {            
            var connectionString = new Npgsql.NpgsqlConnectionStringBuilder
            {
                Host = "localhost",
                Username = "postgres",
                Password = pgSqlPassword,
                Database = $"{appNameStr}_dev_db"
            }.ToString();
            using (var connection = new Npgsql.NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Delete the "public" schema
                var command = new Npgsql.NpgsqlCommand("DROP SCHEMA IF EXISTS public CASCADE; CREATE  SCHEMA public; ", connection);
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting PostgreSQL schema: {ex.Message}", ex);
        }
        
        RunTarget(runMigration);
        
        // delete the Config.json file and copy the backup file back to Config.json
        RevertConfigFile();
    }
    else
    {
        throw new Exception($"File {opConfigPath} does not exist.");
    }
});

Task(checkEnumWithProd)
    .Does(() =>
{
    Console.WriteLine("Backing up file...");
    BackupConfigFile();
    Console.WriteLine("Completed");

    if (System.IO.File.Exists(opConfigPath))
    {
        Console.WriteLine("Replacing Config file With QA Config");
        ChangeSettingInConfig(toQAPGString);
        Console.WriteLine("Completed.");

        Console.WriteLine("Running the Project.");
        DotNetRun(dbUtilProjectFile, "check-enums");

        Console.WriteLine("Reverting Config File");
        // delete the Config.json file and copy the backup file back to Config.json
        RevertConfigFile();
        Console.WriteLine("Completed.");
    }
    else
    {
        throw new Exception($"File {opConfigPath} does not exist.");
    }
});


string GetArgumentValue(string key, string defaultValue = "")
{
	var result = Argument<string>(key,"");
    if (result == null || result.Trim() == "")
    {
        var args = Environment.GetCommandLineArgs();
        
        result = "";
        foreach(var a in args)
        {
            Console.WriteLine("a = " + a);
            var lst = a.Split(" ");
            foreach(var l in lst)
            {
                Console.WriteLine("l = " + l);
                var idx = l.IndexOf("=");
                if (l.StartsWith("--" + key) &&  idx != -1)
                {
                    if (idx + 1 < l.Length) 
                    {
                        result = l.Substring(idx + 1);
                        Console.WriteLine("result = " + result);
                    }
                    else
                    {
                        Console.WriteLine("idx + 1 = " + (idx + 1).ToString());
                    }
                    break;
                }
            }
        }
    }
    return result;
}

Task(generateCrud)
.Does(() =>
{
    Warning($"Generating Crud {appNameStr}:");

    var tableName = GetArgumentValue(crudTableName,"");
    if (tableName == null || tableName.Trim() == "")
        throw new Exception("Table Name parameter ( --table=<TableName> ) is missing");

    //DotNetRun(crudGenProjectFileStr, "crud \"" + tableName + "\"", new DotNetRunSettings  { NoBuild = false, Configuration = "Debug", WorkingDirectory = crudGenExeWorkingDir });
    var code = StartProcess(crudGenExe, new ProcessSettings{ Arguments = "crud \"" + tableName + "\"", WorkingDirectory = crudGenExeWorkingDir });
    if (code != 0)
        throw new Exception("Crud generator errored out");
    Warning($"Completed generating crud...");
});


Task(gitMergeWithoutCommit)
.Does(() =>
{
    Warning($"Running Git Merge Without Commit :");

    var bName = GetArgumentValue(branchName,"");
    if (bName == null || bName.Trim() == "")
        throw new Exception("Branch Name parameter ( --branch=<BranchName> ) is missing");

    var code = StartProcess("git", new ProcessSettings{ Arguments = "merge  " + bName + " --no-commit --no-ff "});
    Warning($"Completed generating crud...");
});


Task(generateReact)
.Does(() =>
{
    Warning($"Generating React Models {appNameStr}:");

    DotNetRun(crudGenProjectFileStr, "react ", new DotNetRunSettings  { Configuration = "Debug", WorkingDirectory = crudGenExeWorkingDir });
    //if (code != 0)
    //    throw new Exception("Model generator errored out");
    Warning($"Completed generating React Models...");
});

Task(generateAngular)
.Does(() =>
{
    Warning($"Generating Angular Models {appNameStr}:");
    var path = MakeAbsolute(Directory(webAppPath)).FullPath;;

    DotNetRun(crudGenProjectFileStr, $"angular \"{path}\"", new DotNetRunSettings  { Configuration = "Debug", WorkingDirectory = crudGenExeWorkingDir });
    //if (code != 0)
    //    throw new Exception("Model generator errored out");
    Warning($"Completed generating Angular Models...");
});


Task(gitStats)
.Does(() =>
{
    Warning($"Generating Git Stats");

    var code = StartProcess("git", new ProcessSettings{ Arguments = "shortlog -sn --no-merges ", WorkingDirectory = crudGenExeWorkingDir });
    if (code != 0)
        throw new Exception("Git Stats errored out");
    Warning($"Completed generating Git Stats ...");
});



Task(generateUI)
.Does(() =>
{
    Warning($"Generating Angular UI {appNameStr}:");

    var tableName = GetArgumentValue(crudTableName,"");
    if (tableName == null || tableName.Trim() == "")
        throw new Exception("Table Name parameter ( --table=<TableName> ) is missing");


    DotNetRun(crudGenProjectFileStr, "ui " + tableName, new DotNetRunSettings  { Configuration = "Debug", WorkingDirectory = crudGenExeWorkingDir });
    //if (code != 0)
   //     throw new Exception("Model UI errored out");
    Warning($"Completed generating Angular UI ...");
});

Task(generateCredList)
.Does(() =>
{
    Warning($"Generating Cred List {appNameStr}:");

    var tableName = GetArgumentValue(crudTableName,"");
    if (tableName == null || tableName.Trim() == "")
        throw new Exception("Table Name parameter ( --table=<TableName> ) is missing");


    DotNetRun(crudGenProjectFileStr, "cred-list " + tableName, new DotNetRunSettings  {  Configuration = "Debug", WorkingDirectory = crudGenExeWorkingDir });
    //if (code != 0)
   //     throw new Exception("Model Cred List errored out");
    Warning($"Completed generating Angular Cred List ...");
});


Task(generateEnumMigration)
.Does(() =>
{
    Warning($"Generating Enum Migration {appNameStr}:");

    DotNetRun(crudGenProjectFileStr, "migration", new DotNetRunSettings  {  Configuration = "Debug", WorkingDirectory = crudGenExeWorkingDir });
   // if (code != 0)
    //    throw new Exception("Enum generator errored out");
    Warning($"Completed generating Enum Migration ...");
});

Task(generateTestApiModel)
.Does(() =>
{
    Warning($"Generating Test Api Models {appNameStr}:");

    DotNetRun(crudGenProjectFileStr, "testapi", new DotNetRunSettings  {  Configuration = "Debug", WorkingDirectory = crudGenExeWorkingDir });
    //if (code != 0)
    //    throw new Exception("Test API generator errored out");
    Warning($"Completed generating Test Api Models...");
});

Task(generateWordApiModel)
.Does(() =>
{
    Warning($"Generating Word Api Models {appNameStr}:");

    DotNetRun(crudGenProjectFileStr, "wordapi", new DotNetRunSettings  {  Configuration = "Debug", WorkingDirectory = crudGenExeWorkingDir });
    //if (code != 0)
    //    throw new Exception("Word Poco generator errored out");
    Warning($"Completed generating Word Api Models...");
});


Task(buildGenerateEnumMigration)
 .IsDependentOn(buildCodeGenerator)
 .IsDependentOn(generateEnumMigration)
.Does(() =>
{
});

Task(buildGenerateCrud)
 .IsDependentOn(buildCodeGenerator)
 .IsDependentOn(generateCrud)
.Does(() =>
{
});

Task(buildGenerateReact)
 .IsDependentOn(buildCodeGenerator)
 .IsDependentOn(generateReact)
.Does(() =>
{
});

Task(buildGenerateAngular)
 .IsDependentOn(buildCodeGenerator)
 .IsDependentOn(generateAngular)
.Does(() =>
{
});

Task(buildGenerateUI)
 .IsDependentOn(buildCodeGenerator)
 .IsDependentOn(generateUI)
.Does(() =>
{
});


Task(buildGenerateCredList)
 .IsDependentOn(buildCodeGenerator)
 .IsDependentOn(generateCredList)
.Does(() =>
{
});

Task(buildGenerateTestApiModel)
 .IsDependentOn(buildCodeGenerator)
 .IsDependentOn(generateTestApiModel)
.Does(() =>
{
});

Task(buildGenerateWordApiModel)
 .IsDependentOn(buildCodeGenerator)
 .IsDependentOn(generateWordApiModel)
.Does(() =>
{
});

Task(generateFlatFile)
    .IsDependentOn(buildPocoGenerator)
.Does(() =>
{
    var flatFile = GetArgumentValue("flat-file","");
    if (flatFile == null || flatFile.Trim() == "")
        throw new Exception("Flat File Name parameter ( --flat-file=<FlatFileName> ) is missing");

    Warning($"Creating Flat File {appNameStr}:");
    DotNetRun(pocoGeneratorProjectFileStr ,"--poco-generator --config \"" + pocoScriptPath + "\" --canWriteToFlatFile true --writeToFlatFileLocation \"" +  "" +"\"");
    //var code = StartProcess(pocoGenExe, new ProcessSettings{ Arguments = "--config \"" + pocoScriptPath + "\"" });
    //if (code != 0)
    //    throw new Exception("Poco generator errored out");

    Warning($"Completed creating the Flat File ...");
});

Task(generatePoco)
    .IsDependentOn(buildPocoGenerator)
.Does(() =>
{
    Warning($"Creating Poco {appNameStr}:");
    DotNetRun(pocoGeneratorProjectFileStr ,"--poco-generator --config \"" + pocoScriptPath + "\" --canWriteToFlatFile false");
    //var code = StartProcess(pocoGenExe, new ProcessSettings{ Arguments = "--config \"" + pocoScriptPath + "\"" });
    //if (code != 0)
    //    throw new Exception("Poco generator errored out");

    Warning($"Completed creating the Poco...");
});

Task(generateMigration)
    .IsDependentOn(buildPocoGenerator)
.Does(() =>
{
    Warning($"Creating Migration code for {appNameStr}:");
    
    DotNetRun(pocoGeneratorProjectFileStr ,"--poco-generator --config \"" + migrationScriptPath + "\"  --canWriteToFlatFile false");

    //var code = StartProcess(pocoGenExe, new ProcessSettings{ Arguments = "--config \"" + migrationScriptPath + "\" --canWriteToFlatFile false" });
    //if (code != 0)
    //    throw new Exception("Migration generator errored out");

    Warning($"Completed creating the Migration Code...");
});

Task(generateMigrationFromFlatFile)
    .IsDependentOn(buildPocoGenerator)
.Does(() =>
{
    var flatFile = GetArgumentValue("flat-file","");
    if (flatFile == null || flatFile.Trim() == "")
        throw new Exception("Flat File Name parameter ( --flat-file=<FlatFileName> ) is missing");

    if (FileExists(flatFile) == false)
            throw new Exception($"Flat File does not exists in Path - {flatFile}");

    Warning($"Creating Migration code from flat file for {appNameStr}:");
    
    DotNetRun(pocoGeneratorProjectFileStr ,"----mig-from-flat-file-generator \"" + flatFile + "\" ");

    //var code = StartProcess(pocoGenExe, new ProcessSettings{ Arguments = "--config \"" + migrationScriptPath + "\"" });
    //if (code != 0)
    //    throw new Exception("Migration generator errored out");

    Warning($"Completed creating the Migration Code...");
});

Task(generateCode)
    .IsDependentOn(generatePoco)
    .IsDependentOn(generateMigration)
    .IsDependentOn(buildGenerateEnumMigration)
    .IsDependentOn(buildGenerateTestApiModel)
    //.IsDependentOn(buildGenerateReact)
    .IsDependentOn(buildGenerateAngular)
.Does(() =>
{
});

Task(generateAllModels)
    .IsDependentOn(buildGenerateEnumMigration)
    .IsDependentOn(buildGenerateTestApiModel)
    //.IsDependentOn(buildGenerateReact)
    .IsDependentOn(buildGenerateAngular)
.Does(() =>
{
});

Task(generateAllAPiModels)
    .IsDependentOn(generatePoco)
    .IsDependentOn(generateMigration)
    .IsDependentOn(buildGenerateEnumMigration)
    .IsDependentOn(buildGenerateTestApiModel)
.Does(() =>
{
});

Task(installChoco)
.Does(() =>
{
    var pathToAdd = @";%ALLUSERSPROFILE%\\chocolatey\\bin";
    var existingPath  = System.Environment.GetEnvironmentVariable("PATH");
    if (!existingPath.Contains(pathToAdd))
    {
        Warning($"Adding Choco path to environment variable:");
        System.Environment.SetEnvironmentVariable("PATH", existingPath + pathToAdd, EnvironmentVariableTarget.Machine);
    }

    StartProcess("powershell.exe", new ProcessSettings{ Arguments = "-NoProfile -InputFormat None -ExecutionPolicy Bypass -Command \"iex ((New-Object System.Net.WebClient).DownloadString(\'https://chocolatey.org/install.ps1\'))\"" });

});

Task(installDbServer)
.Does(() =>
{
    Warning($"Initializing Sqlite Server...");
    if (System.IO.File.Exists($"../Data/Database/{appNameStr}DB.sqlite3"))
    {
    System.IO.File.Delete($"../Data/Database/{appNameStr}DB.sqlite3");
    }

    Warning($"Initializing LocalDB Server...");

    StartProcess("sqllocaldb.exe", new ProcessSettings{ Arguments = $"Stop {appNameStr}DBServer -k" });
    StartProcess("sqllocaldb.exe", new ProcessSettings{ Arguments = $"delete {appNameStr}DBServer" });

    var exitCode = StartProcess("sqllocaldb.exe", new ProcessSettings{ Arguments = $"Create {appNameStr}DBServer -s" });

    StartProcess("sqllocaldb.exe", new ProcessSettings{ Arguments = $"Share {appNameStr}DBServer {appNameStr}DBServer" });

    var path = MakeAbsolute(Directory("./../Data/Database/")).FullPath;;

    if(DirectoryExists(path) == false)
    {
        CreateDirectory(path);
    }

    var cmdParam = $"-S (localdb)\\{appNameStr}DBServer -Q  \"CREATE LOGIN admin WITH PASSWORD = \'@Dmin123##\' \"";
    StartProcess("sqlcmd.exe", new ProcessSettings{ Arguments = cmdParam });

    cmdParam = $"-S (localdb)\\{appNameStr}DBServer -Q  \"CREATE USER admin \"";
    StartProcess("sqlcmd.exe", new ProcessSettings{ Arguments = cmdParam });

    cmdParam = $"-S (localdb)\\{appNameStr}DBServer -Q  \" SP_ADDSRVROLEMEMBER \'admin\',\'SYSADMIN\'\"";
    StartProcess("sqlcmd.exe", new ProcessSettings{ Arguments = cmdParam });

    CreateDB(path, $"{appNameStr}DB_data.mdf",  $"{appNameStr}DB_log.ldf", $"{appNameStr}Db");
});


void CreateDB(string path, string dbFileName, string logFileName, string dbName)
{
    var dbPath = path + "/" + dbFileName;
    var logPath = path + "/" + logFileName;
    
    if (FileExists(dbPath))
        DeleteFile(dbPath);

    if (FileExists(logPath))
        DeleteFile(logPath);

    path = path.Replace("/","\\");


    var cmdParam = $"-S (localdb)\\{appNameStr}DBServer -Q  \"CREATE DATABASE [{dbName}] ON PRIMARY ( NAME=[{System.IO.Path.GetFileNameWithoutExtension(dbFileName)}], FILENAME =\'{path}\\{dbFileName}\') LOG ON (NAME=[{System.IO.Path.GetFileNameWithoutExtension(logFileName)}], FILENAME =\'{path}\\{logFileName}\');\" "; 
    StartProcess("sqlcmd.exe", new ProcessSettings{ Arguments = cmdParam });
}


Task(initDevEnvironment)
    .IsDependentOn(installChoco)
    .IsDependentOn(deleteCreateDb)
.Does(() =>
{
    Warning($"Installing NodeJs :");
    ChocolateyInstall("nodejs-lts", new ChocolateyInstallSettings {
        Version = "12.18.4",
        NotSilent = false
    });

    Warning($"Installing Libreoffice :");
    ChocolateyInstall("libreoffice-fresh", new ChocolateyInstallSettings {
        Version = "6.4.6",
        NotSilent = false,
        IgnoreChecksums = true
    });

    Warning($"Installing WinMerge :");
    ChocolateyInstall("Winmerge", new ChocolateyInstallSettings {
        Version = "2.16.4.20191007",
        NotSilent = false
    });
});

Task(deleteCreateDb)
    .IsDependentOn(installDbServer)
    .IsDependentOn(runMigration)
.Does(() =>
{
});

Task(deleteCreateDbAndGenc)
    .IsDependentOn(deleteCreateDb)
    .IsDependentOn(generateCode)
.Does(() =>
{
});



Task(runMigration)
    .IsDependentOn(buildDbMigrator)
.Does(() =>
{
    Warning($"Creating Db Migrator:");
    var settings = new DotNetRunSettings
    {
     Configuration = "Debug"
    };

    DotNetRun(dbMigratorProjectFile, settings);

    //var code = StartProcess("dotnet", new ProcessSettings{ Arguments = $"run {dbMigratorProjectFile}" });

    //if (code != 0)
    //    throw new Exception("DB Migrator errored out");

    Warning($"Completed running the Db Migrator...");
});


Task(clean)
.Does(() =>
{
    var path = MakeAbsolute(Directory(webAppCodePublishPathStr)).FullPath;
    Warning("Cleaning bin/obj folders from Path " + path);
    CleanDirectories(path + "/**/bin/" + "Debug");
    CleanDirectories(path + "/**/bin/" + "Release");
    CleanDirectories(path + "/**/obj/" + "Debug");
    CleanDirectories(path + "/**/obj/" + "Release");

});

Task(installWeb)
//.IsDependentOn(clean)
.Does(() =>
{
    var path = MakeAbsolute(Directory(webAppPath)).FullPath;;

    Warning($"Installing Web App ...");

    NpmInstall (new NpmInstallSettings { WorkingDirectory = path });
});


Task(runWeb)
//.IsDependentOn(clean)
.Does(() =>
{
    var path = MakeAbsolute(Directory(webAppPath)).FullPath;;

    Warning($"Running Web App ...");

    NpmRunScript (new NpmRunScriptSettings { ScriptName = "start", WorkingDirectory = path });

});



Task(watchbuild)
//.IsDependentOn(clean)
.Does(() =>
{
    Warning($"Watching and Building {appNameStr} App ...");
    var code = StartProcess("dotnet", new ProcessSettings{ Arguments = $" watch build --project {webAppCodeProjectFileStr}" });
    if (code != 0)
        throw new Exception("dotnet watch build errored out");
});

Task(run)
//.IsDependentOn(clean)
.Does(() =>
{
    Warning($"Running {appNameStr} App ...");
    var code = StartProcess("dotnet", new ProcessSettings{ Arguments = $" run --project {webAppCodeProjectFileStr}" });
    if (code != 0)
        throw new Exception("dotnet run errored out");
});


Task(build)
.IsDependentOn(clean)
.Does(() =>
{
    BuildProject($"{appNameStr} App",webAppCodeProjectFileStr,webAppCodePublishPathStr);
});

void BuildProject(string appName, string projectFileStr,string publishPathStr)
{
    Warning($"Building {appName} ...");
    DotNetPublish(projectFileStr, new DotNetPublishSettings
    {
        Configuration = "Release",
        OutputDirectory = publishPathStr,
    });

    FileWriteText($"{publishPathStr}/version_number.txt", GetVersionWithDate());

    if (System.IO.Directory.Exists(publishPathStr + "/Configuration"))
    {
        Warning($"Configuration found under - {publishPathStr + "/Configuration"} - deleting it");
        System.IO.Directory.Delete(publishPathStr + "/Configuration", true);
    }
    else
    {
        Warning($"Configuration not found under - {publishPathStr + "/Configuration"}");
    }
}

Task(debugBuild)
.Does(() =>
{
    Warning($"Building {appNameStr} App in Debug Mode...");
    DotNetBuild(solutionPath, new DotNetBuildSettings
    {
        Configuration = "Debug",
    });
});


Task(test)
.IsDependentOn(coverage)
.Does(() =>
{
});


var artifactsDirectory = MakeAbsolute(Directory(artifactsPathStr));

Task(coverage)
.Does(() =>
{
        var resultsFile = artifactsDirectory.CombineWithFilePath("coverage.xml");
        if(FileExists(resultsFile))
        {
            DeleteFile(resultsFile);
        }

        DotNetBuild("../Source/Solutions/Manifest.All.sln", new DotNetBuildSettings
        {
            Configuration = "Debug"
        });


        var testSettings = new DotNetTestSettings
        {
            Configuration = "Debug",
            NoBuild = true,
            NoRestore = true,
            //TestAdapterPath = ".",
            //Logger = "trx",
            ArgumentCustomization = args=>args.Append($" -p:ParallelizeTestCollections=false  ")
        };

        var coverletSettings = new CoverletSettings
        {
            CollectCoverage = true,
            CoverletOutputFormat = CoverletOutputFormat.opencover,
            CoverletOutputDirectory = artifactsDirectory,
            CoverletOutputName = $"coverage.xml"
        };

        DotNetTest("../Source/Solutions/Manifest.All.sln", testSettings, coverletSettings);

        var reportSettings = new ReportGeneratorSettings
    {
        ArgumentCustomization = args => args.Append($"-reportTypes:HtmlInline_AzurePipelines")
    };
    ReportGenerator(resultsFile, artifactsDirectory, reportSettings);
});

GitVersion GetVersionObj()
{
     var versionInfo = GitVersion(new GitVersionSettings{
         ArgumentCustomization = args => args.Append("/overrideconfig tag-prefix=test /overrideconfig mode=ContinuousDeployment")
     });

     return versionInfo;
}

string GetVersionWithDate()
{
	try
	{
     var versionInfo = GetVersionObj();
     return versionInfo.MajorMinorPatch+"."+ versionInfo.PreReleaseNumber + " (" + DateTime.Now.ToShortDateString() +  ")";
	}
	catch
	{
		return $"{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}";
	}
}


Task(generateUiPill)
.Does(() =>
{    
    var enumName = GetArgumentValue(enumTableName,"");
    if (enumName == null || enumName.Trim() == "")
        throw new Exception("Enum Name parameter ( --enum=<EnumName> --table=<TableName>) is missing");

    var tableName = GetArgumentValue(crudTableName,"");
    if (tableName == null || tableName.Trim() == "")
        throw new Exception("Table Name parameter ( --enum=<EnumName> --table=<TableName>) is missing");


    DotNetRun(crudGenProjectFileStr, "ui-enum-pill " + enumName + " " + tableName, new DotNetRunSettings {  Configuration = "Debug", WorkingDirectory = crudGenExeWorkingDir });
    //if (code != 0)
    //    throw new Exception("Pill Generator errored out");
    Warning($"Completed generating UI Pills ...");
});


Task(generateUiInputBlock)
.Does(() =>
{    
    var propertyType = GetArgumentValue(uiPropertyType,"");
    if (propertyType == null || propertyType.Trim() == "")
        throw new Exception("Property Type parameter ( --property=<PropertyName> --property-type=<propertyTypeName> --table=<TableName>) is missing");

    var propertyName = GetArgumentValue(uiPropertyName,"");
    if (propertyName == null || propertyName.Trim() == "")
        throw new Exception("Property Name parameter ( --property=<PropertyName> --property-type=<propertyTypeName> --table=<TableName>) is missing");

    var tableName = GetArgumentValue(crudTableName,"");
    if (tableName == null || tableName.Trim() == "")
        throw new Exception("Table Name parameter ( --property=<PropertyName> --property-type=<propertyTypeName> --table=<TableName>) is missing");


    DotNetRun(crudGenProjectFileStr, "ui-input-block " + tableName + " " + propertyName + " " + propertyType , new DotNetRunSettings {  Configuration = "Debug", WorkingDirectory = crudGenExeWorkingDir });
    //if (code != 0)
    //    throw new Exception("Pill Generator errored out");
    Warning($"Completed generating UI Pills ...");
});


Task(generateAllTableMig)
.IsDependentOn(buildCodeGenerator)
.Does(() =>
{    
    var code = StartProcess(crudGenExe, new ProcessSettings{ Arguments = "all-db-fkeys", WorkingDirectory = crudGenExeWorkingDir });
    if (code != 0)
        throw new Exception("All Table Mig Gen errored out");
    Warning($"Completed generating All Table Mig Gen ...");

});

Task(generateSelectedTableMig)
.IsDependentOn(buildCodeGenerator)
.Does(() =>
{
    var tableNames = GetArgumentValue(crudTableNames,"");
    if (tableNames == null || tableNames.Trim() == "")
        throw new Exception("Table Names parameter ( --tables=<TableNamesWithColonSeperation>) is missing");
    Warning($"tableNames = {tableNames}");
    var code = StartProcess(crudGenExe, new ProcessSettings{ Arguments = $"selected-db-fkeys {tableNames}", WorkingDirectory = crudGenExeWorkingDir });
    if (code != 0)
        throw new Exception("Selected Table Mig Gen errored out");
    Warning($"Completed generating Selected Table Mig Gen ...");

});

Task(newColumn)
.IsDependentOn(buildCodeGenerator)
.Does(() =>
{    
    var tableName = GetArgumentValue(crudTableName,"");
    if (tableName == null || tableName.Trim() == "")
        throw new Exception("Table Name parameter ( --table=<TableName>) is missing");

    var colName = GetArgumentValue(crudColumnName,"");
    if (colName == null || colName.Trim() == "")
        throw new Exception("Column Name parameter ( --column=<ColumnName>) is missing");

    var code = StartProcess(crudGenExe, new ProcessSettings{ Arguments = $"new-column {tableName} {colName}", WorkingDirectory = crudGenExeWorkingDir });
    if (code != 0)
        throw new Exception("New Column code errored out");
    Warning($"Completed generating New Column code ...");
});

Task(newFunction)
.IsDependentOn(buildCodeGenerator)
.Does(() =>
{    
    var param1 = $"--{paramHttpMethod}=<GET|POST|PUT>";
    var param2 = $"--{paramReturnType}=<ReturnType>";
    var param3 = $"--{paramItemOrList}=<Item|List>";
    var param4 = $"--{paramFunctionName}=<Function Name>";
    var param5 = $"--{paramParameterType}=<Parameter Type>";
    var allParam = $"Command Line params :\n{param1} {param2} {param3} {param4} {param5}";

    var httpMethod = GetArgumentValue(paramHttpMethod,"");
    if (httpMethod == null || httpMethod.Trim() == "")
        throw new Exception($"Http Method parameter ( {param1} ) is missing \n {allParam}");

    var returnType = GetArgumentValue(paramReturnType,"");
    if (returnType == null || returnType.Trim() == "")
        throw new Exception($"Return Type parameter ( {param2} ) is missing \n {allParam}");

    var itemOrList = GetArgumentValue(paramItemOrList,"");
    if (itemOrList == null || itemOrList.Trim() == "")
        throw new Exception($"Item Or List parameter ( {param3} ) is missing \n {allParam}");

    var functionName = GetArgumentValue(paramFunctionName,"");
    if (functionName == null || functionName.Trim() == "")
        throw new Exception($"Table Name parameter ( {param4} ) is missing \n {allParam}");

    var parameterType = GetArgumentValue(paramParameterType,"");
    if (parameterType == null || parameterType.Trim() == "")
        throw new Exception($"Table Name parameter ( {param5} ) is missing \n {allParam}");

    var code = StartProcess(crudGenExe, new ProcessSettings{ Arguments = $"new-function \"{httpMethod}\" \"{returnType}\" \"{itemOrList}\" \"{functionName}\" \"{parameterType}\"", WorkingDirectory = crudGenExeWorkingDir });
    if (code != 0)
        throw new Exception("New Column code errored out");
    Warning($"Completed generating New Column code ...");
});



Task(generateTableMig)
.IsDependentOn(buildCodeGenerator)
.Does(() =>
{    
    var tableName = GetArgumentValue(crudTableName,"");
    if (tableName == null || tableName.Trim() == "")
        throw new Exception("Table Name parameter ( --table=<TableName> ) is missing");


    DotNetRun(crudGenProjectFileStr, "db-fkey " + tableName, new DotNetRunSettings {  Configuration = "Debug", WorkingDirectory = crudGenExeWorkingDir });
    //if (code != 0)
    //    throw new Exception("Table Mig Gen errored out");
    Warning($"Completed generating Table Mig Gen ...");

});

Task(generateTableMig + "-alt")
.Does(() =>
{
    var table = Argument("table","");
    if (table == null || table.Trim() == "")
    {
        throw new Exception("Table parameter is not specified");
    } 

    var str = @$"
        Create.Index(""IX_{table}_ModifiedByUserId"").OnTable(""{table}"")
            .OnColumn(""ModifiedByUserId"");

        Create.Index(""IX_{table}_CreatedByUserId"").OnTable(""{table}"")
            .OnColumn(""CreatedByUserId""); 
            
            
        Create.ForeignKey(""FK_{table}_CreatedByUser"").FromTable(""{table}"").ForeignColumns(""CreatedByUserId"")
            .ToTable(""User"").PrimaryColumns(""UserId"");
        
        Create.ForeignKey(""FK_{table}_ModifiedByUser"").FromTable(""{table}"").ForeignColumns(""ModifiedByUserId"")
            .ToTable(""User"").PrimaryColumns(""UserId"");
            ";

    System.Console.WriteLine(str);
});

Task(generateColumnMig)
.Does(() =>
{
    var table = Argument("table","");
    if (table == null || table.Trim() == "")
    {
        throw new Exception("Table parameter is not specified");
    }

    var column = Argument("column","");
    if (column == null || column.Trim() == "")
    {
        throw new Exception("Column parameter is not specified");
    }

    var fkcolumn = column;
    if (column.ToLower().StartsWith("predefined"))
    {
        fkcolumn = "EnumTypeId";
    }

    var str = @$"
        Create.Index(""IX_{table}_{column}"").OnTable(""{table}"")
            .OnColumn(""{column}"");
            
        Create.ForeignKey(""FK_{table}_{column}"").FromTable(""{table}"").ForeignColumns(""{column}"")
            .ToTable(""{fkcolumn.Replace("Id","")}"").PrimaryColumns(""{fkcolumn}"");
        ";

    System.Console.WriteLine(str);

});

Task(generateData)
.Does(() =>
{
        var doDataImportFromFile = GetArgumentValue("import-from-file","false");
        var filePath = GetArgumentValue("filepath","");
        var server = GetArgumentValue("server","");
        var companyCode = GetArgumentValue("company-code","");
        Warning($"Running Data Migration App ...");
        var code = StartProcess("dotnet", new ProcessSettings{ Arguments = $" run --project {testDataProjectFileStr} --import-from-file {doDataImportFromFile} --filepath {filePath} --company-code {companyCode}  --server {server}" });
        if (code != 0)
            throw new Exception("dotnet run errored out");

});

Task(generateDataForPS)
.Does(() =>
{
        Warning($"Running Data Migration App ...");
        var server = GetArgumentValue("server","");
        var code = StartProcess("dotnet", new ProcessSettings{ Arguments = $" run --project {testDataProjectFileStr} --import-from-file true --server {server}" });
        if (code != 0)
            throw new Exception("dotnet run errored out");

});


//////////////////////////////////////////////////////////////////////
// COMMAND PROCESS INFRASTRUCTURE
//////////////////////////////////////////////////////////////////////

class CommandProcess
{
    public CommandProcess(string commandName, string shortcut1, string shortcut2,string shortcut3, string description)
    {
        this.CommandName = commandName.Trim();
        this.Shortcut1 = shortcut1.Trim();
        this.Shortcut2 = shortcut2.Trim();
        this.Shortcut3 = shortcut3.Trim();
        this.Description = description.Trim();
    }

    public string CommandName {get; private set;}
    public string Shortcut1 {get; private set;}
    public string Shortcut2 {get; private set;}
    public string Shortcut3 {get; private set;}
    public string Description {get; private set;}

    public string Shortcuts
    {
        get
        {
            return Shortcut1 + (Shortcut2 == "" ? "" : "," + Shortcut2) +(Shortcut3 == "" ?  "" : "," + Shortcut3);
        }
    }
}

List<CommandProcess> commandList = new List<CommandProcess>()
{
    new CommandProcess(initDevEnvironment, "idev", "", "", "Initialize the dev environment"),
    new CommandProcess(generatePoco, "genp", "", "", "Generate Pocos from Db"),
    new CommandProcess(generateFlatFile, "genff", "", "", "Generate Flat File from Db"),
    new CommandProcess(generateMigration, "genm", "", "", "Generate Migration script from Db"),
    new CommandProcess(generateMigrationFromFlatFile, "genmfff", "", "", "Generate Migration script from Flat File"),
    new CommandProcess(generateCode , "genc", "", "", "Generate Poco and Migration scrips"),
    new CommandProcess(deleteCreateDb , "delcdb", "", "",  "Delete and Recreate Db Server"),
    new CommandProcess(deleteCreateDbAndGenc , "delcdb-genc", "", "",  "Delete and Recreate Db Server and Run Genc"),
    new CommandProcess(runMigration , "runm", "", "",  "Migrate the Db with latest Migration script"),
    new CommandProcess(installDbServer , "insdb", "", "", "Install Db Driver"),
    new CommandProcess(clean , "cln", "", "", "Clean the project intermediate files"),
    new CommandProcess(build , "bld", "", "", "Build the solution"),
    new CommandProcess(watchbuild , "wbld", "", "", "Build the solution"),
    new CommandProcess(test , "tst", "", "", "Run unit tests"),
    new CommandProcess(run , "r", "", "", "Run app"),
    new CommandProcess(coverage , "cov", "", "", "Run coverage"),
    new CommandProcess(debugBuild , "dbld", "", "", "Build solution in Debug Mode"),
    new CommandProcess(generateAllModels, "genmod", "", "", "Generate all external Models"),
    new CommandProcess(gitMergeWithoutCommit, "gitmnoc", "", "", "Git Merge without Commit"),
    new CommandProcess(generateCrud , "crud", "", "", "Generate Crud"),
    new CommandProcess(buildGenerateCrud , "bcrud", "", "", "Build Generator and Generate Crud"),
    new CommandProcess(generateReact , "react", "", "", "Generate React"),
    new CommandProcess(buildGenerateReact , "breact", "", "", "Build Generator and Generate React"),
    new CommandProcess(generateAngular , "angular", "", "", "Generate Angular Models"),
    new CommandProcess(buildGenerateAngular, "bangular", "", "", "Build Generator and Generate Angular Models"),
    new CommandProcess(generateUI , "ui", "", "", "Generate Angular UI"),
    new CommandProcess(buildGenerateUI, "bui", "", "", "Build Generator and Generate Angular UI"),
	new CommandProcess(generateCredList , "clst", "", "", "Generate Angular Cred list"),
    new CommandProcess(buildGenerateCredList, "bclst", "", "", "Build Generator and Generate Angular Cred list"),
	
    new CommandProcess(generateTestApiModel , "tapi", "", "", "Generate Test Api Model"),
    new CommandProcess(buildGenerateTestApiModel , "btapi", "", "", "Build Generator and Generate Test Api Model"),
    new CommandProcess(generateWordApiModel , "wapi", "", "", "Generate Word Api Model"),
    new CommandProcess(buildGenerateWordApiModel , "bwapi", "", "", "Build Generator and Generate Word Api Model"),
    new CommandProcess(generateEnumMigration , "enumm", "", "", "Generate Enum Migration"),
    new CommandProcess(buildGenerateEnumMigration , "benumm", "", "", "Build Generator and Generate Enum Migration"),
    new CommandProcess(generateAllAPiModels , "gapi", "", "", "Generate All Api Model"),
    new CommandProcess(gitStats , "gs", "gitstats", "", "Show Git checkin stats"),
    
    new CommandProcess(generateTableMig , "tm", "tmig", "", "Generate Table Migration statements"),
    new CommandProcess(generateColumnMig , "cm", "cmig", "", "Generate Column Migration statements"),
    new CommandProcess(generateAllTableMig, "atm", "atmig", "", "Generate All Table Migration statements"),
    new CommandProcess(generateSelectedTableMig, "stm", "stmig", "", "Generate Selected Table Migration statements"),

    new CommandProcess(localPgMigrate , "lpgm", "lpgmig", "", "Delete Local PG DB and Migrate"),
    new CommandProcess(checkEnumWithProd , "check-enum", "cenum", "", "Check Enum in production"),
    new CommandProcess(generateUiPill, "ui-pill", "upill","", "Generate UI pill"),
    new CommandProcess(generateUiInputBlock, "ui-input-block", "uiBlock","", "Generate UI Input Block"),

    new CommandProcess(installWeb, "install-web", "iw","", "Install Web App"),
    new CommandProcess(runWeb, "run-web", "rw","", "Run Web App"),
    new CommandProcess(newColumn, "add-col", "ac","", "Add New Column to Table in App"),
    new CommandProcess(newFunction, "add-func", "af","", "Add New Function App"),    
    new CommandProcess(generateData, "gend", "gen-data","", "Generate data"),    
    new CommandProcess(generateDataForPS, "gend-ps", "gen-data-ps","", "Generate data for PS"),
};

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var command = Argument(commandStr, "");


Task("Default")
    .IsDependentOn(customCommandStr);


Task(customCommandStr)
    .Does(() =>
{
    var command = Argument<string>(commandStr);

    if(command == null || command.Trim() == "")
    {
        if (command.Trim().Length > 0)
            Error("Invalid command");
        else
            Error("No Parameter specified");
        ShowUsage();
        return;
    }

    command = command.Trim().ToLower();

    var item = commandList.FirstOrDefault(x=> x.CommandName.Trim().ToLower() == command || x.Shortcut1.Trim().ToLower() == command || x.Shortcut2.Trim().ToLower() == command|| x.Shortcut3.Trim().ToLower() == command);
    if (item == null)
    {
        if (command.IndexOf(" ") != -1)
        {
            var cmds = command.Split(" ");
            command = cmds[0];
        }
        else
        {
            Error("Invalid command - " + command);
            ShowUsage();
            return;
        }

        item = commandList.FirstOrDefault(x=> x.CommandName.Trim().ToLower() == command || x.Shortcut1.Trim().ToLower() == command || x.Shortcut2.Trim().ToLower() == command|| x.Shortcut3.Trim().ToLower() == command);
        if (item == null)
        {
            Error("Invalid command - " + command);
            ShowUsage();
            return;
        }
    }

    RunTarget(item.CommandName);

});


void ShowUsage()
{
    Warning("=====");
    Warning("Usage");
    Warning("=====");
    Warning("ci.bat <Command>");
    int commandPadding = 30;
    int shortcutPadding = 10;

    var header = "Command".PadRight(commandPadding,' ')+" - "+"Shortcut".PadRight(shortcutPadding,' ')+" - "+"Description";
    var line = "=".PadRight(header.Length + 40,'=');

    Warning(line);
    Warning(header);
    Warning(line);
    foreach(var item in commandList)
    {
        Warning(item.CommandName.PadRight(commandPadding,' ')+" - "+item.Shortcuts.PadRight(shortcutPadding,' ')+" - "+item.Description);
    }
    Warning(line);
}

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);