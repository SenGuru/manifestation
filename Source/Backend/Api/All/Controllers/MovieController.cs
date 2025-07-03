using Manifest.Common.Enums;
using Manifest.Common.Models.DBView;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.ViewModels;
using Manifest.Web.Filters;
using Framework.Infrastructure.Models.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Framework.Infrastructure.Models.Search;
using Manifest.Common.Interfaces.Repository.DbAccess;
using Framework.Infrastructure.Logging;
using Swashbuckle.AspNetCore.Annotations;
using Framework.Infrastructure.Models;
using Framework.Infrastructure.Interfaces.Services;
using Framework.Infrastructure.Interfaces.Helpers;
using Manifest.Service.App;
using Manifest.Web.Models;
using Manifest.Service.Storage;
using Framework.Infrastructure.Utils;
using System.IO;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.ViewModels;
using System.Net;
using Manifest.Common.Models.ViewModels.ImportExportModels;
using Manifest.Common.Models.DB;

namespace Manifest.Api.All.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public partial class MovieController : BaseController
    {
        private readonly MovieService movieService;
        private readonly DummyDataGenerationService dummyDataGenerationService;
        private readonly CompanyService companyService;
        public MovieController(CompanyService companyService, MovieService movieService, DummyDataGenerationService dummyDataGenerationService, ILog log)
            : base(log)
        {
            this.companyService = companyService;
            this.movieService = movieService;
            this.dummyDataGenerationService = dummyDataGenerationService;
        }

        #region FileAttachmentFolder
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FileAttachmentFolderModel>> GetFileAttachmentFolders([FromQuery] FileAttachmentFolderSearchCriteria search)
        {
            return movieService.GetFileAttachmentFoldersAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<FileAttachmentFolderModel>> GetFileAttachmentFolder([FromQuery] long id)
        {
            return movieService.GetFileAttachmentFolderByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<FileAttachmentFolderModel>> CreateFileAttachmentFolder([FromBody] FileAttachmentFolderCreateModel fileAttachmentFolderModel)
        {
            return movieService.CreateFileAttachmentFolderAsync(fileAttachmentFolderModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FileAttachmentFolderModel>> CreateMultipleFileAttachmentFolder([FromBody] MultipleFileAttachmentFolderCreateModel fileAttachmentFolderModels)
        {
            return movieService.CreateMultipleFileAttachmentFolderAsync(fileAttachmentFolderModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<FileAttachmentFolderModel>> UpdateFileAttachmentFolder([FromBody] FileAttachmentFolderUpdateModel fileAttachmentFolderModel)
        {
            return movieService.UpdateFileAttachmentFolderAsync(fileAttachmentFolderModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkFileAttachmentFolderAsDeleted([FromQuery] long id)
        {
            return movieService.MarkFileAttachmentFolderAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkFileAttachmentFolderAsNotDeleted([FromBody] long id)
        {
            return movieService.MarkFileAttachmentFolderAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsFileAttachmentFolderFileValid([FromQuery] long fileAttachmentFolderId)
        {
            return movieService.IsFileAttachmentFolderFileValidAsync(fileAttachmentFolderId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadFileAttachmentFolderFile([FromQuery] long fileAttachmentFolderId)
        {
            var result = await movieService.GetFileAttachmentFolderFileAsync(fileAttachmentFolderId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadFileAttachmentFolderFile([FromForm] FileAttachmentFolderFileWithFileFormModel model)
        {
            FileStreamModel streamModel = null;
            if (model.UploadedFile != null)
            {
                streamModel = new FileStreamModel
                {
                    FileData = model.UploadedFile.OpenReadStream(),
                    ContentType = model.UploadedFile.ContentType,
                    FileName = model.UploadedFile.FileName
                };
            }

            return productService.UploadFileAttachmentFolderFileAsync(model.FileAttachmentFolderId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearFileAttachmentFolderFile([FromQuery] long movieId)
        {
            return productService.ClearFileAttachmentFolderFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsFileAttachmentFolderSourceWordFileValid([FromQuery] long fileAttachmentFolderId)
        {
            return movieService.IsFileAttachmentFolderWordOrPdfFileValidAsync(fileAttachmentFolderId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadFileAttachmentFolderSourceWordFile([FromQuery] long fileAttachmentFolderId)
        {
            var result = await movieService.GetFileAttachmentFolderWordOrdPdfFileAsync(fileAttachmentFolderId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableFileAttachmentFolder([FromBody] long id)
        {
            return movieService.EnableFileAttachmentFolderByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableFileAttachmentFolder([FromBody] long id)
        {
            return movieService.DisableFileAttachmentFolderByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByFileAttachmentFolder(long id)
        {
            return movieService.GetXYZZYXsByFileAttachmentFolderAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithFileAttachmentFolder(FileAttachmentFolderXYZZYXAssociationModel model)
        {
            return movieService.AssociateXYZZYXWithFileAttachmentFolderAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithFileAttachmentFolder(FileAttachmentFolderXYZZYXAssociationModel model)
        {
            return movieService.UnAssociateXYZZYXWithFileAttachmentFolderAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportFileAttachmentFolder([FromBody] ImportFileAttachmentFolderModel model)
        {
            return movieService.ImportFileAttachmentFolderAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportFileAttachmentFolder()
        {
            var lst = new List<ImportFileAttachmentFolderModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportFileAttachmentFolderModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleFileAttachmentFolderImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<FileAttachmentFolderExportModel>> FileAttachmentFolderReport([FromQuery] FileAttachmentFolderExportParameter parameter)
        {
            return reportService.ExportFileAttachmentFolderAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> FileAttachmentFolderAsReportCsv([FromQuery] FileAttachmentFolderExportParameter parameter)
        {
            var result = await reportService.ExportFileAttachmentFolderAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("FileAttachmentFolderAsCsv.csv"))
                {
                    FileDownloadName = "FileAttachmentFolderExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region FileAttachment
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Movie" })]
        public Task<ReturnListModel<FileAttachmentModel>> GetFileAttachments([FromQuery] FileAttachmentSearchCriteria search)
        {
            return movieService.GetFileAttachmentsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Movie" })]
        public Task<ReturnModel<FileAttachmentModel>> GetFileAttachment([FromQuery] long id)
        {
            return movieService.GetFileAttachmentByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Movie" })]
        public Task<ReturnModel<FileAttachmentModel>> CreateFileAttachment([FromForm] FileAttachmentCreateWithFileFormModel model)
        {
            FileStreamModel streamModel = null;
            if (model != null && model.UploadedFile != null)
            {
                streamModel = new FileStreamModel
                {
                    FileData = model.UploadedFile.OpenReadStream(),
                    ContentType = model.UploadedFile.ContentType,
                    FileName = model.UploadedFile.FileName
                };
            }

            return movieService.CreateFileAttachmentAsync(model, streamModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Movie" })]
        public Task<ReturnModel<FileAttachmentModel>> UpdateFileAttachment([FromBody] FileAttachmentUpdateModel fileAttachmentModel)
        {
            return movieService.UpdateFileAttachmentAsync(fileAttachmentModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Movie" })]
        public Task<ReturnModel<bool>> MarkFileAttachmentAsDeleted([FromQuery] long id)
        {
            return movieService.MarkFileAttachmentAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Movie" })]
        public Task<ReturnModel<bool>> MarkFileAttachmentAsNotDeleted([FromBody] long id)
        {
            return movieService.MarkFileAttachmentAsNotDeletedByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsFileAttachmentFileValid([FromQuery] long fileAttachmentId)
        {
            return movieService.IsFileAttachmentFileValidAsync(fileAttachmentId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadFileAttachmentFile([FromQuery] long fileAttachmentId)
        {
            var result = await movieService.GetFileAttachmentFileAsync(fileAttachmentId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsFileAttachmentSourceWordFileValid([FromQuery] long fileAttachmentId)
        {
            return movieService.IsFileAttachmentWordOrPdfFileValidAsync(fileAttachmentId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadFileAttachmentSourceWordFile([FromQuery] long fileAttachmentId)
        {
            var result = await movieService.GetFileAttachmentWordOrdPdfFileAsync(fileAttachmentId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Movie" })]
        public Task<ReturnModel<bool>> EnableFileAttachment([FromBody] long id)
        {
            return movieService.EnableFileAttachmentByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Movie" })]
        public Task<ReturnModel<bool>> DisableFileAttachment([FromBody] long id)
        {
            return movieService.DisableFileAttachmentByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Movie" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByFileAttachment(long id)
        {
            return movieService.GetXYZZYXsByFileAttachmentAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Movie" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithFileAttachment(FileAttachmentXYZZYXAssociationModel model)
        {
            return movieService.AssociateXYZZYXWithFileAttachmentAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Movie" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithFileAttachment(FileAttachmentXYZZYXAssociationModel model)
        {
            return movieService.UnAssociateXYZZYXWithFileAttachmentAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportFileAttachment([FromBody] ImportFileAttachmentModel model)
        {
            return movieService.ImportFileAttachmentAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportFileAttachment()
        {
            var lst = new List<ImportFileAttachmentModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportFileAttachmentModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleFileAttachmentImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<FileAttachmentExportModel>> FileAttachmentReport([FromQuery] FileAttachmentExportParameter parameter)
        {
            return reportService.ExportFileAttachmentAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> FileAttachmentAsReportCsv([FromQuery] FileAttachmentExportParameter parameter)
        {
            var result = await reportService.ExportFileAttachmentAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("FileAttachmentAsCsv.csv"))
                {
                    FileDownloadName = "FileAttachmentExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion
    }
}