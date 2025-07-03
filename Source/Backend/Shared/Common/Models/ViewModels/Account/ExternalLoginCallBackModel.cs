using System;

namespace Manifest.Common.Models.ViewModels.Account
{
    public class ExternalLoginCallBackModel
    {
        public string Provider { get; set; }

        public string CallBackUri { get; set; }

        public string SingleSignOnIdentifier { get; set; }

        public string Code { get; set; }

        public string Id_token { get; set; }

        public string Access_token { get; set; }

        public string Error { get; set; }

        public Guid State { get; set; }
    }
}