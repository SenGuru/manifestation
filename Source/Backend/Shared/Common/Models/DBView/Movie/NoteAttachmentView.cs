using Manifest.Common.Models.DB;
using LinqToDB.Mapping;

namespace Manifest.Common.Models.DBView
{
    public class NoteAttachmentView : NoteAttachment
    {
        [NotColumn]
        public string UserName { get; set; }

        [NotColumn]
        public string MovieCode { get; set; }

        [NotColumn]
        public string ArtistCode { get; set; }

        [NotColumn]
        public string TechnicianCode { get; set; }

        [NotColumn]
        public string EquipmentCode { get; set; }

        [NotColumn]
        public string VendorCode { get; set; }

        [NotColumn]
        public string PropertyCode { get; set; }

        [NotColumn]
        public string MovieArtistCode { get; set; }

        [NotColumn]
        public string MovieTechnicianCode { get; set; }

        [NotColumn]
        public string MovieEquipmentCode { get; set; }

        [NotColumn]
        public string MovieLocationCode { get; set; }

        [NotColumn]
        public string MovieSceneCodeInternal { get; set; }

        [NotColumn]
        public string MovieVendorCode { get; set; }

        [NotColumn]
        public string MoviePropertyCode { get; set; }

        [NotColumn]
        public string MovieSceneArtistName { get; set; }

        [NotColumn]
        public string MovieSceneTechnicianName { get; set; }

        [NotColumn]
        public string MovieSceneEquipmentName { get; set; }

        [NotColumn]
        public string MovieSceneVendorName { get; set; }

        [NotColumn]
        public string MovieScenePropertyName { get; set; }

        [NotColumn]
        public string MovieSceneCode { get; set; }

        [NotColumn]
        public string MovieSceneArtistCode { get; set; }

        [NotColumn]
        public string MovieSceneTechnicianCode { get; set; }

        [NotColumn]
        public string MovieSceneEquipmentCode { get; set; }

        [NotColumn]
        public string MovieSceneVendorCode { get; set; }

        [NotColumn]
        public string MovieScenePropertyCode { get; set; }

        [NotColumn]
        public string MovieShootDayCode { get; set; }

        [NotColumn]
        public string MovieShootDayArtistCode { get; set; }

        [NotColumn]
        public string MovieShootDayTechnicianCode { get; set; }

        [NotColumn]
        public string MovieShootDayEquipmentCode { get; set; }

        [NotColumn]
        public string MovieShootDayVendorCode { get; set; }

        [NotColumn]
        public string MovieShootDayPropertyCode { get; set; }

        [NotColumn]
        public string MovieShootDayExpenseCode { get; set; }

        public string CreatedByUserFirstName { get; set; }

        public string CreatedByUserLastName { get; set; }

        public string CreatedByUserEmail { get; set; }

        public string ModifiedByUserFirstName { get; set; }

        public string ModifiedByUserLastName { get; set; }

        public string ModifiedByUserEmail { get; set; }

        public string CreatedBy
        {
            get
            {
                if (CreatedByUserId == null)
                {
                    return "";
                }
                else
                {
                    return $"{CreatedByUserFirstName} {CreatedByUserLastName}";
                }
            }
        }

        public string ModifiedBy
        {
            get
            {
                if (ModifiedByUserId == null)
                {
                    return "";
                }
                else
                {
                    return $"{ModifiedByUserFirstName} {ModifiedByUserLastName}";
                }
            }
        }
    }
}