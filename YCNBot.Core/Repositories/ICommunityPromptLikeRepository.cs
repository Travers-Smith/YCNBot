using YCNBot.Core.Entities;

namespace YCNBot.Core.Repositories
{
    public interface ICommunityPromptLikeRepository : IRepository<CommunityPromptLike>
    {
        Task<IEnumerable<CommunityPromptLike>> GetByCommunityPromptId(int communityPromptId, int skip, int take);

        Task<CommunityPromptLike?> GetByCommunityPromptIdAndUser(int communityPromptId, Guid userIdentifier);
    }
}