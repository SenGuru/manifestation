        #region EEntity123

        public async Task<ReturnListModel<EEntity123Model>> GetEEntity123sAsync(EEntity123SearchCriteria search)
        {
            try
            {
                if (!await permissionService.IsUserSuperAdmin())
                {
                    search.CompanyId = await currentSessionService.GetCurrentCompanyIdAsync();
                }

                return this.ConvertToList(await wipRepository.GetEEntity123sAsync(search), objectMapperService.EEntity123ViewListToEEntity123ModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<EEntity123Model>($"Unexpected error when getting FFntity123s", ex);
            }
        }

        public Task<ReturnModel<EEntity123Model>> GetEEntity123ByIdForReadAsync(long eentity123Id)
        {
            return GetEEntity123ByIdInternalAsync(forWrite: false, eentity123Id);
        }

        public Task<ReturnModel<EEntity123Model>> GetEEntity123ByIdForWriteAsync(long eentity123Id)
        {
            return GetEEntity123ByIdInternalAsync(forWrite: true, eentity123Id);
        }

        public async Task<ReturnModel<EEntity123Model>> GetEEntity123ByIdInternalAsync(bool forWrite, long eentity123Id)
        {
            try
            {
                var item = await wipRepository.GetEEntity123ViewByIdAsync(eentity123Id);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<EEntity123Model>($"Unable to find the FFntity123", new { eentity123Id });
                }

                //var existingMovieModel = forWrite ? await GetMovieByIdForWriteAsync(item.MovieId) : await GetMovieByIdForReadAsync(item.MovieId);
                //if (!existingMovieModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<CompanyModel>(existingMovieModel);
                //}

                var (resultValue, _, errorStr) = forWrite ? await permissionService.CanConfigureCompany(item.CompanyId) : await permissionService.CanViewCompany(item.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<EEntity123Model>(errorStr);
                }

                //var result = this.ConvertToItem(item,objectMapperService.EEntity123ViewToEEntity123Model);
                //var childItems = await GetEEntity123XYZZYXsAsync(new EEntity123XYZZYXSearchCriteria { EEntity123Id = item.EEntity123Id, Page = 1, PageSize = int.MaxValue });
                //result.Model.ExistingEEntity123XYZZYXs = childItems.Model.Where(x => x.EEntity123Id == item.EEntity123Id).ToList();
                //return result;

                return this.ConvertToItem(item, objectMapperService.EEntity123ViewToEEntity123Model);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<EEntity123Model>($"Unexpected error when getting FFntity123", new { eentity123Id }, ex);
            }
        }

        public async Task<ReturnListModel<EEntity123Model>> CreateMultipleEEntity123Async(MultipleEEntity123CreateModel eentity123s)
        {
            try
            {
                if (eentity123s == null)
                {
                    return log.ErrorAndReturnListModel<EEntity123Model>($"Unable to Create FFntity123s", new ReturnErrorItem("", "FFntity123s information is not valid"));
                }

                if (eentity123s.NewEEntity123s.IsNullOrEmpty())
                {
                    return log.ErrorAndReturnListModel<EEntity123Model>($"Unable to Create FFntity123s", new ReturnErrorItem("", "FFntity123s information is not provided"));
                }

                var lst = new List<EEntity123Model>();

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    foreach (var eentity123 in eentity123s.NewEEntity123s)
                    {
                        var cResult = await CreateEEntity123Async(eentity123);
                        if (!cResult.IsSuccess)
                            throw new RollbackException(cResult);

                        lst.Add(cResult.Model);
                    }

                    await trx.CompleteAsync();
                }

                return ReturnListModel<EEntity123Model>.Success(lst);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModelWithRollbackExceptionHandler<EEntity123Model>($"Unexpected error when creating FFntity123s", new { eentity123s }, ex);
            }
        }

        public async Task<ReturnModel<EEntity123Model>> CreateEEntity123Async(EEntity123CreateModel eentity123)
        {
            try
            {
                if (eentity123 == null)
                {
                    return log.ErrorAndReturnModel<EEntity123Model>($"Unable to create FFntity123", new ReturnErrorItem("", "FFntity123 information is not valid"));
                }

                if (!await permissionService.IsUserSuperAdmin())
                {
                    eentity123.CompanyId = await currentSessionService.GetCurrentCompanyIdAsync() ?? 0;
                }

                //var companyModel = await companyService.GetCompanyByIdForReadAsync(eentity123.CompanyId);
                //if (!companyModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<EEntity123Model>(companyModel);
                //}
                //
                //var movieModel = await GetMovieByIdForReadAsync(eentity123.MovieId);
                //if (!movieModel.IsSuccess)
                //{
                //    return log.ErrorAndReturnModel<EEntity123Model>(movieModel);
                //}
                //
                //%FKeyAssignmentInCreateUpdateCode%

                /*var ignoreCode = false;
                if (eentity123.EEntity123Code.IsNullOrEmpty())
                {
                    ignoreCode = true;
                    eentity123.EEntity123Code = StringConstants.PrefixForCode.EEntity123 + Guid.NewGuid().ToString();
                }
                */

                var errorList = await IsEEntity123ModelValidForCreateAsync(eentity123);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<EEntity123Model>($"Unable to create FFntity123", errorList);
                }

                eentity123.IsDeleted = false;

                var item = objectMapperService.EEntity123CreateModelToEEntity123(eentity123);

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await wipRepository.CreateEEntity123Async(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create FFntity123");
                    }

                    //await CreateOrUpdateEEntity123DependentsAsync(eentity123)

                    /*if (ignoreCode)
                    {
                        if (!await wipRepository.UpdateEEntity123CodeAsync(item.CompanyId, item.EEntity123Id, StringConstants.PrefixForCode.EEntity123 + item.EEntity123Id, await currentSessionService.GetCurrentUserIdAsync()))
                        {
                            throw new RollbackException($"Unable update the FFntity123 Code", item);
                        }
                    }*/

                    await trx.CompleteAsync();
                }

                await auditService.EEntity123CreatedAsync(item.EEntity123Id);

                var view = await wipRepository.GetEEntity123ViewByIdAsync(item.EEntity123Id);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<EEntity123Model>($"Unable to find the FFntity123", new { item.EEntity123Id });
                }

                return this.ConvertToItem(view, objectMapperService.EEntity123ViewToEEntity123Model);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<EEntity123Model>($"Unexpected error when creating FFntity123", new { eentity123?.EEntity123Id }, ex);
            }
        }

        public async Task<ReturnModel<EEntity123Model>> UpdateEEntity123Async(EEntity123UpdateModel eentity123)
        {
            try
            {
                if (eentity123 == null)
                {
                    return log.ErrorAndReturnModel<EEntity123Model>($"Unable to update FFntity123", new ReturnErrorItem("", "FFntity123 information is not valid"));
                }

                var existingEEntity123Model = await GetEEntity123ByIdForWriteAsync(eentity123.EEntity123Id);
                if (!existingEEntity123Model.IsSuccess)
                {
                    return log.ErrorAndReturnModel<EEntity123Model>(existingEEntity123Model);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(eentity123.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<EEntity123Model>(errorStr);
                }

                if (canAssign)
                {
                    eentity123.CompanyId = await currentSessionService.GetCurrentCompanyIdAsync() ?? 0;
                }

                eentity123.CreatedByUserId = existingEEntity123Model.Model.CreatedByUserId;
                eentity123.CreatedDate = existingEEntity123Model.Model.CreatedDate;
                eentity123.IsDeleted = existingEEntity123Model.Model.IsDeleted;
                eentity123.CompanyId = existingEEntity123Model.Model.CompanyId;
                //eentity123.MovieId = existingEEntity123Model.Model.MovieId;
                //eentity123.ProfileImageActualFileName = existingEEntity123Model.Model.ProfileImageActualFileName;
                //eentity123.ProfileImageLocation = existingEEntity123Model.Model.ProfileImageLocation;

                //%FKeyAssignmentInCreateUpdateCode%

                var errorList = await IsEEntity123ModelValidForUpdateAsync(eentity123);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<EEntity123Model>($"Unable to update FFntity123", errorList);
                }

                var item = objectMapperService.EEntity123UpdateModelToEEntity123(eentity123);

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await wipRepository.UpdateEEntity123Async(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (result == false)
                    {
                        throw new RollbackException($"Unable to update FFntity123");
                    }

                    //await CreateOrUpdateEEntity123DependentsAsync(eentity123)

                    await trx.CompleteAsync();
                }

                await auditService.EEntity123UpdatedAsync(eentity123.EEntity123Id);

                var view = await wipRepository.GetEEntity123ViewByIdAsync(eentity123.EEntity123Id);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<EEntity123Model>($"Unable to find the FFntity123", new { eentity123.EEntity123Id });
                }

                return this.ConvertToItem(view, objectMapperService.EEntity123ViewToEEntity123Model);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<EEntity123Model>($"Unexpected error when updating FFntity123", new { eentity123?.EEntity123Id }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkEEntity123AsNotDeletedByIdAsync(long eentity123Id)
        {
            try
            {
                var existingEEntity123Model = await GetEEntity123ByIdForWriteAsync(eentity123Id);
                if (!existingEEntity123Model.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingEEntity123Model);
                }

                var (resultValue, _, errorStr) = await permissionService.CanConfigureCompany(existingEEntity123Model.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await wipRepository.MarkEEntity123AsNotDeletedByIdAsync(eentity123Id, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.EEntity123DeletedAsync(eentity123Id);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking FFntity123 as not deleted", new { eentity123Id }, ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkEEntity123AsDeletedByIdAsync(long eentity123Id)
        {
            try
            {
                var existingEEntity123Model = await GetEEntity123ByIdForWriteAsync(eentity123Id);
                if (!existingEEntity123Model.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingEEntity123Model);
                }

                var (resultValue, _, errorStr) = await permissionService.CanConfigureCompany(existingEEntity123Model.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await wipRepository.MarkEEntity123AsDeletedByIdAsync(eentity123Id, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.EEntity123DeletedAsync(eentity123Id);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when marking FFntity123 as deleted", new { eentity123Id }, ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteEEntity123ByAsync(long eentity123Id)
        {
            try
            {
                var existingEEntity123Model = await GetEEntity123ByIdForWriteAsync(eentity123Id);
                if (!existingEEntity123Model.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(existingEEntity123Model);
                }

                var (resultValue, _, errorStr) = await permissionService.CanConfigureCompany(existingEEntity123Model.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await wipRepository.HardDeleteEEntity123ByIdAsync(eentity123Id);
                    await trx.CompleteAsync();
                }


                await auditService.HardDeleteEEntity123Async(eentity123Id);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting FFntity123", new { eentity123Id }, ex);
            }
        }

        public Task<List<ReturnErrorItem>> IsEEntity123ModelValidForCreateAsync(EEntity123UpdateModel eentity123Model)
        {
            return IsEEntity123ModelValidForAllAsync(true, eentity123Model);
        }

        public Task<List<ReturnErrorItem>> IsEEntity123ModelValidForUpdateAsync(EEntity123UpdateModel eentity123Model)
        {
            return IsEEntity123ModelValidForAllAsync(false, eentity123Model);
        }

        public async Task<List<ReturnErrorItem>> IsEEntity123ModelValidForAllAsync(bool isCreate, EEntity123UpdateModel eentity123Model)
        {
            var result = new List<ReturnErrorItem>();

            if (isCreate)
            {
                if (await wipRepository.IsEEntity123NameAlreadyExistsForOtherEEntity123Async(eentity123Model.EEntity123Name, eentity123Model.CompanyId))
                {
                    result.Add(new ReturnErrorItem("", "Another FFntity123 already exists"));
                }

                if (eentity123Model.EEntity123Id > 0)
                {
                    result.Add(new ReturnErrorItem("", "FFntity123 Identifier is not valid"));
                }
            }
            else
            {
                if (await wipRepository.IsEEntity123NameAlreadyExistsForOtherEEntity123Async(eentity123Model.EEntity123Id, eentity123Model.EEntity123Name, eentity123Model.CompanyId))
                {
                    result.Add(new ReturnErrorItem("", "Another FFntity123 already exists"));
                }

                if (eentity123Model.EEntity123Id <= 0)
                {
                    result.Add(new ReturnErrorItem("", "FFntity123 Unique Identifier is not valid"));
                }
            }

            //%ServiceFKeyAssignment%

            //%ServiceChecks%

            return result.Count > 0 ? result : null;
        }
        
        //%ServiceFkeyBasedFunctions%

        //%ParentCommentBegin%
        public async Task<ReturnModel<bool>> IsEEntity123WordOrPdfFileValidAsync(long eentity123Id, bool isPdf)
        {
            var tName = isPdf ? "preview" : "source";

            try
            {
                var fileModel = await GetEEntity123WordOrPdfFileAsync(eentity123Id,isPdf);
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

        public async Task<ReturnModel<FileStreamModel>> GetEEntity123WordOrdPdfFileAsync(long eentity123Id, bool isPdf)
        {
            try
            {
                var view = await GetEEntity123ByIdForReadAsync(eentity123Id);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the FFntity123", new { eentity123Id });
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
                    FileName = view.Model.ActualFileName.IsTrimmedStringNullOrEmpty() ? $"EEntity123File.{docType}" : view.Model.ActualFileName,
                    ContentType = FileUtils.GetMimeTypeFromFileName(view.Model.ActualFileName),
                    FileData = await wipFileStorage.OpenReadAsync(view.Model.CompanyId, fileName)
                };

                return ReturnModel<FileStreamModel>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for FFntity123 ", new { eentity123Id }, ex);
            }
        }

        public async Task<ReturnModel<bool>> IsEEntity123FileValidAsync(long eentity123Id)
        {
            try
            {
                var fileModel = await GetEEntity123FileAsync(eentity123Id);
                if (!fileModel.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the FFntity123 file", fileModel.ErrorHolder);
                }

                FileUtils.CloseSafely(fileModel.Model.FileData);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting the FFntity123 file", ex);
            }
        }

        public async Task<ReturnModel<FileStreamModel>> GetEEntity123FileAsync(long eentity123Id)
        {
            try
            {
                var view = await GetEEntity123ByIdForReadAsync(eentity123Id);

                if (!view.IsSuccess)
                {
                    return log.ErrorAndReturnModel<FileStreamModel>($"Unable to find the FFntity123", new { eentity123Id });
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

                var actualFileName = view.Model.ActualFileName.IsTrimmedStringNullOrEmpty() ? $"EEntity123File" : view.Model.ActualFileName;

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
                return log.ErrorAndReturnModel<FileStreamModel>($"Unexpected error when getting File for FFntity123 ", new { eentity123Id }, ex);
            }
        }

        public async Task<ReturnModel<bool>> UploadEEntity123FileAsync(long eentity123Id, FileStreamModel streamModel)
        {
            try
            {
                var eentity123 = await GetEEntity123ByIdForWriteAsync(eentity123Id);
                if (!eentity123.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(eentity123);
                }

                if (streamModel == null)
                {
                    return log.ErrorAndReturnModel<bool>("File Content is not provided");
                }

                var company = await companyRepository.GetCompanyByIdAsync(eentity123.Model.CompanyId);
                if (company == null)
                {
                    return log.ErrorAndReturnModel<bool>("Unable to find the Company Information");
                }

                try
                {
                    using (var trx = await transactionManager.BeginTransactionAsync())
                    {
                        var path = profileFileFileStorage.ProfileFilePathForHostBrand(eentity123.Model.CompanyId, eentity123.Model.EEntity123Id, streamModel.FileName);
                        if (company.PredefinedCompanyTypeId == (int)PredefinedCompanyTypes.MarketPlace)
                            path = profileFileFileStorage.ProfileFilePathForMarketplaceBrand(eentity123.Model.CompanyId, eentity123.Model.EEntity123Id, streamModel.FileName);
                        await productRepository.UpdateEEntity123FilePathAsync(eentity123.Model.EEntity123Id, path, streamModel.FileName, await currentSessionService.GetCurrentUserIdAsync());
                        await profileFileFileStorage.WriteAsync(eentity123.Model.CompanyId, path, streamModel.FileData);
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
                return log.ErrorAndReturnModel<bool>($"Unexpected error when Uploading FFntity123 File", ex);
            }
        }

        public async Task<ReturnModel<bool>> ClearEEntity123FileAsync(long eentity123Id)
        {
            try
            {
                var eentity123 = await GetEEntity123ByIdForWriteAsync(eentity123Id);
                if (!eentity123.IsSuccess)
                {
                    return log.ErrorAndReturnModel<bool>(eentity123);
                }

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    await productRepository.ClearEEntity123FilePathAsync(eentity123.Model.EEntity123Id, await currentSessionService.GetCurrentUserIdAsync());
                    await profileFileFileStorage.DeleteAsync(eentity123.Model.CompanyId, eentity123.Model.ProfileImageLocation);
                }

                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when clearing FFntity123 File", ex);
            }
        }

        public async Task CreateOrUpdateEEntity123DependentsAsync(EEntity123UpdateModel model)
        {
            if (model.NewEEntity123XYZZYXModels == null)
                model.NewEEntity123XYZZYXModels = new List<EEntity123XYZZYXCreateModel>();

            if (model.ExistingEEntity123XYZZYXModels == null)
                model.ExistingEEntity123XYZZYXModels = new List<EEntity123XYZZYXUpdateModel>();

            var dbExistingEEntity123s = await wipRepository.GetEEntity123XYZZYXsByEEntity123IdAsync(model.EEntity123Id);
            if (!dbExistingEEntity123s.IsNullOrEmpty())
            {
                var dbExistingEEntity123Ids = dbExistingEEntity123s.SelectToList(x => x.EEntity123XYZZYXId);
                var modelExistingEEntity123Ids = model.ExistingEEntity123XYZZYXModels.SelectToList(x => x.EEntity123XYZZYXId);

                var xyzzyxItemsToDelete = dbExistingEEntity123s.Where(x => !modelExistingEEntity123Ids.Contains(x.EEntity123XYZZYXId));
                foreach (var x in xyzzyxItemsToDelete)
                {
                    var result = await MarkEEntity123XYZZYXAsDeletedByIdAsync(x.EEntity123XYZZYXId);
                    if (!result.IsSuccess)
                    {
                        throw new RollbackException(result);
                    }
                }

                var xyzzyxItemsToUpdate = model.ExistingEEntity123XYZZYXModels.Where(x => dbExistingEEntity123Ids.Contains(x.EEntity123XYZZYXId));
                foreach (var x in xyzzyxItemsToUpdate)
                {
                    x.EEntity123Id = model.EEntity123Id;
                    var result = await UpdateEEntity123XYZZYXAsync(x);
                    if (!result.IsSuccess)
                    {   
                        throw new RollbackException(result);
                    }
                }
            }

            foreach (var x in model.NewEEntity123XYZZYXModels)
            {
                x.EEntity123Id = model.EEntity123Id;
                var result = await CreateEEntity123XYZZYXAsync(x);
                if (!result.IsSuccess)
                {
                    throw new RollbackException(result);
                }
            }
        }

        public async Task<ReturnModel<bool>> EnableEEntity123ByIdAsync(long eentity123Id)
        {
            try
            {
                var existingEEntity123Model = await GetEEntity123ByIdForWriteAsync(eentity123Id);
                if (!existingEEntity123Model.IsSuccess)
                {
                    return log.ErrorAndReturnModel<EEntity123Model>(existingEEntity123Model);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(existingEEntity123Model.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;
                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await wipRepository.EnableEEntity123ByIdAsync(existingEEntity123Model.Model.EEntity123Id, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.EEntity123EnabledAsync(eentity123Id);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when enabling FFntity123", new { eentity123Id }, ex);
            }
        }

        public async Task<ReturnModel<bool>> DisableEEntity123ByIdAsync(long eentity123Id)
        {
            try
            {
                var existingEEntity123Model = await GetEEntity123ByIdForWriteAsync(eentity123Id);
                if (!existingEEntity123Model.IsSuccess)
                {
                    return log.ErrorAndReturnModel<EEntity123Model>(existingEEntity123Model);
                }

                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(existingEEntity123Model.Model.CompanyId);

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await wipRepository.DisableEEntity123ByIdAsync(existingEEntity123Model.Model.EEntity123Id, await currentSessionService.GetCurrentUserIdAsync());
                    await trx.CompleteAsync();
                }

                await auditService.EEntity123DisabledAsync(existingEEntity123Model.Model.EEntity123Id);

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when disabling FFntity123",new { eentity123Id }, ex);
            }
        }


        public async Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByEEntity123Async(long eentity123Id)
        {
            try
            {
                var contract = await wipRepository.GetEEntity123ByIdAsync(eentity123Id);
                if (contract == null)
                {
                    return log.ErrorAndReturnListModel<EEntity123Model>($"Unable to find FFntity123", new { eentity123Id });
                }
                return this.ConvertToList(await wipRepository.GetXYZZYXsByEEntity123sAsync(contract.EEntity123Id), objectMapperService.EEntity123ViewListToEEntity123ModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<EEntity123Model>($"Unexpected error when getting Related FFntity123s of the FFntity123");
            }
        }

        public async Task<ReturnModel<bool>> AssociateXYZZYXWithEEntity123Async(EEntity123XYZZYXAssociationModel model)
        {
            try
            {
                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(await currentSessionService.GetCurrentCompanyIdAsync());

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                var errorLst = await IsEEntity123WithEEntity123AssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("FFntity123 association is invalid", errorLst);
                }

                if (await wipRepository.DoesEEntity123WithXYZZYXAssociationExistsAsync(model.EEntity123Id, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related FFntity123 is already associated with the FFntity123", new { model.EEntity123Id, model.XYZZYXId});
                }

                var rValue = await wipRepository.AssociateEEntity123WithXYZZYXAsync(model.EEntity123Id, model.XYZZYXId, await currentSessionService.GetCurrentUserIdAsync());
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }
                return log.ErrorAndReturnModel<bool>("Unable to associate FFntity123 with XYZZYX", new { model.EEntity123Id, model.XYZZYXId});
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when associating FFntity123 with XYZZYX", new { model.EEntity123Id, model.XYZZYXId }, ex);
            }
        }

        public async Task<ReturnModel<bool>> UnAssociateXYZZYXWithEEntity123Async(EEntity123XYZZYXAssociationModel model)
        {
            try
            {
                var (resultValue, canAssign, errorStr) = await permissionService.CanConfigureCompany(await currentSessionService.GetCurrentCompanyIdAsync());

                if (resultValue == false)
                {
                    return log.ErrorAndReturnModel<bool>(errorStr);
                }

                var errorLst = await IsEEntity123WithXYZZYXAssociationValidAsync(model);
                if (errorLst != null)
                {
                    return ReturnModel<bool>.Error("FFntity123 association is not valid", errorLst);
                }

                if (!await wipRepository.DoesEEntity123WithXYZZYXAssociationExistsAsync(model.EEntity123Id, model.XYZZYXId))
                {
                    return log.ErrorAndReturnModel<bool>("Related FFntity123 is not associated with the FFntity123", new { model.EEntity123Id, model.XYZZYXId });
                }

                var rValue = await wipRepository.UnAssociateEEntity123WithEEntity123Async(model.EEntity123Id, model.XYZZYXId);
                if (rValue)
                {
                    return ReturnModel<bool>.Success(rValue);
                }

                return log.ErrorAndReturnModel<bool>("Unable to unassociate Related FFntity123 with the FFntity123", new { model.EEntity123Id, model.XYZZYXId });
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when unassociating Related FFntity123 with the FFntity123", new { model.EEntity123Id, model.XYZZYXId }, ex);
            }
        }

        public async Task<List<ReturnErrorItem>> IsEEntity123WithXYZZYXAssociationValidAsync(EEntity123XYZZYXAssociationModel model)
        {
            var result = new List<ReturnErrorItem>();
            if (model == null)
            {
                result.Add(new ReturnErrorItem("", "Association is not valid"));
                return result;
            }

            if (model.EEntity123Id == 0)
            {
                result.Add(new ReturnErrorItem("", "FFntity123 is not valid"));
                return result;
            }

            if (model.XYZZYXId == 0)
            {
                result.Add(new ReturnErrorItem("", "Related FFntity123  is not valid"));
                return result;
            }

            var entity = await GetEEntity123ForWriteByIdAsync(model.EEntity123Id);
            if(!entity.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "FFntity123 information is not available"));
                return result;
            }

            var xyzzyx = await GetXYZZYXForWriteByIdAsync(model.XYZZYXId);
            if (!xyzzyx.IsSuccess)
            {
                result.Add(new ReturnErrorItem("", "Related XYZZYX information is not available"));
                return result;
            }

            if (entity.CompanyId != xyzzyx.CompanyId)
            {
                result.Add(new ReturnErrorItem("", "FFntity123s does not belong to the same company"));
            }

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }

        public async Task<ReturnListModel<ImportStatusModel>> ImportEEntity123Async(EEntity123ImportModel model, bool isTest)
        {
            try
            {
                if (model == null)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("FFntity123 Import Info is not provided");
                }

                if (model.EEntity123s == null || model.EEntity123s.Count == 0)
                {
                    return log.ErrorAndReturnListModel<ImportStatusModel>("FFntity123s Info is not available to import");
                }

                var resultLst = new List<ImportStatusModel>();

                foreach (var item in model.EEntity123s)
                {
                    var prod = await wipService.GetEEntity123ByExternalIdentifierAsync(item.EEntity123Identifier);
                    if (prod.Model == null)
                    {
                        //var newProd = new ProductCreateModel();
                        EEntity123CreateModel eentity123CreateModel = new EEntity123CreateModel
                        {
                            IsEnabled = item.IsProductEnabled
                        };

                        var newlyCreatedEEntity123 = await CreateEEntity123Async(eentity123CreateModel);
                        if (!newlyCreatedEEntity123.IsSuccess)
                        {
                            var errorStr = string.Empty;
                            if (newlyCreatedEEntity123.ErrorHolder.ErrorItemList != null && newlyCreatedProd.ErrorHolder.ErrorItemList.Count > 0)
                            {
                                var errorValueList = newlyCreatedEEntity123.ErrorHolder.ErrorItemList.SelectToList(x => x.Value);
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
                        EEntity123UpdateModel eentity123UpdateModel = new EEntity123UpdateModel
                        {
                        };

                        //update product
                        var updatedEEntity123Model = await wipService.UpdateEEntity123Async(eentity123UpdateModel);
                        if (!updatedEEntity123Model.IsSuccess)
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
                return log.ErrorAndReturnListModel<ImportStatusModel>("Unexpected error occurred when importing FFntity123 Info", ex);
            }
        }

        public async Task<ReturnListModel<EEntity123ExportModel>> ExportEEntity123Async(EEntity123ExportParameter filter, bool isTest)
        {
            try
            {
                var eentity123Count = await wipRepository.GetEEntity123CountAsync(filter);
                if (eentity123Count == 0)
                {
                    return log.ErrorAndReturnListModel<EEntity123ExportModel>("No FFntity123 available for export.");
                }

                var itemsToReturn = new List<EEntity123ExportModel>();
                var numberOfLoops = (eentity123Count / pageSize) + 1;
                for (var i = 0; i < numberOfLoops; ++i)
                {
                    var dataResult = await wipService.GetUserViewListAsync(new EEntity123SearchCriteria { Page = i, PageSize = pageSize, Keyword = filter.Keyword });
                    if (dataResult.IsSuccess)
                    {
                        return log.ErrorAndReturnListModel<EEntity123ExportModel>(dataResult);
                    }

                    itemsToReturn.AddRange(objectMapperService.EEntity123ModelListToEEntity123ExportModelList(dataResult.Model));
                }

                return ReturnListModel<EEntity123ExportModel>.Success(itemsToReturn);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<EEntity123ExportModel>("Unexpected error occurred when exporting FFntity123", ex);
            }
        }

        //%ParentCommentEnd%
        #endregion
