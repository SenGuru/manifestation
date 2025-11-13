using System.Collections.Generic;

namespace Manifest.Common.Models.MessageTemplateParams
{
    public class MessageTemplateParamModel
    {
        public long MessageTemplateTypeId { get; set; }

        public List<string> ParameterNames { get; set; }
    }
}
