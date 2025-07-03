#pragma warning disable S2094 // Classes should not be empty
using System.Collections.Generic;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.ViewModels;

namespace Manifest.Common.Models.MessageTemplateParams
{
    public class BaseMessageTemplateParam
    {
        public RealtimeMessageCreateModel RealtimeMessageCreateModel { get; set; }

        public List<User> Users { get; set; }
    }
}
#pragma warning restore S2094 // Classes should not be empty