using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Repository.DbAccess;
using Manifest.Common.Models.DB;
using Manifest.Common.Models.DBView;
using Manifest.Common.Models.SearchCriteria;
using Framework.Data.Utils;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using LinqToDB;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Manifest.Repository.Common
{
    public class ChatRepository : BaseRepository
    {
        public ChatRepository(IAppDBManager appDBManager)
            : base(appDBManager)
        {
        }

        #region Chat

        public IQueryable<ChatView> GetChatsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<Chat>()
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new ChatView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       ChatId = item.ChatId,
                       ChatName = item.ChatName,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<ChatView> GetChatsInternalWithSearchCriteriaAsync(ChatSearchCriteria search)
        {
            var sql1 = GetChatsViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.ChatId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.ChatName.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<ChatView>> GetChatsInternalAsync(ChatSearchCriteria search)
        {
            var sql1 = GetChatsInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.ChatId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetChatsViewAsIQueryable().FirstOrDefaultAsync(x => x.ChatId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<ChatView>> GetChatsAsync(ChatSearchCriteria search)
        {
            return GetChatsInternalAsync(search);
        }

        public Task<Chat> GetChatFromAnyByIdAsync(long chatId)
        {
            return appDBManager.FirstOrDefaultAsync<Chat>(x => x.ChatId == chatId);
        }

        public Task<ChatView> GetChatViewFromAnyByIdAsync(long chatId)
        {
            return GetChatsViewAsIQueryable().FirstOrDefaultAsync(x => x.ChatId == chatId);
        }

        public Task<Chat> GetChatByUniqueIdentifierAsync(string chatId)
        {
            return appDBManager.FirstOrDefaultAsync<Chat>(x => x.ChatId == SafeUtils.Int(chatId) && x.IsDeleted == false);
        }

        public Task<Chat> GetChatByIdAsync(long chatId)
        {
            return appDBManager.FirstOrDefaultAsync<Chat>(x => x.ChatId == chatId && x.IsDeleted == false);
        }

        public Task<ChatView> GetChatViewByIdAsync(long chatId)
        {
            var sql1 = GetChatsViewAsIQueryable();

            sql1 = sql1.Where(x => x.ChatId == chatId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<ChatView> GetChatViewByNameAndCompanyAsync(string chatName, long userId)
        {
            return GetChatsViewAsIQueryable()
                .Where(x => x.IsDeleted == false
                            && x.ChatName.ToLower().Trim() == chatName.ToLower().Trim()
                            && x.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public Task<List<Chat>> GetAllChats()
        {
            return appDBManager.SelectAllAsync<Chat>();
        }

        public async Task<Chat> CreateChatAsync(Chat chat, long? createdBy)
        {
            chat.ChatId = await appDBManager.InsertWithAuditAsync<Chat, long>(chat, createdBy);
            return chat;
        }

        public async Task<List<Chat>> CreateChatsAsync(List<Chat> chats, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<Chat, long>(chats, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                chats[i].ChatId = lst[i];
            return chats;
        }

        public Task<bool> UpdateChatAsync(Chat chat, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<Chat>(chat, modifiedBy);
        }

        public async Task<bool> MarkChatAsNotDeletedByIdAsync(long chatId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Chat>().Where(x => x.ChatId == chatId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkChatAsDeletedByIdAsync(long chatId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Chat>().Where(x => x.ChatId == chatId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteChatByIdAsync(long chatId)
        {
            return await appDBManager.GetTable<Chat>().Where(x => x.ChatId == chatId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetChatActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.Chat);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> DoesChatByIdExistsAsync(long chatId, long? userId = null)
        {
            if (userId == null)
            {
                return appDBManager.GetTable<Chat>().Where(x => x.ChatId == chatId && x.IsDeleted == false).Select(x => x.ChatId).AnyAsync();
            }

            return appDBManager.GetTable<Chat>().Where(x => x.ChatId == chatId && x.UserId == userId && x.IsDeleted == false).Select(x => x.ChatId).AnyAsync();
        }

        public Task<bool> IsChatNameAlreadyExistsForOtherChatAsync(long chatId, string chatName, long userId)
        {
            var sql = appDBManager.GetTable<Chat>().Where(x => x.IsDeleted == false && x.ChatId != chatId && x.ChatName.ToLower().Trim() == chatName.ToLower().Trim() && x.UserId == userId).Select(x => x.ChatId);
            return sql.AnyAsync();
        }

        public Task<bool> IsChatNameAlreadyExistsForOtherChatAsync(string chatName, long userId)
        {
            var sql = appDBManager.GetTable<Chat>().Where(x => x.IsDeleted == false && x.ChatName.ToLower().Trim() == chatName.ToLower().Trim() && x.UserId == userId).Select(x => x.ChatId);
            return sql.AnyAsync();
        }

        /*

        public async Task<bool> UpdateChatPicturePathAsync(long chatId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<Chat>().Where(x => x.ChatId == chatId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearChatProfilePathAsync(long chatId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Chat>().Where(x => x.ChatId == chatId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableChatByIdAsync(long chatId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Chat>().Where(x => x.ChatId == chatId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableChatByIdAsync(long chatId, long? modifiedBy)
        {
            return await appDBManager.GetTable<Chat>().Where(x => x.ChatId == chatId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateChatWithXYZZYXAsync(long chatId, long xYZZYXId, long? createdBy)
        {
            var item = new ChatXYZZYXAssociationModel();
            item.ChatId = chatId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<ChatXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateChatWithXYZZYXAsync(long chatId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<ChatXYZZYX>(x => x.ChatId == chatId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesChatWithXYZZYXAssociationExistsAsync(long chatId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<ChatXYZZYX>(x => x.ChatId == chatId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<ChatView>> GetChatsByXYZZYXAsync(long xYZZYXId)
        {
            var chatIds = GetChatXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.ChatId);
            var chatSql = GetChatsViewAsIQueryable().Where(x => chatIds.Contains(x.ChatId));
            return chatSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByChatsAsync(long chatId)
        {
            var xYZZYXIds = GetChatXYZZYXsViewAsIQueryable().Where(x => x.ChatId == chat).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<ChatXYZZYX> GetChatXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ChatXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetChatXYZZYXActivitiesAsync(long companyId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.CompanyId == companyId && x.ResourceTypeId == (int)ResourceTypes.ChatXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region ChatMessage

        public IQueryable<ChatMessageView> GetChatMessagesViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ChatMessage>()
                   join a in appDBManager.GetTable<User>() on item.UserId equals a.UserId
                join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new ChatMessageView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       UserName = a.UserName,
                       ChatId = item.ChatId,
                       ChatMessageId = item.ChatMessageId,
                       UserId = item.UserId,
                       ChatMessageText = item.ChatMessageText,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<ChatMessageView> GetChatMessagesInternalWithSearchCriteriaAsync(ChatMessageSearchCriteria search)
        {
            var sql1 = GetChatMessagesViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x =>
                                x.ChatMessageId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper()
                                || x.ChatMessageText.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper()));
            }

            if (search.UserId != null && search.UserId > 0)
                sql1 = sql1.Where(x => search.UserId == x.UserId);
            if (search.ChatId != null && search.ChatId > 0)
                sql1 = sql1.Where(x => search.ChatId == x.ChatId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<ChatMessageView>> GetChatMessagesInternalAsync(ChatMessageSearchCriteria search)
        {
            var sql1 = GetChatMessagesInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.ChatMessageId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetChatMessagesViewAsIQueryable().FirstOrDefaultAsync(x => x.ChatMessageId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<ChatMessageView>> GetChatMessagesAsync(ChatMessageSearchCriteria search)
        {
            return GetChatMessagesInternalAsync(search);
        }

        public Task<ChatMessage> GetChatMessageFromAnyByIdAsync(long chatMessageId)
        {
            return appDBManager.FirstOrDefaultAsync<ChatMessage>(x => x.ChatMessageId == chatMessageId);
        }

        public Task<ChatMessageView> GetChatMessageViewFromAnyByIdAsync(long chatMessageId)
        {
            return GetChatMessagesViewAsIQueryable().FirstOrDefaultAsync(x => x.ChatMessageId == chatMessageId);
        }

        public Task<ChatMessage> GetChatMessageByUniqueIdentifierAsync(string chatMessageId)
        {
            return appDBManager.FirstOrDefaultAsync<ChatMessage>(x => x.ChatMessageId == SafeUtils.Int(chatMessageId) && x.IsDeleted == false);
        }

        public Task<ChatMessage> GetChatMessageByIdAsync(long chatMessageId)
        {
            return appDBManager.FirstOrDefaultAsync<ChatMessage>(x => x.ChatMessageId == chatMessageId && x.IsDeleted == false);
        }

        public Task<ChatMessageView> GetChatMessageViewByIdAsync(long chatMessageId)
        {
            var sql1 = GetChatMessagesViewAsIQueryable();

            sql1 = sql1.Where(x => x.ChatMessageId == chatMessageId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<ChatMessage>> GetAllChatMessages()
        {
            return appDBManager.SelectAllAsync<ChatMessage>();
        }

        public async Task<ChatMessage> CreateChatMessageAsync(ChatMessage chatMessage, long? createdBy)
        {
            chatMessage.ChatMessageId = await appDBManager.InsertWithAuditAsync<ChatMessage, long>(chatMessage, createdBy);
            return chatMessage;
        }

        public async Task<List<ChatMessage>> CreateChatMessagesAsync(List<ChatMessage> chatMessages, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<ChatMessage, long>(chatMessages, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                chatMessages[i].ChatMessageId = lst[i];
            return chatMessages;
        }

        public Task<bool> UpdateChatMessageAsync(ChatMessage chatMessage, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<ChatMessage>(chatMessage, modifiedBy);
        }

        public async Task<bool> MarkChatMessageAsNotDeletedByIdAsync(long chatMessageId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatMessage>().Where(x => x.ChatMessageId == chatMessageId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkChatMessageAsDeletedByIdAsync(long chatMessageId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatMessage>().Where(x => x.ChatMessageId == chatMessageId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteChatMessageByIdAsync(long chatMessageId)
        {
            return await appDBManager.GetTable<ChatMessage>().Where(x => x.ChatMessageId == chatMessageId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetChatMessageActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.ChatMessage);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> DoesChatMessageByIdExistsAsync(long chatMessageId, long? userId = null)
        {
            if (userId == null)
            {
                return appDBManager.GetTable<ChatMessage>().Where(x => x.ChatMessageId == chatMessageId && x.IsDeleted == false).Select(x => x.ChatMessageId).AnyAsync();
            }

            return appDBManager.GetTable<ChatMessage>().Where(x => x.ChatMessageId == chatMessageId && x.UserId == userId && x.IsDeleted == false).Select(x => x.ChatMessageId).AnyAsync();
        }

        /*

        public async Task<bool> UpdateChatMessagePicturePathAsync(long chatMessageId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatMessage>().Where(x => x.ChatMessageId == chatMessageId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearChatMessageProfilePathAsync(long chatMessageId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatMessage>().Where(x => x.ChatMessageId == chatMessageId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableChatMessageByIdAsync(long chatMessageId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatMessage>().Where(x => x.ChatMessageId == chatMessageId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableChatMessageByIdAsync(long chatMessageId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatMessage>().Where(x => x.ChatMessageId == chatMessageId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateChatMessageWithXYZZYXAsync(long chatMessageId, long xYZZYXId, long? createdBy)
        {
            var item = new ChatMessageXYZZYXAssociationModel();
            item.ChatMessageId = chatMessageId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<ChatMessageXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateChatMessageWithXYZZYXAsync(long chatMessageId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<ChatMessageXYZZYX>(x => x.ChatMessageId == chatMessageId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesChatMessageWithXYZZYXAssociationExistsAsync(long chatMessageId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<ChatMessageXYZZYX>(x => x.ChatMessageId == chatMessageId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<ChatMessageView>> GetChatMessagesByXYZZYXAsync(long xYZZYXId)
        {
            var chatMessageIds = GetChatMessageXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.ChatMessageId);
            var chatMessageSql = GetChatMessagesViewAsIQueryable().Where(x => chatMessageIds.Contains(x.ChatMessageId));
            return chatMessageSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByChatMessagesAsync(long chatMessageId)
        {
            var xYZZYXIds = GetChatMessageXYZZYXsViewAsIQueryable().Where(x => x.ChatMessageId == chatMessage).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<ChatMessageXYZZYX> GetChatMessageXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ChatMessageXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetChatMessageXYZZYXActivitiesAsync(long companyId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.CompanyId == companyId && x.ResourceTypeId == (int)ResourceTypes.ChatMessageXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region ChatMessageStatus

        public IQueryable<ChatMessageStatusView> GetChatMessageStatussViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ChatMessageStatus>()
                   join a in appDBManager.GetTable<ChatMessage>() on item.ChatMessageId equals a.ChatMessageId
                   join b in appDBManager.GetTable<User>() on item.UserId equals b.UserId
                join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new ChatMessageStatusView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       UserName = b.UserName,
                       UserFirstName = b.FirstName,
                       UserLastName = b.LastName,
                       ChatMessageStatusId = item.ChatMessageStatusId,
                       ChatMessageId = item.ChatMessageId,
                       UserId = item.UserId,
                       ChatMessageViewedDate = item.ChatMessageViewedDate,
                       ChatMessageSentDate = item.ChatMessageSentDate,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<ChatMessageStatusView> GetChatMessageStatussInternalWithSearchCriteriaAsync(ChatMessageStatusSearchCriteria search)
        {
            var sql1 = GetChatMessageStatussViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x => x.ChatMessageStatusId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper());
            }

            if (search.ChatMessageId != null && search.ChatMessageId > 0)
                sql1 = sql1.Where(x => search.ChatMessageId == x.ChatMessageId);
            if (search.UserId != null && search.UserId > 0)
                sql1 = sql1.Where(x => search.UserId == x.UserId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<ChatMessageStatusView>> GetChatMessageStatussInternalAsync(ChatMessageStatusSearchCriteria search)
        {
            var sql1 = GetChatMessageStatussInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.ChatMessageStatusId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetChatMessageStatussViewAsIQueryable().FirstOrDefaultAsync(x => x.ChatMessageStatusId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<ChatMessageStatusView>> GetChatMessageStatussAsync(ChatMessageStatusSearchCriteria search)
        {
            return GetChatMessageStatussInternalAsync(search);
        }

        public Task<ChatMessageStatus> GetChatMessageStatusFromAnyByIdAsync(long chatMessageStatusId)
        {
            return appDBManager.FirstOrDefaultAsync<ChatMessageStatus>(x => x.ChatMessageStatusId == chatMessageStatusId);
        }

        public Task<ChatMessageStatusView> GetChatMessageStatusViewFromAnyByIdAsync(long chatMessageStatusId)
        {
            return GetChatMessageStatussViewAsIQueryable().FirstOrDefaultAsync(x => x.ChatMessageStatusId == chatMessageStatusId);
        }

        public Task<ChatMessageStatus> GetChatMessageStatusByUniqueIdentifierAsync(string chatMessageStatusId)
        {
            return appDBManager.FirstOrDefaultAsync<ChatMessageStatus>(x => x.ChatMessageStatusId == SafeUtils.Int(chatMessageStatusId) && x.IsDeleted == false);
        }

        public Task<ChatMessageStatus> GetChatMessageStatusByIdAsync(long chatMessageStatusId)
        {
            return appDBManager.FirstOrDefaultAsync<ChatMessageStatus>(x => x.ChatMessageStatusId == chatMessageStatusId && x.IsDeleted == false);
        }

        public Task<ChatMessageStatusView> GetChatMessageStatusViewByIdAsync(long chatMessageStatusId)
        {
            var sql1 = GetChatMessageStatussViewAsIQueryable();

            sql1 = sql1.Where(x => x.ChatMessageStatusId == chatMessageStatusId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<ChatMessageStatus>> GetAllChatMessageStatuss()
        {
            return appDBManager.SelectAllAsync<ChatMessageStatus>();
        }

        public async Task<ChatMessageStatus> CreateChatMessageStatusAsync(ChatMessageStatus chatMessageStatus, long? createdBy)
        {
            chatMessageStatus.ChatMessageStatusId = await appDBManager.InsertWithAuditAsync<ChatMessageStatus, long>(chatMessageStatus, createdBy);
            return chatMessageStatus;
        }

        public async Task<List<ChatMessageStatus>> CreateChatMessageStatussAsync(List<ChatMessageStatus> chatMessageStatuss, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<ChatMessageStatus, long>(chatMessageStatuss, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                chatMessageStatuss[i].ChatMessageStatusId = lst[i];
            return chatMessageStatuss;
        }

        public Task<bool> UpdateChatMessageStatusAsync(ChatMessageStatus chatMessageStatus, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<ChatMessageStatus>(chatMessageStatus, modifiedBy);
        }

        public async Task<bool> MarkChatMessageStatusAsNotDeletedByIdAsync(long chatMessageStatusId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatMessageStatus>().Where(x => x.ChatMessageStatusId == chatMessageStatusId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkChatMessageStatusAsDeletedByIdAsync(long chatMessageStatusId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatMessageStatus>().Where(x => x.ChatMessageStatusId == chatMessageStatusId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteChatMessageStatusByIdAsync(long chatMessageStatusId)
        {
            return await appDBManager.GetTable<ChatMessageStatus>().Where(x => x.ChatMessageStatusId == chatMessageStatusId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetChatMessageStatusActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.ChatMessageStatus);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> DoesChatMessageStatusByIdExistsAsync(long chatMessageStatusId, long? companyId = null)
        {
            return appDBManager.GetTable<ChatMessageStatus>().Where(x => x.ChatMessageStatusId == chatMessageStatusId && x.IsDeleted == false).Select(x => x.ChatMessageStatusId).AnyAsync();
        }

        /*
        public async Task<bool> UpdateChatMessageStatusPicturePathAsync(long chatMessageStatusId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatMessageStatus>().Where(x => x.ChatMessageStatusId == chatMessageStatusId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearChatMessageStatusProfilePathAsync(long chatMessageStatusId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatMessageStatus>().Where(x => x.ChatMessageStatusId == chatMessageStatusId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableChatMessageStatusByIdAsync(long chatMessageStatusId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatMessageStatus>().Where(x => x.ChatMessageStatusId == chatMessageStatusId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableChatMessageStatusByIdAsync(long chatMessageStatusId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatMessageStatus>().Where(x => x.ChatMessageStatusId == chatMessageStatusId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateChatMessageStatusWithXYZZYXAsync(long chatMessageStatusId, long xYZZYXId, long? createdBy)
        {
            var item = new ChatMessageStatusXYZZYXAssociationModel();
            item.ChatMessageStatusId = chatMessageStatusId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<ChatMessageStatusXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateChatMessageStatusWithXYZZYXAsync(long chatMessageStatusId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<ChatMessageStatusXYZZYX>(x => x.ChatMessageStatusId == chatMessageStatusId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesChatMessageStatusWithXYZZYXAssociationExistsAsync(long chatMessageStatusId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<ChatMessageStatusXYZZYX>(x => x.ChatMessageStatusId == chatMessageStatusId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<ChatMessageStatusView>> GetChatMessageStatussByXYZZYXAsync(long xYZZYXId)
        {
            var chatMessageStatusIds = GetChatMessageStatusXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.ChatMessageStatusId);
            var chatMessageStatusSql = GetChatMessageStatussViewAsIQueryable().Where(x => chatMessageStatusIds.Contains(x.ChatMessageStatusId));
            return chatMessageStatusSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByChatMessageStatussAsync(long chatMessageStatusId)
        {
            var xYZZYXIds = GetChatMessageStatusXYZZYXsViewAsIQueryable().Where(x => x.ChatMessageStatusId == chatMessageStatus).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<ChatMessageStatusXYZZYX> GetChatMessageStatusXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ChatMessageStatusXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetChatMessageStatusXYZZYXActivitiesAsync(long companyId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.CompanyId == companyId && x.ResourceTypeId == (int)ResourceTypes.ChatMessageStatusXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

        #region ChatUser

        public IQueryable<ChatUserView> GetChatUsersViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ChatUser>()
                   join a in appDBManager.GetTable<User>() on item.UserId equals a.UserId
                   join ut in GetEnumQueryableOfPredefinedUserType() on a.PredefinedUserTypeId equals ut.EnumTypeId
                   join ocu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.CreatedByUserId equals ocu.UserId into cuTemp
                   from cu in cuTemp.DefaultIfEmpty()
                   join omu in appDBManager.GetTableWithOutTenantOrCompanySetter<User>() on item.ModifiedByUserId equals omu.UserId into muTemp
                   from mu in muTemp.DefaultIfEmpty()
                   select new ChatUserView
                   {
                       CreatedByUserFirstName = cu.FirstName,
                       CreatedByUserLastName = cu.LastName,
                       CreatedByUserEmail = cu.Email,
                       ModifiedByUserFirstName = mu.FirstName,
                       ModifiedByUserLastName = mu.LastName,
                       ModifiedByUserEmail = mu.Email,
                       UserName = a.UserName,
                       PredefinedUserTypeId = ut.EnumTypeId,
                       PredefinedUserTypeName = ut.EnumTypeName,
                       UserFirstName = a.FirstName,
                       UserLastName = a.LastName,
                       ChatId = item.ChatId,
                       ChatUserId = item.ChatUserId,
                       UserId = item.UserId,
                       IsDeleted = item.IsDeleted,
                       CreatedDate = item.CreatedDate,
                       CreatedByUserId = item.CreatedByUserId,
                       ModifiedDate = item.ModifiedDate,
                       ModifiedByUserId = item.ModifiedByUserId,
                   };
        }

        public IQueryable<ChatUserView> GetChatUsersInternalWithSearchCriteriaAsync(ChatUserSearchCriteria search)
        {
            var sql1 = GetChatUsersViewAsIQueryable();

            if (!search.Keyword.IsTrimmedStringNullOrEmpty())
            {
                sql1 = sql1.Where(x => x.ChatUserId.ToString().Trim().ToUpper() == search.Keyword.Trim().ToUpper());
            }

            if (search.UserId != null && search.UserId > 0)
                sql1 = sql1.Where(x => search.UserId == x.UserId);
            if (search.ChatId != null && search.ChatId > 0)
                sql1 = sql1.Where(x => search.ChatId == x.ChatId);
            if (search.CreatedByUserId != null && search.CreatedByUserId > 0)
                sql1 = sql1.Where(x => search.CreatedByUserId == x.CreatedByUserId);
            if (search.ModifiedByUserId != null && search.ModifiedByUserId > 0)
                sql1 = sql1.Where(x => search.ModifiedByUserId == x.ModifiedByUserId);

            if (search.ShowDeleted == false)
                sql1 = sql1.Where(x => x.IsDeleted == false);

            return sql1;
        }

        public async Task<DbReturnListModel<ChatUserView>> GetChatUsersInternalAsync(ChatUserSearchCriteria search)
        {
            var sql1 = GetChatUsersInternalWithSearchCriteriaAsync(search);
            var result = await sql1.ReturnListModelResultAsync(search);

            if (search.SelectedId != null && !result.Result.Exists(x => x.ChatUserId == (search.SelectedId ?? 0)))
            {
                var selectedItem = await GetChatUsersViewAsIQueryable().FirstOrDefaultAsync(x => x.ChatUserId == (search.SelectedId ?? 0));
                if (selectedItem != null)
                {
                    result.AppendItem(selectedItem);
                }
            }

            return result;
        }

        public Task<DbReturnListModel<ChatUserView>> GetChatUsersAsync(ChatUserSearchCriteria search)
        {
            return GetChatUsersInternalAsync(search);
        }

        public Task<ChatUser> GetChatUserFromAnyByIdAsync(long chatUserId)
        {
            return appDBManager.FirstOrDefaultAsync<ChatUser>(x => x.ChatUserId == chatUserId);
        }

        public Task<ChatUserView> GetChatUserViewFromAnyByIdAsync(long chatUserId)
        {
            return GetChatUsersViewAsIQueryable().FirstOrDefaultAsync(x => x.ChatUserId == chatUserId);
        }

        public Task<ChatUser> GetChatUserByUniqueIdentifierAsync(string chatUserId)
        {
            return appDBManager.FirstOrDefaultAsync<ChatUser>(x => x.ChatUserId == SafeUtils.Int(chatUserId) && x.IsDeleted == false);
        }

        public Task<ChatUser> GetChatUserByIdAsync(long chatUserId)
        {
            return appDBManager.FirstOrDefaultAsync<ChatUser>(x => x.ChatUserId == chatUserId && x.IsDeleted == false);
        }

        public Task<List<ChatUser>> GetChatUsersByUserIdAsync(long userId)
        {
            return appDBManager.GetTable<ChatUser>().Where(x => x.UserId == userId && x.IsDeleted == false).ToListAsync();
        }

        public Task<List<ChatUser>> GetChatUsersByChatIdAsync(long chatId)
        {
            return appDBManager.GetTable<ChatUser>().Where(x => x.ChatId == chatId && x.IsDeleted == false).ToListAsync();
        }

        public Task<List<ChatUserView>> GetChatUserViewsByChatIdAsync(long chatId)
        {
            return GetChatUsersViewAsIQueryable().Where(x => x.ChatId == chatId && x.IsDeleted == false).ToListAsync();
        }

        public Task<ChatUserView> GetChatUserViewByIdAsync(long chatUserId)
        {
            var sql1 = GetChatUsersViewAsIQueryable();

            sql1 = sql1.Where(x => x.ChatUserId == chatUserId && x.IsDeleted == false);

            return sql1.FirstOrDefaultAsync();
        }

        public Task<List<ChatUser>> GetAllChatUsers()
        {
            return appDBManager.SelectAllAsync<ChatUser>();
        }

        public async Task<ChatUser> CreateChatUserAsync(ChatUser chatUser, long? createdBy)
        {
            chatUser.ChatUserId = await appDBManager.InsertWithAuditAsync<ChatUser, long>(chatUser, createdBy);
            return chatUser;
        }

        public async Task<List<ChatUser>> CreateChatUsersAsync(List<ChatUser> chatUsers, long? createdBy)
        {
            var lst = await appDBManager.InsertWithAuditAsync<ChatUser, long>(chatUsers, createdBy);
            for (int i = 0; i < lst.Count; ++i)
                chatUsers[i].ChatUserId = lst[i];
            return chatUsers;
        }

        public Task<bool> UpdateChatUserAsync(ChatUser chatUser, long? modifiedBy)
        {
            return appDBManager.UpdateWithAuditAsync<ChatUser>(chatUser, modifiedBy);
        }

        public async Task<bool> MarkChatUserAsNotDeletedByIdAsync(long chatUserId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatUser>().Where(x => x.ChatUserId == chatUserId).Set(x => x.IsDeleted, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> MarkChatUserAsDeletedByIdAsync(long chatUserId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatUser>().Where(x => x.ChatUserId == chatUserId).Set(x => x.IsDeleted, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> HardDeleteChatUserByIdAsync(long chatUserId)
        {
            return await appDBManager.GetTable<ChatUser>().Where(x => x.ChatUserId == chatUserId).DeleteAsync() > 0;
        }

        public Task<DbReturnListModel<ActivityView>> GetChatUserActivitiesAsync(long userId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.UserId == userId && x.PredefinedResourceTypeId == (int)PredefinedResourceTypes.ChatUser);

            return GetActivitiesByExpressionAsync(srch, search);
        }

        public Task<bool> DoesChatUserByIdExistsAsync(long chatUserId, long? companyId = null)
        {
            return appDBManager.GetTable<ChatUser>().Where(x => x.ChatUserId == chatUserId && x.IsDeleted == false).Select(x => x.ChatUserId).AnyAsync();
        }

        /*

        public async Task<bool> UpdateChatUserPicturePathAsync(long chatUserId, string path, string actualFileName, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatUser>().Where(x => x.ChatUserId == chatUserId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, path).Set(x => x.ProfileImageActualFileName, actualFileName).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> ClearChatUserProfilePathAsync(long chatUserId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatUser>().Where(x => x.ChatUserId == chatUserId && x.IsDeleted == false).Set(x => x.ProfileImageLocation, (string)null).Set(x => x.ProfileImageActualFileName, (string)null).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> EnableChatUserByIdAsync(long chatUserId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatUser>().Where(x => x.ChatUserId == chatUserId).Set(x => x.IsEnabled, true).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> DisableChatUserByIdAsync(long chatUserId, long? modifiedBy)
        {
            return await appDBManager.GetTable<ChatUser>().Where(x => x.ChatUserId == chatUserId).Set(x => x.IsEnabled, false).UpdateWithAuditAsync(modifiedBy) > 0;
        }

        public async Task<bool> AssociateChatUserWithXYZZYXAsync(long chatUserId, long xYZZYXId, long? createdBy)
        {
            var item = new ChatUserXYZZYXAssociationModel();
            item.ChatUserId = chatUserId;
            item.XYZZYXId = xYZZYXId;

            return (await appDBManager.InsertWithAuditAsync<ChatUserXYZZYX, long>(item, createdBy)) > 0;
        }

        public async Task<bool> UnAssociateChatUserWithXYZZYXAsync(long chatUserId, long xYZZYXId)
        {
            return await appDBManager.DeleteAsync<ChatUserXYZZYX>(x => x.ChatUserId == chatUserId && x.XYZZYXId == xYZZYXId) > 0;
        }

        public Task<bool> DoesChatUserWithXYZZYXAssociationExistsAsync(long chatUserId, long xYZZYXId)
        {
            return appDBManager.ExistsAsync<ChatUserXYZZYX>(x => x.ChatUserId == chatUserId && x.XYZZYXId == xYZZYXId);
        }

        public Task<List<ChatUserView>> GetChatUsersByXYZZYXAsync(long xYZZYXId)
        {
            var chatUserIds = GetChatUserXYZZYXsViewAsIQueryable().Where(x => x.XYZZYXId == xYZZYXId).Select(x => x.ChatUserId);
            var chatUserSql = GetChatUsersViewAsIQueryable().Where(x => chatUserIds.Contains(x.ChatUserId));
            return chatUserSql.ToListAsync();
        }

        public Task<List<XYZZYXView>> GetXYZZYXsByChatUsersAsync(long chatUserId)
        {
            var xYZZYXIds = GetChatUserXYZZYXsViewAsIQueryable().Where(x => x.ChatUserId == chatUser).Select(x => x.XYZZYXId);
            var xYZZYXSql = GetXYZZYXsViewAsIQueryable().Where(x => xYZZYXIds.Contains(x.XYZZYXId));
            return xYZZYXSql.ToListAsync();
        }

        public IQueryable<ChatUserXYZZYX> GetChatUserXYZZYXsViewAsIQueryable()
        {
            return from item in appDBManager.GetTable<ChatUserXYZZYX>()
                    select item;
        }

        public Task<DbReturnListModel<ActivityView>> GetChatUserXYZZYXActivitiesAsync(long companyId, ActivitySearchCriteria search)
        {
            var srch = LinqUtils.True<ActivityView>();
            srch = srch.And(x => x.CompanyId == companyId && x.ResourceTypeId == (int)ResourceTypes.ChatUserXYZZYX);
            return GetActivitiesByExpressionAsync(srch, search);
        }
        */
        #endregion

    }
}