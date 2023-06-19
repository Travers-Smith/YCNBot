using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using YCNBot.Controllers;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;
using YCNBot.Models;

namespace YCNBot.UnitTest.Controllers.MessageControllerTests
{
    public class MessageControllerTests
    {
        [Fact]
        public async Task AddNonRecordedChat_Successful_ReturnsMessage()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var configuration = new Mock<IConfiguration>();
            var identityService = new Mock<IIdentityService>();
            var messageService = new Mock<IMessageService>();
            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            string chatGptMessage = "test message content";

            configuration.Setup(config => config["AllowPersonalMode"]).Returns("true");

            chatModelPicker.Setup(x => x.GetModel(It.IsAny<string>())).Returns(chatCompletionService.Object);

            chatCompletionService.Setup(x => x.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>())).ReturnsAsync(chatGptMessage);

            identityService.Setup(x => x.IsAuthenticated()).Returns(true);
            configuration.Setup(config => config["AllowPersonalMode"]).Returns("true");

            IActionResult result = await new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object)
                .AddNonRecordedChat(new AddNonRecordedMessageModel[]
                {
                    new AddNonRecordedMessageModel
                    {
                        IsSystem = false,
                        Text = "this is a test"
                    }
                });

            Assert.Multiple(() =>
            {
                var actionResult = Assert.IsType<OkObjectResult>(result);
                var message = Assert.IsType<MessageModel>(actionResult.Value);

                Assert.Equal(message.Text, chatGptMessage);
            });
        }

        [Fact]
        public async Task AddNonRecordedChat_PersonalModeForbidden_ReturnsForbiddenResult()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var configuration = new Mock<IConfiguration>();
            var identityService = new Mock<IIdentityService>();
            var messageService = new Mock<IMessageService>();
            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            configuration.Setup(config => config["AllowPersonalMode"]).Returns("false");

            chatCompletionService.Setup(x => x.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>())).ReturnsAsync("new message");

            identityService.Setup(x => x.IsAuthenticated()).Returns(true);

            IActionResult result = await new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object)
                .AddNonRecordedChat(new AddNonRecordedMessageModel[]
                {
                    new AddNonRecordedMessageModel
                    {
                        IsSystem = false,
                        Text = "this is a test"
                    }
                });

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task AddNonRecordedChat_NoChatCompletionService_Returns500()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var configuration = new Mock<IConfiguration>();
            var identityService = new Mock<IIdentityService>();
            var messageService = new Mock<IMessageService>();
            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            string chatGptMessage = "test message content";

            configuration.Setup(config => config["AllowPersonalMode"]).Returns("true");

            chatCompletionService.Setup(x => x.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>())).ReturnsAsync(chatGptMessage);

            identityService.Setup(x => x.IsAuthenticated()).Returns(true);
            configuration.Setup(config => config["AllowPersonalMode"]).Returns("true");

            IActionResult result = await new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object)
                .AddNonRecordedChat(new AddNonRecordedMessageModel[]
                {
                    new AddNonRecordedMessageModel
                    {
                        IsSystem = false,
                        Text = "this is a test"
                    }
                });

            Assert.Multiple(() =>
            {
                var actionResult = Assert.IsType<StatusCodeResult>(result);
                Assert.Equal(500, actionResult.StatusCode);
            });
        }


        [Fact]
        public async Task AddNonRecordedChat_AddFails_ThrowException()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var configuration = new Mock<IConfiguration>();
            var identityService = new Mock<IIdentityService>();
            var messageService = new Mock<IMessageService>();
            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            configuration.Setup(config => config["AllowPersonalMode"]).Returns("true");
            chatModelPicker.Setup(x => x.GetModel(It.IsAny<string>())).Returns(chatCompletionService.Object);

            chatCompletionService.Setup(x => x.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>())).ThrowsAsync(new Exception());

            identityService.Setup(x => x.IsAuthenticated()).Returns(true);

            MessageController controller = new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object);

            await Assert.ThrowsAsync<Exception>(() => controller.AddNonRecordedChat(new AddNonRecordedMessageModel[]
            {
                new AddNonRecordedMessageModel
                {
                    IsSystem = false,
                    Text = "this is a test"
                }
            }));
        }

        [Fact]
        public async Task AddNonRecordedChat_Unauthorized_ReturnsUnauthorized()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var configuration = new Mock<IConfiguration>();
            var identityService = new Mock<IIdentityService>();
            var messageService = new Mock<IMessageService>();
            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            configuration.Setup(config => config["AllowPersonalMode"]).Returns("true");

            chatCompletionService.Setup(x => x.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>())).ReturnsAsync("new message");

            identityService.Setup(x => x.IsAuthenticated()).Returns(false);

            IActionResult result = await new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object)
                .AddNonRecordedChat(new AddNonRecordedMessageModel[]
                {
                    new AddNonRecordedMessageModel
                    {
                        IsSystem = false,
                        Text = "this is a test"
                    }
                });

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task ChangeRating_Success_UpdatesMessage()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var configuration = new Mock<IConfiguration>();
            var identityService = new Mock<IIdentityService>();
            var messageService = new Mock<IMessageService>();
            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            Guid userIdentifier = Guid.NewGuid();

            messageService.Setup(x => x.GetByUniqueIdentifierWithChat(It.IsAny<Guid>())).ReturnsAsync(new Message
            {
                Chat = new Chat
                {
                    UserIdentifier = userIdentifier,
                }
            });

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            IActionResult result = await new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object)
                .ChangeMessageRating(new UpdateMessageRatingModel
                {
                    MessageIdentifier = Guid.NewGuid(),
                    Rating = 5
                });

            messageService.Verify(x => x.Update(It.IsAny<Message>()), Times.Once);
        }

        [Fact]
        public async Task ChangeRating_Success_Returns204()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var configuration = new Mock<IConfiguration>();
            var identityService = new Mock<IIdentityService>();
            var messageService = new Mock<IMessageService>();
            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            Guid userIdentifier = Guid.NewGuid();

            messageService.Setup(x => x.GetByUniqueIdentifierWithChat(It.IsAny<Guid>())).ReturnsAsync(new Message
            {
                Chat = new Chat
                {
                    UserIdentifier = userIdentifier,
                }
            });

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            IActionResult result = await new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object)
                .ChangeMessageRating(new UpdateMessageRatingModel
                {
                    MessageIdentifier = Guid.NewGuid(),
                    Rating = 5
                });

            var actionResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal(204, actionResult.StatusCode);
        }

        [Fact]
        public async Task ChangeRating_UnauthorizedMessage_DoesntUpdateMessage()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var configuration = new Mock<IConfiguration>();
            var identityService = new Mock<IIdentityService>();
            var messageService = new Mock<IMessageService>();
            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            Guid userIdentifier = Guid.NewGuid();

            messageService.Setup(x => x.GetByUniqueIdentifierWithChat(It.IsAny<Guid>())).ReturnsAsync(new Message
            {
                Chat = new Chat
                {
                    UserIdentifier = Guid.NewGuid(),
                }
            });

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            IActionResult result = await new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object)
                .ChangeMessageRating(new UpdateMessageRatingModel
                {
                    MessageIdentifier = Guid.NewGuid(),
                    Rating = 5
                });

            messageService.Verify(x => x.Update(It.IsAny<Message>()), Times.Never);
        }

        [Fact]
        public async Task ChangeRating_UnauthorizedMessage_ReturnsUnauthorized()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var configuration = new Mock<IConfiguration>();
            var identityService = new Mock<IIdentityService>();
            var messageService = new Mock<IMessageService>();
            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            Guid userIdentifier = Guid.NewGuid();

            messageService.Setup(x => x.GetByUniqueIdentifierWithChat(It.IsAny<Guid>())).ReturnsAsync(new Message
            {
                Chat = new Chat
                {
                    UserIdentifier = Guid.NewGuid(),
                }
            });

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            IActionResult result = await new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object)
                .ChangeMessageRating(new UpdateMessageRatingModel
                {
                    MessageIdentifier = Guid.NewGuid(),
                    Rating = 5
                });

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task ChangeRating_UpdateFails_ThrowsException()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var configuration = new Mock<IConfiguration>();
            var identityService = new Mock<IIdentityService>();
            var messageService = new Mock<IMessageService>();
            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            Guid userIdentifier = Guid.NewGuid();

            messageService.Setup(x => x.GetByUniqueIdentifierWithChat(It.IsAny<Guid>())).ReturnsAsync(new Message
            {
                Chat = new Chat
                {
                    UserIdentifier = userIdentifier
                }
            });

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            messageService.Setup(x => x.Update(It.IsAny<Message>())).ThrowsAsync(new Exception());


            MessageController controller = new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object);


            await Assert.ThrowsAsync<Exception>(() => controller.ChangeMessageRating(new UpdateMessageRatingModel
            {
                MessageIdentifier = Guid.NewGuid(),
                Rating = 5
            }));

        }
    }
}
