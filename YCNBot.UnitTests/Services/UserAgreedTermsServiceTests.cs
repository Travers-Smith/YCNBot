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

            mock.Setup(x => x.UserAgreedTerms.AddAsync(It.IsAny<UserAgreedTerm>()));

            await new UserAgreedTermsService(mock.Object).Add(new UserAgreedTerm());

            mock.Verify(p => p.UserAgreedTerms.AddAsync(It.IsAny<UserAgreedTerm>()), Times.Once);
            mock.Verify(p => p.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Add_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.UserAgreedTerms.AddAsync(It.IsAny<UserAgreedTerm>()));

            mock.Setup(x => x.CommitAsync()).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new UserAgreedTermsService(mock.Object).Add(new UserAgreedTerm()));
        }

        [Fact]
        public async Task GetByUser_Success_ReturnsUserAgreedTerm()
        {
            var mock = new Mock<IUnitOfWork>();

            var UserAgreedTerm = new UserAgreedTerm();

            mock.Setup(x => x.UserAgreedTerms.GetByUser(It.IsAny<Guid>())).ReturnsAsync(UserAgreedTerm);

            UserAgreedTerm? result = await new UserAgreedTermsService(mock.Object).GetByUser(Guid.NewGuid());

            Assert.Equal(UserAgreedTerm, result);
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

            mock.Setup(x => x.UserAgreedTerms.AddAsync(It.IsAny<UserAgreedTerm>()));

            await new UserAgreedTermsService(mock.Object).Add(new UserAgreedTerm());

            mock.Verify(p => p.UserAgreedTerms.AddAsync(It.IsAny<UserAgreedTerm>()), Times.Once);
            mock.Verify(p => p.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.UserAgreedTerms.Update(It.IsAny<UserAgreedTerm>()));

            mock.Setup(x => x.CommitAsync()).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new UserAgreedTermsService(mock.Object).Update(new UserAgreedTerm()));
        }
    }
}

