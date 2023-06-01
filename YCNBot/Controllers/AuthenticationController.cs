using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YCNBot.Core.Services;

namespace YCNBot.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IClaimsTransformation _agreedToTermsClaimTransformation;
        private readonly IUserAgreedTermsService _userAgreedTermsService;

        public AuthenticationController(IClaimsTransformation agreedToTermsClaimTranformation, IIdentityService identityService,
            IUserAgreedTermsService userAgreedTermsService)
        {
            _identityService = identityService;
            _agreedToTermsClaimTransformation = agreedToTermsClaimTranformation;
            _userAgreedTermsService = userAgreedTermsService;
        }

        [AllowAnonymous]
        [HttpGet("check-is-logged-in")]
        public IActionResult CheckIsLoggedIn()
        {
            return Ok(_identityService.IsAuthenticated());
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            Guid? userIdentifier = _identityService.GetUserIdentifier();

            if (userIdentifier != null && await _userAgreedTermsService.CheckAgreed(userIdentifier.Value))
            {
                await _agreedToTermsClaimTransformation.TransformAsync(User);
            }

            return Redirect("/chat");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            return SignOut(
                 new AuthenticationProperties
                 {
                     RedirectUri = "/login",
                 },
                 CookieAuthenticationDefaults.AuthenticationScheme,
                 OpenIdConnectDefaults.AuthenticationScheme
                );

        }
    }
}