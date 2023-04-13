using YCNBot.Core.Entities;

namespace YCNBot.Core.Services
{
    public interface IChatCompletionService
    {
        Task<string> AddChatCompletion(IEnumerable<Message> messages, string model);
    }
}