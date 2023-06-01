using YCNBot.Core.Entities;

namespace YCNBot.Core.Repositories
{
    public interface ICommunityPromptRepository : IRepository<CommunityPrompt>
    {
        Task<IEnumerable<CommunityPromptWithLikesAndCommentsCount>> Get(int skip, int take, Guid userIdentifier);
    }
}