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
    public partial class BaseController : Controller
    {
        protected readonly ILog log;
        public BaseController(ILog log)
        {
            this.log = log;
        }

        protected IActionResult GetImageFileResult(ReturnModel<FileStreamModel> result, string fileName)
        {
            if (result.IsSuccess)
                return File(result.Model.FileData, result.Model.ContentType, result.Model.FileName, false);

            var path = Path.GetFullPath(FileUtils.GetApplicationExeDirectory() + "/"+ fileName);

            if (System.IO.File.Exists(path))
                return PhysicalFile(path, FileUtils.GetMimeTypeFromFileName(path));

            path = Path.GetFullPath(FileUtils.GetApplicationExeDirectory() + "/Images/" + fileName);

            if (System.IO.File.Exists(path))
                return PhysicalFile(path, FileUtils.GetMimeTypeFromFileName(path));

            log.Error("Default Profile File does not exists in " + path);

            return StatusCode(404);

        }
    }
}