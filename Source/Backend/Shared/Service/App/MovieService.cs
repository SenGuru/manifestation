using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.ViewModels;
using Manifest.Repository.Common;
using Manifest.Service.Manager;
using Manifest.Service.ObjectMapping;
using Manifest.Service.Storage;
using Manifest.Service.Utils;
using Framework.Infrastructure.Exceptions;
using Framework.Infrastructure.Interfaces.DbAccess;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;

namespace Manifest.Service.App
{
    public class MovieService : BaseAppService
    {
        private readonly CompanyRepository companyRepository;
        private readonly MovieRepository movieRepository;
        private readonly ITransactionManager transactionManager;
        private readonly AuditService auditService;
        private readonly PermissionService permissionService;
        private readonly ICurrentSessionService currentSessionService;
        private readonly FileAttachmentFileStorage fileAttachmentFileStorage;
        private readonly CompanyService companyService;

        public MovieService(
                           ILog log,
                           SystemRepository systemRepository,
                           CompanyRepository companyRepository,
                           MovieRepository movieRepository,
                           ObjectMapperService objectMapperService,
                           AuditService auditService,
                           PermissionService permissionService,
                           ITransactionManager transactionManager,
                           ICurrentSessionService currentSessionService,
                           FileAttachmentFileStorage fileAttachmentFileStorage,
                           ProfilePictureFileStorage profilePictureFileStorage,
                           CompanyService companyService,
                           DocumentConverterService documentConverterService,
                           WordService wordService,
                           ScheduledJobManager scheduledJobManager)
            : base(log, objectMapperService)
        {
            this.companyRepository = companyRepository;
            this.movieRepository = movieRepository;
            this.auditService = auditService;
            this.permissionService = permissionService;
            this.transactionManager = transactionManager;
            this.currentSessionService = currentSessionService;
            this.fileAttachmentFileStorage = fileAttachmentFileStorage;
            this.companyService = companyService;
        }

        #region NoteAttachment

        public async Task<ReturnListModel<NoteAttachmentModel>> GetNoteAttachmentsAsync(NoteAttachmentSearchCriteria search)
        {
            try
            {
                return this.ConvertToList(await movieRepository.GetNoteAttachmentsAsync(search), objectMapperService.NoteAttachmentViewListToNoteAttachmentModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<NoteAttachmentModel>($"Unexpected error when getting NoteAttachments", ex);
            }
        }

        public Task<ReturnModel<NoteAttachmentModel>> GetNoteAttachmentByIdForReadAsync(long noteAttachmentId)
        {
            return GetNoteAttachmentByIdInternalAsync(forWrite: false, noteAttachmentId);
        }

        public Task<ReturnModel<NoteAttachmentModel>> GetNoteAttachmentByIdForWriteAsync(long noteAttachmentId)
        {
            return GetNoteAttachmentByIdInternalAsync(forWrite: true, noteAttachmentId);
        }

        public async Task<ReturnModel<NoteAttachmentModel>> GetNoteAttachmentByIdInternalAsync(bool forWrite, long noteAttachmentId)
        {
            try
            {
                var item = await movieRepository.GetNoteAttachmentViewByIdAsync(noteAttachmentId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<NoteAttachmentModel>($"Unable to find the noteAttachment", new { noteAttachmentId });
                }

                return this.ConvertToItem(item, objectMapperService.NoteAttachmentViewToNoteAttachmentModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<NoteAttachmentModel>($"Unexpected error when getting noteAttachment", new { noteAttachmentId }, ex);
            }
        }

        public async Task<ReturnModel<NoteAttachmentModel>> CreateNoteAttachmentAsync(NoteAttachmentCreateModel noteAttachment)
        {
            try
            {
                if (noteAttachment == null)
                {
                    return log.ErrorAndReturnModel<NoteAttachmentModel>($"Unable to create NoteAttachment", new ReturnErrorItem("", "NoteAttachment information is not valid"));
                }

                noteAttachment.UserId = await this.currentSessionService.GetCurrentUserIdAsync() ?? 0;

                //var companyModel = await GetCompanyForReadById(noteAttachment.CompanyId);
                //if (!companyModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<NoteAttachmentModel>(companyModel);
                //}

                var errorList = await IsNoteAttachmentModelValidForCreateAsync(noteAttachment);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<NoteAttachmentModel>($"Unable to create noteAttachment", errorList);
                }

                noteAttachment.IsDeleted = false;

                var item = objectMapperService.NoteAttachmentCreateModelToNoteAttachment(noteAttachment);

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await movieRepository.CreateNoteAttachmentAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create noteAttachment");
                    }

                    await trx.CompleteAsync();
                }

                await auditService.NoteAttachmentCreatedAsync(item.NoteAttachmentId);

                var view = await movieRepository.GetNoteAttachmentViewByIdAsync(item.NoteAttachmentId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<NoteAttachmentModel>($"Unable to find the noteAttachment", new { item.NoteAttachmentId });
                }

                return this.ConvertToItem(view, objectMapperService.NoteAttachmentViewToNoteAttachmentModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<NoteAttachmentModel>($"Unexpected error when creating NoteAttachment", new { noteAttachment?.NoteAttachmentId }, ex);
            }
        }

        public async Task<ReturnModel<NoteAttachmentModel>> CreateOrUpdateNoteAttachmentAsync(NoteAttachmentCreateModel noteAttachment)
        {
            try
            {
                if (noteAttachment == null)
                {
                    return log.ErrorAndReturnModel<NoteAttachmentModel>($"Unable to create NoteAttachment", new ReturnErrorItem("", "NoteAttachment information is not valid"));
                }

                ReturnModel<NoteAttachmentModel> noteAttachmentModel;
                var existingNoteAttachment = await movieRepository.GetNoteAttachmentByIdAsync(noteAttachment.NoteAttachmentId);
                if (existingNoteAttachment != null)
                {
                    noteAttachmentModel = await UpdateNoteAttachmentAsync(noteAttachment);
                    if (!noteAttachmentModel.IsSuccess)
                    {
                        return log.ErrorAndReturnModel<NoteAttachmentModel>(noteAttachmentModel);
                    }
                }
                else
                {
                    noteAttachmentModel = await CreateNoteAttachmentAsync(noteAttachment);
                    if (!noteAttachmentModel.IsSuccess)
                    {
                        return log.ErrorAndReturnModel<NoteAttachmentModel>(noteAttachmentModel);
                    }
                }

                return noteAttachmentModel;
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<NoteAttachmentModel>($"Unexpected error when creating NoteAttachment", new { noteAttachment?.NoteAttachmentId }, ex);
            }
        }

        public async Task<ReturnModel<NoteAttachmentModel>> UpdateNoteAttachmentAsync(NoteAttachmentUpdateModel noteAttachment)
        {
            try
            {
                if (noteAttachment == null)
                {
                    return log.ErrorAndReturnModel<NoteAttachmentModel>($"Unable to update NoteAttachment", new ReturnErrorItem("", "NoteAttachment information is not valid"));
                }

                var existingNoteAttachmentModel = await GetNoteAttachmentByIdForWriteAsync(noteAttachment.NoteAttachmentId);
                if (!existingNoteAttachmentModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<NoteAttachmentModel>(existingNoteAttachmentModel);
                }

                noteAttachment.CreatedByUserId = existingNoteAttachmentModel.Model.CreatedByUserId;
                noteAttachment.CreatedDate = existingNoteAttachmentModel.Model.CreatedDate;
                noteAttachment.IsDeleted = existingNoteAttachmentModel.Model.IsDeleted;
                noteAttachment.UserId = existingNoteAttachmentModel.Model.UserId;

                var errorList = await IsNoteAttachmentModelValidForUpdateAsync(noteAttachment);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<NoteAttachmentModel>($"Unable to update noteAttachment", errorList);
                }

                var item = objectMapperService.NoteAttachmentUpdateModelToNoteAttachment(noteAttachment);

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.UpdateNoteAttachmentAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update noteAttachment");
                    }

                    await trx.CompleteAsync();
                }

                await auditService.NoteAttachmentUpdatedAsync(noteAttachment.NoteAttachmentId, existingNoteAttachmentModel.Model, item);

                var view = await movieRepository.GetNoteAttachmentViewByIdAsync(noteAttachment.NoteAttachmentId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<NoteAttachmentModel>($"Unable to find the noteAttachment", new { noteAttachment.NoteAttachmentId });
                }

