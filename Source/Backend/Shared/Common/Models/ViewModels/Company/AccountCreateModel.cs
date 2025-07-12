#pragma warning disable CS0108,SA1502,S4015
using System;

namespace Manifest.Common.Models.ViewModels
{
    public class AccountCreateModel
    {
        public string CaptchaCode { get; set; }

        public string InitialPassword { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
#pragma warning restore CS0108, SA1502, S4015