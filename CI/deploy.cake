#addin nuget:?package=Cake.Npm&version=0.17.0
#addin nuget:?package=SshNet.Security.Cryptography&version=1.3.0
#addin nuget:?package=SSH.NET&version=2020.0.2
#tool nuget:?package=GitVersion.CommandLine&version=5.12.0
#addin nuget:?package=Cake.FileHelpers&version=6.1.3

//////////////////////////////////////////////////////////////////////
// CONSTANTS
//////////////////////////////////////////////////////////////////////
const string appNameStr = "Manifest";
const string customCommandStr = "Command";
const string commandStr = "command";

static string opTempFolder = "/op-temp";
string sshKey = "";
string sshKeyFile = "";

string sshUser = "";
string sshMachine = "";
string configFilePath = "";
string nginxDefaultConfigPath = "";

const string uiDirStr = "..";
const string uiReactDirStr = "../Source/Web/Web";
const string uiReactDirDirectStr = "../Source/Web/Web";
const string appDirStr = "..";
const string appDirDirectStr = "";
const string appDirStrFromRoot = ".";

const string docDirStr = "../../app-doc/";
const string docDirDirectStr = "app-doc";
const string docDirStrFromRoot = "../app-doc/";

const string publishedPath = "../app-publish/";
const string publishedFormRootPath = "./app-publish/";

var buildDir = Directory("./build");
var publishDir = Directory("../app-publish/");
var angularDistDir = Directory("../Source/Web/Web");

var remotePath = "/app-files/apps/web/";
var zipFolderPath = "/app-files/deployments/zips/";
var remoteConfigFolderPath = "/app-files/Configuration/";
var remoteSupervisordConfigFolderPath = "/etc/supervisor/";
var remoteNginxConfDFolderPath = "/etc/nginx/conf.d/";

const string appApiIntermediatePath = "../Build/app-intermediate/apps/api";
const string uiAllIntermediatePath = "../Build/app-intermediate/ui/ui-all";

const string appApiPublishedPath = "../Build/app-published/apps/api";
const string uiAllPublishedPath = "../Build/app-published/ui/ui-all";
const string appAPiProjectFileLocation = "./Source/Backend/Api/All/Manifest.App.Api.csproj";
const string appBuildConfiguration = "Debug";
const string appConfigFileName = "ManifestConfig.json";
const string appConfigFileNameForQA = "ManifestConfig-QA.json";
const string appConfigFileNameForDemo = "ManifestConfig-Demo.json";
const string appConfigFileNameForProd = "ManifestConfig-Prod.json";
const string angularDistPath = "Manifest";
//////////////////////////////////////////////////////////////////////
// TASK CONSTANTS
//////////////////////////////////////////////////////////////////////
const string deployDoc = "deploy-doc";
const string deployUI = "deploy-ui";
const string deployApp = "deploy-app";
const string deployAll = "deploy-all";
const string deployLog = "deploy-log";

const string copyDB = "copyDB";
const string removeDB = "remove-db";
const string init = "init";
const string preInit = "pre-init";

const string deployUIOnly = "deploy-ui-only";
const string deployDocOnly = "deploy-doc-only";

//const string deployAppOnly = "deploy-app-only";
//const string deployAllOnly = "deploy-all-only";

const string buildDoc = "build-doc";
const string buildUI = "build-ui";
const string buildApp = "build-app";
const string buildLog = "build-log";
const string prepareEnv = "prepare-env";

const string deleteDbName = "deleteDb";

const string xbuild = "xbuild";
const string xbuildUi = "xbuild-ui";
const string xbuildApp = "xbuild-app";
const string xdeploy = "xdeploy";
const string xdeployOnly = "xdeploy-only";

const string xdeployQA = "xdeploy-qa";
const string xdeployQAOnly = "xdeploy-qa-deploy-only";

const string xdeployProd= "xdeploy-prod";
const string xdeployProdOnly = "xdeploy-prod-deploy-only";

const string xdeployDemo= "xdeploy-demo";
const string xdeployDemoOnly = "xdeploy-demo-deploy-only";

const string xdeployUi = "xdeploy-ui";
const string xdeployApp = "xdeploy-app";
const string terminal = "terminal";
const string terminalQA = "terminal-qa";
const string terminalQADb = "terminal-qa-db";

const string terminalProd = "terminal-prod";
const string terminalProdDb = "terminal-prod-db";

