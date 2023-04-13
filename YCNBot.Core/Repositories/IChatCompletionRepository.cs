using YCNBot.Core.Entities;

namespace YCNBot.Core.Repositories
{
    public interface IChatCompletionRepository
    {
        Task<ChatCompletion> CompleteChat(AddChatCompletionServiceModel addChatCompletion);
    }
}