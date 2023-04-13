using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;

namespace YCNBot.Services
{
    public class OpenAIChatCompletionService : IChatCompletionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OpenAIChatCompletionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> AddChatCompletion(IEnumerable<Message> messages, string model)
        {
            ChatCompletion completedChat = await _unitOfWork.OpenAIChatCompletion.CompleteChat(new AddChatCompletionServiceModel
            {
                Messages = messages.Select(x => new ChatCompletionMessage
                {
                    Content = x.Text,
                    Role = x.IsSystem ? "assistant" : "user"
                }),
                Model = model
            });

            return completedChat.Choices
                .Select(x => x.Message.Content)
                .First();
        }
    }
}
