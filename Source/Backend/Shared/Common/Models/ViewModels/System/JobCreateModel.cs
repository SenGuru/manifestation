#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class JobXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long JobId { get; set; }
    }
    */

    public class JobCreateModel : JobUpdateModel
    {
        public List<JobNotificationEmailCreateModel> Emails { get; set; }

        protected long JobId { get { return base.JobId; } set { base.JobId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015