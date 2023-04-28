using YCNBot.Core.Entities;

namespace YCNBot.Core.Repositories
{
    public interface IUserAgreedTermsRepository : IRepository<UserAgreedTerms>
    {
        Task<UserAgreedTerms?> GetByUser(Guid userIdentifier);
    }
}