const string terminalDemo = "terminal-demo";
const string terminalDemoDb = "terminal-demo-db";

const string installNginxQA = "install-nginx-qa";
const string installNginx = "install-nginx";

const string qaServerIp = "159.89.151.159";
const string prodServerIp = "159.89.151.159";
const string demoServerIp = "159.89.151.159";

const string qaDbServerIp = "159.65.153.42";
const string prodDbServerIp = "159.65.153.42";
const string demoDbServerIp = "159.65.153.42";

//////////////////////////////////////////////////////////////////////
// INTERMEDIATE TASK CONSTANTS
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
// PATH VARIABLES
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
// APP TARGET
//////////////////////////////////////////////////////////////////////
bool deployAllExecutables = true;

var workingDirectory = MakeAbsolute (Directory ("..")).FullPath;
var backendWorkingDirectory = MakeAbsolute (Directory (appDirStr)).FullPath;
var docWorkingDirectory = MakeAbsolute (Directory (docDirStr)).FullPath;
var uiReactWorkingDirectory = MakeAbsolute (Directory (uiReactDirStr)).FullPath;


//////////////////////////////////////////////////////////////////////
// API Based SSH
//////////////////////////////////////////////////////////////////////

static void CreateDirectoryIfNotExists(Renci.SshNet.SftpClient sftpClient,string dir)
{
        if (!sftpClient.Exists(dir))
        {
            sftpClient.CreateDirectory(dir);
        }
}


static void UploadFile(Renci.SshNet.SftpClient sftpClient, string fromFile, string toLocation)
{
        Console.WriteLine($"Copying : {fromFile} to {toLocation}...");
        
        if (!System.IO.File.Exists(fromFile))
        {
            throw new Exception($" {fromFile} does not exists "); 
        }

        var start = Environment.TickCount;

        using (FileStream fs = new FileStream(fromFile, FileMode.Open))
        {
            sftpClient.BufferSize = 8 * 1024;
            sftpClient.UploadFile(fs, toLocation);
        }
        Console.WriteLine($"Took : {PrintTimeTaken(start)}");
}


static string  RenciRunSSH(Renci.SshNet.SshClient sshClient, string command , string erorMsg = "")
{
        Console.WriteLine($"Executing : {command}...");
        var start = Environment.TickCount;
        try
        {
            try
            {
                var sshCommand = sshClient.CreateCommand(command);
                sshCommand.CommandTimeout = TimeSpan.FromSeconds(100); 
                Console.WriteLine($"Timout  : {sshCommand.CommandTimeout}");
                var result = sshCommand.Execute();            
                Console.WriteLine($"After Execute - result = {result}");
                var reader = new StreamReader(sshCommand.ExtendedOutputStream);
                Console.WriteLine("DEBUG:");
                Console.Write(reader.ReadToEnd());

                if (sshCommand.ExitStatus != 0)
                {
                    if (!string.IsNullOrEmpty(erorMsg))
                        throw new Exception($"{erorMsg}.\nError" + sshCommand.Error);
                    else
                        throw new Exception($"Unable to run Command : {command}.\nError" + sshCommand.Error);
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message + " " + ex.StackTrace}");
                throw ex;
            }
        }
        finally
        {
            Console.WriteLine($"Took : {PrintTimeTaken(start)}"); 
        }
}

void RenciCleanSSHDirectory(Renci.SshNet.SshClient sshClient, string remoteDir)
{
    RenciRunSSH (sshClient, $"sudo mkdir -p {remoteDir}", $"unable to create remote directory {remoteDir}");
    RenciRunSSH (sshClient, $"sudo rm -r {remoteDir}", $"unable to remove remote directory {remoteDir}");
}

void RenciCreateSSHDirectory(Renci.SshNet.SshClient sshClient, string remoteDir)
{
    RenciRunSSH (sshClient, $"sudo mkdir -p {remoteDir}", $"unable to create remote directory {remoteDir}");
}

void RenciCreateSSHDirectory(Renci.SshNet.SftpClient sftpClient, string remoteDir)
{
    if (!sftpClient.Exists(remoteDir))
        sftpClient.CreateDirectory(remoteDir);
    //RenciRunSSH (sshClient, $"sudo mkdir -p {remoteDir}", $"unable to create remote directory {remoteDir}");
}

