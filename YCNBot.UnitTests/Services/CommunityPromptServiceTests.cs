using Moq;
using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Services;

namespace YCNBot.UnitTest.Services
{
    public class CommunityPromptServiceTests
    {

        [Fact]
        public async Task Add_Successful_Saved()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.CommunityPrompt.AddAsync(It.IsAny<CommunityPrompt>()));

            await new CommunityPromptService(mock.Object).Add(new CommunityPrompt());

            mock.Verify(p => p.CommunityPrompt.AddAsync(It.IsAny<CommunityPrompt>()), Times.Once);
            mock.Verify(p => p.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Add_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.CommunityPrompt.AddAsync(It.IsAny<CommunityPrompt>()));

            mock.Setup(x => x.CommitAsync()).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new CommunityPromptService(mock.Object).Add(new CommunityPrompt()));
        }
    }
}
