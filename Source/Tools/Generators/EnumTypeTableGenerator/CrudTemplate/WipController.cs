        #region EEntity123
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<EEntity123Model>> GetEEntity123s([FromQuery] EEntity123SearchCriteria search)
        {
            return wipService.GetEEntity123sAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<EEntity123Model>> GetEEntity123([FromQuery] long id)
        {
            return wipService.GetEEntity123ByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<EEntity123Model>> CreateEEntity123([FromBody] EEntity123CreateModel eentity123Model)
        {
            return wipService.CreateEEntity123Async(eentity123Model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<EEntity123Model>> CreateMultipleEEntity123([FromBody] MultipleEEntity123CreateModel eentity123Models)
        {
            return wipService.CreateMultipleEEntity123Async(eentity123Models);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<EEntity123Model>> UpdateEEntity123([FromBody] EEntity123UpdateModel eentity123Model)
        {
            return wipService.UpdateEEntity123Async(eentity123Model);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkEEntity123AsDeleted([FromQuery] long id)
        {
            return wipService.MarkEEntity123AsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkEEntity123AsNotDeleted([FromBody] long id)
        {
            return wipService.MarkEEntity123AsNotDeletedByIdAsync(id);
        }

        //%WebFkeyBasedFunctions%

        //%ParentCommentBegin%
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsEEntity123FileValid([FromQuery] long eentity123Id)
        {
            return wipService.IsEEntity123FileValidAsync(eentity123Id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadEEntity123File([FromQuery] long eentity123Id)
        {
            var result = await wipService.GetEEntity123FileAsync(eentity123Id);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadEEntity123File([FromForm] EEntity123FileWithFileFormModel model)
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

            return productService.UploadEEntity123FileAsync(model.EEntity123Id, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearEEntity123File([FromQuery] long movieId)
        {
            return productService.ClearEEntity123FileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsEEntity123SourceWordFileValid([FromQuery] long eentity123Id)
        {
            return wipService.IsEEntity123WordOrPdfFileValidAsync(eentity123Id,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadEEntity123SourceWordFile([FromQuery] long eentity123Id)
        {
            var result = await wipService.GetEEntity123WordOrdPdfFileAsync(eentity123Id, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableEEntity123([FromBody] long id)
        {
            return wipService.EnableEEntity123ByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableEEntity123([FromBody] long id)
        {
            return wipService.DisableEEntity123ByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByEEntity123(long id)
        {
            return wipService.GetXYZZYXsByEEntity123Async(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithEEntity123(EEntity123XYZZYXAssociationModel model)
        {
            return wipService.AssociateXYZZYXWithEEntity123Async(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithEEntity123(EEntity123XYZZYXAssociationModel model)
        {
            return wipService.UnAssociateXYZZYXWithEEntity123Async(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportEEntity123([FromBody] ImportEEntity123Model model)
        {
            return wipService.ImportEEntity123Async(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportEEntity123()
        {
            var lst = new List<ImportEEntity123Model>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportEEntity123Model { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleEEntity123ImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<EEntity123ExportModel>> EEntity123Report([FromQuery] EEntity123ExportParameter parameter)
        {
            return reportService.ExportEEntity123Async(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> EEntity123AsReportCsv([FromQuery] EEntity123ExportParameter parameter)
        {
            var result = await reportService.ExportEEntity123Async(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("EEntity123AsCsv.csv"))
                {
                    FileDownloadName = "EEntity123Export.csv"
                };
            }

            return Json(result);
        }
        //%ParentCommentEnd%
        #endregion
