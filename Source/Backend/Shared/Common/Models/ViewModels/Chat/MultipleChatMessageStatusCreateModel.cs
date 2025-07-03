#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class ChatMessageStatusXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long ChatMessageStatusId { get; set; }
    }
    */

    public class MultipleChatMessageStatusCreateModel
    {
        public List<ChatMessageStatusCreateModel> NewChatMessageStatuss { get; set; }
    }
}
#pragma warning restore CS0108, SA1502, S4015