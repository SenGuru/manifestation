using System.Threading.Tasks;
using Framework.Infrastructure.Models.Result;
using Loyalty.Common.Constants;
using Loyalty.Common.Interfaces.Service.App;
using Loyalty.Common.Interfaces.Service.Config;
using Loyalty.Common.Models.ViewModels;
using Loyalty.Common.Models.ViewModels.Exchange;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Loyalty.Api.All.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public partial class ExternalController : Controller
    {
        private readonly bool isTest;
        private readonly ICustomerService customerService;
        private readonly IPointProcessService pointProcessService;

        public ExternalController(IPointProcessService pointProcessService, ICustomerService customerService,ILoyaltyWebConfiguration loyaltyWebConfiguration, IHttpContextAccessor contextAccessor)
        {
            this.pointProcessService = pointProcessService;
            this.customerService = customerService;
            isTest = contextAccessor.HttpContext.Request.Headers.ContainsKey(HeaderContants.QATestFlag);

            if (loyaltyWebConfiguration.IsProduction)
            {
                isTest = false;
            }
        }

        [HttpPost]
        public Task<ReturnModel<CustomerModel>> GetCustomerProfile([FromBody] ExternalEncryptedPayload ePayload)
        {
            return customerService.GetCustomerProfileByIdForExternalAsync(ePayload.Payload, isTest);
        }

        [HttpPost]
        public Task<ReturnModel<CustomerModel>> UpdateCustomerProfile([FromBody] ExternalEncryptedPayload ePayload)
        {
            return customerService.UpdateCustomerProfileForExternalAsync(ePayload.Payload, isTest);
        }

        [HttpPost]
        public Task<ReturnModel<bool>> EnrollInLoyalty([FromBody] ExternalEncryptedPayload ePayload)
        {
            return customerService.EnrollCustomerInLoyaltyForExternalAsync(ePayload.Payload, isTest);
        }

        [HttpPost]
        public Task<ReturnModel<bool>> AddPointsExternal([FromBody] ExternalEncryptedPayload ePayload)
        {
            return pointProcessService.AddAndCaculatePointsForExternalAsync(ePayload.Payload, isTest);
        }

        [HttpPost]
        public Task<ReturnModel<bool>> RedeemPointsExternal([FromBody] ExternalEncryptedPayload ePayload)
        {
            return pointProcessService.RedeemPointTransactionForExternalAsync(ePayload.Payload, isTest);
        }

        [HttpPost]
        public Task<ReturnModel<bool>> LossPreventionCheckExternal([FromBody] ExternalEncryptedPayload ePayload)
        {
            return pointProcessService.LossPreventionCheckExternalAsync(ePayload.Payload, isTest);
        }
    }
}
