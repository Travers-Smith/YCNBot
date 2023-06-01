using Microsoft.AspNetCore.Mvc;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;
using YCNBot.Models;

namespace YCNBot.Controllers
{
    [ApiController]
    [Route("community-prompt-like")]
    public class CommunityPromptLikeController : ControllerBase
    {
        private readonly ICommunityPromptLikeService _communityPromptLikeService;
        private readonly IIdentityService _identityService;
        private readonly IUserService _userService;

        public CommunityPromptLikeController(ICommunityPromptLikeService communityPromptLikeService, IIdentityService identityService, IUserService userService)
        {
            _communityPromptLikeService = communityPromptLikeService;
            _identityService = identityService;
            _userService = userService;
        }

        [HttpPatch("update")]
        public async Task<IActionResult> AddOrRemove(AddCommunityPromptLikeModel addCommunityPromptLike)
        {
            Guid? userIdentifier = _identityService.GetUserIdentifier();

            if (userIdentifier == null)
            {
                return Unauthorized();
            }

            CommunityPromptLike? like = await _communityPromptLikeService.GetByCommunityPromptIdAndUser(addCommunityPromptLike.CommunityPromptId, userIdentifier.Value);

            if (addCommunityPromptLike.Liked && like is null)
            {
                await _communityPromptLikeService.Add(new CommunityPromptLike
                {
                    DateAdded = DateTime.UtcNow,
                    CommunityPromptId = addCommunityPromptLike.CommunityPromptId,

                    UserIdentifier = userIdentifier.Value,
                });
            }
            else if (like != null)
            {
                await _communityPromptLikeService.Remove(like);
            }

            return StatusCode(204);
        }

        [HttpGet("get-by-community-prompt-id")]
        public async Task<IActionResult> GetByCommunityPromptId(int communityPromptId, int? pageNumber)
        {
            pageNumber ??= 1;

            int pageSize = 100;

            IEnumerable<CommunityPromptLike> communityPrompts = await _communityPromptLikeService.GetByCommunityPromptId(
                communityPromptId,
                (pageNumber.Value * pageSize) - pageSize,
                pageSize);

            if (!communityPrompts.Any())
            {
                return Ok(Array.Empty<UserModel>());
            }

            List<UserModel> likes = new();

            Dictionary<string, User>? users = await _userService.GetUserDetails(communityPrompts.Select(cq => cq.UserIdentifier));

            if (users is null)
            {
                return StatusCode(500);
            }

            return Ok(users.Select(user => new UserModel
            {
                Department = user.Value.Department,
                Email = user.Value.Email,
                FirstName = user.Value.FirstName,
                LastName = user.Value.LastName,
                JobTitle = user.Value.JobTitle,
            }));
        }
    }
}