void RenciRunCommand(Renci.SshNet.SshClient sshClient, string cmd)
{
    try
    {
        Console.WriteLine($"RUN : {cmd}");
        sshClient.RunCommand(cmd);
    }
    catch (Exception ex)
    {
        Warning($"  Error :");
        Console.WriteLine("  " + ex.Message);
        Console.WriteLine(ex.StackTrace);
        Console.WriteLine(ex.InnerException);
        throw ex;
    }    
}

void RenciRunSCPCopyFile (Renci.SshNet.SftpClient sftpClient, string fromFile, string toServerDir) 
{
    try
    {
        Console.WriteLine($"Copying : {fromFile} to {toServerDir}...");
        var start = Environment.TickCount;

        if (!System.IO.File.Exists(fromFile))
        {
            Console.WriteLine($"Unable to find File in location - {fromFile}");
            throw new Exception($"Unable to find File in location - {fromFile}");
        }

        Console.WriteLine($"About to Open Local File...");
        using (FileStream fs = new FileStream(fromFile, FileMode.Open))
        {
            Console.WriteLine($"Opened Local File...and About to Transfer the File to Remote...");
            sftpClient.BufferSize = 8 * 1024;
            sftpClient.UploadFile(fs, toServerDir,null);
        }
        Console.WriteLine($"Took : {PrintTimeTaken(start)}");
        //var cmdLine = $"{sshKey} {fromFile} {sshUser}@{sshMachine}:{toServerDir}";
        //RunExecutable ("scp.exe", new ProcessSettings { Arguments = cmdLine, WorkingDirectory = workingDirectory });
    }
    catch (Exception ex)
    {
        Warning($"  Error :");
        Console.WriteLine("  " + ex.Message);
        Console.WriteLine(ex.StackTrace);
        Console.WriteLine(ex.InnerException);
        throw ex;
    }

}

void RunExecutable (string executable, ProcessSettings settings) {
    var argValue = "";
    if (settings.Arguments != null)
        argValue = settings.Arguments.Render();

    Warning ($"Running - {executable} , parameter - {argValue} , WorkingDirectory = {settings.WorkingDirectory} ");
    if (DirectoryExists (settings.WorkingDirectory) == false)
        throw new Exception ($"Unable to find the working directory {settings.WorkingDirectory} to run {executable} - cmdLine = {settings.Arguments.Render()}, workingDirectory = {settings.WorkingDirectory}");

    var code = StartProcess (executable, settings);
    if (code != 0)
        throw new Exception ($"Unable to run {executable} - cmdLine = {argValue}, workingDirectory = {settings.WorkingDirectory}");
}

//////////////////////////////////////////////////////////////////////
// APP TARGET
//////////////////////////////////////////////////////////////////////

Task (prepareEnv)
    .IsDependentOn(init)
    .Does (() => {
        //RunSSH("sudo apt-get install wget apt-transport-https supervisor unzip apache2-utils libcairo2-dev default-jre ");
        //RunSSH("wget -q https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb && sudo dpkg -i packages-microsoft-prod.deb");
        //RunSSH("sudo add-apt-repository universe");
        
        //RunSSH("sudo apt-get update");
        //RunSSH("sudo apt-get install dotnet-sdk-8.0");
        //RunSSH("sudo apt install nginx");
        //RunSSH("sudo ufw app list");
        //RunSSH("sudo ufw allow 'Nginx HTTP'");
        //RunSSH("sudo ufw status");
        //RunSSH("sudo apt install libgssapi3-heimdal"); //for sql server
        
        //RunSSH("sudo htpasswd -c -b /etc/nginx/.htpasswd opuser1 opuser1");
        //RunSSH("sudo add-apt-repository ppa:libreoffice/libreoffice-7-0");
        //RunSSH("sudo add-apt-repository ppa:libreoffice/libreoffice 7.0.0.1");
        //RunSSH("wget http://downloadarchive.documentfoundation.org/libreoffice/old/7.1.0.1/deb/x86_64/LibreOffice_7.1.0.1_Linux_x86-64_deb.tar.gz");
        //RunSSH("tar zxvf LibreOffice_7.1.0.1_Linux_x86-64_deb.tar.gz");

        //RunSSH("cd LibreOffice_7.1.0.1_Linux_x86-64_deb/DEBS");
        //RunSSH("sudo dpkg -i *.deb");
        //RunSSH("sudo ln -s /opt/libreoffice7.1/program/soffice /usr/bin/soffice");

	//Manual - Open /etc/ssh/sshd_config.d/allow_ssh-rsa.conf
	// echo "PubkeyAcceptedAlgorithms +ssh-rsa" >> /etc/ssh/sshd_config.d/allow_ssh-rsa.conf ; systemctl restart sshd
    //Open - /etc/postgresql/16/main/pg_hba.conf
    //add  host    all             all             all                     md5 
    // do - sudo service postgresql restart
    //Open - /etc/postgresql/16/main/postgresql.conf
    // change listen_addresses = '*'       
});


