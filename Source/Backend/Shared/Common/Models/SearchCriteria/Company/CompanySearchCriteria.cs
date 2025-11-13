using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class CompanySearchCriteria : BaseSearchCriteria
    {
        public bool? IsActive { get; set; }

        public bool ShowDeleted { get; set; } = false;

        public List<long> PredefinedCompanyTypeIds { get; set; }

        public long? MailServerId { get; set; }

        public long? DigitalSignatureServerId { get; set; }

        public long? WhatsAppNotificationServerId { get; set; }

        public long? SMSNotificationServerId { get; set; }

        public long? SingleSignOnServerId { get; set; }

        public long? VoiceNotificationServerId { get; set; }

        public long? PushNotificationServerId { get; set; }

        public long? StateId { get; set; }

        public long? CountryId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}