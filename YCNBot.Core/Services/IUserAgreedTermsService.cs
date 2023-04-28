using YCNBot.Core.Entities;

namespace YCNBot.Core.Services
{
    public interface IUserAgreedTermsService
    {
        Task Add(UserAgreedTerms userAgreedTerms);

        Task<bool> CheckAgreed(Guid userIdentifier);

        Task<UserAgreedTerms?> GetByUser(Guid userIdentifier);

        Task Update(UserAgreedTerms userAgreedTerms);
    }
}