Task ("test")
    .Does (() => {
        //RunSCP(workingDirectory+"/./configurations/ngnix/default.conf","~/");
        //RunSSH("sudo mv ~/default.conf /etc/nginx/conf.d/");
        //RunSSH("sudo nginx -t");
        //RunSSH("sudo service nginx restart");
        //RunSSH("sudo wget http://localhost:8080/api/v1/admin/userlist");
    });

Task (preInit)
    .Does (() => {

        var arg =  Argument<string>("ssh-key", "");
        if (arg == "" && sshKeyFile != "")
            arg = sshKeyFile;
        sshKeyFile = arg;

        if (sshKeyFile == null || sshKeyFile.Trim() == "")
        {
            throw new Exception("SSH Key is needed \n Use --ssh-key <FileName>");
        }
        
        Information($" sshKeyFile = {sshKeyFile}");

        sshKeyFile = workingDirectory + "/Build/SshKeys/" + sshKeyFile;

        Information($" Full sshKeyFile = {sshKeyFile}");

        //sshKeyFile = System.IO.Path.GetFileName(sshKeyFile);

        //Information($" Get FileName sshKeyFile = {sshKeyFile}");

        if (!System.IO.File.Exists(sshKeyFile))
        {
            throw new Exception($"SSH Key does not exists in {sshKeyFile}\n Use --ssh-key <FileName>");
        }
        
        sshKey = " -i " + sshKeyFile;

        arg =  Argument<string>("ssh-user", "");
        if (arg == "" && sshUser != "")
            arg = sshUser;
        sshUser = arg;

        arg = Argument<string>("ssh-machine", "");
        if (arg == "" && sshMachine != "")
            arg = sshMachine;
        sshMachine = arg;

        if (sshUser == null || sshUser.Trim() == "")
        {
            throw new Exception("SSH User is needed\n Use --ssh-user <username>");
        }

        if (sshMachine == null || sshMachine.Trim() == "")
        {
            throw new Exception("SSH Machine is needed\n Use --ssh-machine <machinename>");
        }

        Information($"Using following SSH Paramters : ");
        Information($"   SSH Key : {sshKeyFile}");
        Information($"   SSH User : {sshUser}");
        Information($"   SSH Machine : {sshMachine}");
        Information($"");
        Information($"Validating SSH Connectivity :");        
       
    });

Task (init)
    .IsDependentOn(preInit)
    .Does (() => {
        var pKeyfile = new Renci.SshNet.PrivateKeyFile(sshKeyFile);
        try
        {
            using (var sftpClient = new Renci.SshNet.SftpClient(sshMachine, sshUser, new Renci.SshNet.PrivateKeyFile[] { pKeyfile }))
            {
                sftpClient.Connect();
                Information($"SSH Connection is good.");
            }
        }
        catch (Exception ex)
        {
            Warning($"SSH Connection is not good.");
            Warning($"  Error :");
            Console.WriteLine("  " + ex.Message);
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine(ex.InnerException);
            throw ex;
        }
    });


static string PrintTimeTaken(long oldTicks)
{
    var ts = TimeSpan.FromMilliseconds(Environment.TickCount - oldTicks);
    return $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
}


Task("App-Clean-Publish")
    .Does(() =>
{
    CleanDirectory(appApiPublishedPath);
    CleanDirectory(appApiIntermediatePath);
});

Task("UI-Clean-Publish")
    .Does(() =>
{
    CleanDirectory(uiAllPublishedPath);
    CleanDirectory(uiAllIntermediatePath);
});

string GetVersionWithDate()
{
	try
	{
     var versionInfo = GitVersion(new GitVersionSettings{
         ArgumentCustomization = args => args.Append("/overrideconfig tag-prefix=test /overrideconfig mode=ContinuousDeployment")
     });

     return versionInfo.MajorMinorPatch+"."+ versionInfo.PreReleaseNumber + " (" + DateTime.Now.ToShortDateString() +  ")";
	}
	catch
	{
		return $"{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}";
	}
}

