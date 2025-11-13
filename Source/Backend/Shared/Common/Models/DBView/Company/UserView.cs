using Manifest.Common.Models.DB;
using LinqToDB.Mapping;

namespace Manifest.Common.Models.DBView
{
    public class UserView : User
    {
        [NotColumn]
        public string PredefinedUserTypeName { get; set; }

        [NotColumn]
        public string ModifiedByUserFirstName { get; set; }

        [NotColumn]
        public string ModifiedByUserLastName { get; set; }

        [NotColumn]
        public string ModifiedByUserEmail { get; set; }
    }
}