using YCNBot.Core.Entities;

namespace YCNBot.Core.Services
{
    public interface ICommunityPromptLikeService
    {
        Task Add(CommunityPromptLike communityPrompt);

        Task<IEnumerable<CommunityPromptLike>> GetByCommunityPromptId(int communityPromptId, int skip, int take);

        Task<CommunityPromptLike?> GetByCommunityPromptIdAndUser(int communityPromptId, Guid userIdentifier);

        Task Remove(CommunityPromptLike communityPromptLike);
    }
}