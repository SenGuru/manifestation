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

namespace Manifest.Api.All.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public partial class ChatController : BaseController
    {
        private readonly ChatService chatService;
        private readonly ManifestService movieService;
        private readonly DummyDataGenerationService dummyDataGenerationService;
        private readonly CompanyService companyService;
        public ChatController(ChatService chatService, CompanyService companyService, ManifestService movieService, DummyDataGenerationService dummyDataGenerationService, ILog log)
            : base(log)
        {
            this.chatService = chatService;
            this.companyService = companyService;
            this.movieService = movieService;
            this.dummyDataGenerationService = dummyDataGenerationService;
        }

        #region Chat
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<ChatModel>> GetChats([FromQuery] ChatSearchCriteria search)
        {
            return chatService.GetChatsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<ChatModel>> GetChat([FromQuery] long id)
        {
            return chatService.GetChatByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<ChatModel>> CreateChat([FromBody] ChatCreateModel chatModel)
        {
            return chatService.CreateChatAsync(chatModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<ChatModel>> CreateMultipleChat([FromBody] MultipleChatCreateModel chatModels)
        {
            return chatService.CreateMultipleChatAsync(chatModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<ChatModel>> UpdateChat([FromBody] ChatUpdateModel chatModel)
        {
            return chatService.UpdateChatAsync(chatModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkChatAsDeleted([FromQuery] long id)
        {
            return chatService.MarkChatAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkChatAsNotDeleted([FromBody] long id)
        {
            return chatService.MarkChatAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsChatFileValid([FromQuery] long chatId)
        {
            return chatService.IsChatFileValidAsync(chatId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadChatFile([FromQuery] long chatId)
        {
            var result = await chatService.GetChatFileAsync(chatId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadChatFile([FromForm] ChatFileWithFileFormModel model)
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

            return productService.UploadChatFileAsync(model.ChatId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearChatFile([FromQuery] long movieId)
        {
            return productService.ClearChatFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsChatSourceWordFileValid([FromQuery] long chatId)
        {
            return chatService.IsChatWordOrPdfFileValidAsync(chatId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadChatSourceWordFile([FromQuery] long chatId)
        {
            var result = await chatService.GetChatWordOrdPdfFileAsync(chatId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableChat([FromBody] long id)
        {
            return chatService.EnableChatByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableChat([FromBody] long id)
        {
            return chatService.DisableChatByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByChat(long id)
        {
            return chatService.GetXYZZYXsByChatAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithChat(ChatXYZZYXAssociationModel model)
        {
            return chatService.AssociateXYZZYXWithChatAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithChat(ChatXYZZYXAssociationModel model)
        {
            return chatService.UnAssociateXYZZYXWithChatAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportChat([FromBody] ImportChatModel model)
        {
            return chatService.ImportChatAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportChat()
        {
            var lst = new List<ImportChatModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportChatModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleChatImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<ChatExportModel>> ChatReport([FromQuery] ChatExportParameter parameter)
        {
            return reportService.ExportChatAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> ChatAsReportCsv([FromQuery] ChatExportParameter parameter)
        {
            var result = await reportService.ExportChatAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("ChatAsCsv.csv"))
                {
                    FileDownloadName = "ChatExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region ChatMessage
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<ChatMessageModel>> GetChatMessages([FromQuery] ChatMessageSearchCriteria search)
        {
            return chatService.GetChatMessagesAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<ChatMessageModel>> GetChatMessage([FromQuery] long id)
        {
            return chatService.GetChatMessageByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<ChatMessageModel>> CreateChatMessage([FromBody] ChatMessageCreateModel chatMessageModel)
        {
            return chatService.CreateChatMessageAsync(chatMessageModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<ChatMessageModel>> CreateMultipleChatMessage([FromBody] MultipleChatMessageCreateModel chatMessageModels)
        {
            return chatService.CreateMultipleChatMessageAsync(chatMessageModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<ChatMessageModel>> UpdateChatMessage([FromBody] ChatMessageUpdateModel chatMessageModel)
        {
            return chatService.UpdateChatMessageAsync(chatMessageModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkChatMessageAsDeleted([FromQuery] long id)
        {
            return chatService.MarkChatMessageAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkChatMessageAsNotDeleted([FromBody] long id)
        {
            return chatService.MarkChatMessageAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsChatMessageFileValid([FromQuery] long chatMessageId)
        {
            return chatService.IsChatMessageFileValidAsync(chatMessageId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadChatMessageFile([FromQuery] long chatMessageId)
        {
            var result = await chatService.GetChatMessageFileAsync(chatMessageId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadChatMessageFile([FromForm] ChatMessageFileWithFileFormModel model)
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

            return productService.UploadChatMessageFileAsync(model.ChatMessageId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearChatMessageFile([FromQuery] long movieId)
        {
            return productService.ClearChatMessageFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsChatMessageSourceWordFileValid([FromQuery] long chatMessageId)
        {
            return chatService.IsChatMessageWordOrPdfFileValidAsync(chatMessageId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadChatMessageSourceWordFile([FromQuery] long chatMessageId)
        {
            var result = await chatService.GetChatMessageWordOrdPdfFileAsync(chatMessageId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableChatMessage([FromBody] long id)
        {
            return chatService.EnableChatMessageByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableChatMessage([FromBody] long id)
        {
            return chatService.DisableChatMessageByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByChatMessage(long id)
        {
            return chatService.GetXYZZYXsByChatMessageAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithChatMessage(ChatMessageXYZZYXAssociationModel model)
        {
            return chatService.AssociateXYZZYXWithChatMessageAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithChatMessage(ChatMessageXYZZYXAssociationModel model)
        {
            return chatService.UnAssociateXYZZYXWithChatMessageAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportChatMessage([FromBody] ImportChatMessageModel model)
        {
            return chatService.ImportChatMessageAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportChatMessage()
        {
            var lst = new List<ImportChatMessageModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportChatMessageModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleChatMessageImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<ChatMessageExportModel>> ChatMessageReport([FromQuery] ChatMessageExportParameter parameter)
        {
            return reportService.ExportChatMessageAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> ChatMessageAsReportCsv([FromQuery] ChatMessageExportParameter parameter)
        {
            var result = await reportService.ExportChatMessageAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("ChatMessageAsCsv.csv"))
                {
                    FileDownloadName = "ChatMessageExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region ChatMessageStatus
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<ChatMessageStatusModel>> GetChatMessageStatuss([FromQuery] ChatMessageStatusSearchCriteria search)
        {
            return chatService.GetChatMessageStatussAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<ChatMessageStatusModel>> GetChatMessageStatus([FromQuery] long id)
        {
            return chatService.GetChatMessageStatusByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<ChatMessageStatusModel>> CreateChatMessageStatus([FromBody] ChatMessageStatusCreateModel chatMessageStatusModel)
        {
            return chatService.CreateChatMessageStatusAsync(chatMessageStatusModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<ChatMessageStatusModel>> CreateMultipleChatMessageStatus([FromBody] MultipleChatMessageStatusCreateModel chatMessageStatusModels)
        {
            return chatService.CreateMultipleChatMessageStatusAsync(chatMessageStatusModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<ChatMessageStatusModel>> UpdateChatMessageStatus([FromBody] ChatMessageStatusUpdateModel chatMessageStatusModel)
        {
            return chatService.UpdateChatMessageStatusAsync(chatMessageStatusModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkChatMessageStatusAsDeleted([FromQuery] long id)
        {
            return chatService.MarkChatMessageStatusAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkChatMessageStatusAsNotDeleted([FromBody] long id)
        {
            return chatService.MarkChatMessageStatusAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsChatMessageStatusFileValid([FromQuery] long chatMessageStatusId)
        {
            return chatService.IsChatMessageStatusFileValidAsync(chatMessageStatusId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadChatMessageStatusFile([FromQuery] long chatMessageStatusId)
        {
            var result = await chatService.GetChatMessageStatusFileAsync(chatMessageStatusId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadChatMessageStatusFile([FromForm] ChatMessageStatusFileWithFileFormModel model)
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

            return productService.UploadChatMessageStatusFileAsync(model.ChatMessageStatusId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearChatMessageStatusFile([FromQuery] long movieId)
        {
            return productService.ClearChatMessageStatusFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsChatMessageStatusSourceWordFileValid([FromQuery] long chatMessageStatusId)
        {
            return chatService.IsChatMessageStatusWordOrPdfFileValidAsync(chatMessageStatusId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadChatMessageStatusSourceWordFile([FromQuery] long chatMessageStatusId)
        {
            var result = await chatService.GetChatMessageStatusWordOrdPdfFileAsync(chatMessageStatusId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableChatMessageStatus([FromBody] long id)
        {
            return chatService.EnableChatMessageStatusByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableChatMessageStatus([FromBody] long id)
        {
            return chatService.DisableChatMessageStatusByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByChatMessageStatus(long id)
        {
            return chatService.GetXYZZYXsByChatMessageStatusAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithChatMessageStatus(ChatMessageStatusXYZZYXAssociationModel model)
        {
            return chatService.AssociateXYZZYXWithChatMessageStatusAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithChatMessageStatus(ChatMessageStatusXYZZYXAssociationModel model)
        {
            return chatService.UnAssociateXYZZYXWithChatMessageStatusAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportChatMessageStatus([FromBody] ImportChatMessageStatusModel model)
        {
            return chatService.ImportChatMessageStatusAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportChatMessageStatus()
        {
            var lst = new List<ImportChatMessageStatusModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportChatMessageStatusModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleChatMessageStatusImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<ChatMessageStatusExportModel>> ChatMessageStatusReport([FromQuery] ChatMessageStatusExportParameter parameter)
        {
            return reportService.ExportChatMessageStatusAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> ChatMessageStatusAsReportCsv([FromQuery] ChatMessageStatusExportParameter parameter)
        {
            var result = await reportService.ExportChatMessageStatusAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("ChatMessageStatusAsCsv.csv"))
                {
                    FileDownloadName = "ChatMessageStatusExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region ChatUser
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<ChatUserModel>> GetChatUsers([FromQuery] ChatUserSearchCriteria search)
        {
            return chatService.GetChatUsersAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<ChatUserModel>> GetChatUser([FromQuery] long id)
        {
            return chatService.GetChatUserByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<ChatUserModel>> CreateChatUser([FromBody] ChatUserCreateModel chatUserModel)
        {
            return chatService.CreateChatUserAsync(chatUserModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<ChatUserModel>> CreateMultipleChatUser([FromBody] MultipleChatUserCreateModel chatUserModels)
        {
            return chatService.CreateMultipleChatUserAsync(chatUserModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<ChatUserModel>> UpdateChatUser([FromBody] ChatUserUpdateModel chatUserModel)
        {
            return chatService.UpdateChatUserAsync(chatUserModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkChatUserAsDeleted([FromQuery] long id)
        {
            return chatService.MarkChatUserAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkChatUserAsNotDeleted([FromBody] long id)
        {
            return chatService.MarkChatUserAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsChatUserFileValid([FromQuery] long chatUserId)
        {
            return chatService.IsChatUserFileValidAsync(chatUserId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadChatUserFile([FromQuery] long chatUserId)
        {
            var result = await chatService.GetChatUserFileAsync(chatUserId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadChatUserFile([FromForm] ChatUserFileWithFileFormModel model)
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

            return productService.UploadChatUserFileAsync(model.ChatUserId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearChatUserFile([FromQuery] long movieId)
        {
            return productService.ClearChatUserFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsChatUserSourceWordFileValid([FromQuery] long chatUserId)
        {
            return chatService.IsChatUserWordOrPdfFileValidAsync(chatUserId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadChatUserSourceWordFile([FromQuery] long chatUserId)
        {
            var result = await chatService.GetChatUserWordOrdPdfFileAsync(chatUserId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableChatUser([FromBody] long id)
        {
            return chatService.EnableChatUserByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableChatUser([FromBody] long id)
        {
            return chatService.DisableChatUserByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByChatUser(long id)
        {
            return chatService.GetXYZZYXsByChatUserAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithChatUser(ChatUserXYZZYXAssociationModel model)
        {
            return chatService.AssociateXYZZYXWithChatUserAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithChatUser(ChatUserXYZZYXAssociationModel model)
        {
            return chatService.UnAssociateXYZZYXWithChatUserAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportChatUser([FromBody] ImportChatUserModel model)
        {
            return chatService.ImportChatUserAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportChatUser()
        {
            var lst = new List<ImportChatUserModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportChatUserModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleChatUserImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<ChatUserExportModel>> ChatUserReport([FromQuery] ChatUserExportParameter parameter)
        {
            return reportService.ExportChatUserAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> ChatUserAsReportCsv([FromQuery] ChatUserExportParameter parameter)
        {
            var result = await reportService.ExportChatUserAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("ChatUserAsCsv.csv"))
                {
                    FileDownloadName = "ChatUserExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion
    }
}