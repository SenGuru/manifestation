namespace Manifest.Common.Models.ViewModels.UserProfile
{
    public class UserProfileModel
    {
        public long UserId { get; set; }

        public bool IsPrivate { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string WorkPhone { get; set; }

        public string CellPhone { get; set; }

        public string BillingAddress1 { get; set; }

        public string BillingAddress2 { get; set; }

        public string LocationCity { get; set; }

        public string LocationState { get; set; }

        public string LocationZip { get; set; }

        public string LocationCountry { get; set; }

        public long? CountryGuid { get; set; }

        public long? StateGuid { get; set; }
    }
}
