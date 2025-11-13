namespace Manifest.Common.Models.ViewModels.Account
{
    public class ChangeForgotPasswordModel : ChangePasswordModel
    {
        public string UserLogin { get; set; }

        public string SessionId { get; set; }
    }
}
