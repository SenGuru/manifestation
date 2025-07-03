using System.Collections.Generic;

namespace Manifest.Common.Models.LetterTemplateParams
{
    public class LetterTemplateParamModel
    {
        public long LetterTemplateTypeId { get; set; }

        public List<string> ParameterNames { get; set; }
    }
}
