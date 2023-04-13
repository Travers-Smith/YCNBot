using YCNBot.Core.Entities;

namespace YCNBot.Core.HttpClients
{
    public interface IAzureOpenAIClient
    {
        Task<HttpResponseMessage> AddChatCompletion(AddChatCompletionServiceModel content);
    }
}
