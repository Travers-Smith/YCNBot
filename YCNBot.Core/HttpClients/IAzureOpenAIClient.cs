using YCNBot.Core.Entities;

namespace YCNBot.Core.HttpClients
{
    public interface IAzureOpenAiClient
    {
        Task<HttpResponseMessage> AddChatCompletion(AddChatCompletionServiceModel content);
    }
}
