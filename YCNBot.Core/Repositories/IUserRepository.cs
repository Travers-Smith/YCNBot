using YCNBot.Core.Entities;

namespace YCNBot.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserDetails(Guid id);

        Task<IEnumerable<User>?> GetUserDetails(IEnumerable<Guid> ids);
    }
}