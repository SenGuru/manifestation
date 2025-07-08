using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class CategorySearchCriteria : BaseSearchCriteria
    {
    //public DateTime? StartDate { get; set; }

    //public DateTime? EndDate { get; set; }

        public bool ShowDeleted { get; set; } = false;

        
public long? DefaultCategoryId { get; set; }

        public List<long> PredefinedCategoryTypeIds { get; set; }

        public long? UserId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }

    }

    

        public class CategoryByDefaultCategoryIdSearchCriteria : BaseSearchCriteria
        {
                public long DefaultCategoryId { get; set; }
        }


        public class DefaultCategoryByCategoryIdSearchCriteria : BaseSearchCriteria
        {
                public long CategoryId { get; set; }
        }


        public class CategoryByUserIdSearchCriteria : BaseSearchCriteria
        {
                public long UserId { get; set; }
        }


        public class UserByCategoryIdSearchCriteria : BaseSearchCriteria
        {
                public long CategoryId { get; set; }
        }


}