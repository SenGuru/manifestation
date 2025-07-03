#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    public class ChatCreateModel : ChatUpdateModel
    {
        protected long ChatId { get { return base.ChatId; } set { base.ChatId = value; } }
        //protected List<ChatXYZZYXModel> ExistingChatXYZZYXModels {get {return base.ExistingChatXYZZYXModels;} set { base.ExistingChatXYZZYXModels = value; }}
    }
}
#pragma warning restore CS0108, SA1502, S4015