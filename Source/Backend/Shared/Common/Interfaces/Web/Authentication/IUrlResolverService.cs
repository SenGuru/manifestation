using System;
using System.Threading.Tasks;
using Framework.Infrastructure.Models.Result;

namespace Op.Common.Interfaces.Web.Authentication
{
    public interface IUrlResolverService
    {
        Task<ReturnModel<string>> GetDesktopUrlAsync(long companyId, Uri callbackUri);

        Task<ReturnModel<string>> GetMobileUrlAsync(long companyId, Uri callbackUri);
    }
}