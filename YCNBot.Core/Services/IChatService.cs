using YCNBot.Core.Entities;

namespace YCNBot.Core.Services
{
    public interface IChatService
    {
        Task Add(Chat chat);

        Task<IEnumerable<Chat>> GetAllByUser(Guid userIdentifier, int skip, int take);

        Task<Chat> GetByUniqueIdentifier(Guid uniqueIdentifier);

        Task<Chat> GetByUniqueIdentifierWithMessages(Guid uniqueIdentifier);

        Task<Dictionary<Guid, int>> GetUsersUsage(int skip, int take);

        Task Update(Chat chat);
    }
}