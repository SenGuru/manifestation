using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.ViewModels;
using Manifest.Repository.Common;
using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.Exceptions;
using Framework.Infrastructure.Interfaces.DbAccess;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;

namespace Manifest.Service.App
{
    public class ChatService : BaseAppService
    {
        private readonly ChatRepository chatRepository;
        private readonly CompanyRepository companyRepository;
        private readonly ITransactionManager transactionManager;
        private readonly AuditService auditService;
        private readonly PermissionService permissionService;
        private readonly ICurrentSessionService currentSessionService;

        public ChatService(
                           ILog log,
                           ChatRepository chatRepository,
                           CompanyRepository companyRepository,
                           ObjectMapperService objectMapperService,
                           AuditService auditService,
                           PermissionService permissionService,
                           ITransactionManager transactionManager,
                           ICurrentSessionService currentSessionService)
            : base(log, objectMapperService)
        {
            this.chatRepository = chatRepository;
            this.companyRepository = companyRepository;
            this.auditService = auditService;
            this.permissionService = permissionService;
            this.transactionManager = transactionManager;
            this.currentSessionService = currentSessionService;
        }

        #region Chat

        public async Task<ReturnListModel<ChatModel>> GetChatsAsync(ChatSearchCriteria search)
        {
            try
            {
                //if (!await permissionService.IsUserSuperAdmin())
                //{
                //    search.CompanyId = await currentSessionService.GetCurrentCompanyIdAsync();
                //}

                return this.ConvertToList(await chatRepository.GetChatsAsync(search),objectMapperService.ChatViewListToChatModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ChatModel>($"Unexpected error when getting Chats", ex);
            }
        }

        public Task<ReturnModel<ChatModel>> GetChatByIdForReadAsync(long chatId)
        {
            return GetChatByIdInternalAsync(forWrite: false, chatId);
        }

        public Task<ReturnModel<ChatModel>> GetChatByIdForWriteAsync(long chatId)
        {
            return GetChatByIdInternalAsync(forWrite: true, chatId);
        }

        public async Task<ReturnModel<ChatModel>> GetChatByIdInternalAsync(bool forWrite, long chatId)
        {
            try
            {
                var item = await chatRepository.GetChatViewByIdAsync(chatId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<ChatModel>($"Unable to find the Chat", new { chatId });
                }

                //var existingMovieModel = forWrite ? await GetMovieByIdForWriteAsync(item.MovieId) : await GetMovieByIdForReadAsync(item.MovieId);
                //if (!existingMovieModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<CompanyModel>(existingMovieModel);
                //}

                //var result = this.ConvertToItem(item,objectMapperService.ChatViewToChatModel);
                //var childItems = await GetChatXYZZYXsAsync(new ChatXYZZYXSearchCriteria { ChatId = item.ChatId, Page = 1, PageSize = int.MaxValue });
                //result.Model.ExistingChatXYZZYXs = childItems.Model.Where(x => x.ChatId == item.ChatId).ToList();
                //return result;

                return this.ConvertToItem(item, objectMapperService.ChatViewToChatModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<ChatModel>($"Unexpected error when getting Chat", new { chatId }, ex);
            }
        }

        public async Task<ReturnListModel<ChatModel>> CreateMultipleChatAsync(MultipleChatCreateModel chats)
        {
            try
            {
                if (chats == null)
                {
                    return log.ErrorAndReturnListModel<ChatModel>($"Unable to Create Chats", new ReturnErrorItem("", "Chats information is not valid"));
                }

                if (chats.NewChats.IsNullOrEmpty())
                {
                    return log.ErrorAndReturnListModel<ChatModel>($"Unable to Create Chats", new ReturnErrorItem("", "Chats information is not provided"));
                }

                var lst = new List<ChatModel>();

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    foreach (var chat in chats.NewChats)
                    {
                        var cResult = await CreateChatAsync(chat);
                        if (!cResult.IsSuccess)
                            throw new RollbackException(cResult);

                        lst.Add(cResult.Model);
                    }

                    await trx.CompleteAsync();
                }

                return ReturnListModel<ChatModel>.Success(lst);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModelWithRollbackExceptionHandler<ChatModel>($"Unexpected error when creating Chats", new { chats }, ex);
            }
        }

        public async Task<ReturnModel<ChatModel>> CreateChatAsync(ChatCreateModel chat, bool isAnonymous = false)
        {
            try
            {
                if (chat == null)
                {
                    return log.ErrorAndReturnModel<ChatModel>($"Unable to create Chat", new ReturnErrorItem("", "Chat information is not valid"));
                }

                //var companyModel = await companyService.GetCompanyByIdForReadAsync(chat.CompanyId);
                //if (!companyModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<ChatModel>(companyModel);
                //}
                //
                //var movieModel = await GetMovieByIdForReadAsync(chat.MovieId);
                //if (!movieModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<ChatModel>(movieModel);
                //}
                //

                /*var ignoreCode = false;
                if (chat.ChatCode.IsNullOrEmpty())
                {
                    ignoreCode = true;
                    chat.ChatCode = StringConstants.PrefixForCode.Chat + Guid.NewGuid().ToString();
                }
                */

                chat.NewChatUsers = chat.NewChatUsers ?? new List<ChatUserCreateModel>();

                long userId = 0;
                if (!isAnonymous)
                {
                    userId = await currentSessionService.GetCurrentUserIdAsync() ?? 0;
                }

                chat.NewChatUsers.Add(new ChatUserCreateModel { UserId = userId });

                var errorList = await IsChatModelValidForCreateAsync(chat);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<ChatModel>($"Unable to create Chat", errorList);
                }

                chat.IsDeleted = false;

                var item = objectMapperService.ChatCreateModelToChat(chat);

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await chatRepository.CreateChatAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create Chat");
                    }

                    chat.ChatId = item.ChatId;
                    await CreateOrUpdateChatDependentsAsync(chat);

                    await trx.CompleteAsync();
                }

                await auditService.ChatCreatedAsync(item.ChatId);

                var view = await chatRepository.GetChatViewByIdAsync(item.ChatId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<ChatModel>($"Unable to find the Chat", new { item.ChatId });
                }

                return this.ConvertToItem(view, objectMapperService.ChatViewToChatModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<ChatModel>($"Unexpected error when creating Chat", new { chat?.ChatId }, ex);
            }
        }

        public async Task<ReturnModel<ChatModel>> UpdateChatAsync(ChatUpdateModel chat)
        {
            try
            {
                if (chat == null)
                {
                    return log.ErrorAndReturnModel<ChatModel>($"Unable to update Chat", new ReturnErrorItem("", "Chat information is not valid"));
                }

                var existingChatModel = await GetChatByIdForWriteAsync(chat.ChatId);
                if (!existingChatModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<ChatModel>(existingChatModel);
                }

                chat.CreatedByUserId = existingChatModel.Model.CreatedByUserId;
                chat.CreatedDate = existingChatModel.Model.CreatedDate;
                chat.IsDeleted = existingChatModel.Model.IsDeleted;
                chat.UserId = existingChatModel.Model.UserId;
                //chat.MovieId = existingChatModel.Model.MovieId;
                //chat.ProfileImageActualFileName = existingChatModel.Model.ProfileImageActualFileName;
                //chat.ProfileImageLocation = existingChatModel.Model.ProfileImageLocation;

                var errorList = await IsChatModelValidForUpdateAsync(chat);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<ChatModel>($"Unable to update Chat", errorList);
                }

                var item = objectMapperService.ChatUpdateModelToChat(chat);

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.UpdateChatAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update Chat");
                    }

                    await CreateOrUpdateChatDependentsAsync(objectMapperService.ChatUpdateModelToChatCreateModel(chat));

