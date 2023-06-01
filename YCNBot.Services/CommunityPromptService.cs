using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;

namespace YCNBot.Services
{
    public class CommunityPromptService : ICommunityPromptService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommunityPromptService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(CommunityPrompt CommunityPrompt)
        {
            await _unitOfWork.CommunityPrompt.AddAsync(CommunityPrompt);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<CommunityPromptWithLikesAndCommentsCount>> Get(int skip, int take, Guid userIdentifier)
        {
            return await _unitOfWork.CommunityPrompt.Get(skip, take, userIdentifier);
        }
    }
}
