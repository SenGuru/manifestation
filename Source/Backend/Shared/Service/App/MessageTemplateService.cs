using System;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.MessageTemplateParams;
using Manifest.Common.Models.ViewModels;
using Manifest.Repository.Common;
using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using SmartFormat;
using SmartFormat.Core.Settings;
using SmartFormat.Extensions;

namespace Manifest.Service.App
{
    public class MessageTemplateService : BaseAppService
    {
        private readonly SystemRepository systemRepository;
        private readonly ICurrentSessionService sessionService;

        static MessageTemplateService()
        {
            Smart.Default.AddExtensions(new ListFormatter(), new ReflectionSource());
            Smart.Default.Settings.Formatter.ErrorAction = FormatErrorAction.Ignore;
        }

        public MessageTemplateService(ILog log, ObjectMapperService objectMapperService, SystemRepository systemRepository, ICurrentSessionService sessionService)
            : base(log, objectMapperService)
        {
            this.sessionService = sessionService;
            this.systemRepository = systemRepository;
        }

        public async Task<ReturnModel<bool>> AddMessageTemplateAsync(MessageTemplate item)
        {
            try
            {
                await systemRepository.CreateMessageTemplateAsync(item, await sessionService.GetCurrentUserNameAsStringAsync());
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>("Unexpected error occurred when deleting Message Template", ex);
            }
        }

        public async Task<ReturnModel<bool>> DeleteMessageTemplateAsync(long messageTemplateId)
        {
            try
            {
                await systemRepository.DeleteMessageTemplateAsync(messageTemplateId);
                return ReturnModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>("Unexpected error occurred when deleting CompanyMessageTemplate", ex);
            }
        }

        public async Task<ReturnModel<MessageTemplate>> GetMessageTemplateByMessageTemplateIdAsync(long messageTemplateId)
        {
            try
            {
                var result = await systemRepository.GetMessageTemplateByIdAsync(messageTemplateId);
                if (result == null)
                    return log.ErrorAndReturnModel<MessageTemplate>("Unable to find the Message Template");

                return ReturnModel<MessageTemplate>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<MessageTemplate>("Unexpected error occurred when deleting CompanyMessageTemplate", ex);
            }
        }

        public async Task<ReturnModel<MessageTemplate>> GetMessageTemplateByTypeAsync(long templateType)
        {
            try
            {
                var result = await systemRepository.GetMessageTemplateByTypeAsync(templateType);
                if (result == null)
                    return log.ErrorAndReturnModel<MessageTemplate>("Unable to find the Message Template");

                return ReturnModel<MessageTemplate>.Success(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<MessageTemplate>("Unexpected error occurred when checking CompanyMessageTemplate", ex);
            }
        }

        public Task<MessageTemplateModel> ChangePasswordAsync(ChangedPasswordMessageTemplateParam param)
        {
            return FindEmailTemplateForCurrentTenantAsync(PredefinedMessageTemplateTypes.ChangedPassword, param);
        }

        public Task<MessageTemplateModel> ForgotPasswordAsync(ForgotPasswordMessageTemplateParam param)
        {
            return FindEmailTemplateForCurrentTenantAsync(PredefinedMessageTemplateTypes.ForgotPassword, param);
        }

        public async Task<MessageTemplateModel> FindEmailTemplateForCurrentTenantAsync(PredefinedMessageTemplateTypes messageTemplateType, object param)
        {
            var result = await FindEmailForTemplateAsync(messageTemplateType, null, param);
            if (result.IsSuccess)
            {
                return result.Model;
            }

            return null;
        }

        public async Task<ReturnModel<MessageTemplateModel>> FindEmailForTemplateAsync(PredefinedMessageTemplateTypes emailType, long? companyId, object parameter)
        {
            try
            {
                    var item = await systemRepository.GetMessageTemplateByTypeAsync((int)emailType);
                    if (item == null)
                        return log.ErrorAndReturnModel<MessageTemplateModel>($"No Template Found", new { emailType });

                return ReturnModel<MessageTemplateModel>.Success(new MessageTemplateModel
                {
                    EmailSubject = Smart.Format(item.EmailSubject ?? "", parameter),
                    EmailBody = Smart.Format(item.EmailBody ?? "", parameter),
                    AdditionalRecipients = item.AdditionalRecipients,
                    SmsBody = Smart.Format(item.SmsBody ?? "", parameter),
                    AppNotificationSubject = Smart.Format(item.AppNotificationSubject ?? "", parameter),
                    AppNotificationBody = Smart.Format(item.AppNotificationBody ?? "", parameter),
                    WebNotificationSubject = Smart.Format(item.WebNotificationSubject ?? "", parameter),
                    WebNotificationBody = Smart.Format(item.WebNotificationBody, parameter),
                    SendEmailForInternalUser = item.SendEmailForInternalUser ?? false,
                    SendEmailForExternalUser = item.SendEmailForExternalUser
                });
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<MessageTemplateModel>("Unexpected error when getting the Message template", ex);
            }
        }
    }
}
