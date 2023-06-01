using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;

namespace YCNBot.Services
{
    public class UserAgreedTermsService : IUserAgreedTermsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserAgreedTermsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(UserAgreedTerm userAgreedTerms)
        {
            await _unitOfWork.UserAgreedTerms.AddAsync(userAgreedTerms);

            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> CheckAgreed(Guid userIdentifier)
        {
            return (await _unitOfWork.UserAgreedTerms.GetByUser(userIdentifier))?.Agreed == true;
        }

        public async Task<UserAgreedTerm?> GetByUser(Guid userIdentifier)
        {
            return await _unitOfWork.UserAgreedTerms.GetByUser(userIdentifier);
        }

        public async Task Update(UserAgreedTerm userAgreedTerms)
        {
            _unitOfWork.UserAgreedTerms.Update(userAgreedTerms);

            await _unitOfWork.CommitAsync();
        }
    }
}
