using Moq;
using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Services;

namespace YCNBot.UnitTest.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUsers_Success_ReturnsListOfUsers()
        {
            Mock<IUnitOfWork> unitOfWork = new ();

            IEnumerable<Guid> userIdentifiers = new Guid[]
            {
                Guid.NewGuid(),
            };

            unitOfWork
                .Setup(uow => uow.User.GetUserDetails(userIdentifiers))
                .ReturnsAsync(new List<User>()
                {
                    new User()
                });

            IEnumerable<User>? users = await new UserService(unitOfWork.Object).GetUsers(userIdentifiers);

            Assert.IsAssignableFrom<IEnumerable<User>>(users);
            Assert.NotNull(users);
            Assert.True(users.Count() == 1);
        }

        [Fact]
        public async Task GetUsers_Failure_ThrowsException()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            IEnumerable<Guid> userIdentifiers = new Guid[]
            {
                Guid.NewGuid(),
            };

            unitOfWork
                .Setup(uow => uow.User.GetUserDetails(userIdentifiers))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new UserService(unitOfWork.Object).GetUsers(userIdentifiers));
        }

        [Fact]
        public async Task GetUsers_NullResponse_ReturnsNull()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            IEnumerable<Guid> userIdentifiers = new Guid[]
            {
                Guid.NewGuid(),
            };

            IEnumerable<User>? users = null;

            unitOfWork
                .Setup(x => x.User.GetUserDetails(userIdentifiers))
                .ReturnsAsync(users);

            Assert.Null(await new UserService(unitOfWork.Object).GetUsers(userIdentifiers));
        }
    }
}
