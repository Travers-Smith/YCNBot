using Microsoft.EntityFrameworkCore;
using YCNBot.Core.Entities;
using YCNBot.Core.Repositories;

namespace YCNBot.Data.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(YCNBotContext context) : base(context)
        { }
        public async Task<Message> GetByUniqueIdentifierWithChat(Guid uniqueIdentifier)
        {
            return await _context.Messages
                .Include(x => x.Chat)
                .FirstAsync(x => x.UniqueIdentifier == uniqueIdentifier);
        }

        public async Task<IEnumerable<Tuple<DateTime, int>>> GetDateBreakdown(int previousDays)
        {
            return await _context.Messages
                .Where(message => message.DateAdded != null && message.DateAdded > DateTime.Now.AddDays(-previousDays))
                .GroupBy(message => message.DateAdded.Value.Date)
                .OrderBy(x => x.Key)
                .Select(x => Tuple.Create(x.Key, x.Count()))
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Message>> GetMessagesByChat(Guid chatIdentifier, int skip, int take)
        {
            return await _context.Messages
                .Where(x => x.Chat.UniqueIdentifier == chatIdentifier)
                .OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToArrayAsync();
        }
    }
}
