namespace Manifest.Common.Models.MessageTemplateParams
{
    public class ReportSuccessMessageTemplateParam : BaseMessageTemplateParam
    {
        public string ReportGuid { get; set; }

        public string ReportName { get; set; }

        public string ReportStartedOn { get; set; }

        public string ReportCompletedOn { get; set; }
    }
}