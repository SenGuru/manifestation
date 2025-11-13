using System;
using System.Collections.Generic;
using Manifest.Common.Enums;
using Manifest.Common.Models.MessageTemplateParams;
using Framework.Infrastructure.Models.Result;

namespace Manifest.Service.App
{
    public class MessageTemplateHelperService
    {
        private readonly ReturnListModel<MessageTemplateParamModel> msgTypeParams = GetMsgTypeParamsInternal();

        public ReturnListModel<MessageTemplateParamModel> GetMsgTypeParams()
        {
            return msgTypeParams;
        }

        private static ReturnListModel<MessageTemplateParamModel> GetMsgTypeParamsInternal()
        {
            var lst = new List<KeyValuePair<PredefinedMessageTemplateTypes, List<string>>>
            {
                //Basic
                GetMsgTypeParamInternal(PredefinedMessageTemplateTypes.UserCreation, typeof(UserCreationMessageTemplateParam)),
                GetMsgTypeParamInternal(PredefinedMessageTemplateTypes.ForgotPassword, typeof(ForgotPasswordMessageTemplateParam)),
                GetMsgTypeParamInternal(PredefinedMessageTemplateTypes.ChangedPassword, typeof(ChangedPasswordMessageTemplateParam)),

                //Reports
                GetMsgTypeParamInternal(PredefinedMessageTemplateTypes.ReportSuccess, typeof(ReportSuccessMessageTemplateParam)),
                GetMsgTypeParamInternal(PredefinedMessageTemplateTypes.ReportError, typeof(ReportErrorMessageTemplateParam)),
            };

            var models = new List<MessageTemplateParamModel>();
            foreach (var i in lst)
            {
                models.Add(new MessageTemplateParamModel { MessageTemplateTypeId = (int)i.Key, ParameterNames = i.Value });
            }

            return ReturnListModel<MessageTemplateParamModel>.Success(models);
        }

        private static KeyValuePair<PredefinedMessageTemplateTypes, List<string>> GetMsgTypeParamInternal(PredefinedMessageTemplateTypes messageTemplateTypes, Type paramClass)
        {
            var result = new KeyValuePair<PredefinedMessageTemplateTypes, List<string>>(messageTemplateTypes, new List<string>());
            foreach (var item in paramClass.GetProperties())
            {
                result.Value.Add(item.Name);
            }

            return result;
        }
    }
}
