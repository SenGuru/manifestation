using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace Manifest.Common.Models.SearchCriteria
{
    public class NoteAttachmentSearchCriteria : BaseSearchCriteria
    {
        public List<long> NoteAttachmentIds { get; set; }

        public bool GetAllMovieShootDayExpenseNoteAttachments { get; set; }

        public bool ShowDeleted { get; set; } = false;

        public long? MovieId { get; set; }

        public long? UserId { get; set; }

        public long? MovieArtistId { get; set; }

        public long? ArtistId { get; set; }

        public long? EquipmentId { get; set; }

        public long? TechnicianId { get; set; }

        public long? VendorId { get; set; }

        public long? PropertyId { get; set; }

        public long? ContractId { get; set; }

        public long? MovieEquipmentId { get; set; }

        public long? MovieLocationId { get; set; }

        public long? MovieSceneId { get; set; }

        public long? MovieSceneArtistId { get; set; }

        public long? MovieSceneEquipmentId { get; set; }

        public long? MovieSceneTechnicianId { get; set; }

        public long? MovieShootDayId { get; set; }

        public long? MovieShootDayArtistId { get; set; }

        public long? MovieShootDayBudgetId { get; set; }

        public long? MovieShootDayEquipmentId { get; set; }

        public long? MovieShootDayExpenseId { get; set; }

        public long? MovieShootDayRequirementId { get; set; }

        public long? MovieShootDaySceneId { get; set; }

        public long? MovieShootDayTechnicianId { get; set; }

        public long? MovieTechnicianId { get; set; }

        public long? MovieUnitId { get; set; }

        public long? MovieUserId { get; set; }

        public long? MovieBudgetCategoryId { get; set; }

        public long? MovieVendorId { get; set; }

        public long? MoviePropertyId { get; set; }

        public long? CreatedByUserId { get; set; }

        public long? ModifiedByUserId { get; set; }
    }
}