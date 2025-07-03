using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Manifest.Common.Enums;
using Manifest.Common.Models.DBView;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.ViewModels;
using Manifest.Web.Filters;
using Framework.Infrastructure.Models.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
using Framework.Infrastructure.Utils;

namespace Manifest.Api.All.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public partial class CompanyController : BaseController
    {
        private readonly CompanyService companyService;
        private readonly CompanyService wipService;

        public CompanyController(CompanyService companyService, ILog log)
            : base(log)
        {
            this.companyService = companyService;
            wipService = companyService;
        }

        #region User
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<bool>> UploadUserPicture([FromForm] UserPictureWithFileFormModel model)
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

            return companyService.UploadUserPictureAsync(model.UserId, streamModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<bool>> ClearUserPicture([FromQuery] long userId)
        {
            return companyService.ClearUserPictureAsync(userId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public async Task<IActionResult> GetUserPicture([FromQuery] long userId, [FromQuery] string profileGuid)
        {
            var result = await companyService.GetUserPictureStreamAsync(userId);
            return GetImageFileResult(result, "default-user-picture.png");
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnListModel<UserModel>> GetUsers([FromQuery] UserSearchCriteria search)
        {
            return companyService.GetUsersAsync(search);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<UserModel>> CreateUser([FromForm] UserCreateWithFileFormModel userModel)
        {
            FileStreamModel streamModel = null;
            if (userModel.UploadedFile != null)
            {
                streamModel = new FileStreamModel
                {
                    FileData = userModel.UploadedFile.OpenReadStream(),
                    ContentType = userModel.UploadedFile.ContentType,
                    FileName = userModel.UploadedFile.FileName
                };
            }
            return companyService.CreateUserAsync(userModel, streamModel);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<UserModel>> GetUser([FromQuery] long id)
        {
            return companyService.GetUserByIdAsync(id);
        }


        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<UserModel>> UpdateUserByAdmin([FromForm] UserUpdateWithFileFormModel userModel)
        {
            FileStreamModel streamModel = null;
            if (userModel.UploadedFile != null)
            {
                streamModel = new FileStreamModel
                {
                    FileData = userModel.UploadedFile.OpenReadStream(),
                    ContentType = userModel.UploadedFile.ContentType,
                    FileName = userModel.UploadedFile.FileName
                };
            }
            return companyService.UpdateUserAsync(userModel, streamModel);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<bool>> EnableUserById([FromQuery] long userId)
        {
            return companyService.EnableUserByIdAsync(userId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<bool>> DisableUserById([FromQuery] long userId)
        {
            return companyService.DisableUserByIdAsync(userId);
        }
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<bool>> MarkUserAsDeleted([FromQuery] long id)
        {
            return companyService.MarkUserAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<bool>> MarkUserAsNotDeleted([FromBody] long id)
        {
            return companyService.MarkUserAsNotDeletedByIdAsync(id);
        }
        #endregion User

        #region UserPermission
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnListModel<UserPermissionModel>> GetUserPermissions([FromQuery] UserPermissionSearchCriteria search)
        {
            return companyService.GetUserPermissionsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<UserPermissionModel>> GetUserPermission([FromQuery] long id)
        {
            return companyService.GetUserPermissionByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<UserPermissionModel>> CreateUserPermission([FromBody] UserPermissionCreateModel userPermissionModel)
        {
            return companyService.CreateUserPermissionAsync(userPermissionModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<UserPermissionModel>> UpdateUserPermission([FromBody] UserPermissionUpdateModel userPermissionModel)
        {
            return companyService.UpdateUserPermissionAsync(userPermissionModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<bool>> MarkUserPermissionAsDeleted([FromQuery] long id)
        {
            return companyService.MarkUserPermissionAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<bool>> MarkUserPermissionAsNotDeleted([FromBody] long id)
        {
            return companyService.MarkUserPermissionAsNotDeletedByIdAsync(id);
        }

        /*
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsUserPermissionFileValid([FromQuery] long userPermissionId)
        {
            return companyService.IsUserPermissionFileValidAsync(userPermissionId);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadUserPermissionFile([FromQuery] long userPermissionId)
        {
            var result = await companyService.GetUserPermissionFileAsync(userPermissionId);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public Task<ReturnModel<bool>> IsUserPermissionSourceWordFileValid([FromQuery] long userPermissionId)
        {
            return companyService.IsUserPermissionWordOrPdfFileValidAsync(userPermissionId,isPdf: false);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Template" })]
        public async Task<IActionResult> DownloadUserPermissionSourceWordFile([FromQuery] long userPermissionId)
        {
            var result = await companyService.GetUserPermissionWordOrdPdfFileAsync(userPermissionId, isPdf: false);
            if (!result.IsSuccess)
                return new NotFoundResult();
            return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<bool>> EnableUserPermission([FromBody] long id)
        {
            return companyService.EnableUserPermissionByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<bool>> DisableUserPermission([FromBody] long id)
        {
            return companyService.DisableUserPermissionByIdAsync(id);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnListModel<XYZZYXModel>> GetXYZZYXsByUserPermission(long id)
        {
            return companyService.GetXYZZYXsByUserPermissionAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<bool>> AssociateXYZZYXWithUserPermission(UserPermissionXYZZYXAssociationModel model)
        {
            return companyService.AssociateXYZZYXWithUserPermissionAsync(model);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Company" })]
        public Task<ReturnModel<bool>> UnAssociateXYZZYXWithUserPermission(UserPermissionXYZZYXAssociationModel model)
        {
            return companyService.UnAssociateXYZZYXWithUserPermissionAsync(model);
        }

        [HttpPost]
        public Task<ReturnListModel<ImportStatusModel>> ImportUserPermission([FromBody] ImportUserPermissionModel model)
        {
            return companyService.ImportUserPermissionAsync(model, isTest:false);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSampleImportUserPermission()
        {
            var lst = new List<ImportUserPermissionModel>();
            for (int i = 1; i <= 11; i++)
            {
                lst.Add(new ImportUserPermissionModel { });
            }

            return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(lst), FileUtils.GetMimeTypeFromFileName("File.csv"))
            {
                FileDownloadName = "SampleUserPermissionImportFile.csv"
            };
        }

        [HttpGet]
        public Task<ReturnListModel<UserPermissionExportModel>> UserPermissionReport([FromQuery] UserPermissionExportParameter parameter)
        {
            return reportService.ExportUserPermissionAsync(parameter, isTest);
        }

        [HttpGet]
        public async Task<IActionResult> UserPermissionAsReportCsv([FromQuery] UserPermissionExportParameter parameter)
        {
            var result = await reportService.ExportUserPermissionAsync(parameter, isTest);
            if (result.IsSuccess)
            {
                return new FileStreamResult(await CsvUtils.GetCsvFileContentAsync(result.Model), FileUtils.GetMimeTypeFromFileName("UserPermissionAsCsv.csv"))
                {
                    FileDownloadName = "UserPermissionExport.csv"
                };
            }

            return Json(result);
        }
        */
        #endregion

        #region UserGroup
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserGroupModel>> GetUserGroups([FromQuery] UserGroupSearchCriteria search)
        {
            return wipService.GetUserGroupsAsync(search);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<UserGroupModel>> GetUserGroup([FromQuery] long id)
        {
            return wipService.GetUserGroupByIdForReadAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<UserGroupModel>> CreateUserGroup([FromBody] UserGroupCreateModel userGroupModel)
        {
            return wipService.CreateUserGroupAsync(userGroupModel);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnListModel<UserGroupModel>> CreateMultipleUserGroup([FromBody] MultipleUserGroupCreateModel userGroupModels)
        {
            return wipService.CreateMultipleUserGroupAsync(userGroupModels);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<UserGroupModel>> UpdateUserGroup([FromBody] UserGroupUpdateModel userGroupModel)
        {
            return wipService.UpdateUserGroupAsync(userGroupModel);
        }

        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkUserGroupAsDeleted([FromQuery] long id)
        {
            return wipService.MarkUserGroupAsDeletedByIdAsync(id);
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "ToTest" })]
        public Task<ReturnModel<bool>> MarkUserGroupAsNotDeleted([FromBody] long id)
        {
            return wipService.MarkUserGroupAsNotDeletedByIdAsync(id);
        }
        #endregion
    }
}