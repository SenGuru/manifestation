#pragma warning disable CS0108,SA1502,S4015

namespace Manifest.Common.Models.ViewModels
{
    public class ChatMessageStatusCreateModel : ChatMessageStatusUpdateModel
    {
        protected long ChatMessageStatusId { get { return base.ChatMessageStatusId; } set { base.ChatMessageStatusId = value; } }
        //protected List<ChatMessageStatusXYZZYXModel> ExistingChatMessageStatusXYZZYXModels {get {return base.ExistingChatMessageStatusXYZZYXModels;} set { base.ExistingChatMessageStatusXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015