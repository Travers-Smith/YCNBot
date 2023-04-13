using YCNBot.Core.Entities;

namespace YCNBot.Core.HttpClients
{
    public interface IOpenAIClient
    {
        Task<HttpResponseMessage> AddChatCompletion(AddChatCompletionServiceModel content);
    }
}
