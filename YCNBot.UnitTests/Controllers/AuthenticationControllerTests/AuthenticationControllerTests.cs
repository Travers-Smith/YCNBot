using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using YCNBot.Controllers;
using YCNBot.Core.Services;

namespace YCNBot.UnitTest.Controllers.AuthenticationControllerTests
{
    public class AuthenticationControllerTests
    {
        [Fact]
        public void CheckIsLoggedIn_IsLoggedIn_ReturnsTrueActionResult()
        {
            var identityService = new Mock<IIdentityService>();
            var agreedToTermsClaimTranformation = new Mock<IClaimsTransformation>();
            var userAgreedToTermsService = new Mock<IUserAgreedTermsService>();

            identityService.Setup(x => x.IsAuthenticated()).Returns(true);

            IActionResult result = new AuthenticationController(agreedToTermsClaimTranformation.Object, identityService.Object, userAgreedToTermsService.Object)
                .CheckIsLoggedIn();

            var actionResult = Assert.IsType<OkObjectResult>(result);

            var returnedValue = Assert.IsType<bool>(actionResult.Value);

            Assert.True(returnedValue);
        }

        [Fact]
        public void CheckIsLoggedIn_NoLoggedIn_ReturnsFalseActionResult()
        {
            var identityService = new Mock<IIdentityService>();
            var agreedToTermsClaimTranformation = new Mock<IClaimsTransformation>();
            var userAgreedToTermsService = new Mock<IUserAgreedTermsService>();
            identityService.Setup(x => x.IsAuthenticated()).Returns(false);

            IActionResult result = new AuthenticationController(agreedToTermsClaimTranformation.Object, identityService.Object, userAgreedToTermsService.Object)
                .CheckIsLoggedIn();

            var actionResult = Assert.IsType<OkObjectResult>(result);

            var returnedValue = Assert.IsType<bool>(actionResult.Value);

            Assert.False(returnedValue);
        }

        [Fact]
        public async Task Login_SuccessUserAgreedToTerms_ReturnsRedirectResult()
        {
            var identityService = new Mock<IIdentityService>();
            var agreedToTermsClaimTranformation = new Mock<IClaimsTransformation>();
            var userAgreedToTermsService = new Mock<IUserAgreedTermsService>();
            userAgreedToTermsService.Setup(x => x.CheckAgreed(It.IsAny<Guid>())).ReturnsAsync(true);

            IActionResult result = await new AuthenticationController(agreedToTermsClaimTranformation.Object, identityService.Object, userAgreedToTermsService.Object)
                .Login();

            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public async Task Login_SuccessUserNotAgreedToTerms_ReturnsRedirectResult()
        {
            var identityService = new Mock<IIdentityService>();
            var agreedToTermsClaimTranformation = new Mock<IClaimsTransformation>();
            var userAgreedToTermsService = new Mock<IUserAgreedTermsService>();

            userAgreedToTermsService.Setup(x => x.CheckAgreed(It.IsAny<Guid>())).ReturnsAsync(false);

            IActionResult result = await new AuthenticationController(agreedToTermsClaimTranformation.Object, identityService.Object, userAgreedToTermsService.Object)
                .Login();

            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public async Task Login_UserAgreedToTerms_AddsClaim()
        {
            var identityService = new Mock<IIdentityService>();
            var agreedToTermsClaimTranformation = new Mock<IClaimsTransformation>();
            var userAgreedToTermsService = new Mock<IUserAgreedTermsService>();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            userAgreedToTermsService.Setup(x => x.CheckAgreed(It.IsAny<Guid>())).ReturnsAsync(true);

            IActionResult result = await new AuthenticationController(agreedToTermsClaimTranformation.Object, identityService.Object, userAgreedToTermsService.Object)
                .Login();

            agreedToTermsClaimTranformation.Verify(x => x.TransformAsync(It.IsAny<ClaimsPrincipal>()), Times.Once);
        }


        [Fact]
        public async Task Login_UserHasntAgreedToTerms_DoesntAddClaim()
        {
            var identityService = new Mock<IIdentityService>();
            var agreedToTermsClaimTranformation = new Mock<IClaimsTransformation>();
            var userAgreedToTermsService = new Mock<IUserAgreedTermsService>();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());
            userAgreedToTermsService.Setup(x => x.CheckAgreed(It.IsAny<Guid>())).ReturnsAsync(false);

            IActionResult result = await new AuthenticationController(agreedToTermsClaimTranformation.Object, identityService.Object, userAgreedToTermsService.Object)
                .Login();

            agreedToTermsClaimTranformation.Verify(x => x.TransformAsync(It.IsAny<ClaimsPrincipal>()), Times.Never);
        }

        [Fact]
        public void Logout_Success_ReturnsSignoutResult()
        {
            var identityService = new Mock<IIdentityService>();
            var agreedToTermsClaimTranformation = new Mock<IClaimsTransformation>();
            var userAgreedToTermsService = new Mock<IUserAgreedTermsService>();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());
            userAgreedToTermsService.Setup(x => x.CheckAgreed(It.IsAny<Guid>())).ReturnsAsync(false);

            IActionResult result = new AuthenticationController(agreedToTermsClaimTranformation.Object, identityService.Object, userAgreedToTermsService.Object)
                .Logout();

            Assert.IsType<SignOutResult>(result);
        }
    }
}
