using Microsoft.AspNetCore.Mvc;
using Moq;
using YCNBot.Controllers;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;
using YCNBot.Models;

namespace YCNBot.UnitTest.Controllers.UserFeedbackControllerTests
{
    public class UserFeedbackControllerTests
    {
        [Fact]
        public async Task Add_Success_Returns201StatusCodeResult()
        {
            Mock<IIdentityService> identityService = new();
            Mock<IUserFeedbackService> userFeedbackService = new();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            IActionResult result = await new UserFeedbackController(identityService.Object, userFeedbackService.Object)
                .Add(new AddUserFeedbackModel());

            userFeedbackService.Verify(x => x.Add(It.IsAny<UserFeedback>()), Times.Once);

            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal(201, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Add_Unauthorized_ReturnsUnauthorizedResult()
        {
            Mock<IIdentityService> identityService = new();
            Mock<IUserFeedbackService> userFeedbackService = new();

            IActionResult result = await new UserFeedbackController(identityService.Object, userFeedbackService.Object)
                .Add(new AddUserFeedbackModel());

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Add_AddFails_ThrowsException()
        {
            Mock<IIdentityService> identityService = new();
            Mock<IUserFeedbackService> userFeedbackService = new();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            userFeedbackService
                .Setup(x => x.Add(It.IsAny<UserFeedback>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new UserFeedbackController(identityService.Object, userFeedbackService.Object)
                .Add(new AddUserFeedbackModel()));
        }
    }
}
