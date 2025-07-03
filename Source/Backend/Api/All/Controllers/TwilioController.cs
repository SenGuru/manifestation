using Manifest.Common.Enums;
using Manifest.Common.Models.DBView;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.ViewModels;
using Manifest.Web.Filters;
using Framework.Infrastructure.Models.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
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
using Manifest.Service.Storage;
using Framework.Infrastructure.Utils;
using System.IO;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.ViewModels;
using Twilio.AspNet.Core;
using System.ServiceModel.Channels;
using Microsoft.AspNetCore.Mvc;
using Twilio.Types;
using Twilio.TwiML;
using MathNet.Numerics.Providers.SparseSolver;


namespace Manifest.Api.All.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public partial class TwilioController : BaseController
    {
        private readonly TwilioService twilioService;
        public TwilioController(TwilioService twilioService, CompanyService companyService, MovieService movieService, DummyDataGenerationService dummyDataGenerationService, ILog log)
            : base(log)
        {
            this.twilioService = twilioService;
        }

        #region Twilio
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Twilio" })]
        public IActionResult InitiateCall([FromQuery] TwilioInitiateCallModel model)
        {
            var call = twilioService.InitiateCall(model.ToPhoneNumber, model.TwilioAccountSid, model.TwilioAuthToken, model.TwilioPhoneNumber);
            return Ok($"Call initiated with SID: {call.Sid}");
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Twilio" })]
        public IActionResult HandleQuestion()
        {
            var response = twilioService.HandleQuestion();
            return Content(response.ToString(), "application/xml");
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Twilio" })]
        public IActionResult HandleResponse([FromQuery] string digits)
        {
            var response = twilioService.HandleResponse(digits);
            return Content(response.ToString(), "application/xml");
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Twilio" })]
        public async Task<IActionResult> HandleGather([FromForm] string digits, [FromQuery] long msdrid, [FromQuery] long msdrtid, [FromQuery] long ntid)
        {
            var result = await twilioService.ProcessGatherResultAsync(digits, msdrid, msdrtid, ntid);
            return Content(result.ToString(), "application/xml");
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Twilio" })]
        public async Task<IActionResult> GenerateCallTwiML([FromQuery] long msdrid, [FromQuery] long msdrtid, [FromQuery] long ntid, [FromQuery] string question)
        {
            var result = twilioService.GenerateCallTwiML(msdrid, msdrtid, ntid, question);
            return Content(result.ToString(), "application/xml");
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Twilio" })]
        public async Task<IActionResult> HandleStatus([FromForm] string callStatus, [FromForm] string smsStatus, [FromQuery] long msdrid, [FromQuery] long msdrtid, [FromQuery] long ntid)
        {
            await twilioService.ProcessTelecomStatusAsync(callStatus, smsStatus, msdrid, msdrtid, ntid);
            return Ok();
        }
        #endregion
    }
}