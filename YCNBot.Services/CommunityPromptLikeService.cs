using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;

namespace YCNBot.Services
{
    public class CommunityPromptLikeService : ICommunityPromptLikeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommunityPromptLikeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(CommunityPromptLike communityPrompt)
        {
            await _unitOfWork.CommunityPromptLike.AddAsync(communityPrompt);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<CommunityPromptLike>> GetByCommunityPromptId(int communityPromptId, int skip, int take)
        {
            return await _unitOfWork.CommunityPromptLike.GetByCommunityPromptId(communityPromptId, skip, take);
        }

        public async Task<CommunityPromptLike?> GetByCommunityPromptIdAndUser(int communityPromptId, Guid userIdentifier)
        {
            return await _unitOfWork.CommunityPromptLike.GetByCommunityPromptIdAndUser(communityPromptId, userIdentifier);
        }

        public async Task Remove(CommunityPromptLike communityPromptLike)
        {
            _unitOfWork.CommunityPromptLike.Remove(communityPromptLike);

            await _unitOfWork.CommitAsync();
        }
    }
}
