namespace Manifest.Common.Models.MessageTemplateParams
{
    public class ChangedPasswordMessageTemplateParam : BaseMessageTemplateParam
    {
        public string UserName { get; set; }

        public string ChangedPasswordOn { get; set; }

        public string ChangedFromIpAddress { get; set; }
    }
}
