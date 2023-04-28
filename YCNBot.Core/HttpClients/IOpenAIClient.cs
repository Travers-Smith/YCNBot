using YCNBot.Core.Entities;

namespace YCNBot.Core.HttpClients
{
    public interface IOpenAiClient
    {
        Task<HttpResponseMessage> AddChatCompletion(AddChatCompletionServiceModel content);
    }
}
