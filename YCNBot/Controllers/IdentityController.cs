using Microsoft.AspNetCore.Mvc;
using YCNBot.Core.Services;
using YCNBot.Models;

namespace YCNBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IUserAgreedTermsService _userAgreedTermsService;

        public IdentityController(IIdentityService identityService, IUserAgreedTermsService userAgreedTermsService)
        {
            _identityService = identityService;
            _userAgreedTermsService = userAgreedTermsService;
        }

        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser()
        {
            Guid? userIdentifier = _identityService.GetUserIdentifier();

            if(userIdentifier == null)
            {
                return Unauthorized();
            }

            UserModel user = new()
            {
                AgreedToTerms = await _userAgreedTermsService.CheckAgreed(userIdentifier.Value),
                Email = _identityService.GetEmail(),
                IsAdmin = _identityService.IsAdmin()
            };

            string? name = _identityService.GetName();

            if(name == null)
            {
                return NotFound();
            }

            string[] names = name.Split(",");

            user.LastName = names.FirstOrDefault()?.Trim();

            if(names.Length > 1)
            {
                user.FirstName = names[1].Trim();
            }

            return Ok(user);
        }
    }
}