Task(xbuildApp)
    .IsDependentOn("App-Clean-Publish")
    .Does(() =>
{
    DotNetClean(backendWorkingDirectory + "/" + appAPiProjectFileLocation, new DotNetCleanSettings {
        Configuration = appBuildConfiguration,
    });

    DotNetPublish( backendWorkingDirectory + "/" + appAPiProjectFileLocation, new DotNetPublishSettings {
        Configuration = appBuildConfiguration,
        OutputDirectory = appApiIntermediatePath,
        NoBuild = false,
        //PublishSingleFile = true,
        //SelfContained = true,
        //PublishReadyToRun = true,
        //IncludeAllContentForSelfExtract = true,
        //IncludeNativeLibrariesForSelfExtract = true
    });
    
    FileWriteText($"{appApiIntermediatePath}/version_number.txt", GetVersionWithDate());

    if (!System.IO.Directory.Exists($"{appApiIntermediatePath}/Data"))
    {
        Console.WriteLine($"Letter Folder Data does not exists in - {appApiIntermediatePath}/Data");
        System.IO.Directory.CreateDirectory($"{appApiIntermediatePath}/Data");
    }

    Console.WriteLine($"Copy word and excel template files from - {backendWorkingDirectory}/Source/Data/");
    CopyFile(backendWorkingDirectory + "/Source/Data/invoice_template.docx", $"{appApiIntermediatePath}/Data/invoice_template.docx");
    CopyFile(backendWorkingDirectory + "/Source/Data/voucher_template.docx", $"{appApiIntermediatePath}/Data/voucher_template.docx");
    CopyFile(backendWorkingDirectory + "/Source/Data/TallySampleFormatFile.xlsx", $"{appApiIntermediatePath}/Data/TallySampleFormatFile.xlsx");

    Zip(appApiIntermediatePath, $"{appApiPublishedPath}/app-api-all.zip");
});

Task(xbuildUi)
    .IsDependentOn("UI-Clean-Publish")
    .Does(() =>
{
    NpmRunScript (new NpmRunScriptSettings { ScriptName = "fi", WorkingDirectory = uiReactWorkingDirectory });
    NpmRunScript (new NpmRunScriptSettings { ScriptName = "build", WorkingDirectory = uiReactWorkingDirectory });
    Zip(uiReactWorkingDirectory+$"/dist/{angularDistPath}/browser/", $"{uiAllPublishedPath}/ui-all.zip");
});

Task(xdeployProdOnly)
    .Does(() =>
{
    sshKeyFile = "google_compute_engine";
    sshUser = "root";
    sshMachine = prodServerIp;
    configFilePath = appConfigFileNameForProd;
    RunTarget (xdeployOnly);
});


Task(xdeployProd)
    .Does(() =>
{
    sshKeyFile = "google_compute_engine";
    sshUser = "root";
    sshMachine = prodServerIp;
    configFilePath = appConfigFileNameForProd;
    RunTarget (xdeploy);
});

Task(terminalProd)
    .Does(() =>
{
    sshKeyFile = "google_compute_engine";
    sshUser = "root";
    sshMachine = prodServerIp;
    RunTarget (terminal);
});

Task(terminalProdDb)
    .Does(() =>
{
    sshKeyFile = "google_compute_engine";
    sshUser = "root";
    sshMachine = prodDbServerIp;
    RunTarget (terminal);
});


Task(xdeployDemoOnly)
    .Does(() =>
{
    sshKeyFile = "google_compute_engine";
    sshUser = "root";
    sshMachine = demoServerIp;
    configFilePath = appConfigFileNameForDemo;
    RunTarget (xdeployOnly);
});


Task(xdeployDemo)
    .Does(() =>
{
    sshKeyFile = "google_compute_engine";
    sshUser = "root";
    sshMachine = demoServerIp;
    configFilePath = appConfigFileNameForDemo;
    RunTarget (xdeploy);
});


Task(terminalDemo)
    .Does(() =>
{
    sshKeyFile = "google_compute_engine";
    sshUser = "root";
    sshMachine = demoServerIp;
    RunTarget (terminal);
});

