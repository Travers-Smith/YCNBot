using Microsoft.EntityFrameworkCore;
using YCNBot.Core.Entities;
using YCNBot.Core.Repositories;

namespace YCNBot.Data.Repositories
{
    public class CommunityPromptCommentRepository : Repository<CommunityPromptComment>, ICommunityPromptCommentRepository
    {
        public CommunityPromptCommentRepository(YCNBotContext context) : base(context) { }

        public async Task<IEnumerable<CommunityPromptComment>> GetByCommunityPromptId(int communityPromptId, int skip, int take)
        {
            return await _context.CommunityPromptComments
                .Where(cps => cps.CommunityPromptId == communityPromptId)
                .OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToArrayAsync();
        }
    }
}
