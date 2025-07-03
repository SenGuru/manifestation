using System.Threading.Tasks;
using Manifest.Common.Models.Queue;
using Manifest.Common.Models.ViewModels;

namespace Manifest.Common.Interfaces.Service.Notification
{
    public interface IRealtimeNotificationHubService
    {
        Task UserTypingAsync(UserModel msg);

        Task ReceiveGroupNameChangedAsync(ChatModel model);

        Task ReceiveChatMessageDeletedAsync(ChatMessageModel model);

        Task ReceiveNewChatUsersAddedAsync();

        Task RecieveMessageAsync(ChatMessageModel msg);

        Task SendMessageAsync(ChatMessageModel msg);

        Task SendNotificationMessage(RealtimeMessageQueueModel msg);

        Task RecieveNotificationMessage(RealtimeMessageQueueModel msg);
    }
}
