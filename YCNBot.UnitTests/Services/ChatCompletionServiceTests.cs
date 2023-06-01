using Moq;
using YCNBot.Core;
using YCNBot.Core.Services;
using YCNBot.Services;

namespace YCNBot.UnitTest.Services
{
    public class ChatCompletionServiceTests
    {
        [Fact]
        public void GetModel_OpenAI_ReturnsIChatService()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            Assert.IsType<OpenAIChatCompletionService>(new ChatModelPickerService(new List<IChatCompletionService>() {
                new OpenAIChatCompletionService(unitOfWork.Object)
            })
                .GetModel("OpenAI"));
        }

        [Fact]
        public void GetModel_Azure_ReturnsIChatService()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            Assert.IsType<AzureChatCompletionService>(new ChatModelPickerService(new List<IChatCompletionService>() {
                new AzureChatCompletionService(unitOfWork.Object)
            })
                .GetModel("AzureOpenAI"));
        }

        [Fact]
        public void GetModel_NoModel_ReturnsNull()
        {
            Assert.Null(new ChatModelPickerService(new List<IChatCompletionService>() { }).GetModel("test"));
        }
    }
}
