using System;

namespace Manifest.Common.Models.ViewModels.Account
{
    [Serializable]
    public class SignedUpUserModel
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Token { get; set; }
    }
}
