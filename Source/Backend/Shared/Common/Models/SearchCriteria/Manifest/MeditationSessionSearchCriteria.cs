using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class MeditationSessionSearchCriteria : BaseSearchCriteria
    {
    //public DateTime? StartDate { get; set; }

    //public DateTime? EndDate { get; set; }

        public bool ShowDeleted { get; set; } = false;

        
public long? MeditationFileId { get; set; }

        public long? UserId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }

    }

    

        public class MeditationSessionByMeditationFileIdSearchCriteria : BaseSearchCriteria
        {
                public long MeditationFileId { get; set; }
        }


        public class MeditationFileByMeditationSessionIdSearchCriteria : BaseSearchCriteria
        {
                public long MeditationSessionId { get; set; }
        }


        public class MeditationSessionByUserIdSearchCriteria : BaseSearchCriteria
        {
                public long UserId { get; set; }
        }


        public class UserByMeditationSessionIdSearchCriteria : BaseSearchCriteria
        {
                public long MeditationSessionId { get; set; }
        }


}