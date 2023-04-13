using System.Text;
using System.Text.Json;
using YCNBot.Core.Entities;
using YCNBot.Core.Repositories;

namespace YCNBot.Data.Repositories
{
    public class OpenAIChatCompletionRepository : IChatCompletionRepository
    {
        private readonly HttpClient _httpClient;

        public OpenAIChatCompletionRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ChatCompletion> CompleteChat(AddChatCompletionServiceModel addChatCompletion)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(
                "chat/completions",
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
