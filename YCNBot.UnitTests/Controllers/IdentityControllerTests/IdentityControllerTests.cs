using Microsoft.AspNetCore.Mvc;
using Moq;
using YCNBot.Controllers;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;
using YCNBot.Models;

namespace YCNBot.UnitTest.Controllers.IdentityControllerTests
{
    public class IdentityControllerTests
    {
        [Fact]
        public async Task GetUser_SuccessAgreedToTerm_ReturnsUserWithAgreed()
        {
            var mockIdentityService = new Mock<IIdentityService>();
            var mockUserService = new Mock<IUserService>();

            Guid user = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(user);
            mockIdentityService.Setup(x => x.GetName()).Returns("Test username");

            mockUserService.Setup(x => x.GetUser(user)).ReturnsAsync(new User
            {
                FirstName = "Test first name",
                LastName = "Test last name",
                Email = "test@test.com",
                Department = "Test department",
                JobTitle = "Test job title"
            });

            var mockUserAgreedTermsService = new Mock<IUserAgreedTermsService>();

            mockUserAgreedTermsService.Setup(x => x.CheckAgreed(user)).ReturnsAsync(true);

            IActionResult result = await new IdentityController(mockIdentityService.Object, mockUserAgreedTermsService.Object, mockUserService.Object).GetUser();

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
            var mockUserService = new Mock<IUserService>();

            Guid user = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(user);
            mockIdentityService.Setup(x => x.GetName()).Returns("Test username");

            mockUserService.Setup(x => x.GetUser(user)).ReturnsAsync(new User
            {
                FirstName = "Test first name",
                LastName = "Test last name",
                Email = "test@test.com",
                Department = "Test department",
                JobTitle = "Test job title"
            });
            var mockUserAgreedTermsService = new Mock<IUserAgreedTermsService>();

            mockUserAgreedTermsService.Setup(x => x.CheckAgreed(user)).ReturnsAsync(false);


            IActionResult result = await new IdentityController(mockIdentityService.Object, mockUserAgreedTermsService.Object, mockUserService.Object).GetUser();

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
            var mockUserService = new Mock<IUserService>();

            Guid user = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(user);

            mockUserService.Setup(x => x.GetUser(user)).ReturnsAsync(new User
            {
                FirstName = "Test first name",
                LastName = "Test last name",
                Email = "test@test.com",
                Department = "Test department",
                JobTitle = "Test job title"
            });
            var mockUserAgreedTermsService = new Mock<IUserAgreedTermsService>();

            mockUserAgreedTermsService.Setup(x => x.GetByUser(user)).ReturnsAsync(new UserAgreedTerm
            {
                Agreed = true,
                UserIdentifier = user
            });

            IActionResult result = await new IdentityController(mockIdentityService.Object, mockUserAgreedTermsService.Object, mockUserService.Object).GetUser();

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetUser_UserIdentifierNotFound_ReturnsUnauthorized()
        {
            var mockIdentityService = new Mock<IIdentityService>();
            var mockUserService = new Mock<IUserService>();

            var mockUserAgreedTermsService = new Mock<IUserAgreedTermsService>();

            IActionResult result = await new IdentityController(mockIdentityService.Object, mockUserAgreedTermsService.Object, mockUserService.Object).GetUser();

            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
