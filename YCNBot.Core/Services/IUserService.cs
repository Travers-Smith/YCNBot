using YCNBot.Core.Entities;

namespace YCNBot.Core.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>?> GetUsers(IEnumerable<Guid> ids);
    }
}