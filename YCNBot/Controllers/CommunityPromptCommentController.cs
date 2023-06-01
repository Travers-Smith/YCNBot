using Microsoft.AspNetCore.Mvc;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;
using YCNBot.Models;

namespace YCNBot.Controllers
{
    [ApiController]
    [Route("community-prompt-comment")]
    public class CommunityPromptCommentController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ICommunityPromptCommentService _communityPromptCommentService;
        private readonly IUserService _userService;

        public CommunityPromptCommentController(ICommunityPromptCommentService communityPromptCommentService, IIdentityService identityService,
            IUserService userService)
        {
            _identityService = identityService;
            _communityPromptCommentService = communityPromptCommentService;
            _userService = userService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddCommunityPromptCommentModel addCommunityPromptCommentModel)
        {
            Guid? userIdentifier = _identityService.GetUserIdentifier();

            if (userIdentifier == null)
            {
                return Unauthorized();
            }

            await _communityPromptCommentService.Add(new CommunityPromptComment
            {
                Comment = addCommunityPromptCommentModel.Comment,
                CommunityPromptId = addCommunityPromptCommentModel.CommunityPromptId,
                DateAdded = DateTime.Now,
                UserIdentifier = userIdentifier.Value
            });

            return StatusCode(201);
        }

        [HttpGet("get-by-community-prompt-id")]
        public async Task<IActionResult> GetByCommunityPromptId(int communityPromptId, int? pageNumber)
        {
            pageNumber ??= 1;

            int pageSize = 100;

            IEnumerable<CommunityPromptComment> communityPrompts = await _communityPromptCommentService
                .GetByCommunityPromptId(communityPromptId, (pageNumber.Value * pageSize) - pageSize, pageSize);

            if (!communityPrompts.Any())
            {
                return Ok(Array.Empty<CommunityPromptModel>());
            }

            List<CommunityPromptCommentModel> communityPromptModels = new();

            Dictionary<string, User>? users = await _userService.GetUserDetails(communityPrompts.Select(cq => cq.UserIdentifier));

            if (users is null)
            {
                return StatusCode(500);
            }

            foreach (CommunityPromptComment communityPromptComment in communityPrompts)
            {
                users.TryGetValue(communityPromptComment.UserIdentifier.ToString(), out User? user);

                if (user is null)
                {
                    continue;
                }

                communityPromptModels.Add(new CommunityPromptCommentModel
                {
                    Comment = communityPromptComment.Comment,
                    DateAdded = communityPromptComment.DateAdded,
                    User = new UserModel
                    {
                        Department = user.Department,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        JobTitle = user.JobTitle,
                    }
                });
            }

            return Ok(communityPromptModels);
        }
    }
}