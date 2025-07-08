using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class FileAttachmentFolderSearchCriteria : BaseSearchCriteria
    {
        public bool ShowDeleted { get; set; } = false;

        public long? ParentFileAttachmentFolderId { get; set; }

        public long? UserId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}