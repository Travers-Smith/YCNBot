using Microsoft.AspNetCore.Mvc;
using Moq;
using YCNBot.Controllers;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;
using YCNBot.Models;

namespace YCNBot.UnitTest.Controllers.CommunityPromptCommentControllerTests
{
    public class CommunityPromptCommentControllerTests
    {
        [Fact]
        public async Task Add_Successful_Returns201StatusCode()
        {
            var communityPromptCommentService = new Mock<ICommunityPromptCommentService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            IActionResult result = await new CommunityPromptCommentController(communityPromptCommentService.Object, identityService.Object, userService.Object)
                .Add(new AddCommunityPromptCommentModel());

            communityPromptCommentService.Verify(x => x.Add(It.IsAny<CommunityPromptComment>()), Times.Once);

            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal(201, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Add_NoUserGuid_ReturnsUnauthorizedResult()
        {
            var communityPromptCommentService = new Mock<ICommunityPromptCommentService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            IActionResult result = await new CommunityPromptCommentController(communityPromptCommentService.Object, identityService.Object, userService.Object)
                .Add(new AddCommunityPromptCommentModel());

            communityPromptCommentService.Verify(x => x.Add(It.IsAny<CommunityPromptComment>()), Times.Never);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Add_AddFails_ThrowsException()
        {
            var communityPromptCommentService = new Mock<ICommunityPromptCommentService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            communityPromptCommentService.Setup(x => x.Add(It.IsAny<CommunityPromptComment>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new CommunityPromptCommentController(communityPromptCommentService.Object, identityService.Object, userService.Object)
            .Add(new AddCommunityPromptCommentModel()));
        }

        [Fact]
        public async Task GetByCommunityPromptId_Successful_ReturnsComments()
        {
            var communityPromptCommentService = new Mock<ICommunityPromptCommentService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            Guid userIdentifier = Guid.NewGuid();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            CommunityPromptComment[] comments = new CommunityPromptComment[]
            {
                new CommunityPromptComment
                {
                    UserIdentifier = userIdentifier
                }
            };

            communityPromptCommentService
                .Setup(x => x.GetByCommunityPromptId(1, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(comments);

            Dictionary<string, User>? users = new()
            {
                { userIdentifier.ToString(), new User() }
            };

            userService.Setup(x => x.GetUserDetails(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(users);

            IActionResult result = await new CommunityPromptCommentController(communityPromptCommentService.Object, identityService.Object, userService.Object)
                .GetByCommunityPromptId(1, 1);

            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);

            IEnumerable<CommunityPromptCommentModel> commentModels = Assert.IsAssignableFrom<IEnumerable<CommunityPromptCommentModel>>(okObjectResult.Value);

            Assert.Equal(commentModels.Count(), comments.Length);
        }

        [Fact]
        public async Task GetByCommunityPromptId_FailedToGetComments_ThrowsException()
        {
            var communityPromptCommentService = new Mock<ICommunityPromptCommentService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            Guid userIdentifier = Guid.NewGuid();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            communityPromptCommentService
             .Setup(x => x.GetByCommunityPromptId(1, It.IsAny<int>(), It.IsAny<int>()))
             .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new CommunityPromptCommentController(communityPromptCommentService.Object, identityService.Object, userService.Object)
            .GetByCommunityPromptId(1, 1));
        }

        [Fact]
        public async Task GetByCommunityPromptId_FailedToGetUsers_ThrowsException()
        {
            var communityPromptCommentService = new Mock<ICommunityPromptCommentService>();
            var identityService = new Mock<IIdentityService>();
            var userService = new Mock<IUserService>();

            Guid userIdentifier = Guid.NewGuid();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            communityPromptCommentService.Setup(x => x.GetByCommunityPromptId(1, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new CommunityPromptComment[]
                {
                    new CommunityPromptComment
                    {
                        UserIdentifier = Guid.NewGuid()
                    }
                });

            IActionResult actionResult = await new CommunityPromptCommentController(communityPromptCommentService.Object, identityService.Object, userService.Object)
                .GetByCommunityPromptId(1, 1);

            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(actionResult);

            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
