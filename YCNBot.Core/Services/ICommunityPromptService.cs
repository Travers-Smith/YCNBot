using YCNBot.Core.Entities;

namespace YCNBot.Core.Services
{
    public interface ICommunityPromptService
    {
        Task Add(CommunityPrompt CommunityPrompt);

        Task<IEnumerable<CommunityPromptWithLikesAndCommentsCount>> Get(int skip, int take, Guid userIdentifier);
    }
}