                    await trx.CompleteAsync();
                }

                await auditService.ChatUpdatedAsync(chat.ChatId);

                var view = await chatRepository.GetChatViewByIdAsync(chat.ChatId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<ChatModel>($"Unable to find the Chat", new { chat.ChatId });
                }

                return this.ConvertToItem(view,objectMapperService.ChatViewToChatModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<ChatModel>($"Unexpected error when updating Chat", new { chat?.ChatId }, ex);
            }
        }

        public async Task CreateOrUpdateChatDependentsAsync(ChatCreateModel model)
        {
            if (model.NewChatUsers == null)
                model.NewChatUsers = new List<ChatUserCreateModel>();

            if (model.ExistingChatUsers == null)
                model.ExistingChatUsers = new List<ChatUserUpdateModel>();

            var dbExistingChatUser = await chatRepository.GetChatUserViewsByChatIdAsync(model.ChatId);
            if (!dbExistingChatUser.IsNullOrEmpty())
            {
                var dbExistingChatUserIds = dbExistingChatUser.SelectToList(x => x.ChatUserId);
                var modelExistingChatUserIds = model.ExistingChatUsers.SelectToList(x => x.ChatUserId);

                var chatUserItemsToDelete = dbExistingChatUser.Where(x => !modelExistingChatUserIds.Contains(x.ChatUserId));
                foreach (var x in chatUserItemsToDelete)
                {
                    await MarkChatUserAsDeletedByIdAsync(x.ChatUserId);
                }

                var chatUserItemsToUpdate = model.ExistingChatUsers.Where(x => dbExistingChatUserIds.Contains(x.ChatUserId));
                foreach (var x in chatUserItemsToUpdate)
                {
                    x.ChatId = model.ChatId;
                    var result = await UpdateChatUserAsync(x);
                    if (!result.IsSuccess)
                    {
                        throw new RollbackException(result);
                    }
                }
            }

            foreach (var x in model.NewChatUsers)
            {
                x.ChatId = model.ChatId;
                var result = await CreateChatUserAsync(x);
                if (!result.IsSuccess)
                {
                    throw new RollbackException(result);
                }
            }
        }

        public async Task<ReturnModel<bool>> MarkChatAsNotDeletedByIdAsync(long chatId)
        {
            try
            {
                var existingChatModel = await GetChatByIdForWriteAsync(chatId);
                if (!existingChatModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingChatModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.MarkChatAsNotDeletedByIdAsync(chatId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.ChatDeletedAsync(chatId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking Chat as not deleted", new { chatId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkChatAsDeletedByIdAsync(long chatId)
        {
            try
            {
                var existingChatModel = await GetChatByIdForWriteAsync(chatId);
                if (!existingChatModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingChatModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.MarkChatAsDeletedByIdAsync(chatId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.ChatDeletedAsync(chatId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking Chat as deleted", new { chatId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteChatByAsync(long chatId)
        {
            try
            {
                var existingChatModel = await GetChatByIdForWriteAsync(chatId);
                if (!existingChatModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingChatModel);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.HardDeleteChatByIdAsync(chatId);
                    await trx.CompleteAsync();
                }

                await auditService.HardDeleteChatAsync(chatId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting Chat", new { chatId }, ex);
            }
        }

        public Task<List<ReturnErrorItem>> IsChatModelValidForCreateAsync(ChatUpdateModel chatModel)
        {
            return IsChatModelValidForAllAsync(true, chatModel);
        }

        public Task<List<ReturnErrorItem>> IsChatModelValidForUpdateAsync(ChatUpdateModel chatModel)
        {
            return IsChatModelValidForAllAsync(false, chatModel);
        }

        public async Task<List<ReturnErrorItem>> IsChatModelValidForAllAsync(bool isCreate, ChatUpdateModel chatModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (await chatRepository.IsChatNameAlreadyExistsForOtherChatAsync(chatModel.ChatName, chatModel.UserId))
                {
                    result.Add(new ReturnErrorItem("", "Another Chat already exists"));
                }

                if (chatModel.ChatId > 0)
                {
                    result.Add(new ReturnErrorItem("", "Chat Identifier is not valid"));
                }
            }
            else
            {
                if (await chatRepository.IsChatNameAlreadyExistsForOtherChatAsync(chatModel.ChatId, chatModel.ChatName, chatModel.UserId))
                {
                    result.Add(new ReturnErrorItem("", "Another Chat already exists"));
                }

                if (chatModel.ChatId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "Chat Unique Identifier is not valid"));
                }
            }

            if (chatModel.ChatName != null && chatModel.ChatName.Length > ChatModel.COLUMN_SIZE_ChatName)
            {
                result.Add(new ReturnErrorItem("ChatName", $"ChatName size should not exceed {ChatModel.COLUMN_SIZE_ChatName}"));
            }

            if (chatModel.ChatName == null)
            {
                result.Add(new ReturnErrorItem("ChatName", "ChatName is required "));
            }

            return result.Count > 0 ? result : null;
        }

        /*
        public async Task<ReturnModel<bool>> IsChatWordOrPdfFileValidAsync(long chatId, bool isPdf)
        {
            var tName = isPdf ? "preview" : "source";

            try
            {
                var fileModel = await GetChatWordOrPdfFileAsync(chatId,isPdf);
                if (!fileModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the {tName} file", fileModel.ErrorHolder);
                }

                FileUtils.CloseSafely(fileModel.Model.FileData);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the {tName} file", ex);
            }
        }

        public async Task<ReturnModel<FileStreamModel>> GetChatWordOrdPdfFileAsync(long chatId, bool isPdf)
        {
            try
            {
                var view = await GetChatByIdForReadAsync(chatId);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the Chat", new { chatId });
                }

                var fileName = view.Model.SourceStoredFilePath;
                var docType = "Word";
                var docExt = ".docx";

                if (isPdf)
                {
                    fileName = view.Model.PreviewStoredFilePath;
                    docType = "Pdf";
                    docExt = ".pdf";
                }

                if (fileName.IsTrimmedStringNullOrEmpty())
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: {docType} file is not available.");
                }

                if (!await contractFileStorage.ExistsAsync(view.Model.CompanyId, fileName))
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: {docType} file is not available.");
                }

                var result = new FileStreamModel
                {
                    FileName = view.Model.ActualFileName.IsTrimmedStringNullOrEmpty() ? $"ChatFile.{docType}" : view.Model.ActualFileName,
                    ContentType = FileUtils.GetMimeTypeFromFileName(view.Model.ActualFileName),
                    FileData = await wipFileStorage.OpenReadAsync(view.Model.CompanyId, fileName)
                };

                return ReturnModel<FileStreamModel>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for Chat ", new { chatId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> IsChatFileValidAsync(long chatId)
        {
            try
            {
                var fileModel = await GetChatFileAsync(chatId);
                if (!fileModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the Chat file", fileModel.ErrorHolder);
                }

                FileUtils.CloseSafely(fileModel.Model.FileData);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the Chat file", ex);
            }
        }

        public async Task<ReturnModel<FileStreamModel>> GetChatFileAsync(long chatId)
        {
            try
            {
                var view = await GetChatByIdForReadAsync(chatId);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the Chat", new { chatId });
                }

                var fileName = view.Model.SourceStoredFilePath;

                if (fileName.IsTrimmedStringNullOrEmpty())
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: file is not available.");
                }

                if (!await wipFileStorage.ExistsAsync(view.Model.CompanyId, fileName))
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: file is not available.");
                }

                var actualFileName = view.Model.ActualFileName.IsTrimmedStringNullOrEmpty() ? $"ChatFile" : view.Model.ActualFileName;

                var result = new FileStreamModel
                {
                    FileName = actualFileName,
                    ContentType = FileUtils.GetMimeTypeFromFileName(actualFileName),
                    FileData = await wipFileStorage.OpenReadAsync(view.Model.CompanyId, fileName)
                };

                return ReturnModel<FileStreamModel>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for Chat ", new { chatId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> UploadChatFileAsync(long chatId, FileStreamModel streamModel)
        {
            try
            {
                var chat = await GetChatByIdForWriteAsync(chatId);
                if (!chat.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(chat);
                }

                if (streamModel == null)
                {
                    return log.ErrorAndReturnModel<bool>("File Content is not provided");
                }

                var company = await companyRepository.GetCompanyByIdAsync(chat.Model.CompanyId);
                if (company == null)
                {
                    return log.ErrorAndReturnModel<bool>("Unable to find the Company Information");
                }

                try
                {
                    using (var trx = await transactionManager.BeginTransactionAsync())
                    {
                        var path = profileFileFileStorage.ProfileFilePathForHostBrand(chat.Model.CompanyId, chat.Model.ChatId, streamModel.FileName);
                        if (company.PredefinedCompanyTypeId == (int)PredefinedCompanyTypes.MarketPlace)
                            path = profileFileFileStorage.ProfileFilePathForMarketplaceBrand(chat.Model.CompanyId, chat.Model.ChatId, streamModel.FileName);
                        await productRepository.UpdateChatFilePathAsync(chat.Model.ChatId, path, streamModel.FileName, await currentSessionService.GetCurrentUserIdAsync());
                        await profileFileFileStorage.WriteAsync(chat.Model.CompanyId, path, streamModel.FileData);
                        await trx.CompleteAsync();
                    }
                }
                finally
                {
                    FileUtils.CloseSafely(streamModel.FileData);
                }

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when Uploading Chat File", ex);
            }
        }

        public async Task<ReturnModel<bool>> ClearChatFileAsync(long chatId)
        {
            try
            {
                var chat = await GetChatByIdForWriteAsync(chatId);
                if (!chat.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(chat);
                }

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    await productRepository.ClearChatFilePathAsync(chat.Model.ChatId, await currentSessionService.GetCurrentUserIdAsync());
                    await profileFileFileStorage.DeleteAsync(chat.Model.CompanyId, chat.Model.ProfileImageLocation);
                }

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when clearing Chat File", ex);
            }
        }

        public async Task CreateOrUpdateChatDependentsAsync(ChatUpdateModel model)
        {
            if (model.NewChatXYZZYXModels == null)
                model.NewChatXYZZYXModels = new List<ChatXYZZYXCreateModel>();

            if (model.ExistingChatXYZZYXModels == null)
                model.ExistingChatXYZZYXModels = new List<ChatXYZZYXUpdateModel>();

            var dbExistingChats = await chatRepository.GetChatXYZZYXsByChatIdAsync(model.ChatId);
            if (!dbExistingChats.IsNullOrEmpty())
            {
                var dbExistingChatIds = dbExistingChats.SelectToList(x => x.ChatXYZZYXId);
                var modelExistingChatIds = model.ExistingChatXYZZYXModels.SelectToList(x => x.ChatXYZZYXId);

                var xYZZYXItemsToDelete = dbExistingChats.Where(x => !modelExistingChatIds.Contains(x.ChatXYZZYXId));
                foreach (var x in xYZZYXItemsToDelete)
                {
                    var result = await MarkChatXYZZYXAsDeletedByIdAsync(x.ChatXYZZYXId);
                    if (!result.IsSuccess)
                    {
                        throw new RollbackException(result);
                    }
                }

                var xYZZYXItemsToUpdate = model.ExistingChatXYZZYXModels.Where(x => dbExistingChatIds.Contains(x.ChatXYZZYXId));
                foreach (var x in xYZZYXItemsToUpdate)
                {
                    x.ChatId = model.ChatId;
                    var result = await UpdateChatXYZZYXAsync(x);
                    if (!result.IsSuccess)
                    {
                        throw new RollbackException(result);
                    }
                }
            }

            foreach (var x in model.NewChatXYZZYXModels)
            {
                x.ChatId = model.ChatId;
                var result = await CreateChatXYZZYXAsync(x);
                if (!result.IsSuccess)
                {
                    throw new RollbackException(result);
                }
            }
        }

        public async Task<ReturnModel<bool>> EnableChatByIdAsync(long chatId)
        {
            try
            {
                var existingChatModel = await GetChatByIdForWriteAsync(chatId);
                if (!existingChatModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<ChatModel>(existingChatModel);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(existingChatModel.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.EnableChatByIdAsync(existingChatModel.Model.ChatId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.ChatEnabledAsync(chatId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when enabling Chat", new { chatId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> DisableChatByIdAsync(long chatId)
        {
            try
            {
                var existingChatModel = await GetChatByIdForWriteAsync(chatId);
                if (!existingChatModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<ChatModel>(existingChatModel);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(existingChatModel.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.DisableChatByIdAsync(existingChatModel.Model.ChatId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.ChatDisabledAsync(existingChatModel.Model.ChatId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when disabling Chat",new { chatId }, ex);
            }
        }


        public async Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByChatAsync(long chatId)
        {
            try
            {
                var contract = await chatRepository.GetChatByIdAsync(chatId);
                if (contract == null)
                {
                    return log.ErrorAndReturnListModel<ChatModel>($"Unable to find Chat", new { chatId });
                }
                return this.ConvertToList(await chatRepository.GetXYZZYXsByChatsAsync(contract.ChatId), objectMapperService.ChatViewListToChatModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ChatModel>($"Unexpected error when getting Related Chats of the Chat");
            }
        }

        public async Task<ReturnModel<bool>> AssociateXYZZYXWithChatAsync(ChatXYZZYXAssociationModel model)
        {
            try
            {
                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(await currentSessionService.GetCurrentCompanyIdAsync());

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                var errorLst = await IsChatWithChatAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("Chat association is invalid", errorLst);
                }

                if (await chatRepository.DoesChatWithXYZZYXAssociationExistsAsync(model.ChatId, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related Chat is already associated with the Chat", new { model.ChatId, model.XYZZYXId});
                }

                var rValue = await chatRepository.AssociateChatWithXYZZYXAsync(model.ChatId, model.XYZZYXId, await currentSessionService.GetCurrentUserIdAsync());
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }
                return log.ErrorAndReturnModel<bool>("Unable to associate Chat with XYZZYX", new { model.ChatId, model.XYZZYXId});
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when associating Chat with XYZZYX", new { model.ChatId, model.XYZZYXId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> UnAssociateXYZZYXWithChatAsync(ChatXYZZYXAssociationModel model)
        {
            try
            {
                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(await currentSessionService.GetCurrentCompanyIdAsync());

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                var errorLst = await IsChatWithXYZZYXAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("Chat association is not valid", errorLst);
                }

                if (!await chatRepository.DoesChatWithXYZZYXAssociationExistsAsync(model.ChatId, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related Chat is not associated with the Chat", new { model.ChatId, model.XYZZYXId });
                }

                var rValue = await chatRepository.UnAssociateChatWithChatAsync(model.ChatId, model.XYZZYXId);
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }

                return log.ErrorAndReturnModel<bool>("Unable to unassociate Related Chat with the Chat", new { model.ChatId, model.XYZZYXId });
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when unassociating Related Chat with the Chat", new { model.ChatId, model.XYZZYXId }, ex);
            }
        }

        public async Task<List<ReturnErrorItem>> IsChatWithXYZZYXAssociationValidAsync(ChatXYZZYXAssociationModel model)
        {
            var result = new List<ReturnErrorItem>();
            if (model == null)
            {
                result.Add(new ReturnErrorItem("", "Association is not valid"));
                return result;
            }

            if (model.ChatId == 0)
            {
                result.Add(new ReturnErrorItem("", "Chat is not valid"));
                return result;
            }

            if (model.XYZZYXId == 0)
            {
                result.Add(new ReturnErrorItem("", "Related Chat  is not valid"));
                return result;
            }

            var entity = await GetChatForWriteByIdAsync(model.ChatId);
            if(!entity.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "Chat information is not available"));
                return result;
            }

            var xYZZYX = await GetXYZZYXForWriteByIdAsync(model.XYZZYXId);
            if (!xYZZYX.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "Related XYZZYX information is not available"));
                return result;
            }

            if (entity.CompanyId != xYZZYX.CompanyId)
            {
                result.Add(new ReturnErrorItem("", "Chats does not belong to the same company"));
            }

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }

        public async Task<ReturnListModel<ImportStatusModel>> ImportChatAsync(ChatImportModel model, bool isTest)
        {
            try
            {
                if (model == null)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("Chat Import Info is not provided");
                }

                if (model.Chats == null || model.Chats.Count == 0)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("Chats Info is not available to import");
                }

                var resultLst = new List<ImportStatusModel>();

                foreach (var item in model.Chats)
                {
                    var prod = await wipService.GetChatByExternalIdentifierAsync(item.ChatIdentifier);
                    if (prod.Model == null)
                    {
                        //var newProd = new ProductCreateModel();
                        ChatCreateModel chatCreateModel = new ChatCreateModel
                        {
                            IsEnabled = item.IsProductEnabled
                        };

                        var newlyCreatedChat = await CreateChatAsync(chatCreateModel);
                        if (!newlyCreatedChat.IsSuccess)
                        {
                            var errorStr = string.Empty;
                            if (newlyCreatedChat.ErrorHolder.ErrorItemList != null && newlyCreatedProd.ErrorHolder.ErrorItemList.Count > 0)
                            {
                                var errorValueList = newlyCreatedChat.ErrorHolder.ErrorItemList.SelectToList(x => x.Value);
                                errorStr = string.Join(",", errorValueList);
                            }

                            resultLst.Add(new ImportStatusModel() { IsSucess = false, Message = errorStr.IsNotNullOrEmpty() ? errorStr : newlyCreatedProd.ErrorHolder.FriendlyMessage });
                            continue;
                        }
                        else
                        {
                            resultLst.Add(new ImportStatusModel() { IsSucess = true });
                        }
                    }
                    else
                    {
                        ChatUpdateModel chatUpdateModel = new ChatUpdateModel
                        {
                        };

                        //update product
                        var updatedChatModel = await wipService.UpdateChatAsync(chatUpdateModel);
                        if (!updatedChatModel.IsSuccess)
                        {
                            var errorStr = string.Empty;
                            if (updateProdModel.ErrorHolder.ErrorItemList != null && updateProdModel.ErrorHolder.ErrorItemList.Count > 0)
                            {
                                var errorValueList = updateProdModel.ErrorHolder.ErrorItemList.SelectToList(x => x.Value);
                                errorStr = string.Join(",", errorValueList);
                            }

                            resultLst.Add(new ImportStatusModel() { IsSucess = false, Message = errorStr.IsNotNullOrEmpty() ? errorStr : updateProdModel.ErrorHolder.FriendlyMessage });
                            continue;
                        }
                        else
                        {
                            resultLst.Add(new ImportStatusModel() { IsSucess = true });
                        }
                    }
                }

                return ReturnListModel<ImportStatusModel>.Success(resultLst);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ImportStatusModel>("Unexpected error occurred when importing Chat Info", ex);
            }
        }

        public async Task<ReturnListModel<ChatExportModel>> ExportChatAsync(ChatExportParameter filter, bool isTest)
        {
            try
            {
                var chatCount = await chatRepository.GetChatCountAsync(filter);
                if (chatCount == 0)
                {
                    return log.ErrorAndReturnListModel<ChatExportModel>("No Chat available for export.");
                }

                var itemsToReturn = new List<ChatExportModel>();
                var numberOfLoops = (chatCount / pageSize) + 1;
                for (var i = 0; i < numberOfLoops; ++i)
                {
                    var dataResult = await wipService.GetUserViewListAsync(new ChatSearchCriteria { Page = i, PageSize = pageSize, Keyword = filter.Keyword });
                    if (dataResult.IsSuccess)
                    {
                        return log.ErrorAndReturnListModel<ChatExportModel>(dataResult);
                    }

                    itemsToReturn.AddRange(objectMapperService.MapList<ChatModel, ChatExportModel>(dataResult.Model));
                }

                return ReturnListModel<ChatExportModel>.Success(itemsToReturn);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ChatExportModel>("Unexpected error occurred when exporting Chat", ex);
            }
        }

        */
        #endregion

        #region ChatMessage

        public async Task<ReturnListModel<ChatMessageModel>> GetChatMessagesAsync(ChatMessageSearchCriteria search)
        {
            try
            {
                return this.ConvertToList(await chatRepository.GetChatMessagesAsync(search), objectMapperService.ChatMessageViewListToChatMessageModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ChatMessageModel>($"Unexpected error when getting Chat Messages", ex);
            }
        }

        public Task<ReturnModel<ChatMessageModel>> GetChatMessageByIdForReadAsync(long chatMessageId)
        {
            return GetChatMessageByIdInternalAsync(forWrite: false, chatMessageId);
        }

        public Task<ReturnModel<ChatMessageModel>> GetChatMessageByIdForWriteAsync(long chatMessageId)
        {
            return GetChatMessageByIdInternalAsync(forWrite: true, chatMessageId);
        }

        public async Task<ReturnModel<ChatMessageModel>> GetChatMessageByIdInternalAsync(bool forWrite, long chatMessageId)
        {
            try
            {
                var item = await chatRepository.GetChatMessageViewByIdAsync(chatMessageId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<ChatMessageModel>($"Unable to find the Chat Message", new { chatMessageId });
                }

                //var existingMovieModel = forWrite ? await GetMovieByIdForWriteAsync(item.MovieId) : await GetMovieByIdForReadAsync(item.MovieId);
                //if (!existingMovieModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<CompanyModel>(existingMovieModel);
                //}

                //var result = this.ConvertToItem(item,objectMapperService.ChatMessageViewToChatMessageModel);
                //var childItems = await GetChatMessageXYZZYXsAsync(new ChatMessageXYZZYXSearchCriteria { ChatMessageId = item.ChatMessageId, Page = 1, PageSize = int.MaxValue });
                //result.Model.ExistingChatMessageXYZZYXs = childItems.Model.Where(x => x.ChatMessageId == item.ChatMessageId).ToList();
                //return result;

                return this.ConvertToItem(item,objectMapperService.ChatMessageViewToChatMessageModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<ChatMessageModel>($"Unexpected error when getting Chat Message", new { chatMessageId }, ex);
            }
        }

        public async Task<ReturnListModel<ChatMessageModel>> CreateMultipleChatMessageAsync(MultipleChatMessageCreateModel chatMessages)
        {
            try
            {
                if (chatMessages == null)
                {
                    return log.ErrorAndReturnListModel<ChatMessageModel>($"Unable to Create Chat Messages", new ReturnErrorItem("", "Chat Messages information is not valid"));
                }

                if (chatMessages.NewChatMessages.IsNullOrEmpty())
                {
                    return log.ErrorAndReturnListModel<ChatMessageModel>($"Unable to Create Chat Messages", new ReturnErrorItem("", "Chat Messages information is not provided"));
                }

                var lst = new List<ChatMessageModel>();

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    foreach (var chatMessage in chatMessages.NewChatMessages)
                    {
                        var cResult = await CreateChatMessageAsync(chatMessage);
                        if (!cResult.IsSuccess)
                            throw new RollbackException(cResult);

                        lst.Add(cResult.Model);
                    }

                    await trx.CompleteAsync();
                }

                return ReturnListModel<ChatMessageModel>.Success(lst);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModelWithRollbackExceptionHandler<ChatMessageModel>($"Unexpected error when creating Chat Messages", new { chatMessages }, ex);
            }
        }

        public async Task<ReturnModel<ChatMessageModel>> CreateChatMessageAsync(ChatMessageCreateModel chatMessage)
        {
            try
            {
                if (chatMessage == null)
                {
                    return log.ErrorAndReturnModel<ChatMessageModel>($"Unable to create Chat Message", new ReturnErrorItem("", "Chat Message information is not valid"));
                }

                //var companyModel = await companyService.GetCompanyByIdForReadAsync(chatMessage.CompanyId);
                //if (!companyModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<ChatMessageModel>(companyModel);
                //}
                //
                //var movieModel = await GetMovieByIdForReadAsync(chatMessage.MovieId);
                //if (!movieModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<ChatMessageModel>(movieModel);
                //}
                //

                /*var ignoreCode = false;
                if (chatMessage.ChatMessageCode.IsNullOrEmpty())
                {
                    ignoreCode = true;
                    chatMessage.ChatMessageCode = StringConstants.PrefixForCode.ChatMessage + Guid.NewGuid().ToString();
                }
                */

                var errorList = await IsChatMessageModelValidForCreateAsync(chatMessage);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<ChatMessageModel>($"Unable to create Chat Message", errorList);
                }

                chatMessage.IsDeleted = false;

                var item = objectMapperService.ChatMessageCreateModelToChatMessage(chatMessage);

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await chatRepository.CreateChatMessageAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create Chat Message");
                    }

                    //await CreateOrUpdateChatMessageDependentsAsync(chatMessage)

                    /*if (ignoreCode)
                    {
                        if (!await chatRepository.UpdateChatMessageCodeAsync(item.CompanyId, item.ChatMessageId, StringConstants.PrefixForCode.ChatMessage + item.ChatMessageId, await currentSessionService.GetCurrentUserIdAsync()))
                        {
                            throw new RollbackException($"Unable update the Chat Message Code", item);
                        }
                    }*/

                    await trx.CompleteAsync();
                }

                await auditService.ChatMessageCreatedAsync(item.ChatMessageId);

                var view = await chatRepository.GetChatMessageViewByIdAsync(item.ChatMessageId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<ChatMessageModel>($"Unable to find the Chat Message", new { item.ChatMessageId });
                }

                return this.ConvertToItem(view, objectMapperService.ChatMessageViewToChatMessageModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<ChatMessageModel>($"Unexpected error when creating Chat Message", new { chatMessage?.ChatMessageId }, ex);
            }
        }

        public async Task<ReturnModel<ChatMessageModel>> UpdateChatMessageAsync(ChatMessageUpdateModel chatMessage)
        {
            try
            {
                if (chatMessage == null)
                {
                    return log.ErrorAndReturnModel<ChatMessageModel>($"Unable to update Chat Message", new ReturnErrorItem("", "Chat Message information is not valid"));
                }

                var existingChatMessageModel = await GetChatMessageByIdForWriteAsync(chatMessage.ChatMessageId);
                if (!existingChatMessageModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<ChatMessageModel>(existingChatMessageModel);
                }

                chatMessage.CreatedByUserId = existingChatMessageModel.Model.CreatedByUserId;
                chatMessage.CreatedDate = existingChatMessageModel.Model.CreatedDate;
                chatMessage.IsDeleted = existingChatMessageModel.Model.IsDeleted;
                chatMessage.UserId = existingChatMessageModel.Model.UserId;
                //chatMessage.MovieId = existingChatMessageModel.Model.MovieId;
                //chatMessage.ProfileImageActualFileName = existingChatMessageModel.Model.ProfileImageActualFileName;
                //chatMessage.ProfileImageLocation = existingChatMessageModel.Model.ProfileImageLocation;

                var errorList = await IsChatMessageModelValidForUpdateAsync(chatMessage);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<ChatMessageModel>($"Unable to update Chat Message", errorList);
                }

                var item = objectMapperService.ChatMessageUpdateModelToChatMessage(chatMessage);

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.UpdateChatMessageAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update Chat Message");
                    }

                    //await CreateOrUpdateChatMessageDependentsAsync(chatMessage)

                    await trx.CompleteAsync();
                }

                await auditService.ChatMessageUpdatedAsync(chatMessage.ChatMessageId);

                var view = await chatRepository.GetChatMessageViewByIdAsync(chatMessage.ChatMessageId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<ChatMessageModel>($"Unable to find the Chat Message", new { chatMessage.ChatMessageId });
                }

                return this.ConvertToItem(view, objectMapperService.ChatMessageViewToChatMessageModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<ChatMessageModel>($"Unexpected error when updating Chat Message", new { chatMessage?.ChatMessageId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkChatMessageAsNotDeletedByIdAsync(long chatMessageId)
        {
            try
            {
                var existingChatMessageModel = await GetChatMessageByIdForWriteAsync(chatMessageId);
                if (!existingChatMessageModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingChatMessageModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.MarkChatMessageAsNotDeletedByIdAsync(chatMessageId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.ChatMessageDeletedAsync(chatMessageId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking Chat Message as not deleted", new { chatMessageId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkChatMessageAsDeletedByIdAsync(long chatMessageId)
        {
            try
            {
                var existingChatMessageModel = await GetChatMessageByIdForWriteAsync(chatMessageId);
                if (!existingChatMessageModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingChatMessageModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.MarkChatMessageAsDeletedByIdAsync(chatMessageId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.ChatMessageDeletedAsync(chatMessageId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking Chat Message as deleted", new { chatMessageId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteChatMessageByAsync(long chatMessageId)
        {
            try
            {
                var existingChatMessageModel = await GetChatMessageByIdForWriteAsync(chatMessageId);
                if (!existingChatMessageModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingChatMessageModel);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.HardDeleteChatMessageByIdAsync(chatMessageId);
                    await trx.CompleteAsync();
                }

                await auditService.HardDeleteChatMessageAsync(chatMessageId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting Chat Message", new { chatMessageId }, ex);
            }
        }

        public Task<List<ReturnErrorItem>> IsChatMessageModelValidForCreateAsync(ChatMessageUpdateModel chatMessageModel)
        {
            return IsChatMessageModelValidForAllAsync(true, chatMessageModel);
        }

        public Task<List<ReturnErrorItem>> IsChatMessageModelValidForUpdateAsync(ChatMessageUpdateModel chatMessageModel)
        {
            return IsChatMessageModelValidForAllAsync(false, chatMessageModel);
        }

        public async Task<List<ReturnErrorItem>> IsChatMessageModelValidForAllAsync(bool isCreate, ChatMessageUpdateModel chatMessageModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (chatMessageModel.ChatMessageId > 0)
                {
                    result.Add(new ReturnErrorItem("", "Chat Message Identifier is not valid"));
                }
            }
            else
            {
                if (chatMessageModel.ChatMessageId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "Chat Message Unique Identifier is not valid"));
                }
            }

            if (!await companyRepository.DoesUserByIdExistsAsync(chatMessageModel.UserId, chatMessageModel.CompanyId))
            {
                result.Add(new ReturnErrorItem("", $"Unable to Get the User Information"));
                return result;
            }

            if (!ValidationUtils.IsLongValid(chatMessageModel.UserId))
            {
                result.Add(new ReturnErrorItem("UserId", "User cannot be empty"));
            }

            if (!await companyRepository.DoesUserByIdExistsAsync(chatMessageModel.UserId, chatMessageModel.CompanyId))
            {
                result.Add(new ReturnErrorItem("UserId", "User is not available"));
            }

            if (chatMessageModel.ChatMessageText != null && chatMessageModel.ChatMessageText.Length > ChatMessageModel.COLUMN_SIZE_ChatMessageText)
            {
                result.Add(new ReturnErrorItem("ChatMessageText", $"ChatMessageText size should not exceed {ChatMessageModel.COLUMN_SIZE_ChatMessageText}"));
            }

            if (chatMessageModel.ChatMessageText == null)
            {
                result.Add(new ReturnErrorItem("ChatMessageText", "ChatMessageText is required "));
            }

            var chatUsers = await chatRepository.GetChatUsersByChatIdAsync(chatMessageModel.ChatId);
            if (chatUsers == null)
            {
                result.Add(new ReturnErrorItem("ChatId", "There are no users in this chat"));
            }
            else if (!chatUsers.Exists(x => x.UserId == chatMessageModel.UserId))
            {
                result.Add(new ReturnErrorItem("ChatUser", "Your not part of this chat"));
            }

            return result.Count > 0 ? result : null;
        }

        /*
        public async Task<ReturnModel<bool>> IsChatMessageWordOrPdfFileValidAsync(long chatMessageId, bool isPdf)
        {
            var tName = isPdf ? "preview" : "source";

            try
            {
                var fileModel = await GetChatMessageWordOrPdfFileAsync(chatMessageId,isPdf);
                if (!fileModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the {tName} file", fileModel.ErrorHolder);
                }

                FileUtils.CloseSafely(fileModel.Model.FileData);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the {tName} file", ex);
            }
        }

        public async Task<ReturnModel<FileStreamModel>> GetChatMessageWordOrdPdfFileAsync(long chatMessageId, bool isPdf)
        {
            try
            {
                var view = await GetChatMessageByIdForReadAsync(chatMessageId);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the Chat Message", new { chatMessageId });
                }

                var fileName = view.Model.SourceStoredFilePath;
                var docType = "Word";
                var docExt = ".docx";

                if (isPdf)
                {
                    fileName = view.Model.PreviewStoredFilePath;
                    docType = "Pdf";
                    docExt = ".pdf";
                }

                if (fileName.IsTrimmedStringNullOrEmpty())
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: {docType} file is not available.");
                }

                if (!await contractFileStorage.ExistsAsync(view.Model.CompanyId, fileName))
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: {docType} file is not available.");
                }

                var result = new FileStreamModel
                {
                    FileName = view.Model.ActualFileName.IsTrimmedStringNullOrEmpty() ? $"ChatMessageFile.{docType}" : view.Model.ActualFileName,
                    ContentType = FileUtils.GetMimeTypeFromFileName(view.Model.ActualFileName),
                    FileData = await wipFileStorage.OpenReadAsync(view.Model.CompanyId, fileName)
                };

                return ReturnModel<FileStreamModel>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for Chat Message ", new { chatMessageId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> IsChatMessageFileValidAsync(long chatMessageId)
        {
            try
            {
                var fileModel = await GetChatMessageFileAsync(chatMessageId);
                if (!fileModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the Chat Message file", fileModel.ErrorHolder);
                }

                FileUtils.CloseSafely(fileModel.Model.FileData);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the Chat Message file", ex);
            }
        }

        public async Task<ReturnModel<FileStreamModel>> GetChatMessageFileAsync(long chatMessageId)
        {
            try
            {
                var view = await GetChatMessageByIdForReadAsync(chatMessageId);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the Chat Message", new { chatMessageId });
                }

                var fileName = view.Model.SourceStoredFilePath;

                if (fileName.IsTrimmedStringNullOrEmpty())
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: file is not available.");
                }

                if (!await wipFileStorage.ExistsAsync(view.Model.CompanyId, fileName))
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: file is not available.");
                }

                var actualFileName = view.Model.ActualFileName.IsTrimmedStringNullOrEmpty() ? $"ChatMessageFile" : view.Model.ActualFileName;

                var result = new FileStreamModel
                {
                    FileName = actualFileName,
                    ContentType = FileUtils.GetMimeTypeFromFileName(actualFileName),
                    FileData = await wipFileStorage.OpenReadAsync(view.Model.CompanyId, fileName)
                };

                return ReturnModel<FileStreamModel>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for Chat Message ", new { chatMessageId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> UploadChatMessageFileAsync(long chatMessageId, FileStreamModel streamModel)
        {
            try
            {
                var chatMessage = await GetChatMessageByIdForWriteAsync(chatMessageId);
                if (!chatMessage.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(chatMessage);
                }

                if (streamModel == null)
                {
                    return log.ErrorAndReturnModel<bool>("File Content is not provided");
                }

                var company = await companyRepository.GetCompanyByIdAsync(chatMessage.Model.CompanyId);
                if (company == null)
                {
                    return log.ErrorAndReturnModel<bool>("Unable to find the Company Information");
                }

                try
                {
                    using (var trx = await transactionManager.BeginTransactionAsync())
                    {
                        var path = profileFileFileStorage.ProfileFilePathForHostBrand(chatMessage.Model.CompanyId, chatMessage.Model.ChatMessageId, streamModel.FileName);
                        if (company.PredefinedCompanyTypeId == (int)PredefinedCompanyTypes.MarketPlace)
                            path = profileFileFileStorage.ProfileFilePathForMarketplaceBrand(chatMessage.Model.CompanyId, chatMessage.Model.ChatMessageId, streamModel.FileName);
                        await productRepository.UpdateChatMessageFilePathAsync(chatMessage.Model.ChatMessageId, path, streamModel.FileName, await currentSessionService.GetCurrentUserIdAsync());
                        await profileFileFileStorage.WriteAsync(chatMessage.Model.CompanyId, path, streamModel.FileData);
                        await trx.CompleteAsync();
                    }
                }
                finally
                {
                    FileUtils.CloseSafely(streamModel.FileData);
                }

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when Uploading Chat Message File", ex);
            }
        }

        public async Task<ReturnModel<bool>> ClearChatMessageFileAsync(long chatMessageId)
        {
            try
            {
                var chatMessage = await GetChatMessageByIdForWriteAsync(chatMessageId);
                if (!chatMessage.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(chatMessage);
                }

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    await productRepository.ClearChatMessageFilePathAsync(chatMessage.Model.ChatMessageId, await currentSessionService.GetCurrentUserIdAsync());
                    await profileFileFileStorage.DeleteAsync(chatMessage.Model.CompanyId, chatMessage.Model.ProfileImageLocation);
                }

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when clearing Chat Message File", ex);
            }
        }

        public async Task CreateOrUpdateChatMessageDependentsAsync(ChatMessageUpdateModel model)
        {
            if (model.NewChatMessageXYZZYXModels == null)
                model.NewChatMessageXYZZYXModels = new List<ChatMessageXYZZYXCreateModel>();

            if (model.ExistingChatMessageXYZZYXModels == null)
                model.ExistingChatMessageXYZZYXModels = new List<ChatMessageXYZZYXUpdateModel>();

            var dbExistingChatMessages = await chatRepository.GetChatMessageXYZZYXsByChatMessageIdAsync(model.ChatMessageId);
            if (!dbExistingChatMessages.IsNullOrEmpty())
            {
                var dbExistingChatMessageIds = dbExistingChatMessages.SelectToList(x => x.ChatMessageXYZZYXId);
                var modelExistingChatMessageIds = model.ExistingChatMessageXYZZYXModels.SelectToList(x => x.ChatMessageXYZZYXId);

                var xYZZYXItemsToDelete = dbExistingChatMessages.Where(x => !modelExistingChatMessageIds.Contains(x.ChatMessageXYZZYXId));
                foreach (var x in xYZZYXItemsToDelete)
                {
                    var result = await MarkChatMessageXYZZYXAsDeletedByIdAsync(x.ChatMessageXYZZYXId);
                    if (!result.IsSuccess)
                    {
                        throw new RollbackException(result);
                    }
                }

                var xYZZYXItemsToUpdate = model.ExistingChatMessageXYZZYXModels.Where(x => dbExistingChatMessageIds.Contains(x.ChatMessageXYZZYXId));
                foreach (var x in xYZZYXItemsToUpdate)
                {
                    x.ChatMessageId = model.ChatMessageId;
                    var result = await UpdateChatMessageXYZZYXAsync(x);
                    if (!result.IsSuccess)
                    {
                        throw new RollbackException(result);
                    }
                }
            }

            foreach (var x in model.NewChatMessageXYZZYXModels)
            {
                x.ChatMessageId = model.ChatMessageId;
                var result = await CreateChatMessageXYZZYXAsync(x);
                if (!result.IsSuccess)
                {
                    throw new RollbackException(result);
                }
            }
        }

        public async Task<ReturnModel<bool>> EnableChatMessageByIdAsync(long chatMessageId)
        {
            try
            {
                var existingChatMessageModel = await GetChatMessageByIdForWriteAsync(chatMessageId);
                if (!existingChatMessageModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<ChatMessageModel>(existingChatMessageModel);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(existingChatMessageModel.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.EnableChatMessageByIdAsync(existingChatMessageModel.Model.ChatMessageId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.ChatMessageEnabledAsync(chatMessageId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when enabling Chat Message", new { chatMessageId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> DisableChatMessageByIdAsync(long chatMessageId)
        {
            try
            {
                var existingChatMessageModel = await GetChatMessageByIdForWriteAsync(chatMessageId);
                if (!existingChatMessageModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<ChatMessageModel>(existingChatMessageModel);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(existingChatMessageModel.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.DisableChatMessageByIdAsync(existingChatMessageModel.Model.ChatMessageId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.ChatMessageDisabledAsync(existingChatMessageModel.Model.ChatMessageId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when disabling Chat Message",new { chatMessageId }, ex);
            }
        }


        public async Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByChatMessageAsync(long chatMessageId)
        {
            try
            {
                var contract = await chatRepository.GetChatMessageByIdAsync(chatMessageId);
                if (contract == null)
                {
                    return log.ErrorAndReturnListModel<ChatMessageModel>($"Unable to find Chat Message", new { chatMessageId });
                }
                return this.ConvertToList(await chatRepository.GetXYZZYXsByChatMessagesAsync(contract.ChatMessageId), objectMapperService.ChatMessageViewListToChatMessageModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ChatMessageModel>($"Unexpected error when getting Related Chat Messages of the Chat Message");
            }
        }

        public async Task<ReturnModel<bool>> AssociateXYZZYXWithChatMessageAsync(ChatMessageXYZZYXAssociationModel model)
        {
            try
            {
                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(await currentSessionService.GetCurrentCompanyIdAsync());

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                var errorLst = await IsChatMessageWithChatMessageAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("Chat Message association is invalid", errorLst);
                }

                if (await chatRepository.DoesChatMessageWithXYZZYXAssociationExistsAsync(model.ChatMessageId, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related Chat Message is already associated with the Chat Message", new { model.ChatMessageId, model.XYZZYXId});
                }

                var rValue = await chatRepository.AssociateChatMessageWithXYZZYXAsync(model.ChatMessageId, model.XYZZYXId, await currentSessionService.GetCurrentUserIdAsync());
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }
                return log.ErrorAndReturnModel<bool>("Unable to associate Chat Message with XYZZYX", new { model.ChatMessageId, model.XYZZYXId});
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when associating Chat Message with XYZZYX", new { model.ChatMessageId, model.XYZZYXId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> UnAssociateXYZZYXWithChatMessageAsync(ChatMessageXYZZYXAssociationModel model)
        {
            try
            {
                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(await currentSessionService.GetCurrentCompanyIdAsync());

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                var errorLst = await IsChatMessageWithXYZZYXAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("Chat Message association is not valid", errorLst);
                }

                if (!await chatRepository.DoesChatMessageWithXYZZYXAssociationExistsAsync(model.ChatMessageId, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related Chat Message is not associated with the Chat Message", new { model.ChatMessageId, model.XYZZYXId });
                }

                var rValue = await chatRepository.UnAssociateChatMessageWithChatMessageAsync(model.ChatMessageId, model.XYZZYXId);
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }

                return log.ErrorAndReturnModel<bool>("Unable to unassociate Related Chat Message with the Chat Message", new { model.ChatMessageId, model.XYZZYXId });
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when unassociating Related Chat Message with the Chat Message", new { model.ChatMessageId, model.XYZZYXId }, ex);
            }
        }

        public async Task<List<ReturnErrorItem>> IsChatMessageWithXYZZYXAssociationValidAsync(ChatMessageXYZZYXAssociationModel model)
        {
            var result = new List<ReturnErrorItem>();
            if (model == null)
            {
                result.Add(new ReturnErrorItem("", "Association is not valid"));
                return result;
            }

            if (model.ChatMessageId == 0)
            {
                result.Add(new ReturnErrorItem("", "Chat Message is not valid"));
                return result;
            }

            if (model.XYZZYXId == 0)
            {
                result.Add(new ReturnErrorItem("", "Related Chat Message  is not valid"));
                return result;
            }

            var entity = await GetChatMessageForWriteByIdAsync(model.ChatMessageId);
            if(!entity.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "Chat Message information is not available"));
                return result;
            }

            var xYZZYX = await GetXYZZYXForWriteByIdAsync(model.XYZZYXId);
            if (!xYZZYX.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "Related XYZZYX information is not available"));
                return result;
            }

            if (entity.CompanyId != xYZZYX.CompanyId)
            {
                result.Add(new ReturnErrorItem("", "Chat Messages does not belong to the same company"));
            }

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }

        public async Task<ReturnListModel<ImportStatusModel>> ImportChatMessageAsync(ChatMessageImportModel model, bool isTest)
        {
            try
            {
                if (model == null)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("Chat Message Import Info is not provided");
                }

                if (model.ChatMessages == null || model.ChatMessages.Count == 0)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("Chat Messages Info is not available to import");
                }

                var resultLst = new List<ImportStatusModel>();

                foreach (var item in model.ChatMessages)
                {
                    var prod = await wipService.GetChatMessageByExternalIdentifierAsync(item.ChatMessageIdentifier);
                    if (prod.Model == null)
                    {
                        //var newProd = new ProductCreateModel();
                        ChatMessageCreateModel chatMessageCreateModel = new ChatMessageCreateModel
                        {
                            IsEnabled = item.IsProductEnabled
                        };

                        var newlyCreatedChatMessage = await CreateChatMessageAsync(chatMessageCreateModel);
                        if (!newlyCreatedChatMessage.IsSuccess)
                        {
                            var errorStr = string.Empty;
                            if (newlyCreatedChatMessage.ErrorHolder.ErrorItemList != null && newlyCreatedProd.ErrorHolder.ErrorItemList.Count > 0)
                            {
                                var errorValueList = newlyCreatedChatMessage.ErrorHolder.ErrorItemList.SelectToList(x => x.Value);
                                errorStr = string.Join(",", errorValueList);
                            }

                            resultLst.Add(new ImportStatusModel() { IsSucess = false, Message = errorStr.IsNotNullOrEmpty() ? errorStr : newlyCreatedProd.ErrorHolder.FriendlyMessage });
                            continue;
                        }
                        else
                        {
                            resultLst.Add(new ImportStatusModel() { IsSucess = true });
                        }
                    }
                    else
                    {
                        ChatMessageUpdateModel chatMessageUpdateModel = new ChatMessageUpdateModel
                        {
                        };

                        //update product
                        var updatedChatMessageModel = await wipService.UpdateChatMessageAsync(chatMessageUpdateModel);
                        if (!updatedChatMessageModel.IsSuccess)
                        {
                            var errorStr = string.Empty;
                            if (updateProdModel.ErrorHolder.ErrorItemList != null && updateProdModel.ErrorHolder.ErrorItemList.Count > 0)
                            {
                                var errorValueList = updateProdModel.ErrorHolder.ErrorItemList.SelectToList(x => x.Value);
                                errorStr = string.Join(",", errorValueList);
                            }

                            resultLst.Add(new ImportStatusModel() { IsSucess = false, Message = errorStr.IsNotNullOrEmpty() ? errorStr : updateProdModel.ErrorHolder.FriendlyMessage });
                            continue;
                        }
                        else
                        {
                            resultLst.Add(new ImportStatusModel() { IsSucess = true });
                        }
                    }
                }

                return ReturnListModel<ImportStatusModel>.Success(resultLst);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ImportStatusModel>("Unexpected error occurred when importing Chat Message Info", ex);
            }
        }

        public async Task<ReturnListModel<ChatMessageExportModel>> ExportChatMessageAsync(ChatMessageExportParameter filter, bool isTest)
        {
            try
            {
                var chatMessageCount = await chatRepository.GetChatMessageCountAsync(filter);
                if (chatMessageCount == 0)
                {
                    return log.ErrorAndReturnListModel<ChatMessageExportModel>("No Chat Message available for export.");
                }

                var itemsToReturn = new List<ChatMessageExportModel>();
                var numberOfLoops = (chatMessageCount / pageSize) + 1;
                for (var i = 0; i < numberOfLoops; ++i)
                {
                    var dataResult = await wipService.GetUserViewListAsync(new ChatMessageSearchCriteria { Page = i, PageSize = pageSize, Keyword = filter.Keyword });
                    if (dataResult.IsSuccess)
                    {
                        return log.ErrorAndReturnListModel<ChatMessageExportModel>(dataResult);
                    }

                    itemsToReturn.AddRange(objectMapperService.MapList<ChatMessageModel, ChatMessageExportModel>(dataResult.Model));
                }

                return ReturnListModel<ChatMessageExportModel>.Success(itemsToReturn);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ChatMessageExportModel>("Unexpected error occurred when exporting Chat Message", ex);
            }
        }

        */
        #endregion

        #region ChatMessageStatus

        public async Task<ReturnListModel<ChatMessageStatusModel>> GetChatMessageStatussAsync(ChatMessageStatusSearchCriteria search)
        {
            try
            {
                return this.ConvertToList(await chatRepository.GetChatMessageStatussAsync(search), objectMapperService.ChatMessageStatusViewListToChatMessageStatusModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ChatMessageStatusModel>($"Unexpected error when getting Chat Message Statuss", ex);
            }
        }

        public Task<ReturnModel<ChatMessageStatusModel>> GetChatMessageStatusByIdForReadAsync(long chatMessageStatusId)
        {
            return GetChatMessageStatusByIdInternalAsync(forWrite: false, chatMessageStatusId);
        }

        public Task<ReturnModel<ChatMessageStatusModel>> GetChatMessageStatusByIdForWriteAsync(long chatMessageStatusId)
        {
            return GetChatMessageStatusByIdInternalAsync(forWrite: true, chatMessageStatusId);
        }

        public async Task<ReturnModel<ChatMessageStatusModel>> GetChatMessageStatusByIdInternalAsync(bool forWrite, long chatMessageStatusId)
        {
            try
            {
                var item = await chatRepository.GetChatMessageStatusViewByIdAsync(chatMessageStatusId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<ChatMessageStatusModel>($"Unable to find the Chat Message Status", new { chatMessageStatusId });
                }

                //var existingMovieModel = forWrite ? await GetMovieByIdForWriteAsync(item.MovieId) : await GetMovieByIdForReadAsync(item.MovieId);
                //if (!existingMovieModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<CompanyModel>(existingMovieModel);
                //}

                //var result = this.ConvertToItem(item,objectMapperService.ChatMessageStatusViewToChatMessageStatusModel);
                //var childItems = await GetChatMessageStatusXYZZYXsAsync(new ChatMessageStatusXYZZYXSearchCriteria { ChatMessageStatusId = item.ChatMessageStatusId, Page = 1, PageSize = int.MaxValue });
                //result.Model.ExistingChatMessageStatusXYZZYXs = childItems.Model.Where(x => x.ChatMessageStatusId == item.ChatMessageStatusId).ToList();
                //return result;

                return this.ConvertToItem(item, objectMapperService.ChatMessageStatusViewToChatMessageStatusModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<ChatMessageStatusModel>($"Unexpected error when getting Chat Message Status", new { chatMessageStatusId }, ex);
            }
        }

        public async Task<ReturnListModel<ChatMessageStatusModel>> CreateMultipleChatMessageStatusAsync(MultipleChatMessageStatusCreateModel chatMessageStatuss)
        {
            try
            {
                if (chatMessageStatuss == null)
                {
                    return log.ErrorAndReturnListModel<ChatMessageStatusModel>($"Unable to Create Chat Message Statuss", new ReturnErrorItem("", "Chat Message Statuss information is not valid"));
                }

                if (chatMessageStatuss.NewChatMessageStatuss.IsNullOrEmpty())
                {
                    return log.ErrorAndReturnListModel<ChatMessageStatusModel>($"Unable to Create Chat Message Statuss", new ReturnErrorItem("", "Chat Message Statuss information is not provided"));
                }

                var lst = new List<ChatMessageStatusModel>();

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    foreach (var chatMessageStatus in chatMessageStatuss.NewChatMessageStatuss)
                    {
                        var cResult = await CreateChatMessageStatusAsync(chatMessageStatus);
                        if (!cResult.IsSuccess)
                            throw new RollbackException(cResult);

                        lst.Add(cResult.Model);
                    }

                    await trx.CompleteAsync();
                }

                return ReturnListModel<ChatMessageStatusModel>.Success(lst);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModelWithRollbackExceptionHandler<ChatMessageStatusModel>($"Unexpected error when creating Chat Message Statuss", new { chatMessageStatuss }, ex);
            }
        }

        public async Task<ReturnModel<ChatMessageStatusModel>> CreateChatMessageStatusAsync(ChatMessageStatusCreateModel chatMessageStatus)
        {
            try
            {
                if (chatMessageStatus == null)
                {
                    return log.ErrorAndReturnModel<ChatMessageStatusModel>($"Unable to create Chat Message Status", new ReturnErrorItem("", "Chat Message Status information is not valid"));
                }

                //var companyModel = await companyService.GetCompanyByIdForReadAsync(chatMessageStatus.CompanyId);
                //if (!companyModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<ChatMessageStatusModel>(companyModel);
                //}
                //
                //var movieModel = await GetMovieByIdForReadAsync(chatMessageStatus.MovieId);
                //if (!movieModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<ChatMessageStatusModel>(movieModel);
                //}
                //

                /*var ignoreCode = false;
                if (chatMessageStatus.ChatMessageStatusCode.IsNullOrEmpty())
                {
                    ignoreCode = true;
                    chatMessageStatus.ChatMessageStatusCode = StringConstants.PrefixForCode.ChatMessageStatus + Guid.NewGuid().ToString();
                }
                */

                var errorList = await IsChatMessageStatusModelValidForCreateAsync(chatMessageStatus);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<ChatMessageStatusModel>($"Unable to create Chat Message Status", errorList);
                }

                chatMessageStatus.IsDeleted = false;

                var item = objectMapperService.ChatMessageStatusCreateModelToChatMessageStatus(chatMessageStatus);

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await chatRepository.CreateChatMessageStatusAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create Chat Message Status");
                    }

                    //await CreateOrUpdateChatMessageStatusDependentsAsync(chatMessageStatus)

                    /*if (ignoreCode)
                    {
                        if (!await chatRepository.UpdateChatMessageStatusCodeAsync(item.CompanyId, item.ChatMessageStatusId, StringConstants.PrefixForCode.ChatMessageStatus + item.ChatMessageStatusId, await currentSessionService.GetCurrentUserIdAsync()))
                        {
                            throw new RollbackException($"Unable update the Chat Message Status Code", item);
                        }
                    }*/

                    await trx.CompleteAsync();
                }

                await auditService.ChatMessageStatusCreatedAsync(item.ChatMessageStatusId);

                var view = await chatRepository.GetChatMessageStatusViewByIdAsync(item.ChatMessageStatusId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<ChatMessageStatusModel>($"Unable to find the Chat Message Status", new { item.ChatMessageStatusId });
                }

                return this.ConvertToItem(view, objectMapperService.ChatMessageStatusViewToChatMessageStatusModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<ChatMessageStatusModel>($"Unexpected error when creating Chat Message Status", new { chatMessageStatus?.ChatMessageStatusId }, ex);
            }
        }

        public async Task<ReturnModel<ChatMessageStatusModel>> UpdateChatMessageStatusAsync(ChatMessageStatusUpdateModel chatMessageStatus)
        {
            try
            {
                if (chatMessageStatus == null)
                {
                    return log.ErrorAndReturnModel<ChatMessageStatusModel>($"Unable to update Chat Message Status", new ReturnErrorItem("", "Chat Message Status information is not valid"));
                }

                var existingChatMessageStatusModel = await GetChatMessageStatusByIdForWriteAsync(chatMessageStatus.ChatMessageStatusId);
                if (!existingChatMessageStatusModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<ChatMessageStatusModel>(existingChatMessageStatusModel);
                }

                chatMessageStatus.CreatedByUserId = existingChatMessageStatusModel.Model.CreatedByUserId;
                chatMessageStatus.CreatedDate = existingChatMessageStatusModel.Model.CreatedDate;
                chatMessageStatus.IsDeleted = existingChatMessageStatusModel.Model.IsDeleted;
                //chatMessageStatus.MovieId = existingChatMessageStatusModel.Model.MovieId;
                //chatMessageStatus.ProfileImageActualFileName = existingChatMessageStatusModel.Model.ProfileImageActualFileName;
                //chatMessageStatus.ProfileImageLocation = existingChatMessageStatusModel.Model.ProfileImageLocation;

                var errorList = await IsChatMessageStatusModelValidForUpdateAsync(chatMessageStatus);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<ChatMessageStatusModel>($"Unable to update Chat Message Status", errorList);
                }

                var item = objectMapperService.ChatMessageStatusUpdateModelToChatMessageStatus(chatMessageStatus);

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.UpdateChatMessageStatusAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update Chat Message Status");
                    }

                    //await CreateOrUpdateChatMessageStatusDependentsAsync(chatMessageStatus)

                    await trx.CompleteAsync();
                }

                await auditService.ChatMessageStatusUpdatedAsync(chatMessageStatus.ChatMessageStatusId);

                var view = await chatRepository.GetChatMessageStatusViewByIdAsync(chatMessageStatus.ChatMessageStatusId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<ChatMessageStatusModel>($"Unable to find the Chat Message Status", new { chatMessageStatus.ChatMessageStatusId });
                }

                return this.ConvertToItem(view, objectMapperService.ChatMessageStatusViewToChatMessageStatusModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<ChatMessageStatusModel>($"Unexpected error when updating Chat Message Status", new { chatMessageStatus?.ChatMessageStatusId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkChatMessageStatusAsNotDeletedByIdAsync(long chatMessageStatusId)
        {
            try
            {
                var existingChatMessageStatusModel = await GetChatMessageStatusByIdForWriteAsync(chatMessageStatusId);
                if (!existingChatMessageStatusModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingChatMessageStatusModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.MarkChatMessageStatusAsNotDeletedByIdAsync(chatMessageStatusId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.ChatMessageStatusDeletedAsync(chatMessageStatusId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking Chat Message Status as not deleted", new { chatMessageStatusId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkChatMessageStatusAsDeletedByIdAsync(long chatMessageStatusId)
        {
            try
            {
                var existingChatMessageStatusModel = await GetChatMessageStatusByIdForWriteAsync(chatMessageStatusId);
                if (!existingChatMessageStatusModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingChatMessageStatusModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.MarkChatMessageStatusAsDeletedByIdAsync(chatMessageStatusId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.ChatMessageStatusDeletedAsync(chatMessageStatusId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking Chat Message Status as deleted", new { chatMessageStatusId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteChatMessageStatusByAsync(long chatMessageStatusId)
        {
            try
            {
                var existingChatMessageStatusModel = await GetChatMessageStatusByIdForWriteAsync(chatMessageStatusId);
                if (!existingChatMessageStatusModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingChatMessageStatusModel);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.HardDeleteChatMessageStatusByIdAsync(chatMessageStatusId);
                    await trx.CompleteAsync();
                }

                await auditService.HardDeleteChatMessageStatusAsync(chatMessageStatusId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting Chat Message Status", new { chatMessageStatusId }, ex);
            }
        }

        public Task<List<ReturnErrorItem>> IsChatMessageStatusModelValidForCreateAsync(ChatMessageStatusUpdateModel chatMessageStatusModel)
        {
            return IsChatMessageStatusModelValidForAllAsync(true, chatMessageStatusModel);
        }

        public Task<List<ReturnErrorItem>> IsChatMessageStatusModelValidForUpdateAsync(ChatMessageStatusUpdateModel chatMessageStatusModel)
        {
            return IsChatMessageStatusModelValidForAllAsync(false, chatMessageStatusModel);
        }

        public async Task<List<ReturnErrorItem>> IsChatMessageStatusModelValidForAllAsync(bool isCreate, ChatMessageStatusUpdateModel chatMessageStatusModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (chatMessageStatusModel.ChatMessageStatusId > 0)
                {
                    result.Add(new ReturnErrorItem("", "Chat Message Status Identifier is not valid"));
                }
            }
            else
            {
                if (chatMessageStatusModel.ChatMessageStatusId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "Chat Message Status Unique Identifier is not valid"));
                }
            }

            if (!await chatRepository.DoesChatMessageByIdExistsAsync(chatMessageStatusModel.ChatMessageId, chatMessageStatusModel.CompanyId))
            {
                result.Add(new ReturnErrorItem("", $"Unable to Get the ChatMessage Information"));
                return result;
            }

            if (!await companyRepository.DoesUserByIdExistsAsync(chatMessageStatusModel.UserId, chatMessageStatusModel.CompanyId))
            {
                result.Add(new ReturnErrorItem("", $"Unable to Get the User Information"));
                return result;
            }

            if (!ValidationUtils.IsLongValid(chatMessageStatusModel.ChatMessageId))
            {
                result.Add(new ReturnErrorItem("ChatMessageId", "ChatMessage cannot be empty"));
            }

            var itemForChatMessage = await chatRepository.GetChatMessageByIdAsync(chatMessageStatusModel.ChatMessageId);
            if (itemForChatMessage == null)
            {
                result.Add(new ReturnErrorItem("ChatMessageId", "ChatMessage is not available"));
            }

            if (!ValidationUtils.IsLongValid(chatMessageStatusModel.UserId))
            {
                result.Add(new ReturnErrorItem("UserId", "User cannot be empty"));
            }

            var itemForUser = await companyRepository.GetUserByIdAsync(chatMessageStatusModel.UserId);
            if (itemForUser == null)
            {
                result.Add(new ReturnErrorItem("UserId", "User is not available"));
            }

            if (chatMessageStatusModel.ChatMessageViewedDate != null && !DateUtils.IsValidDate(chatMessageStatusModel.ChatMessageViewedDate.Value))
            {
                result.Add(new ReturnErrorItem("ChatMessageViewedDate", $"ChatMessageViewedDate is not a valid date"));
            }

            if (chatMessageStatusModel.ChatMessageSentDate != null && !DateUtils.IsValidDate(chatMessageStatusModel.ChatMessageSentDate.Value))
            {
                result.Add(new ReturnErrorItem("ChatMessageSentDate", $"ChatMessageSentDate is not a valid date"));
            }

            return result.Count > 0 ? result : null;
        }

        /*
        public async Task<ReturnModel<bool>> IsChatMessageStatusWordOrPdfFileValidAsync(long chatMessageStatusId, bool isPdf)
        {
            var tName = isPdf ? "preview" : "source";

            try
            {
                var fileModel = await GetChatMessageStatusWordOrPdfFileAsync(chatMessageStatusId,isPdf);
                if (!fileModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the {tName} file", fileModel.ErrorHolder);
                }

                FileUtils.CloseSafely(fileModel.Model.FileData);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the {tName} file", ex);
            }
        }

        public async Task<ReturnModel<FileStreamModel>> GetChatMessageStatusWordOrdPdfFileAsync(long chatMessageStatusId, bool isPdf)
        {
            try
            {
                var view = await GetChatMessageStatusByIdForReadAsync(chatMessageStatusId);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the Chat Message Status", new { chatMessageStatusId });
                }

                var fileName = view.Model.SourceStoredFilePath;
                var docType = "Word";
                var docExt = ".docx";

                if (isPdf)
                {
                    fileName = view.Model.PreviewStoredFilePath;
                    docType = "Pdf";
                    docExt = ".pdf";
                }

                if (fileName.IsTrimmedStringNullOrEmpty())
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: {docType} file is not available.");
                }

                if (!await contractFileStorage.ExistsAsync(view.Model.CompanyId, fileName))
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: {docType} file is not available.");
                }

                var result = new FileStreamModel
                {
                    FileName = view.Model.ActualFileName.IsTrimmedStringNullOrEmpty() ? $"ChatMessageStatusFile.{docType}" : view.Model.ActualFileName,
                    ContentType = FileUtils.GetMimeTypeFromFileName(view.Model.ActualFileName),
                    FileData = await wipFileStorage.OpenReadAsync(view.Model.CompanyId, fileName)
                };

                return ReturnModel<FileStreamModel>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for Chat Message Status ", new { chatMessageStatusId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> IsChatMessageStatusFileValidAsync(long chatMessageStatusId)
        {
            try
            {
                var fileModel = await GetChatMessageStatusFileAsync(chatMessageStatusId);
                if (!fileModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the Chat Message Status file", fileModel.ErrorHolder);
                }

                FileUtils.CloseSafely(fileModel.Model.FileData);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the Chat Message Status file", ex);
            }
        }

        public async Task<ReturnModel<FileStreamModel>> GetChatMessageStatusFileAsync(long chatMessageStatusId)
        {
            try
            {
                var view = await GetChatMessageStatusByIdForReadAsync(chatMessageStatusId);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the Chat Message Status", new { chatMessageStatusId });
                }

                var fileName = view.Model.SourceStoredFilePath;

                if (fileName.IsTrimmedStringNullOrEmpty())
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: file is not available.");
                }

                if (!await wipFileStorage.ExistsAsync(view.Model.CompanyId, fileName))
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: file is not available.");
                }

                var actualFileName = view.Model.ActualFileName.IsTrimmedStringNullOrEmpty() ? $"ChatMessageStatusFile" : view.Model.ActualFileName;

                var result = new FileStreamModel
                {
                    FileName = actualFileName,
                    ContentType = FileUtils.GetMimeTypeFromFileName(actualFileName),
                    FileData = await wipFileStorage.OpenReadAsync(view.Model.CompanyId, fileName)
                };

                return ReturnModel<FileStreamModel>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for Chat Message Status ", new { chatMessageStatusId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> UploadChatMessageStatusFileAsync(long chatMessageStatusId, FileStreamModel streamModel)
        {
            try
            {
                var chatMessageStatus = await GetChatMessageStatusByIdForWriteAsync(chatMessageStatusId);
                if (!chatMessageStatus.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(chatMessageStatus);
                }

                if (streamModel == null)
                {
                    return log.ErrorAndReturnModel<bool>("File Content is not provided");
                }

                var company = await companyRepository.GetCompanyByIdAsync(chatMessageStatus.Model.CompanyId);
                if (company == null)
                {
                    return log.ErrorAndReturnModel<bool>("Unable to find the Company Information");
                }

                try
                {
                    using (var trx = await transactionManager.BeginTransactionAsync())
                    {
                        var path = profileFileFileStorage.ProfileFilePathForHostBrand(chatMessageStatus.Model.CompanyId, chatMessageStatus.Model.ChatMessageStatusId, streamModel.FileName);
                        if (company.PredefinedCompanyTypeId == (int)PredefinedCompanyTypes.MarketPlace)
                            path = profileFileFileStorage.ProfileFilePathForMarketplaceBrand(chatMessageStatus.Model.CompanyId, chatMessageStatus.Model.ChatMessageStatusId, streamModel.FileName);
                        await productRepository.UpdateChatMessageStatusFilePathAsync(chatMessageStatus.Model.ChatMessageStatusId, path, streamModel.FileName, await currentSessionService.GetCurrentUserIdAsync());
                        await profileFileFileStorage.WriteAsync(chatMessageStatus.Model.CompanyId, path, streamModel.FileData);
                        await trx.CompleteAsync();
                    }
                }
                finally
                {
                    FileUtils.CloseSafely(streamModel.FileData);
                }

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when Uploading Chat Message Status File", ex);
            }
        }

        public async Task<ReturnModel<bool>> ClearChatMessageStatusFileAsync(long chatMessageStatusId)
        {
            try
            {
                var chatMessageStatus = await GetChatMessageStatusByIdForWriteAsync(chatMessageStatusId);
                if (!chatMessageStatus.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(chatMessageStatus);
                }

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    await productRepository.ClearChatMessageStatusFilePathAsync(chatMessageStatus.Model.ChatMessageStatusId, await currentSessionService.GetCurrentUserIdAsync());
                    await profileFileFileStorage.DeleteAsync(chatMessageStatus.Model.CompanyId, chatMessageStatus.Model.ProfileImageLocation);
                }

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when clearing Chat Message Status File", ex);
            }
        }

        public async Task CreateOrUpdateChatMessageStatusDependentsAsync(ChatMessageStatusUpdateModel model)
        {
            if (model.NewChatMessageStatusXYZZYXModels == null)
                model.NewChatMessageStatusXYZZYXModels = new List<ChatMessageStatusXYZZYXCreateModel>();

            if (model.ExistingChatMessageStatusXYZZYXModels == null)
                model.ExistingChatMessageStatusXYZZYXModels = new List<ChatMessageStatusXYZZYXUpdateModel>();

            var dbExistingChatMessageStatuss = await chatRepository.GetChatMessageStatusXYZZYXsByChatMessageStatusIdAsync(model.ChatMessageStatusId);
            if (!dbExistingChatMessageStatuss.IsNullOrEmpty())
            {
                var dbExistingChatMessageStatusIds = dbExistingChatMessageStatuss.SelectToList(x => x.ChatMessageStatusXYZZYXId);
                var modelExistingChatMessageStatusIds = model.ExistingChatMessageStatusXYZZYXModels.SelectToList(x => x.ChatMessageStatusXYZZYXId);

                var xYZZYXItemsToDelete = dbExistingChatMessageStatuss.Where(x => !modelExistingChatMessageStatusIds.Contains(x.ChatMessageStatusXYZZYXId));
                foreach (var x in xYZZYXItemsToDelete)
                {
                    var result = await MarkChatMessageStatusXYZZYXAsDeletedByIdAsync(x.ChatMessageStatusXYZZYXId);
                    if (!result.IsSuccess)
                    {
                        throw new RollbackException(result);
                    }
                }

                var xYZZYXItemsToUpdate = model.ExistingChatMessageStatusXYZZYXModels.Where(x => dbExistingChatMessageStatusIds.Contains(x.ChatMessageStatusXYZZYXId));
                foreach (var x in xYZZYXItemsToUpdate)
                {
                    x.ChatMessageStatusId = model.ChatMessageStatusId;
                    var result = await UpdateChatMessageStatusXYZZYXAsync(x);
                    if (!result.IsSuccess)
                    {
                        throw new RollbackException(result);
                    }
                }
            }

            foreach (var x in model.NewChatMessageStatusXYZZYXModels)
            {
                x.ChatMessageStatusId = model.ChatMessageStatusId;
                var result = await CreateChatMessageStatusXYZZYXAsync(x);
                if (!result.IsSuccess)
                {
                    throw new RollbackException(result);
                }
            }
        }

        public async Task<ReturnModel<bool>> EnableChatMessageStatusByIdAsync(long chatMessageStatusId)
        {
            try
            {
                var existingChatMessageStatusModel = await GetChatMessageStatusByIdForWriteAsync(chatMessageStatusId);
                if (!existingChatMessageStatusModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<ChatMessageStatusModel>(existingChatMessageStatusModel);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(existingChatMessageStatusModel.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.EnableChatMessageStatusByIdAsync(existingChatMessageStatusModel.Model.ChatMessageStatusId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.ChatMessageStatusEnabledAsync(chatMessageStatusId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when enabling Chat Message Status", new { chatMessageStatusId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> DisableChatMessageStatusByIdAsync(long chatMessageStatusId)
        {
            try
            {
                var existingChatMessageStatusModel = await GetChatMessageStatusByIdForWriteAsync(chatMessageStatusId);
                if (!existingChatMessageStatusModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<ChatMessageStatusModel>(existingChatMessageStatusModel);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(existingChatMessageStatusModel.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.DisableChatMessageStatusByIdAsync(existingChatMessageStatusModel.Model.ChatMessageStatusId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.ChatMessageStatusDisabledAsync(existingChatMessageStatusModel.Model.ChatMessageStatusId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when disabling Chat Message Status",new { chatMessageStatusId }, ex);
            }
        }


        public async Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByChatMessageStatusAsync(long chatMessageStatusId)
        {
            try
            {
                var contract = await chatRepository.GetChatMessageStatusByIdAsync(chatMessageStatusId);
                if (contract == null)
                {
                    return log.ErrorAndReturnListModel<ChatMessageStatusModel>($"Unable to find Chat Message Status", new { chatMessageStatusId });
                }
                return this.ConvertToList(await chatRepository.GetXYZZYXsByChatMessageStatussAsync(contract.ChatMessageStatusId), objectMapperService.ChatMessageStatusViewListToChatMessageStatusModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ChatMessageStatusModel>($"Unexpected error when getting Related Chat Message Statuss of the Chat Message Status");
            }
        }

        public async Task<ReturnModel<bool>> AssociateXYZZYXWithChatMessageStatusAsync(ChatMessageStatusXYZZYXAssociationModel model)
        {
            try
            {
                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(await currentSessionService.GetCurrentCompanyIdAsync());

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                var errorLst = await IsChatMessageStatusWithChatMessageStatusAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("Chat Message Status association is invalid", errorLst);
                }

                if (await chatRepository.DoesChatMessageStatusWithXYZZYXAssociationExistsAsync(model.ChatMessageStatusId, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related Chat Message Status is already associated with the Chat Message Status", new { model.ChatMessageStatusId, model.XYZZYXId});
                }

                var rValue = await chatRepository.AssociateChatMessageStatusWithXYZZYXAsync(model.ChatMessageStatusId, model.XYZZYXId, await currentSessionService.GetCurrentUserIdAsync());
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }
                return log.ErrorAndReturnModel<bool>("Unable to associate Chat Message Status with XYZZYX", new { model.ChatMessageStatusId, model.XYZZYXId});
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when associating Chat Message Status with XYZZYX", new { model.ChatMessageStatusId, model.XYZZYXId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> UnAssociateXYZZYXWithChatMessageStatusAsync(ChatMessageStatusXYZZYXAssociationModel model)
        {
            try
            {
                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(await currentSessionService.GetCurrentCompanyIdAsync());

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                var errorLst = await IsChatMessageStatusWithXYZZYXAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("Chat Message Status association is not valid", errorLst);
                }

                if (!await chatRepository.DoesChatMessageStatusWithXYZZYXAssociationExistsAsync(model.ChatMessageStatusId, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related Chat Message Status is not associated with the Chat Message Status", new { model.ChatMessageStatusId, model.XYZZYXId });
                }

                var rValue = await chatRepository.UnAssociateChatMessageStatusWithChatMessageStatusAsync(model.ChatMessageStatusId, model.XYZZYXId);
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }

                return log.ErrorAndReturnModel<bool>("Unable to unassociate Related Chat Message Status with the Chat Message Status", new { model.ChatMessageStatusId, model.XYZZYXId });
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when unassociating Related Chat Message Status with the Chat Message Status", new { model.ChatMessageStatusId, model.XYZZYXId }, ex);
            }
        }

        public async Task<List<ReturnErrorItem>> IsChatMessageStatusWithXYZZYXAssociationValidAsync(ChatMessageStatusXYZZYXAssociationModel model)
        {
            var result = new List<ReturnErrorItem>();
            if (model == null)
            {
                result.Add(new ReturnErrorItem("", "Association is not valid"));
                return result;
            }

            if (model.ChatMessageStatusId == 0)
            {
                result.Add(new ReturnErrorItem("", "Chat Message Status is not valid"));
                return result;
            }

            if (model.XYZZYXId == 0)
            {
                result.Add(new ReturnErrorItem("", "Related Chat Message Status  is not valid"));
                return result;
            }

            var entity = await GetChatMessageStatusForWriteByIdAsync(model.ChatMessageStatusId);
            if(!entity.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "Chat Message Status information is not available"));
                return result;
            }

            var xYZZYX = await GetXYZZYXForWriteByIdAsync(model.XYZZYXId);
            if (!xYZZYX.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "Related XYZZYX information is not available"));
                return result;
            }

            if (entity.CompanyId != xYZZYX.CompanyId)
            {
                result.Add(new ReturnErrorItem("", "Chat Message Statuss does not belong to the same company"));
            }

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }

        public async Task<ReturnListModel<ImportStatusModel>> ImportChatMessageStatusAsync(ChatMessageStatusImportModel model, bool isTest)
        {
            try
            {
                if (model == null)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("Chat Message Status Import Info is not provided");
                }

                if (model.ChatMessageStatuss == null || model.ChatMessageStatuss.Count == 0)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("Chat Message Statuss Info is not available to import");
                }

                var resultLst = new List<ImportStatusModel>();

                foreach (var item in model.ChatMessageStatuss)
                {
                    var prod = await wipService.GetChatMessageStatusByExternalIdentifierAsync(item.ChatMessageStatusIdentifier);
                    if (prod.Model == null)
                    {
                        //var newProd = new ProductCreateModel();
                        ChatMessageStatusCreateModel chatMessageStatusCreateModel = new ChatMessageStatusCreateModel
                        {
                            IsEnabled = item.IsProductEnabled
                        };

                        var newlyCreatedChatMessageStatus = await CreateChatMessageStatusAsync(chatMessageStatusCreateModel);
                        if (!newlyCreatedChatMessageStatus.IsSuccess)
                        {
                            var errorStr = string.Empty;
                            if (newlyCreatedChatMessageStatus.ErrorHolder.ErrorItemList != null && newlyCreatedProd.ErrorHolder.ErrorItemList.Count > 0)
                            {
                                var errorValueList = newlyCreatedChatMessageStatus.ErrorHolder.ErrorItemList.SelectToList(x => x.Value);
                                errorStr = string.Join(",", errorValueList);
                            }

                            resultLst.Add(new ImportStatusModel() { IsSucess = false, Message = errorStr.IsNotNullOrEmpty() ? errorStr : newlyCreatedProd.ErrorHolder.FriendlyMessage });
                            continue;
                        }
                        else
                        {
                            resultLst.Add(new ImportStatusModel() { IsSucess = true });
                        }
                    }
                    else
                    {
                        ChatMessageStatusUpdateModel chatMessageStatusUpdateModel = new ChatMessageStatusUpdateModel
                        {
                        };

                        //update product
                        var updatedChatMessageStatusModel = await wipService.UpdateChatMessageStatusAsync(chatMessageStatusUpdateModel);
                        if (!updatedChatMessageStatusModel.IsSuccess)
                        {
                            var errorStr = string.Empty;
                            if (updateProdModel.ErrorHolder.ErrorItemList != null && updateProdModel.ErrorHolder.ErrorItemList.Count > 0)
                            {
                                var errorValueList = updateProdModel.ErrorHolder.ErrorItemList.SelectToList(x => x.Value);
                                errorStr = string.Join(",", errorValueList);
                            }

                            resultLst.Add(new ImportStatusModel() { IsSucess = false, Message = errorStr.IsNotNullOrEmpty() ? errorStr : updateProdModel.ErrorHolder.FriendlyMessage });
                            continue;
                        }
                        else
                        {
                            resultLst.Add(new ImportStatusModel() { IsSucess = true });
                        }
                    }
                }

                return ReturnListModel<ImportStatusModel>.Success(resultLst);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ImportStatusModel>("Unexpected error occurred when importing Chat Message Status Info", ex);
            }
        }

        public async Task<ReturnListModel<ChatMessageStatusExportModel>> ExportChatMessageStatusAsync(ChatMessageStatusExportParameter filter, bool isTest)
        {
            try
            {
                var chatMessageStatusCount = await chatRepository.GetChatMessageStatusCountAsync(filter);
                if (chatMessageStatusCount == 0)
                {
                    return log.ErrorAndReturnListModel<ChatMessageStatusExportModel>("No Chat Message Status available for export.");
                }

                var itemsToReturn = new List<ChatMessageStatusExportModel>();
                var numberOfLoops = (chatMessageStatusCount / pageSize) + 1;
                for (var i = 0; i < numberOfLoops; ++i)
                {
                    var dataResult = await wipService.GetUserViewListAsync(new ChatMessageStatusSearchCriteria { Page = i, PageSize = pageSize, Keyword = filter.Keyword });
                    if (dataResult.IsSuccess)
                    {
                        return log.ErrorAndReturnListModel<ChatMessageStatusExportModel>(dataResult);
                    }

                    itemsToReturn.AddRange(objectMapperService.MapList<ChatMessageStatusModel, ChatMessageStatusExportModel>(dataResult.Model));
                }

                return ReturnListModel<ChatMessageStatusExportModel>.Success(itemsToReturn);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ChatMessageStatusExportModel>("Unexpected error occurred when exporting Chat Message Status", ex);
            }
        }

        */
        #endregion

        #region ChatUser

        public async Task<ReturnListModel<ChatUserModel>> GetChatUsersAsync(ChatUserSearchCriteria search)
        {
            try
            {
                return this.ConvertToList(await chatRepository.GetChatUsersAsync(search), objectMapperService.ChatUserViewListToChatUserModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ChatUserModel>($"Unexpected error when getting Chat Users", ex);
            }
        }

        public Task<ReturnModel<ChatUserModel>> GetChatUserByIdForReadAsync(long chatUserId)
        {
            return GetChatUserByIdInternalAsync(forWrite: false, chatUserId);
        }

        public Task<ReturnModel<ChatUserModel>> GetChatUserByIdForWriteAsync(long chatUserId)
        {
            return GetChatUserByIdInternalAsync(forWrite: true, chatUserId);
        }

        public async Task<ReturnModel<ChatUserModel>> GetChatUserByIdInternalAsync(bool forWrite, long chatUserId)
        {
            try
            {
                var item = await chatRepository.GetChatUserViewByIdAsync(chatUserId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<ChatUserModel>($"Unable to find the Chat User", new { chatUserId });
                }

                //var existingMovieModel = forWrite ? await GetMovieByIdForWriteAsync(item.MovieId) : await GetMovieByIdForReadAsync(item.MovieId);
                //if (!existingMovieModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<CompanyModel>(existingMovieModel);
                //}

                //var result = this.ConvertToItem(item,objectMapperService.ChatUserViewToChatUserModel);
                //var childItems = await GetChatUserXYZZYXsAsync(new ChatUserXYZZYXSearchCriteria { ChatUserId = item.ChatUserId, Page = 1, PageSize = int.MaxValue });
                //result.Model.ExistingChatUserXYZZYXs = childItems.Model.Where(x => x.ChatUserId == item.ChatUserId).ToList();
                //return result;

                return this.ConvertToItem(item, objectMapperService.ChatUserViewToChatUserModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<ChatUserModel>($"Unexpected error when getting Chat User", new { chatUserId }, ex);
            }
        }

        public async Task<ReturnListModel<ChatUserModel>> CreateMultipleChatUserAsync(MultipleChatUserCreateModel chatUsers)
        {
            try
            {
                if (chatUsers == null)
                {
                    return log.ErrorAndReturnListModel<ChatUserModel>($"Unable to Create Chat Users", new ReturnErrorItem("", "Chat Users information is not valid"));
                }

                if (chatUsers.NewChatUsers.IsNullOrEmpty())
                {
                    return log.ErrorAndReturnListModel<ChatUserModel>($"Unable to Create Chat Users", new ReturnErrorItem("", "Chat Users information is not provided"));
                }

                var lst = new List<ChatUserModel>();

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    foreach (var chatUser in chatUsers.NewChatUsers)
                    {
                        var cResult = await CreateChatUserAsync(chatUser);
                        if (!cResult.IsSuccess)
                            throw new RollbackException(cResult);

                        lst.Add(cResult.Model);
                    }

                    await trx.CompleteAsync();
                }

                return ReturnListModel<ChatUserModel>.Success(lst);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModelWithRollbackExceptionHandler<ChatUserModel>($"Unexpected error when creating Chat Users", new { chatUsers }, ex);
            }
        }

        public async Task<ReturnModel<ChatUserModel>> CreateChatUserAsync(ChatUserCreateModel chatUser)
        {
            try
            {
                if (chatUser == null)
                {
                    return log.ErrorAndReturnModel<ChatUserModel>($"Unable to create Chat User", new ReturnErrorItem("", "Chat User information is not valid"));
                }

                var errorList = await IsChatUserModelValidForCreateAsync(chatUser);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<ChatUserModel>($"Unable to create Chat User", errorList);
                }

                chatUser.IsDeleted = false;

                var item = objectMapperService.ChatUserCreateModelToChatUser(chatUser);

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await chatRepository.CreateChatUserAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create Chat User");
                    }

                    //await CreateOrUpdateChatUserDependentsAsync(chatUser)

                    /*if (ignoreCode)
                    {
                        if (!await chatRepository.UpdateChatUserCodeAsync(item.CompanyId, item.ChatUserId, StringConstants.PrefixForCode.ChatUser + item.ChatUserId, await currentSessionService.GetCurrentUserIdAsync()))
                        {
                            throw new RollbackException($"Unable update the Chat User Code", item);
                        }
                    }*/

                    await trx.CompleteAsync();
                }

                await auditService.ChatUserCreatedAsync(item.ChatUserId);

                var view = await chatRepository.GetChatUserViewByIdAsync(item.ChatUserId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<ChatUserModel>($"Unable to find the Chat User", new { item.ChatUserId });
                }

                return this.ConvertToItem(view, objectMapperService.ChatUserViewToChatUserModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<ChatUserModel>($"Unexpected error when creating Chat User", new { chatUser?.ChatUserId }, ex);
            }
        }

        public async Task<ReturnModel<ChatUserModel>> UpdateChatUserAsync(ChatUserUpdateModel chatUser)
        {
            try
            {
                if (chatUser == null)
                {
                    return log.ErrorAndReturnModel<ChatUserModel>($"Unable to update Chat User", new ReturnErrorItem("", "Chat User information is not valid"));
                }

                var existingChatUserModel = await GetChatUserByIdForWriteAsync(chatUser.ChatUserId);
                if (!existingChatUserModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<ChatUserModel>(existingChatUserModel);
                }

                chatUser.CreatedByUserId = existingChatUserModel.Model.CreatedByUserId;
                chatUser.CreatedDate = existingChatUserModel.Model.CreatedDate;
                chatUser.IsDeleted = existingChatUserModel.Model.IsDeleted;
                //chatUser.MovieId = existingChatUserModel.Model.MovieId;
                //chatUser.ProfileImageActualFileName = existingChatUserModel.Model.ProfileImageActualFileName;
                //chatUser.ProfileImageLocation = existingChatUserModel.Model.ProfileImageLocation;

                var errorList = await IsChatUserModelValidForUpdateAsync(chatUser);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<ChatUserModel>($"Unable to update Chat User", errorList);
                }

                var item = objectMapperService.ChatUserUpdateModelToChatUser(chatUser);

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.UpdateChatUserAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update Chat User");
                    }

                    //await CreateOrUpdateChatUserDependentsAsync(chatUser)

                    await trx.CompleteAsync();
                }

                await auditService.ChatUserUpdatedAsync(chatUser.ChatUserId);

                var view = await chatRepository.GetChatUserViewByIdAsync(chatUser.ChatUserId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<ChatUserModel>($"Unable to find the Chat User", new { chatUser.ChatUserId });
                }

                return this.ConvertToItem(view, objectMapperService.ChatUserViewToChatUserModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<ChatUserModel>($"Unexpected error when updating Chat User", new { chatUser?.ChatUserId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkChatUserAsNotDeletedByIdAsync(long chatUserId)
        {
            try
            {
                var existingChatUserModel = await GetChatUserByIdForWriteAsync(chatUserId);
                if (!existingChatUserModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingChatUserModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.MarkChatUserAsNotDeletedByIdAsync(chatUserId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.ChatUserDeletedAsync(chatUserId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking Chat User as not deleted", new { chatUserId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkChatUserAsDeletedByIdAsync(long chatUserId)
        {
            try
            {
                var existingChatUserModel = await GetChatUserByIdForWriteAsync(chatUserId);
                if (!existingChatUserModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingChatUserModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.MarkChatUserAsDeletedByIdAsync(chatUserId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.ChatUserDeletedAsync(chatUserId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking Chat User as deleted", new { chatUserId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteChatUserByAsync(long chatUserId)
        {
            try
            {
                var existingChatUserModel = await GetChatUserByIdForWriteAsync(chatUserId);
                if (!existingChatUserModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingChatUserModel);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.HardDeleteChatUserByIdAsync(chatUserId);
                    await trx.CompleteAsync();
                }

                await auditService.HardDeleteChatUserAsync(chatUserId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting Chat User", new { chatUserId }, ex);
            }
        }

        public Task<List<ReturnErrorItem>> IsChatUserModelValidForCreateAsync(ChatUserUpdateModel chatUserModel)
        {
            return IsChatUserModelValidForAllAsync(true, chatUserModel);
        }

        public Task<List<ReturnErrorItem>> IsChatUserModelValidForUpdateAsync(ChatUserUpdateModel chatUserModel)
        {
            return IsChatUserModelValidForAllAsync(false, chatUserModel);
        }

        public async Task<List<ReturnErrorItem>> IsChatUserModelValidForAllAsync(bool isCreate, ChatUserUpdateModel chatUserModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (chatUserModel.ChatUserId > 0)
                {
                    result.Add(new ReturnErrorItem("", "Chat User Identifier is not valid"));
                }
            }
            else
            {
                if (chatUserModel.ChatUserId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "Chat User Unique Identifier is not valid"));
                }
            }

            if (!await companyRepository.DoesUserByIdExistsAsync(chatUserModel.UserId, chatUserModel.CompanyId))
            {
                result.Add(new ReturnErrorItem("", $"Unable to Get the User Information"));
                return result;
            }


            if (!ValidationUtils.IsLongValid(chatUserModel.UserId))
            {
                result.Add(new ReturnErrorItem("UserId", "User cannot be empty"));
            }

            var itemForUser = await companyRepository.GetUserByIdAsync(chatUserModel.UserId);
            if (itemForUser == null)
            {
                result.Add(new ReturnErrorItem("UserId", "User is not available"));
            }

            return result.Count > 0 ? result : null;
        }

        /*
        public async Task<ReturnModel<bool>> IsChatUserWordOrPdfFileValidAsync(long chatUserId, bool isPdf)
        {
            var tName = isPdf ? "preview" : "source";

            try
            {
                var fileModel = await GetChatUserWordOrPdfFileAsync(chatUserId,isPdf);
                if (!fileModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the {tName} file", fileModel.ErrorHolder);
                }

                FileUtils.CloseSafely(fileModel.Model.FileData);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the {tName} file", ex);
            }
        }

        public async Task<ReturnModel<FileStreamModel>> GetChatUserWordOrdPdfFileAsync(long chatUserId, bool isPdf)
        {
            try
            {
                var view = await GetChatUserByIdForReadAsync(chatUserId);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the Chat User", new { chatUserId });
                }

                var fileName = view.Model.SourceStoredFilePath;
                var docType = "Word";
                var docExt = ".docx";

                if (isPdf)
                {
                    fileName = view.Model.PreviewStoredFilePath;
                    docType = "Pdf";
                    docExt = ".pdf";
                }

                if (fileName.IsTrimmedStringNullOrEmpty())
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: {docType} file is not available.");
                }

                if (!await contractFileStorage.ExistsAsync(view.Model.CompanyId, fileName))
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: {docType} file is not available.");
                }

                var result = new FileStreamModel
                {
                    FileName = view.Model.ActualFileName.IsTrimmedStringNullOrEmpty() ? $"ChatUserFile.{docType}" : view.Model.ActualFileName,
                    ContentType = FileUtils.GetMimeTypeFromFileName(view.Model.ActualFileName),
                    FileData = await wipFileStorage.OpenReadAsync(view.Model.CompanyId, fileName)
                };

                return ReturnModel<FileStreamModel>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for Chat User ", new { chatUserId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> IsChatUserFileValidAsync(long chatUserId)
        {
            try
            {
                var fileModel = await GetChatUserFileAsync(chatUserId);
                if (!fileModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the Chat User file", fileModel.ErrorHolder);
                }

                FileUtils.CloseSafely(fileModel.Model.FileData);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the Chat User file", ex);
            }
        }

        public async Task<ReturnModel<FileStreamModel>> GetChatUserFileAsync(long chatUserId)
        {
            try
            {
                var view = await GetChatUserByIdForReadAsync(chatUserId);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the Chat User", new { chatUserId });
                }

                var fileName = view.Model.SourceStoredFilePath;

                if (fileName.IsTrimmedStringNullOrEmpty())
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: file is not available.");
                }

                if (!await wipFileStorage.ExistsAsync(view.Model.CompanyId, fileName))
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: file is not available.");
                }

                var actualFileName = view.Model.ActualFileName.IsTrimmedStringNullOrEmpty() ? $"ChatUserFile" : view.Model.ActualFileName;

                var result = new FileStreamModel
                {
                    FileName = actualFileName,
                    ContentType = FileUtils.GetMimeTypeFromFileName(actualFileName),
                    FileData = await wipFileStorage.OpenReadAsync(view.Model.CompanyId, fileName)
                };

                return ReturnModel<FileStreamModel>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for Chat User ", new { chatUserId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> UploadChatUserFileAsync(long chatUserId, FileStreamModel streamModel)
        {
            try
            {
                var chatUser = await GetChatUserByIdForWriteAsync(chatUserId);
                if (!chatUser.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(chatUser);
                }

                if (streamModel == null)
                {
                    return log.ErrorAndReturnModel<bool>("File Content is not provided");
                }

                var company = await companyRepository.GetCompanyByIdAsync(chatUser.Model.CompanyId);
                if (company == null)
                {
                    return log.ErrorAndReturnModel<bool>("Unable to find the Company Information");
                }

                try
                {
                    using (var trx = await transactionManager.BeginTransactionAsync())
                    {
                        var path = profileFileFileStorage.ProfileFilePathForHostBrand(chatUser.Model.CompanyId, chatUser.Model.ChatUserId, streamModel.FileName);
                        if (company.PredefinedCompanyTypeId == (int)PredefinedCompanyTypes.MarketPlace)
                            path = profileFileFileStorage.ProfileFilePathForMarketplaceBrand(chatUser.Model.CompanyId, chatUser.Model.ChatUserId, streamModel.FileName);
                        await productRepository.UpdateChatUserFilePathAsync(chatUser.Model.ChatUserId, path, streamModel.FileName, await currentSessionService.GetCurrentUserIdAsync());
                        await profileFileFileStorage.WriteAsync(chatUser.Model.CompanyId, path, streamModel.FileData);
                        await trx.CompleteAsync();
                    }
                }
                finally
                {
                    FileUtils.CloseSafely(streamModel.FileData);
                }

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when Uploading Chat User File", ex);
            }
        }

        public async Task<ReturnModel<bool>> ClearChatUserFileAsync(long chatUserId)
        {
            try
            {
                var chatUser = await GetChatUserByIdForWriteAsync(chatUserId);
                if (!chatUser.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(chatUser);
                }

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    await productRepository.ClearChatUserFilePathAsync(chatUser.Model.ChatUserId, await currentSessionService.GetCurrentUserIdAsync());
                    await profileFileFileStorage.DeleteAsync(chatUser.Model.CompanyId, chatUser.Model.ProfileImageLocation);
                }

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when clearing Chat User File", ex);
            }
        }

        public async Task CreateOrUpdateChatUserDependentsAsync(ChatUserUpdateModel model)
        {
            if (model.NewChatUserXYZZYXModels == null)
                model.NewChatUserXYZZYXModels = new List<ChatUserXYZZYXCreateModel>();

            if (model.ExistingChatUserXYZZYXModels == null)
                model.ExistingChatUserXYZZYXModels = new List<ChatUserXYZZYXUpdateModel>();

            var dbExistingChatUsers = await chatRepository.GetChatUserXYZZYXsByChatUserIdAsync(model.ChatUserId);
            if (!dbExistingChatUsers.IsNullOrEmpty())
            {
                var dbExistingChatUserIds = dbExistingChatUsers.SelectToList(x => x.ChatUserXYZZYXId);
                var modelExistingChatUserIds = model.ExistingChatUserXYZZYXModels.SelectToList(x => x.ChatUserXYZZYXId);

                var xYZZYXItemsToDelete = dbExistingChatUsers.Where(x => !modelExistingChatUserIds.Contains(x.ChatUserXYZZYXId));
                foreach (var x in xYZZYXItemsToDelete)
                {
                    var result = await MarkChatUserXYZZYXAsDeletedByIdAsync(x.ChatUserXYZZYXId);
                    if (!result.IsSuccess)
                    {
                        throw new RollbackException(result);
                    }
                }

                var xYZZYXItemsToUpdate = model.ExistingChatUserXYZZYXModels.Where(x => dbExistingChatUserIds.Contains(x.ChatUserXYZZYXId));
                foreach (var x in xYZZYXItemsToUpdate)
                {
                    x.ChatUserId = model.ChatUserId;
                    var result = await UpdateChatUserXYZZYXAsync(x);
                    if (!result.IsSuccess)
                    {
                        throw new RollbackException(result);
                    }
                }
            }

            foreach (var x in model.NewChatUserXYZZYXModels)
            {
                x.ChatUserId = model.ChatUserId;
                var result = await CreateChatUserXYZZYXAsync(x);
                if (!result.IsSuccess)
                {
                    throw new RollbackException(result);
                }
            }
        }

        public async Task<ReturnModel<bool>> EnableChatUserByIdAsync(long chatUserId)
        {
            try
            {
                var existingChatUserModel = await GetChatUserByIdForWriteAsync(chatUserId);
                if (!existingChatUserModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<ChatUserModel>(existingChatUserModel);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(existingChatUserModel.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.EnableChatUserByIdAsync(existingChatUserModel.Model.ChatUserId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.ChatUserEnabledAsync(chatUserId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when enabling Chat User", new { chatUserId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> DisableChatUserByIdAsync(long chatUserId)
        {
            try
            {
                var existingChatUserModel = await GetChatUserByIdForWriteAsync(chatUserId);
                if (!existingChatUserModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<ChatUserModel>(existingChatUserModel);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(existingChatUserModel.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await chatRepository.DisableChatUserByIdAsync(existingChatUserModel.Model.ChatUserId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.ChatUserDisabledAsync(existingChatUserModel.Model.ChatUserId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when disabling Chat User",new { chatUserId }, ex);
            }
        }


        public async Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByChatUserAsync(long chatUserId)
        {
            try
            {
                var contract = await chatRepository.GetChatUserByIdAsync(chatUserId);
                if (contract == null)
                {
                    return log.ErrorAndReturnListModel<ChatUserModel>($"Unable to find Chat User", new { chatUserId });
                }
                return this.ConvertToList(await chatRepository.GetXYZZYXsByChatUsersAsync(contract.ChatUserId), objectMapperService.ChatUserViewListToChatUserModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ChatUserModel>($"Unexpected error when getting Related Chat Users of the Chat User");
            }
        }

        public async Task<ReturnModel<bool>> AssociateXYZZYXWithChatUserAsync(ChatUserXYZZYXAssociationModel model)
        {
            try
            {
                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(await currentSessionService.GetCurrentCompanyIdAsync());

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                var errorLst = await IsChatUserWithChatUserAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("Chat User association is invalid", errorLst);
                }

                if (await chatRepository.DoesChatUserWithXYZZYXAssociationExistsAsync(model.ChatUserId, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related Chat User is already associated with the Chat User", new { model.ChatUserId, model.XYZZYXId});
                }

                var rValue = await chatRepository.AssociateChatUserWithXYZZYXAsync(model.ChatUserId, model.XYZZYXId, await currentSessionService.GetCurrentUserIdAsync());
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }
                return log.ErrorAndReturnModel<bool>("Unable to associate Chat User with XYZZYX", new { model.ChatUserId, model.XYZZYXId});
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when associating Chat User with XYZZYX", new { model.ChatUserId, model.XYZZYXId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> UnAssociateXYZZYXWithChatUserAsync(ChatUserXYZZYXAssociationModel model)
        {
            try
            {
                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(await currentSessionService.GetCurrentCompanyIdAsync());

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                var errorLst = await IsChatUserWithXYZZYXAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("Chat User association is not valid", errorLst);
                }

                if (!await chatRepository.DoesChatUserWithXYZZYXAssociationExistsAsync(model.ChatUserId, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related Chat User is not associated with the Chat User", new { model.ChatUserId, model.XYZZYXId });
                }

                var rValue = await chatRepository.UnAssociateChatUserWithChatUserAsync(model.ChatUserId, model.XYZZYXId);
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }

                return log.ErrorAndReturnModel<bool>("Unable to unassociate Related Chat User with the Chat User", new { model.ChatUserId, model.XYZZYXId });
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when unassociating Related Chat User with the Chat User", new { model.ChatUserId, model.XYZZYXId }, ex);
            }
        }

        public async Task<List<ReturnErrorItem>> IsChatUserWithXYZZYXAssociationValidAsync(ChatUserXYZZYXAssociationModel model)
        {
            var result = new List<ReturnErrorItem>();
            if (model == null)
            {
                result.Add(new ReturnErrorItem("", "Association is not valid"));
                return result;
            }

            if (model.ChatUserId == 0)
            {
                result.Add(new ReturnErrorItem("", "Chat User is not valid"));
                return result;
            }

            if (model.XYZZYXId == 0)
            {
                result.Add(new ReturnErrorItem("", "Related Chat User  is not valid"));
                return result;
            }

            var entity = await GetChatUserForWriteByIdAsync(model.ChatUserId);
            if(!entity.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "Chat User information is not available"));
                return result;
            }

            var xYZZYX = await GetXYZZYXForWriteByIdAsync(model.XYZZYXId);
            if (!xYZZYX.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "Related XYZZYX information is not available"));
                return result;
            }

            if (entity.CompanyId != xYZZYX.CompanyId)
            {
                result.Add(new ReturnErrorItem("", "Chat Users does not belong to the same company"));
            }

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }

        public async Task<ReturnListModel<ImportStatusModel>> ImportChatUserAsync(ChatUserImportModel model, bool isTest)
        {
            try
            {
                if (model == null)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("Chat User Import Info is not provided");
                }

                if (model.ChatUsers == null || model.ChatUsers.Count == 0)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("Chat Users Info is not available to import");
                }

                var resultLst = new List<ImportStatusModel>();

                foreach (var item in model.ChatUsers)
                {
                    var prod = await wipService.GetChatUserByExternalIdentifierAsync(item.ChatUserIdentifier);
                    if (prod.Model == null)
                    {
                        //var newProd = new ProductCreateModel();
                        ChatUserCreateModel chatUserCreateModel = new ChatUserCreateModel
                        {
                            IsEnabled = item.IsProductEnabled
                        };

                        var newlyCreatedChatUser = await CreateChatUserAsync(chatUserCreateModel);
                        if (!newlyCreatedChatUser.IsSuccess)
                        {
                            var errorStr = string.Empty;
                            if (newlyCreatedChatUser.ErrorHolder.ErrorItemList != null && newlyCreatedProd.ErrorHolder.ErrorItemList.Count > 0)
                            {
                                var errorValueList = newlyCreatedChatUser.ErrorHolder.ErrorItemList.SelectToList(x => x.Value);
                                errorStr = string.Join(",", errorValueList);
                            }

                            resultLst.Add(new ImportStatusModel() { IsSucess = false, Message = errorStr.IsNotNullOrEmpty() ? errorStr : newlyCreatedProd.ErrorHolder.FriendlyMessage });
                            continue;
                        }
                        else
                        {
                            resultLst.Add(new ImportStatusModel() { IsSucess = true });
                        }
                    }
                    else
                    {
                        ChatUserUpdateModel chatUserUpdateModel = new ChatUserUpdateModel
                        {
                        };

                        //update product
                        var updatedChatUserModel = await wipService.UpdateChatUserAsync(chatUserUpdateModel);
                        if (!updatedChatUserModel.IsSuccess)
                        {
                            var errorStr = string.Empty;
                            if (updateProdModel.ErrorHolder.ErrorItemList != null && updateProdModel.ErrorHolder.ErrorItemList.Count > 0)
                            {
                                var errorValueList = updateProdModel.ErrorHolder.ErrorItemList.SelectToList(x => x.Value);
                                errorStr = string.Join(",", errorValueList);
                            }

                            resultLst.Add(new ImportStatusModel() { IsSucess = false, Message = errorStr.IsNotNullOrEmpty() ? errorStr : updateProdModel.ErrorHolder.FriendlyMessage });
                            continue;
                        }
                        else
                        {
                            resultLst.Add(new ImportStatusModel() { IsSucess = true });
                        }
                    }
                }

                return ReturnListModel<ImportStatusModel>.Success(resultLst);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ImportStatusModel>("Unexpected error occurred when importing Chat User Info", ex);
            }
        }

        public async Task<ReturnListModel<ChatUserExportModel>> ExportChatUserAsync(ChatUserExportParameter filter, bool isTest)
        {
            try
            {
                var chatUserCount = await chatRepository.GetChatUserCountAsync(filter);
                if (chatUserCount == 0)
                {
                    return log.ErrorAndReturnListModel<ChatUserExportModel>("No Chat User available for export.");
                }

                var itemsToReturn = new List<ChatUserExportModel>();
                var numberOfLoops = (chatUserCount / pageSize) + 1;
                for (var i = 0; i < numberOfLoops; ++i)
                {
                    var dataResult = await wipService.GetUserViewListAsync(new ChatUserSearchCriteria { Page = i, PageSize = pageSize, Keyword = filter.Keyword });
                    if (dataResult.IsSuccess)
                    {
                        return log.ErrorAndReturnListModel<ChatUserExportModel>(dataResult);
                    }

                    itemsToReturn.AddRange(objectMapperService.MapList<ChatUserModel, ChatUserExportModel>(dataResult.Model));
                }

                return ReturnListModel<ChatUserExportModel>.Success(itemsToReturn);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<ChatUserExportModel>("Unexpected error occurred when exporting Chat User", ex);
            }
        }

        */
        #endregion

    }
}
