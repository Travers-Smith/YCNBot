using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;

namespace YCNBot.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Chat chat)
        {
            await _unitOfWork.Chat.AddAsync(chat);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Chat>> GetAllByUser(Guid userIdentifier, int skip, int take)
        {
            return await _unitOfWork.Chat.GetAllByUser(userIdentifier, skip, take);
        }

        public async Task<Chat> GetByUniqueIdentifier(Guid uniqueIdentifier)
        {
            return await _unitOfWork.Chat.GetByUniqueIdentifier(uniqueIdentifier);
        }

        public async Task<Chat> GetByUniqueIdentifierWithMessages(Guid uniqueIdentifier)
        {
            return await _unitOfWork.Chat.GetByUniqueIdentifierWithMessages(uniqueIdentifier);
        }

        public async Task<int> GetCount()
        {
            return await _unitOfWork.Chat.CountAsync();
        }
        public async Task<int> GetUsersCount()
        {
            return await _unitOfWork.Chat.GetUsersCount();
        }

        public async Task<Dictionary<Guid, int>> GetUsersUsage(int skip, int take)
        {
            return await _unitOfWork.Chat.GetUsersUsage(skip, take);
        }

        public async Task Update(Chat chat)
        {
            _unitOfWork.Chat.Update(chat);

            await _unitOfWork.CommitAsync();
        }
    }
}