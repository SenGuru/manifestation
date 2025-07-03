#pragma warning disable CS0108,SA1502,S4015

namespace Manifest.Common.Models.ViewModels
{
    public class ChatMessageCreateModel : ChatMessageUpdateModel
    {
        protected long ChatMessageId { get { return base.ChatMessageId; } set { base.ChatMessageId = value; } }
        //protected List<ChatMessageXYZZYXModel> ExistingChatMessageXYZZYXModels {get {return base.ExistingChatMessageXYZZYXModels;} set { base.ExistingChatMessageXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015