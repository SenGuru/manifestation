using System;

namespace Manifest.Common.Models.ViewModels.Account
{
    public class LoggedInUserSessionModel
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public double UserLatitude { get; set; }

        public double UserLongitude { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public long UserId { get; set; }

        public string Token { get; set; }

        public DateTime TokenValidUntil { get; set; }
    }
}
