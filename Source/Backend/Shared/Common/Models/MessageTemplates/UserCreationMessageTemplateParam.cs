namespace Manifest.Common.Models.MessageTemplateParams
{
    public class UserCreationMessageTemplateParam : BaseMessageTemplateParam
    {
        public string UserName { get; set; }

        public string IpAddress { get; set; }

        public string UserRole { get; set; }

        public string AddBy { get; set; }

        public string AddOn { get; set; }
    }
}
