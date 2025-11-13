using System;
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels.Account
{
    [Serializable]
    public class LoggedInUserModel
    {
        public Guid? UserProfileGuid { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public long UserTypeId { get; set; }

        public string UserTypeName { get; set; }

        public long UserId { get; set; }

       public long PredefinedCurrencyTypeId { get; set; }

        public string Token { get; set; }

        public List<long> Permissions { get; set; }
    }
}
