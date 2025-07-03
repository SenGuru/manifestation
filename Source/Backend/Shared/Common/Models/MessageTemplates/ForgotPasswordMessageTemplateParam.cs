using System;

namespace Manifest.Common.Models.MessageTemplateParams
{
    public class ForgotPasswordMessageTemplateParam : BaseMessageTemplateParam
    {
        public string UserName { get; set; }

        public Guid PasswordResetGuid { get; set; }

        public DateTime RequestDate { get; set; }

        public string UrlLink { get; set; }
    }
}
