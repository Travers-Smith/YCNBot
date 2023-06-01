using YCNBot.Core.Entities;

namespace YCNBot.Core.Services
{
    public interface ICommunityPromptCommentService
    {
        Task Add(CommunityPromptComment communityPrompt);

        Task<IEnumerable<CommunityPromptComment>> GetByCommunityPromptId(int communityPromptId, int skip, int take);
    }
}