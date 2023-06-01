using Microsoft.AspNetCore.Mvc;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;

namespace YCNBot.Controllers
{
    [ApiController]
    [Route("user-terms")]
    public class UserAgreedTermsController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IUserAgreedTermsService _userAgreedTermsService;

        public UserAgreedTermsController(IIdentityService identityService, IUserAgreedTermsService userAgreedTermsService)
        {
            _identityService = identityService;
            _userAgreedTermsService = userAgreedTermsService;
        }

        [HttpPost("accept-terms")]
        public async Task<IActionResult> AcceptTerms()
        {
            Guid? userIdentifier = _identityService.GetUserIdentifier();

            if (userIdentifier == null)
            {
                return Unauthorized();

            }

            UserAgreedTerm? userAgreedTerms = await _userAgreedTermsService.GetByUser(userIdentifier.Value);

            if (userAgreedTerms == null)
            {
                await _userAgreedTermsService.Add(new UserAgreedTerm
                {
                    UserIdentifier = userIdentifier.Value,
                    Agreed = true
                });
            }
            else
            {
                userAgreedTerms.Agreed = true;

                await _userAgreedTermsService.Update(userAgreedTerms);
            }

            return StatusCode(204);
        }
    }
}