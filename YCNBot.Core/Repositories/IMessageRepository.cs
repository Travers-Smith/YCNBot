using YCNBot.Core.Entities;

namespace YCNBot.Core.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<Message> GetByUniqueIdentifierWithChat(Guid uniqueIdentifier);

        Task<IEnumerable<Message>> GetMessagesByChat(Guid chatIdentifier, int skip, int take);

        Task<IEnumerable<Tuple<DateTime, int>>> GetDateBreakdown(int previousDays);
    }
}