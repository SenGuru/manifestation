using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace %AppName%.Common.Models.SearchCriteria
{
    public class EEntity123SearchCriteria : BaseSearchCriteria
    {
    //public DateTime? StartDate { get; set; }

    //public DateTime? EndDate { get; set; }

        public bool ShowDeleted { get; set; } = false;

        //%SearchVariable%
    }

    //%SearchCriteriaModelFkeyBasedFunctions%
}