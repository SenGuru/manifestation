#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class ChatUserXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long ChatUserId { get; set; }
    }
    */

    public class MultipleChatUserCreateModel
    {
        public List<ChatUserCreateModel> NewChatUsers { get; set; }
    }
}
#pragma warning restore CS0108, SA1502, S4015