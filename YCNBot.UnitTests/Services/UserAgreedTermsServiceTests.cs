using Moq;
using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Services;

namespace YCNBot.UnitTest.Services
{
    public class UserAgreedTermsServiceTests
    {
        [Fact]
        public async Task Add_Successful_Saved()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.UserAgreedTerms.AddAsync(It.IsAny<UserAgreedTerms>()));

            await new UserAgreedTermsService(mock.Object).Add(new UserAgreedTerms());

            mock.Verify(p => p.UserAgreedTerms.AddAsync(It.IsAny<UserAgreedTerms>()), Times.Once);
            mock.Verify(p => p.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Add_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.UserAgreedTerms.AddAsync(It.IsAny<UserAgreedTerms>()));

            mock.Setup(x => x.CommitAsync()).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new UserAgreedTermsService(mock.Object).Add(new UserAgreedTerms()));
        }

        [Fact]
        public async Task GetByUser_Success_ReturnsUserAgreedTerms()
        {
            var mock = new Mock<IUnitOfWork>();

            var userAgreedTerms = new UserAgreedTerms();

            mock.Setup(x => x.UserAgreedTerms.GetByUser(It.IsAny<Guid>())).ReturnsAsync(userAgreedTerms);

            UserAgreedTerms? result = await new UserAgreedTermsService(mock.Object).GetByUser(Guid.NewGuid());

            Assert.Equal(userAgreedTerms, result);
        }

        [Fact]
        public async Task GetByUser_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.UserAgreedTerms.GetByUser(It.IsAny<Guid>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new UserAgreedTermsService(mock.Object).GetByUser(Guid.NewGuid()));
        }

        [Fact]
        public async Task Update_Successful_Saved()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.UserAgreedTerms.AddAsync(It.IsAny<UserAgreedTerms>()));

            await new UserAgreedTermsService(mock.Object).Add(new UserAgreedTerms());

            mock.Verify(p => p.UserAgreedTerms.AddAsync(It.IsAny<UserAgreedTerms>()), Times.Once);
            mock.Verify(p => p.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.UserAgreedTerms.Update(It.IsAny<UserAgreedTerms>()));

            mock.Setup(x => x.CommitAsync()).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new UserAgreedTermsService(mock.Object).Update(new UserAgreedTerms()));
        }
    }
}

