using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Framework.Infrastructure.Handlers;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.DocumentHandlers;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using static Manifest.Common.Constants.ConfigurationConstants;

namespace Manifest.Service.App
{
    public class DocumentConverterService
    {
        private readonly ILog log;

        public DocumentConverterService(ILog log)
        {
            this.log = log;
        }

        public ReturnModel<bool> ConvertDocument(string inputFile, string outputFile)
        {
            try
            {
                //If input file does not exists, return error.
                if (!File.Exists(inputFile))
                {
                    return log.ErrorAndReturnModel<bool>($"Input file does not exists.");
                }

                //If output file aleady exists, return error.
                if (File.Exists(outputFile))
                {
                    if (!FileUtils.DeleteFileSafely(outputFile))
                        return log.ErrorAndReturnModel<bool>($"Output file already exists.");
                }

                //If the output folder does not exists, create the folder.
                if (!Directory.Exists(Path.GetDirectoryName(outputFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                }

                var commandArgs = new List<string>();
                string libreOfficePath = GetLibreOfficePath();
                string convertedFile = Path.ChangeExtension(inputFile, Path.GetExtension(outputFile));

                if (libreOfficePath == "")
                {
                    return log.ErrorAndReturnModel<bool>($"Could not find Libre office.", new { libreOfficePath });
                }

                if (!inputFile.ToLower().EndsWith(".docx"))
                {
                    return log.ErrorAndReturnModel<bool>($"Input file type is not supported.");
                }

                if (!(outputFile.ToLower().EndsWith(".htm") || outputFile.ToLower().EndsWith(".html") || outputFile.ToLower().EndsWith(".pdf")))
                {
                    return log.ErrorAndReturnModel<bool>($"Output file type is not supported.");
                }

                commandArgs.Add("--convert-to");

                if (inputFile.ToLower().EndsWith(".docx") && outputFile.ToLower().EndsWith(".pdf"))
                {
                    commandArgs.Add("pdf:writer_pdf_Export");
                }
                else if (inputFile.ToLower().EndsWith(".docx") && (outputFile.ToLower().EndsWith(".html") || outputFile.ToLower().EndsWith(".htm")))
                {
                    commandArgs.Add("html:HTML:EmbedImages");
                }

                /*
                else if ((inputFile.EndsWith(".html") || inputFile.EndsWith(".htm")) && outputFile.EndsWith(".pdf"))
                {
                    commandArgs.Add("pdf:writer_pdf_Export");
                    //commandString = String.Format("--convert-to pdf:writer_pdf_Export {1} --outdir {0}", System.IO.Path.GetDirectoryName(pdfFile), inputFile);
                    convertedFile = Path.Combine(tmpFolder, Path.GetFileNameWithoutExtension(inputFile) + ".pdf");

                }

                else if (inputFile.EndsWith(".docx") && (outputFile.EndsWith(".html") || outputFile.EndsWith(".htm")))
                {
                    commandArgs.Add("html:HTML:EmbedImages");
                    convertedFile = Path.Combine(tmpFolder, Path.GetFileNameWithoutExtension(inputFile) + ".html");
                }
                else if ((inputFile.EndsWith(".html") || inputFile.EndsWith(".htm")) && (outputFile.EndsWith(".docx")))
                {
                    commandArgs.Add("docx:\"Office Open XML Text\"");
                    convertedFile = Path.Combine(tmpFolder, Path.GetFileNameWithoutExtension(inputFile) + ".docx");
                }
                */
                commandArgs.AddRange(new[] { inputFile, "--norestore", "--writer", "--headless", "--outdir", Path.GetDirectoryName(convertedFile) });

                var procStartInfo = new ProcessStartInfo(libreOfficePath);
                foreach (var arg in commandArgs)
                {
                    procStartInfo.ArgumentList.Add(arg);
                }

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                procStartInfo.WorkingDirectory = Environment.CurrentDirectory;

                log.Debug($"Starting OpenOffice Executable - {libreOfficePath} {string.Join(" ", procStartInfo.ArgumentList)}");

                var process = new Process() { StartInfo = procStartInfo };

                process.Start();
                process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                process.WaitForExit(1000 * 60 * 60 * 3);    // Wait for a minute

                // Check for failed exit code.
                if (process.ExitCode != 0)
                {
                    string errorCode = process.ExitCode.ToString();
                    return log.ErrorAndReturnModel<bool>($"Unexpected error while converting the document. InputFile: {inputFile} Error Code : {errorCode} ");
                    //throw new Exception(process.ExitCode.ToString());
                }
                else
                {
                    //By default, the pdf will be generated with the name of docx file. Hence, renaming the file to the desired name.
                    if (File.Exists(convertedFile))
                    {
                        File.Move(convertedFile, outputFile);
                    }
                    else
                    {
                        return log.ErrorAndReturnModel<bool>($"Unexpected error while converting the document. Pdf file not generated. ConvertedFile not found. ConvertedFile: {convertedFile} InputFile: {inputFile} OutputFile: {outputFile} ");
                    }

                    if (!File.Exists(outputFile))
                    {
                        return log.ErrorAndReturnModel<bool>($"Unexpected error while converting the document. Pdf file not generated. OutputFile not found. ConvertedFile: {convertedFile} InputFile: {inputFile} OutputFile: {outputFile} ");
                    }
                }

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error while converting the document.", new { inputFile }, ex);
            }
        }

        public void GenerateReportFromDocxToPdf(string sourcePath, string pdfTarget, Placeholders rep)
        {
            var docx = new DocXHandler(sourcePath, rep);
            var ms = docx.ReplaceAll();
            var tmpFile = FileUtils.GetRandomFileNameInTempDirectory();
            var tmpDocFile = Path.ChangeExtension(tmpFile, ".docx");
            FileUtils.WriteMemoryStreamToDisk(ms, tmpDocFile);
            ConvertDocument(tmpDocFile, pdfTarget);
            File.Delete(tmpDocFile);
        }

        private static string GetLibreOfficePath()
        {
            var path = "";
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    {
                        path = "/usr/bin/soffice";
                        break;
                    }

                case PlatformID.Win32NT:
                    {
                        path = GetWindowsOfficePath();
                        break;
                    }
            }

            if (File.Exists(path))
                return path;

            return "";
        }

        private static string GetWindowsOfficePath()
        {
            string binaryDirectory = DocumentConverter.DefaultWindowsLibreOfficeExecutable;
            if (File.Exists(binaryDirectory))
            {
                return binaryDirectory;
            }

            //If the executable is not found in the default installation path, iterate the current path
            string applicationPath = FileUtils.GetApplicationExeDirectory();

            while (true)
            {
                binaryDirectory = Path.Combine(applicationPath, DocumentConverter.LocalLibreOfficeExecutable);
                if (File.Exists(binaryDirectory))
                {
                    return binaryDirectory;
                }
                else if (Path.GetPathRoot(applicationPath).ToString() == applicationPath)
                {
                    break;
                }
                else
                {
                    applicationPath = Directory.GetParent(applicationPath).ToString();
                }
            }

            applicationPath = FileUtils.GetApplicationExeDirectory();
            while (true)
            {
                binaryDirectory = Path.Combine(applicationPath, DocumentConverter.LocalLibreOfficeExecutableDev);
                if (File.Exists(binaryDirectory))
                {
                    return binaryDirectory;
                }
                else if (Path.GetPathRoot(applicationPath).ToString() == applicationPath)
                {
                    return "";
                }
                else
                {
                    applicationPath = Directory.GetParent(applicationPath).ToString();
                }
            }
        }
    }
}