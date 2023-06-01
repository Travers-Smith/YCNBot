using YCNBot.Core.Entities;

namespace YCNBot.Core.Repositories
{
    public interface ICommunityPromptCommentRepository : IRepository<CommunityPromptComment>
    {
        Task<IEnumerable<CommunityPromptComment>> GetByCommunityPromptId(int communityPromptId, int skip, int take);
    }
}