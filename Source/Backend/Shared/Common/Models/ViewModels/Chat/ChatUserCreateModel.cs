#pragma warning disable CS0108,SA1502,S4015

namespace Manifest.Common.Models.ViewModels
{
    public class ChatUserCreateModel : ChatUserUpdateModel
    {
        protected long ChatUserId { get { return base.ChatUserId; } set { base.ChatUserId = value; } }
        //protected List<ChatUserXYZZYXModel> ExistingChatUserXYZZYXModels {get {return base.ExistingChatUserXYZZYXModels;} set { base.ExistingChatUserXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015