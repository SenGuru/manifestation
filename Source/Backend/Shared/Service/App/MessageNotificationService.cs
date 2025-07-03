using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.Queue;
using Manifest.Common.Models.SearchCriteria;
using Manifest.Common.Models.ViewModels;
using Manifest.Repository.Common;
using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.Exceptions;
using Framework.Infrastructure.Interfaces.DbAccess;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using MassTransit;

namespace Manifest.Service.App
{
    public partial class MessageNotificationService : BaseAppService
    {
        private readonly SystemRepository systemRepository;
        private readonly ICurrentSessionService currentSessionService;
        private readonly ITransactionManager transactionManager;
        private readonly IBusControl busControl;

        public MessageNotificationService(IBusControl busControl, ObjectMapperService objectMapperService, ILog log, SystemRepository systemRepository, ICurrentSessionService currentSessionService, ITransactionManager transactionManager)
            : base(log, objectMapperService)
        {
            this.busControl = busControl;
            this.systemRepository = systemRepository;
            this.currentSessionService = currentSessionService;
            this.transactionManager = transactionManager;
        }

        #region RealtimeMessage

        public async Task<ReturnListModel<RealtimeMessageModel>> GetNotShownRealtimeMessagesAsync(RealtimeMessageSearchCriteria search)
        {
            try
            {
                return this.ConvertToList(await systemRepository.GetNotShownRealtimeMessagesAsync(await currentSessionService.GetCurrentUserIdAsync() ?? 0, search),objectMapperService.RealtimeMessageViewListToRealtimeMessageModelList);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnListModel<RealtimeMessageModel>($"Unexpected error when getting Realtime Messages", ex);
            }
        }

        public async Task<ReturnModel<bool>> MarkRealtimeMessagesAsReadAsync(List<long> realtimeMessageIds)
        {
            try
            {
                return ReturnModel<bool>.Success(await systemRepository.MarkRealtimeMessagesAsReadAsync(realtimeMessageIds, await currentSessionService.GetCurrentUserIdAsync() ?? 0, await currentSessionService.GetCurrentUserIdAsync()));
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when getting when marking Messages", ex);
            }
        }

        public Task<ReturnModel<RealtimeMessageModel>> GetRealtimeMessageByIdForReadAsync(long realtimeMessageId)
        {
            return GetRealtimeMessageByIdInternalAsync(forWrite: false, realtimeMessageId);
        }

        public Task<ReturnModel<RealtimeMessageModel>> GetRealtimeMessageByIdForWriteAsync(long realtimeMessageId)
        {
            return GetRealtimeMessageByIdInternalAsync(forWrite: true, realtimeMessageId);
        }

        public async Task<ReturnModel<RealtimeMessageModel>> GetRealtimeMessageByIdInternalAsync(bool forWrite, long realtimeMessageId)
        {
            try
            {
                var item = await systemRepository.GetRealtimeMessageViewByIdAsync(realtimeMessageId);
                if (item == null)
                {
                    return log.ErrorAndReturnModel<RealtimeMessageModel>($"Unable to find the Realtime Message", new { realtimeMessageId });
                }

                if (item.UserId != await currentSessionService.GetCurrentUserIdAsync())
                {
                    return log.ErrorAndReturnModel<RealtimeMessageModel>("This message is not meant for you");
                }

                return this.ConvertToItem(item, objectMapperService.RealtimeMessageViewToRealtimeMessageModel);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<RealtimeMessageModel>($"Unexpected error when getting Realtime Message", new { realtimeMessageId }, ex);
            }
        }

        public async Task<ReturnListModel<RealtimeMessageModel>> CreateRealtimeMessageAsync(List<User> users, RealtimeMessageCreateModel realtimeMessage)
        {
            var result = new List<RealtimeMessageModel>();
            foreach (var u in users)
            {
                var createRealtimeMessage = ObjectUtils.DeepCopy(realtimeMessage);
                createRealtimeMessage.RealtimeMessageId = 0;
                createRealtimeMessage.UserId = u.UserId;
                var ur = await CreateRealtimeMessageAsync(createRealtimeMessage);
                if (!ur.IsSuccess)
                    throw new RollbackException($"Unable to create Realtime message for user {u.UserName}", createRealtimeMessage);
                result.Add(ur.Model);
            }

            return ReturnListModel<RealtimeMessageModel>.Success(result);
        }

