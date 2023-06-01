using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;

namespace YCNBot.Services
{
    public class CommunityPromptCommentService : ICommunityPromptCommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommunityPromptCommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(CommunityPromptComment communityPrompt)
        {
            await _unitOfWork.CommunityPromptComment.AddAsync(communityPrompt);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<CommunityPromptComment>> GetByCommunityPromptId(int communityPromptId, int skip, int take)
        {
            return await _unitOfWork.CommunityPromptComment.GetByCommunityPromptId(communityPromptId, skip, take);
        }
    }
}
