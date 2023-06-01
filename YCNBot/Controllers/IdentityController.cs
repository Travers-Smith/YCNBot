using Microsoft.AspNetCore.Mvc;
using YCNBot.Core.Entities;
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
        private readonly IUserService _userService;
        public IdentityController(IIdentityService identityService, IUserAgreedTermsService userAgreedTermsService, IUserService userService)
        {
            _identityService = identityService;
            _userAgreedTermsService = userAgreedTermsService;
            _userService = userService;
        }

        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser()
        {
            Guid? userIdentifier = _identityService.GetUserIdentifier();

            if (userIdentifier == null)
            {
                return Unauthorized();
            }

            User? user = await _userService.GetUser(userIdentifier.Value);

            UserModel userModel = new()
            {
                AgreedToTerms = await _userAgreedTermsService.CheckAgreed(userIdentifier.Value),
                Email = _identityService.GetEmail(),
                Department = user?.Department,
                FirstName = user?.FirstName,
                LastName = user?.LastName,
                JobTitle = user?.JobTitle,
                IsAdmin = _identityService.IsAdmin()
            };

            string? name = _identityService.GetName();

            if (name == null)
            {
                return NotFound();
            }

            string[] names = name.Split(",");

            userModel.LastName = names.FirstOrDefault()?.Trim();

            if (names.Length > 1)
            {
                userModel.FirstName = names[1].Trim();
            }

            return Ok(userModel);
        }
    }
}