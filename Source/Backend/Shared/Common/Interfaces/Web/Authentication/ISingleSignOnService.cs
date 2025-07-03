using System;
using System.Threading.Tasks;
using Manifest.Common.Models.ViewModels.Account;
using Framework.Infrastructure.Models.Result;

namespace Manifest.Common.Interfaces.Web.Authentication
{
    public interface ISingleSignOnService
    {
        Task<ReturnModel<LoggedInUserModel>> GetProviderEmailAndTokenAsync(ExternalLoginCallBackModel callBackModel);

        Task<ReturnModel<string>> GetProviderLoginUrlAsync(string singleSignOnIdentifier, string state, Uri callbackUri);
    }
}