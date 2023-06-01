using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;

namespace YCNBot.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User?> GetUser(Guid uniqueIdentifier)
        {
            return await _unitOfWork.User.GetUserDetails(uniqueIdentifier);
        }

        public async Task<Dictionary<string, User>?> GetUserDetails(IEnumerable<Guid> ids)
        {
            return await _unitOfWork.User.GetUserDetails(ids);
        }
    }
}
