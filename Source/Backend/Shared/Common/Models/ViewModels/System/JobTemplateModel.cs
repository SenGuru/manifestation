#pragma warning disable S2094 // Classes should not be empty
using System.Collections.Generic;
using Manifest.Common.Models.DBView;

namespace Manifest.Common.Models.ViewModels
{
    public class JobTemplateModel : JobTemplateView
    {
        public List<JobTemplateParameterModel> Parameters { get; set; }
    }
}
#pragma warning restore S2094 // Classes should not be empty