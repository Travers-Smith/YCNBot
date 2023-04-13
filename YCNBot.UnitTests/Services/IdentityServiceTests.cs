using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using YCNBot.Services;

namespace YCNBot.UnitTest.Services
{
    public class IdentityServiceTests
    {
        [Fact]
        public void GetUserIdentifier_Success_ReturnsGuid()
        {
            var mock = new Mock<IHttpContextAccessor>();

            Guid userGuid = Guid.NewGuid();

            mock.SetupGet(x => x.HttpContext.User.Claims).Returns(new List<Claim>
            {
                new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", userGuid.ToString())
            });

            Guid? userIdentifier = new IdentityService(mock.Object).GetUserIdentifier();

            Assert.Equal(userGuid, userIdentifier);
        }

        [Fact]
        public void GetUserIdentifier_NoUserIdentifier_ReturnsNull()
        {
            var mock = new Mock<IHttpContextAccessor>();

            mock.SetupGet(x => x.HttpContext.User.Claims).Returns(new List<Claim>());

            Guid? userIdentifier = new IdentityService(mock.Object).GetUserIdentifier();

            Assert.Null(userIdentifier);
        }

        [Fact]
        public void GetEmail_Success_ReturnsEmail()
        {
            var mock = new Mock<IHttpContextAccessor>();

            string userEmail = "test@email.com";

            mock.SetupGet(x => x.HttpContext.User.Claims).Returns(new List<Claim>
            {
                new Claim("preferred_username", userEmail.ToString())
            });

            string? email = new IdentityService(mock.Object).GetEmail();

            Assert.Equal(userEmail, email);
        }

        [Fact]
        public void GetEmail_NoEmail_ReturnsNull()
        {
            var mock = new Mock<IHttpContextAccessor>();

            mock.SetupGet(x => x.HttpContext.User.Claims).Returns(new List<Claim>());

            string? email = new IdentityService(mock.Object).GetEmail();

            Assert.Null(email);
        }

        [Fact]
        public void IsAuthenticated_UserAuthenticated_ReturnsTrue()
        {
            var mock = new Mock<IHttpContextAccessor>();

            mock.SetupGet(x => x.HttpContext.User.Identity.IsAuthenticated).Returns(true);

            Assert.True(new IdentityService(mock.Object).IsAuthenticated());
        }

        [Fact]
        public void IsAuthenticated_NoUserAuthenticated_ReturnsFalse()
        {
            var mock = new Mock<IHttpContextAccessor>();

            mock.SetupGet(x => x.HttpContext.User.Identity.IsAuthenticated).Returns(false);

            Assert.False(new IdentityService(mock.Object).IsAuthenticated());
        }
    }
}
