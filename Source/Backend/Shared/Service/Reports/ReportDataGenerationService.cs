using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure.Interfaces.Services;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Interfaces.Service.Config;
using Manifest.Repository.Common;

namespace Manifest.Service.Report
{
    public class ReportDataGenerationService
    {
        private readonly ILog log = null;
        private readonly ReportFactoryService reportFactoryService = null;
        private readonly IProcessUtilService processUtilService;
        private readonly IReportServerConfiguration reportServerConfiguration = null;
        private readonly ReportRepository reportsRepository = null;
        private readonly IPathResolutionService pathResolutionService;

        public ReportDataGenerationService(
            ILog logger,
            IReportServerConfiguration reportServerConfiguration,
            IProcessUtilService processUtilService,
            IPathResolutionService pathResolutionService,
            ReportFactoryService reportFactoryService,
            ReportRepository reportsRepository)
        {
            log = logger;
            this.reportFactoryService = reportFactoryService;
            this.reportServerConfiguration = reportServerConfiguration;
            this.reportsRepository = reportsRepository;
            this.processUtilService = processUtilService;
            this.pathResolutionService = pathResolutionService;
        }

        public enum ReportFileName
        {
            Report,
            Invoice
        }

        public string ReportOutputFolder { get; set; }

        public string SqliteDatafilePath { get; set; }

        public string SqliteBasePath { get; set; }

        public string TemplateFilePath { get; set; }

        public string TemplateFileName { get; set; }

        public ReportCommandLineParametersModel CommandLineParameters { get; set; }

        public bool IsIntermediateDatabeseCreated { get; set; }

        public async Task<bool> Execute(long jobId)
        {
            try
            {
                bool result = false;
                string dbFileName;
                IReportService reportService = null;

                var task = await reportsRepository.GetJobTemplateByJobIdAsync(jobId);
                if (task == null)
                {
                    log.Error("Report template file path not defiend in reportserver configuration!" + jobId);
                    return false;
                }

                TemplateFileName = task.TaskIdentifierName;
                TemplateFilePath = pathResolutionService.JasperTemplatePath(TemplateFileName);
                if (TemplateFilePath.IsNullOrEmpty())
                {
                    log.Error("Couldn't find the template file path for Template." + TemplateFileName);
                    return false;
                }

                if (reportFactoryService == null)
                {
                    log.Error("Error in report factory service creation.");
                    return false;
                }

                reportService = reportFactoryService.GetReportService(task.JobId);
                string outputFolderPath;
                task.Parameters = await reportsRepository.GetAllJobParameterAsync(jobId);

                outputFolderPath = reportServerConfiguration.ReportIntermediateDataFilePath;

                var configPath = PathUtils.GetConfigPath(outputFolderPath);
                ReportOutputFolder = PathUtils.CreateMultiLevelPath(configPath, new List<string>() { DateTime.Now.Year.ToString(), MonthFromNumberAsString(DateTime.Now.Month), jobId.ToString() });
                SqliteBasePath = ReportOutputFolder;
                dbFileName = ReportFileName.Report.ToString() + ".db";
                SqliteDatafilePath = PathUtils.CreatePath(SqliteBasePath, dbFileName);

                if (reportService == null)
                {
                    log.Error("Error in creating intermediate database service for job:" + jobId);
                    return false;
                }

                (CommandLineParameters, IsIntermediateDatabeseCreated) = await reportService.CreateIntermediateDatabaseAsync(SqliteBasePath, dbFileName, task);

                if (!IsIntermediateDatabeseCreated)
                {
                    log.Error("Error found in creating datasource for job:" + jobId);
                    return false;
                }

                var report = new ReportParameterModel
                {
                    TemplateFilePath = TemplateFilePath,
                    OutputFolder = ReportOutputFolder,
                    OutputFileName = ReportFileName.Report.ToString()
                };
                if (CommandLineParameters != null)
                {
                    report.CommandLineParameters = CommandLineParameters;
                }

                report.IntermediateDataFilePath = SqliteDatafilePath;
                report.OutputFormats = OutPutFormat(task);
                result = RunJasperReport(report);

                if (!result)
                {
                    log.Error("Error in report generation for job:" + jobId);
                    return false;
                }

                return await reportService.OnReportSuccessfullyCompletedAsync(report.OutputFileName, ReportOutputFolder, jobId, task.Parameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while running the report:" + ex);
                return false;
            }
        }

        private List<JobOutputFormatTypes> OutPutFormat(Manifest.Common.Models.DBView.JobTemplateView templateView)
        {
            var listOutputReportType = new List<JobOutputFormatTypes>();

            if (templateView != null)
            {
                if (templateView.CanExportCSV == 1)
                {
                    listOutputReportType.Add(JobOutputFormatTypes.Csv);
                }

                if (templateView.CanExportDOC == 1)
                {
                    listOutputReportType.Add(JobOutputFormatTypes.Docx);
                }

                if (templateView.CanExportPDF == 1)
                {
                    listOutputReportType.Add(JobOutputFormatTypes.Pdf);
                }

                if (templateView.CanExportPPT == 1)
                {
                    listOutputReportType.Add(JobOutputFormatTypes.Pptx);
                }

                if (templateView.CanExportXLS == 1)
                {
                    listOutputReportType.Add(JobOutputFormatTypes.Xls);
                }

                if (templateView.CanExportXML == 1)
                {
                    listOutputReportType.Add(JobOutputFormatTypes.Xml);
                }
            }
            else
            {
                listOutputReportType.Add(JobOutputFormatTypes.Pdf);
            }

            return listOutputReportType;
        }

        private string MonthFromNumberAsString(int month)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }

