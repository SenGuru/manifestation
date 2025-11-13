namespace Manifest.Common.Models.MessageTemplateParams
{
    public class MemberOtpMessageTemplateParam : BaseMessageTemplateParam
    {
        public string MemberFirstName { get; set; }

        public string MemberLastName { get; set; }

        public string OtpCode { get; set; }
    }
}