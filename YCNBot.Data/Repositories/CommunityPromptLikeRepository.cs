using Microsoft.EntityFrameworkCore;
using YCNBot.Core.Entities;
using YCNBot.Core.Repositories;

namespace YCNBot.Data.Repositories
{
    public class CommunityPromptLikeRepository : Repository<CommunityPromptLike>, ICommunityPromptLikeRepository
    {
        public CommunityPromptLikeRepository(YCNBotContext context) : base(context) { }

        public async Task<CommunityPromptLike?> GetByCommunityPromptIdAndUser(int communityPromptId, Guid userIdentifier)
        {
            return await _context.CommunityPromptLikes
                .FirstOrDefaultAsync(cpl => cpl.CommunityPromptId == communityPromptId && cpl.UserIdentifier == userIdentifier);
        }
        public async Task<IEnumerable<CommunityPromptLike>> GetByCommunityPromptId(int communityPromptId, int skip, int take)
        {
            return await _context.CommunityPromptLikes
                .Where(cps => cps.CommunityPromptId == communityPromptId)
                .OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToArrayAsync();
        }
    }
}
