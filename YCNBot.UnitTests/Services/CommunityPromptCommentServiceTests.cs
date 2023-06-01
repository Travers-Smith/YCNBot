using Moq;
using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Services;

namespace YCNBot.UnitTest.Services
{
    public class CommunityPromptCommentServiceTests
    {
        [Fact]
        public async Task Add_Successful_Saved()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.CommunityPromptComment.AddAsync(It.IsAny<CommunityPromptComment>()));

            await new CommunityPromptCommentService(mock.Object).Add(new CommunityPromptComment());

            mock.Verify(p => p.CommunityPromptComment.AddAsync(It.IsAny<CommunityPromptComment>()), Times.Once);
            mock.Verify(p => p.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Add_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            mock
                .Setup(x => x.CommunityPromptComment.AddAsync(It.IsAny<CommunityPromptComment>()));

            mock.Setup(x => x.CommitAsync()).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new CommunityPromptCommentService(mock.Object).Add(new CommunityPromptComment()));
        }

        [Fact]
        public async Task GetByCommunityPromptId_Successful_ReturnsIEnumerableCommunityPromptLikes()
        {
            var mock = new Mock<IUnitOfWork>();

            IEnumerable<CommunityPromptComment> communityPromptComments = new CommunityPromptComment[]
            {
                new CommunityPromptComment()
            };

            mock
                .Setup(x => x.CommunityPromptComment.GetByCommunityPromptId(1, 0, 10))
                .ReturnsAsync(communityPromptComments);

            IEnumerable<CommunityPromptComment> likes = await new CommunityPromptCommentService(mock.Object).GetByCommunityPromptId(1, 0, 10);

            Assert.Equal(likes, communityPromptComments);
        }

        [Fact]
        public async Task GetByCommunityPromptId_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            mock
                .Setup(x => x.CommunityPromptComment.GetByCommunityPromptId(1, 0, 10))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new CommunityPromptCommentService(mock.Object).GetByCommunityPromptId(1, 0, 10));
        }
    }
}
