using YCNBot.Core.Services;

namespace YCNBot.Services
{
    public class ChatModelPickerService : IChatModelPickerService
    {
        private readonly IEnumerable<IChatCompletionService> _models;

        public ChatModelPickerService(IEnumerable<IChatCompletionService> models)
        {
            _models = models;
        }

        public IChatCompletionService? GetModel(string model)
        {
            return model switch
            {
                "AzureOpenAI" => _models.FirstOrDefault(x => x.GetType() == typeof(AzureChatCompletionService)),
                "OpenAI" => _models.FirstOrDefault(x => x.GetType() == typeof(OpenAIChatCompletionService)),
                _ => null,
            };
        }
    }
}
