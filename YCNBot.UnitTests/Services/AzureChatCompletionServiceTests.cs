using Moq;
using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Services;

namespace YCNBot.UnitTest.Services
{
    public class AzureChatCompletionServiceTests
    {
        [Fact]
        public async Task CompleteChat_Successful_ReturnsMessage()
        {
            var mock = new Mock<IUnitOfWork>();

            string completedMessage = "completed message";

            ChatCompletion chatCompletion = new()
            {
                Choices = new List<ChatCompletionChoice>
                {
                    new ChatCompletionChoice
                    {
                        Message = new ChatCompletionChoiceMessage
                        {
                            Content = completedMessage,
                            Role = "System"
                        }
                    }
                }
            };

            mock
                .Setup(x => x.AzureOpenAIChatCompletion.CompleteChat(It.IsAny<AddChatCompletionServiceModel>()))
                .ReturnsAsync(chatCompletion);

            string result = await new AzureChatCompletionService(mock.Object).AddChatCompletion(new List<Message>(), "modelname");

            Assert.Equal(result, completedMessage);
        }

        [Fact]
        public async Task CompleteChat_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            mock
                .Setup(x => x.AzureOpenAIChatCompletion.CompleteChat(It.IsAny<AddChatCompletionServiceModel>()))
                .Throws(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new AzureChatCompletionService(mock.Object).AddChatCompletion(new List<Message>(), "modelname"));
        }
    }
}
