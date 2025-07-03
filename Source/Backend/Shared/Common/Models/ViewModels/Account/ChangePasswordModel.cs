namespace Manifest.Common.Models.ViewModels.Account
{
    public class ChangePasswordModel
    {
        public string CurrentPassword { get; set; }

        public string Password { get; set; }

        public string RetypedPassword { get; set; }
    }
}
