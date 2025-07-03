using Manifest.Common.Models.DB;
using LinqToDB.Mapping;

namespace Manifest.Common.Models.DBView
{
    public class JobView : Job
    {
        [NotColumn]
        public string PredefinedJobTypeName { get; set; }

        [NotColumn]
        public string PredefinedJobOrReportTypeName { get; set; }

        [NotColumn]
        public string PredefinedJobStatusTypeName { get; set; }

        [NotColumn]
        public string SubmittedByUserName { get; set; }

        public string CreatedByUserFirstName { get; set; }

        public string CreatedByUserLastName { get; set; }

        public string CreatedByUserEmail { get; set; }

        public string ModifiedByUserFirstName { get; set; }

        public string ModifiedByUserLastName { get; set; }

        public string ModifiedByUserEmail { get; set; }

        public string CreatedBy
        {
            get
            {
                if (CreatedByUserId == null)
                {
                    return "";
                }
                else
                {
                    return $"{CreatedByUserFirstName} {CreatedByUserLastName}";
                }
            }
        }

        public string ModifiedBy
        {
            get
            {
                if (ModifiedByUserId == null)
                {
                    return "";
                }
                else
                {
                    return $"{ModifiedByUserFirstName} {ModifiedByUserLastName}";
                }
            }
        }
    }
}