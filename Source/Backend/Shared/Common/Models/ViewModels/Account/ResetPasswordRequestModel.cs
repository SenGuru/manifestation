

namespace Manifest.Common.Models.ViewModels.Account
{
    public class ResetPasswordRequestModel
    {
        public string LoginRedirecturl { get; set; }

        public string Token { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
