#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class ChatMessageXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long ChatMessageId { get; set; }
    }
    */

    public class MultipleChatMessageCreateModel
    {
        public List<ChatMessageCreateModel> NewChatMessages { get; set; }
    }
}
#pragma warning restore CS0108, SA1502, S4015