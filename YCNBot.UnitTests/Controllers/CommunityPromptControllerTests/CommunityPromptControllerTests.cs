using Microsoft.AspNetCore.Mvc;
using Moq;
using YCNBot.Controllers;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;
using YCNBot.Models;

namespace YCNBot.UnitTest.Controllers.CommunityPromptControllerTests
{
    public class CommunityPromptControllerTests
    {
        [Fact]
        public async Task Add_Successful_Returns201StatusCode()
        {
            var communityPromptService = new Mock<ICommunityPromptService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            IActionResult result = await new CommunityPromptController(communityPromptService.Object, identityService.Object, userService.Object).Add(new AddCommunityPromptModel());

            communityPromptService.Verify(x => x.Add(It.IsAny<CommunityPrompt>()), Times.Once);

            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal(201, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Add_NoUserGuid_ReturnsUnauthorizedResult()
        {
            var communityPromptService = new Mock<ICommunityPromptService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            IActionResult result = await new CommunityPromptController(communityPromptService.Object, identityService.Object, userService.Object).Add(new AddCommunityPromptModel());

            communityPromptService.Verify(x => x.Add(It.IsAny<CommunityPrompt>()), Times.Never);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Add_AddFails_ThrowsException()
        {
            var communityPromptService = new Mock<ICommunityPromptService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            communityPromptService.Setup(x => x.Add(It.IsAny<CommunityPrompt>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new CommunityPromptController(communityPromptService.Object, identityService.Object, userService.Object).Add(new AddCommunityPromptModel()));
        }

        [Fact]
        public async Task GetByCommunityPromptId_Successful_ReturnsCommunityPromtModels()
        {
            var communityPromptService = new Mock<ICommunityPromptService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            Guid userIdentifier = Guid.NewGuid();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            CommunityPromptWithLikesAndCommentsCount[] communityPrompts = new[]
            {
                new CommunityPromptWithLikesAndCommentsCount
                {
                    CommunityPrompt = new CommunityPrompt
                    {
                        UserIdentifier = userIdentifier
                    }
                }
            };

            communityPromptService
                .Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>(), userIdentifier))
                .ReturnsAsync(communityPrompts);

            Dictionary<string, User>? users = new()
            {
                { userIdentifier.ToString(), new User() }
            };

            userService.Setup(x => x.GetUserDetails(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(users);

            IActionResult result = await new CommunityPromptController(communityPromptService.Object, identityService.Object, userService.Object)
                .Get(1);

            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);

            IEnumerable<CommunityPromptModel> communityPromptModels = Assert.IsAssignableFrom<IEnumerable<CommunityPromptModel>>(okObjectResult.Value);

            Assert.Equal(communityPromptModels.Count(), communityPrompts.Length);
        }

        [Fact]
        public async Task Get_FailedToGet_ThrowsException()
        {
            var communityPromptService = new Mock<ICommunityPromptService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            Guid userIdentifier = Guid.NewGuid();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            communityPromptService
             .Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>(), userIdentifier))
             .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new CommunityPromptController(communityPromptService.Object, identityService.Object, userService.Object)
                .Get(1));
        }

        [Fact]
        public async Task GetByCommunityPromptId_FailedToGetUsers_ThrowsException()
        {
            var communityPromptService = new Mock<ICommunityPromptService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            Guid userIdentifier = Guid.NewGuid();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            CommunityPromptWithLikesAndCommentsCount[] communityPrompts = new[]
  {
                new CommunityPromptWithLikesAndCommentsCount()
            };

            communityPromptService
                .Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>(), userIdentifier))
                .ReturnsAsync(communityPrompts);

            IActionResult actionResult = await new CommunityPromptController(communityPromptService.Object, identityService.Object, userService.Object)
                .Get(1);

            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(actionResult);

            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
