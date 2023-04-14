using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using YCNBot.Core.Entities;
using YCNBot.Core.Repositories;

namespace YCNBot.Data.Repositories
{
    public class AzureChatCompletionRepository : IChatCompletionRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public AzureChatCompletionRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<ChatCompletion> CompleteChat(AddChatCompletionServiceModel addChatCompletion)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(
                "deployments/chatgpt3/chat/completions?api-version=" + _configuration["AzureApiVersion"],
                new StringContent(JsonSerializer.Serialize(addChatCompletion), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            ChatCompletion? chatCompletion = JsonSerializer.Deserialize<ChatCompletion>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (chatCompletion is null)
            {
                throw new Exception("Unable to get chat completion");
            }

            return chatCompletion;

        }
    }
}
