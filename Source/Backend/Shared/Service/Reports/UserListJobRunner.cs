using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.SearchCriteria.System.Parameters;
using Manifest.Common.Models.ViewModels.ImportExportModels;
using Manifest.Repository.Common;
using Manifest.Service.ObjectMapping;
using Manifest.Service.Report;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;

namespace Manifest.Service.JobServer
{
    public class UserListJobRunner : PredefinedJobRunner<UserListJobParameter>
    {
        protected readonly CompanyRepository userRepository;
        protected readonly UserListJobParameter parameter;
        private readonly int pageSize = 1000;

        public UserListJobRunner(UserListJobParameter parameter, IntermediateDBRepository intermediateDBCreationRepository, ObjectMapperService objectMapperService, ILog log, CompanyRepository companyRepository)
            : base(parameter, true, intermediateDBCreationRepository, objectMapperService, log)
        {
            this.userRepository = companyRepository;
            if (parameter == null)
            {
                this.Parameter = new UserListJobParameter { ShowDeleted = false };
            }
        }

        public override async Task<ReturnModel<bool>> GenerateDataAsync(string fileName, string reportFile, string jasperExeFilePath)
        {
            //Populate the user model
            var userCount = await userRepository.GetUserCountAsync();
            if (userCount == 0)
            {
                return log.ErrorAndReturnModel<bool>("No Users available for export.");
            }

            var itemsToReturn = new List<UserInfoImportExportModel>();
            var numberOfLoops = (userCount / pageSize) + 1;
            for (var i = 0; i < numberOfLoops; ++i)
            {
                var userResult = await userRepository.GetUsersAsync(new UserSearchCriteria { Page = i, PageSize = pageSize, ShowDeleted = Parameter?.ShowDeleted });
                if (userResult == null)
                {
                    return log.ErrorAndReturnModel<bool>("Unable to retrieve user list.");
                }

                itemsToReturn.AddRange(objectMapperService.UserViewListToUserInfoImportExportModelList(userResult.Result));
            }

            await intermediateDBCreationRepository.CreateReportDbAndItemsAsync(Path.GetDirectoryName(fileName), "report.db", itemsToReturn);
            //generate jasper report
            return ReturnModel<bool>.Success(true);
        }
    }
}