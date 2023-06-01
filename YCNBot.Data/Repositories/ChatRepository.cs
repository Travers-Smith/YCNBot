using Microsoft.EntityFrameworkCore;
using YCNBot.Core.Entities;
using YCNBot.Core.Repositories;

namespace YCNBot.Data.Repositories
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        public ChatRepository(YCNBotContext context) : base(context)
        { }

        public async Task<IEnumerable<Chat>> GetAllByUser(Guid userIdentifier, int skip, int take)
        {
            return await _context.Chats
                .Where(x => x.UserIdentifier == userIdentifier && x.Deleted != true)
                .OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToArrayAsync();
        }

        public async Task<Chat> GetByUniqueIdentifier(Guid uniqueIdentifier)
        {
            return await _context.Chats
                .AsNoTracking()
                .FirstAsync(x => x.UniqueIdentifier == uniqueIdentifier && x.Deleted != true);
        }

        public async Task<Chat> GetByUniqueIdentifierWithMessages(Guid uniqueIdentifier)
        {
            return await _context.Chats
                .Where(x => x.UniqueIdentifier == uniqueIdentifier && x.Deleted != true)
                .Include(x => x.Messages)
                .FirstAsync();
        }

        public async Task<Dictionary<Guid, int>> GetUsersUsage(int skip, int take)
        {
            return await _context.Chats
                .GroupBy(x => x.UserIdentifier)
                .ToDictionaryAsync(x => x.Key, x => x.Count());
        }

        public async Task<int> GetUsersCount()
        {
            return await _context.Chats
                .Select(x => x.UserIdentifier)
                .Distinct()
                .CountAsync();
        }
    }
}
