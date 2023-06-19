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
    public class MessageController : ControllerBase
    {
        private readonly ICaseLawDetectionService _caseLawDetectionService;
        private readonly IChatService _chatService;
        private readonly IChatModelPickerService _chatModelPickerService;
        private readonly IConfiguration _configuration;
        private readonly IIdentityService _identityService;
        private readonly IMessageService _messageService;
        private readonly IPersonalInformationCheckerService _personalInformationCheckerService;

        public MessageController(ICaseLawDetectionService caseLawDetectionService, IChatService chatService, IChatModelPickerService chatModelPickerService,
            IConfiguration configuration, IIdentityService identityService, IMessageService messageService,
            IPersonalInformationCheckerService personalInformationCheckerService)
        {
            _caseLawDetectionService = caseLawDetectionService;
            _chatModelPickerService = chatModelPickerService;
            _chatService = chatService;
            _messageService = messageService;
            _configuration = configuration;
            _identityService = identityService;
            _personalInformationCheckerService = personalInformationCheckerService;
        }

        [HttpPost("add-recorded-message")]
        public async Task<IActionResult> AddRecordedChat(AddMessageModel message)
        {
            if (!message.AllowPersonalInformation && _personalInformationCheckerService.CheckIfStringHasNames(message.Message))
            {
                return BadRequest("contains personal information");
            }

            List<Message> newMessages = new();

            Message newMessage = new()
            {
                IsSystem = false,
                Text = message.Message,
                DateAdded = DateTime.Now
            };

            Guid? userIdentifier = _identityService.GetUserIdentifier();

            if (userIdentifier == null)
            {
                return Unauthorized();
            }

            Chat chat;

            if (message.ChatIdentifier != null)
            {
                chat = await _chatService.GetByUniqueIdentifierWithMessages(message.ChatIdentifier.Value);

                if (chat.UserIdentifier != userIdentifier)
                {
                    return Unauthorized();
                }

                newMessage.ChatId = chat.Id;
            }
            else
            {
                chat = new Chat
                {
                    Name = message.Message[..Math.Min(message.Message.Length, 100)],
                    UserIdentifier = userIdentifier.Value,
                    UniqueIdentifier = Guid.NewGuid()
                };

                newMessage.Chat = chat;
            }

            newMessages.Add(newMessage);

            IChatCompletionService? chatCompletionService = _chatModelPickerService.GetModel(_configuration["ChatCompletionType"] ?? "AzureOpenAI");

            if (chatCompletionService is null)
            {
                return StatusCode(500);
            }

            string systemMessage = await chatCompletionService
                .AddChatCompletion(chat.Messages.Concat(newMessages), _configuration["ChatModel"] ?? "");

            if (!message.AllowPersonalInformation && _personalInformationCheckerService.CheckIfStringHasNames(systemMessage))
            {
                return BadRequest("contains personal information");
            }

            bool containsCaseLaw = _caseLawDetectionService.CheckContainsCaseLaw(systemMessage);

            newMessages.Add(new Message
            {
                ChatId = chat.Id,
                Chat = chat.Id == 0 ? chat : null,
                IsSystem = true,
                Text = systemMessage,
                DateAdded = DateTime.Now,
                ContainsCaseLaw = containsCaseLaw
            });

            await _messageService.AddRange(newMessages);

            return Ok(new MessageModel
            {
                Chat = new ChatModel
                {
                    Name = chat.Name,
                    UniqueIdentifier = chat.UniqueIdentifier
                },
                ContainsCaseLaw = containsCaseLaw,
                IsSystem = true,
                Text = systemMessage,
                UniqueIdentifier = newMessages.Select(x => x.UniqueIdentifier).LastOrDefault(),
                InputContainedPersonalInformation = message.AllowPersonalInformation && _personalInformationCheckerService.CheckIfStringHasNames(message.Message),
                OutputContainedPersonalInformation = message.AllowPersonalInformation && _personalInformationCheckerService.CheckIfStringHasNames(systemMessage)
            });
        }

        [HttpPost("add-non-recorded-message")]
        public async Task<IActionResult> AddNonRecordedChat(IEnumerable<AddNonRecordedMessageModel> messages)
        {
            if (_configuration["AllowPersonalMode"] != "true")
            {
                return Forbid();
            }

            if (!_identityService.IsAuthenticated())
            {
                return Unauthorized();
            }

            IChatCompletionService? chatCompletionService = _chatModelPickerService.GetModel(_configuration["ChatCompletionType"] ?? "AzureOpenAI");

            if (chatCompletionService is null)
            {
                return StatusCode(500);
            }

            string systemMessage = await chatCompletionService
                .AddChatCompletion(messages
                    .Select(x => new Message
                    {
                        Text = x.Text,
                        IsSystem = x.IsSystem
                    }),
                    _configuration["ChatModel"] ?? "");

            return Ok(new MessageModel
            {
                IsSystem = true,
                Text = systemMessage
            });
        }

        [HttpPatch("change-rating")]
        public async Task<IActionResult> ChangeMessageRating(UpdateMessageRatingModel messageRating)
        {
            Message message = await _messageService.GetByUniqueIdentifierWithChat(messageRating.MessageIdentifier);

            if (message.Chat.UserIdentifier != _identityService.GetUserIdentifier())
            {
                return Unauthorized();
            }

            message.Rating = messageRating.Rating;

            await _messageService.Update(message);

            return StatusCode(204);
        }

        [HttpGet("get-count")]
        public async Task<IActionResult> GetUserCount()
        {
            return Ok(await _messageService.GetCount());
        }

        [HttpGet("get-message-breakdown")]
        public async Task<IActionResult> GetMessageBreakdown()
        {
            return Ok((await _messageService.GetDateBreakdown(20))
                .Select(x => new DateBreakdownModel
                {
                    DateAdded = x.Item1.ToString("dd/MM/yy"),
                    Messages = x.Item2
                }));
        }
    }
}