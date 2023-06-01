using Moq;
using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Services;

namespace YCNBot.UnitTest.Services
{
    public class CommunityPromptLikeServiceTests
    {
        [Fact]
        public async Task Add_Successful_Saved()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.CommunityPromptLike.AddAsync(It.IsAny<CommunityPromptLike>()));

            await new CommunityPromptLikeService(mock.Object).Add(new CommunityPromptLike());

            mock.Verify(p => p.CommunityPromptLike.AddAsync(It.IsAny<CommunityPromptLike>()), Times.Once);
            mock.Verify(p => p.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Add_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.CommunityPromptLike.AddAsync(It.IsAny<CommunityPromptLike>()));

            mock.Setup(x => x.CommitAsync()).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new CommunityPromptLikeService(mock.Object).Add(new CommunityPromptLike()));
        }

        [Fact]
        public async Task GetByCommunityPromptId_Successful_ReturnsIEnumerableCommunityPromptLikes()
        {
            var mock = new Mock<IUnitOfWork>();

            IEnumerable<CommunityPromptLike> communityPromptLikes = new CommunityPromptLike[]
            {
                new CommunityPromptLike()
            };

            mock
                .Setup(x => x.CommunityPromptLike.GetByCommunityPromptId(1, 0, 10))
                .ReturnsAsync(communityPromptLikes);

            IEnumerable<CommunityPromptLike> likes = await new CommunityPromptLikeService(mock.Object).GetByCommunityPromptId(1, 0, 10);

            Assert.Equal(likes, communityPromptLikes);
        }

        [Fact]
        public async Task GetByCommunityPromptId_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            mock
                .Setup(x => x.CommunityPromptLike.GetByCommunityPromptId(1, 0, 10))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new CommunityPromptLikeService(mock.Object).GetByCommunityPromptId(1, 0, 10));
        }

        [Fact]
        public async Task GetByCommunityPromptIdAndUser_Successful_ReturnsCommunityPromptLike()
        {
            var mock = new Mock<IUnitOfWork>();

            Guid userGuid = Guid.NewGuid();

            CommunityPromptLike communityPromptLike = new();

            mock
                .Setup(x => x.CommunityPromptLike.GetByCommunityPromptIdAndUser(1, userGuid))
                .ReturnsAsync(communityPromptLike);

            CommunityPromptLike? like = await new CommunityPromptLikeService(mock.Object).GetByCommunityPromptIdAndUser(1, userGuid);

            Assert.Equal(like, communityPromptLike);
        }

        [Fact]
        public async Task GetByCommunityPromptIdAndUser_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            Guid userGuid = Guid.NewGuid();

            mock
                .Setup(x => x.CommunityPromptLike.GetByCommunityPromptIdAndUser(1, userGuid))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new CommunityPromptLikeService(mock.Object).GetByCommunityPromptIdAndUser(1, userGuid));
        }

        [Fact]
        public async Task Remove_Successful_Saved()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.CommunityPromptLike.Remove(It.IsAny<CommunityPromptLike>()));

            await new CommunityPromptLikeService(mock.Object).Remove(new CommunityPromptLike());

            mock.Verify(p => p.CommunityPromptLike.Remove(It.IsAny<CommunityPromptLike>()), Times.Once);
            mock.Verify(p => p.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Remove_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.CommunityPromptLike.Remove(It.IsAny<CommunityPromptLike>()));

            mock.Setup(x => x.CommitAsync()).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new CommunityPromptLikeService(mock.Object).Remove(new CommunityPromptLike()));
        }
    }
}
