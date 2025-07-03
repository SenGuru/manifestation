using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class ExportResourceSearchCriteria : BaseSearchCriteria
    {
        public long CompanyId { get; set; }

        public long? MovieId { get; set; }

        public List<long> SelectedIds { get; set; }
    }
}