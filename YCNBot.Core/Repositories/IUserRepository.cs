using YCNBot.Core.Entities;

namespace YCNBot.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserDetails(Guid id);

        Task<Dictionary<string, Core.Entities.User>?> GetUserDetails(IEnumerable<Guid> ids);
    }
}