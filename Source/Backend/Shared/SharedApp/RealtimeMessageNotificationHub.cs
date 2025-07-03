using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manifest.Common.Interfaces.Service.Notification;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.ViewModels;
using Manifest.Repository.Common;
using Manifest.Service.App;
using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.Utils;
using Microsoft.AspNetCore.SignalR;

namespace Manifest.SharedApp
{
    public class RealtimeMessageNotificationHub : Hub<IRealtimeNotificationHubService>
    {
        private readonly CompanyRepository companyRepository;
        private readonly ChatRepository chatRepository;
        private readonly ChatService chatService;
        private readonly ObjectMapperService objectMapperService;

        public RealtimeMessageNotificationHub(CompanyRepository companyRepository, ChatRepository chatRepository, ObjectMapperService objectMapperService, ChatService chatService)
        {
            this.companyRepository = companyRepository;
            this.chatRepository = chatRepository;
            this.objectMapperService = objectMapperService;
            this.chatService = chatService;
        }

        public async Task SendMessageAsync(ChatMessageCreateModel chatMessageCreateModel)
        {
            var session = Context.User;
            if (session == null || session.Identity == null || !session.Identity.IsAuthenticated)
            {
                throw new HubException("User is not authenticated");
            }

            var user = await companyRepository.GetUserByUserNameAsync(session.Identity.Name);
            if (user == null)
                return;

            var chatMessage = await chatService.CreateChatMessageAsync(chatMessageCreateModel);
            if (!chatMessage.IsSuccess)
                return;

            await Clients.Group(chatMessage.Model.ChatId.ToString()).RecieveMessageAsync(chatMessage.Model);
        }

        public async Task JoinChatAsync(ChatUserCreateModel chatUserCreateModel)
        {
            var session = Context.User;
            if (session == null || session.Identity == null || !session.Identity.IsAuthenticated)
            {
                throw new HubException("User is not authenticated");
            }

            var user = await companyRepository.GetUserByUserNameAsync(session.Identity.Name);
            if (user == null)
                return;

            var chatUser = await chatRepository.CreateChatUserAsync(objectMapperService.ChatUserCreateModelToChatUser(chatUserCreateModel), user.UserId);
            if (chatUser == null)
                return;

            await Groups.AddToGroupAsync(this.Context.ConnectionId, chatUser.ChatId.ToString());
        }

        //public async Task LeaveChat(long chatId)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        //}

        public async Task SendTypingIndicatorAsync(long chatId)
        {
            var session = Context.User;
            if (session != null && session.Identity != null && session.Identity.IsAuthenticated)
            {
                var user = await companyRepository.GetUserByUserNameAsync(session.Identity.Name);
                if (user == null)
                    return;

                await Clients.Group(chatId.ToString()).UserTypingAsync(objectMapperService.UserToUserModel(user));
            }
        }

        public async Task SendChatNameChangeAsync(ChatModel model)
        {
            var session = Context.User;
            if (session != null && session.Identity != null && session.Identity.IsAuthenticated)
            {
                var user = await companyRepository.GetUserByUserNameAsync(session.Identity.Name);
                if (user == null)
                    return;

                await Clients.Group(model.ChatId.ToString()).ReceiveGroupNameChangedAsync(model);
            }
        }

        public async Task ChatMessageDeletedAsync(ChatMessageModel model)
        {
            var session = Context.User;
            if (session != null && session.Identity != null && session.Identity.IsAuthenticated)
            {
                var user = await companyRepository.GetUserByUserNameAsync(session.Identity.Name);
                if (user == null)
                    return;

                await Clients.Group(model.ChatId.ToString()).ReceiveChatMessageDeletedAsync(model);
            }
        }

        public async Task ChatUserAddedAsync(long chatId)
        {
            var session = Context.User;
            if (session != null && session.Identity != null && session.Identity.IsAuthenticated)
            {
                var user = await companyRepository.GetUserByUserNameAsync(session.Identity.Name);
                if (user == null)
                    return;

                await Clients.Group(chatId.ToString()).ReceiveNewChatUsersAddedAsync();
            }
        }

        public override async Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            Console.WriteLine($"{name}-{Context.ConnectionId}");
            if (!name.IsTrimmedStringNullOrEmpty())
                await Groups.AddToGroupAsync(this.Context.ConnectionId, name);

            var user = await companyRepository.GetUserByUserNameAsync(name);
            if (user == null)
                return;

            var userChats = await chatRepository.GetChatUsersByUserIdAsync(user.UserId);
            userChats ??= new List<ChatUser>();
            var userChatsIds = userChats.Select(x => x.ChatId).Distinct().ToList();

            foreach (var chatId in userChatsIds)
            {
                await Groups.AddToGroupAsync(this.Context.ConnectionId, chatId.ToString());
            }

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
           string name = Context.User.Identity.Name;
           Console.WriteLine($"{name}-{Context.ConnectionId}");
           if (exception != null)
            {
                Console.WriteLine(exception.Message);
            }

           return base.OnDisconnectedAsync(exception);
        }
    }
}