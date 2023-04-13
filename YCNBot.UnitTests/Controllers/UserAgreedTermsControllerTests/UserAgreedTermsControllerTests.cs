using Microsoft.AspNetCore.Mvc;
using Moq;
using YCNBot.Controllers;
using YCNBot.Core.Services;
using YCNBot.Entities;

namespace YCNBot.UnitTest.Controllers.UserAgreedTermsControllerTests
{
    public class UserAgreedTermsControllerTests
    {
        [Fact]
        public async Task AcceptTerms_AddsAgreedTerms_ReturnSuccessCode()
        {
            var mockUserAgreedTermsService = new Mock<IUserAgreedTermsService>();
            
            var mockIdentityService = new Mock<IIdentityService>();

            Guid userGuid = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(userGuid);

            mockUserAgreedTermsService.Setup(x => x.GetByUser(It.IsAny<Guid>()));

            IActionResult result = await new UserAgreedTermsController(mockIdentityService.Object, mockUserAgreedTermsService.Object).AcceptTerms();

            Assert.Multiple(() =>
            {
                var statusResult = Assert.IsType<StatusCodeResult>(result);
                Assert.Equal(204, statusResult.StatusCode);
            });
        }

        [Fact]
        public async Task AcceptTerms_AddsAgreedTerms_AddsUserAgreedTerms()
        {
            var mockUserAgreedTermsService = new Mock<IUserAgreedTermsService>();

            var mockIdentityService = new Mock<IIdentityService>();

            Guid userGuid = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(userGuid);

            mockUserAgreedTermsService.Setup(x => x.GetByUser(It.IsAny<Guid>()));

            IActionResult result = await new UserAgreedTermsController(mockIdentityService.Object, mockUserAgreedTermsService.Object).AcceptTerms();

            mockUserAgreedTermsService.Verify(x => x.Add(It.IsAny<UserAgreedTerms>()), Times.Once);
        }

        [Fact]
        public async Task AcceptTerms_UpdatesAgreedTerms_ReturnsSuccess()
        {
            var mockUserAgreedTermsService = new Mock<IUserAgreedTermsService>();

            var mockIdentityService = new Mock<IIdentityService>();

            Guid userGuid = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(userGuid);

            mockUserAgreedTermsService.Setup(x => x.GetByUser(It.IsAny<Guid>())).ReturnsAsync(new UserAgreedTerms
            {
                Agreed = true
            });

            IActionResult result = await new UserAgreedTermsController(mockIdentityService.Object, mockUserAgreedTermsService.Object).AcceptTerms();

            Assert.Multiple(() =>
            {
                var statusResult = Assert.IsType<StatusCodeResult>(result);
                Assert.Equal(204, statusResult.StatusCode);
            });
        }

        [Fact]
        public async Task AcceptTerms_UpdatesAgreedTerms_Updates()
        {
            var mockUserAgreedTermsService = new Mock<IUserAgreedTermsService>();

            var mockIdentityService = new Mock<IIdentityService>();

            Guid userGuid = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(userGuid);

            mockUserAgreedTermsService.Setup(x => x.GetByUser(It.IsAny<Guid>())).ReturnsAsync(new UserAgreedTerms
            {
                Agreed = true
            });

            IActionResult result = await new UserAgreedTermsController(mockIdentityService.Object, mockUserAgreedTermsService.Object).AcceptTerms();

            mockUserAgreedTermsService.Verify(x => x.Update(It.IsAny<UserAgreedTerms>()), Times.Once);
        }
    }
}