        public async Task<ReturnModel<RealtimeMessageModel>> CreateRealtimeMessageAsync(RealtimeMessageCreateModel realtimeMessage)
        {
            try
            {
                if (realtimeMessage == null)
                {
                    return log.ErrorAndReturnModel<RealtimeMessageModel>($"Unable to create Realtime Message", new ReturnErrorItem("", "Realtime Message information is not valid"));
                }

                if (realtimeMessage.UserId <= 1)
                    realtimeMessage.UserId = await currentSessionService.GetCurrentUserIdAsync() ?? 0;

                var errorList = IsRealtimeMessageModelValidForAll(realtimeMessage);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<RealtimeMessageModel>($"Unable to create Realtime Message", errorList);
                }

                var item = objectMapperService.RealtimeMessageCreateModelToRealtimeMessage(realtimeMessage);

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await systemRepository.CreateRealtimeMessageAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create Realtime Message");
                    }

                    await trx.CompleteAsync();
                }

                var view = await systemRepository.GetRealtimeMessageViewByIdAsync(item.RealtimeMessageId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<RealtimeMessageModel>($"Unable to find the Realtime Message", new { item.RealtimeMessageId });
                }

                var result = this.ConvertToItem(view, objectMapperService.RealtimeMessageViewToRealtimeMessageModel);
                var rMsg = objectMapperService.RealtimeMessageViewToRealtimeMessageQueueModel(view);
                await busControl.Publish<RealtimeMessageQueueModel>(rMsg);
                return result;
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<RealtimeMessageModel>($"Unexpected error when creating Realtime Message {realtimeMessage?.RealtimeMessageId}", ex);
            }
        }

        public async Task<ReturnModel<RealtimeMessageModel>> CreateRealtimeMessageWithoutPublishAsync(RealtimeMessageCreateModel realtimeMessage)
        {
            try
            {
                if (realtimeMessage == null)
                {
                    return log.ErrorAndReturnModel<RealtimeMessageModel>($"Unable to create Realtime Message", new ReturnErrorItem("", "Realtime Message information is not valid"));
                }

                if (realtimeMessage.UserId <= 1)
                    realtimeMessage.UserId = await currentSessionService.GetCurrentUserIdAsync() ?? 0;

                var errorList = IsRealtimeMessageModelValidForAll(realtimeMessage);
                if (errorList != null)
                {
                    return log.ErrorAndReturnModel<RealtimeMessageModel>($"Unable to create Realtime Message", errorList);
                }

                var item = objectMapperService.RealtimeMessageCreateModelToRealtimeMessage(realtimeMessage);

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    item = await systemRepository.CreateRealtimeMessageAsync(item, await currentSessionService.GetCurrentUserIdAsync());
                    if (item == null)
                    {
                        throw new Exception($"Unable to create Realtime Message");
                    }

                    await trx.CompleteAsync();
                }

                var view = await systemRepository.GetRealtimeMessageViewByIdAsync(item.RealtimeMessageId);

                if (view == null)
                {
                    return log.ErrorAndReturnModel<RealtimeMessageModel>($"Unable to find the Realtime Message", new { item.RealtimeMessageId });
                }

                var result = this.ConvertToItem(view, objectMapperService.RealtimeMessageViewToRealtimeMessageModel);
                return result;
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModelWithRollbackExceptionHandler<RealtimeMessageModel>($"Unexpected error when creating Realtime Message {realtimeMessage?.RealtimeMessageId}", ex);
            }
        }

        public async Task<ReturnModel<bool>> HardDeleteRealtimeMessageByAsync(long realtimeMessageId)
        {
            try
            {
                var realtimeMessage = await systemRepository.GetRealtimeMessageViewFromAnyByIdAsync(realtimeMessageId);
                if (realtimeMessage == null)
                {
                    return log.ErrorAndReturnModel<bool>($"Unable to find the Realtime Message", new { realtimeMessageId });
                }

                bool result;

                using (var trx = await transactionManager.BeginTransactionAsync())
                {
                    result = await systemRepository.HardDeleteRealtimeMessageByIdAsync(realtimeMessageId);
                    await trx.CompleteAsync();
                }

                return new ReturnModel<bool>(result);
            }
            catch (Exception ex)
            {
                return log.ErrorAndReturnModel<bool>($"Unexpected error when deleting Real time Message {realtimeMessageId}", ex);
            }
        }

        private List<ReturnErrorItem> IsRealtimeMessageModelValidForAll(RealtimeMessageUpdateModel realtimeMessageModel)
        {
            var result = new List<ReturnErrorItem>();

            if (!DateUtils.IsValidDate(realtimeMessageModel.MessageDateTime))
            {
                result.Add(new ReturnErrorItem("MessageDateTime", $"Message DateTime is not a valid date"));
            }

            if (realtimeMessageModel.MessageSubject != null && realtimeMessageModel.MessageSubject.Length > RealtimeMessageModel.COLUMN_SIZE_MessageSubject)
            {
                result.Add(new ReturnErrorItem("MessageSubject", $"Message Subject size should not exceed {RealtimeMessageModel.COLUMN_SIZE_MessageSubject}"));
            }

            if (realtimeMessageModel.MessageSubject.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("MessageSubject", "Message Subject is required "));
            }

            if (realtimeMessageModel.MessageContent != null && realtimeMessageModel.MessageContent.Length > RealtimeMessageModel.COLUMN_SIZE_MessageContent)
            {
                result.Add(new ReturnErrorItem("MessageContent", $"Message Content size should not exceed {RealtimeMessageModel.COLUMN_SIZE_MessageContent}"));
            }

            if (realtimeMessageModel.MessageContent.IsTrimmedStringNullOrEmpty())
            {
                result.Add(new ReturnErrorItem("MessageContent", "Message Content is required "));
            }

            if (realtimeMessageModel.MessageSentDateTime != null && !DateUtils.IsValidDate(realtimeMessageModel.MessageSentDateTime.Value))
            {
                result.Add(new ReturnErrorItem("MessageSentDateTime", $"Message Sent DateTime is not a valid date"));
            }

            if (realtimeMessageModel.MessageViewedDateTime != null && !DateUtils.IsValidDate(realtimeMessageModel.MessageViewedDateTime.Value))
            {
                result.Add(new ReturnErrorItem("MessageViewedDateTime", $"Message Viewed DateTime is not a valid date"));
            }

            if (realtimeMessageModel.PredefinedRealtimeMessageStatusTypeId == 0)
            {
                result.Add(new ReturnErrorItem("PredefinedRealtimeMessageStatusTypeId", "Realtime Message Status Type cannot be empty"));
            }
            else if (!ValidationUtils.IsEnumValid<PredefinedRealtimeMessageStatusTypes, long>(realtimeMessageModel.PredefinedRealtimeMessageStatusTypeId))
            {
                result.Add(new ReturnErrorItem("PredefinedRealtimeMessageStatusTypeId", "Realtime Message Status Type is not valid"));
            }

            if (realtimeMessageModel.ResourceName != null && realtimeMessageModel.ResourceName.Length > RealtimeMessageModel.COLUMN_SIZE_ResourceName)
            {
                result.Add(new ReturnErrorItem("ResourceName", $"Resource Name size should not exceed {RealtimeMessageModel.COLUMN_SIZE_ResourceName}"));
            }

            if (realtimeMessageModel.ErrorMessage != null && realtimeMessageModel.ErrorMessage.Length > RealtimeMessageModel.COLUMN_SIZE_ErrorMessage)
            {
                result.Add(new ReturnErrorItem("ErrorMessage", $"Error Message size should not exceed {RealtimeMessageModel.COLUMN_SIZE_ErrorMessage}"));
            }

            if (realtimeMessageModel.ErrorOccurredOn != null && !DateUtils.IsValidDate(realtimeMessageModel.ErrorOccurredOn.Value))
            {
                result.Add(new ReturnErrorItem("ErrorOccurredOn", $"Error Occurred On is not a valid date"));
            }

            if (realtimeMessageModel.UserId <= 0)
            {
                result.Add(new ReturnErrorItem("UserId", "User cannot be empty"));
            }

            return result.Count > 0 ? result : null;
        }
        #endregion
    }
}
