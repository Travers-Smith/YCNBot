using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;

namespace YCNBot.Services
{
    public class UserFeedbackService : IUserFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserFeedbackService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(UserFeedback userFeedback)
        {
            await _unitOfWork.UserFeedback.AddAsync(userFeedback);

            await _unitOfWork.CommitAsync();
        }
    }
}
