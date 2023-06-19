using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;
using YCNBot.Models;

namespace YCNBot.Controllers
{
    [Authorize(Policy = "AgreedToTerms")]
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IConfiguration _configuration;
        private readonly IIdentityService _identityService;

        public ChatController(IChatService chatService, IConfiguration configuration, IIdentityService identityService)
        {
            _chatService = chatService;
            _configuration = configuration;
            _identityService = identityService;
        }

        [HttpGet("get-chat-count")]
        public async Task<IActionResult> GetChatCount()
        {
            return Ok(await _chatService.GetCount());
        }

        [HttpGet("get-user-count")]
        public async Task<IActionResult> GetUserCount()
        {
            return Ok(await _chatService.GetUsersCount());
        }

        [HttpGet("get-all-by-user")]
        public async Task<IActionResult> GetAllByUser([FromQuery] int? pageNumber)
        {
            pageNumber ??= 1;

            Guid? userIdentifier = _identityService.GetUserIdentifier();

            if (userIdentifier == null)
            {
                return Unauthorized();
            }

            int pageSize = int.Parse(_configuration["PageSize"] ?? "100");

            return Ok((await _chatService.GetAllByUser(userIdentifier.Value, (pageNumber.Value * pageSize) - pageSize, pageSize))
                .Select(chat => new ChatModel
                {
                    Name = chat.Name,
                    UniqueIdentifier = chat.UniqueIdentifier
                }));
        }

        [HttpGet("get-by-unique-identifier/{chatIdentifier}")]
        public async Task<IActionResult> GetByChatIdentifier(Guid chatIdentifier)
        {
            Chat chat = await _chatService.GetByUniqueIdentifierWithMessages(chatIdentifier);

            if (chat.UserIdentifier != _identityService.GetUserIdentifier())
            {
                return Unauthorized();
            }

            return Ok(new ChatModel
            {
                Name = chat.Name,
                UniqueIdentifier = chat.UniqueIdentifier,
                Messages = chat.Messages
                    .Select(message => new MessageModel
                    {
                        ContainsCaseLaw = message.ContainsCaseLaw,
                        IsSystem = message.IsSystem,
                        Rating = message.Rating,
                        Text = message.Text,
                        UniqueIdentifier = message.UniqueIdentifier
                    })
            });
        }

        [HttpPatch("delete/{chatIdentifier}")]
        public async Task<IActionResult> Delete(Guid chatIdentifier)
        {
            Chat chat = await _chatService.GetByUniqueIdentifier(chatIdentifier);

            if (chat.UserIdentifier != _identityService.GetUserIdentifier())
            {
                return Unauthorized();
            }

            chat.Deleted = true;

            await _chatService.Update(chat);

            return StatusCode(204);
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update(UpdateChatModel chat)
        {
            Chat chatEntity = await _chatService.GetByUniqueIdentifier(chat.ChatIdentifier);

            if (chatEntity.UserIdentifier != _identityService.GetUserIdentifier())
            {
                return Unauthorized();
            }

            chatEntity.Name = chat.Name;

            await _chatService.Update(chatEntity);

            return StatusCode(204);
        }
    }
}