namespace Manifest.Common.Models.ViewModels
{
    public class TwilioInitiateCallModel
    {
        public string ToPhoneNumber { get; set; }

        public string TwilioAccountSid { get; set; }

        public string TwilioAuthToken { get; set; }

        public string TwilioPhoneNumber { get; set; }
    }
}
