using YCNBot.Entities;

namespace YCNBot.Core.Repositories
{
    public interface IUserAgreedTermsRepository : IRepository<UserAgreedTerms>
    {
        Task<UserAgreedTerms?> GetByUser(Guid userIdentifier);
    }
}