        private bool GenerateReportInternalSqlite(ReportParameterModel reportParam)
        {
            string jasperstarterExe = string.Empty;
            var commandLineParameters = new StringBuilder();
            var path = pathResolutionService.JasperStarterExePath();
            log.Info("Jasper path:" + path);
            if (File.Exists(path))
            {
                jasperstarterExe = path;
            }

            try
            {
                if (jasperstarterExe.IsNullOrEmpty())
                {
                    log.Error("Unable to determine the Jasper Starter Executable.");
                    return false;
                }

                if (!File.Exists(reportParam.TemplateFilePath))
                {
                    log.Error("Unable to find the Template File path at  " + reportParam.TemplateFilePath);
                    return false;
                }

                if (!File.Exists(reportParam.IntermediateDataFilePath))
                {
                    log.Error("Unable to find the Data File path at  " + reportParam.IntermediateDataFilePath);
                    return false;
                }

                var workingDirectory = reportParam.OutputFolder;

                if (!Directory.Exists(workingDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(workingDirectory);
                    }
                    catch
                    {
                        log.Error("report output folder not created " + reportParam.OutputFolder);
                        return false;
                    }
                }

                if (File.Exists(reportParam.OutputFullFileName))
                {
                    File.Delete(reportParam.OutputFullFileName);
                }

                commandLineParameters.Append($" pr \"{reportParam.TemplateFilePath}\"");
                commandLineParameters.Append($" -o \"{reportParam.OutputFullFileName}\"");
                commandLineParameters.Append($" -t generic --db-driver org.sqlite.JDBC --db-url jdbc:sqlite:\"{reportParam.IntermediateDataFilePath}\" ");

                if (reportParam.CommandLineParameters != null)
                {
                    if (reportParam.CommandLineParameters.IsGroupByRequired > 0)
                    {
                        commandLineParameters.Append($" -P IsGroupingRequired={reportParam.CommandLineParameters.IsGroupByRequired} ");
                    }

                    if (reportParam.CommandLineParameters.JobStatusFilter != null)
                    {
                        commandLineParameters.Append($" JobStatusFilter=\"{reportParam.CommandLineParameters.JobStatusFilter}\" ");
                    }
                }

                foreach (var outputFormat in reportParam.OutputFormats)
                {
                    commandLineParameters.Append($" -f {outputFormat.ToString().ToLower()} ");
                    var processResult = processUtilService.ExecuteProcess(jasperstarterExe, commandLineParameters.ToString(), workingDirectory, true, true);
                    if (!processResult.Result)
                    {
                        log.Error("Error in running report process.");
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool RunJasperReport(ReportParameterModel jasperReportParam)
        {
            bool result;
            try
            {
                result = GenerateReportInternalSqlite(jasperReportParam);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to run report error encountered: " + ex.Message);
            }
        }
    }
}
