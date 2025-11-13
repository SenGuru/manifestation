namespace Manifest.Common.Models.ViewModels.Account
{
    public class LoginOptionModel
    {
        public bool UseBuiltInAuthentication { get; set; }

        public string SignInUrl { get; set; }

        public string CompanyDesktopUrl { get; set; }

        public string CompanyMobileUrl { get; set; }

        public string CompanySlogan { get; set; }

        public string CompanyEntityName { get; set; }
    }
}