Task(terminalDemoDb)
    .Does(() =>
{
    sshKeyFile = "google_compute_engine";
    sshUser = "root";
    sshMachine = demoDbServerIp;
    RunTarget (terminal);
});


Task(xdeployQAOnly)
    .Does(() =>
{
    sshKeyFile = "google_compute_engine";
    sshUser = "root";
    sshMachine = qaServerIp;
    configFilePath = appConfigFileNameForQA;
    nginxDefaultConfigPath = "qa.default.conf";
    RunTarget (xdeployOnly);
});


Task(xdeployQA)
    .Does(() =>
{
    sshKeyFile = "google_compute_engine";
    sshUser = "root";
    sshMachine = qaServerIp;
    configFilePath = appConfigFileNameForQA;
    nginxDefaultConfigPath = "qa.default.conf";
    RunTarget (xdeploy);
});

Task(terminalQA)
    .Does(() =>
{
    sshKeyFile = "google_compute_engine";
    sshUser = "root";
    sshMachine = qaServerIp;
    RunTarget (terminal);
});

Task(terminalQADb)
    .Does(() =>
{
    sshKeyFile = "google_compute_engine";
    sshUser = "root";
    sshMachine = qaDbServerIp;
    RunTarget (terminal);
});

Task(xdeploy)
    .IsDependentOn(init)
    .IsDependentOn(xbuildUi)
    .IsDependentOn(xbuildApp)
    .IsDependentOn(xdeployOnly)
    .Does(() =>
    {
    });


Task(xdeployOnly)
    .IsDependentOn(init)
    .Does(() =>
    {
        var configFile = Argument<string>("app-config", "");
        if (configFile == "" && configFilePath != "")
        {
            configFile = configFilePath; // to support the argument passage from another Target
        }

        if (configFile == null || configFile.Trim() == "")
        {
            throw new Exception("Deployment Configuration needed");
        }

        configFilePath = workingDirectory+"/"+appDirDirectStr+"/Configuration/"+configFile;
        
        if (!System.IO.File.Exists(configFilePath))
        {
            throw new Exception($"configFilePath = {configFilePath}");
        }

        Information($" configFilePath = {configFilePath}");

        var pKeyfile = new Renci.SshNet.PrivateKeyFile(sshKeyFile);
        using (var sshClient = new Renci.SshNet.SshClient(sshMachine, sshUser, new Renci.SshNet.PrivateKeyFile[] { pKeyfile }))
        {

            Warning($"Connecting to SSH Server...");
            sshClient.Connect();
            RenciRunCommand(sshClient,$"rm -rf {zipFolderPath}*"); // Clean the zip folder directory
            RenciRunCommand(sshClient,$"mkdir -p {zipFolderPath}*"); // Clean the zip folder directory
            RenciRunCommand(sshClient,$"sudo service supervisor stop"); // Clean the zip folder directory

            using (var sftpClient = new Renci.SshNet.SftpClient(sshMachine, sshUser, new Renci.SshNet.PrivateKeyFile[] { pKeyfile }))
            {
                sftpClient.Connect();                
                RenciRunSCPCopyFile(sftpClient, configFilePath, zipFolderPath + appConfigFileName);
                RenciRunSCPCopyFile(sftpClient, workingDirectory+"/Build/configs/supervisord/supervisord.conf", zipFolderPath + "supervisord.conf");
                RenciRunSCPCopyFile(sftpClient, appApiPublishedPath +"/app-api-all.zip", zipFolderPath + "app-api-all.zip");
                RenciRunSCPCopyFile(sftpClient, uiAllPublishedPath + "/ui-all.zip", zipFolderPath + "ui-all.zip");

                if (!string.IsNullOrEmpty(nginxDefaultConfigPath))
                {
                    RenciRunSCPCopyFile(sftpClient, workingDirectory+"/Build/configs/ngnix/" + nginxDefaultConfigPath, zipFolderPath + "nginx-default.conf");
                }

                sftpClient.Disconnect();
            }
            Warning($"Completed Uploading files...");
            sshClient.Disconnect();
        }
        
        using (var sshClient = new Renci.SshNet.SshClient(sshMachine, sshUser, new Renci.SshNet.PrivateKeyFile[] { pKeyfile }))
        {
            Warning($"Continuing to execute commands in SSH Server...");
            sshClient.Connect();        
            RenciRunCommand(sshClient,$"rm -rf {remotePath}"); // Clean the remote directory
            RenciRunCommand(sshClient,$"mkdir -p {remotePath}"); // Clean the remote directory
            RenciRunCommand(sshClient,$"mkdir -p {remoteConfigFolderPath}"); // Clean the remote directory

            RenciRunCommand(sshClient,$"cp -f {zipFolderPath + appConfigFileName} {remoteConfigFolderPath + appConfigFileName}");
            RenciRunCommand(sshClient,$"cp -f {zipFolderPath + "supervisord.conf"} {remoteSupervisordConfigFolderPath + "supervisord.conf"}");
            
            RenciRunCommand(sshClient,$"unzip -o {zipFolderPath}/app-api-all.zip -d {remotePath}");            
            RenciRunCommand(sshClient,$"unzip -o {zipFolderPath}/ui-all.zip -d {remotePath}wwwroot/");
            RenciRunCommand(sshClient,$"sudo service supervisor start"); // Clean the zip folder directory

            if (!string.IsNullOrEmpty(nginxDefaultConfigPath))
            {
                RenciRunCommand(sshClient,$"sudo mkdir -p {remoteNginxConfDFolderPath}");
                RenciRunCommand(sshClient,$"cp -f {zipFolderPath}nginx-default.conf {remoteNginxConfDFolderPath}default.conf");
                RenciRunCommand(sshClient,$"sudo nginx -t");
                RenciRunCommand(sshClient,$"sudo service nginx restart");
            }

            sshClient.Disconnect();
        }
    });

