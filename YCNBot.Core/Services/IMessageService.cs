using YCNBot.Core.Entities;

namespace YCNBot.Core.Services
{
    public interface IMessageService
    {
        Task AddRange(IEnumerable<Message> messages);

        Task<IEnumerable<Tuple<DateTime, int>>> GetDateBreakdown(int previousDays);

        Task<Message> GetByUniqueIdentifierWithChat(Guid uniqueIdentifier);

        Task<int> GetCount();

        Task Update(Message message);
    }
}