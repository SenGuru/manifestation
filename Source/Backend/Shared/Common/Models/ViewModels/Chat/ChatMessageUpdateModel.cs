#pragma warning disable CS0108,SA1502,S4015, SA1025
using System;
using Manifest.Common.Models.DB;

namespace Manifest.Common.Models.ViewModels
{
    public class ChatMessageUpdateModel : ChatMessage
    {
        //public List<ChatMessageXYZZYXCreateModel> NewChatMessageXYZZYXModels { get; set; }
        //public List<ChatMessageXYZZYXUpdateModel> ExistingChatMessageXYZZYXModels { get; set; }
        //protected long CompanyId { get { return base.CompanyId; } set { base.CompanyId = value; } }

        protected bool IsDeleted { get { return base.IsDeleted; } set { base.IsDeleted = value; } }

        protected DateTime CreatedDate { get { return base.CreatedDate; } set { base.CreatedDate = value; } }

        protected long? CreatedByUserId { get { return base.CreatedByUserId; } set { base.CreatedByUserId = value; } }

        protected DateTime ModifiedDate { get { return base.ModifiedDate; } set { base.ModifiedDate = value; } }

        protected long? ModifiedByUserId { get { return base.ModifiedByUserId; } set { base.ModifiedByUserId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015, SA1025