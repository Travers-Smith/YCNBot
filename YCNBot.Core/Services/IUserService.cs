using YCNBot.Core.Entities;

namespace YCNBot.Core.Services
{
    public interface IUserService
    {
        Task<User?> GetUser(Guid uniqueIdentifier);

        Task<Dictionary<string, User>?> GetUserDetails(IEnumerable<Guid> ids);
    }
}