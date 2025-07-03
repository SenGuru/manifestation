using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Infrastructure.Models.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Manifest.Common.Enums;
using Manifest.Common.Models.ConfigModels;
using Manifest.Service.Completed.Config;
using Manifest.Web.Filters;
using Swashbuckle.AspNetCore.Annotations;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.ViewModels;
using Manifest.Service.App;
using Manifest.Service.Utils;
using Manifest.Web.Models;
using Manifest.Service.Admin;
using Manifest.Common.Models.SearchCriteria.System;
using Manifest.Common.Models.ViewModels.System;
using System.Net;
using Manifest.Common.Models.ViewModels.ImportExportModels;

namespace Manifest.Api.All.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public partial class SystemController : Controller
    {
        private readonly ConfigurationCrudService configurationCrudService;
        private readonly SystemService systemService;
        private readonly ReportCreationService reportCreationService;

        public SystemController(ConfigurationCrudService configurationCrudService, SystemService systemService, ReportCreationService reportCreationService)
        {
            this.configurationCrudService = configurationCrudService;
            this.systemService = systemService;
            this.reportCreationService = reportCreationService;
        }

        #region Country & State

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<CountryModel>> GetSupportedCountries()
        {
            return systemService.GetCountriesByIdsAsync(new List<long> { 1, 2, 3, 80 });
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<CountryModel>> GetCountriesByKeyword([FromQuery] string keyword)
        {
            return systemService.GetCountriesAsync(keyword);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<StateModel>> GetStates([FromQuery] StateSearchCriteria search)
        {
            return systemService.GetStatesAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<StateModel>> GetState([FromQuery] long id)
        {
            return systemService.GetStateByIdAsync(id);
        }
        #endregion

        #region Configurations
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Settings" })]
        [AllowPermission(PredefinedUserPermissionTypes.ConfigureSystem)]
        public Task<ReturnModel<LogConfigurationModel>> GetLogConfigurationAsync()
        {
            return configurationCrudService.GetLogConfigurationAsync();
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Settings" })]
        [AllowPermission(PredefinedUserPermissionTypes.ConfigureSystem)]
        public Task<ReturnModel<bool>> SaveLogConfigurationAsync(LogConfigurationModel obj)
        {
            return configurationCrudService.SaveLogConfigurationAsync(obj);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Settings" })]
        [AllowPermission(PredefinedUserPermissionTypes.ConfigureSystem)]
        public Task<ReturnModel<ManifestWebConfigurationModel>> GetWebConfigurationAsync()
        {
            return configurationCrudService.GetWebConfigurationAsync();
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Settings" })]
        [AllowPermission(PredefinedUserPermissionTypes.ConfigureSystem)]
        public Task<ReturnModel<bool>> SaveWebConfigurationAsync(ManifestWebConfigurationModel obj)
        {
            return configurationCrudService.SaveWebConfigurationAsync(obj);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Settings" })]
        [AllowPermission(PredefinedUserPermissionTypes.ConfigureSystem)]
        public Task<ReturnModel<AppConfigurationModel>> GetAppConfigurationAsync()
        {
            return configurationCrudService.GetAppConfigurationAsync();
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Account" })]
        [AllowPermission(PredefinedUserPermissionTypes.ConfigureSystem)]
        public Task<ReturnModel<bool>> SaveAppConfigurationAsync(AppConfigurationModel obj)
        {
            return configurationCrudService.SaveAppConfigurationAsync(obj);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Settings" })]
        [AllowPermission(PredefinedUserPermissionTypes.ConfigureSystem)]
        public Task<ReturnModel<CommonConfigurationModel>> GetCommonConfigurationAsync()
        {
            return configurationCrudService.GetCommonConfigurationAsync();
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Account" })]
        [AllowPermission(PredefinedUserPermissionTypes.ConfigureSystem)]
        public Task<ReturnModel<bool>> SaveCommonConfigurationAsync(CommonConfigurationModel obj)
        {
            return configurationCrudService.SaveCommonConfigurationAsync(obj);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Settings" })]
        [AllowPermission(PredefinedUserPermissionTypes.ConfigureSystem)]
        public Task<ReturnModel<CoordinatorConfigurationModel>> GetCoordinatorConfigurationAsync()
        {
            return configurationCrudService.GetCoordinatorConfigurationAsync();
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Settings" })]
        [AllowPermission(PredefinedUserPermissionTypes.ConfigureSystem)]
        public Task<ReturnModel<bool>> SaveCoordinatorConfigurationAsync(CoordinatorConfigurationModel obj)
        {
            return configurationCrudService.SaveCoordinatorConfigurationAsync(obj);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Settings" })]
        [AllowPermission(PredefinedUserPermissionTypes.ConfigureSystem)]
        public Task<ReturnModel<ReportServerConfigurationModel>> GetReportServerConfigurationAsync()
        {
            return configurationCrudService.GetReportServerConfigurationAsync();
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Settings" })]
        [AllowPermission(PredefinedUserPermissionTypes.ConfigureSystem)]
        public Task<ReturnModel<bool>> SaveReportServerConfigurationAsync(ReportServerConfigurationModel obj)
        {
            return configurationCrudService.SaveReportServerConfigurationAsync(obj);
        }
        #endregion Configurations

        #region ConfigurationGroup
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<ConfigurationGroupModel>> GetConfigurationGroups([FromQuery] ConfigurationGroupSearchCriteria search)
        {
            return systemService.GetConfigurationGroupsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<ConfigurationGroupModel>> GetConfigurationGroup([FromQuery] long id)
        {
            return systemService.GetConfigurationGroupByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<ConfigurationGroupModel>> CreateConfigurationGroup([FromBody] ConfigurationGroupCreateModel configurationGroupModel)
        {
            return systemService.CreateConfigurationGroupAsync(configurationGroupModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<ConfigurationGroupModel>> UpdateConfigurationGroup([FromBody] ConfigurationGroupUpdateModel configurationGroupModel)
        {
            return systemService.UpdateConfigurationGroupAsync(configurationGroupModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkConfigurationGroupAsDeleted([FromQuery] long id)
        {
            return systemService.MarkConfigurationGroupAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkConfigurationGroupAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkConfigurationGroupAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsConfigurationGroupFileValid([FromQuery] long configurationGroupId)
        {
            return systemService.IsConfigurationGroupFileValidAsync(configurationGroupId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadConfigurationGroupFile([FromQuery] long configurationGroupId)
        {
            var result = await systemService.GetConfigurationGroupFileAsync(configurationGroupId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsConfigurationGroupSourceWordFileValid([FromQuery] long configurationGroupId)
        {
            return systemService.IsConfigurationGroupWordOrPdfFileValidAsync(configurationGroupId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadConfigurationGroupSourceWordFile([FromQuery] long configurationGroupId)
        {
            var result = await systemService.GetConfigurationGroupWordOrdPdfFileAsync(configurationGroupId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableConfigurationGroup([FromBody] long id)
        {
            return systemService.EnableConfigurationGroupByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableConfigurationGroup([FromBody] long id)
        {
            return systemService.DisableConfigurationGroupByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByConfigurationGroup(long id)
        {
            return systemService.GetXYZZYXsByConfigurationGroupAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithConfigurationGroup(ConfigurationGroupXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithConfigurationGroupAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithConfigurationGroup(ConfigurationGroupXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithConfigurationGroupAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportConfigurationGroup([FromBody] ImportConfigurationGroupModel model)
        {
            return systemService.ImportConfigurationGroupAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportConfigurationGroup()
        {
            var lst = new List<ImportConfigurationGroupModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportConfigurationGroupModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleConfigurationGroupImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<ConfigurationGroupExportModel>> ConfigurationGroupReport([FromQuery] ConfigurationGroupExportParameter parameter)
        {
            return reportService.ExportConfigurationGroupAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> ConfigurationGroupAsReportCsv([FromQuery] ConfigurationGroupExportParameter parameter)
        {
            var result = await reportService.ExportConfigurationGroupAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("ConfigurationGroupAsCsv.csv"))
                {
                    FileDownloadName = "ConfigurationGroupExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region ConfigurationItem
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<ConfigurationItemModel>> GetConfigurationItems([FromQuery] ConfigurationItemSearchCriteria search)
        {
            return systemService.GetConfigurationItemsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<ConfigurationItemModel>> GetConfigurationItem([FromQuery] long id)
        {
            return systemService.GetConfigurationItemByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<ConfigurationItemModel>> CreateConfigurationItem([FromBody] ConfigurationItemCreateModel configurationItemModel)
        {
            return systemService.CreateConfigurationItemAsync(configurationItemModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<ConfigurationItemModel>> UpdateConfigurationItem([FromBody] ConfigurationItemUpdateModel configurationItemModel)
        {
            return systemService.UpdateConfigurationItemAsync(configurationItemModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkConfigurationItemAsDeleted([FromQuery] long id)
        {
            return systemService.MarkConfigurationItemAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkConfigurationItemAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkConfigurationItemAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsConfigurationItemFileValid([FromQuery] long configurationItemId)
        {
            return systemService.IsConfigurationItemFileValidAsync(configurationItemId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadConfigurationItemFile([FromQuery] long configurationItemId)
        {
            var result = await systemService.GetConfigurationItemFileAsync(configurationItemId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsConfigurationItemSourceWordFileValid([FromQuery] long configurationItemId)
        {
            return systemService.IsConfigurationItemWordOrPdfFileValidAsync(configurationItemId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadConfigurationItemSourceWordFile([FromQuery] long configurationItemId)
        {
            var result = await systemService.GetConfigurationItemWordOrdPdfFileAsync(configurationItemId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableConfigurationItem([FromBody] long id)
        {
            return systemService.EnableConfigurationItemByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableConfigurationItem([FromBody] long id)
        {
            return systemService.DisableConfigurationItemByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByConfigurationItem(long id)
        {
            return systemService.GetXYZZYXsByConfigurationItemAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithConfigurationItem(ConfigurationItemXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithConfigurationItemAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithConfigurationItem(ConfigurationItemXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithConfigurationItemAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportConfigurationItem([FromBody] ImportConfigurationItemModel model)
        {
            return systemService.ImportConfigurationItemAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportConfigurationItem()
        {
            var lst = new List<ImportConfigurationItemModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportConfigurationItemModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleConfigurationItemImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<ConfigurationItemExportModel>> ConfigurationItemReport([FromQuery] ConfigurationItemExportParameter parameter)
        {
            return reportService.ExportConfigurationItemAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> ConfigurationItemAsReportCsv([FromQuery] ConfigurationItemExportParameter parameter)
        {
            var result = await reportService.ExportConfigurationItemAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("ConfigurationItemAsCsv.csv"))
                {
                    FileDownloadName = "ConfigurationItemExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region EnumGroup
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<EnumGroupModel>> GetEnumGroups([FromQuery] EnumGroupSearchCriteria search)
        {
            return systemService.GetEnumGroupsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<EnumGroupModel>> GetEnumGroup([FromQuery] long id)
        {
            return systemService.GetEnumGroupByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<EnumGroupModel>> CreateEnumGroup([FromBody] EnumGroupCreateModel enumGroupModel)
        {
            return systemService.CreateEnumGroupAsync(enumGroupModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<EnumGroupModel>> UpdateEnumGroup([FromBody] EnumGroupUpdateModel enumGroupModel)
        {
            return systemService.UpdateEnumGroupAsync(enumGroupModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkEnumGroupAsDeleted([FromQuery] long id)
        {
            return systemService.MarkEnumGroupAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkEnumGroupAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkEnumGroupAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsEnumGroupFileValid([FromQuery] long enumGroupId)
        {
            return systemService.IsEnumGroupFileValidAsync(enumGroupId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadEnumGroupFile([FromQuery] long enumGroupId)
        {
            var result = await systemService.GetEnumGroupFileAsync(enumGroupId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsEnumGroupSourceWordFileValid([FromQuery] long enumGroupId)
        {
            return systemService.IsEnumGroupWordOrPdfFileValidAsync(enumGroupId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadEnumGroupSourceWordFile([FromQuery] long enumGroupId)
        {
            var result = await systemService.GetEnumGroupWordOrdPdfFileAsync(enumGroupId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableEnumGroup([FromBody] long id)
        {
            return systemService.EnableEnumGroupByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableEnumGroup([FromBody] long id)
        {
            return systemService.DisableEnumGroupByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByEnumGroup(long id)
        {
            return systemService.GetXYZZYXsByEnumGroupAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithEnumGroup(EnumGroupXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithEnumGroupAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithEnumGroup(EnumGroupXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithEnumGroupAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportEnumGroup([FromBody] ImportEnumGroupModel model)
        {
            return systemService.ImportEnumGroupAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportEnumGroup()
        {
            var lst = new List<ImportEnumGroupModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportEnumGroupModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleEnumGroupImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<EnumGroupExportModel>> EnumGroupReport([FromQuery] EnumGroupExportParameter parameter)
        {
            return reportService.ExportEnumGroupAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> EnumGroupAsReportCsv([FromQuery] EnumGroupExportParameter parameter)
        {
            var result = await reportService.ExportEnumGroupAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("EnumGroupAsCsv.csv"))
                {
                    FileDownloadName = "EnumGroupExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region EnumType
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<EnumTypeModel>> GetEnumTypes([FromQuery] EnumTypeSearchCriteria search)
        {
            return systemService.GetEnumTypesAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<EnumTypeModel>> GetEnumType([FromQuery] long id)
        {
            return systemService.GetEnumTypeByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<EnumTypeModel>> CreateEnumType([FromBody] EnumTypeCreateModel enumTypeModel)
        {
            return systemService.CreateEnumTypeAsync(enumTypeModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<EnumTypeModel>> UpdateEnumType([FromBody] EnumTypeUpdateModel enumTypeModel)
        {
            return systemService.UpdateEnumTypeAsync(enumTypeModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkEnumTypeAsDeleted([FromQuery] long id)
        {
            return systemService.MarkEnumTypeAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkEnumTypeAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkEnumTypeAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsEnumTypeFileValid([FromQuery] long enumTypeId)
        {
            return systemService.IsEnumTypeFileValidAsync(enumTypeId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadEnumTypeFile([FromQuery] long enumTypeId)
        {
            var result = await systemService.GetEnumTypeFileAsync(enumTypeId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsEnumTypeSourceWordFileValid([FromQuery] long enumTypeId)
        {
            return systemService.IsEnumTypeWordOrPdfFileValidAsync(enumTypeId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadEnumTypeSourceWordFile([FromQuery] long enumTypeId)
        {
            var result = await systemService.GetEnumTypeWordOrdPdfFileAsync(enumTypeId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableEnumType([FromBody] long id)
        {
            return systemService.EnableEnumTypeByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableEnumType([FromBody] long id)
        {
            return systemService.DisableEnumTypeByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByEnumType(long id)
        {
            return systemService.GetXYZZYXsByEnumTypeAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithEnumType(EnumTypeXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithEnumTypeAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithEnumType(EnumTypeXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithEnumTypeAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportEnumType([FromBody] ImportEnumTypeModel model)
        {
            return systemService.ImportEnumTypeAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportEnumType()
        {
            var lst = new List<ImportEnumTypeModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportEnumTypeModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleEnumTypeImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<EnumTypeExportModel>> EnumTypeReport([FromQuery] EnumTypeExportParameter parameter)
        {
            return reportService.ExportEnumTypeAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> EnumTypeAsReportCsv([FromQuery] EnumTypeExportParameter parameter)
        {
            var result = await reportService.ExportEnumTypeAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("EnumTypeAsCsv.csv"))
                {
                    FileDownloadName = "EnumTypeExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region Job
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<JobModel>> GetJobs([FromQuery] JobSearchCriteria search)
        {
            return systemService.GetJobsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobModel>> GetJob([FromQuery] long id)
        {
            return systemService.GetJobByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobModel>> CreateJob([FromBody] JobCreateModel jobModel)
        {
            return systemService.CreateJobAsync(jobModel);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobModel>> CreateJobFromScheduledJob([FromQuery] long scheduleJobId)
        {
            return reportCreationService.CreateJobFromScheduledJobAsync(scheduleJobId);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobModel>> CreateJobFromJobTemplate([FromBody] JobCreateModel jobTemplateJob)
        {
            return reportCreationService.CreateJobFromJobTemplateAsync(jobTemplateJob);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobModel>> UpdateJob([FromBody] JobUpdateModel jobModel)
        {
            return systemService.UpdateJobAsync(jobModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkJobAsDeleted([FromQuery] long id)
        {
            return systemService.MarkJobAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkJobAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkJobAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsJobFileValid([FromQuery] long jobId)
        {
            return systemService.IsJobFileValidAsync(jobId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadJobFile([FromQuery] long jobId)
        {
            var result = await systemService.GetJobFileAsync(jobId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsJobSourceWordFileValid([FromQuery] long jobId)
        {
            return systemService.IsJobWordOrPdfFileValidAsync(jobId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadJobSourceWordFile([FromQuery] long jobId)
        {
            var result = await systemService.GetJobWordOrdPdfFileAsync(jobId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableJob([FromBody] long id)
        {
            return systemService.EnableJobByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableJob([FromBody] long id)
        {
            return systemService.DisableJobByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByJob(long id)
        {
            return systemService.GetXYZZYXsByJobAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithJob(JobXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithJobAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithJob(JobXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithJobAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportJob([FromBody] ImportJobModel model)
        {
            return systemService.ImportJobAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportJob()
        {
            var lst = new List<ImportJobModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportJobModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleJobImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<JobExportModel>> JobReport([FromQuery] JobExportParameter parameter)
        {
            return reportService.ExportJobAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> JobAsReportCsv([FromQuery] JobExportParameter parameter)
        {
            var result = await reportService.ExportJobAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("JobAsCsv.csv"))
                {
                    FileDownloadName = "JobExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region JobAttachment
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<JobAttachmentModel>> GetJobAttachments([FromQuery] JobAttachmentSearchCriteria search)
        {
            return systemService.GetJobAttachmentsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobAttachmentModel>> GetJobAttachment([FromQuery] long id)
        {
            return systemService.GetJobAttachmentByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobAttachmentModel>> CreateJobAttachment([FromBody] JobAttachmentCreateModel jobAttachmentModel)
        {
            return systemService.CreateJobAttachmentAsync(jobAttachmentModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobAttachmentModel>> UpdateJobAttachment([FromBody] JobAttachmentUpdateModel jobAttachmentModel)
        {
            return systemService.UpdateJobAttachmentAsync(jobAttachmentModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkJobAttachmentAsDeleted([FromQuery] long id)
        {
            return systemService.MarkJobAttachmentAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkJobAttachmentAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkJobAttachmentAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsJobAttachmentFileValid([FromQuery] long jobAttachmentId)
        {
            return systemService.IsJobAttachmentFileValidAsync(jobAttachmentId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadJobAttachmentFile([FromQuery] long jobAttachmentId)
        {
            var result = await systemService.GetJobAttachmentFileAsync(jobAttachmentId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsJobAttachmentSourceWordFileValid([FromQuery] long jobAttachmentId)
        {
            return systemService.IsJobAttachmentWordOrPdfFileValidAsync(jobAttachmentId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadJobAttachmentSourceWordFile([FromQuery] long jobAttachmentId)
        {
            var result = await systemService.GetJobAttachmentWordOrdPdfFileAsync(jobAttachmentId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableJobAttachment([FromBody] long id)
        {
            return systemService.EnableJobAttachmentByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableJobAttachment([FromBody] long id)
        {
            return systemService.DisableJobAttachmentByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByJobAttachment(long id)
        {
            return systemService.GetXYZZYXsByJobAttachmentAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithJobAttachment(JobAttachmentXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithJobAttachmentAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithJobAttachment(JobAttachmentXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithJobAttachmentAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportJobAttachment([FromBody] ImportJobAttachmentModel model)
        {
            return systemService.ImportJobAttachmentAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportJobAttachment()
        {
            var lst = new List<ImportJobAttachmentModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportJobAttachmentModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleJobAttachmentImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<JobAttachmentExportModel>> JobAttachmentReport([FromQuery] JobAttachmentExportParameter parameter)
        {
            return reportService.ExportJobAttachmentAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> JobAttachmentAsReportCsv([FromQuery] JobAttachmentExportParameter parameter)
        {
            var result = await reportService.ExportJobAttachmentAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("JobAttachmentAsCsv.csv"))
                {
                    FileDownloadName = "JobAttachmentExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region JobNotificationEmail
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<JobNotificationEmailModel>> GetJobNotificationEmails([FromQuery] JobNotificationEmailSearchCriteria search)
        {
            return systemService.GetJobNotificationEmailsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobNotificationEmailModel>> GetJobNotificationEmail([FromQuery] long id)
        {
            return systemService.GetJobNotificationEmailByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobNotificationEmailModel>> CreateJobNotificationEmail([FromBody] JobNotificationEmailCreateModel jobNotificationEmailModel)
        {
            return systemService.CreateJobNotificationEmailAsync(jobNotificationEmailModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobNotificationEmailModel>> UpdateJobNotificationEmail([FromBody] JobNotificationEmailUpdateModel jobNotificationEmailModel)
        {
            return systemService.UpdateJobNotificationEmailAsync(jobNotificationEmailModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkJobNotificationEmailAsDeleted([FromQuery] long id)
        {
            return systemService.MarkJobNotificationEmailAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkJobNotificationEmailAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkJobNotificationEmailAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsJobNotificationEmailFileValid([FromQuery] long jobNotificationEmailId)
        {
            return systemService.IsJobNotificationEmailFileValidAsync(jobNotificationEmailId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadJobNotificationEmailFile([FromQuery] long jobNotificationEmailId)
        {
            var result = await systemService.GetJobNotificationEmailFileAsync(jobNotificationEmailId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsJobNotificationEmailSourceWordFileValid([FromQuery] long jobNotificationEmailId)
        {
            return systemService.IsJobNotificationEmailWordOrPdfFileValidAsync(jobNotificationEmailId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadJobNotificationEmailSourceWordFile([FromQuery] long jobNotificationEmailId)
        {
            var result = await systemService.GetJobNotificationEmailWordOrdPdfFileAsync(jobNotificationEmailId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableJobNotificationEmail([FromBody] long id)
        {
            return systemService.EnableJobNotificationEmailByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableJobNotificationEmail([FromBody] long id)
        {
            return systemService.DisableJobNotificationEmailByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByJobNotificationEmail(long id)
        {
            return systemService.GetXYZZYXsByJobNotificationEmailAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithJobNotificationEmail(JobNotificationEmailXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithJobNotificationEmailAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithJobNotificationEmail(JobNotificationEmailXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithJobNotificationEmailAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportJobNotificationEmail([FromBody] ImportJobNotificationEmailModel model)
        {
            return systemService.ImportJobNotificationEmailAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportJobNotificationEmail()
        {
            var lst = new List<ImportJobNotificationEmailModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportJobNotificationEmailModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleJobNotificationEmailImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<JobNotificationEmailExportModel>> JobNotificationEmailReport([FromQuery] JobNotificationEmailExportParameter parameter)
        {
            return reportService.ExportJobNotificationEmailAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> JobNotificationEmailAsReportCsv([FromQuery] JobNotificationEmailExportParameter parameter)
        {
            var result = await reportService.ExportJobNotificationEmailAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("JobNotificationEmailAsCsv.csv"))
                {
                    FileDownloadName = "JobNotificationEmailExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region JobTemplateCategory
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<JobTemplateCategoryModel>> GetJobTemplateCategorys([FromQuery] JobTemplateCategorySearchCriteria search)
        {
            return systemService.GetJobTemplateCategorysAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobTemplateCategoryModel>> GetJobTemplateCategory([FromQuery] long id)
        {
            return systemService.GetJobTemplateCategoryByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobTemplateCategoryModel>> CreateJobTemplateCategory([FromBody] JobTemplateCategoryCreateModel jobTemplateCategoryModel)
        {
            return systemService.CreateJobTemplateCategoryAsync(jobTemplateCategoryModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobTemplateCategoryModel>> UpdateJobTemplateCategory([FromBody] JobTemplateCategoryUpdateModel jobTemplateCategoryModel)
        {
            return systemService.UpdateJobTemplateCategoryAsync(jobTemplateCategoryModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkJobTemplateCategoryAsDeleted([FromQuery] long id)
        {
            return systemService.MarkJobTemplateCategoryAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkJobTemplateCategoryAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkJobTemplateCategoryAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsJobTemplateCategoryFileValid([FromQuery] long jobTemplateCategoryId)
        {
            return systemService.IsJobTemplateCategoryFileValidAsync(jobTemplateCategoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadJobTemplateCategoryFile([FromQuery] long jobTemplateCategoryId)
        {
            var result = await systemService.GetJobTemplateCategoryFileAsync(jobTemplateCategoryId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsJobTemplateCategorySourceWordFileValid([FromQuery] long jobTemplateCategoryId)
        {
            return systemService.IsJobTemplateCategoryWordOrPdfFileValidAsync(jobTemplateCategoryId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadJobTemplateCategorySourceWordFile([FromQuery] long jobTemplateCategoryId)
        {
            var result = await systemService.GetJobTemplateCategoryWordOrdPdfFileAsync(jobTemplateCategoryId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableJobTemplateCategory([FromBody] long id)
        {
            return systemService.EnableJobTemplateCategoryByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableJobTemplateCategory([FromBody] long id)
        {
            return systemService.DisableJobTemplateCategoryByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByJobTemplateCategory(long id)
        {
            return systemService.GetXYZZYXsByJobTemplateCategoryAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithJobTemplateCategory(JobTemplateCategoryXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithJobTemplateCategoryAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithJobTemplateCategory(JobTemplateCategoryXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithJobTemplateCategoryAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportJobTemplateCategory([FromBody] ImportJobTemplateCategoryModel model)
        {
            return systemService.ImportJobTemplateCategoryAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportJobTemplateCategory()
        {
            var lst = new List<ImportJobTemplateCategoryModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportJobTemplateCategoryModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleJobTemplateCategoryImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<JobTemplateCategoryExportModel>> JobTemplateCategoryReport([FromQuery] JobTemplateCategoryExportParameter parameter)
        {
            return reportService.ExportJobTemplateCategoryAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> JobTemplateCategoryAsReportCsv([FromQuery] JobTemplateCategoryExportParameter parameter)
        {
            var result = await reportService.ExportJobTemplateCategoryAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("JobTemplateCategoryAsCsv.csv"))
                {
                    FileDownloadName = "JobTemplateCategoryExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region JobTemplate
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<JobTemplateModel>> GetJobTemplates([FromQuery] JobTemplateSearchCriteria search)
        {
            return systemService.GetJobTemplatesAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobTemplateModel>> GetJobTemplate([FromQuery] long id)
        {
            return systemService.GetJobTemplateByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobTemplateModel>> CreateJobTemplate([FromBody] JobTemplateCreateModel jobTemplateModel)
        {
            return systemService.CreateJobTemplateAsync(jobTemplateModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobTemplateModel>> UpdateJobTemplate([FromBody] JobTemplateUpdateModel jobTemplateModel)
        {
            return systemService.UpdateJobTemplateAsync(jobTemplateModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkJobTemplateAsDeleted([FromQuery] long id)
        {
            return systemService.MarkJobTemplateAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkJobTemplateAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkJobTemplateAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsJobTemplateFileValid([FromQuery] long jobTemplateId)
        {
            return systemService.IsJobTemplateFileValidAsync(jobTemplateId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadJobTemplateFile([FromQuery] long jobTemplateId)
        {
            var result = await systemService.GetJobTemplateFileAsync(jobTemplateId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsJobTemplateSourceWordFileValid([FromQuery] long jobTemplateId)
        {
            return systemService.IsJobTemplateWordOrPdfFileValidAsync(jobTemplateId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadJobTemplateSourceWordFile([FromQuery] long jobTemplateId)
        {
            var result = await systemService.GetJobTemplateWordOrdPdfFileAsync(jobTemplateId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableJobTemplate([FromBody] long id)
        {
            return systemService.EnableJobTemplateByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableJobTemplate([FromBody] long id)
        {
            return systemService.DisableJobTemplateByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByJobTemplate(long id)
        {
            return systemService.GetXYZZYXsByJobTemplateAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithJobTemplate(JobTemplateXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithJobTemplateAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithJobTemplate(JobTemplateXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithJobTemplateAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportJobTemplate([FromBody] ImportJobTemplateModel model)
        {
            return systemService.ImportJobTemplateAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportJobTemplate()
        {
            var lst = new List<ImportJobTemplateModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportJobTemplateModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleJobTemplateImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<JobTemplateExportModel>> JobTemplateReport([FromQuery] JobTemplateExportParameter parameter)
        {
            return reportService.ExportJobTemplateAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> JobTemplateAsReportCsv([FromQuery] JobTemplateExportParameter parameter)
        {
            var result = await reportService.ExportJobTemplateAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("JobTemplateAsCsv.csv"))
                {
                    FileDownloadName = "JobTemplateExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region JobTemplateParameter
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<JobTemplateParameterModel>> GetJobTemplateParameters([FromQuery] JobTemplateParameterSearchCriteria search)
        {
            return systemService.GetJobTemplateParametersAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobTemplateParameterModel>> GetJobTemplateParameter([FromQuery] long id)
        {
            return systemService.GetJobTemplateParameterByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobTemplateParameterModel>> CreateJobTemplateParameter([FromBody] JobTemplateParameterCreateModel jobTemplateParameterModel)
        {
            return systemService.CreateJobTemplateParameterAsync(jobTemplateParameterModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<JobTemplateParameterModel>> UpdateJobTemplateParameter([FromBody] JobTemplateParameterUpdateModel jobTemplateParameterModel)
        {
            return systemService.UpdateJobTemplateParameterAsync(jobTemplateParameterModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkJobTemplateParameterAsDeleted([FromQuery] long id)
        {
            return systemService.MarkJobTemplateParameterAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkJobTemplateParameterAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkJobTemplateParameterAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsJobTemplateParameterFileValid([FromQuery] long jobTemplateParameterId)
        {
            return systemService.IsJobTemplateParameterFileValidAsync(jobTemplateParameterId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadJobTemplateParameterFile([FromQuery] long jobTemplateParameterId)
        {
            var result = await systemService.GetJobTemplateParameterFileAsync(jobTemplateParameterId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsJobTemplateParameterSourceWordFileValid([FromQuery] long jobTemplateParameterId)
        {
            return systemService.IsJobTemplateParameterWordOrPdfFileValidAsync(jobTemplateParameterId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadJobTemplateParameterSourceWordFile([FromQuery] long jobTemplateParameterId)
        {
            var result = await systemService.GetJobTemplateParameterWordOrdPdfFileAsync(jobTemplateParameterId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableJobTemplateParameter([FromBody] long id)
        {
            return systemService.EnableJobTemplateParameterByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableJobTemplateParameter([FromBody] long id)
        {
            return systemService.DisableJobTemplateParameterByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByJobTemplateParameter(long id)
        {
            return systemService.GetXYZZYXsByJobTemplateParameterAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithJobTemplateParameter(JobTemplateParameterXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithJobTemplateParameterAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithJobTemplateParameter(JobTemplateParameterXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithJobTemplateParameterAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportJobTemplateParameter([FromBody] ImportJobTemplateParameterModel model)
        {
            return systemService.ImportJobTemplateParameterAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportJobTemplateParameter()
        {
            var lst = new List<ImportJobTemplateParameterModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportJobTemplateParameterModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleJobTemplateParameterImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<JobTemplateParameterExportModel>> JobTemplateParameterReport([FromQuery] JobTemplateParameterExportParameter parameter)
        {
            return reportService.ExportJobTemplateParameterAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> JobTemplateParameterAsReportCsv([FromQuery] JobTemplateParameterExportParameter parameter)
        {
            var result = await reportService.ExportJobTemplateParameterAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("JobTemplateParameterAsCsv.csv"))
                {
                    FileDownloadName = "JobTemplateParameterExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region MessageToSend
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<MessageToSendModel>> GetMessageToSends([FromQuery] MessageToSendSearchCriteria search)
        {
            return systemService.GetMessageToSendsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<MessageToSendModel>> GetMessageToSend([FromQuery] long id)
        {
            return systemService.GetMessageToSendByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<MessageToSendModel>> CreateMessageToSend([FromBody] MessageToSendCreateModel messageToSendModel)
        {
            return systemService.CreateMessageToSendAsync(messageToSendModel);
        }
        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsMessageToSendFileValid([FromQuery] long messageToSendId)
        {
            return systemService.IsMessageToSendFileValidAsync(messageToSendId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadMessageToSendFile([FromQuery] long messageToSendId)
        {
            var result = await systemService.GetMessageToSendFileAsync(messageToSendId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsMessageToSendSourceWordFileValid([FromQuery] long messageToSendId)
        {
            return systemService.IsMessageToSendWordOrPdfFileValidAsync(messageToSendId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadMessageToSendSourceWordFile([FromQuery] long messageToSendId)
        {
            var result = await systemService.GetMessageToSendWordOrdPdfFileAsync(messageToSendId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableMessageToSend([FromBody] long id)
        {
            return systemService.EnableMessageToSendByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableMessageToSend([FromBody] long id)
        {
            return systemService.DisableMessageToSendByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByMessageToSend(long id)
        {
            return systemService.GetXYZZYXsByMessageToSendAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithMessageToSend(MessageToSendXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithMessageToSendAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithMessageToSend(MessageToSendXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithMessageToSendAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportMessageToSend([FromBody] ImportMessageToSendModel model)
        {
            return systemService.ImportMessageToSendAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportMessageToSend()
        {
            var lst = new List<ImportMessageToSendModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportMessageToSendModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleMessageToSendImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<MessageToSendExportModel>> MessageToSendReport([FromQuery] MessageToSendExportParameter parameter)
        {
            return reportService.ExportMessageToSendAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> MessageToSendAsReportCsv([FromQuery] MessageToSendExportParameter parameter)
        {
            var result = await reportService.ExportMessageToSendAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("MessageToSendAsCsv.csv"))
                {
                    FileDownloadName = "MessageToSendExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region RealtimeMessage
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<RealtimeMessageModel>> GetRealtimeMessages([FromQuery] RealtimeMessageSearchCriteria search)
        {
            return systemService.GetRealtimeMessagesAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<RealtimeMessageModel>> GetRealtimeMessage([FromQuery] long id)
        {
            return systemService.GetRealtimeMessageByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<RealtimeMessageModel>> CreateRealtimeMessage([FromBody] RealtimeMessageCreateModel realtimeMessageModel)
        {
            return systemService.CreateRealtimeMessageAsync(realtimeMessageModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<RealtimeMessageModel>> UpdateRealtimeMessage([FromBody] RealtimeMessageUpdateModel realtimeMessageModel)
        {
            return systemService.UpdateRealtimeMessageAsync(realtimeMessageModel);
        }
        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsRealtimeMessageFileValid([FromQuery] long realtimeMessageId)
        {
            return systemService.IsRealtimeMessageFileValidAsync(realtimeMessageId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadRealtimeMessageFile([FromQuery] long realtimeMessageId)
        {
            var result = await systemService.GetRealtimeMessageFileAsync(realtimeMessageId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsRealtimeMessageSourceWordFileValid([FromQuery] long realtimeMessageId)
        {
            return systemService.IsRealtimeMessageWordOrPdfFileValidAsync(realtimeMessageId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadRealtimeMessageSourceWordFile([FromQuery] long realtimeMessageId)
        {
            var result = await systemService.GetRealtimeMessageWordOrdPdfFileAsync(realtimeMessageId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableRealtimeMessage([FromBody] long id)
        {
            return systemService.EnableRealtimeMessageByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableRealtimeMessage([FromBody] long id)
        {
            return systemService.DisableRealtimeMessageByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByRealtimeMessage(long id)
        {
            return systemService.GetXYZZYXsByRealtimeMessageAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithRealtimeMessage(RealtimeMessageXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithRealtimeMessageAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithRealtimeMessage(RealtimeMessageXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithRealtimeMessageAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportRealtimeMessage([FromBody] ImportRealtimeMessageModel model)
        {
            return systemService.ImportRealtimeMessageAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportRealtimeMessage()
        {
            var lst = new List<ImportRealtimeMessageModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportRealtimeMessageModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleRealtimeMessageImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<RealtimeMessageExportModel>> RealtimeMessageReport([FromQuery] RealtimeMessageExportParameter parameter)
        {
            return reportService.ExportRealtimeMessageAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> RealtimeMessageAsReportCsv([FromQuery] RealtimeMessageExportParameter parameter)
        {
            var result = await reportService.ExportRealtimeMessageAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("RealtimeMessageAsCsv.csv"))
                {
                    FileDownloadName = "RealtimeMessageExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region SavedJob
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<SavedJobModel>> GetSavedJobs([FromQuery] SavedJobSearchCriteria search)
        {
            return systemService.GetSavedJobsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<SavedJobModel>> GetSavedJob([FromQuery] long id)
        {
            return systemService.GetSavedJobByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<SavedJobModel>> CreateSavedJob([FromBody] SavedJobCreateModel savedJobModel)
        {
            return systemService.CreateSavedJobAsync(savedJobModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<SavedJobModel>> UpdateSavedJob([FromBody] SavedJobUpdateModel savedJobModel)
        {
            return systemService.UpdateSavedJobAsync(savedJobModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkSavedJobAsDeleted([FromQuery] long id)
        {
            return systemService.MarkSavedJobAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkSavedJobAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkSavedJobAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsSavedJobFileValid([FromQuery] long savedJobId)
        {
            return systemService.IsSavedJobFileValidAsync(savedJobId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadSavedJobFile([FromQuery] long savedJobId)
        {
            var result = await systemService.GetSavedJobFileAsync(savedJobId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsSavedJobSourceWordFileValid([FromQuery] long savedJobId)
        {
            return systemService.IsSavedJobWordOrPdfFileValidAsync(savedJobId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadSavedJobSourceWordFile([FromQuery] long savedJobId)
        {
            var result = await systemService.GetSavedJobWordOrdPdfFileAsync(savedJobId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableSavedJob([FromBody] long id)
        {
            return systemService.EnableSavedJobByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableSavedJob([FromBody] long id)
        {
            return systemService.DisableSavedJobByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsBySavedJob(long id)
        {
            return systemService.GetXYZZYXsBySavedJobAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithSavedJob(SavedJobXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithSavedJobAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithSavedJob(SavedJobXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithSavedJobAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportSavedJob([FromBody] ImportSavedJobModel model)
        {
            return systemService.ImportSavedJobAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportSavedJob()
        {
            var lst = new List<ImportSavedJobModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportSavedJobModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleSavedJobImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<SavedJobExportModel>> SavedJobReport([FromQuery] SavedJobExportParameter parameter)
        {
            return reportService.ExportSavedJobAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> SavedJobAsReportCsv([FromQuery] SavedJobExportParameter parameter)
        {
            var result = await reportService.ExportSavedJobAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("SavedJobAsCsv.csv"))
                {
                    FileDownloadName = "SavedJobExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region ScheduledJob
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<ScheduledJobModel>> GetScheduledJobs([FromQuery] ScheduledJobSearchCriteria search)
        {
            return systemService.GetScheduledJobsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<ScheduledJobModel>> GetScheduledJob([FromQuery] long id)
        {
            return systemService.GetScheduledJobByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<ScheduledJobModel>> CreateScheduledJob([FromBody] ScheduledJobCreateModel scheduledJobModel)
        {
            return systemService.CreateScheduledJobAsync(scheduledJobModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<ScheduledJobModel>> UpdateScheduledJob([FromBody] ScheduledJobUpdateModel scheduledJobModel)
        {
            return systemService.UpdateScheduledJobAsync(scheduledJobModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkScheduledJobAsDeleted([FromQuery] long id)
        {
            return systemService.MarkScheduledJobAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkScheduledJobAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkScheduledJobAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsScheduledJobFileValid([FromQuery] long scheduledJobId)
        {
            return systemService.IsScheduledJobFileValidAsync(scheduledJobId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadScheduledJobFile([FromQuery] long scheduledJobId)
        {
            var result = await systemService.GetScheduledJobFileAsync(scheduledJobId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsScheduledJobSourceWordFileValid([FromQuery] long scheduledJobId)
        {
            return systemService.IsScheduledJobWordOrPdfFileValidAsync(scheduledJobId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadScheduledJobSourceWordFile([FromQuery] long scheduledJobId)
        {
            var result = await systemService.GetScheduledJobWordOrdPdfFileAsync(scheduledJobId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableScheduledJob([FromBody] long id)
        {
            return systemService.EnableScheduledJobByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableScheduledJob([FromBody] long id)
        {
            return systemService.DisableScheduledJobByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByScheduledJob(long id)
        {
            return systemService.GetXYZZYXsByScheduledJobAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithScheduledJob(ScheduledJobXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithScheduledJobAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithScheduledJob(ScheduledJobXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithScheduledJobAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportScheduledJob([FromBody] ImportScheduledJobModel model)
        {
            return systemService.ImportScheduledJobAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportScheduledJob()
        {
            var lst = new List<ImportScheduledJobModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportScheduledJobModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleScheduledJobImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<ScheduledJobExportModel>> ScheduledJobReport([FromQuery] ScheduledJobExportParameter parameter)
        {
            return reportService.ExportScheduledJobAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> ScheduledJobAsReportCsv([FromQuery] ScheduledJobExportParameter parameter)
        {
            var result = await reportService.ExportScheduledJobAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("ScheduledJobAsCsv.csv"))
                {
                    FileDownloadName = "ScheduledJobExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region AuditOperation
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<AuditOperationModel>> GetAuditOperations([FromQuery] AuditOperationSearchCriteria search)
        {
            return systemService.GetAuditOperationsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<AuditOperationModel>> GetAuditOperation([FromQuery] long id)
        {
            return systemService.GetAuditOperationByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<AuditOperationModel>> CreateAuditOperation([FromBody] AuditOperationCreateModel auditOperationModel)
        {
            return systemService.CreateAuditOperationAsync(auditOperationModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<AuditOperationModel>> UpdateAuditOperation([FromBody] AuditOperationUpdateModel auditOperationModel)
        {
            return systemService.UpdateAuditOperationAsync(auditOperationModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkAuditOperationAsDeleted([FromQuery] long id)
        {
            return systemService.MarkAuditOperationAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkAuditOperationAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkAuditOperationAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsAuditOperationFileValid([FromQuery] long auditOperationId)
        {
            return systemService.IsAuditOperationFileValidAsync(auditOperationId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadAuditOperationFile([FromQuery] long auditOperationId)
        {
            var result = await systemService.GetAuditOperationFileAsync(auditOperationId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsAuditOperationSourceWordFileValid([FromQuery] long auditOperationId)
        {
            return systemService.IsAuditOperationWordOrPdfFileValidAsync(auditOperationId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadAuditOperationSourceWordFile([FromQuery] long auditOperationId)
        {
            var result = await systemService.GetAuditOperationWordOrdPdfFileAsync(auditOperationId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableAuditOperation([FromBody] long id)
        {
            return systemService.EnableAuditOperationByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableAuditOperation([FromBody] long id)
        {
            return systemService.DisableAuditOperationByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByAuditOperation(long id)
        {
            return systemService.GetXYZZYXsByAuditOperationAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithAuditOperation(AuditOperationXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithAuditOperationAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithAuditOperation(AuditOperationXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithAuditOperationAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportAuditOperation([FromBody] ImportAuditOperationModel model)
        {
            return systemService.ImportAuditOperationAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportAuditOperation()
        {
            var lst = new List<ImportAuditOperationModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportAuditOperationModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleAuditOperationImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<AuditOperationExportModel>> AuditOperationReport([FromQuery] AuditOperationExportParameter parameter)
        {
            return reportService.ExportAuditOperationAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> AuditOperationAsReportCsv([FromQuery] AuditOperationExportParameter parameter)
        {
            var result = await reportService.ExportAuditOperationAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("AuditOperationAsCsv.csv"))
                {
                    FileDownloadName = "AuditOperationExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region AuditOperationDetail
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<AuditOperationDetailModel>> GetAuditOperationDetailByAuditOperationId([FromQuery] long auditOperationId)
        {
            return systemService.GetAuditOperationDetailByAuditOperationIdAsync(auditOperationId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> IsAuditOperationDetailAvailableByAuditOperationId([FromQuery] long auditOperationId)
        {
            return systemService.IsAuditOperationDetailAvailableByAuditOperationIdAsync(auditOperationId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<AuditOperationDetailModel>> GetAuditOperationDetails([FromQuery] AuditOperationDetailSearchCriteria search)
        {
            return systemService.GetAuditOperationDetailsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<AuditOperationDetailModel>> GetAuditOperationDetail([FromQuery] long id)
        {
            return systemService.GetAuditOperationDetailByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<AuditOperationDetailModel>> CreateAuditOperationDetail([FromBody] AuditOperationDetailCreateModel auditOperationDetailModel)
        {
            return systemService.CreateAuditOperationDetailAsync(auditOperationDetailModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<AuditOperationDetailModel>> UpdateAuditOperationDetail([FromBody] AuditOperationDetailUpdateModel auditOperationDetailModel)
        {
            return systemService.UpdateAuditOperationDetailAsync(auditOperationDetailModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkAuditOperationDetailAsDeleted([FromQuery] long id)
        {
            return systemService.MarkAuditOperationDetailAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkAuditOperationDetailAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkAuditOperationDetailAsNotDeletedByIdAsync(id);
        }
        #endregion

        #region SavedJobNotificationEmail
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<SavedJobNotificationEmailModel>> GetSavedJobNotificationEmails([FromQuery] SavedJobNotificationEmailSearchCriteria search)
        {
            return systemService.GetSavedJobNotificationEmailsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<SavedJobNotificationEmailModel>> GetSavedJobNotificationEmail([FromQuery] long id)
        {
            return systemService.GetSavedJobNotificationEmailByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<SavedJobNotificationEmailModel>> CreateSavedJobNotificationEmail([FromBody] SavedJobNotificationEmailCreateModel savedJobNotificationEmailModel)
        {
            return systemService.CreateSavedJobNotificationEmailAsync(savedJobNotificationEmailModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<SavedJobNotificationEmailModel>> UpdateSavedJobNotificationEmail([FromBody] SavedJobNotificationEmailUpdateModel savedJobNotificationEmailModel)
        {
            return systemService.UpdateSavedJobNotificationEmailAsync(savedJobNotificationEmailModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkSavedJobNotificationEmailAsDeleted([FromQuery] long id)
        {
            return systemService.MarkSavedJobNotificationEmailAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkSavedJobNotificationEmailAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkSavedJobNotificationEmailAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsSavedJobNotificationEmailFileValid([FromQuery] long savedJobNotificationEmailId)
        {
            return systemService.IsSavedJobNotificationEmailFileValidAsync(savedJobNotificationEmailId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadSavedJobNotificationEmailFile([FromQuery] long savedJobNotificationEmailId)
        {
            var result = await systemService.GetSavedJobNotificationEmailFileAsync(savedJobNotificationEmailId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsSavedJobNotificationEmailSourceWordFileValid([FromQuery] long savedJobNotificationEmailId)
        {
            return systemService.IsSavedJobNotificationEmailWordOrPdfFileValidAsync(savedJobNotificationEmailId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadSavedJobNotificationEmailSourceWordFile([FromQuery] long savedJobNotificationEmailId)
        {
            var result = await systemService.GetSavedJobNotificationEmailWordOrdPdfFileAsync(savedJobNotificationEmailId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableSavedJobNotificationEmail([FromBody] long id)
        {
            return systemService.EnableSavedJobNotificationEmailByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableSavedJobNotificationEmail([FromBody] long id)
        {
            return systemService.DisableSavedJobNotificationEmailByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsBySavedJobNotificationEmail(long id)
        {
            return systemService.GetXYZZYXsBySavedJobNotificationEmailAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithSavedJobNotificationEmail(SavedJobNotificationEmailXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithSavedJobNotificationEmailAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithSavedJobNotificationEmail(SavedJobNotificationEmailXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithSavedJobNotificationEmailAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportSavedJobNotificationEmail([FromBody] ImportSavedJobNotificationEmailModel model)
        {
            return systemService.ImportSavedJobNotificationEmailAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportSavedJobNotificationEmail()
        {
            var lst = new List<ImportSavedJobNotificationEmailModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportSavedJobNotificationEmailModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleSavedJobNotificationEmailImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<SavedJobNotificationEmailExportModel>> SavedJobNotificationEmailReport([FromQuery] SavedJobNotificationEmailExportParameter parameter)
        {
            return reportService.ExportSavedJobNotificationEmailAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> SavedJobNotificationEmailAsReportCsv([FromQuery] SavedJobNotificationEmailExportParameter parameter)
        {
            var result = await reportService.ExportSavedJobNotificationEmailAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("SavedJobNotificationEmailAsCsv.csv"))
                {
                    FileDownloadName = "SavedJobNotificationEmailExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region ScheduledJobHistory
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<ScheduledJobHistoryModel>> GetScheduledJobHistorys([FromQuery] ScheduledJobHistorySearchCriteria search)
        {
            return systemService.GetScheduledJobHistorysAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<ScheduledJobHistoryModel>> GetScheduledJobHistory([FromQuery] long id)
        {
            return systemService.GetScheduledJobHistoryByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<ScheduledJobHistoryModel>> CreateScheduledJobHistory([FromBody] ScheduledJobHistoryCreateModel scheduledJobHistoryModel)
        {
            return systemService.CreateScheduledJobHistoryAsync(scheduledJobHistoryModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<ScheduledJobHistoryModel>> UpdateScheduledJobHistory([FromBody] ScheduledJobHistoryUpdateModel scheduledJobHistoryModel)
        {
            return systemService.UpdateScheduledJobHistoryAsync(scheduledJobHistoryModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkScheduledJobHistoryAsDeleted([FromQuery] long id)
        {
            return systemService.MarkScheduledJobHistoryAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkScheduledJobHistoryAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkScheduledJobHistoryAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsScheduledJobHistoryFileValid([FromQuery] long scheduledJobHistoryId)
        {
            return systemService.IsScheduledJobHistoryFileValidAsync(scheduledJobHistoryId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadScheduledJobHistoryFile([FromQuery] long scheduledJobHistoryId)
        {
            var result = await systemService.GetScheduledJobHistoryFileAsync(scheduledJobHistoryId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsScheduledJobHistorySourceWordFileValid([FromQuery] long scheduledJobHistoryId)
        {
            return systemService.IsScheduledJobHistoryWordOrPdfFileValidAsync(scheduledJobHistoryId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadScheduledJobHistorySourceWordFile([FromQuery] long scheduledJobHistoryId)
        {
            var result = await systemService.GetScheduledJobHistoryWordOrdPdfFileAsync(scheduledJobHistoryId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableScheduledJobHistory([FromBody] long id)
        {
            return systemService.EnableScheduledJobHistoryByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableScheduledJobHistory([FromBody] long id)
        {
            return systemService.DisableScheduledJobHistoryByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByScheduledJobHistory(long id)
        {
            return systemService.GetXYZZYXsByScheduledJobHistoryAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithScheduledJobHistory(ScheduledJobHistoryXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithScheduledJobHistoryAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithScheduledJobHistory(ScheduledJobHistoryXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithScheduledJobHistoryAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportScheduledJobHistory([FromBody] ImportScheduledJobHistoryModel model)
        {
            return systemService.ImportScheduledJobHistoryAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportScheduledJobHistory()
        {
            var lst = new List<ImportScheduledJobHistoryModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportScheduledJobHistoryModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleScheduledJobHistoryImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<ScheduledJobHistoryExportModel>> ScheduledJobHistoryReport([FromQuery] ScheduledJobHistoryExportParameter parameter)
        {
            return reportService.ExportScheduledJobHistoryAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> ScheduledJobHistoryAsReportCsv([FromQuery] ScheduledJobHistoryExportParameter parameter)
        {
            var result = await reportService.ExportScheduledJobHistoryAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("ScheduledJobHistoryAsCsv.csv"))
                {
                    FileDownloadName = "ScheduledJobHistoryExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region ScheduledJobNotificationEmail
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<ScheduledJobNotificationEmailModel>> GetScheduledJobNotificationEmails([FromQuery] ScheduledJobNotificationEmailSearchCriteria search)
        {
            return systemService.GetScheduledJobNotificationEmailsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<ScheduledJobNotificationEmailModel>> GetScheduledJobNotificationEmail([FromQuery] long id)
        {
            return systemService.GetScheduledJobNotificationEmailByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<ScheduledJobNotificationEmailModel>> CreateScheduledJobNotificationEmail([FromBody] ScheduledJobNotificationEmailCreateModel scheduledJobNotificationEmailModel)
        {
            return systemService.CreateScheduledJobNotificationEmailAsync(scheduledJobNotificationEmailModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<ScheduledJobNotificationEmailModel>> UpdateScheduledJobNotificationEmail([FromBody] ScheduledJobNotificationEmailUpdateModel scheduledJobNotificationEmailModel)
        {
            return systemService.UpdateScheduledJobNotificationEmailAsync(scheduledJobNotificationEmailModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkScheduledJobNotificationEmailAsDeleted([FromQuery] long id)
        {
            return systemService.MarkScheduledJobNotificationEmailAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkScheduledJobNotificationEmailAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkScheduledJobNotificationEmailAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsScheduledJobNotificationEmailFileValid([FromQuery] long scheduledJobNotificationEmailId)
        {
            return systemService.IsScheduledJobNotificationEmailFileValidAsync(scheduledJobNotificationEmailId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadScheduledJobNotificationEmailFile([FromQuery] long scheduledJobNotificationEmailId)
        {
            var result = await systemService.GetScheduledJobNotificationEmailFileAsync(scheduledJobNotificationEmailId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsScheduledJobNotificationEmailSourceWordFileValid([FromQuery] long scheduledJobNotificationEmailId)
        {
            return systemService.IsScheduledJobNotificationEmailWordOrPdfFileValidAsync(scheduledJobNotificationEmailId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadScheduledJobNotificationEmailSourceWordFile([FromQuery] long scheduledJobNotificationEmailId)
        {
            var result = await systemService.GetScheduledJobNotificationEmailWordOrdPdfFileAsync(scheduledJobNotificationEmailId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableScheduledJobNotificationEmail([FromBody] long id)
        {
            return systemService.EnableScheduledJobNotificationEmailByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableScheduledJobNotificationEmail([FromBody] long id)
        {
            return systemService.DisableScheduledJobNotificationEmailByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByScheduledJobNotificationEmail(long id)
        {
            return systemService.GetXYZZYXsByScheduledJobNotificationEmailAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithScheduledJobNotificationEmail(ScheduledJobNotificationEmailXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithScheduledJobNotificationEmailAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithScheduledJobNotificationEmail(ScheduledJobNotificationEmailXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithScheduledJobNotificationEmailAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportScheduledJobNotificationEmail([FromBody] ImportScheduledJobNotificationEmailModel model)
        {
            return systemService.ImportScheduledJobNotificationEmailAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportScheduledJobNotificationEmail()
        {
            var lst = new List<ImportScheduledJobNotificationEmailModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportScheduledJobNotificationEmailModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleScheduledJobNotificationEmailImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<ScheduledJobNotificationEmailExportModel>> ScheduledJobNotificationEmailReport([FromQuery] ScheduledJobNotificationEmailExportParameter parameter)
        {
            return reportService.ExportScheduledJobNotificationEmailAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> ScheduledJobNotificationEmailAsReportCsv([FromQuery] ScheduledJobNotificationEmailExportParameter parameter)
        {
            var result = await reportService.ExportScheduledJobNotificationEmailAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("ScheduledJobNotificationEmailAsCsv.csv"))
                {
                    FileDownloadName = "ScheduledJobNotificationEmailExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region CompanyStorageConfiguration
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<CompanyStorageConfigurationModel>> GetCompanyStorageConfiguration([FromQuery] long id)
        {
            return systemService.GetCompanyStorageConfigurationByIdForReadAsync(id,allowOnlySuperAdmin: true);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<CompanyStorageConfigurationModel>> GetCompanyStorageConfigurationByCompanyId([FromQuery] long companyId)
        {
            return systemService.GetCompanyStorageConfigurationByCompanyIdAsync(allowOnlySuperAdmin: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DoesCompanyStorageConfigurationByCompanyIdExists()
        {
            return systemService.DoesCompanyStorageConfigurationByCompanyIdExistsAsync();
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<CompanyStorageConfigurationModel>> CreateCompanyStorageConfiguration([FromBody] CompanyStorageConfigurationCreateModel companyStorageConfigurationModel)
        {
            return systemService.CreateCompanyStorageConfigurationAsync(companyStorageConfigurationModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<CompanyStorageConfigurationModel>> UpdateCompanyStorageConfiguration([FromBody] CompanyStorageConfigurationUpdateModel companyStorageConfigurationModel)
        {
            return systemService.UpdateCompanyStorageConfigurationAsync(companyStorageConfigurationModel);
        }
        #endregion

        #region MailServer
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<MailServerModel>> GetMailServers([FromQuery] MailServerSearchCriteria search)
        {
            return systemService.GetMailServersAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<MailServerModel>> GetMailServer([FromQuery] long id)
        {
            return systemService.GetMailServerByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<MailServerModel>> CreateMailServer([FromBody] MailServerCreateModel mailServerModel)
        {
            return systemService.CreateMailServerAsync(mailServerModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<MailServerModel>> UpdateMailServer([FromBody] MailServerUpdateModel mailServerModel)
        {
            return systemService.UpdateMailServerAsync(mailServerModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkMailServerAsDeleted([FromQuery] long id)
        {
            return systemService.MarkMailServerAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkMailServerAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkMailServerAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsMailServerFileValid([FromQuery] long mailServerId)
        {
            return systemService.IsMailServerFileValidAsync(mailServerId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadMailServerFile([FromQuery] long mailServerId)
        {
            var result = await systemService.GetMailServerFileAsync(mailServerId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsMailServerSourceWordFileValid([FromQuery] long mailServerId)
        {
            return systemService.IsMailServerWordOrPdfFileValidAsync(mailServerId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadMailServerSourceWordFile([FromQuery] long mailServerId)
        {
            var result = await systemService.GetMailServerWordOrdPdfFileAsync(mailServerId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableMailServer([FromBody] long id)
        {
            return systemService.EnableMailServerByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableMailServer([FromBody] long id)
        {
            return systemService.DisableMailServerByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByMailServer(long id)
        {
            return systemService.GetXYZZYXsByMailServerAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithMailServer(MailServerXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithMailServerAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithMailServer(MailServerXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithMailServerAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportMailServer([FromBody] ImportMailServerModel model)
        {
            return systemService.ImportMailServerAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportMailServer()
        {
            var lst = new List<ImportMailServerModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportMailServerModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleMailServerImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<MailServerExportModel>> MailServerReport([FromQuery] MailServerExportParameter parameter)
        {
            return reportService.ExportMailServerAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> MailServerAsReportCsv([FromQuery] MailServerExportParameter parameter)
        {
            var result = await reportService.ExportMailServerAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("MailServerAsCsv.csv"))
                {
                    FileDownloadName = "MailServerExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region SmsServer
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<SmsServerModel>> GetSmsServers([FromQuery] SmsServerSearchCriteria search)
        {
            return systemService.GetSmsServersAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<SmsServerModel>> GetSmsServer([FromQuery] long id)
        {
            return systemService.GetSmsServerByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<SmsServerModel>> CreateSmsServer([FromBody] SmsServerCreateModel smsServerModel)
        {
            return systemService.CreateSmsServerAsync(smsServerModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<SmsServerModel>> UpdateSmsServer([FromBody] SmsServerUpdateModel smsServerModel)
        {
            return systemService.UpdateSmsServerAsync(smsServerModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkSmsServerAsDeleted([FromQuery] long id)
        {
            return systemService.MarkSmsServerAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkSmsServerAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkSmsServerAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsSmsServerFileValid([FromQuery] long smsServerId)
        {
            return systemService.IsSmsServerFileValidAsync(smsServerId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadSmsServerFile([FromQuery] long smsServerId)
        {
            var result = await systemService.GetSmsServerFileAsync(smsServerId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsSmsServerSourceWordFileValid([FromQuery] long smsServerId)
        {
            return systemService.IsSmsServerWordOrPdfFileValidAsync(smsServerId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadSmsServerSourceWordFile([FromQuery] long smsServerId)
        {
            var result = await systemService.GetSmsServerWordOrdPdfFileAsync(smsServerId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableSmsServer([FromBody] long id)
        {
            return systemService.EnableSmsServerByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableSmsServer([FromBody] long id)
        {
            return systemService.DisableSmsServerByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsBySmsServer(long id)
        {
            return systemService.GetXYZZYXsBySmsServerAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithSmsServer(SmsServerXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithSmsServerAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithSmsServer(SmsServerXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithSmsServerAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportSmsServer([FromBody] ImportSmsServerModel model)
        {
            return systemService.ImportSmsServerAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportSmsServer()
        {
            var lst = new List<ImportSmsServerModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportSmsServerModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleSmsServerImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<SmsServerExportModel>> SmsServerReport([FromQuery] SmsServerExportParameter parameter)
        {
            return reportService.ExportSmsServerAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> SmsServerAsReportCsv([FromQuery] SmsServerExportParameter parameter)
        {
            var result = await reportService.ExportSmsServerAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("SmsServerAsCsv.csv"))
                {
                    FileDownloadName = "SmsServerExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region SingleSignOnServer
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<SingleSignOnServerModel>> GetSingleSignOnServers([FromQuery] SingleSignOnServerSearchCriteria search)
        {
            return systemService.GetSingleSignOnServersAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<SingleSignOnServerModel>> GetSingleSignOnServer([FromQuery] long id)
        {
            return systemService.GetSingleSignOnServerByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<SingleSignOnServerModel>> CreateSingleSignOnServer([FromBody] SingleSignOnServerCreateModel singleSignOnServerModel)
        {
            return systemService.CreateSingleSignOnServerAsync(singleSignOnServerModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<SingleSignOnServerModel>> UpdateSingleSignOnServer([FromBody] SingleSignOnServerUpdateModel singleSignOnServerModel)
        {
            return systemService.UpdateSingleSignOnServerAsync(singleSignOnServerModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkSingleSignOnServerAsDeleted([FromQuery] long id)
        {
            return systemService.MarkSingleSignOnServerAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkSingleSignOnServerAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkSingleSignOnServerAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsSingleSignOnServerFileValid([FromQuery] long singleSignOnServerId)
        {
            return systemService.IsSingleSignOnServerFileValidAsync(singleSignOnServerId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadSingleSignOnServerFile([FromQuery] long singleSignOnServerId)
        {
            var result = await systemService.GetSingleSignOnServerFileAsync(singleSignOnServerId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsSingleSignOnServerSourceWordFileValid([FromQuery] long singleSignOnServerId)
        {
            return systemService.IsSingleSignOnServerWordOrPdfFileValidAsync(singleSignOnServerId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadSingleSignOnServerSourceWordFile([FromQuery] long singleSignOnServerId)
        {
            var result = await systemService.GetSingleSignOnServerWordOrdPdfFileAsync(singleSignOnServerId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableSingleSignOnServer([FromBody] long id)
        {
            return systemService.EnableSingleSignOnServerByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableSingleSignOnServer([FromBody] long id)
        {
            return systemService.DisableSingleSignOnServerByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsBySingleSignOnServer(long id)
        {
            return systemService.GetXYZZYXsBySingleSignOnServerAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithSingleSignOnServer(SingleSignOnServerXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithSingleSignOnServerAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithSingleSignOnServer(SingleSignOnServerXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithSingleSignOnServerAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportSingleSignOnServer([FromBody] ImportSingleSignOnServerModel model)
        {
            return systemService.ImportSingleSignOnServerAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportSingleSignOnServer()
        {
            var lst = new List<ImportSingleSignOnServerModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportSingleSignOnServerModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleSingleSignOnServerImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<SingleSignOnServerExportModel>> SingleSignOnServerReport([FromQuery] SingleSignOnServerExportParameter parameter)
        {
            return reportService.ExportSingleSignOnServerAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> SingleSignOnServerAsReportCsv([FromQuery] SingleSignOnServerExportParameter parameter)
        {
            var result = await reportService.ExportSingleSignOnServerAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("SingleSignOnServerAsCsv.csv"))
                {
                    FileDownloadName = "SingleSignOnServerExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region PushNotificationServer
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<PushNotificationServerModel>> GetPushNotificationServers([FromQuery] PushNotificationServerSearchCriteria search)
        {
            return systemService.GetPushNotificationServersAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<PushNotificationServerModel>> GetPushNotificationServer([FromQuery] long id)
        {
            return systemService.GetPushNotificationServerByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<PushNotificationServerModel>> CreatePushNotificationServer([FromBody] PushNotificationServerCreateModel pushNotificationConfigurationModel)
        {
            return systemService.CreatePushNotificationServerAsync(pushNotificationConfigurationModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<PushNotificationServerModel>> CreateMultiplePushNotificationServer([FromBody] MultiplePushNotificationServerCreateModel pushNotificationConfigurationModels)
        {
            return systemService.CreateMultiplePushNotificationServerAsync(pushNotificationConfigurationModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<PushNotificationServerModel>> UpdatePushNotificationServer([FromBody] PushNotificationServerUpdateModel pushNotificationConfigurationModel)
        {
            return systemService.UpdatePushNotificationServerAsync(pushNotificationConfigurationModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkPushNotificationServerAsDeleted([FromQuery] long id)
        {
            return systemService.MarkPushNotificationServerAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkPushNotificationServerAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkPushNotificationServerAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsPushNotificationServerFileValid([FromQuery] long pushNotificationConfigurationId)
        {
            return systemService.IsPushNotificationServerFileValidAsync(pushNotificationConfigurationId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadPushNotificationServerFile([FromQuery] long pushNotificationConfigurationId)
        {
            var result = await systemService.GetPushNotificationServerFileAsync(pushNotificationConfigurationId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadPushNotificationServerFile([FromForm] PushNotificationServerFileWithFileFormModel model)
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

            return productService.UploadPushNotificationServerFileAsync(model.PushNotificationServerId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearPushNotificationServerFile([FromQuery] long movieId)
        {
            return productService.ClearPushNotificationServerFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsPushNotificationServerSourceWordFileValid([FromQuery] long pushNotificationConfigurationId)
        {
            return systemService.IsPushNotificationServerWordOrPdfFileValidAsync(pushNotificationConfigurationId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadPushNotificationServerSourceWordFile([FromQuery] long pushNotificationConfigurationId)
        {
            var result = await systemService.GetPushNotificationServerWordOrdPdfFileAsync(pushNotificationConfigurationId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnablePushNotificationServer([FromBody] long id)
        {
            return systemService.EnablePushNotificationServerByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisablePushNotificationServer([FromBody] long id)
        {
            return systemService.DisablePushNotificationServerByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByPushNotificationServer(long id)
        {
            return systemService.GetXYZZYXsByPushNotificationServerAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithPushNotificationServer(PushNotificationServerXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithPushNotificationServerAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithPushNotificationServer(PushNotificationServerXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithPushNotificationServerAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportPushNotificationServer([FromBody] ImportPushNotificationServerModel model)
        {
            return systemService.ImportPushNotificationServerAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportPushNotificationServer()
        {
            var lst = new List<ImportPushNotificationServerModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportPushNotificationServerModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SamplePushNotificationServerImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<PushNotificationServerExportModel>> PushNotificationServerReport([FromQuery] PushNotificationServerExportParameter parameter)
        {
            return reportService.ExportPushNotificationServerAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> PushNotificationServerAsReportCsv([FromQuery] PushNotificationServerExportParameter parameter)
        {
            var result = await reportService.ExportPushNotificationServerAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("PushNotificationServerAsCsv.csv"))
                {
                    FileDownloadName = "PushNotificationServerExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region VoiceNotificationServer
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VoiceNotificationServerModel>> GetVoiceNotificationServers([FromQuery] VoiceNotificationServerSearchCriteria search)
        {
            return systemService.GetVoiceNotificationServersAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<VoiceNotificationServerModel>> GetVoiceNotificationServer([FromQuery] long id)
        {
            return systemService.GetVoiceNotificationServerByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<VoiceNotificationServerModel>> CreateVoiceNotificationServer([FromBody] VoiceNotificationServerCreateModel voiceNotificationServerModel)
        {
            return systemService.CreateVoiceNotificationServerAsync(voiceNotificationServerModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<VoiceNotificationServerModel>> CreateMultipleVoiceNotificationServer([FromBody] MultipleVoiceNotificationServerCreateModel voiceNotificationServerModels)
        {
            return systemService.CreateMultipleVoiceNotificationServerAsync(voiceNotificationServerModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<VoiceNotificationServerModel>> UpdateVoiceNotificationServer([FromBody] VoiceNotificationServerUpdateModel voiceNotificationServerModel)
        {
            return systemService.UpdateVoiceNotificationServerAsync(voiceNotificationServerModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkVoiceNotificationServerAsDeleted([FromQuery] long id)
        {
            return systemService.MarkVoiceNotificationServerAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkVoiceNotificationServerAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkVoiceNotificationServerAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsVoiceNotificationServerFileValid([FromQuery] long voiceNotificationServerId)
        {
            return systemService.IsVoiceNotificationServerFileValidAsync(voiceNotificationServerId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadVoiceNotificationServerFile([FromQuery] long voiceNotificationServerId)
        {
            var result = await systemService.GetVoiceNotificationServerFileAsync(voiceNotificationServerId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadVoiceNotificationServerFile([FromForm] VoiceNotificationServerFileWithFileFormModel model)
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

            return productService.UploadVoiceNotificationServerFileAsync(model.VoiceNotificationServerId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearVoiceNotificationServerFile([FromQuery] long movieId)
        {
            return productService.ClearVoiceNotificationServerFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsVoiceNotificationServerSourceWordFileValid([FromQuery] long voiceNotificationServerId)
        {
            return systemService.IsVoiceNotificationServerWordOrPdfFileValidAsync(voiceNotificationServerId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadVoiceNotificationServerSourceWordFile([FromQuery] long voiceNotificationServerId)
        {
            var result = await systemService.GetVoiceNotificationServerWordOrdPdfFileAsync(voiceNotificationServerId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableVoiceNotificationServer([FromBody] long id)
        {
            return systemService.EnableVoiceNotificationServerByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableVoiceNotificationServer([FromBody] long id)
        {
            return systemService.DisableVoiceNotificationServerByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByVoiceNotificationServer(long id)
        {
            return systemService.GetXYZZYXsByVoiceNotificationServerAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithVoiceNotificationServer(VoiceNotificationServerXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithVoiceNotificationServerAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithVoiceNotificationServer(VoiceNotificationServerXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithVoiceNotificationServerAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportVoiceNotificationServer([FromBody] ImportVoiceNotificationServerModel model)
        {
            return systemService.ImportVoiceNotificationServerAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportVoiceNotificationServer()
        {
            var lst = new List<ImportVoiceNotificationServerModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportVoiceNotificationServerModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleVoiceNotificationServerImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<VoiceNotificationServerExportModel>> VoiceNotificationServerReport([FromQuery] VoiceNotificationServerExportParameter parameter)
        {
            return reportService.ExportVoiceNotificationServerAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> VoiceNotificationServerAsReportCsv([FromQuery] VoiceNotificationServerExportParameter parameter)
        {
            var result = await reportService.ExportVoiceNotificationServerAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("VoiceNotificationServerAsCsv.csv"))
                {
                    FileDownloadName = "VoiceNotificationServerExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region WhatsAppNotificationServer
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<WhatsAppNotificationServerModel>> GetWhatsAppNotificationServers([FromQuery] WhatsAppNotificationServerSearchCriteria search)
        {
            return systemService.GetWhatsAppNotificationServersAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<WhatsAppNotificationServerModel>> GetWhatsAppNotificationServer([FromQuery] long id)
        {
            return systemService.GetWhatsAppNotificationServerByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<WhatsAppNotificationServerModel>> CreateWhatsAppNotificationServer([FromBody] WhatsAppNotificationServerCreateModel whatsAppNotificationServerModel)
        {
            return systemService.CreateWhatsAppNotificationServerAsync(whatsAppNotificationServerModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<WhatsAppNotificationServerModel>> CreateMultipleWhatsAppNotificationServer([FromBody] MultipleWhatsAppNotificationServerCreateModel whatsAppNotificationServerModels)
        {
            return systemService.CreateMultipleWhatsAppNotificationServerAsync(whatsAppNotificationServerModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<WhatsAppNotificationServerModel>> UpdateWhatsAppNotificationServer([FromBody] WhatsAppNotificationServerUpdateModel whatsAppNotificationServerModel)
        {
            return systemService.UpdateWhatsAppNotificationServerAsync(whatsAppNotificationServerModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkWhatsAppNotificationServerAsDeleted([FromQuery] long id)
        {
            return systemService.MarkWhatsAppNotificationServerAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkWhatsAppNotificationServerAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkWhatsAppNotificationServerAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsWhatsAppNotificationServerFileValid([FromQuery] long whatsAppNotificationServerId)
        {
            return systemService.IsWhatsAppNotificationServerFileValidAsync(whatsAppNotificationServerId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadWhatsAppNotificationServerFile([FromQuery] long whatsAppNotificationServerId)
        {
            var result = await systemService.GetWhatsAppNotificationServerFileAsync(whatsAppNotificationServerId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UploadWhatsAppNotificationServerFile([FromForm] WhatsAppNotificationServerFileWithFileFormModel model)
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

            return productService.UploadWhatsAppNotificationServerFileAsync(model.WhatsAppNotificationServerId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> ClearWhatsAppNotificationServerFile([FromQuery] long movieId)
        {
            return productService.ClearWhatsAppNotificationServerFileAsync(movieId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> IsWhatsAppNotificationServerSourceWordFileValid([FromQuery] long whatsAppNotificationServerId)
        {
            return systemService.IsWhatsAppNotificationServerWordOrPdfFileValidAsync(whatsAppNotificationServerId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public async Task<IActionResult> DownloadWhatsAppNotificationServerSourceWordFile([FromQuery] long whatsAppNotificationServerId)
        {
            var result = await systemService.GetWhatsAppNotificationServerWordOrdPdfFileAsync(whatsAppNotificationServerId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> EnableWhatsAppNotificationServer([FromBody] long id)
        {
            return systemService.EnableWhatsAppNotificationServerByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> DisableWhatsAppNotificationServer([FromBody] long id)
        {
            return systemService.DisableWhatsAppNotificationServerByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByWhatsAppNotificationServer(long id)
        {
            return systemService.GetXYZZYXsByWhatsAppNotificationServerAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithWhatsAppNotificationServer(WhatsAppNotificationServerXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithWhatsAppNotificationServerAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithWhatsAppNotificationServer(WhatsAppNotificationServerXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithWhatsAppNotificationServerAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportWhatsAppNotificationServer([FromBody] ImportWhatsAppNotificationServerModel model)
        {
            return systemService.ImportWhatsAppNotificationServerAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportWhatsAppNotificationServer()
        {
            var lst = new List<ImportWhatsAppNotificationServerModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportWhatsAppNotificationServerModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleWhatsAppNotificationServerImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<WhatsAppNotificationServerExportModel>> WhatsAppNotificationServerReport([FromQuery] WhatsAppNotificationServerExportParameter parameter)
        {
            return reportService.ExportWhatsAppNotificationServerAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> WhatsAppNotificationServerAsReportCsv([FromQuery] WhatsAppNotificationServerExportParameter parameter)
        {
            var result = await reportService.ExportWhatsAppNotificationServerAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("WhatsAppNotificationServerAsCsv.csv"))
                {
                    FileDownloadName = "WhatsAppNotificationServerExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region MessageTemplate
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<MessageTemplateModel>> GetMessageTemplates([FromQuery] MessageTemplateSearchCriteria search)
        {
            return systemService.GetMessageTemplatesAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<MessageTemplateModel>> GetMessageTemplate([FromQuery] long id)
        {
            return systemService.GetMessageTemplateByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<MessageTemplateModel>> CreateMessageTemplate([FromBody] MessageTemplateCreateModel messageTemplateModel)
        {
            return systemService.CreateMessageTemplateAsync(messageTemplateModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<MessageTemplateModel>> UpdateMessageTemplate([FromBody] MessageTemplateUpdateModel messageTemplateModel)
        {
            return systemService.UpdateMessageTemplateAsync(messageTemplateModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkMessageTemplateAsDeleted([FromQuery] long id)
        {
            return systemService.MarkMessageTemplateAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> MarkMessageTemplateAsNotDeleted([FromBody] long id)
        {
            return systemService.MarkMessageTemplateAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsMessageTemplateFileValid([FromQuery] long messageTemplateId)
        {
            return systemService.IsMessageTemplateFileValidAsync(messageTemplateId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadMessageTemplateFile([FromQuery] long messageTemplateId)
        {
            var result = await systemService.GetMessageTemplateFileAsync(messageTemplateId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsMessageTemplateSourceWordFileValid([FromQuery] long messageTemplateId)
        {
            return systemService.IsMessageTemplateWordOrPdfFileValidAsync(messageTemplateId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadMessageTemplateSourceWordFile([FromQuery] long messageTemplateId)
        {
            var result = await systemService.GetMessageTemplateWordOrdPdfFileAsync(messageTemplateId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> EnableMessageTemplate([FromBody] long id)
        {
            return systemService.EnableMessageTemplateByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> DisableMessageTemplate([FromBody] long id)
        {
            return systemService.DisableMessageTemplateByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByMessageTemplate(long id)
        {
            return systemService.GetXYZZYXsByMessageTemplateAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithMessageTemplate(MessageTemplateXYZZYXAssociationModel model)
        {
            return systemService.AssociateXYZZYXWithMessageTemplateAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithMessageTemplate(MessageTemplateXYZZYXAssociationModel model)
        {
            return systemService.UnAssociateXYZZYXWithMessageTemplateAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportMessageTemplate([FromBody] ImportMessageTemplateModel model)
        {
            return systemService.ImportMessageTemplateAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportMessageTemplate()
        {
            var lst = new List<ImportMessageTemplateModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportMessageTemplateModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleMessageTemplateImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<MessageTemplateExportModel>> MessageTemplateReport([FromQuery] MessageTemplateExportParameter parameter)
        {
            return reportService.ExportMessageTemplateAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> MessageTemplateAsReportCsv([FromQuery] MessageTemplateExportParameter parameter)
        {
            var result = await reportService.ExportMessageTemplateAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("MessageTemplateAsCsv.csv"))
                {
                    FileDownloadName = "MessageTemplateExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region System Functions
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "System" })]
        public Task<ReturnModel<SystemSummaryModel>> GetAppDetails()
        {
            return systemService.GetSystemSummaryAsync();
        }
        #endregion
    }
}
