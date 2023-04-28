﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;
using YCNBot.Models;

namespace YCNBot.Controllers
{
    [ApiController]
    [Authorize(Policy = "Admins")]
    [Route("user-reporting")]
    public class UserReportingController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IConfiguration _configuration;
        private readonly IIdentityService _identityService;
        private readonly IUserService _userService;

        public UserReportingController(IChatService chatService, IConfiguration configuration, IIdentityService identityService, IUserService userService)
        {
            _chatService = chatService;
            _configuration = configuration;
            _identityService = identityService;
            _userService = userService;
        }

        [HttpGet("get-active-users")] 
        public async Task<IActionResult> GetActiveUsers(int? pageNumber)
        {
            Guid? userIdentifier = _identityService.GetUserIdentifier();

            if(userIdentifier == null)
            {
                return Unauthorized();
            }

            pageNumber ??= 1;

            int pageSize = int.Parse(_configuration["PageSize"] ?? "100");

            Dictionary<Guid, int> usersChatUsage = await _chatService.GetUsersUsage((pageSize * pageNumber.Value) - pageSize, pageSize);

            IEnumerable<User>? users = await _userService.GetUsers(usersChatUsage.Keys);

            if(users is null)
            {
                return StatusCode(500);
            }

            return Ok(users
                .Select(user => new UserUsageModel
                {
                    TotalChats = usersChatUsage[user.Id],
                    User = new UserModel
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Department = user.Department,
                        JobTitle = user.JobTitle,
                    }
                })
                .OrderByDescending(x => x.TotalChats));
        }
    }
}