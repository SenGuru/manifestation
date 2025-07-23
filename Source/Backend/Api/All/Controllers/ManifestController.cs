using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.EMMA;
using Framework.Infrastructure.Interfaces.Helpers;
using Framework.Infrastructure.Interfaces.Services;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search;
using Framework.Infrastructure.Utils;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Repository.DbAccess;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.DBView;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.ViewModels;
using Manifest.Common.Models.ViewModels;
using Manifest.Common.Models.ViewModels.ImportExportModels;
using Manifest.Service.App;
using Manifest.Service.Storage;
using Manifest.Web.Filters;
using Manifest.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Manifest.Api.All.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public partial class ManifestController : BaseController
    {
        private readonly ManifestService manifestService;
        private readonly DummyDataGenerationService dummyDataGenerationService;
        private readonly CompanyService companyService;
        public ManifestController(CompanyService companyService, ManifestService manifestService, DummyDataGenerationService dummyDataGenerationService, ILog log)
            : base(log)
        {
            this.companyService = companyService;
            this.manifestService = manifestService;
            this.dummyDataGenerationService = dummyDataGenerationService;
        }

        #region FileAttachmentFolder
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FileAttachmentFolderModel>> GetFileAttachmentFolders([FromQuery] FileAttachmentFolderSearchCriteria search)
        {
            return manifestService.GetFileAttachmentFoldersAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<FileAttachmentFolderModel>> GetFileAttachmentFolder([FromQuery] long id)
        {
            return manifestService.GetFileAttachmentFolderByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<FileAttachmentFolderModel>> CreateFileAttachmentFolder([FromBody] FileAttachmentFolderCreateModel fileAttachmentFolderModel)
        {
            return manifestService.CreateFileAttachmentFolderAsync(fileAttachmentFolderModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FileAttachmentFolderModel>> CreateMultipleFileAttachmentFolder([FromBody] MultipleFileAttachmentFolderCreateModel fileAttachmentFolderModels)
        {
            return manifestService.CreateMultipleFileAttachmentFolderAsync(fileAttachmentFolderModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<FileAttachmentFolderModel>> UpdateFileAttachmentFolder([FromBody] FileAttachmentFolderUpdateModel fileAttachmentFolderModel)
        {
            return manifestService.UpdateFileAttachmentFolderAsync(fileAttachmentFolderModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkFileAttachmentFolderAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkFileAttachmentFolderAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkFileAttachmentFolderAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkFileAttachmentFolderAsNotDeletedByIdAsync(id);
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
            return manifestService.GetFileAttachmentsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Movie" })]
        public Task<ReturnModel<FileAttachmentModel>> GetFileAttachment([FromQuery] long id)
        {
            return manifestService.GetFileAttachmentByIdForReadAsync(id);
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

            return manifestService.CreateFileAttachmentAsync(model, streamModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Movie" })]
        public Task<ReturnModel<FileAttachmentModel>> UpdateFileAttachment([FromBody] FileAttachmentUpdateModel fileAttachmentModel)
        {
            return manifestService.UpdateFileAttachmentAsync(fileAttachmentModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Movie" })]
        public Task<ReturnModel<bool>> MarkFileAttachmentAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkFileAttachmentAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Movie" })]
        public Task<ReturnModel<bool>> MarkFileAttachmentAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkFileAttachmentAsNotDeletedByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsFileAttachmentFileValid([FromQuery] long fileAttachmentId)
        {
            return manifestService.IsFileAttachmentFileValidAsync(fileAttachmentId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadFileAttachmentFile([FromQuery] long fileAttachmentId)
        {
            var result = await manifestService.GetFileAttachmentFileAsync(fileAttachmentId);
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

        #region VisionBoardSessionPicture
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardSessionPictureModel>> GetVisionBoardSessionPictures([FromQuery] VisionBoardSessionPictureSearchCriteria search)
        {
            return manifestService.GetVisionBoardSessionPicturesAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<VisionBoardSessionPictureModel>> GetVisionBoardSessionPicture([FromQuery] long id)
        {
            return manifestService.GetVisionBoardSessionPictureByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<VisionBoardSessionPictureModel>> CreateVisionBoardSessionPicture([FromBody] VisionBoardSessionPictureCreateModel visionBoardSessionPictureModel)
        {
            return manifestService.CreateVisionBoardSessionPictureAsync(visionBoardSessionPictureModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardSessionPictureModel>> CreateMultipleVisionBoardSessionPicture([FromBody] MultipleVisionBoardSessionPictureCreateModel visionBoardSessionPictureModels)
        {
            return manifestService.CreateMultipleVisionBoardSessionPictureAsync(visionBoardSessionPictureModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<VisionBoardSessionPictureModel>> UpdateVisionBoardSessionPicture([FromBody] VisionBoardSessionPictureUpdateModel visionBoardSessionPictureModel)
        {
            return manifestService.UpdateVisionBoardSessionPictureAsync(visionBoardSessionPictureModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkVisionBoardSessionPictureAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkVisionBoardSessionPictureAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkVisionBoardSessionPictureAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkVisionBoardSessionPictureAsNotDeletedByIdAsync(id);
        }



        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardSessionPictureModel>> GetVisionBoardSessionPictureByVisionBoardSessionId([FromQuery] long visionBoardSessionId)
        {
            return manifestService.GetVisionBoardSessionPictureByVisionBoardSessionIdAsync(visionBoardSessionId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardSessionPictureModel>> GetVisionBoardSessionPictureByVisionBoardSessionIdSearch(VisionBoardSessionPictureByVisionBoardSessionIdSearchCriteria search)
        {
            return manifestService.GetVisionBoardSessionPictureByVisionBoardSessionIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardSessionModel>> GetVisionBoardSessionByVisionBoardSessionPictureId(long visionBoardSessionPictureId)
        {
            return manifestService.GetVisionBoardSessionByVisionBoardSessionPictureIdAsync(visionBoardSessionPictureId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardSessionModel>> GetVisionBoardSessionByVisionBoardSessionPictureIdSearch(VisionBoardSessionByVisionBoardSessionPictureIdSearchCriteria search)
        {
            return manifestService.GetVisionBoardSessionByVisionBoardSessionPictureIdAsync(search);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardSessionPictureModel>> GetVisionBoardSessionPictureByVisionBoardPictureId([FromQuery] long visionBoardPictureId)
        {
            return manifestService.GetVisionBoardSessionPictureByVisionBoardPictureIdAsync(visionBoardPictureId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardSessionPictureModel>> GetVisionBoardSessionPictureByVisionBoardPictureIdSearch(VisionBoardSessionPictureByVisionBoardPictureIdSearchCriteria search)
        {
            return manifestService.GetVisionBoardSessionPictureByVisionBoardPictureIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardPictureModel>> GetVisionBoardPictureByVisionBoardSessionPictureId(long visionBoardSessionPictureId)
        {
            return manifestService.GetVisionBoardPictureByVisionBoardSessionPictureIdAsync(visionBoardSessionPictureId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardPictureModel>> GetVisionBoardPictureByVisionBoardSessionPictureIdSearch(VisionBoardPictureByVisionBoardSessionPictureIdSearchCriteria search)
        {
            return manifestService.GetVisionBoardPictureByVisionBoardSessionPictureIdAsync(search);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardSessionPictureModel>> GetVisionBoardSessionPictureByUserId([FromQuery] long userId)
        {
            return manifestService.GetVisionBoardSessionPictureByUserIdAsync(userId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardSessionPictureModel>> GetVisionBoardSessionPictureByUserIdSearch(VisionBoardSessionPictureByUserIdSearchCriteria search)
        {
            return manifestService.GetVisionBoardSessionPictureByUserIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByVisionBoardSessionPictureId(long visionBoardSessionPictureId)
        {
            return manifestService.GetUserByVisionBoardSessionPictureIdAsync(visionBoardSessionPictureId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByVisionBoardSessionPictureIdSearch(UserByVisionBoardSessionPictureIdSearchCriteria search)
        {
            return manifestService.GetUserByVisionBoardSessionPictureIdAsync(search);
        }



        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsVisionBoardSessionPictureFileValid([FromQuery] long visionBoardSessionPictureId)
        {
            return manifestService.IsVisionBoardSessionPictureFileValidAsync(visionBoardSessionPictureId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadVisionBoardSessionPictureFile([FromQuery] long visionBoardSessionPictureId)
        {
            var result = await manifestService.GetVisionBoardSessionPictureFileAsync(visionBoardSessionPictureId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadVisionBoardSessionPictureFile([FromForm] VisionBoardSessionPictureFileWithFileFormModel model)
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

            return productService.UploadVisionBoardSessionPictureFileAsync(model.VisionBoardSessionPictureId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearVisionBoardSessionPictureFile([FromQuery] long movieId)
        {
            return productService.ClearVisionBoardSessionPictureFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsVisionBoardSessionPictureSourceWordFileValid([FromQuery] long visionBoardSessionPictureId)
        {
            return manifestService.IsVisionBoardSessionPictureWordOrPdfFileValidAsync(visionBoardSessionPictureId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadVisionBoardSessionPictureSourceWordFile([FromQuery] long visionBoardSessionPictureId)
        {
            var result = await manifestService.GetVisionBoardSessionPictureWordOrdPdfFileAsync(visionBoardSessionPictureId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableVisionBoardSessionPicture([FromBody] long id)
        {
            return manifestService.EnableVisionBoardSessionPictureByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableVisionBoardSessionPicture([FromBody] long id)
        {
            return manifestService.DisableVisionBoardSessionPictureByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByVisionBoardSessionPicture(long id)
        {
            return manifestService.GetXYZZYXsByVisionBoardSessionPictureAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithVisionBoardSessionPicture(VisionBoardSessionPictureXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithVisionBoardSessionPictureAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithVisionBoardSessionPicture(VisionBoardSessionPictureXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithVisionBoardSessionPictureAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportVisionBoardSessionPicture([FromBody] ImportVisionBoardSessionPictureModel model)
        {
            return manifestService.ImportVisionBoardSessionPictureAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportVisionBoardSessionPicture()
        {
            var lst = new List<ImportVisionBoardSessionPictureModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportVisionBoardSessionPictureModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleVisionBoardSessionPictureImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<VisionBoardSessionPictureExportModel>> VisionBoardSessionPictureReport([FromQuery] VisionBoardSessionPictureExportParameter parameter)
        {
            return reportService.ExportVisionBoardSessionPictureAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> VisionBoardSessionPictureAsReportCsv([FromQuery] VisionBoardSessionPictureExportParameter parameter)
        {
            var result = await reportService.ExportVisionBoardSessionPictureAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("VisionBoardSessionPictureAsCsv.csv"))
                {
                    FileDownloadName = "VisionBoardSessionPictureExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region Category
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<CategoryModel>> GetCategorys([FromQuery] CategorySearchCriteria search)
        {
            return manifestService.GetCategorysAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<CategoryModel>> GetCategory([FromQuery] long id)
        {
            return manifestService.GetCategoryByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<CategoryModel>> CreateCategory([FromBody] CategoryCreateModel categoryModel)
        {
            return manifestService.CreateCategoryAsync(categoryModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<CategoryModel>> CreateMultipleCategory([FromBody] MultipleCategoryCreateModel categoryModels)
        {
            return manifestService.CreateMultipleCategoryAsync(categoryModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<CategoryModel>> UpdateCategory([FromBody] CategoryUpdateModel categoryModel)
        {
            return manifestService.UpdateCategoryAsync(categoryModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkCategoryAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkCategoryAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkCategoryAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkCategoryAsNotDeletedByIdAsync(id);
        }



        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<CategoryModel>> GetCategoryByDefaultCategoryId([FromQuery] long defaultCategoryId)
        {
            return manifestService.GetCategoryByDefaultCategoryIdAsync(defaultCategoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<CategoryModel>> GetCategoryByDefaultCategoryIdSearch(CategoryByDefaultCategoryIdSearchCriteria search)
        {
            return manifestService.GetCategoryByDefaultCategoryIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultCategoryModel>> GetDefaultCategoryByCategoryId(long categoryId)
        {
            return manifestService.GetDefaultCategoryByCategoryIdAsync(categoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultCategoryModel>> GetDefaultCategoryByCategoryIdSearch(DefaultCategoryByCategoryIdSearchCriteria search)
        {
            return manifestService.GetDefaultCategoryByCategoryIdAsync(search);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<CategoryModel>> GetCategoryByUserId([FromQuery] long userId)
        {
            return manifestService.GetCategoryByUserIdAsync(userId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<CategoryModel>> GetCategoryForCurrentUser()
        {
            return manifestService.GetCategoryForCurrentUserAsync();
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<CategoryModel>> GetCategoryByUserIdSearch(CategoryByUserIdSearchCriteria search)
        {
            return manifestService.GetCategoryByUserIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByCategoryId(long categoryId)
        {
            return manifestService.GetUserByCategoryIdAsync(categoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByCategoryIdSearch(UserByCategoryIdSearchCriteria search)
        {
            return manifestService.GetUserByCategoryIdAsync(search);
        }



        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsCategoryFileValid([FromQuery] long categoryId)
        {
            return manifestService.IsCategoryFileValidAsync(categoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadCategoryFile([FromQuery] long categoryId)
        {
            var result = await manifestService.GetCategoryFileAsync(categoryId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadCategoryFile([FromForm] CategoryFileWithFileFormModel model)
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

            return productService.UploadCategoryFileAsync(model.CategoryId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearCategoryFile([FromQuery] long movieId)
        {
            return productService.ClearCategoryFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsCategorySourceWordFileValid([FromQuery] long categoryId)
        {
            return manifestService.IsCategoryWordOrPdfFileValidAsync(categoryId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadCategorySourceWordFile([FromQuery] long categoryId)
        {
            var result = await manifestService.GetCategoryWordOrdPdfFileAsync(categoryId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableCategory([FromBody] long id)
        {
            return manifestService.EnableCategoryByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableCategory([FromBody] long id)
        {
            return manifestService.DisableCategoryByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByCategory(long id)
        {
            return manifestService.GetXYZZYXsByCategoryAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithCategory(CategoryXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithCategoryAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithCategory(CategoryXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithCategoryAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportCategory([FromBody] ImportCategoryModel model)
        {
            return manifestService.ImportCategoryAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportCategory()
        {
            var lst = new List<ImportCategoryModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportCategoryModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleCategoryImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<CategoryExportModel>> CategoryReport([FromQuery] CategoryExportParameter parameter)
        {
            return reportService.ExportCategoryAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryAsReportCsv([FromQuery] CategoryExportParameter parameter)
        {
            var result = await reportService.ExportCategoryAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("CategoryAsCsv.csv"))
                {
                    FileDownloadName = "CategoryExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region DailyAffirmationSession
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationSessionModel>> GetDailyAffirmationSessions([FromQuery] DailyAffirmationSessionSearchCriteria search)
        {
            return manifestService.GetDailyAffirmationSessionsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DailyAffirmationSessionModel>> GetDailyAffirmationSession([FromQuery] long id)
        {
            return manifestService.GetDailyAffirmationSessionByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DailyAffirmationSessionModel>> CreateDailyAffirmationSession([FromBody] DailyAffirmationSessionCreateModel dailyAffirmationSessionModel)
        {
            return manifestService.CreateDailyAffirmationSessionAsync(dailyAffirmationSessionModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationSessionModel>> CreateMultipleDailyAffirmationSession([FromBody] MultipleDailyAffirmationSessionCreateModel dailyAffirmationSessionModels)
        {
            return manifestService.CreateMultipleDailyAffirmationSessionAsync(dailyAffirmationSessionModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DailyAffirmationSessionModel>> UpdateDailyAffirmationSession([FromBody] DailyAffirmationSessionUpdateModel dailyAffirmationSessionModel)
        {
            return manifestService.UpdateDailyAffirmationSessionAsync(dailyAffirmationSessionModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDailyAffirmationSessionAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkDailyAffirmationSessionAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDailyAffirmationSessionAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkDailyAffirmationSessionAsNotDeletedByIdAsync(id);
        }



        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationSessionModel>> GetDailyAffirmationSessionByUserId([FromQuery] long userId)
        {
            return manifestService.GetDailyAffirmationSessionByUserIdAsync(userId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationSessionModel>> GetDailyAffirmationSessionByUserIdSearch(DailyAffirmationSessionByUserIdSearchCriteria search)
        {
            return manifestService.GetDailyAffirmationSessionByUserIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByDailyAffirmationSessionId(long dailyAffirmationSessionId)
        {
            return manifestService.GetUserByDailyAffirmationSessionIdAsync(dailyAffirmationSessionId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByDailyAffirmationSessionIdSearch(UserByDailyAffirmationSessionIdSearchCriteria search)
        {
            return manifestService.GetUserByDailyAffirmationSessionIdAsync(search);
        }



        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDailyAffirmationSessionFileValid([FromQuery] long dailyAffirmationSessionId)
        {
            return manifestService.IsDailyAffirmationSessionFileValidAsync(dailyAffirmationSessionId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDailyAffirmationSessionFile([FromQuery] long dailyAffirmationSessionId)
        {
            var result = await manifestService.GetDailyAffirmationSessionFileAsync(dailyAffirmationSessionId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadDailyAffirmationSessionFile([FromForm] DailyAffirmationSessionFileWithFileFormModel model)
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

            return productService.UploadDailyAffirmationSessionFileAsync(model.DailyAffirmationSessionId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearDailyAffirmationSessionFile([FromQuery] long movieId)
        {
            return productService.ClearDailyAffirmationSessionFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDailyAffirmationSessionSourceWordFileValid([FromQuery] long dailyAffirmationSessionId)
        {
            return manifestService.IsDailyAffirmationSessionWordOrPdfFileValidAsync(dailyAffirmationSessionId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDailyAffirmationSessionSourceWordFile([FromQuery] long dailyAffirmationSessionId)
        {
            var result = await manifestService.GetDailyAffirmationSessionWordOrdPdfFileAsync(dailyAffirmationSessionId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableDailyAffirmationSession([FromBody] long id)
        {
            return manifestService.EnableDailyAffirmationSessionByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableDailyAffirmationSession([FromBody] long id)
        {
            return manifestService.DisableDailyAffirmationSessionByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByDailyAffirmationSession(long id)
        {
            return manifestService.GetXYZZYXsByDailyAffirmationSessionAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithDailyAffirmationSession(DailyAffirmationSessionXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithDailyAffirmationSessionAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithDailyAffirmationSession(DailyAffirmationSessionXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithDailyAffirmationSessionAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportDailyAffirmationSession([FromBody] ImportDailyAffirmationSessionModel model)
        {
            return manifestService.ImportDailyAffirmationSessionAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportDailyAffirmationSession()
        {
            var lst = new List<ImportDailyAffirmationSessionModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportDailyAffirmationSessionModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleDailyAffirmationSessionImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<DailyAffirmationSessionExportModel>> DailyAffirmationSessionReport([FromQuery] DailyAffirmationSessionExportParameter parameter)
        {
            return reportService.ExportDailyAffirmationSessionAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> DailyAffirmationSessionAsReportCsv([FromQuery] DailyAffirmationSessionExportParameter parameter)
        {
            var result = await reportService.ExportDailyAffirmationSessionAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("DailyAffirmationSessionAsCsv.csv"))
                {
                    FileDownloadName = "DailyAffirmationSessionExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region DailyAffirmationSessionCategory
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationSessionCategoryModel>> GetDailyAffirmationSessionCategorys([FromQuery] DailyAffirmationSessionCategorySearchCriteria search)
        {
            return manifestService.GetDailyAffirmationSessionCategorysAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DailyAffirmationSessionCategoryModel>> GetDailyAffirmationSessionCategory([FromQuery] long id)
        {
            return manifestService.GetDailyAffirmationSessionCategoryByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DailyAffirmationSessionCategoryModel>> CreateDailyAffirmationSessionCategory([FromBody] DailyAffirmationSessionCategoryCreateModel dailyAffirmationSessionCategoryModel)
        {
            return manifestService.CreateDailyAffirmationSessionCategoryAsync(dailyAffirmationSessionCategoryModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationSessionCategoryModel>> CreateMultipleDailyAffirmationSessionCategory([FromBody] MultipleDailyAffirmationSessionCategoryCreateModel dailyAffirmationSessionCategoryModels)
        {
            return manifestService.CreateMultipleDailyAffirmationSessionCategoryAsync(dailyAffirmationSessionCategoryModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DailyAffirmationSessionCategoryModel>> UpdateDailyAffirmationSessionCategory([FromBody] DailyAffirmationSessionCategoryUpdateModel dailyAffirmationSessionCategoryModel)
        {
            return manifestService.UpdateDailyAffirmationSessionCategoryAsync(dailyAffirmationSessionCategoryModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDailyAffirmationSessionCategoryAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkDailyAffirmationSessionCategoryAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDailyAffirmationSessionCategoryAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkDailyAffirmationSessionCategoryAsNotDeletedByIdAsync(id);
        }



        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationSessionCategoryModel>> GetDailyAffirmationSessionCategoryByCategoryId([FromQuery] long categoryId)
        {
            return manifestService.GetDailyAffirmationSessionCategoryByCategoryIdAsync(categoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationSessionCategoryModel>> GetDailyAffirmationSessionCategoryByCategoryIdSearch(DailyAffirmationSessionCategoryByCategoryIdSearchCriteria search)
        {
            return manifestService.GetDailyAffirmationSessionCategoryByCategoryIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<CategoryModel>> GetCategoryByDailyAffirmationSessionCategoryId(long dailyAffirmationSessionCategoryId)
        {
            return manifestService.GetCategoryByDailyAffirmationSessionCategoryIdAsync(dailyAffirmationSessionCategoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<CategoryModel>> GetCategoryByDailyAffirmationSessionCategoryIdSearch(CategoryByDailyAffirmationSessionCategoryIdSearchCriteria search)
        {
            return manifestService.GetCategoryByDailyAffirmationSessionCategoryIdAsync(search);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationSessionCategoryModel>> GetDailyAffirmationSessionCategoryByDailyAffirmationSessionId([FromQuery] long dailyAffirmationSessionId)
        {
            return manifestService.GetDailyAffirmationSessionCategoryByDailyAffirmationSessionIdAsync(dailyAffirmationSessionId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationSessionCategoryModel>> GetDailyAffirmationSessionCategoryByDailyAffirmationSessionIdSearch(DailyAffirmationSessionCategoryByDailyAffirmationSessionIdSearchCriteria search)
        {
            return manifestService.GetDailyAffirmationSessionCategoryByDailyAffirmationSessionIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationSessionModel>> GetDailyAffirmationSessionByDailyAffirmationSessionCategoryId(long dailyAffirmationSessionCategoryId)
        {
            return manifestService.GetDailyAffirmationSessionByDailyAffirmationSessionCategoryIdAsync(dailyAffirmationSessionCategoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationSessionModel>> GetDailyAffirmationSessionByDailyAffirmationSessionCategoryIdSearch(DailyAffirmationSessionByDailyAffirmationSessionCategoryIdSearchCriteria search)
        {
            return manifestService.GetDailyAffirmationSessionByDailyAffirmationSessionCategoryIdAsync(search);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationSessionCategoryModel>> GetDailyAffirmationSessionCategoryByUserId([FromQuery] long userId)
        {
            return manifestService.GetDailyAffirmationSessionCategoryByUserIdAsync(userId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationSessionCategoryModel>> GetDailyAffirmationSessionCategoryByUserIdSearch(DailyAffirmationSessionCategoryByUserIdSearchCriteria search)
        {
            return manifestService.GetDailyAffirmationSessionCategoryByUserIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByDailyAffirmationSessionCategoryId(long dailyAffirmationSessionCategoryId)
        {
            return manifestService.GetUserByDailyAffirmationSessionCategoryIdAsync(dailyAffirmationSessionCategoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByDailyAffirmationSessionCategoryIdSearch(UserByDailyAffirmationSessionCategoryIdSearchCriteria search)
        {
            return manifestService.GetUserByDailyAffirmationSessionCategoryIdAsync(search);
        }



        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDailyAffirmationSessionCategoryFileValid([FromQuery] long dailyAffirmationSessionCategoryId)
        {
            return manifestService.IsDailyAffirmationSessionCategoryFileValidAsync(dailyAffirmationSessionCategoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDailyAffirmationSessionCategoryFile([FromQuery] long dailyAffirmationSessionCategoryId)
        {
            var result = await manifestService.GetDailyAffirmationSessionCategoryFileAsync(dailyAffirmationSessionCategoryId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadDailyAffirmationSessionCategoryFile([FromForm] DailyAffirmationSessionCategoryFileWithFileFormModel model)
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

            return productService.UploadDailyAffirmationSessionCategoryFileAsync(model.DailyAffirmationSessionCategoryId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearDailyAffirmationSessionCategoryFile([FromQuery] long movieId)
        {
            return productService.ClearDailyAffirmationSessionCategoryFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDailyAffirmationSessionCategorySourceWordFileValid([FromQuery] long dailyAffirmationSessionCategoryId)
        {
            return manifestService.IsDailyAffirmationSessionCategoryWordOrPdfFileValidAsync(dailyAffirmationSessionCategoryId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDailyAffirmationSessionCategorySourceWordFile([FromQuery] long dailyAffirmationSessionCategoryId)
        {
            var result = await manifestService.GetDailyAffirmationSessionCategoryWordOrdPdfFileAsync(dailyAffirmationSessionCategoryId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableDailyAffirmationSessionCategory([FromBody] long id)
        {
            return manifestService.EnableDailyAffirmationSessionCategoryByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableDailyAffirmationSessionCategory([FromBody] long id)
        {
            return manifestService.DisableDailyAffirmationSessionCategoryByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByDailyAffirmationSessionCategory(long id)
        {
            return manifestService.GetXYZZYXsByDailyAffirmationSessionCategoryAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithDailyAffirmationSessionCategory(DailyAffirmationSessionCategoryXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithDailyAffirmationSessionCategoryAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithDailyAffirmationSessionCategory(DailyAffirmationSessionCategoryXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithDailyAffirmationSessionCategoryAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportDailyAffirmationSessionCategory([FromBody] ImportDailyAffirmationSessionCategoryModel model)
        {
            return manifestService.ImportDailyAffirmationSessionCategoryAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportDailyAffirmationSessionCategory()
        {
            var lst = new List<ImportDailyAffirmationSessionCategoryModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportDailyAffirmationSessionCategoryModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleDailyAffirmationSessionCategoryImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<DailyAffirmationSessionCategoryExportModel>> DailyAffirmationSessionCategoryReport([FromQuery] DailyAffirmationSessionCategoryExportParameter parameter)
        {
            return reportService.ExportDailyAffirmationSessionCategoryAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> DailyAffirmationSessionCategoryAsReportCsv([FromQuery] DailyAffirmationSessionCategoryExportParameter parameter)
        {
            var result = await reportService.ExportDailyAffirmationSessionCategoryAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("DailyAffirmationSessionCategoryAsCsv.csv"))
                {
                    FileDownloadName = "DailyAffirmationSessionCategoryExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region DailyAffirmationText
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationTextModel>> GetDailyAffirmationTexts([FromQuery] DailyAffirmationTextSearchCriteria search)
        {
            return manifestService.GetDailyAffirmationTextsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DailyAffirmationTextModel>> GetDailyAffirmationText([FromQuery] long id)
        {
            return manifestService.GetDailyAffirmationTextByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DailyAffirmationTextModel>> CreateDailyAffirmationText([FromBody] DailyAffirmationTextCreateModel dailyAffirmationTextModel)
        {
            return manifestService.CreateDailyAffirmationTextAsync(dailyAffirmationTextModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationTextModel>> CreateMultipleDailyAffirmationText([FromBody] MultipleDailyAffirmationTextCreateModel dailyAffirmationTextModels)
        {
            return manifestService.CreateMultipleDailyAffirmationTextAsync(dailyAffirmationTextModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DailyAffirmationTextModel>> UpdateDailyAffirmationText([FromBody] DailyAffirmationTextUpdateModel dailyAffirmationTextModel)
        {
            return manifestService.UpdateDailyAffirmationTextAsync(dailyAffirmationTextModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDailyAffirmationTextAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkDailyAffirmationTextAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDailyAffirmationTextAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkDailyAffirmationTextAsNotDeletedByIdAsync(id);
        }



        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationTextModel>> GetDailyAffirmationTextByCategoryId([FromQuery] long categoryId)
        {
            return manifestService.GetDailyAffirmationTextByCategoryIdAsync(categoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationTextModel>> GetDailyAffirmationTextByCategoryIdSearch(DailyAffirmationTextByCategoryIdSearchCriteria search)
        {
            return manifestService.GetDailyAffirmationTextByCategoryIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<CategoryModel>> GetCategoryByDailyAffirmationTextId(long dailyAffirmationTextId)
        {
            return manifestService.GetCategoryByDailyAffirmationTextIdAsync(dailyAffirmationTextId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<CategoryModel>> GetCategoryByDailyAffirmationTextIdSearch(CategoryByDailyAffirmationTextIdSearchCriteria search)
        {
            return manifestService.GetCategoryByDailyAffirmationTextIdAsync(search);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationTextModel>> GetDailyAffirmationTextForCurrentUser()
        {
            return manifestService.GetDailyAffirmationTextForCurrentUserAsync();
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationTextModel>> GetDailyAffirmationTextByUserId([FromQuery] long userId)
        {
            return manifestService.GetDailyAffirmationTextByUserIdAsync(userId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationTextModel>> GetDailyAffirmationTextByUserIdSearch(DailyAffirmationTextByUserIdSearchCriteria search)
        {
            return manifestService.GetDailyAffirmationTextByUserIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByDailyAffirmationTextId(long dailyAffirmationTextId)
        {
            return manifestService.GetUserByDailyAffirmationTextIdAsync(dailyAffirmationTextId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByDailyAffirmationTextIdSearch(UserByDailyAffirmationTextIdSearchCriteria search)
        {
            return manifestService.GetUserByDailyAffirmationTextIdAsync(search);
        }



        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDailyAffirmationTextFileValid([FromQuery] long dailyAffirmationTextId)
        {
            return manifestService.IsDailyAffirmationTextFileValidAsync(dailyAffirmationTextId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDailyAffirmationTextFile([FromQuery] long dailyAffirmationTextId)
        {
            var result = await manifestService.GetDailyAffirmationTextFileAsync(dailyAffirmationTextId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadDailyAffirmationTextFile([FromForm] DailyAffirmationTextFileWithFileFormModel model)
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

            return productService.UploadDailyAffirmationTextFileAsync(model.DailyAffirmationTextId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearDailyAffirmationTextFile([FromQuery] long movieId)
        {
            return productService.ClearDailyAffirmationTextFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDailyAffirmationTextSourceWordFileValid([FromQuery] long dailyAffirmationTextId)
        {
            return manifestService.IsDailyAffirmationTextWordOrPdfFileValidAsync(dailyAffirmationTextId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDailyAffirmationTextSourceWordFile([FromQuery] long dailyAffirmationTextId)
        {
            var result = await manifestService.GetDailyAffirmationTextWordOrdPdfFileAsync(dailyAffirmationTextId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableDailyAffirmationText([FromBody] long id)
        {
            return manifestService.EnableDailyAffirmationTextByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableDailyAffirmationText([FromBody] long id)
        {
            return manifestService.DisableDailyAffirmationTextByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByDailyAffirmationText(long id)
        {
            return manifestService.GetXYZZYXsByDailyAffirmationTextAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithDailyAffirmationText(DailyAffirmationTextXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithDailyAffirmationTextAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithDailyAffirmationText(DailyAffirmationTextXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithDailyAffirmationTextAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportDailyAffirmationText([FromBody] ImportDailyAffirmationTextModel model)
        {
            return manifestService.ImportDailyAffirmationTextAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportDailyAffirmationText()
        {
            var lst = new List<ImportDailyAffirmationTextModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportDailyAffirmationTextModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleDailyAffirmationTextImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<DailyAffirmationTextExportModel>> DailyAffirmationTextReport([FromQuery] DailyAffirmationTextExportParameter parameter)
        {
            return reportService.ExportDailyAffirmationTextAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> DailyAffirmationTextAsReportCsv([FromQuery] DailyAffirmationTextExportParameter parameter)
        {
            var result = await reportService.ExportDailyAffirmationTextAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("DailyAffirmationTextAsCsv.csv"))
                {
                    FileDownloadName = "DailyAffirmationTextExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region DefaultCategory
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultCategoryModel>> GetDefaultCategorys([FromQuery] DefaultCategorySearchCriteria search)
        {
            return manifestService.GetDefaultCategorysAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultCategoryModel>> GetDefaultCategory([FromQuery] long id)
        {
            return manifestService.GetDefaultCategoryByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultCategoryModel>> CreateDefaultCategory([FromBody] DefaultCategoryCreateModel defaultCategoryModel)
        {
            return manifestService.CreateDefaultCategoryAsync(defaultCategoryModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultCategoryModel>> CreateMultipleDefaultCategory([FromBody] MultipleDefaultCategoryCreateModel defaultCategoryModels)
        {
            return manifestService.CreateMultipleDefaultCategoryAsync(defaultCategoryModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultCategoryModel>> UpdateDefaultCategory([FromBody] DefaultCategoryUpdateModel defaultCategoryModel)
        {
            return manifestService.UpdateDefaultCategoryAsync(defaultCategoryModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDefaultCategoryAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkDefaultCategoryAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDefaultCategoryAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkDefaultCategoryAsNotDeletedByIdAsync(id);
        }




        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDefaultCategoryFileValid([FromQuery] long defaultCategoryId)
        {
            return manifestService.IsDefaultCategoryFileValidAsync(defaultCategoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDefaultCategoryFile([FromQuery] long defaultCategoryId)
        {
            var result = await manifestService.GetDefaultCategoryFileAsync(defaultCategoryId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadDefaultCategoryFile([FromForm] DefaultCategoryFileWithFileFormModel model)
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

            return productService.UploadDefaultCategoryFileAsync(model.DefaultCategoryId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearDefaultCategoryFile([FromQuery] long movieId)
        {
            return productService.ClearDefaultCategoryFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDefaultCategorySourceWordFileValid([FromQuery] long defaultCategoryId)
        {
            return manifestService.IsDefaultCategoryWordOrPdfFileValidAsync(defaultCategoryId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDefaultCategorySourceWordFile([FromQuery] long defaultCategoryId)
        {
            var result = await manifestService.GetDefaultCategoryWordOrdPdfFileAsync(defaultCategoryId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableDefaultCategory([FromBody] long id)
        {
            return manifestService.EnableDefaultCategoryByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableDefaultCategory([FromBody] long id)
        {
            return manifestService.DisableDefaultCategoryByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByDefaultCategory(long id)
        {
            return manifestService.GetXYZZYXsByDefaultCategoryAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithDefaultCategory(DefaultCategoryXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithDefaultCategoryAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithDefaultCategory(DefaultCategoryXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithDefaultCategoryAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportDefaultCategory([FromBody] ImportDefaultCategoryModel model)
        {
            return manifestService.ImportDefaultCategoryAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportDefaultCategory()
        {
            var lst = new List<ImportDefaultCategoryModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportDefaultCategoryModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleDefaultCategoryImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<DefaultCategoryExportModel>> DefaultCategoryReport([FromQuery] DefaultCategoryExportParameter parameter)
        {
            return reportService.ExportDefaultCategoryAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> DefaultCategoryAsReportCsv([FromQuery] DefaultCategoryExportParameter parameter)
        {
            var result = await reportService.ExportDefaultCategoryAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("DefaultCategoryAsCsv.csv"))
                {
                    FileDownloadName = "DefaultCategoryExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region DefaultDailyAffirmationText
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultDailyAffirmationTextModel>> GetDefaultDailyAffirmationTexts([FromQuery] DefaultDailyAffirmationTextSearchCriteria search)
        {
            return manifestService.GetDefaultDailyAffirmationTextsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultDailyAffirmationTextModel>> GetDefaultDailyAffirmationText([FromQuery] long id)
        {
            return manifestService.GetDefaultDailyAffirmationTextByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultDailyAffirmationTextModel>> CreateDefaultDailyAffirmationText([FromBody] DefaultDailyAffirmationTextCreateModel defaultDailyAffirmationTextModel)
        {
            return manifestService.CreateDefaultDailyAffirmationTextAsync(defaultDailyAffirmationTextModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultDailyAffirmationTextModel>> CreateMultipleDefaultDailyAffirmationText([FromBody] MultipleDefaultDailyAffirmationTextCreateModel defaultDailyAffirmationTextModels)
        {
            return manifestService.CreateMultipleDefaultDailyAffirmationTextAsync(defaultDailyAffirmationTextModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultDailyAffirmationTextModel>> UpdateDefaultDailyAffirmationText([FromBody] DefaultDailyAffirmationTextUpdateModel defaultDailyAffirmationTextModel)
        {
            return manifestService.UpdateDefaultDailyAffirmationTextAsync(defaultDailyAffirmationTextModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDefaultDailyAffirmationTextAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkDefaultDailyAffirmationTextAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDefaultDailyAffirmationTextAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkDefaultDailyAffirmationTextAsNotDeletedByIdAsync(id);
        }



        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultDailyAffirmationTextModel>> GetDefaultDailyAffirmationTextByDefaultCategoryId([FromQuery] long defaultCategoryId)
        {
            return manifestService.GetDefaultDailyAffirmationTextByDefaultCategoryIdAsync(defaultCategoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultDailyAffirmationTextModel>> GetDefaultDailyAffirmationTextByDefaultCategoryIdSearch(DefaultDailyAffirmationTextByDefaultCategoryIdSearchCriteria search)
        {
            return manifestService.GetDefaultDailyAffirmationTextByDefaultCategoryIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultCategoryModel>> GetDefaultCategoryByDefaultDailyAffirmationTextId(long defaultDailyAffirmationTextId)
        {
            return manifestService.GetDefaultCategoryByDefaultDailyAffirmationTextIdAsync(defaultDailyAffirmationTextId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultCategoryModel>> GetDefaultCategoryByDefaultDailyAffirmationTextIdSearch(DefaultCategoryByDefaultDailyAffirmationTextIdSearchCriteria search)
        {
            return manifestService.GetDefaultCategoryByDefaultDailyAffirmationTextIdAsync(search);
        }



        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDefaultDailyAffirmationTextFileValid([FromQuery] long defaultDailyAffirmationTextId)
        {
            return manifestService.IsDefaultDailyAffirmationTextFileValidAsync(defaultDailyAffirmationTextId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDefaultDailyAffirmationTextFile([FromQuery] long defaultDailyAffirmationTextId)
        {
            var result = await manifestService.GetDefaultDailyAffirmationTextFileAsync(defaultDailyAffirmationTextId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadDefaultDailyAffirmationTextFile([FromForm] DefaultDailyAffirmationTextFileWithFileFormModel model)
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

            return productService.UploadDefaultDailyAffirmationTextFileAsync(model.DefaultDailyAffirmationTextId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearDefaultDailyAffirmationTextFile([FromQuery] long movieId)
        {
            return productService.ClearDefaultDailyAffirmationTextFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDefaultDailyAffirmationTextSourceWordFileValid([FromQuery] long defaultDailyAffirmationTextId)
        {
            return manifestService.IsDefaultDailyAffirmationTextWordOrPdfFileValidAsync(defaultDailyAffirmationTextId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDefaultDailyAffirmationTextSourceWordFile([FromQuery] long defaultDailyAffirmationTextId)
        {
            var result = await manifestService.GetDefaultDailyAffirmationTextWordOrdPdfFileAsync(defaultDailyAffirmationTextId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableDefaultDailyAffirmationText([FromBody] long id)
        {
            return manifestService.EnableDefaultDailyAffirmationTextByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableDefaultDailyAffirmationText([FromBody] long id)
        {
            return manifestService.DisableDefaultDailyAffirmationTextByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByDefaultDailyAffirmationText(long id)
        {
            return manifestService.GetXYZZYXsByDefaultDailyAffirmationTextAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithDefaultDailyAffirmationText(DefaultDailyAffirmationTextXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithDefaultDailyAffirmationTextAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithDefaultDailyAffirmationText(DefaultDailyAffirmationTextXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithDefaultDailyAffirmationTextAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportDefaultDailyAffirmationText([FromBody] ImportDefaultDailyAffirmationTextModel model)
        {
            return manifestService.ImportDefaultDailyAffirmationTextAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportDefaultDailyAffirmationText()
        {
            var lst = new List<ImportDefaultDailyAffirmationTextModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportDefaultDailyAffirmationTextModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleDefaultDailyAffirmationTextImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<DefaultDailyAffirmationTextExportModel>> DefaultDailyAffirmationTextReport([FromQuery] DefaultDailyAffirmationTextExportParameter parameter)
        {
            return reportService.ExportDefaultDailyAffirmationTextAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> DefaultDailyAffirmationTextAsReportCsv([FromQuery] DefaultDailyAffirmationTextExportParameter parameter)
        {
            var result = await reportService.ExportDefaultDailyAffirmationTextAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("DefaultDailyAffirmationTextAsCsv.csv"))
                {
                    FileDownloadName = "DefaultDailyAffirmationTextExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region DefaultGoalTracker
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultGoalTrackerModel>> GetDefaultGoalTrackers([FromQuery] DefaultGoalTrackerSearchCriteria search)
        {
            return manifestService.GetDefaultGoalTrackersAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultGoalTrackerModel>> GetDefaultGoalTracker([FromQuery] long id)
        {
            return manifestService.GetDefaultGoalTrackerByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultGoalTrackerModel>> CreateDefaultGoalTracker([FromBody] DefaultGoalTrackerCreateModel defaultGoalTrackerModel)
        {
            return manifestService.CreateDefaultGoalTrackerAsync(defaultGoalTrackerModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultGoalTrackerModel>> CreateMultipleDefaultGoalTracker([FromBody] MultipleDefaultGoalTrackerCreateModel defaultGoalTrackerModels)
        {
            return manifestService.CreateMultipleDefaultGoalTrackerAsync(defaultGoalTrackerModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultGoalTrackerModel>> UpdateDefaultGoalTracker([FromBody] DefaultGoalTrackerUpdateModel defaultGoalTrackerModel)
        {
            return manifestService.UpdateDefaultGoalTrackerAsync(defaultGoalTrackerModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDefaultGoalTrackerAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkDefaultGoalTrackerAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDefaultGoalTrackerAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkDefaultGoalTrackerAsNotDeletedByIdAsync(id);
        }




        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDefaultGoalTrackerFileValid([FromQuery] long defaultGoalTrackerId)
        {
            return manifestService.IsDefaultGoalTrackerFileValidAsync(defaultGoalTrackerId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDefaultGoalTrackerFile([FromQuery] long defaultGoalTrackerId)
        {
            var result = await manifestService.GetDefaultGoalTrackerFileAsync(defaultGoalTrackerId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadDefaultGoalTrackerFile([FromForm] DefaultGoalTrackerFileWithFileFormModel model)
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

            return productService.UploadDefaultGoalTrackerFileAsync(model.DefaultGoalTrackerId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearDefaultGoalTrackerFile([FromQuery] long movieId)
        {
            return productService.ClearDefaultGoalTrackerFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDefaultGoalTrackerSourceWordFileValid([FromQuery] long defaultGoalTrackerId)
        {
            return manifestService.IsDefaultGoalTrackerWordOrPdfFileValidAsync(defaultGoalTrackerId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDefaultGoalTrackerSourceWordFile([FromQuery] long defaultGoalTrackerId)
        {
            var result = await manifestService.GetDefaultGoalTrackerWordOrdPdfFileAsync(defaultGoalTrackerId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableDefaultGoalTracker([FromBody] long id)
        {
            return manifestService.EnableDefaultGoalTrackerByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableDefaultGoalTracker([FromBody] long id)
        {
            return manifestService.DisableDefaultGoalTrackerByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByDefaultGoalTracker(long id)
        {
            return manifestService.GetXYZZYXsByDefaultGoalTrackerAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithDefaultGoalTracker(DefaultGoalTrackerXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithDefaultGoalTrackerAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithDefaultGoalTracker(DefaultGoalTrackerXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithDefaultGoalTrackerAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportDefaultGoalTracker([FromBody] ImportDefaultGoalTrackerModel model)
        {
            return manifestService.ImportDefaultGoalTrackerAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportDefaultGoalTracker()
        {
            var lst = new List<ImportDefaultGoalTrackerModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportDefaultGoalTrackerModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleDefaultGoalTrackerImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<DefaultGoalTrackerExportModel>> DefaultGoalTrackerReport([FromQuery] DefaultGoalTrackerExportParameter parameter)
        {
            return reportService.ExportDefaultGoalTrackerAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> DefaultGoalTrackerAsReportCsv([FromQuery] DefaultGoalTrackerExportParameter parameter)
        {
            var result = await reportService.ExportDefaultGoalTrackerAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("DefaultGoalTrackerAsCsv.csv"))
                {
                    FileDownloadName = "DefaultGoalTrackerExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region DefaultGoalTrackerCategory
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultGoalTrackerCategoryModel>> GetDefaultGoalTrackerCategorys([FromQuery] DefaultGoalTrackerCategorySearchCriteria search)
        {
            return manifestService.GetDefaultGoalTrackerCategorysAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultGoalTrackerCategoryModel>> GetDefaultGoalTrackerCategory([FromQuery] long id)
        {
            return manifestService.GetDefaultGoalTrackerCategoryByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultGoalTrackerCategoryModel>> CreateDefaultGoalTrackerCategory([FromBody] DefaultGoalTrackerCategoryCreateModel defaultGoalTrackerCategoryModel)
        {
            return manifestService.CreateDefaultGoalTrackerCategoryAsync(defaultGoalTrackerCategoryModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultGoalTrackerCategoryModel>> CreateMultipleDefaultGoalTrackerCategory([FromBody] MultipleDefaultGoalTrackerCategoryCreateModel defaultGoalTrackerCategoryModels)
        {
            return manifestService.CreateMultipleDefaultGoalTrackerCategoryAsync(defaultGoalTrackerCategoryModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultGoalTrackerCategoryModel>> UpdateDefaultGoalTrackerCategory([FromBody] DefaultGoalTrackerCategoryUpdateModel defaultGoalTrackerCategoryModel)
        {
            return manifestService.UpdateDefaultGoalTrackerCategoryAsync(defaultGoalTrackerCategoryModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDefaultGoalTrackerCategoryAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkDefaultGoalTrackerCategoryAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDefaultGoalTrackerCategoryAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkDefaultGoalTrackerCategoryAsNotDeletedByIdAsync(id);
        }



        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultGoalTrackerCategoryModel>> GetDefaultGoalTrackerCategoryByDefaultCategoryId([FromQuery] long defaultCategoryId)
        {
            return manifestService.GetDefaultGoalTrackerCategoryByDefaultCategoryIdAsync(defaultCategoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultGoalTrackerCategoryModel>> GetDefaultGoalTrackerCategoryByDefaultCategoryIdSearch(DefaultGoalTrackerCategoryByDefaultCategoryIdSearchCriteria search)
        {
            return manifestService.GetDefaultGoalTrackerCategoryByDefaultCategoryIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultCategoryModel>> GetDefaultCategoryByDefaultGoalTrackerCategoryId(long defaultGoalTrackerCategoryId)
        {
            return manifestService.GetDefaultCategoryByDefaultGoalTrackerCategoryIdAsync(defaultGoalTrackerCategoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultCategoryModel>> GetDefaultCategoryByDefaultGoalTrackerCategoryIdSearch(DefaultCategoryByDefaultGoalTrackerCategoryIdSearchCriteria search)
        {
            return manifestService.GetDefaultCategoryByDefaultGoalTrackerCategoryIdAsync(search);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultGoalTrackerCategoryModel>> GetDefaultGoalTrackerCategoryByDefaultGoalTrackerId([FromQuery] long defaultGoalTrackerId)
        {
            return manifestService.GetDefaultGoalTrackerCategoryByDefaultGoalTrackerIdAsync(defaultGoalTrackerId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultGoalTrackerCategoryModel>> GetDefaultGoalTrackerCategoryByDefaultGoalTrackerIdSearch(DefaultGoalTrackerCategoryByDefaultGoalTrackerIdSearchCriteria search)
        {
            return manifestService.GetDefaultGoalTrackerCategoryByDefaultGoalTrackerIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultGoalTrackerModel>> GetDefaultGoalTrackerByDefaultGoalTrackerCategoryId(long defaultGoalTrackerCategoryId)
        {
            return manifestService.GetDefaultGoalTrackerByDefaultGoalTrackerCategoryIdAsync(defaultGoalTrackerCategoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultGoalTrackerModel>> GetDefaultGoalTrackerByDefaultGoalTrackerCategoryIdSearch(DefaultGoalTrackerByDefaultGoalTrackerCategoryIdSearchCriteria search)
        {
            return manifestService.GetDefaultGoalTrackerByDefaultGoalTrackerCategoryIdAsync(search);
        }



        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDefaultGoalTrackerCategoryFileValid([FromQuery] long defaultGoalTrackerCategoryId)
        {
            return manifestService.IsDefaultGoalTrackerCategoryFileValidAsync(defaultGoalTrackerCategoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDefaultGoalTrackerCategoryFile([FromQuery] long defaultGoalTrackerCategoryId)
        {
            var result = await manifestService.GetDefaultGoalTrackerCategoryFileAsync(defaultGoalTrackerCategoryId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadDefaultGoalTrackerCategoryFile([FromForm] DefaultGoalTrackerCategoryFileWithFileFormModel model)
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

            return productService.UploadDefaultGoalTrackerCategoryFileAsync(model.DefaultGoalTrackerCategoryId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearDefaultGoalTrackerCategoryFile([FromQuery] long movieId)
        {
            return productService.ClearDefaultGoalTrackerCategoryFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDefaultGoalTrackerCategorySourceWordFileValid([FromQuery] long defaultGoalTrackerCategoryId)
        {
            return manifestService.IsDefaultGoalTrackerCategoryWordOrPdfFileValidAsync(defaultGoalTrackerCategoryId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDefaultGoalTrackerCategorySourceWordFile([FromQuery] long defaultGoalTrackerCategoryId)
        {
            var result = await manifestService.GetDefaultGoalTrackerCategoryWordOrdPdfFileAsync(defaultGoalTrackerCategoryId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableDefaultGoalTrackerCategory([FromBody] long id)
        {
            return manifestService.EnableDefaultGoalTrackerCategoryByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableDefaultGoalTrackerCategory([FromBody] long id)
        {
            return manifestService.DisableDefaultGoalTrackerCategoryByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByDefaultGoalTrackerCategory(long id)
        {
            return manifestService.GetXYZZYXsByDefaultGoalTrackerCategoryAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithDefaultGoalTrackerCategory(DefaultGoalTrackerCategoryXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithDefaultGoalTrackerCategoryAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithDefaultGoalTrackerCategory(DefaultGoalTrackerCategoryXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithDefaultGoalTrackerCategoryAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportDefaultGoalTrackerCategory([FromBody] ImportDefaultGoalTrackerCategoryModel model)
        {
            return manifestService.ImportDefaultGoalTrackerCategoryAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportDefaultGoalTrackerCategory()
        {
            var lst = new List<ImportDefaultGoalTrackerCategoryModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportDefaultGoalTrackerCategoryModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleDefaultGoalTrackerCategoryImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<DefaultGoalTrackerCategoryExportModel>> DefaultGoalTrackerCategoryReport([FromQuery] DefaultGoalTrackerCategoryExportParameter parameter)
        {
            return reportService.ExportDefaultGoalTrackerCategoryAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> DefaultGoalTrackerCategoryAsReportCsv([FromQuery] DefaultGoalTrackerCategoryExportParameter parameter)
        {
            var result = await reportService.ExportDefaultGoalTrackerCategoryAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("DefaultGoalTrackerCategoryAsCsv.csv"))
                {
                    FileDownloadName = "DefaultGoalTrackerCategoryExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region DefaultMeditationFile
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultMeditationFileModel>> GetDefaultMeditationFiles([FromQuery] DefaultMeditationFileSearchCriteria search)
        {
            return manifestService.GetDefaultMeditationFilesAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultMeditationFileModel>> GetDefaultMeditationFile([FromQuery] long id)
        {
            return manifestService.GetDefaultMeditationFileByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultMeditationFileModel>> CreateDefaultMeditationFile([FromBody] DefaultMeditationFileCreateModel defaultMeditationFileModel)
        {
            return manifestService.CreateDefaultMeditationFileAsync(defaultMeditationFileModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultMeditationFileModel>> CreateMultipleDefaultMeditationFile([FromBody] MultipleDefaultMeditationFileCreateModel defaultMeditationFileModels)
        {
            return manifestService.CreateMultipleDefaultMeditationFileAsync(defaultMeditationFileModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultMeditationFileModel>> UpdateDefaultMeditationFile([FromBody] DefaultMeditationFileUpdateModel defaultMeditationFileModel)
        {
            return manifestService.UpdateDefaultMeditationFileAsync(defaultMeditationFileModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDefaultMeditationFileAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkDefaultMeditationFileAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDefaultMeditationFileAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkDefaultMeditationFileAsNotDeletedByIdAsync(id);
        }




        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDefaultMeditationFileFileValid([FromQuery] long defaultMeditationFileId)
        {
            return manifestService.IsDefaultMeditationFileFileValidAsync(defaultMeditationFileId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDefaultMeditationFileFile([FromQuery] long defaultMeditationFileId)
        {
            var result = await manifestService.GetDefaultMeditationFileFileAsync(defaultMeditationFileId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadDefaultMeditationFileFile([FromForm] DefaultMeditationFileFileWithFileFormModel model)
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

            return productService.UploadDefaultMeditationFileFileAsync(model.DefaultMeditationFileId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearDefaultMeditationFileFile([FromQuery] long movieId)
        {
            return productService.ClearDefaultMeditationFileFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDefaultMeditationFileSourceWordFileValid([FromQuery] long defaultMeditationFileId)
        {
            return manifestService.IsDefaultMeditationFileWordOrPdfFileValidAsync(defaultMeditationFileId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDefaultMeditationFileSourceWordFile([FromQuery] long defaultMeditationFileId)
        {
            var result = await manifestService.GetDefaultMeditationFileWordOrdPdfFileAsync(defaultMeditationFileId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableDefaultMeditationFile([FromBody] long id)
        {
            return manifestService.EnableDefaultMeditationFileByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableDefaultMeditationFile([FromBody] long id)
        {
            return manifestService.DisableDefaultMeditationFileByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByDefaultMeditationFile(long id)
        {
            return manifestService.GetXYZZYXsByDefaultMeditationFileAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithDefaultMeditationFile(DefaultMeditationFileXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithDefaultMeditationFileAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithDefaultMeditationFile(DefaultMeditationFileXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithDefaultMeditationFileAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportDefaultMeditationFile([FromBody] ImportDefaultMeditationFileModel model)
        {
            return manifestService.ImportDefaultMeditationFileAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportDefaultMeditationFile()
        {
            var lst = new List<ImportDefaultMeditationFileModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportDefaultMeditationFileModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleDefaultMeditationFileImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<DefaultMeditationFileExportModel>> DefaultMeditationFileReport([FromQuery] DefaultMeditationFileExportParameter parameter)
        {
            return reportService.ExportDefaultMeditationFileAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> DefaultMeditationFileAsReportCsv([FromQuery] DefaultMeditationFileExportParameter parameter)
        {
            var result = await reportService.ExportDefaultMeditationFileAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("DefaultMeditationFileAsCsv.csv"))
                {
                    FileDownloadName = "DefaultMeditationFileExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region DefaultVisionBoardPicture
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultVisionBoardPictureModel>> GetDefaultVisionBoardPictures([FromQuery] DefaultVisionBoardPictureSearchCriteria search)
        {
            return manifestService.GetDefaultVisionBoardPicturesAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultVisionBoardPictureModel>> GetDefaultVisionBoardPicture([FromQuery] long id)
        {
            return manifestService.GetDefaultVisionBoardPictureByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultVisionBoardPictureModel>> CreateDefaultVisionBoardPicture([FromBody] DefaultVisionBoardPictureCreateModel defaultVisionBoardPictureModel)
        {
            return manifestService.CreateDefaultVisionBoardPictureAsync(defaultVisionBoardPictureModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DefaultVisionBoardPictureModel>> CreateMultipleDefaultVisionBoardPicture([FromBody] MultipleDefaultVisionBoardPictureCreateModel defaultVisionBoardPictureModels)
        {
            return manifestService.CreateMultipleDefaultVisionBoardPictureAsync(defaultVisionBoardPictureModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DefaultVisionBoardPictureModel>> UpdateDefaultVisionBoardPicture([FromBody] DefaultVisionBoardPictureUpdateModel defaultVisionBoardPictureModel)
        {
            return manifestService.UpdateDefaultVisionBoardPictureAsync(defaultVisionBoardPictureModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDefaultVisionBoardPictureAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkDefaultVisionBoardPictureAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDefaultVisionBoardPictureAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkDefaultVisionBoardPictureAsNotDeletedByIdAsync(id);
        }




        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDefaultVisionBoardPictureFileValid([FromQuery] long defaultVisionBoardPictureId)
        {
            return manifestService.IsDefaultVisionBoardPictureFileValidAsync(defaultVisionBoardPictureId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDefaultVisionBoardPictureFile([FromQuery] long defaultVisionBoardPictureId)
        {
            var result = await manifestService.GetDefaultVisionBoardPictureFileAsync(defaultVisionBoardPictureId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadDefaultVisionBoardPictureFile([FromForm] DefaultVisionBoardPictureFileWithFileFormModel model)
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

            return productService.UploadDefaultVisionBoardPictureFileAsync(model.DefaultVisionBoardPictureId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearDefaultVisionBoardPictureFile([FromQuery] long movieId)
        {
            return productService.ClearDefaultVisionBoardPictureFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDefaultVisionBoardPictureSourceWordFileValid([FromQuery] long defaultVisionBoardPictureId)
        {
            return manifestService.IsDefaultVisionBoardPictureWordOrPdfFileValidAsync(defaultVisionBoardPictureId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDefaultVisionBoardPictureSourceWordFile([FromQuery] long defaultVisionBoardPictureId)
        {
            var result = await manifestService.GetDefaultVisionBoardPictureWordOrdPdfFileAsync(defaultVisionBoardPictureId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableDefaultVisionBoardPicture([FromBody] long id)
        {
            return manifestService.EnableDefaultVisionBoardPictureByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableDefaultVisionBoardPicture([FromBody] long id)
        {
            return manifestService.DisableDefaultVisionBoardPictureByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByDefaultVisionBoardPicture(long id)
        {
            return manifestService.GetXYZZYXsByDefaultVisionBoardPictureAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithDefaultVisionBoardPicture(DefaultVisionBoardPictureXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithDefaultVisionBoardPictureAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithDefaultVisionBoardPicture(DefaultVisionBoardPictureXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithDefaultVisionBoardPictureAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportDefaultVisionBoardPicture([FromBody] ImportDefaultVisionBoardPictureModel model)
        {
            return manifestService.ImportDefaultVisionBoardPictureAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportDefaultVisionBoardPicture()
        {
            var lst = new List<ImportDefaultVisionBoardPictureModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportDefaultVisionBoardPictureModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleDefaultVisionBoardPictureImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<DefaultVisionBoardPictureExportModel>> DefaultVisionBoardPictureReport([FromQuery] DefaultVisionBoardPictureExportParameter parameter)
        {
            return reportService.ExportDefaultVisionBoardPictureAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> DefaultVisionBoardPictureAsReportCsv([FromQuery] DefaultVisionBoardPictureExportParameter parameter)
        {
            var result = await reportService.ExportDefaultVisionBoardPictureAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("DefaultVisionBoardPictureAsCsv.csv"))
                {
                    FileDownloadName = "DefaultVisionBoardPictureExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region Feed
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FeedModel>> GetFeeds([FromQuery] FeedSearchCriteria search)
        {
            return manifestService.GetFeedsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<FeedModel>> GetFeed([FromQuery] long id)
        {
            return manifestService.GetFeedByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<FeedModel>> CreateFeed([FromBody] FeedCreateModel feedModel)
        {
            return manifestService.CreateFeedAsync(feedModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FeedModel>> CreateMultipleFeed([FromBody] MultipleFeedCreateModel feedModels)
        {
            return manifestService.CreateMultipleFeedAsync(feedModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<FeedModel>> UpdateFeed([FromBody] FeedUpdateModel feedModel)
        {
            return manifestService.UpdateFeedAsync(feedModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkFeedAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkFeedAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkFeedAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkFeedAsNotDeletedByIdAsync(id);
        }



        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FeedModel>> GetFeedByUserId([FromQuery] long userId)
        {
            return manifestService.GetFeedByUserIdAsync(userId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FeedModel>> GetFeedByUserIdSearch(FeedByUserIdSearchCriteria search)
        {
            return manifestService.GetFeedByUserIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByFeedId(long feedId)
        {
            return manifestService.GetUserByFeedIdAsync(feedId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByFeedIdSearch(UserByFeedIdSearchCriteria search)
        {
            return manifestService.GetUserByFeedIdAsync(search);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FeedModel>> GetFeedByDailyAffirmationSessionId([FromQuery] long dailyAffirmationSessionId)
        {
            return manifestService.GetFeedByDailyAffirmationSessionIdAsync(dailyAffirmationSessionId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FeedModel>> GetFeedByDailyAffirmationSessionIdSearch(FeedByDailyAffirmationSessionIdSearchCriteria search)
        {
            return manifestService.GetFeedByDailyAffirmationSessionIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationSessionModel>> GetDailyAffirmationSessionByFeedId(long feedId)
        {
            return manifestService.GetDailyAffirmationSessionByFeedIdAsync(feedId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyAffirmationSessionModel>> GetDailyAffirmationSessionByFeedIdSearch(DailyAffirmationSessionByFeedIdSearchCriteria search)
        {
            return manifestService.GetDailyAffirmationSessionByFeedIdAsync(search);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FeedModel>> GetFeedByVisionBoardSessionId([FromQuery] long visionBoardSessionId)
        {
            return manifestService.GetFeedByVisionBoardSessionIdAsync(visionBoardSessionId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FeedModel>> GetFeedByVisionBoardSessionIdSearch(FeedByVisionBoardSessionIdSearchCriteria search)
        {
            return manifestService.GetFeedByVisionBoardSessionIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardSessionModel>> GetVisionBoardSessionByFeedId(long feedId)
        {
            return manifestService.GetVisionBoardSessionByFeedIdAsync(feedId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardSessionModel>> GetVisionBoardSessionByFeedIdSearch(VisionBoardSessionByFeedIdSearchCriteria search)
        {
            return manifestService.GetVisionBoardSessionByFeedIdAsync(search);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FeedModel>> GetFeedByMeditationSessionId([FromQuery] long meditationSessionId)
        {
            return manifestService.GetFeedByMeditationSessionIdAsync(meditationSessionId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FeedModel>> GetFeedByMeditationSessionIdSearch(FeedByMeditationSessionIdSearchCriteria search)
        {
            return manifestService.GetFeedByMeditationSessionIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<MeditationSessionModel>> GetMeditationSessionByFeedId(long feedId)
        {
            return manifestService.GetMeditationSessionByFeedIdAsync(feedId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<MeditationSessionModel>> GetMeditationSessionByFeedIdSearch(MeditationSessionByFeedIdSearchCriteria search)
        {
            return manifestService.GetMeditationSessionByFeedIdAsync(search);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FeedModel>> GetFeedByGoalTrackerId([FromQuery] long goalTrackerId)
        {
            return manifestService.GetFeedByGoalTrackerIdAsync(goalTrackerId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FeedModel>> GetFeedByGoalTrackerIdSearch(FeedByGoalTrackerIdSearchCriteria search)
        {
            return manifestService.GetFeedByGoalTrackerIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerModel>> GetGoalTrackerByFeedId(long feedId)
        {
            return manifestService.GetGoalTrackerByFeedIdAsync(feedId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerModel>> GetGoalTrackerByFeedIdSearch(GoalTrackerByFeedIdSearchCriteria search)
        {
            return manifestService.GetGoalTrackerByFeedIdAsync(search);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FeedModel>> GetFeedByGoalTrackerMileStoneId([FromQuery] long goalTrackerMileStoneId)
        {
            return manifestService.GetFeedByGoalTrackerMileStoneIdAsync(goalTrackerMileStoneId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<FeedModel>> GetFeedByGoalTrackerMileStoneIdSearch(FeedByGoalTrackerMileStoneIdSearchCriteria search)
        {
            return manifestService.GetFeedByGoalTrackerMileStoneIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerMileStoneModel>> GetGoalTrackerMileStoneByFeedId(long feedId)
        {
            return manifestService.GetGoalTrackerMileStoneByFeedIdAsync(feedId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerMileStoneModel>> GetGoalTrackerMileStoneByFeedIdSearch(GoalTrackerMileStoneByFeedIdSearchCriteria search)
        {
            return manifestService.GetGoalTrackerMileStoneByFeedIdAsync(search);
        }



        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsFeedFileValid([FromQuery] long feedId)
        {
            return manifestService.IsFeedFileValidAsync(feedId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadFeedFile([FromQuery] long feedId)
        {
            var result = await manifestService.GetFeedFileAsync(feedId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadFeedFile([FromForm] FeedFileWithFileFormModel model)
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

            return productService.UploadFeedFileAsync(model.FeedId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearFeedFile([FromQuery] long movieId)
        {
            return productService.ClearFeedFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsFeedSourceWordFileValid([FromQuery] long feedId)
        {
            return manifestService.IsFeedWordOrPdfFileValidAsync(feedId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadFeedSourceWordFile([FromQuery] long feedId)
        {
            var result = await manifestService.GetFeedWordOrdPdfFileAsync(feedId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableFeed([FromBody] long id)
        {
            return manifestService.EnableFeedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableFeed([FromBody] long id)
        {
            return manifestService.DisableFeedByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByFeed(long id)
        {
            return manifestService.GetXYZZYXsByFeedAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithFeed(FeedXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithFeedAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithFeed(FeedXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithFeedAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportFeed([FromBody] ImportFeedModel model)
        {
            return manifestService.ImportFeedAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportFeed()
        {
            var lst = new List<ImportFeedModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportFeedModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleFeedImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<FeedExportModel>> FeedReport([FromQuery] FeedExportParameter parameter)
        {
            return reportService.ExportFeedAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> FeedAsReportCsv([FromQuery] FeedExportParameter parameter)
        {
            var result = await reportService.ExportFeedAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("FeedAsCsv.csv"))
                {
                    FileDownloadName = "FeedExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region GoalTracker

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerModel>> GetGoalTrackersByCategoryId([FromQuery] long categoryId)
        {
            return manifestService.GetGoalTrackersByCategoryIdAsync(categoryId < 0 ? null : categoryId);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerModel>> GetGoalTrackers([FromQuery] GoalTrackerSearchCriteria search)
        {
            return manifestService.GetGoalTrackersAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<GoalTrackerModel>> GetGoalTracker([FromQuery] long id)
        {
            return manifestService.GetGoalTrackerByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<GoalTrackerModel>> CreateGoalTracker([FromBody] GoalTrackerCreateModel goalTrackerModel)
        {
            return manifestService.CreateGoalTrackerAsync(goalTrackerModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerModel>> CreateMultipleGoalTracker([FromBody] MultipleGoalTrackerCreateModel goalTrackerModels)
        {
            return manifestService.CreateMultipleGoalTrackerAsync(goalTrackerModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<GoalTrackerModel>> UpdateGoalTracker([FromBody] GoalTrackerUpdateModel goalTrackerModel)
        {
            return manifestService.UpdateGoalTrackerAsync(goalTrackerModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkGoalTrackerAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkGoalTrackerAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkGoalTrackerAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkGoalTrackerAsNotDeletedByIdAsync(id);
        }



        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerModel>> GetGoalTrackerByUserId([FromQuery] long userId)
        {
            return manifestService.GetGoalTrackerByUserIdAsync(userId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerModel>> GetGoalTrackerByUserIdSearch(GoalTrackerByUserIdSearchCriteria search)
        {
            return manifestService.GetGoalTrackerByUserIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByGoalTrackerId(long goalTrackerId)
        {
            return manifestService.GetUserByGoalTrackerIdAsync(goalTrackerId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByGoalTrackerIdSearch(UserByGoalTrackerIdSearchCriteria search)
        {
            return manifestService.GetUserByGoalTrackerIdAsync(search);
        }



        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsGoalTrackerFileValid([FromQuery] long goalTrackerId)
        {
            return manifestService.IsGoalTrackerFileValidAsync(goalTrackerId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadGoalTrackerFile([FromQuery] long goalTrackerId)
        {
            var result = await manifestService.GetGoalTrackerFileAsync(goalTrackerId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadGoalTrackerFile([FromForm] GoalTrackerFileWithFileFormModel model)
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

            return productService.UploadGoalTrackerFileAsync(model.GoalTrackerId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearGoalTrackerFile([FromQuery] long movieId)
        {
            return productService.ClearGoalTrackerFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsGoalTrackerSourceWordFileValid([FromQuery] long goalTrackerId)
        {
            return manifestService.IsGoalTrackerWordOrPdfFileValidAsync(goalTrackerId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadGoalTrackerSourceWordFile([FromQuery] long goalTrackerId)
        {
            var result = await manifestService.GetGoalTrackerWordOrdPdfFileAsync(goalTrackerId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableGoalTracker([FromBody] long id)
        {
            return manifestService.EnableGoalTrackerByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableGoalTracker([FromBody] long id)
        {
            return manifestService.DisableGoalTrackerByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByGoalTracker(long id)
        {
            return manifestService.GetXYZZYXsByGoalTrackerAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithGoalTracker(GoalTrackerXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithGoalTrackerAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithGoalTracker(GoalTrackerXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithGoalTrackerAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportGoalTracker([FromBody] ImportGoalTrackerModel model)
        {
            return manifestService.ImportGoalTrackerAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportGoalTracker()
        {
            var lst = new List<ImportGoalTrackerModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportGoalTrackerModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleGoalTrackerImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<GoalTrackerExportModel>> GoalTrackerReport([FromQuery] GoalTrackerExportParameter parameter)
        {
            return reportService.ExportGoalTrackerAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> GoalTrackerAsReportCsv([FromQuery] GoalTrackerExportParameter parameter)
        {
            var result = await reportService.ExportGoalTrackerAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("GoalTrackerAsCsv.csv"))
                {
                    FileDownloadName = "GoalTrackerExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region GoalTrackerCategory
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<CategoryModel>> GetAllDistinctCategorysForAllGoalTrackerForCurrentUser()
        {
            return manifestService.GetAllDistinctCategorysForAllGoalTrackerForCurrentUserAsync();
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerCategoryModel>> GetGoalTrackerCategorys([FromQuery] GoalTrackerCategorySearchCriteria search)
        {
            return manifestService.GetGoalTrackerCategorysAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<GoalTrackerCategoryModel>> GetGoalTrackerCategory([FromQuery] long id)
        {
            return manifestService.GetGoalTrackerCategoryByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<GoalTrackerCategoryModel>> CreateGoalTrackerCategory([FromBody] GoalTrackerCategoryCreateModel goalTrackerCategoryModel)
        {
            return manifestService.CreateGoalTrackerCategoryAsync(goalTrackerCategoryModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerCategoryModel>> CreateMultipleGoalTrackerCategory([FromBody] MultipleGoalTrackerCategoryCreateModel goalTrackerCategoryModels)
        {
            return manifestService.CreateMultipleGoalTrackerCategoryAsync(goalTrackerCategoryModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<GoalTrackerCategoryModel>> UpdateGoalTrackerCategory([FromBody] GoalTrackerCategoryUpdateModel goalTrackerCategoryModel)
        {
            return manifestService.UpdateGoalTrackerCategoryAsync(goalTrackerCategoryModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkGoalTrackerCategoryAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkGoalTrackerCategoryAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkGoalTrackerCategoryAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkGoalTrackerCategoryAsNotDeletedByIdAsync(id);
        }



        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerCategoryModel>> GetGoalTrackerCategoryByCategoryId([FromQuery] long categoryId)
        {
            return manifestService.GetGoalTrackerCategoryByCategoryIdAsync(categoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerCategoryModel>> GetGoalTrackerCategoryByCategoryIdSearch(GoalTrackerCategoryByCategoryIdSearchCriteria search)
        {
            return manifestService.GetGoalTrackerCategoryByCategoryIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<CategoryModel>> GetCategoryByGoalTrackerCategoryId(long goalTrackerCategoryId)
        {
            return manifestService.GetCategoryByGoalTrackerCategoryIdAsync(goalTrackerCategoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<CategoryModel>> GetCategoryByGoalTrackerCategoryIdSearch(CategoryByGoalTrackerCategoryIdSearchCriteria search)
        {
            return manifestService.GetCategoryByGoalTrackerCategoryIdAsync(search);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerCategoryModel>> GetGoalTrackerCategoryByGoalTrackerId([FromQuery] long goalTrackerId)
        {
            return manifestService.GetGoalTrackerCategoryByGoalTrackerIdAsync(goalTrackerId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerCategoryModel>> GetGoalTrackerCategoryByGoalTrackerIdSearch(GoalTrackerCategoryByGoalTrackerIdSearchCriteria search)
        {
            return manifestService.GetGoalTrackerCategoryByGoalTrackerIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerModel>> GetGoalTrackerByGoalTrackerCategoryId(long goalTrackerCategoryId)
        {
            return manifestService.GetGoalTrackerByGoalTrackerCategoryIdAsync(goalTrackerCategoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerModel>> GetGoalTrackerByGoalTrackerCategoryIdSearch(GoalTrackerByGoalTrackerCategoryIdSearchCriteria search)
        {
            return manifestService.GetGoalTrackerByGoalTrackerCategoryIdAsync(search);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerCategoryModel>> GetGoalTrackerCategoryByUserId([FromQuery] long userId)
        {
            return manifestService.GetGoalTrackerCategoryByUserIdAsync(userId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerCategoryModel>> GetGoalTrackerCategoryByUserIdSearch(GoalTrackerCategoryByUserIdSearchCriteria search)
        {
            return manifestService.GetGoalTrackerCategoryByUserIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByGoalTrackerCategoryId(long goalTrackerCategoryId)
        {
            return manifestService.GetUserByGoalTrackerCategoryIdAsync(goalTrackerCategoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByGoalTrackerCategoryIdSearch(UserByGoalTrackerCategoryIdSearchCriteria search)
        {
            return manifestService.GetUserByGoalTrackerCategoryIdAsync(search);
        }



        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsGoalTrackerCategoryFileValid([FromQuery] long goalTrackerCategoryId)
        {
            return manifestService.IsGoalTrackerCategoryFileValidAsync(goalTrackerCategoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadGoalTrackerCategoryFile([FromQuery] long goalTrackerCategoryId)
        {
            var result = await manifestService.GetGoalTrackerCategoryFileAsync(goalTrackerCategoryId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadGoalTrackerCategoryFile([FromForm] GoalTrackerCategoryFileWithFileFormModel model)
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

            return productService.UploadGoalTrackerCategoryFileAsync(model.GoalTrackerCategoryId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearGoalTrackerCategoryFile([FromQuery] long movieId)
        {
            return productService.ClearGoalTrackerCategoryFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsGoalTrackerCategorySourceWordFileValid([FromQuery] long goalTrackerCategoryId)
        {
            return manifestService.IsGoalTrackerCategoryWordOrPdfFileValidAsync(goalTrackerCategoryId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadGoalTrackerCategorySourceWordFile([FromQuery] long goalTrackerCategoryId)
        {
            var result = await manifestService.GetGoalTrackerCategoryWordOrdPdfFileAsync(goalTrackerCategoryId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableGoalTrackerCategory([FromBody] long id)
        {
            return manifestService.EnableGoalTrackerCategoryByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableGoalTrackerCategory([FromBody] long id)
        {
            return manifestService.DisableGoalTrackerCategoryByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByGoalTrackerCategory(long id)
        {
            return manifestService.GetXYZZYXsByGoalTrackerCategoryAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithGoalTrackerCategory(GoalTrackerCategoryXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithGoalTrackerCategoryAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithGoalTrackerCategory(GoalTrackerCategoryXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithGoalTrackerCategoryAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportGoalTrackerCategory([FromBody] ImportGoalTrackerCategoryModel model)
        {
            return manifestService.ImportGoalTrackerCategoryAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportGoalTrackerCategory()
        {
            var lst = new List<ImportGoalTrackerCategoryModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportGoalTrackerCategoryModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleGoalTrackerCategoryImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<GoalTrackerCategoryExportModel>> GoalTrackerCategoryReport([FromQuery] GoalTrackerCategoryExportParameter parameter)
        {
            return reportService.ExportGoalTrackerCategoryAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> GoalTrackerCategoryAsReportCsv([FromQuery] GoalTrackerCategoryExportParameter parameter)
        {
            var result = await reportService.ExportGoalTrackerCategoryAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("GoalTrackerCategoryAsCsv.csv"))
                {
                    FileDownloadName = "GoalTrackerCategoryExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region GoalTrackerMileStone
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerMileStoneModel>> GetGoalTrackerMileStones([FromQuery] GoalTrackerMileStoneSearchCriteria search)
        {
            return manifestService.GetGoalTrackerMileStonesAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<GoalTrackerMileStoneModel>> GetGoalTrackerMileStone([FromQuery] long id)
        {
            return manifestService.GetGoalTrackerMileStoneByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<GoalTrackerMileStoneModel>> CreateGoalTrackerMileStone([FromBody] GoalTrackerMileStoneCreateModel goalTrackerMileStoneModel)
        {
            return manifestService.CreateGoalTrackerMileStoneAsync(goalTrackerMileStoneModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerMileStoneModel>> CreateMultipleGoalTrackerMileStone([FromBody] MultipleGoalTrackerMileStoneCreateModel goalTrackerMileStoneModels)
        {
            return manifestService.CreateMultipleGoalTrackerMileStoneAsync(goalTrackerMileStoneModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<GoalTrackerMileStoneModel>> UpdateGoalTrackerMileStone([FromBody] GoalTrackerMileStoneUpdateModel goalTrackerMileStoneModel)
        {
            return manifestService.UpdateGoalTrackerMileStoneAsync(goalTrackerMileStoneModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkGoalTrackerMileStoneAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkGoalTrackerMileStoneAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkGoalTrackerMileStoneAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkGoalTrackerMileStoneAsNotDeletedByIdAsync(id);
        }



        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerMileStoneModel>> GetGoalTrackerMileStoneByGoalTrackerId([FromQuery] long goalTrackerId)
        {
            return manifestService.GetGoalTrackerMileStoneByGoalTrackerIdAsync(goalTrackerId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerMileStoneModel>> GetGoalTrackerMileStoneByGoalTrackerIdSearch(GoalTrackerMileStoneByGoalTrackerIdSearchCriteria search)
        {
            return manifestService.GetGoalTrackerMileStoneByGoalTrackerIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerModel>> GetGoalTrackerByGoalTrackerMileStoneId(long goalTrackerMileStoneId)
        {
            return manifestService.GetGoalTrackerByGoalTrackerMileStoneIdAsync(goalTrackerMileStoneId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerModel>> GetGoalTrackerByGoalTrackerMileStoneIdSearch(GoalTrackerByGoalTrackerMileStoneIdSearchCriteria search)
        {
            return manifestService.GetGoalTrackerByGoalTrackerMileStoneIdAsync(search);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerMileStoneModel>> GetGoalTrackerMileStoneByUserId([FromQuery] long userId)
        {
            return manifestService.GetGoalTrackerMileStoneByUserIdAsync(userId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<GoalTrackerMileStoneModel>> GetGoalTrackerMileStoneByUserIdSearch(GoalTrackerMileStoneByUserIdSearchCriteria search)
        {
            return manifestService.GetGoalTrackerMileStoneByUserIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByGoalTrackerMileStoneId(long goalTrackerMileStoneId)
        {
            return manifestService.GetUserByGoalTrackerMileStoneIdAsync(goalTrackerMileStoneId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByGoalTrackerMileStoneIdSearch(UserByGoalTrackerMileStoneIdSearchCriteria search)
        {
            return manifestService.GetUserByGoalTrackerMileStoneIdAsync(search);
        }



        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsGoalTrackerMileStoneFileValid([FromQuery] long goalTrackerMileStoneId)
        {
            return manifestService.IsGoalTrackerMileStoneFileValidAsync(goalTrackerMileStoneId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadGoalTrackerMileStoneFile([FromQuery] long goalTrackerMileStoneId)
        {
            var result = await manifestService.GetGoalTrackerMileStoneFileAsync(goalTrackerMileStoneId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadGoalTrackerMileStoneFile([FromForm] GoalTrackerMileStoneFileWithFileFormModel model)
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

            return productService.UploadGoalTrackerMileStoneFileAsync(model.GoalTrackerMileStoneId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearGoalTrackerMileStoneFile([FromQuery] long movieId)
        {
            return productService.ClearGoalTrackerMileStoneFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsGoalTrackerMileStoneSourceWordFileValid([FromQuery] long goalTrackerMileStoneId)
        {
            return manifestService.IsGoalTrackerMileStoneWordOrPdfFileValidAsync(goalTrackerMileStoneId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadGoalTrackerMileStoneSourceWordFile([FromQuery] long goalTrackerMileStoneId)
        {
            var result = await manifestService.GetGoalTrackerMileStoneWordOrdPdfFileAsync(goalTrackerMileStoneId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableGoalTrackerMileStone([FromBody] long id)
        {
            return manifestService.EnableGoalTrackerMileStoneByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableGoalTrackerMileStone([FromBody] long id)
        {
            return manifestService.DisableGoalTrackerMileStoneByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByGoalTrackerMileStone(long id)
        {
            return manifestService.GetXYZZYXsByGoalTrackerMileStoneAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithGoalTrackerMileStone(GoalTrackerMileStoneXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithGoalTrackerMileStoneAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithGoalTrackerMileStone(GoalTrackerMileStoneXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithGoalTrackerMileStoneAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportGoalTrackerMileStone([FromBody] ImportGoalTrackerMileStoneModel model)
        {
            return manifestService.ImportGoalTrackerMileStoneAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportGoalTrackerMileStone()
        {
            var lst = new List<ImportGoalTrackerMileStoneModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportGoalTrackerMileStoneModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleGoalTrackerMileStoneImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<GoalTrackerMileStoneExportModel>> GoalTrackerMileStoneReport([FromQuery] GoalTrackerMileStoneExportParameter parameter)
        {
            return reportService.ExportGoalTrackerMileStoneAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> GoalTrackerMileStoneAsReportCsv([FromQuery] GoalTrackerMileStoneExportParameter parameter)
        {
            var result = await reportService.ExportGoalTrackerMileStoneAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("GoalTrackerMileStoneAsCsv.csv"))
                {
                    FileDownloadName = "GoalTrackerMileStoneExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region MeditationFile
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<MeditationFileModel>> GetMeditationFiles([FromQuery] MeditationFileSearchCriteria search)
        {
            return manifestService.GetMeditationFilesAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<MeditationFileModel>> GetMeditationFile([FromQuery] long id)
        {
            return manifestService.GetMeditationFileByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<MeditationFileModel>> CreateMeditationFile([FromBody] MeditationFileCreateModel meditationFileModel)
        {
            return manifestService.CreateMeditationFileAsync(meditationFileModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<MeditationFileModel>> CreateMultipleMeditationFile([FromBody] MultipleMeditationFileCreateModel meditationFileModels)
        {
            return manifestService.CreateMultipleMeditationFileAsync(meditationFileModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<MeditationFileModel>> UpdateMeditationFile([FromBody] MeditationFileUpdateModel meditationFileModel)
        {
            return manifestService.UpdateMeditationFileAsync(meditationFileModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkMeditationFileAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkMeditationFileAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkMeditationFileAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkMeditationFileAsNotDeletedByIdAsync(id);
        }



        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<MeditationFileModel>> GetMeditationFileByUserId([FromQuery] long userId)
        {
            return manifestService.GetMeditationFileByUserIdAsync(userId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<MeditationFileModel>> GetMeditationFileByUserIdSearch(MeditationFileByUserIdSearchCriteria search)
        {
            return manifestService.GetMeditationFileByUserIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByMeditationFileId(long meditationFileId)
        {
            return manifestService.GetUserByMeditationFileIdAsync(meditationFileId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByMeditationFileIdSearch(UserByMeditationFileIdSearchCriteria search)
        {
            return manifestService.GetUserByMeditationFileIdAsync(search);
        }



        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsMeditationFileFileValid([FromQuery] long meditationFileId)
        {
            return manifestService.IsMeditationFileFileValidAsync(meditationFileId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadMeditationFileFile([FromQuery] long meditationFileId)
        {
            var result = await manifestService.GetMeditationFileFileAsync(meditationFileId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadMeditationFileFile([FromForm] MeditationFileFileWithFileFormModel model)
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

            return productService.UploadMeditationFileFileAsync(model.MeditationFileId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearMeditationFileFile([FromQuery] long movieId)
        {
            return productService.ClearMeditationFileFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsMeditationFileSourceWordFileValid([FromQuery] long meditationFileId)
        {
            return manifestService.IsMeditationFileWordOrPdfFileValidAsync(meditationFileId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadMeditationFileSourceWordFile([FromQuery] long meditationFileId)
        {
            var result = await manifestService.GetMeditationFileWordOrdPdfFileAsync(meditationFileId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableMeditationFile([FromBody] long id)
        {
            return manifestService.EnableMeditationFileByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableMeditationFile([FromBody] long id)
        {
            return manifestService.DisableMeditationFileByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByMeditationFile(long id)
        {
            return manifestService.GetXYZZYXsByMeditationFileAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithMeditationFile(MeditationFileXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithMeditationFileAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithMeditationFile(MeditationFileXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithMeditationFileAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportMeditationFile([FromBody] ImportMeditationFileModel model)
        {
            return manifestService.ImportMeditationFileAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportMeditationFile()
        {
            var lst = new List<ImportMeditationFileModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportMeditationFileModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleMeditationFileImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<MeditationFileExportModel>> MeditationFileReport([FromQuery] MeditationFileExportParameter parameter)
        {
            return reportService.ExportMeditationFileAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> MeditationFileAsReportCsv([FromQuery] MeditationFileExportParameter parameter)
        {
            var result = await reportService.ExportMeditationFileAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("MeditationFileAsCsv.csv"))
                {
                    FileDownloadName = "MeditationFileExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region MeditationSession
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<MeditationSessionModel>> GetMeditationSessions([FromQuery] MeditationSessionSearchCriteria search)
        {
            return manifestService.GetMeditationSessionsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<MeditationSessionModel>> GetMeditationSession([FromQuery] long id)
        {
            return manifestService.GetMeditationSessionByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<MeditationSessionModel>> CreateMeditationSession([FromBody] MeditationSessionCreateModel meditationSessionModel)
        {
            return manifestService.CreateMeditationSessionAsync(meditationSessionModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<MeditationSessionModel>> CreateMultipleMeditationSession([FromBody] MultipleMeditationSessionCreateModel meditationSessionModels)
        {
            return manifestService.CreateMultipleMeditationSessionAsync(meditationSessionModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<MeditationSessionModel>> UpdateMeditationSession([FromBody] MeditationSessionUpdateModel meditationSessionModel)
        {
            return manifestService.UpdateMeditationSessionAsync(meditationSessionModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkMeditationSessionAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkMeditationSessionAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkMeditationSessionAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkMeditationSessionAsNotDeletedByIdAsync(id);
        }



        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<MeditationSessionModel>> GetMeditationSessionByMeditationFileId([FromQuery] long meditationFileId)
        {
            return manifestService.GetMeditationSessionByMeditationFileIdAsync(meditationFileId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<MeditationSessionModel>> GetMeditationSessionByMeditationFileIdSearch(MeditationSessionByMeditationFileIdSearchCriteria search)
        {
            return manifestService.GetMeditationSessionByMeditationFileIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<MeditationFileModel>> GetMeditationFileByMeditationSessionId(long meditationSessionId)
        {
            return manifestService.GetMeditationFileByMeditationSessionIdAsync(meditationSessionId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<MeditationFileModel>> GetMeditationFileByMeditationSessionIdSearch(MeditationFileByMeditationSessionIdSearchCriteria search)
        {
            return manifestService.GetMeditationFileByMeditationSessionIdAsync(search);
        }


        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<MeditationSessionModel>> GetMeditationSessionByUserId([FromQuery] long userId)
        {
            return manifestService.GetMeditationSessionByUserIdAsync(userId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<MeditationSessionModel>> GetMeditationSessionByUserIdSearch(MeditationSessionByUserIdSearchCriteria search)
        {
            return manifestService.GetMeditationSessionByUserIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByMeditationSessionId(long meditationSessionId)
        {
            return manifestService.GetUserByMeditationSessionIdAsync(meditationSessionId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByMeditationSessionIdSearch(UserByMeditationSessionIdSearchCriteria search)
        {
            return manifestService.GetUserByMeditationSessionIdAsync(search);
        }



        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsMeditationSessionFileValid([FromQuery] long meditationSessionId)
        {
            return manifestService.IsMeditationSessionFileValidAsync(meditationSessionId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadMeditationSessionFile([FromQuery] long meditationSessionId)
        {
            var result = await manifestService.GetMeditationSessionFileAsync(meditationSessionId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadMeditationSessionFile([FromForm] MeditationSessionFileWithFileFormModel model)
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

            return productService.UploadMeditationSessionFileAsync(model.MeditationSessionId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearMeditationSessionFile([FromQuery] long movieId)
        {
            return productService.ClearMeditationSessionFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsMeditationSessionSourceWordFileValid([FromQuery] long meditationSessionId)
        {
            return manifestService.IsMeditationSessionWordOrPdfFileValidAsync(meditationSessionId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadMeditationSessionSourceWordFile([FromQuery] long meditationSessionId)
        {
            var result = await manifestService.GetMeditationSessionWordOrdPdfFileAsync(meditationSessionId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableMeditationSession([FromBody] long id)
        {
            return manifestService.EnableMeditationSessionByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableMeditationSession([FromBody] long id)
        {
            return manifestService.DisableMeditationSessionByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByMeditationSession(long id)
        {
            return manifestService.GetXYZZYXsByMeditationSessionAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithMeditationSession(MeditationSessionXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithMeditationSessionAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithMeditationSession(MeditationSessionXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithMeditationSessionAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportMeditationSession([FromBody] ImportMeditationSessionModel model)
        {
            return manifestService.ImportMeditationSessionAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportMeditationSession()
        {
            var lst = new List<ImportMeditationSessionModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportMeditationSessionModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleMeditationSessionImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<MeditationSessionExportModel>> MeditationSessionReport([FromQuery] MeditationSessionExportParameter parameter)
        {
            return reportService.ExportMeditationSessionAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> MeditationSessionAsReportCsv([FromQuery] MeditationSessionExportParameter parameter)
        {
            var result = await reportService.ExportMeditationSessionAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("MeditationSessionAsCsv.csv"))
                {
                    FileDownloadName = "MeditationSessionExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region VisionBoardPicture
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardPictureModel>> GetVisionBoardPictures([FromQuery] VisionBoardPictureSearchCriteria search)
        {
            return manifestService.GetVisionBoardPicturesAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<VisionBoardPictureModel>> GetVisionBoardPicture([FromQuery] long id)
        {
            return manifestService.GetVisionBoardPictureByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<VisionBoardPictureModel>> CreateVisionBoardPicture([FromForm] VisionBoardPictureCreateWithFileFormModel model)
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

            return manifestService.CreateVisionBoardPictureAsync(model, streamModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardPictureModel>> CreateMultipleVisionBoardPicture([FromBody] MultipleVisionBoardPictureCreateModel visionBoardPictureModels)
        {
            return manifestService.CreateMultipleVisionBoardPictureAsync(visionBoardPictureModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<VisionBoardPictureModel>> UpdateVisionBoardPicture([FromForm] VisionBoardPictureUpdateWithFileFormModel model)
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

            return manifestService.UpdateVisionBoardPictureAsync(model, streamModel);
        }
        
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkVisionBoardPictureAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkVisionBoardPictureAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkVisionBoardPictureAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkVisionBoardPictureAsNotDeletedByIdAsync(id);
        }



        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardPictureModel>> GetVisionBoardPictureByUserId([FromQuery] long userId)
        {
            return manifestService.GetVisionBoardPictureByUserIdAsync(userId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardPictureModel>> GetVisionBoardPictureByUserIdSearch(VisionBoardPictureByUserIdSearchCriteria search)
        {
            return manifestService.GetVisionBoardPictureByUserIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByVisionBoardPictureId(long visionBoardPictureId)
        {
            return manifestService.GetUserByVisionBoardPictureIdAsync(visionBoardPictureId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByVisionBoardPictureIdSearch(UserByVisionBoardPictureIdSearchCriteria search)
        {
            return manifestService.GetUserByVisionBoardPictureIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsVisionBoardPictureFileValid([FromQuery] long visionBoardPictureId)
        {
            return manifestService.IsVisionBoardPictureFileValidAsync(visionBoardPictureId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadVisionBoardPictureFile([FromQuery] long visionBoardPictureId)
        {
            var result = await manifestService.GetVisionBoardPictureFileAsync(visionBoardPictureId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }


        /*

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadVisionBoardPictureFile([FromForm] VisionBoardPictureFileWithFileFormModel model)
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

            return productService.UploadVisionBoardPictureFileAsync(model.VisionBoardPictureId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearVisionBoardPictureFile([FromQuery] long movieId)
        {
            return productService.ClearVisionBoardPictureFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsVisionBoardPictureSourceWordFileValid([FromQuery] long visionBoardPictureId)
        {
            return manifestService.IsVisionBoardPictureWordOrPdfFileValidAsync(visionBoardPictureId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadVisionBoardPictureSourceWordFile([FromQuery] long visionBoardPictureId)
        {
            var result = await manifestService.GetVisionBoardPictureWordOrdPdfFileAsync(visionBoardPictureId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableVisionBoardPicture([FromBody] long id)
        {
            return manifestService.EnableVisionBoardPictureByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableVisionBoardPicture([FromBody] long id)
        {
            return manifestService.DisableVisionBoardPictureByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByVisionBoardPicture(long id)
        {
            return manifestService.GetXYZZYXsByVisionBoardPictureAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithVisionBoardPicture(VisionBoardPictureXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithVisionBoardPictureAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithVisionBoardPicture(VisionBoardPictureXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithVisionBoardPictureAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportVisionBoardPicture([FromBody] ImportVisionBoardPictureModel model)
        {
            return manifestService.ImportVisionBoardPictureAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportVisionBoardPicture()
        {
            var lst = new List<ImportVisionBoardPictureModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportVisionBoardPictureModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleVisionBoardPictureImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<VisionBoardPictureExportModel>> VisionBoardPictureReport([FromQuery] VisionBoardPictureExportParameter parameter)
        {
            return reportService.ExportVisionBoardPictureAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> VisionBoardPictureAsReportCsv([FromQuery] VisionBoardPictureExportParameter parameter)
        {
            var result = await reportService.ExportVisionBoardPictureAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("VisionBoardPictureAsCsv.csv"))
                {
                    FileDownloadName = "VisionBoardPictureExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region VisionBoardSession
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardSessionModel>> GetVisionBoardSessions([FromQuery] VisionBoardSessionSearchCriteria search)
        {
            return manifestService.GetVisionBoardSessionsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<VisionBoardSessionModel>> GetVisionBoardSession([FromQuery] long id)
        {
            return manifestService.GetVisionBoardSessionByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<VisionBoardSessionModel>> CreateVisionBoardSession([FromBody] VisionBoardSessionCreateModel visionBoardSessionModel)
        {
            return manifestService.CreateVisionBoardSessionAsync(visionBoardSessionModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardSessionModel>> CreateMultipleVisionBoardSession([FromBody] MultipleVisionBoardSessionCreateModel visionBoardSessionModels)
        {
            return manifestService.CreateMultipleVisionBoardSessionAsync(visionBoardSessionModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<VisionBoardSessionModel>> UpdateVisionBoardSession([FromBody] VisionBoardSessionUpdateModel visionBoardSessionModel)
        {
            return manifestService.UpdateVisionBoardSessionAsync(visionBoardSessionModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkVisionBoardSessionAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkVisionBoardSessionAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkVisionBoardSessionAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkVisionBoardSessionAsNotDeletedByIdAsync(id);
        }



        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardSessionModel>> GetVisionBoardSessionByUserId([FromQuery] long userId)
        {
            return manifestService.GetVisionBoardSessionByUserIdAsync(userId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VisionBoardSessionModel>> GetVisionBoardSessionByUserIdSearch(VisionBoardSessionByUserIdSearchCriteria search)
        {
            return manifestService.GetVisionBoardSessionByUserIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByVisionBoardSessionId(long visionBoardSessionId)
        {
            return manifestService.GetUserByVisionBoardSessionIdAsync(visionBoardSessionId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByVisionBoardSessionIdSearch(UserByVisionBoardSessionIdSearchCriteria search)
        {
            return manifestService.GetUserByVisionBoardSessionIdAsync(search);
        }



        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsVisionBoardSessionFileValid([FromQuery] long visionBoardSessionId)
        {
            return manifestService.IsVisionBoardSessionFileValidAsync(visionBoardSessionId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadVisionBoardSessionFile([FromQuery] long visionBoardSessionId)
        {
            var result = await manifestService.GetVisionBoardSessionFileAsync(visionBoardSessionId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadVisionBoardSessionFile([FromForm] VisionBoardSessionFileWithFileFormModel model)
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

            return productService.UploadVisionBoardSessionFileAsync(model.VisionBoardSessionId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearVisionBoardSessionFile([FromQuery] long movieId)
        {
            return productService.ClearVisionBoardSessionFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsVisionBoardSessionSourceWordFileValid([FromQuery] long visionBoardSessionId)
        {
            return manifestService.IsVisionBoardSessionWordOrPdfFileValidAsync(visionBoardSessionId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadVisionBoardSessionSourceWordFile([FromQuery] long visionBoardSessionId)
        {
            var result = await manifestService.GetVisionBoardSessionWordOrdPdfFileAsync(visionBoardSessionId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableVisionBoardSession([FromBody] long id)
        {
            return manifestService.EnableVisionBoardSessionByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableVisionBoardSession([FromBody] long id)
        {
            return manifestService.DisableVisionBoardSessionByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByVisionBoardSession(long id)
        {
            return manifestService.GetXYZZYXsByVisionBoardSessionAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithVisionBoardSession(VisionBoardSessionXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithVisionBoardSessionAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithVisionBoardSession(VisionBoardSessionXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithVisionBoardSessionAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportVisionBoardSession([FromBody] ImportVisionBoardSessionModel model)
        {
            return manifestService.ImportVisionBoardSessionAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportVisionBoardSession()
        {
            var lst = new List<ImportVisionBoardSessionModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportVisionBoardSessionModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleVisionBoardSessionImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<VisionBoardSessionExportModel>> VisionBoardSessionReport([FromQuery] VisionBoardSessionExportParameter parameter)
        {
            return reportService.ExportVisionBoardSessionAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> VisionBoardSessionAsReportCsv([FromQuery] VisionBoardSessionExportParameter parameter)
        {
            var result = await reportService.ExportVisionBoardSessionAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("VisionBoardSessionAsCsv.csv"))
                {
                    FileDownloadName = "VisionBoardSessionExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region DailyJournal
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyJournalModel>> GetDailyJournals([FromQuery] DailyJournalSearchCriteria search)
        {
            return manifestService.GetDailyJournalsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DailyJournalModel>> GetDailyJournal([FromQuery] long id)
        {
            return manifestService.GetDailyJournalByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DailyJournalModel>> CreateDailyJournal([FromBody] DailyJournalCreateModel dailyJournalModel)
        {
            return manifestService.CreateDailyJournalAsync(dailyJournalModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyJournalModel>> CreateMultipleDailyJournal([FromBody] MultipleDailyJournalCreateModel dailyJournalModels)
        {
            return manifestService.CreateMultipleDailyJournalAsync(dailyJournalModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<DailyJournalModel>> UpdateDailyJournal([FromBody] DailyJournalUpdateModel dailyJournalModel)
        {
            return manifestService.UpdateDailyJournalAsync(dailyJournalModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDailyJournalAsDeleted([FromQuery] long id)
        {
            return manifestService.MarkDailyJournalAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkDailyJournalAsNotDeleted([FromBody] long id)
        {
            return manifestService.MarkDailyJournalAsNotDeletedByIdAsync(id);
        }



        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyJournalModel>> GetDailyJournalByUserId([FromQuery] long userId)
        {
            return manifestService.GetDailyJournalByUserIdAsync(userId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<DailyJournalModel>> GetDailyJournalByUserIdSearch(DailyJournalByUserIdSearchCriteria search)
        {
            return manifestService.GetDailyJournalByUserIdAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByDailyJournalId(long dailyJournalId)
        {
            return manifestService.GetUserByDailyJournalIdAsync(dailyJournalId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserModel>> GetUserByDailyJournalIdSearch(UserByDailyJournalIdSearchCriteria search)
        {
            return manifestService.GetUserByDailyJournalIdAsync(search);
        }



        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDailyJournalFileValid([FromQuery] long dailyJournalId)
        {
            return manifestService.IsDailyJournalFileValidAsync(dailyJournalId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDailyJournalFile([FromQuery] long dailyJournalId)
        {
            var result = await manifestService.GetDailyJournalFileAsync(dailyJournalId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadDailyJournalFile([FromForm] DailyJournalFileWithFileFormModel model)
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

            return productService.UploadDailyJournalFileAsync(model.DailyJournalId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearDailyJournalFile([FromQuery] long movieId)
        {
            return productService.ClearDailyJournalFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsDailyJournalSourceWordFileValid([FromQuery] long dailyJournalId)
        {
            return manifestService.IsDailyJournalWordOrPdfFileValidAsync(dailyJournalId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadDailyJournalSourceWordFile([FromQuery] long dailyJournalId)
        {
            var result = await manifestService.GetDailyJournalWordOrdPdfFileAsync(dailyJournalId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableDailyJournal([FromBody] long id)
        {
            return manifestService.EnableDailyJournalByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableDailyJournal([FromBody] long id)
        {
            return manifestService.DisableDailyJournalByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByDailyJournal(long id)
        {
            return manifestService.GetXYZZYXsByDailyJournalAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithDailyJournal(DailyJournalXYZZYXAssociationModel model)
        {
            return manifestService.AssociateXYZZYXWithDailyJournalAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithDailyJournal(DailyJournalXYZZYXAssociationModel model)
        {
            return manifestService.UnAssociateXYZZYXWithDailyJournalAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportDailyJournal([FromBody] ImportDailyJournalModel model)
        {
            return manifestService.ImportDailyJournalAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportDailyJournal()
        {
            var lst = new List<ImportDailyJournalModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportDailyJournalModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleDailyJournalImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<DailyJournalExportModel>> DailyJournalReport([FromQuery] DailyJournalExportParameter parameter)
        {
            return reportService.ExportDailyJournalAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> DailyJournalAsReportCsv([FromQuery] DailyJournalExportParameter parameter)
        {
            var result = await reportService.ExportDailyJournalAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("DailyJournalAsCsv.csv"))
                {
                    FileDownloadName = "DailyJournalExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

    }
}