                return this.ConvertToItem(view, objectMapperService.NoteAttachmentViewToNoteAttachmentModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<NoteAttachmentModel>($"Unexpected error when updating NoteAttachment", new { noteAttachment?.NoteAttachmentId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkNoteAttachmentAsNotDeletedByIdAsync(long noteAttachmentId)
        {
            try
            {
                var existingNoteAttachmentModel = await GetNoteAttachmentByIdForWriteAsync(noteAttachmentId);
                if (!existingNoteAttachmentModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingNoteAttachmentModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.MarkNoteAttachmentAsNotDeletedByIdAsync(noteAttachmentId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.NoteAttachmentUnDeletedAsync(noteAttachmentId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking noteAttachment as not deleted", new { noteAttachmentId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkNoteAttachmentAsDeletedByIdAsync(long noteAttachmentId)
        {
            try
            {
                var existingNoteAttachmentModel = await GetNoteAttachmentByIdForWriteAsync(noteAttachmentId);
                if (!existingNoteAttachmentModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingNoteAttachmentModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.MarkNoteAttachmentAsDeletedByIdAsync(noteAttachmentId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.NoteAttachmentDeletedAsync(noteAttachmentId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking noteAttachment as deleted", new { noteAttachmentId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteNoteAttachmentByAsync(long noteAttachmentId)
        {
            try
            {
                var existingNoteAttachmentModel = await GetNoteAttachmentByIdForWriteAsync(noteAttachmentId);
                if (!existingNoteAttachmentModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingNoteAttachmentModel);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.HardDeleteNoteAttachmentByIdAsync(noteAttachmentId);
                    await trx.CompleteAsync();
                }

                await auditService.HardDeleteNoteAttachmentAsync(noteAttachmentId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting noteAttachment", new { noteAttachmentId }, ex);
            }
        }

        public Task<List<ReturnErrorItem>> IsNoteAttachmentModelValidForCreateAsync(NoteAttachmentUpdateModel noteAttachmentModel)
        {
            return IsNoteAttachmentModelValidForAllAsync(true, noteAttachmentModel);
        }

        public Task<List<ReturnErrorItem>> IsNoteAttachmentModelValidForUpdateAsync(NoteAttachmentUpdateModel noteAttachmentModel)
        {
            return IsNoteAttachmentModelValidForAllAsync(false, noteAttachmentModel);
        }

        public async Task<List<ReturnErrorItem>> IsNoteAttachmentModelValidForAllAsync(bool isCreate, NoteAttachmentUpdateModel noteAttachmentModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (noteAttachmentModel.NoteAttachmentId > 0)
                {
                    result.Add(new ReturnErrorItem("", "Note Attachment Identifier is not valid"));
                }
            }
            else
            {
                if (noteAttachmentModel.NoteAttachmentId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "Note Attachment Unique Identifier is not valid"));
                }
            }

            /*if (noteAttachmentModel.MovieId != null && noteAttachmentModel.MovieId > 0)
            {
                var movieItem = await movieRepository.GetMovieViewByIdAsync(noteAttachmentModel.MovieId ?? 0);
                if (movieItem == null)
                {
                    result.Add(new ReturnErrorItem("", $"Unable to Get the Movie Information"));
                    return result;
                }

                if (movieItem.CompanyId != noteAttachmentModel.CompanyId)
                {
                    result.Add(new ReturnErrorItem("", $"You don't have access to this Movie Information"));
                    return result;
                }
            }
            */

            var userItem = await companyRepository.GetUserViewByIdAsync(noteAttachmentModel.UserId);
            if (userItem == null)
            {
                result.Add(new ReturnErrorItem("", $"Unable to Get the User Information"));
                return result;
            }

            /*if (noteAttachmentModel.MovieBudgetCategoryId > 0)
            {
                var movieBudgetCategoryItem = await movieRepository.GetMovieBudgetCategoryViewByIdAsync(noteAttachmentModel.MovieBudgetCategoryId ?? 0);
                if (movieBudgetCategoryItem == null)
                {
                    result.Add(new ReturnErrorItem("", $"Unable to Get the Movie Budget Category Information"));
                    return result;
                }

                if (movieBudgetCategoryItem.CompanyId != noteAttachmentModel.CompanyId)
                {
                    result.Add(new ReturnErrorItem("", $"You dont have access to this Movie User Information"));
                    return result;
                }
            }*/

            if (noteAttachmentModel.Title != null && noteAttachmentModel.Title.Length > NoteAttachmentModel.COLUMN_SIZE_Title)
            {
                result.Add(new ReturnErrorItem("Title", $"Title size should not exceed {NoteAttachmentModel.COLUMN_SIZE_Title}"));
            }

            if (noteAttachmentModel.Title == null)
            {
                result.Add(new ReturnErrorItem("Title", "Title is required "));
            }

            if (noteAttachmentModel.Description != null && noteAttachmentModel.Description.Length > NoteAttachmentModel.COLUMN_SIZE_Description)
            {
                result.Add(new ReturnErrorItem("Description", $"Description size should not exceed {NoteAttachmentModel.COLUMN_SIZE_Description}"));
            }

            if (noteAttachmentModel.Description == null)
            {
                result.Add(new ReturnErrorItem("Description", "Description is required "));
            }

            if (!ValidationUtils.IsLongValid(noteAttachmentModel.UserId))
            {
                result.Add(new ReturnErrorItem("UserId", "User cannot be empty"));
            }

            return result.Count > 0 ? result : null;
        }

        /*
        public async Task<ReturnModel<bool>> IsNoteAttachmentWordOrPdfFileValidAsync(long noteAttachmentId, bool isPdf)
        {
            var tName = isPdf ? "preview" : "source";

            try
            {
                var fileModel = await GetNoteAttachmentWordOrPdfFileAsync(noteAttachmentId,isPdf);
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

        public async Task<ReturnModel<FileStreamModel>> GetNoteAttachmentWordOrdPdfFileAsync(long noteAttachmentId, bool isPdf)
        {
            try
            {
                var view = await GetNoteAttachmentByIdForReadAsync(noteAttachmentId);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the NoteAttachment", new { noteAttachmentId });
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

                if (!await contractFileStorage.ExistsAsync(view.Model.TenantId, fileName))
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: {docType} file is not available.");
                }

                var result = new FileStreamModel
                {
                    FileName = view.Model.ActualFileName.IsTrimmedStringNullOrEmpty() ? $"NoteAttachmentFile.{docType}" : view.Model.ActualFileName,
                    ContentType = FileUtils.GetMimeTypeFromFileName(view.Model.ActualFileName),
                    FileData = await wipFileStorage.OpenReadAsync(view.Model.TenantId, fileName)
                };

                return ReturnModel<FileStreamModel>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for NoteAttachment ", new { noteAttachmentId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> IsNoteAttachmentFileValidAsync(long noteAttachmentId)
        {
            try
            {
                var fileModel = await GetNoteAttachmentFileAsync(noteAttachmentId);
                if (!fileModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the NoteAttachment file", fileModel.ErrorHolder);
                }

                FileUtils.CloseSafely(fileModel.Model.FileData);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the NoteAttachment file", ex);
            }
        }

        public async Task<ReturnModel<FileStreamModel>> GetNoteAttachmentFileAsync(long noteAttachmentId)
        {
            try
            {
                var view = await GetNoteAttachmentByIdForReadAsync(noteAttachmentId);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the NoteAttachment", new { noteAttachmentId });
                }

                var fileName = view.Model.SourceStoredFilePath;

                if (fileName.IsTrimmedStringNullOrEmpty())
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: file is not available.");
                }

                if (!await wipFileStorage.ExistsAsync(view.Model.TenantId, fileName))
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: file is not available.");
                }

                var actualFileName = view.Model.ActualFileName.IsTrimmedStringNullOrEmpty() ? $"NoteAttachmentFile" : view.Model.ActualFileName;

                var result = new FileStreamModel
                {
                    FileName = actualFileName,
                    ContentType = FileUtils.GetMimeTypeFromFileName(actualFileName),
                    FileData = await wipFileStorage.OpenReadAsync(view.Model.TenantId, fileName)
                };

                return ReturnModel<FileStreamModel>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for NoteAttachment ", new { noteAttachmentId }, ex);
            }
        }

        public async Task CreateOrUpdateNoteAttachmentDependentsAsync(NoteAttachmentModel model)
        {
            if (model.NewNoteAttachmentXYZZYXModels == null)
                model.NewNoteAttachmentXYZZYXModels = new List<NoteAttachmentXYZZYXCreateModel>();

            if (model.ExistingNoteAttachmentXYZZYXModels == null)
                model.ExistingNoteAttachmentXYZZYXModels = new List<NoteAttachmentXYZZYXUpdateModel>();

            var dbExistingNoteAttachments = await movieRepository.GetNoteAttachmentXYZZYXsByNoteAttachmentIdAsync(model.NoteAttachmentId);
            if (!dbExistingNoteAttachments.IsNullOrEmpty())
            {
                var dbExistingNoteAttachmentIds = model.ExistingNoteAttachmentXYZZYXModels.Result.SelectToList(x => x.NoteAttachmentXYZZYXId);

                var xYZZYXItemsToDelete = model.ExistingNoteAttachmentXYZZYXModels.Where(x => !dbExistingNoteAttachmentIds.Contains(x.NoteAttachmentXYZZYXId));
                foreach (var x in xYZZYXItemsToDelete)
                {
                    await movieRepository.HardDeleteNoteAttachmentXYZZYXByIdAsync(x.NoteAttachmentXYZZYXId);
                }

                var xYZZYXItemsToUpdate = model.ExistingNoteAttachmentXYZZYXModels.Where(x => dbExistingNoteAttachmentIds.Contains(x.NoteAttachmentXYZZYXId));
                foreach (var x in xYZZYXItemsToUpdate)
                {
                    x.NoteAttachmentId = model.NoteAttachmentId;
                    var result = await UpdateNoteAttachmentXYZZYXAsync(x);
                    if (!result.IsSuccess)
                    {
                        throw new RollbackException(result);
                    }
                }
            }

            foreach (var x in model.NewNoteAttachmentXYZZYXModels)
            {
                x.NoteAttachmentId = model.NoteAttachmentId;
                var result = await CreateNoteAttachmentXYZZYXAsync(x);
                if (!result.IsSuccess)
                {
                    throw new RollbackException(result);
                }
            }
        }

        public async Task<ReturnModel<bool>> EnableNoteAttachmentByIdAsync(long noteAttachmentId)
        {
            try
            {
                var existingNoteAttachmentModel = await GetNoteAttachmentByIdForWriteAsync(noteAttachmentId);
                if (!existingNoteAttachmentModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<NoteAttachmentModel>(existingNoteAttachmentModel);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(existingNoteAttachmentModel.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.EnableNoteAttachmentByIdAsync(existingNoteAttachmentModel.Model.NoteAttachmentId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.NoteAttachmentEnabledAsync(noteAttachmentId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking noteAttachment as deleted", new { noteAttachmentId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> DisableNoteAttachmentByIdAsync(long noteAttachmentId)
        {
            try
            {
                var existingNoteAttachmentModel = await GetNoteAttachmentByIdForWriteAsync(noteAttachmentId);
                if (!existingNoteAttachmentModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<NoteAttachmentModel>(existingNoteAttachmentModel);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(existingNoteAttachmentModel.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.DisableNoteAttachmentByIdAsync(existingNoteAttachmentModel.Model.NoteAttachmentId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.NoteAttachmentDisabledAsync(existingNoteAttachmentModel.Model.NoteAttachmentId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting noteAttachment",new { noteAttachmentId }, ex);
            }
        }


        public async Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByNoteAttachmentAsync(long noteAttachmentId)
        {
            try
            {
                var contract = await movieRepository.GetNoteAttachmentByIdAsync(noteAttachmentId);
                if (contract == null)
                {
                    return log.ErrorAndReturnListModel<NoteAttachmentModel>($"Unable to find NoteAttachment", new { noteAttachmentId });
                }
                return this.ConvertToList(await movieRepository.GetXYZZYXsByNoteAttachmentsAsync(contract.NoteAttachmentId), objectMapperService.NoteAttachmentViewListToNoteAttachmentModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<NoteAttachmentModel>($"Unexpected error when getting Related NoteAttachments of the NoteAttachment");
            }
        }

        public async Task<ReturnModel<bool>> AssociateXYZZYXWithNoteAttachmentAsync(NoteAttachmentXYZZYXAssociationModel model)
        {
            try
            {
                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(await currentSessionService.GetCurrentCompanyIdAsync());

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                var errorLst = await IsNoteAttachmentWithNoteAttachmentAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("NoteAttachment association is invalid", errorLst);
                }

                if (await movieRepository.DoesNoteAttachmentWithXYZZYXAssociationExistsAsync(model.NoteAttachmentId, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related NoteAttachment is already associated with the NoteAttachment", new { model.NoteAttachmentId, model.XYZZYXId});
                }

                var rValue = await movieRepository.AssociateNoteAttachmentWithXYZZYXAsync(model.NoteAttachmentId, model.XYZZYXId, await currentSessionService.GetCurrentUserIdAsync());
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }
                return log.ErrorAndReturnModel<bool>("Unable to associate NoteAttachment with XYZZYX", new { model.NoteAttachmentId, model.XYZZYXId});
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when associating NoteAttachment with XYZZYX", new { model.NoteAttachmentId, model.XYZZYXId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> UnAssociateXYZZYXWithNoteAttachmentAsync(NoteAttachmentXYZZYXAssociationModel model)
        {
            try
            {
                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(await currentSessionService.GetCurrentCompanyIdAsync());

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                var errorLst = await IsNoteAttachmentWithXYZZYXAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("NoteAttachment association is not valid", errorLst);
                }

                if (!await movieRepository.DoesNoteAttachmentWithXYZZYXAssociationExistsAsync(model.NoteAttachmentId, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related NoteAttachment is not associated with the NoteAttachment", new { model.NoteAttachmentId, model.XYZZYXId });
                }

                var rValue = await movieRepository.UnAssociateNoteAttachmentWithNoteAttachmentAsync(model.NoteAttachmentId, model.XYZZYXId);
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }

                return log.ErrorAndReturnModel<bool>("Unable to unassociate Related NoteAttachment with the NoteAttachment", new { model.NoteAttachmentId, model.XYZZYXId });
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when unassociating Related NoteAttachment with the NoteAttachment", new { model.NoteAttachmentId, model.XYZZYXId }, ex);
            }
        }

        public async Task<List<ReturnErrorItem>> IsNoteAttachmentWithXYZZYXAssociationValidAsync(NoteAttachmentXYZZYXAssociationModel model)
        {
            var result = new List<ReturnErrorItem>();
            if (model == null)
            {
                result.Add(new ReturnErrorItem("", "Association is not valid"));
                return result;
            }

            if (model.NoteAttachmentId == 0)
            {
                result.Add(new ReturnErrorItem("", "NoteAttachment is not valid"));
                return result;
            }

            if (model.XYZZYXId == 0)
            {
                result.Add(new ReturnErrorItem("", "Related NoteAttachment  is not valid"));
                return result;
            }

            var entity = await GetNoteAttachmentForWriteByIdAsync(model.NoteAttachmentId);
            if(!entity.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "NoteAttachment information is not avialable"));
                return result;
            }

            var xYZZYX = await GetXYZZYXForWriteByIdAsync(model.XYZZYXId);
            if (!xYZZYX.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "Related XYZZYX information is not avialable"));
                return result;
            }

            if (entity.CompanyId != xYZZYX.CompanyId)
            {
                result.Add(new ReturnErrorItem("", "NoteAttachments does not belong to the same company"));
            }

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }

        public async Task<ReturnListModel<ImportStatusModel>> ImportNoteAttachmentAsync(NoteAttachmentImportModel model, bool isTest)
        {
            try
            {
                if (model == null)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("NoteAttachment Info is not provided");
                }

                if (model.NoteAttachments == null || model.NoteAttachments.Count == 0)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("NoteAttachments Info is not available to import");
                }

                var resultLst = new List<ImportStatusModel>();

                foreach (var item in model.NoteAttachments)
                {
                    var prod = await wipService.GetNoteAttachmentByExternalIdentifierAsync(item.NoteAttachmentIdentifier);
                    if (prod.Model == null)
                    {
                        //var newProd = new ProductCreateModel();
                        NoteAttachmentCreateModel noteAttachmentCreateModel = new NoteAttachmentCreateModel
                        {
                            IsEnabled = item.IsProductEnabled
                        };

                        var newlyCreatedNoteAttachment = await CreateNoteAttachmentAsync(noteAttachmentCreateModel);
                        if (!newlyCreatedNoteAttachment.IsSuccess)
                        {
                            var errorStr = string.Empty;
                            if (newlyCreatedNoteAttachment.ErrorHolder.ErrorItemList != null && newlyCreatedProd.ErrorHolder.ErrorItemList.Count > 0)
                            {
                                var errorValueList = newlyCreatedNoteAttachment.ErrorHolder.ErrorItemList.SelectToList(x => x.Value);
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
                        NoteAttachmentUpdateModel noteAttachmentUpdateModel = new NoteAttachmentUpdateModel
                        {
                        };

                        //update product
                        var updatedNoteAttachmentModel = await wipService.UpdateNoteAttachmentAsync(noteAttachmentUpdateModel);
                        if (!updatedNoteAttachmentModel.IsSuccess)
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
                return log.ErrorAndReturnListModel<ImportStatusModel>("Unexpected error occured when importing NoteAttachment Info", ex);
            }
        }

        public async Task<ReturnListModel<NoteAttachmentExportModel>> ExportNoteAttachmentAsync(NoteAttachmentExportParameter filter, bool isTest)
        {
            try
            {
                var noteAttachmentCount = await movieRepository.GetNoteAttachmentCountAsync(filter);
                if (noteAttachmentCount == 0)
                {
                    return log.ErrorAndReturnListModel<NoteAttachmentExportModel>("No NoteAttachment available for export.");
                }

                var itemsToReturn = new List<NoteAttachmentExportModel>();
                var numberOfLoops = (noteAttachmentCount / pageSize) + 1;
                for (var i = 0; i < numberOfLoops; ++i)
                {
                    var dataResult = await wipService.GetUserViewListAsync(new NoteAttachmentSearchCriteria { Page = i, PageSize = pageSize, Keyword = filter.Keyword });
                    if (dataResult.IsSuccess)
                    {
                        return log.ErrorAndReturnListModel<NoteAttachmentExportModel>(dataResult);
                    }

                    itemsToReturn.AddRange(objectMapperService.NoteAttachmentModelToNoteAttachmentExportModel(dataResult.Model));
                }

                return ReturnListModel<NoteAttachmentExportModel>.Success(itemsToReturn);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<NoteAttachmentExportModel>("Unexpected error occured when exporting NoteAttachment", ex);
            }
        }

        */
        #endregion

        #region FileAttachmentFolder
        public async Task<ReturnListModel<FileAttachmentFolderModel>> GetFileAttachmentFoldersAsync(FileAttachmentFolderSearchCriteria search)
        {
            try
            {
                return this.ConvertToList(await movieRepository.GetFileAttachmentFoldersAsync(search), objectMapperService.FileAttachmentFolderViewListToFileAttachmentFolderModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<FileAttachmentFolderModel>($"Unexpected error when getting File Attachment Folders", ex);
            }
        }

        public Task<ReturnModel<FileAttachmentFolderModel>> GetFileAttachmentFolderByIdForReadAsync(long fileAttachmentFolderId)
        {
            return GetFileAttachmentFolderByIdInternalAsync(forWrite: false, fileAttachmentFolderId);
        }

        public Task<ReturnModel<FileAttachmentFolderModel>> GetFileAttachmentFolderByIdForWriteAsync(long fileAttachmentFolderId)
        {
            return GetFileAttachmentFolderByIdInternalAsync(forWrite: true, fileAttachmentFolderId);
        }

        public async Task<ReturnModel<FileAttachmentFolderModel>> GetFileAttachmentFolderByIdInternalAsync(bool forWrite, long fileAttachmentFolderId)
        {
            try
            {
                var item = await movieRepository.GetFileAttachmentFolderViewByIdAsync(fileAttachmentFolderId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<FileAttachmentFolderModel>($"Unable to find the File Attachment Folder", new { fileAttachmentFolderId });
                }

                //var result = this.ConvertToItem(item,objectMapperService.FileAttachmentFolderViewToFileAttachmentFolderModel);
                //var childItems = await GetFileAttachmentFolderXYZZYXsAsync(new FileAttachmentFolderXYZZYXSearchCriteria { FileAttachmentFolderId = item.FileAttachmentFolderId, Page = 1, PageSize = int.MaxValue });
                //result.Model.ExistingFileAttachmentFolderXYZZYXs = childItems.Model.Where(x => x.FileAttachmentFolderId == item.FileAttachmentFolderId).ToList();
                //return result;

                return this.ConvertToItem(item, objectMapperService.FileAttachmentFolderViewToFileAttachmentFolderModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileAttachmentFolderModel>($"Unexpected error when getting File Attachment Folder", new { fileAttachmentFolderId }, ex);
            }
        }

        public async Task<ReturnListModel<FileAttachmentFolderModel>> CreateMultipleFileAttachmentFolderAsync(MultipleFileAttachmentFolderCreateModel fileAttachmentFolders)
        {
            try
            {
                if (fileAttachmentFolders == null)
                {
                    return log.ErrorAndReturnListModel<FileAttachmentFolderModel>($"Unable to Create File Attachment Folders", new ReturnErrorItem("", "File Attachment Folders information is not valid"));
                }

                if (fileAttachmentFolders.NewFileAttachmentFolders.IsNullOrEmpty())
                {
                    return log.ErrorAndReturnListModel<FileAttachmentFolderModel>($"Unable to Create File Attachment Folders", new ReturnErrorItem("", "File Attachment Folders information is not provided"));
                }

                var lst = new List<FileAttachmentFolderModel>();

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    foreach (var fileAttachmentFolder in fileAttachmentFolders.NewFileAttachmentFolders)
                    {
                        var cResult = await CreateFileAttachmentFolderAsync(fileAttachmentFolder);
                        if (!cResult.IsSuccess)
                            throw new RollbackException(cResult);

                        lst.Add(cResult.Model);
                    }

                    await trx.CompleteAsync();
                }

                return ReturnListModel<FileAttachmentFolderModel>.Success(lst);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModelWithRollbackExceptionHandler<FileAttachmentFolderModel>($"Unexpected error when creating File Attachment Folders", new { fileAttachmentFolders }, ex);
            }
        }

        public async Task<ReturnModel<FileAttachmentFolderModel>> CreateFileAttachmentFolderAsync(FileAttachmentFolderCreateModel fileAttachmentFolder)
        {
            try
            {
                if (fileAttachmentFolder == null)
                {
                    return log.ErrorAndReturnModel<FileAttachmentFolderModel>($"Unable to create File Attachment Folder", new ReturnErrorItem("", "File Attachment Folder information is not valid"));
                }

                //var companyModel = await companyService.GetCompanyByIdForReadAsync(fileAttachmentFolder.CompanyId);
                //if (!companyModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<FileAttachmentFolderModel>(companyModel);
                //}
                //
                //var movieModel = await GetMovieByIdForReadAsync(fileAttachmentFolder.MovieId);
                //if (!movieModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<FileAttachmentFolderModel>(movieModel);
                //}
                //

                /*var ignoreCode = false;
                if (fileAttachmentFolder.FileAttachmentFolderCode.IsNullOrEmpty())
                {
                    ignoreCode = true;
                    fileAttachmentFolder.FileAttachmentFolderCode = StringConstants.PrefixForCode.FileAttachmentFolder + Guid.NewGuid().ToString();
                }
                */

                fileAttachmentFolder.UserId = await currentSessionService.GetCurrentUserIdAsync() ?? 0;
                fileAttachmentFolder.IsDeleted = false;

                var errorList = await IsFileAttachmentFolderModelValidForCreateAsync(fileAttachmentFolder);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<FileAttachmentFolderModel>($"Unable to create File Attachment Folder", errorList);
                }

                var item = objectMapperService.FileAttachmentFolderCreateModelToFileAttachmentFolder(fileAttachmentFolder);

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await movieRepository.CreateFileAttachmentFolderAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create File Attachment Folder");
                    }

                    //await CreateOrUpdateFileAttachmentFolderDependentsAsync(fileAttachmentFolder)

                    /*if (ignoreCode)
                    {
                        if (!await movieRepository.UpdateFileAttachmentFolderCodeAsync(item.CompanyId, item.FileAttachmentFolderId, StringConstants.PrefixForCode.FileAttachmentFolder + item.FileAttachmentFolderId, await currentSessionService.GetCurrentUserIdAsync()))
                        {
                            throw new RollbackException($"Unable update the File Attachment Folder Code", item);
                        }
                    }*/

                    await trx.CompleteAsync();
                }

                await auditService.FileAttachmentFolderCreatedAsync(item.FileAttachmentFolderId);

                var view = await movieRepository.GetFileAttachmentFolderViewByIdAsync(item.FileAttachmentFolderId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<FileAttachmentFolderModel>($"Unable to find the File Attachment Folder", new { item.FileAttachmentFolderId });
                }

                return this.ConvertToItem(view, objectMapperService.FileAttachmentFolderViewToFileAttachmentFolderModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<FileAttachmentFolderModel>($"Unexpected error when creating File Attachment Folder", new { fileAttachmentFolder?.FileAttachmentFolderId }, ex);
            }
        }

        public async Task<ReturnModel<FileAttachmentFolderModel>> UpdateFileAttachmentFolderAsync(FileAttachmentFolderUpdateModel fileAttachmentFolder)
        {
            try
            {
                if (fileAttachmentFolder == null)
                {
                    return log.ErrorAndReturnModel<FileAttachmentFolderModel>($"Unable to update File Attachment Folder", new ReturnErrorItem("", "File Attachment Folder information is not valid"));
                }

                var existingFileAttachmentFolderModel = await GetFileAttachmentFolderByIdForWriteAsync(fileAttachmentFolder.FileAttachmentFolderId);
                if (!existingFileAttachmentFolderModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileAttachmentFolderModel>(existingFileAttachmentFolderModel);
                }

                fileAttachmentFolder.CreatedByUserId = existingFileAttachmentFolderModel.Model.CreatedByUserId;
                fileAttachmentFolder.CreatedDate = existingFileAttachmentFolderModel.Model.CreatedDate;
                fileAttachmentFolder.IsDeleted = existingFileAttachmentFolderModel.Model.IsDeleted;
                fileAttachmentFolder.UserId = existingFileAttachmentFolderModel.Model.UserId;
                //fileAttachmentFolder.MovieId = existingFileAttachmentFolderModel.Model.MovieId;
                //fileAttachmentFolder.ProfileImageActualFileName = existingFileAttachmentFolderModel.Model.ProfileImageActualFileName;
                //fileAttachmentFolder.ProfileImageLocation = existingFileAttachmentFolderModel.Model.ProfileImageLocation;

                var errorList = await IsFileAttachmentFolderModelValidForUpdateAsync(fileAttachmentFolder);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<FileAttachmentFolderModel>($"Unable to update File Attachment Folder", errorList);
                }

                var item = objectMapperService.FileAttachmentFolderUpdateModelToFileAttachmentFolder(fileAttachmentFolder);

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.UpdateFileAttachmentFolderAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update File Attachment Folder");
                    }

                    //await CreateOrUpdateFileAttachmentFolderDependentsAsync(fileAttachmentFolder)

                    await trx.CompleteAsync();
                }

                await auditService.FileAttachmentFolderUpdatedAsync(fileAttachmentFolder.FileAttachmentFolderId);

                var view = await movieRepository.GetFileAttachmentFolderViewByIdAsync(fileAttachmentFolder.FileAttachmentFolderId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<FileAttachmentFolderModel>($"Unable to find the File Attachment Folder", new { fileAttachmentFolder.FileAttachmentFolderId });
                }

                return this.ConvertToItem(view, objectMapperService.FileAttachmentFolderViewToFileAttachmentFolderModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileAttachmentFolderModel>($"Unexpected error when updating File Attachment Folder", new { fileAttachmentFolder?.FileAttachmentFolderId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkFileAttachmentFolderAsNotDeletedByIdAsync(long fileAttachmentFolderId)
        {
            try
            {
                var existingFileAttachmentFolderModel = await GetFileAttachmentFolderByIdForWriteAsync(fileAttachmentFolderId);
                if (!existingFileAttachmentFolderModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingFileAttachmentFolderModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.MarkFileAttachmentFolderAsNotDeletedByIdAsync(fileAttachmentFolderId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.FileAttachmentFolderDeletedAsync(fileAttachmentFolderId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking File Attachment Folder as not deleted", new { fileAttachmentFolderId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkFileAttachmentFolderAsDeletedByIdAsync(long fileAttachmentFolderId)
        {
            try
            {
                var existingFileAttachmentFolderModel = await GetFileAttachmentFolderByIdForWriteAsync(fileAttachmentFolderId);
                if (!existingFileAttachmentFolderModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingFileAttachmentFolderModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.MarkFileAttachmentFolderAsDeletedByIdAsync(fileAttachmentFolderId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.FileAttachmentFolderDeletedAsync(fileAttachmentFolderId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking File Attachment Folder as deleted", new { fileAttachmentFolderId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteFileAttachmentFolderByAsync(long fileAttachmentFolderId)
        {
            try
            {
                var existingFileAttachmentFolderModel = await GetFileAttachmentFolderByIdForWriteAsync(fileAttachmentFolderId);
                if (!existingFileAttachmentFolderModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingFileAttachmentFolderModel);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.HardDeleteFileAttachmentFolderByIdAsync(fileAttachmentFolderId);
                    await trx.CompleteAsync();
                }

                await auditService.HardDeleteFileAttachmentFolderAsync(fileAttachmentFolderId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting File Attachment Folder", new { fileAttachmentFolderId }, ex);
            }
        }

        public Task<List<ReturnErrorItem>> IsFileAttachmentFolderModelValidForCreateAsync(FileAttachmentFolderUpdateModel fileAttachmentFolderModel)
        {
            return IsFileAttachmentFolderModelValidForAllAsync(true, fileAttachmentFolderModel);
        }

        public Task<List<ReturnErrorItem>> IsFileAttachmentFolderModelValidForUpdateAsync(FileAttachmentFolderUpdateModel fileAttachmentFolderModel)
        {
            return IsFileAttachmentFolderModelValidForAllAsync(false, fileAttachmentFolderModel);
        }

        public async Task<List<ReturnErrorItem>> IsFileAttachmentFolderModelValidForAllAsync(bool isCreate, FileAttachmentFolderUpdateModel fileAttachmentFolderModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (await movieRepository.IsFileAttachmentFolderNameAlreadyExistsForOtherFileAttachmentFolderAsync(fileAttachmentFolderModel.FileAttachmentFolderName, fileAttachmentFolderModel.UserId))
                {
                    result.Add(new ReturnErrorItem("", "Another File Attachment Folder already exists"));
                }

                if (fileAttachmentFolderModel.FileAttachmentFolderId > 0)
                {
                    result.Add(new ReturnErrorItem("", "File Attachment Folder Identifier is not valid"));
                }
            }
            else
            {
                if (await movieRepository.IsFileAttachmentFolderNameAlreadyExistsForOtherFileAttachmentFolderAsync(fileAttachmentFolderModel.FileAttachmentFolderId, fileAttachmentFolderModel.FileAttachmentFolderName, fileAttachmentFolderModel.UserId))
                {
                    result.Add(new ReturnErrorItem("", "Another File Attachment Folder already exists"));
                }

                if (fileAttachmentFolderModel.FileAttachmentFolderId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "File Attachment Folder Unique Identifier is not valid"));
                }
            }

            if (fileAttachmentFolderModel.ParentFileAttachmentFolderId > 0)
            {
                var parentFileAttachmentFolderItem = await movieRepository.GetFileAttachmentFolderViewByIdAsync(fileAttachmentFolderModel.ParentFileAttachmentFolderId ?? 0);
                if (parentFileAttachmentFolderItem == null)
                {
                    result.Add(new ReturnErrorItem("", $"Unable to Get the ParentFileAttachmentFolder Information"));
                    return result;
                }
            }

            /*
            if (fileAttachmentFolderModel.MovieId > 0)
            {
                var movieItem = await movieRepository.GetMovieViewByIdAsync(fileAttachmentFolderModel.MovieId ?? 0);
                if (movieItem == null)
                {
                    result.Add(new ReturnErrorItem("", $"Unable to Get the Movie Information"));
                    return result;
                }

                if (movieItem.CompanyId != fileAttachmentFolderModel.CompanyId)
                {
                    result.Add(new ReturnErrorItem("", $"You dont have access to this Movie Information"));
                    return result;
                }
            }*/

            var userItem = await companyRepository.GetUserViewByIdAsync(fileAttachmentFolderModel.UserId);
            if (userItem == null)
            {
                result.Add(new ReturnErrorItem("", $"Unable to Get the User Information"));
                return result;
            }

            if (fileAttachmentFolderModel.FileAttachmentFolderName != null && fileAttachmentFolderModel.FileAttachmentFolderName.Length > FileAttachmentFolderModel.COLUMN_SIZE_FileAttachmentFolderName)
            {
                result.Add(new ReturnErrorItem("FileAttachmentFolderName", $"FileAttachmentFolderName size should not exceed {FileAttachmentFolderModel.COLUMN_SIZE_FileAttachmentFolderName}"));
            }

            if (fileAttachmentFolderModel.FileAttachmentFolderName == null)
            {
                result.Add(new ReturnErrorItem("FileAttachmentFolderName", "FileAttachmentFolderName is required "));
            }

            if (fileAttachmentFolderModel.Description != null && fileAttachmentFolderModel.Description.Length > FileAttachmentFolderModel.COLUMN_SIZE_Description)
            {
                result.Add(new ReturnErrorItem("Description", $"Description size should not exceed {FileAttachmentFolderModel.COLUMN_SIZE_Description}"));
            }

            if (!ValidationUtils.IsLongValid(fileAttachmentFolderModel.UserId))
            {
                result.Add(new ReturnErrorItem("UserId", "User cannot be empty"));
            }

            var itemForUser = await companyRepository.GetUserByIdAsync(fileAttachmentFolderModel.UserId);
            if (itemForUser == null)
            {
                result.Add(new ReturnErrorItem("UserId", "User is not available"));
            }

            return result.Count > 0 ? result : null;
        }

        /*
        public async Task<ReturnModel<bool>> IsFileAttachmentFolderWordOrPdfFileValidAsync(long fileAttachmentFolderId, bool isPdf)
        {
            var tName = isPdf ? "preview" : "source";

            try
            {
                var fileModel = await GetFileAttachmentFolderWordOrPdfFileAsync(fileAttachmentFolderId,isPdf);
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

        public async Task<ReturnModel<FileStreamModel>> GetFileAttachmentFolderWordOrdPdfFileAsync(long fileAttachmentFolderId, bool isPdf)
        {
            try
            {
                var view = await GetFileAttachmentFolderByIdForReadAsync(fileAttachmentFolderId);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the File Attachment Folder", new { fileAttachmentFolderId });
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
                    FileName = view.Model.ActualFileName.IsTrimmedStringNullOrEmpty() ? $"FileAttachmentFolderFile.{docType}" : view.Model.ActualFileName,
                    ContentType = FileUtils.GetMimeTypeFromFileName(view.Model.ActualFileName),
                    FileData = await wipFileStorage.OpenReadAsync(view.Model.CompanyId, fileName)
                };

                return ReturnModel<FileStreamModel>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for File Attachment Folder ", new { fileAttachmentFolderId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> IsFileAttachmentFolderFileValidAsync(long fileAttachmentFolderId)
        {
            try
            {
                var fileModel = await GetFileAttachmentFolderFileAsync(fileAttachmentFolderId);
                if (!fileModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the File Attachment Folder file", fileModel.ErrorHolder);
                }

                FileUtils.CloseSafely(fileModel.Model.FileData);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the File Attachment Folder file", ex);
            }
        }

        public async Task<ReturnModel<FileStreamModel>> GetFileAttachmentFolderFileAsync(long fileAttachmentFolderId)
        {
            try
            {
                var view = await GetFileAttachmentFolderByIdForReadAsync(fileAttachmentFolderId);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the File Attachment Folder", new { fileAttachmentFolderId });
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

                var actualFileName = view.Model.ActualFileName.IsTrimmedStringNullOrEmpty() ? $"FileAttachmentFolderFile" : view.Model.ActualFileName;

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
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for File Attachment Folder ", new { fileAttachmentFolderId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> UploadFileAttachmentFolderFileAsync(long fileAttachmentFolderId, FileStreamModel streamModel)
        {
            try
            {
                var fileAttachmentFolder = await GetFileAttachmentFolderByIdForWriteAsync(fileAttachmentFolderId);
                if (!fileAttachmentFolder.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(fileAttachmentFolder);
                }

                if (streamModel == null)
                {
                    return log.ErrorAndReturnModel<bool>("File Content is not provided");
                }

                var company = await companyRepository.GetCompanyByIdAsync(fileAttachmentFolder.Model.CompanyId);
                if (company == null)
                {
                    return log.ErrorAndReturnModel<bool>("Unable to find the Company Information");
                }

                try
                {
                    using (var trx = await transactionManager.BeginTransactionAsync())
                    {
                        var path = profileFileFileStorage.ProfileFilePathForHostBrand(fileAttachmentFolder.Model.CompanyId, fileAttachmentFolder.Model.FileAttachmentFolderId, streamModel.FileName);
                        if (company.PredefinedCompanyTypeId == (int)PredefinedCompanyTypes.MarketPlace)
                            path = profileFileFileStorage.ProfileFilePathForMarketplaceBrand(fileAttachmentFolder.Model.CompanyId, fileAttachmentFolder.Model.FileAttachmentFolderId, streamModel.FileName);
                        await productRepository.UpdateFileAttachmentFolderFilePathAsync(fileAttachmentFolder.Model.FileAttachmentFolderId, path, streamModel.FileName, await currentSessionService.GetCurrentUserIdAsync());
                        await profileFileFileStorage.WriteAsync(fileAttachmentFolder.Model.CompanyId, path, streamModel.FileData);
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
                return log.ErrorAndReturnModel<bool>($"Unexpected error when Uploading File Attachment Folder File", ex);
            }
        }

        public async Task<ReturnModel<bool>> ClearFileAttachmentFolderFileAsync(long fileAttachmentFolderId)
        {
            try
            {
                var fileAttachmentFolder = await GetFileAttachmentFolderByIdForWriteAsync(fileAttachmentFolderId);
                if (!fileAttachmentFolder.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(fileAttachmentFolder);
                }

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    await productRepository.ClearFileAttachmentFolderFilePathAsync(fileAttachmentFolder.Model.FileAttachmentFolderId, await currentSessionService.GetCurrentUserIdAsync());
                    await profileFileFileStorage.DeleteAsync(fileAttachmentFolder.Model.CompanyId, fileAttachmentFolder.Model.ProfileImageLocation);
                }

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when clearing File Attachment Folder File", ex);
            }
        }

        public async Task CreateOrUpdateFileAttachmentFolderDependentsAsync(FileAttachmentFolderUpdateModel model)
        {
            if (model.NewFileAttachmentFolderXYZZYXModels == null)
                model.NewFileAttachmentFolderXYZZYXModels = new List<FileAttachmentFolderXYZZYXCreateModel>();

            if (model.ExistingFileAttachmentFolderXYZZYXModels == null)
                model.ExistingFileAttachmentFolderXYZZYXModels = new List<FileAttachmentFolderXYZZYXUpdateModel>();

            var dbExistingFileAttachmentFolders = await movieRepository.GetFileAttachmentFolderXYZZYXsByFileAttachmentFolderIdAsync(model.FileAttachmentFolderId);
            if (!dbExistingFileAttachmentFolders.IsNullOrEmpty())
            {
                var dbExistingFileAttachmentFolderIds = dbExistingFileAttachmentFolders.SelectToList(x => x.FileAttachmentFolderXYZZYXId);
                var modelExistingFileAttachmentFolderIds = model.ExistingFileAttachmentFolderXYZZYXModels.SelectToList(x => x.FileAttachmentFolderXYZZYXId);

                var xYZZYXItemsToDelete = dbExistingFileAttachmentFolders.Where(x => !modelExistingFileAttachmentFolderIds.Contains(x.FileAttachmentFolderXYZZYXId));
                foreach (var x in xYZZYXItemsToDelete)
                {
                    var result = await MarkFileAttachmentFolderXYZZYXAsDeletedByIdAsync(x.FileAttachmentFolderXYZZYXId);
                    if (!result.IsSuccess)
                    {
                        throw new RollbackException(result);
                    }
                }

                var xYZZYXItemsToUpdate = model.ExistingFileAttachmentFolderXYZZYXModels.Where(x => dbExistingFileAttachmentFolderIds.Contains(x.FileAttachmentFolderXYZZYXId));
                foreach (var x in xYZZYXItemsToUpdate)
                {
                    x.FileAttachmentFolderId = model.FileAttachmentFolderId;
                    var result = await UpdateFileAttachmentFolderXYZZYXAsync(x);
                    if (!result.IsSuccess)
                    {
                        throw new RollbackException(result);
                    }
                }
            }

            foreach (var x in model.NewFileAttachmentFolderXYZZYXModels)
            {
                x.FileAttachmentFolderId = model.FileAttachmentFolderId;
                var result = await CreateFileAttachmentFolderXYZZYXAsync(x);
                if (!result.IsSuccess)
                {
                    throw new RollbackException(result);
                }
            }
        }

        public async Task<ReturnModel<bool>> EnableFileAttachmentFolderByIdAsync(long fileAttachmentFolderId)
        {
            try
            {
                var existingFileAttachmentFolderModel = await GetFileAttachmentFolderByIdForWriteAsync(fileAttachmentFolderId);
                if (!existingFileAttachmentFolderModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileAttachmentFolderModel>(existingFileAttachmentFolderModel);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(existingFileAttachmentFolderModel.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.EnableFileAttachmentFolderByIdAsync(existingFileAttachmentFolderModel.Model.FileAttachmentFolderId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.FileAttachmentFolderEnabledAsync(fileAttachmentFolderId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when enabling File Attachment Folder", new { fileAttachmentFolderId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> DisableFileAttachmentFolderByIdAsync(long fileAttachmentFolderId)
        {
            try
            {
                var existingFileAttachmentFolderModel = await GetFileAttachmentFolderByIdForWriteAsync(fileAttachmentFolderId);
                if (!existingFileAttachmentFolderModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileAttachmentFolderModel>(existingFileAttachmentFolderModel);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(existingFileAttachmentFolderModel.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.DisableFileAttachmentFolderByIdAsync(existingFileAttachmentFolderModel.Model.FileAttachmentFolderId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.FileAttachmentFolderDisabledAsync(existingFileAttachmentFolderModel.Model.FileAttachmentFolderId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when disabling File Attachment Folder",new { fileAttachmentFolderId }, ex);
            }
        }


        public async Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByFileAttachmentFolderAsync(long fileAttachmentFolderId)
        {
            try
            {
                var contract = await movieRepository.GetFileAttachmentFolderByIdAsync(fileAttachmentFolderId);
                if (contract == null)
                {
                    return log.ErrorAndReturnListModel<FileAttachmentFolderModel>($"Unable to find File Attachment Folder", new { fileAttachmentFolderId });
                }
                return this.ConvertToList(await movieRepository.GetXYZZYXsByFileAttachmentFoldersAsync(contract.FileAttachmentFolderId), objectMapperService.FileAttachmentFolderViewListToFileAttachmentFolderModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<FileAttachmentFolderModel>($"Unexpected error when getting Related File Attachment Folders of the File Attachment Folder");
            }
        }

        public async Task<ReturnModel<bool>> AssociateXYZZYXWithFileAttachmentFolderAsync(FileAttachmentFolderXYZZYXAssociationModel model)
        {
            try
            {
                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(await currentSessionService.GetCurrentCompanyIdAsync());

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                var errorLst = await IsFileAttachmentFolderWithFileAttachmentFolderAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("File Attachment Folder association is invalid", errorLst);
                }

                if (await movieRepository.DoesFileAttachmentFolderWithXYZZYXAssociationExistsAsync(model.FileAttachmentFolderId, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related File Attachment Folder is already associated with the File Attachment Folder", new { model.FileAttachmentFolderId, model.XYZZYXId});
                }

                var rValue = await movieRepository.AssociateFileAttachmentFolderWithXYZZYXAsync(model.FileAttachmentFolderId, model.XYZZYXId, await currentSessionService.GetCurrentUserIdAsync());
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }
                return log.ErrorAndReturnModel<bool>("Unable to associate File Attachment Folder with XYZZYX", new { model.FileAttachmentFolderId, model.XYZZYXId});
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when associating File Attachment Folder with XYZZYX", new { model.FileAttachmentFolderId, model.XYZZYXId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> UnAssociateXYZZYXWithFileAttachmentFolderAsync(FileAttachmentFolderXYZZYXAssociationModel model)
        {
            try
            {
                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(await currentSessionService.GetCurrentCompanyIdAsync());

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                var errorLst = await IsFileAttachmentFolderWithXYZZYXAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("File Attachment Folder association is not valid", errorLst);
                }

                if (!await movieRepository.DoesFileAttachmentFolderWithXYZZYXAssociationExistsAsync(model.FileAttachmentFolderId, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related File Attachment Folder is not associated with the File Attachment Folder", new { model.FileAttachmentFolderId, model.XYZZYXId });
                }

                var rValue = await movieRepository.UnAssociateFileAttachmentFolderWithFileAttachmentFolderAsync(model.FileAttachmentFolderId, model.XYZZYXId);
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }

                return log.ErrorAndReturnModel<bool>("Unable to unassociate Related File Attachment Folder with the File Attachment Folder", new { model.FileAttachmentFolderId, model.XYZZYXId });
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when unassociating Related File Attachment Folder with the File Attachment Folder", new { model.FileAttachmentFolderId, model.XYZZYXId }, ex);
            }
        }

        public async Task<List<ReturnErrorItem>> IsFileAttachmentFolderWithXYZZYXAssociationValidAsync(FileAttachmentFolderXYZZYXAssociationModel model)
        {
            var result = new List<ReturnErrorItem>();
            if (model == null)
            {
                result.Add(new ReturnErrorItem("", "Association is not valid"));
                return result;
            }

            if (model.FileAttachmentFolderId == 0)
            {
                result.Add(new ReturnErrorItem("", "File Attachment Folder is not valid"));
                return result;
            }

            if (model.XYZZYXId == 0)
            {
                result.Add(new ReturnErrorItem("", "Related File Attachment Folder  is not valid"));
                return result;
            }

            var entity = await GetFileAttachmentFolderForWriteByIdAsync(model.FileAttachmentFolderId);
            if(!entity.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "File Attachment Folder information is not available"));
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
                result.Add(new ReturnErrorItem("", "File Attachment Folders does not belong to the same company"));
            }

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }

        public async Task<ReturnListModel<ImportStatusModel>> ImportFileAttachmentFolderAsync(FileAttachmentFolderImportModel model, bool isTest)
        {
            try
            {
                if (model == null)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("File Attachment Folder Import Info is not provided");
                }

                if (model.FileAttachmentFolders == null || model.FileAttachmentFolders.Count == 0)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("File Attachment Folders Info is not available to import");
                }

                var resultLst = new List<ImportStatusModel>();

                foreach (var item in model.FileAttachmentFolders)
                {
                    var prod = await wipService.GetFileAttachmentFolderByExternalIdentifierAsync(item.FileAttachmentFolderIdentifier);
                    if (prod.Model == null)
                    {
                        //var newProd = new ProductCreateModel();
                        FileAttachmentFolderCreateModel fileAttachmentFolderCreateModel = new FileAttachmentFolderCreateModel
                        {
                            IsEnabled = item.IsProductEnabled
                        };

                        var newlyCreatedFileAttachmentFolder = await CreateFileAttachmentFolderAsync(fileAttachmentFolderCreateModel);
                        if (!newlyCreatedFileAttachmentFolder.IsSuccess)
                        {
                            var errorStr = string.Empty;
                            if (newlyCreatedFileAttachmentFolder.ErrorHolder.ErrorItemList != null && newlyCreatedProd.ErrorHolder.ErrorItemList.Count > 0)
                            {
                                var errorValueList = newlyCreatedFileAttachmentFolder.ErrorHolder.ErrorItemList.SelectToList(x => x.Value);
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
                        FileAttachmentFolderUpdateModel fileAttachmentFolderUpdateModel = new FileAttachmentFolderUpdateModel
                        {
                        };

                        //update product
                        var updatedFileAttachmentFolderModel = await wipService.UpdateFileAttachmentFolderAsync(fileAttachmentFolderUpdateModel);
                        if (!updatedFileAttachmentFolderModel.IsSuccess)
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
                return log.ErrorAndReturnListModel<ImportStatusModel>("Unexpected error occurred when importing File Attachment Folder Info", ex);
            }
        }

        public async Task<ReturnListModel<FileAttachmentFolderExportModel>> ExportFileAttachmentFolderAsync(FileAttachmentFolderExportParameter filter, bool isTest)
        {
            try
            {
                var fileAttachmentFolderCount = await movieRepository.GetFileAttachmentFolderCountAsync(filter);
                if (fileAttachmentFolderCount == 0)
                {
                    return log.ErrorAndReturnListModel<FileAttachmentFolderExportModel>("No File Attachment Folder available for export.");
                }

                var itemsToReturn = new List<FileAttachmentFolderExportModel>();
                var numberOfLoops = (fileAttachmentFolderCount / pageSize) + 1;
                for (var i = 0; i < numberOfLoops; ++i)
                {
                    var dataResult = await wipService.GetUserViewListAsync(new FileAttachmentFolderSearchCriteria { Page = i, PageSize = pageSize, Keyword = filter.Keyword });
                    if (dataResult.IsSuccess)
                    {
                        return log.ErrorAndReturnListModel<FileAttachmentFolderExportModel>(dataResult);
                    }

                    itemsToReturn.AddRange(objectMapperService.FileAttachmentFolderModelListToFileAttachmentFolderExportModelList(dataResult.Model));
                }

                return ReturnListModel<FileAttachmentFolderExportModel>.Success(itemsToReturn);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<FileAttachmentFolderExportModel>("Unexpected error occurred when exporting File Attachment Folder", ex);
            }
        }

        */
        #endregion

        #region FileAttachment

        public async Task<ReturnListModel<FileAttachmentModel>> GetFileAttachmentsAsync(FileAttachmentSearchCriteria search)
        {
            try
            {
                return this.ConvertToList(await movieRepository.GetFileAttachmentsAsync(search), objectMapperService.FileAttachmentViewListToFileAttachmentModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<FileAttachmentModel>($"Unexpected error when getting File Attachments", ex);
            }
        }

        public Task<ReturnModel<FileAttachmentModel>> GetFileAttachmentByIdForReadAsync(long fileAttachmentId, bool includeDeleted = false)
        {
            return GetFileAttachmentByIdInternalAsync(forWrite: false, fileAttachmentId, includeDeleted: includeDeleted);
        }

        public Task<ReturnModel<FileAttachmentModel>> GetFileAttachmentByIdForWriteAsync(long fileAttachmentId, bool includeDeleted = false)
        {
            return GetFileAttachmentByIdInternalAsync(forWrite: true, fileAttachmentId, includeDeleted: includeDeleted);
        }

        public async Task<ReturnModel<FileAttachmentModel>> GetFileAttachmentByIdInternalAsync(bool forWrite, long fileAttachmentId, bool includeDeleted = false)
        {
            try
            {
                var item = !includeDeleted ? await movieRepository.GetFileAttachmentViewByIdAsync(fileAttachmentId) : await movieRepository.GetFileAttachmentViewFromAnyByIdAsync(fileAttachmentId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<FileAttachmentModel>($"Unable to find the File Attachment", new { fileAttachmentId });
                }

                //var existingMovieModel = forWrite ? await GetMovieByIdForWriteAsync(item.MovieId) : await GetMovieByIdForReadAsync(item.MovieId);
                //if (!existingMovieModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<CompanyModel>(existingMovieModel);
                //}

                return this.ConvertToItem(item, objectMapperService.FileAttachmentViewToFileAttachmentModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileAttachmentModel>($"Unexpected error when getting File Attachment", new { fileAttachmentId }, ex);
            }
        }

        public async Task<ReturnModel<FileAttachmentModel>> CreateFileAttachmentAsync(FileAttachmentCreateModel fileAttachment, FileStreamModel fsModel)
        {
            try
            {
                if (fileAttachment == null)
                {
                    return log.ErrorAndReturnModel<FileAttachmentModel>($"Unable to create File Attachment", new ReturnErrorItem("", "File Attachment information is not valid"));
                }

                Stream fileStream = null;

                if (fsModel != null && fsModel.FileData != null && fsModel.FileData.Length > 0)
                {
                    if (fsModel.FileName.IsTrimmedStringNullOrEmpty())
                    {
                        return ReturnModel<FileAttachmentModel>.Error($"Uploaded file name is not available. Please provide a valid file document.");
                    }

                    fileStream = fsModel.FileData;
                }

                if (fileStream == null)
                {
                    return ReturnModel<FileAttachmentModel>.Error($"Template File should not be empty. Please upload valid Template file.");
                }

                fileAttachment.UserId = await currentSessionService.GetCurrentUserIdAsync() ?? 0;
                fileAttachment.FileAttachmentFileLocation = fsModel.FileName;
                fileAttachment.FileAttachmentFileName = fsModel.FileName;
                fileAttachment.FileAttachmentFileSizeInBytes = fileStream.Length;

                var errorList = await IsFileAttachmentModelValidForCreateAsync(fileAttachment);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<FileAttachmentModel>($"Unable to create File Attachment", errorList);
                }

                fileAttachment.IsDeleted = false;

                var item = objectMapperService.FileAttachmentCreateModelToFileAttachment(fileAttachment);

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await movieRepository.CreateFileAttachmentAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create File Attachment");
                    }

                    await trx.CompleteAsync();
                }

                await auditService.FileAttachmentCreatedAsync(item.FileAttachmentId);

                var view = await movieRepository.GetFileAttachmentViewByIdAsync(item.FileAttachmentId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<FileAttachmentModel>($"Unable to find the File Attachment", new { item.FileAttachmentId });
                }

                fileStream.Position = 0;
                var fName = fileAttachmentFileStorage.FileAttachmentFilePath(0.ToString(), item.FileAttachmentId.ToString(), item.FileAttachmentFileName);
                await fileAttachmentFileStorage.WriteAsync(0, fName, fileStream);
                await movieRepository.UpdateFileAttachmentFileLocationAsync(item.FileAttachmentId, fName, item.FileAttachmentFileName, await currentSessionService.GetCurrentUserIdAsync());
                view.FileAttachmentFileLocation = fName;
                return this.ConvertToItem(view, objectMapperService.FileAttachmentViewToFileAttachmentModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<FileAttachmentModel>($"Unexpected error when creating File Attachment", new { fileAttachment?.FileAttachmentId }, ex);
            }
        }

        public async Task<ReturnModel<FileAttachmentModel>> UpdateFileAttachmentAsync(FileAttachmentUpdateModel fileAttachment)
        {
            try
            {
                if (fileAttachment == null)
                {
                    return log.ErrorAndReturnModel<FileAttachmentModel>($"Unable to update File Attachment", new ReturnErrorItem("", "File Attachment information is not valid"));
                }

                var existingFileAttachmentModel = await GetFileAttachmentByIdForWriteAsync(fileAttachment.FileAttachmentId);
                if (!existingFileAttachmentModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileAttachmentModel>(existingFileAttachmentModel);
                }

                fileAttachment.CreatedByUserId = existingFileAttachmentModel.Model.CreatedByUserId;
                fileAttachment.CreatedDate = existingFileAttachmentModel.Model.CreatedDate;
                fileAttachment.IsDeleted = existingFileAttachmentModel.Model.IsDeleted;
                fileAttachment.FileAttachmentFileName = existingFileAttachmentModel.Model.FileAttachmentFileName;
                fileAttachment.FileAttachmentFileLocation = existingFileAttachmentModel.Model.FileAttachmentFileLocation;
                fileAttachment.FileAttachmentPreviewFileLocation = existingFileAttachmentModel.Model.FileAttachmentPreviewFileLocation;
                fileAttachment.FileAttachmentHasPreview = existingFileAttachmentModel.Model.FileAttachmentHasPreview;
                fileAttachment.FileAttachmentFileSizeInBytes = existingFileAttachmentModel.Model.FileAttachmentFileSizeInBytes;

                var errorList = await IsFileAttachmentModelValidForUpdateAsync(fileAttachment);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<FileAttachmentModel>($"Unable to update File Attachment", errorList);
                }

                var item = objectMapperService.FileAttachmentUpdateModelToFileAttachment(fileAttachment);

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.UpdateFileAttachmentAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update File Attachment");
                    }

                    await trx.CompleteAsync();
                }

                await auditService.FileAttachmentUpdatedAsync(fileAttachment.FileAttachmentId);

                var view = await movieRepository.GetFileAttachmentViewByIdAsync(fileAttachment.FileAttachmentId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<FileAttachmentModel>($"Unable to find the File Attachment", new { fileAttachment.FileAttachmentId });
                }

                return this.ConvertToItem(view, objectMapperService.FileAttachmentViewToFileAttachmentModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileAttachmentModel>($"Unexpected error when updating File Attachment", new { fileAttachment?.FileAttachmentId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkFileAttachmentAsNotDeletedByIdAsync(long fileAttachmentId)
        {
            try
            {
                var existingFileAttachmentModel = await GetFileAttachmentByIdForWriteAsync(fileAttachmentId);
                if (!existingFileAttachmentModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingFileAttachmentModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.MarkFileAttachmentAsNotDeletedByIdAsync(fileAttachmentId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.FileAttachmentDeletedAsync(fileAttachmentId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking File Attachment as not deleted", new { fileAttachmentId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkFileAttachmentAsDeletedByIdAsync(long fileAttachmentId)
        {
            try
            {
                var existingFileAttachmentModel = await GetFileAttachmentByIdForWriteAsync(fileAttachmentId, includeDeleted: true);
                if (!existingFileAttachmentModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingFileAttachmentModel);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.MarkFileAttachmentAsDeletedByIdAsync(fileAttachmentId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.FileAttachmentDeletedAsync(fileAttachmentId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking File Attachment as deleted", new { fileAttachmentId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteFileAttachmentByAsync(long fileAttachmentId)
        {
            try
            {
                var existingFileAttachmentModel = await GetFileAttachmentByIdForWriteAsync(fileAttachmentId);
                if (!existingFileAttachmentModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingFileAttachmentModel);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.HardDeleteFileAttachmentByIdAsync(fileAttachmentId);
                    await trx.CompleteAsync();
                }

                await auditService.HardDeleteFileAttachmentAsync(fileAttachmentId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting File Attachment", new { fileAttachmentId }, ex);
            }
        }

        public Task<List<ReturnErrorItem>> IsFileAttachmentModelValidForCreateAsync(FileAttachmentUpdateModel fileAttachmentModel)
        {
            return IsFileAttachmentModelValidForAllAsync(true, fileAttachmentModel);
        }

        public Task<List<ReturnErrorItem>> IsFileAttachmentModelValidForUpdateAsync(FileAttachmentUpdateModel fileAttachmentModel)
        {
            return IsFileAttachmentModelValidForAllAsync(false, fileAttachmentModel);
        }

        public async Task<List<ReturnErrorItem>> IsFileAttachmentModelValidForAllAsync(bool isCreate, FileAttachmentUpdateModel fileAttachmentModel)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (fileAttachmentModel.FileAttachmentId > 0)
                {
                    result.Add(new ReturnErrorItem("", "File Attachment Identifier is not valid"));
                }
            }
            else
            {
                if (fileAttachmentModel.FileAttachmentId <= 0)
                {
                    result.Add(new ReturnErrorItem("", "File Attachment Unique Identifier is not valid"));
                }
            }

            /*
            if (fileAttachmentModel.MovieId != null)
            {
                var movieItem = await movieRepository.GetMovieViewByIdAsync(fileAttachmentModel.MovieId ?? 0);
                if (movieItem == null)
                {
                    result.Add(new ReturnErrorItem("", $"Unable to Get the Movie Information"));
                    return result;
                }

                if (movieItem.CompanyId != fileAttachmentModel.CompanyId)
                {
                    result.Add(new ReturnErrorItem("", $"You dont have access to this Movie Information"));
                    return result;
                }
            }*/

            var userItem = await companyRepository.GetUserViewByIdAsync(fileAttachmentModel.UserId);
            if (userItem == null)
            {
                result.Add(new ReturnErrorItem("", $"Unable to Get the User Information"));
                return result;
            }

            /*if (fileAttachmentModel.ArtistId > 0)
            {
                var artistItem = await companyRepository.GetArtistViewByIdAsync(fileAttachmentModel.ArtistId ?? 0);
                if (artistItem == null)
                {
                    result.Add(new ReturnErrorItem("", $"Unable to Get the Artist Information"));
                    return result;
                }

                if (artistItem.CompanyId != fileAttachmentModel.CompanyId)
                {
                    result.Add(new ReturnErrorItem("", $"You dont have access to this Artist Information"));
                    return result;
                }
            }*/

            if (fileAttachmentModel.FileAttachmentFileName != null && fileAttachmentModel.FileAttachmentFileName.Length > FileAttachmentModel.COLUMN_SIZE_FileAttachmentFileName)
            {
                result.Add(new ReturnErrorItem("FileAttachmentFileName", $"FileAttachmentFileName size should not exceed {FileAttachmentModel.COLUMN_SIZE_FileAttachmentFileName}"));
            }

            if (fileAttachmentModel.FileAttachmentFileName == null)
            {
                result.Add(new ReturnErrorItem("FileAttachmentFileName", "FileAttachmentFileName is required"));
            }

            if (fileAttachmentModel.FileAttachmentFileLocation != null && fileAttachmentModel.FileAttachmentFileLocation.Length > FileAttachmentModel.COLUMN_SIZE_FileAttachmentFileLocation)
            {
                result.Add(new ReturnErrorItem("FileAttachmentFileLocation", $"FileAttachmentFileLocation size should not exceed {FileAttachmentModel.COLUMN_SIZE_FileAttachmentFileLocation}"));
            }

            if (fileAttachmentModel.FileAttachmentFileLocation == null)
            {
                result.Add(new ReturnErrorItem("FileAttachmentFileLocation", "FileAttachmentFileLocation is required"));
            }

            if (fileAttachmentModel.FileAttachmentPreviewFileLocation != null && fileAttachmentModel.FileAttachmentPreviewFileLocation.Length > FileAttachmentModel.COLUMN_SIZE_FileAttachmentPreviewFileLocation)
            {
                result.Add(new ReturnErrorItem("FileAttachmentPreviewFileLocation", $"FileAttachmentPreviewFileLocation size should not exceed {FileAttachmentModel.COLUMN_SIZE_FileAttachmentPreviewFileLocation}"));
            }

            if (fileAttachmentModel.Description != null && fileAttachmentModel.Description.Length > FileAttachmentModel.COLUMN_SIZE_Description)
            {
                result.Add(new ReturnErrorItem("Description", $"Description size should not exceed {FileAttachmentModel.COLUMN_SIZE_Description}"));
            }

            /*if (fileAttachmentModel.MovieId != null)
            {
                var itemForMovie = await movieRepository.GetMovieByIdAsync(fileAttachmentModel.MovieId ?? 0);
                if (itemForMovie == null)
                {
                    result.Add(new ReturnErrorItem("MovieId", "Movie is not available"));
                }
                else
                {
                    if (itemForMovie.CompanyId != fileAttachmentModel.CompanyId)
                    {
                        result.Add(new ReturnErrorItem("MovieId", "Movie does not belong to you."));
                    }
                }
            }*/

            if (!ValidationUtils.IsLongValid(fileAttachmentModel.UserId))
            {
                result.Add(new ReturnErrorItem("UserId", "User cannot be empty"));
            }

            var itemForUser = await companyRepository.GetUserByIdAsync(fileAttachmentModel.UserId);
            if (itemForUser == null)
            {
                result.Add(new ReturnErrorItem("UserId", "User is not available"));
            }

            return result.Count > 0 ? result : null;
        }

        public async Task<ReturnModel<bool>> IsFileAttachmentFileValidAsync(long fileAttachmentId)
        {
            try
            {
                var fileModel = await GetFileAttachmentFileAsync(fileAttachmentId);
                if (!fileModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the File Attachment file", fileModel.ErrorHolder);
                }

                FileUtils.CloseSafely(fileModel.Model.FileData);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the File Attachment file", ex);
            }
        }

        public async Task<ReturnModel<FileStreamModel>> GetFileAttachmentFileAsync(long fileAttachmentId)
        {
            try
            {
                var view = await GetFileAttachmentByIdForReadAsync(fileAttachmentId);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the File Attachment", new { fileAttachmentId });
                }

                var fileName = view.Model.FileAttachmentFileLocation;

                if (fileName.IsTrimmedStringNullOrEmpty())
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"System error: file is not available.");
                }

                var actualFileName = view.Model.FileAttachmentFileName.IsTrimmedStringNullOrEmpty() ? $"FileAttachmentFile" : view.Model.FileAttachmentFileName;

                var result = new FileStreamModel
                {
                    FileName = actualFileName,
                    ContentType = FileUtils.GetMimeTypeFromFileName(actualFileName),
                    FileData = await fileAttachmentFileStorage.OpenReadAsync(0, fileName)
                };

                return ReturnModel<FileStreamModel>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for File Attachment ", new { fileAttachmentId }, ex);
            }
        }

        /*
        public async Task CreateOrUpdateFileAttachmentDependentsAsync(FileAttachmentModel model)
        {
            if (model.NewFileAttachmentXYZZYXModels == null)
                model.NewFileAttachmentXYZZYXModels = new List<FileAttachmentXYZZYXCreateModel>();

            if (model.ExistingFileAttachmentXYZZYXModels == null)
                model.ExistingFileAttachmentXYZZYXModels = new List<FileAttachmentXYZZYXUpdateModel>();

            var dbExistingFileAttachments = await movieRepository.GetFileAttachmentXYZZYXsByFileAttachmentIdAsync(model.FileAttachmentId);
            if (!dbExistingFileAttachments.IsNullOrEmpty())
            {
                var dbExistingFileAttachmentIds = dbExistingFileAttachments.SelectToList(x => x.FileAttachmentXYZZYXId);
                var modelExistingFileAttachmentIds = model.ExistingFileAttachmentXYZZYXModels.SelectToList(x => x.FileAttachmentXYZZYXId);

                var xYZZYXItemsToDelete = dbExistingFileAttachments.Where(x => !modelExistingFileAttachmentIds.Contains(x.FileAttachmentXYZZYXId));
                foreach (var x in xYZZYXItemsToDelete)
                {
                    await MarkFileAttachmentXYZZYXAsDeletedByIdAsync(x.FileAttachmentXYZZYXId);
                }

                var xYZZYXItemsToUpdate = model.ExistingFileAttachmentXYZZYXModels.Where(x => dbExistingFileAttachmentIds.Contains(x.FileAttachmentXYZZYXId));
                foreach (var x in xYZZYXItemsToUpdate)
                {
                    x.FileAttachmentId = model.FileAttachmentId;
                    var result = await UpdateFileAttachmentXYZZYXAsync(x);
                    if (!result.IsSuccess)
                    {
                        throw new RollbackException(result);
                    }
                }
            }

            foreach (var x in model.NewFileAttachmentXYZZYXModels)
            {
                x.FileAttachmentId = model.FileAttachmentId;
                var result = await CreateFileAttachmentXYZZYXAsync(x);
                if (!result.IsSuccess)
                {
                    throw new RollbackException(result);
                }
            }
        }

        public async Task<ReturnModel<bool>> EnableFileAttachmentByIdAsync(long fileAttachmentId)
        {
            try
            {
                var existingFileAttachmentModel = await GetFileAttachmentByIdForWriteAsync(fileAttachmentId);
                if (!existingFileAttachmentModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileAttachmentModel>(existingFileAttachmentModel);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(existingFileAttachmentModel.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.EnableFileAttachmentByIdAsync(existingFileAttachmentModel.Model.FileAttachmentId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.FileAttachmentEnabledAsync(fileAttachmentId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when enabling File Attachment", new { fileAttachmentId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> DisableFileAttachmentByIdAsync(long fileAttachmentId)
        {
            try
            {
                var existingFileAttachmentModel = await GetFileAttachmentByIdForWriteAsync(fileAttachmentId);
                if (!existingFileAttachmentModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileAttachmentModel>(existingFileAttachmentModel);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(existingFileAttachmentModel.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await movieRepository.DisableFileAttachmentByIdAsync(existingFileAttachmentModel.Model.FileAttachmentId, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.FileAttachmentDisabledAsync(existingFileAttachmentModel.Model.FileAttachmentId);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when disabling File Attachment",new { fileAttachmentId }, ex);
            }
        }


        public async Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByFileAttachmentAsync(long fileAttachmentId)
        {
            try
            {
                var contract = await movieRepository.GetFileAttachmentByIdAsync(fileAttachmentId);
                if (contract == null)
                {
                    return log.ErrorAndReturnListModel<FileAttachmentModel>($"Unable to find File Attachment", new { fileAttachmentId });
                }
                return this.ConvertToList(await movieRepository.GetXYZZYXsByFileAttachmentsAsync(contract.FileAttachmentId), objectMapperService.FileAttachmentViewListToFileAttachmentModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<FileAttachmentModel>($"Unexpected error when getting Related File Attachments of the File Attachment");
            }
        }

        public async Task<ReturnModel<bool>> AssociateXYZZYXWithFileAttachmentAsync(FileAttachmentXYZZYXAssociationModel model)
        {
            try
            {
                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(await currentSessionService.GetCurrentCompanyIdAsync());

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                var errorLst = await IsFileAttachmentWithFileAttachmentAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("File Attachment association is invalid", errorLst);
                }

                if (await movieRepository.DoesFileAttachmentWithXYZZYXAssociationExistsAsync(model.FileAttachmentId, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related File Attachment is already associated with the File Attachment", new { model.FileAttachmentId, model.XYZZYXId});
                }

                var rValue = await movieRepository.AssociateFileAttachmentWithXYZZYXAsync(model.FileAttachmentId, model.XYZZYXId, await currentSessionService.GetCurrentUserIdAsync());
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }
                return log.ErrorAndReturnModel<bool>("Unable to associate File Attachment with XYZZYX", new { model.FileAttachmentId, model.XYZZYXId});
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when associating File Attachment with XYZZYX", new { model.FileAttachmentId, model.XYZZYXId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> UnAssociateXYZZYXWithFileAttachmentAsync(FileAttachmentXYZZYXAssociationModel model)
        {
            try
            {
                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(await currentSessionService.GetCurrentCompanyIdAsync());

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                var errorLst = await IsFileAttachmentWithXYZZYXAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("File Attachment association is not valid", errorLst);
                }

                if (!await movieRepository.DoesFileAttachmentWithXYZZYXAssociationExistsAsync(model.FileAttachmentId, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related File Attachment is not associated with the File Attachment", new { model.FileAttachmentId, model.XYZZYXId });
                }

                var rValue = await movieRepository.UnAssociateFileAttachmentWithFileAttachmentAsync(model.FileAttachmentId, model.XYZZYXId);
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }

                return log.ErrorAndReturnModel<bool>("Unable to unassociate Related File Attachment with the File Attachment", new { model.FileAttachmentId, model.XYZZYXId });
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when unassociating Related File Attachment with the File Attachment", new { model.FileAttachmentId, model.XYZZYXId }, ex);
            }
        }

        public async Task<List<ReturnErrorItem>> IsFileAttachmentWithXYZZYXAssociationValidAsync(FileAttachmentXYZZYXAssociationModel model)
        {
            var result = new List<ReturnErrorItem>();
            if (model == null)
            {
                result.Add(new ReturnErrorItem("", "Association is not valid"));
                return result;
            }

            if (model.FileAttachmentId == 0)
            {
                result.Add(new ReturnErrorItem("", "File Attachment is not valid"));
                return result;
            }

            if (model.XYZZYXId == 0)
            {
                result.Add(new ReturnErrorItem("", "Related File Attachment  is not valid"));
                return result;
            }

            var entity = await GetFileAttachmentForWriteByIdAsync(model.FileAttachmentId);
            if(!entity.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "File Attachment information is not avialable"));
                return result;
            }

            var xYZZYX = await GetXYZZYXForWriteByIdAsync(model.XYZZYXId);
            if (!xYZZYX.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "Related XYZZYX information is not avialable"));
                return result;
            }

            if (entity.CompanyId != xYZZYX.CompanyId)
            {
                result.Add(new ReturnErrorItem("", "File Attachments does not belong to the same company"));
            }

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }

        public async Task<ReturnListModel<ImportStatusModel>> ImportFileAttachmentAsync(FileAttachmentImportModel model, bool isTest)
        {
            try
            {
                if (model == null)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("File Attachment Import Info is not provided");
                }

                if (model.FileAttachments == null || model.FileAttachments.Count == 0)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("File Attachments Info is not available to import");
                }

                var resultLst = new List<ImportStatusModel>();

                foreach (var item in model.FileAttachments)
                {
                    var prod = await wipService.GetFileAttachmentByExternalIdentifierAsync(item.FileAttachmentIdentifier);
                    if (prod.Model == null)
                    {
                        //var newProd = new ProductCreateModel();
                        FileAttachmentCreateModel fileAttachmentCreateModel = new FileAttachmentCreateModel
                        {
                            IsEnabled = item.IsProductEnabled
                        };

                        var newlyCreatedFileAttachment = await CreateFileAttachmentAsync(fileAttachmentCreateModel);
                        if (!newlyCreatedFileAttachment.IsSuccess)
                        {
                            var errorStr = string.Empty;
                            if (newlyCreatedFileAttachment.ErrorHolder.ErrorItemList != null && newlyCreatedProd.ErrorHolder.ErrorItemList.Count > 0)
                            {
                                var errorValueList = newlyCreatedFileAttachment.ErrorHolder.ErrorItemList.SelectToList(x => x.Value);
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
                        FileAttachmentUpdateModel fileAttachmentUpdateModel = new FileAttachmentUpdateModel
                        {
                        };

                        //update product
                        var updatedFileAttachmentModel = await wipService.UpdateFileAttachmentAsync(fileAttachmentUpdateModel);
                        if (!updatedFileAttachmentModel.IsSuccess)
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
                return log.ErrorAndReturnListModel<ImportStatusModel>("Unexpected error occured when importing File Attachment Info", ex);
            }
        }

        public async Task<ReturnListModel<FileAttachmentExportModel>> ExportFileAttachmentAsync(FileAttachmentExportParameter filter, bool isTest)
        {
            try
            {
                var fileAttachmentCount = await movieRepository.GetFileAttachmentCountAsync(filter);
                if (fileAttachmentCount == 0)
                {
                    return log.ErrorAndReturnListModel<FileAttachmentExportModel>("No File Attachment available for export.");
                }

                var itemsToReturn = new List<FileAttachmentExportModel>();
                var numberOfLoops = (fileAttachmentCount / pageSize) + 1;
                for (var i = 0; i < numberOfLoops; ++i)
                {
                    var dataResult = await wipService.GetUserViewListAsync(new FileAttachmentSearchCriteria { Page = i, PageSize = pageSize, Keyword = filter.Keyword });
                    if (dataResult.IsSuccess)
                    {
                        return log.ErrorAndReturnListModel<FileAttachmentExportModel>(dataResult);
                    }

                    itemsToReturn.AddRange(objectMapperService.MapList<FileAttachmentModel, FileAttachmentExportModel>(dataResult.Model));
                }

                return ReturnListModel<FileAttachmentExportModel>.Success(itemsToReturn);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<FileAttachmentExportModel>("Unexpected error occured when exporting File Attachment", ex);
            }
        }

        */
        #endregion
    }
}
