using Microsoft.EntityFrameworkCore;
using YCNBot.Core.Entities;
using YCNBot.Core.Repositories;

namespace YCNBot.Data.Repositories
{
    public class CommunityPromptRepository : Repository<CommunityPrompt>, ICommunityPromptRepository
    {
        public CommunityPromptRepository(YCNBotContext context) : base(context) { }

        public async Task<IEnumerable<CommunityPromptWithLikesAndCommentsCount>> Get(int skip, int take, Guid userIdentifier)
        {
            return await _context.CommunityPrompts
                .OrderByDescending(cq => cq.Id)
                .Skip(skip)
                .Take(take)
                .Include(cp => cp.CommunityPromptLikes.Where(cpl => cpl.UserIdentifier == userIdentifier))
                .Select(x => new CommunityPromptWithLikesAndCommentsCount
                {
                    CommunityPrompt = x,
                    CommentsCount = x.CommunityPromptComments.Count(),
                    LikesCount = x.CommunityPromptLikes.Count()
                })
                .ToArrayAsync();
        }
    }
}
