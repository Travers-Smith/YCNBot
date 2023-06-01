using YCNBot.Core.Entities;

namespace YCNBot.Core.Services
{
    public interface IUserAgreedTermsService
    {
        Task Add(UserAgreedTerm userAgreedTerms);

        Task<bool> CheckAgreed(Guid userIdentifier);

        Task<UserAgreedTerm?> GetByUser(Guid userIdentifier);

        Task Update(UserAgreedTerm userAgreedTerms);
    }
}