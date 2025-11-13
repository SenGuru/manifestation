#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    /*
    public class ChatXYZZYXAssociationModel
    {
        public long XYZZYXId { get; set; }
        public long ChatId { get; set; }
    }
    */

    public class MultipleChatCreateModel
    {
        public List<ChatCreateModel> NewChats { get; set; }
    }
}
#pragma warning restore CS0108, SA1502, S4015