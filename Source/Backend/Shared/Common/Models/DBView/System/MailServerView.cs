using Manifest.Common.Models.DB;

namespace Manifest.Common.Models.DBView
{
    public class MailServerView : MailServer
    {
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