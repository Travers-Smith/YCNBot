using YCNBot.Core.Entities;

namespace YCNBot.Core.Services
{
    public interface IMessageService
    {
        Task AddRange(IEnumerable<Message> messages);

        Task<Message> GetByUniqueIdentifierWithChat(Guid uniqueIdentifier);

        Task Update(Message message);
    }
}