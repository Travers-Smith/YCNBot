using Microsoft.AspNetCore.Mvc;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;
using YCNBot.Models;

namespace YCNBot.Controllers
{
    [ApiController]
    [Route("community-prompt")]
    public class CommunityPromptController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ICommunityPromptService _communityPromptService;
        private readonly IUserService _userService;

        public CommunityPromptController(ICommunityPromptService communityPromptService, IIdentityService identityService, IUserService userService)
        {
            _identityService = identityService;
            _communityPromptService = communityPromptService;
            _userService = userService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddCommunityPromptModel addCommunityPromptData)
        {
            Guid? userIdentifier = _identityService.GetUserIdentifier();

            if (userIdentifier == null)
            {
                return Unauthorized();
            }

            await _communityPromptService.Add(new CommunityPrompt
            {
                DateAdded = DateTime.UtcNow,
                Answer = addCommunityPromptData.Answer,
                Question = addCommunityPromptData.Question,
                UserIdentifier = userIdentifier.Value,
                UniqueIdentifier = Guid.NewGuid()
            });

            return StatusCode(201);
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get(int? pageNumber)
        {
            pageNumber ??= 1;

            int pageSize = 100;

            Guid? userIdentifier = _identityService.GetUserIdentifier();

            if (userIdentifier == null)
            {
                return Unauthorized();
            }

            IEnumerable<CommunityPromptWithLikesAndCommentsCount> communityPromptItems = await _communityPromptService.Get(
                (pageNumber.Value * pageSize) - pageSize,
                pageSize,
                userIdentifier.Value
            );

            if (!communityPromptItems.Any())
            {
                return Ok(Array.Empty<CommunityPromptModel>());
            }

            List<CommunityPromptModel> communityPromptModels = new();

            Dictionary<string, User>? users = await _userService.GetUserDetails(communityPromptItems.Select(cq => cq.CommunityPrompt.UserIdentifier));

            if (users is null)
            {
                return StatusCode(500);
            }

            foreach (CommunityPromptWithLikesAndCommentsCount communityPromptData in communityPromptItems)
            {
                users.TryGetValue(communityPromptData.CommunityPrompt.UserIdentifier.ToString(), out User? user);

                CommunityPrompt communityPrompt = communityPromptData.CommunityPrompt;

                if (user is null)
                {
                    continue;
                }

                communityPromptModels.Add(new CommunityPromptModel
                {
                    Id = communityPrompt.Id,
                    Answer = communityPrompt.Answer,
                    LikesCount = communityPromptData.LikesCount,
                    CommentsCount = communityPromptData.CommentsCount,
                    Question = communityPrompt.Question,
                    UniqueIdentifier = communityPrompt.UniqueIdentifier,
                    User = new UserModel
                    {
                        Department = user.Department,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        JobTitle = user.JobTitle,
                    },
                    UserLiked = communityPrompt.CommunityPromptLikes.Any(cpl => cpl.UserIdentifier == userIdentifier)
                });
            }

            return Ok(communityPromptModels);
        }
    }
}