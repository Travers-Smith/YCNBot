using System.Text.Json.Serialization;

namespace YCNBot.Core.Entities
{
    public class AddChatCompletionServiceModel
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("messages")]
        public IEnumerable<ChatCompletionMessage> Messages { get; set; }
    }
}