Task(installNginxQA)
    .Does(() =>
{
    sshKeyFile = "google_compute_engine";
    sshUser = "root";
    sshMachine = qaServerIp;
    RunTarget(installNginx);
});

Task(installNginx)
    .IsDependentOn(init)
    .Does(() =>
{
    var pKeyfile = new Renci.SshNet.PrivateKeyFile(sshKeyFile);
    using (var sshClient = new Renci.SshNet.SshClient(sshMachine, sshUser, new Renci.SshNet.PrivateKeyFile[] { pKeyfile }))
    {
        sshClient.Connect();
        RenciRunCommand(sshClient, "sudo apt-get update");
        RenciRunCommand(sshClient, "sudo apt-get install -y nginx");
        RenciRunCommand(sshClient, "sudo rm -f /etc/nginx/sites-enabled/default");
        RenciRunCommand(sshClient, "sudo ufw allow 'Nginx HTTP'");
        RenciRunCommand(sshClient, "sudo systemctl enable nginx");
        RenciRunCommand(sshClient, "sudo systemctl start nginx");
        sshClient.Disconnect();
    }
});

Task(terminal)
.IsDependentOn(preInit)
.Does(() =>
{
    var cmdLine = $"-i {sshKeyFile} {sshUser}@{sshMachine}";
    RunExecutable("ssh", new ProcessSettings { Arguments = cmdLine, WorkingDirectory = workingDirectory });
});

//////////////////////////////////////////////////////////////////////
// COMMAND PROCESS INFRASTRUCTURE
//////////////////////////////////////////////////////////////////////

class CommandProcess {
    public CommandProcess (string commandName, string shortcut1, string shortcut2, string shortcut3, string description) {
        this.CommandName = commandName.Trim ();
        this.Shortcut1 = shortcut1.Trim ();
        this.Shortcut2 = shortcut2.Trim ();
        this.Shortcut3 = shortcut3.Trim ();
        this.Description = description.Trim ();
    }

    public string CommandName { get; private set; }
    public string Shortcut1 { get; private set; }
    public string Shortcut2 { get; private set; }
    public string Shortcut3 { get; private set; }
    public string Description { get; private set; }

    public string Shortcuts {
        get {
            return Shortcut1 + (Shortcut2 == "" ? "" : "," + Shortcut2) + (Shortcut3 == "" ? "" : "," + Shortcut3);
        }
    }
}

