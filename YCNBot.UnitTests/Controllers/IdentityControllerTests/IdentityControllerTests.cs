using Microsoft.AspNetCore.Mvc;
using Moq;
using YCNBot.Core.Services;
using YCNBot.Models;
using YCNBot.Controllers;
using YCNBot.Core.Entities;

namespace YCNBot.UnitTest.Controllers.IdentityControllerTests
{
    public class IdentityControllerTests
    {
        [Fact]
        public async Task GetUser_SuccessAgreedToTerm_ReturnsUserWithAgreed()
        {
            var mockIdentityService = new Mock<IIdentityService>();

            Guid user = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(user);
            mockIdentityService.Setup(x => x.GetName()).Returns("Test username");

            var mockUserAgreedTermsService = new Mock<IUserAgreedTermsService>();

            mockUserAgreedTermsService.Setup(x => x.CheckAgreed(user)).ReturnsAsync(true);

            IActionResult result = await new IdentityController(mockIdentityService.Object, mockUserAgreedTermsService.Object).GetUser();

            Assert.Multiple(() =>
            {
                var actionResult = Assert.IsType<OkObjectResult>(result);

                var user = Assert.IsType<UserModel>(actionResult.Value);

                Assert.True(user.AgreedToTerms);
            });
        }

        [Fact]
        public async Task GetUser_NotAgreedToTerm_ReturnsUserWithNotAgreed()
        {
            var mockIdentityService = new Mock<IIdentityService>();

            Guid user = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(user);
            mockIdentityService.Setup(x => x.GetName()).Returns("Test username");

            var mockUserAgreedTermsService = new Mock<IUserAgreedTermsService>();

            mockUserAgreedTermsService.Setup(x => x.CheckAgreed(user)).ReturnsAsync(false);


            IActionResult result = await new IdentityController(mockIdentityService.Object, mockUserAgreedTermsService.Object).GetUser();

            Assert.Multiple(() =>
            {
                var actionResult = Assert.IsType<OkObjectResult>(result);

                var user = Assert.IsType<UserModel>(actionResult.Value);

                Assert.False(user.AgreedToTerms);
            });
        }

        [Fact]
        public async Task GetUser_UsernameNotFound_ReturnsNotFound()
        {
            var mockIdentityService = new Mock<IIdentityService>();

            Guid user = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(user);

            var mockUserAgreedTermsService = new Mock<IUserAgreedTermsService>();

            mockUserAgreedTermsService.Setup(x => x.GetByUser(user)).ReturnsAsync(new UserAgreedTerms
            {
                Agreed = true,
                UserIdentifier = user
            });

            IActionResult result = await new IdentityController(mockIdentityService.Object, mockUserAgreedTermsService.Object).GetUser();

            Assert.IsType<NotFoundResult>(result);  
        }

        [Fact]
        public async Task GetUser_UserIdentifierNotFound_ReturnsUnauthorized()
        {
            var mockIdentityService = new Mock<IIdentityService>();

            var mockUserAgreedTermsService = new Mock<IUserAgreedTermsService>();

            IActionResult result = await new IdentityController(mockIdentityService.Object, mockUserAgreedTermsService.Object).GetUser();

            Assert.IsType<UnauthorizedResult>(result);
        }
    }    
}
