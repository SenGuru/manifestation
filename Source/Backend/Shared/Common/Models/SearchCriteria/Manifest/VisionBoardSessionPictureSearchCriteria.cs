using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class VisionBoardSessionPictureSearchCriteria : BaseSearchCriteria
    {
    //public DateTime? StartDate { get; set; }

    //public DateTime? EndDate { get; set; }

        public bool ShowDeleted { get; set; } = false;

        
public long? VisionBoardSessionId { get; set; }

        public long? VisionBoardPictureId { get; set; }

        public long? UserId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }

    }

    

        public class VisionBoardSessionPictureByVisionBoardSessionIdSearchCriteria : BaseSearchCriteria
        {
                public long VisionBoardSessionId { get; set; }
        }


        public class VisionBoardSessionByVisionBoardSessionPictureIdSearchCriteria : BaseSearchCriteria
        {
                public long VisionBoardSessionPictureId { get; set; }
        }


        public class VisionBoardSessionPictureByVisionBoardPictureIdSearchCriteria : BaseSearchCriteria
        {
                public long VisionBoardPictureId { get; set; }
        }


        public class VisionBoardPictureByVisionBoardSessionPictureIdSearchCriteria : BaseSearchCriteria
        {
                public long VisionBoardSessionPictureId { get; set; }
        }


        public class VisionBoardSessionPictureByUserIdSearchCriteria : BaseSearchCriteria
        {
                public long UserId { get; set; }
        }


        public class UserByVisionBoardSessionPictureIdSearchCriteria : BaseSearchCriteria
        {
                public long VisionBoardSessionPictureId { get; set; }
        }


}