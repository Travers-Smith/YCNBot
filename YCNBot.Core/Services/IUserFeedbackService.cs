using YCNBot.Core.Entities;

namespace YCNBot.Core.Services
{
    public interface IUserFeedbackService
    {
        Task Add(UserFeedback userFeedback);
    }
}