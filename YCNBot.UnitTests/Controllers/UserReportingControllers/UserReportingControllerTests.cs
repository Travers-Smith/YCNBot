
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using YCNBot.Controllers;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;
using YCNBot.Models;

namespace YCNBot.UnitTest.Controllers.UserReportingControllers
{
    public class UserReportingControllerTests
    {
        [Fact]
        public async Task GetActiveUsers_Success_ReturnsUserUsageList()
        {
            Mock<IChatService> chatService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();
            Mock<IUserService> userService = new();

            Guid userGuid = Guid.NewGuid();

            chatService
                .Setup(x => x.GetUsersUsage(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Dictionary<Guid, int>
                {
                    { userGuid, 10 }
                });

            userService
                .Setup(x => x.GetUserDetails(It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new Dictionary<string, User>
                {
                    {
                        "userIdentifier",
                        new User
                        {
                            Id = userGuid
                        }
                    }
                });

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());


            IActionResult result = await new UserReportingController(chatService.Object,
                configuration.Object,
                identityService.Object,
                userService.Object).GetActiveUsers(1);

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);

            IEnumerable<UserUsageModel> userUsage = Assert.IsAssignableFrom<IEnumerable<UserUsageModel>>(okResult.Value);

            Assert.Single(userUsage);
        }

        [Fact]
        public async Task GetActiveUsers_Unauthorized_ReturnsUnauthorizedResult()
        {
            Mock<IChatService> chatService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();
            Mock<IUserService> userService = new();

            chatService
                .Setup(x => x.GetUsersUsage(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Dictionary<Guid, int>
                {
                    { Guid.NewGuid(), 10 }
                });

            IActionResult result = await new UserReportingController(chatService.Object,
                configuration.Object,
                identityService.Object,
                userService.Object).GetActiveUsers(1);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetActiveUsers_NullUsers_ReturnsStatusCodeResult500()
        {
            Mock<IChatService> chatService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();
            Mock<IUserService> userService = new();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());


            Guid userGuid = Guid.NewGuid();
            chatService
                .Setup(x => x.GetUsersUsage(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Dictionary<Guid, int>
                {
                    { userGuid, 10 }
                });

            Dictionary<string, User>? users = null;

            userService
                .Setup(x => x.GetUserDetails(It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(users);

            IActionResult result = await new UserReportingController(chatService.Object,
                configuration.Object,
                identityService.Object,
                userService.Object).GetActiveUsers(1);

            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetActiveUsers_FailedToGetUserUsage_ThrowsException()
        {
            Mock<IChatService> chatService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();
            Mock<IUserService> userService = new();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            chatService
                .Setup(x => x.GetUsersUsage(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new UserReportingController(chatService.Object,
                configuration.Object,
                identityService.Object,
                userService.Object).GetActiveUsers(1));
        }

        [Fact]
        public async Task GetActiveUsers_FailedToGetUsers_ThrowsException()
        {
            Mock<IChatService> chatService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();
            Mock<IUserService> userService = new();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            chatService
                .Setup(x => x.GetUsersUsage(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Dictionary<Guid, int>
                {
                    { Guid.NewGuid(), 10 }
                });

            userService
                .Setup(x => x.GetUserDetails(It.IsAny<IEnumerable<Guid>>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new UserReportingController(chatService.Object,
                configuration.Object,
                identityService.Object,
                userService.Object).GetActiveUsers(1));
        }
    }
}
