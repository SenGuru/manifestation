namespace Manifest.Common.Models.ViewModels.Account
{
    public class AnonymousLoginModel
    {
        public string AuthValue { get; set; }

        public long AuthCode { get; set; }

        public bool IsEmail { get; set; }
    }
}
