using Microsoft.AspNetCore.Mvc;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;
using YCNBot.Models;

namespace YCNBot.Controllers
{
    [ApiController]
    [Route("user-feedback")]
    public class UserFeedbackController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IUserFeedbackService _userFeedbackService;

        public UserFeedbackController(IIdentityService identityService, IUserFeedbackService userFeedbackService)
        {
            _identityService = identityService;
            _userFeedbackService = userFeedbackService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddUserFeedbackModel addUserFeedback)
        {
            Guid? userIdentifier = _identityService.GetUserIdentifier();

            if (userIdentifier == null)
            {
                return Unauthorized();
            }

            await _userFeedbackService.Add(new UserFeedback
            {
                FeedbackTypeId = addUserFeedback.FeedbackTypeId,
                Text = addUserFeedback.Text,
                UserIdentifier = userIdentifier.Value
            });

            return StatusCode(201);
        }
    }
}