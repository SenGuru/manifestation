using System;

namespace Manifest.Common.Models.ViewModels.Account
{
    public class ForgotPasswordModel
    {
        public string UserLogin { get; set; }

        public Guid Token { get; set; }

        public string Password { get; set; }

        public string RetypedPassword { get; set; }
    }
}
