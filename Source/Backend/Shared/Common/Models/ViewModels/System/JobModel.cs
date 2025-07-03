#pragma warning disable S2094 // Classes should not be empty
using System.Collections.Generic;
using Manifest.Common.Models.DBView;

namespace Manifest.Common.Models.ViewModels
{
    public class JobModel : JobView
    {
        public List<JobAttachmentModel> Attachments { get; set; }

        public List<JobNotificationEmailModel> NotificationEmails { get; set; }
    }
}
#pragma warning restore S2094 // Classes should not be empty