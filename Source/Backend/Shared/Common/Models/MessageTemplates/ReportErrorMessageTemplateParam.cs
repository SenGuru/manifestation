namespace Manifest.Common.Models.MessageTemplateParams
{
    public class ReportErrorMessageTemplateParam : BaseMessageTemplateParam
    {
        public string ReportGuid { get; set; }

        public string ReportName { get; set; }

        public string ReportStartedOn { get; set; }

        public string ReportCompletedOn { get; set; }

        public string ErrorMessage { get; set; }
    }
}
