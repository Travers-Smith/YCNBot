using YCNBot.Core.Entities;

namespace YCNBot.Core.Repositories
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task<IEnumerable<Chat>> GetAllByUser(Guid userIdentifier, int skip, int take);

        Task<Chat> GetByUniqueIdentifier(Guid uniqueIdentifier);

        Task<Chat> GetByUniqueIdentifierWithMessages(Guid uniqueIdentifier);
    }
}