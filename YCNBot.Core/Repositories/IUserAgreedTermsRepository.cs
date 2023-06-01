using YCNBot.Core.Entities;

namespace YCNBot.Core.Repositories
{
    public interface IUserAgreedTermsRepository : IRepository<UserAgreedTerm>
    {
        Task<UserAgreedTerm?> GetByUser(Guid userIdentifier);
    }
}