List<CommandProcess> commandList = new List<CommandProcess> () {
   new CommandProcess (deployUIOnly, "dui-only", "", "", "Only Deploy UI w/o build"),
    
    new CommandProcess (deployDoc, "ddoc", "", "", "Deploy Docs"),
    new CommandProcess (deployUI, "dui", "", "", "Deploy UI"),
    new CommandProcess (deployApp, "dapp", "", "", "Deploy App"),
    new CommandProcess (deployAll, "dall", "", "", "Deploy All"),
    new CommandProcess (deployAll, "dlog", "", "", "Deploy Log"),

    new CommandProcess (buildDoc, "bdoc", "", "", "Build Docs"),
    new CommandProcess (buildUI, "bui", "", "", "Build UI"),
    new CommandProcess (buildApp, "bapp", "", "", "Build App"),
    new CommandProcess (buildLog, "blog", "", "", "Build Log"),
    new CommandProcess (prepareEnv, "penv", "", "", "Prepare Environment"),
    new CommandProcess (copyDB, "cdb", "", "", "Copy backend sqlite db"),
    new CommandProcess (removeDB, "deldb", "", "", "Delete backend sqlite db"),

    new CommandProcess (xbuild, "xb", "", "", "Build UI and App"),    
    
    new CommandProcess (xbuildUi, "xbui", "", "", "Build UI"),
    new CommandProcess (xbuildApp, "xbapp", "", "", "Build App"),
    new CommandProcess (xdeploy, "xd", "", "", "Deploy UI and App"),
    new CommandProcess (xdeployOnly, "xd-only", "", "", "Deploy Previously built UI and App"),
    new CommandProcess (terminal, "term", "t", "", "Terminal"),

    new CommandProcess (xdeployQA, "xd-qa", "", "", "Build Deploy to QA"),
    new CommandProcess (xdeployQAOnly, "xd-qa-dep-only", "", "", "Only Deploy to QA"),

    new CommandProcess (xdeployProd, "xd-prod", "", "", "Build Deploy to Prod"),
    new CommandProcess (xdeployProdOnly, "xd-prod-dep-only", "", "", "Only Deploy to Prod"),

    new CommandProcess (xdeployDemo, "xd-demo", "", "", "Build Deploy to Demo"),
    new CommandProcess (xdeployDemoOnly, "xd-demo-dep-only", "", "", "Only Deploy to Demo"),

    new CommandProcess (terminalQA, "term-qa", "tqa", "", "Terminal QA"),
    new CommandProcess (terminalQADb , "term-qa-db", "tqadb", "", "Terminal QA DB"),


    new CommandProcess (terminalProd, "term-prod", "tprod", "", "Terminal Prod"),
    new CommandProcess (terminalProdDb , "term-prod-db", "tproddb", "", "Terminal Prod DB"),

    new CommandProcess (terminalDemo, "term-demo", "tdemo", "", "Terminal Demo"),
    new CommandProcess (terminalDemoDb , "term-demo-db", "tdemodb", "", "Terminal Demo DB"),

    new CommandProcess (installNginxQA, "inst-nginx-qa", "", "", "Install Nginx on QA"),

    new CommandProcess ("test", "test", "", "", "test command"),
};

var target = Argument ("target", "Default");
var configuration = Argument ("configuration", "Release");
var command = Argument (commandStr, "");

Task ("Default")
    .IsDependentOn (customCommandStr);

Task (customCommandStr)
    .Does (() => {
        var command = Argument<string> (commandStr);

        if (command == null || command.Trim () == "") {
            if (command.Trim ().Length > 0)
                Error ("Invalid command");
            else
                Error ("No Parameter specified");
            ShowUsage ();
            return;
        }

        command = command.Trim ().ToLower ();

        var item = commandList.FirstOrDefault (x => x.CommandName.Trim ().ToLower () == command || x.Shortcut1.Trim ().ToLower () == command || x.Shortcut2.Trim ().ToLower () == command || x.Shortcut3.Trim ().ToLower () == command);
        if (item == null) {
            Error ("Invalid command - " + command);
            ShowUsage ();
            return;
        }

        RunTarget (item.CommandName);

    });

void ShowUsage () {
    Warning ("=====");
    Warning ("Usage");
    Warning ("=====");
    Warning ("ci.bat <Command>");
    int commandPadding = 20;
    int shortcutPadding = 10;

    var header = "Command".PadRight (commandPadding, ' ') + " - " + "Shortcut".PadRight (shortcutPadding, ' ') + " - " + "Description";
    var line = "=".PadRight (header.Length + 30, '=');

    Warning (line);
    Warning (header);
    Warning (line);
    foreach (var item in commandList) {
        Warning (item.CommandName.PadRight (commandPadding, ' ') + " - " + item.Shortcuts.PadRight (shortcutPadding, ' ') + " - " + item.Description);
    }
    Warning (line);
}

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget (target);