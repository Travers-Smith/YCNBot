using Microsoft.AspNetCore.Mvc;
using Moq;
using YCNBot.Controllers;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;
using YCNBot.Models;

namespace YCNBot.UnitTest.Controllers.CommunityPromptControllerTests
{
    public class CommunityPromptLikeControllerTests
    {
        [Fact]
        public async Task AddOrRemove_Adds_Returns201StatusCode()
        {
            var communityPromptLikeService = new Mock<ICommunityPromptLikeService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            Guid userIdentifier = Guid.NewGuid();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            IActionResult result = await new CommunityPromptLikeController(communityPromptLikeService.Object, identityService.Object, userService.Object)
                .AddOrRemove(new AddCommunityPromptLikeModel { Liked = true });

            communityPromptLikeService.Verify(x => x.Add(It.IsAny<CommunityPromptLike>()), Times.Once);
            communityPromptLikeService.Verify(x => x.Remove(It.IsAny<CommunityPromptLike>()), Times.Never);
            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal(204, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task AddOrRemove_Removes_Returns201StatusCode()
        {
            var communityPromptLikeService = new Mock<ICommunityPromptLikeService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            Guid userIdentifier = Guid.NewGuid();

            communityPromptLikeService
                .Setup(x => x.GetByCommunityPromptIdAndUser(1, userIdentifier))
                .ReturnsAsync(new CommunityPromptLike());

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            IActionResult result = await new CommunityPromptLikeController(communityPromptLikeService.Object, identityService.Object, userService.Object)
                .AddOrRemove(new AddCommunityPromptLikeModel { Liked = false, CommunityPromptId = 1 });

            communityPromptLikeService.Verify(x => x.Add(It.IsAny<CommunityPromptLike>()), Times.Never);
            communityPromptLikeService.Verify(x => x.Remove(It.IsAny<CommunityPromptLike>()), Times.Once);

            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal(204, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task AddOrRemove_NoUserGuid_ReturnsUnauthorizedResult()
        {
            var communityPromptLikeService = new Mock<ICommunityPromptLikeService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            IActionResult result = await new CommunityPromptLikeController(communityPromptLikeService.Object, identityService.Object, userService.Object)
                .AddOrRemove(new AddCommunityPromptLikeModel());

            communityPromptLikeService.Verify(x => x.Add(It.IsAny<CommunityPromptLike>()), Times.Never);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task AddOrRemove_AddFails_ThrowsException()
        {
            var communityPromptLikeService = new Mock<ICommunityPromptLikeService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            communityPromptLikeService
                .Setup(x => x.Add(It.IsAny<CommunityPromptLike>()))
                .ThrowsAsync(new Exception());

            communityPromptLikeService.Setup(x => x.Add(It.IsAny<CommunityPromptLike>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new CommunityPromptLikeController(communityPromptLikeService.Object, identityService.Object, userService.Object)
                .AddOrRemove(new AddCommunityPromptLikeModel
                {
                    Liked = true
                }));
        }

        [Fact]
        public async Task AddOrRemove_RemoveFails_ThrowsException()
        {
            var communityPromptLikeService = new Mock<ICommunityPromptLikeService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            Guid userIdentifier = new Guid();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            communityPromptLikeService
                .Setup(x => x.GetByCommunityPromptIdAndUser(1, userIdentifier))
                .ReturnsAsync(new CommunityPromptLike());

            communityPromptLikeService
                .Setup(x => x.Remove(It.IsAny<CommunityPromptLike>()))
                .ThrowsAsync(new Exception());

            communityPromptLikeService.Setup(x => x.Remove(It.IsAny<CommunityPromptLike>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new CommunityPromptLikeController(communityPromptLikeService.Object, identityService.Object, userService.Object)
                .AddOrRemove(new AddCommunityPromptLikeModel
                {
                    Liked = false,
                    CommunityPromptId = 1
                }));
        }

        [Fact]
        public async Task GetByCommunityPromptId_Successful_ReturnsLikedUsers()
        {
            var communityPromptLikeService = new Mock<ICommunityPromptLikeService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            Guid userIdentifier = Guid.NewGuid();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            communityPromptLikeService.Setup(x => x.GetByCommunityPromptId(1, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new CommunityPromptLike[]
                {
                    new CommunityPromptLike
                    {
                        UserIdentifier = Guid.NewGuid()
                    }
                });

            Dictionary<string, User>? users = new()
            {
                { "uniqueIdentifier", new User() }
            };

            userService.Setup(x => x.GetUserDetails(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(users);

            IActionResult result = await new CommunityPromptLikeController(communityPromptLikeService.Object, identityService.Object, userService.Object)
                .GetByCommunityPromptId(1, 1);

            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);

            IEnumerable<UserModel> expectedUsers = Assert.IsAssignableFrom<IEnumerable<UserModel>>(okObjectResult.Value);

            Assert.Equal(users.Count, expectedUsers.Count());
        }

        [Fact]
        public async Task GetByCommunityPromptId_FailedToGetLikes_ThrowsException()
        {
            var communityPromptLikeService = new Mock<ICommunityPromptLikeService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            Guid userIdentifier = Guid.NewGuid();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            communityPromptLikeService
             .Setup(x => x.GetByCommunityPromptId(1, It.IsAny<int>(), It.IsAny<int>()))
             .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new CommunityPromptLikeController(communityPromptLikeService.Object, identityService.Object, userService.Object)
            .GetByCommunityPromptId(1, 1));
        }

        [Fact]
        public async Task GetByCommunityPromptId_FailedToGetUsers_ThrowsException()
        {
            var communityPromptLikeService = new Mock<ICommunityPromptLikeService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            Guid userIdentifier = Guid.NewGuid();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            communityPromptLikeService.Setup(x => x.GetByCommunityPromptId(1, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new CommunityPromptLike[]
                {
                    new CommunityPromptLike
                    {
                        UserIdentifier = Guid.NewGuid()
                    }
                });

            IActionResult actionResult = await new CommunityPromptLikeController(communityPromptLikeService.Object, identityService.Object, userService.Object)
                .GetByCommunityPromptId(1, 1);

            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(actionResult);

            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
