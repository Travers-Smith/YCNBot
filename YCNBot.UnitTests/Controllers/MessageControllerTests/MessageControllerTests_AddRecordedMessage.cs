using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using YCNBot.Controllers;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;
using YCNBot.Models;
using YCNBot.Services;

namespace YCNBot.UnitTest.Controllers.MessageControllerTests
{
    public class MessageControllerTests_AddRecordedMessage
    {
        [Fact]
        public async Task AddRecordedMessage_NoChatCompletionService_Return500()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var messageService = new Mock<IMessageService>();

            var identityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();


            chatModelPicker.Setup(x => x.GetModel(It.IsAny<string>()));

            var controller = new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object);


            Guid userIdentifier = Guid.NewGuid();

            Chat chat = new()
            {
                UniqueIdentifier = Guid.NewGuid(),
                UserIdentifier = userIdentifier,
            };

            chatService.Setup(x => x.GetByUniqueIdentifierWithMessages(It.IsAny<Guid>())).ReturnsAsync(chat);

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            string assistantTextResponse = "test message content";

            chatCompletionService.Setup(x => x.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>())).ReturnsAsync(assistantTextResponse);

            var result = await controller.AddRecordedChat(new AddMessageModel
            {
                ChatIdentifier = Guid.NewGuid(),
                Message = "I am a test"
            });

            Assert.Multiple(() =>
            {
                var actionResult = Assert.IsType<StatusCodeResult>(result);
                Assert.Equal(500, actionResult.StatusCode);
            });
        }

        [Fact]
        public async Task AddRecordedMessage_FailsOnChatCompletion_DoesntAdd()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var messageService = new Mock<IMessageService>();
            var identityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();
            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            chatModelPicker.Setup(x => x.GetModel(It.IsAny<string>())).Returns(chatCompletionService.Object);

            var controller = new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object);

            Guid userIdentifier = Guid.NewGuid();

            chatService.Setup(x => x.GetByUniqueIdentifierWithMessages(It.IsAny<Guid>()))
                .ReturnsAsync(new Chat
                {
                    UserIdentifier = userIdentifier
                });

            chatCompletionService.Setup(x => x.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>())).Throws(new Exception());

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            await Assert.ThrowsAsync<Exception>(() => controller.AddRecordedChat(new AddMessageModel
            {
                ChatIdentifier = Guid.NewGuid(),
                Message = "I am a test"
            }));

            messageService.Verify(x => x.AddRange(It.IsAny<IEnumerable<Message>>()), Times.Never);
        }

        [Fact]
        public async Task AddRecordedMessage_SuccessfulWithExistingChat_SavesMessage()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var messageService = new Mock<IMessageService>();
            var identityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            chatModelPicker.Setup(x => x.GetModel(It.IsAny<string>())).Returns(chatCompletionService.Object);

            var controller = new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object);

            Guid userIdentifier = Guid.NewGuid();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            chatService.Setup(x => x.GetByUniqueIdentifierWithMessages(It.IsAny<Guid>()))
                .ReturnsAsync(new Chat
                {
                    UserIdentifier = userIdentifier,
                });

            chatCompletionService.Setup(x => x.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>())).ReturnsAsync("new message");

            personalInformationCheckerService
              .Setup(x => x.CheckIfStringHasNames(It.IsAny<string>()))
              .Returns(false);

            await controller.AddRecordedChat(new AddMessageModel
            {
                ChatIdentifier = Guid.NewGuid(),
                Message = "I am a test"
            });

            messageService.Verify(x => x.AddRange(It.IsAny<IEnumerable<Message>>()), Times.Once);
        }

        [Fact]
        public async Task AddRecordedMessage_SuccessfulWithNewChat_SavesMessage()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var messageService = new Mock<IMessageService>();
            var identityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            chatModelPicker.Setup(x => x.GetModel(It.IsAny<string>())).Returns(chatCompletionService.Object);

            var controller = new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object);

            Guid userIdentifier = Guid.NewGuid();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            chatService.Setup(x => x.GetByUniqueIdentifierWithMessages(It.IsAny<Guid>()))
                .ReturnsAsync(new Chat
                {
                    UserIdentifier = userIdentifier,
                });

            chatCompletionService.Setup(x => x.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>())).ReturnsAsync("new message");

            await controller.AddRecordedChat(new AddMessageModel
            {
                Message = "I am a test"
            });

            messageService.Verify(x => x.AddRange(It.IsAny<IEnumerable<Message>>()), Times.Once);
        }

        [Fact]
        public async Task AddRecordedMessage_UnauthorizedChat_DoesntSaveMessage()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var messageService = new Mock<IMessageService>();
            var identityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            var chatModelPicker = new Mock<IChatModelPickerService>();
            chatModelPicker.Setup(x => x.GetModel(It.IsAny<string>())).Returns(chatCompletionService.Object);
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            var controller = new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object);


            Guid chatIdentifier = Guid.NewGuid();
            Guid userIdentifier = Guid.NewGuid();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            chatService.Setup(x => x.GetByUniqueIdentifierWithMessages(It.IsAny<Guid>()))
                .ReturnsAsync(new Chat
                {
                    UniqueIdentifier = chatIdentifier,
                    UserIdentifier = Guid.NewGuid(),
                });

            chatCompletionService.Setup(x => x.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>())).ReturnsAsync("new message");

            await controller.AddRecordedChat(new AddMessageModel
            {
                ChatIdentifier = chatIdentifier,
                Message = "I am a test"
            });

            messageService.Verify(x => x.AddRange(It.IsAny<IEnumerable<Message>>()), Times.Never);
        }

        [Fact]
        public async Task AddRecordedMessage_SuccessfulExistingChat_ReturnsLastMessageWithChat()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var messageService = new Mock<IMessageService>();

            var identityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            chatModelPicker.Setup(x => x.GetModel(It.IsAny<string>())).Returns(chatCompletionService.Object);

            personalInformationCheckerService
              .Setup(x => x.CheckIfStringHasNames(It.IsAny<string>()))
            .Returns(false);

            var controller = new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object);

            Guid userIdentifier = Guid.NewGuid();

            Chat chat = new()
            {
                UniqueIdentifier = Guid.NewGuid(),
                UserIdentifier = userIdentifier,
            };

            chatService.Setup(x => x.GetByUniqueIdentifierWithMessages(It.IsAny<Guid>())).ReturnsAsync(chat);

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            string assistantTextResponse = "test message content";

            chatCompletionService
                .Setup(x => x.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>()))
                .ReturnsAsync(assistantTextResponse);

            var result = await controller.AddRecordedChat(new AddMessageModel
            {
                ChatIdentifier = Guid.NewGuid(),
                Message = "I am a test"
            });

            Assert.Multiple(() =>
            {
                var actionResult = Assert.IsType<OkObjectResult>(result);
                var dataResult = Assert.IsType<MessageModel>(actionResult.Value);
                Assert.Equal(dataResult.Chat.UniqueIdentifier, chat.UniqueIdentifier);
                Assert.Equal(dataResult.Text, assistantTextResponse);
                Assert.True(dataResult.IsSystem);
            });
        }

        [Fact]
        public async Task AddRecordedMessage_ContainsCaseLaw_ReturnsObjectWithContainCaseLawTrue()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var messageService = new Mock<IMessageService>();

            var identityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            chatModelPicker.Setup(x => x.GetModel(It.IsAny<string>())).Returns(chatCompletionService.Object);

            personalInformationCheckerService
              .Setup(x => x.CheckIfStringHasNames(It.IsAny<string>()))
            .Returns(false);

            var controller = new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object);

            Guid userIdentifier = Guid.NewGuid();

            Chat chat = new()
            {
                UniqueIdentifier = Guid.NewGuid(),
                UserIdentifier = userIdentifier,
            };

            chatService.Setup(x => x.GetByUniqueIdentifierWithMessages(It.IsAny<Guid>())).ReturnsAsync(chat);

            string assistantTextResponse = "this is a case Lister v Hesley Hall Ltd";

            caseLawDetectionService.Setup(x => x.CheckContainsCaseLaw(assistantTextResponse)).Returns(true);

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);


            chatCompletionService
                .Setup(x => x.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>()))
                .ReturnsAsync(assistantTextResponse);

            var result = await controller.AddRecordedChat(new AddMessageModel
            {
                ChatIdentifier = Guid.NewGuid(),
                Message = "I am a test"
            });

            Assert.Multiple(() =>
            {
                var actionResult = Assert.IsType<OkObjectResult>(result);
                var dataResult = Assert.IsType<MessageModel>(actionResult.Value);
                Assert.True(dataResult.ContainsCaseLaw);
            });
        }

        [Fact]
        public async Task AddRecordedMessage_NoCaseLaw_ReturnsObjectWithContainCaseLawFalse()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var messageService = new Mock<IMessageService>();

            var identityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            chatModelPicker.Setup(x => x.GetModel(It.IsAny<string>())).Returns(chatCompletionService.Object);

            personalInformationCheckerService
              .Setup(x => x.CheckIfStringHasNames(It.IsAny<string>()))
            .Returns(false);

            var controller = new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object);

            Guid userIdentifier = Guid.NewGuid();

            Chat chat = new()
            {
                UniqueIdentifier = Guid.NewGuid(),
                UserIdentifier = userIdentifier,
            };

            chatService.Setup(x => x.GetByUniqueIdentifierWithMessages(It.IsAny<Guid>())).ReturnsAsync(chat);

            string assistantTextResponse = "this is a response";

            caseLawDetectionService.Setup(x => x.CheckContainsCaseLaw(assistantTextResponse)).Returns(false);

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);


            chatCompletionService
                .Setup(x => x.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>()))
                .ReturnsAsync(assistantTextResponse);

            var result = await controller.AddRecordedChat(new AddMessageModel
            {
                ChatIdentifier = Guid.NewGuid(),
                Message = "I am a test"
            });

            Assert.Multiple(() =>
            {
                var actionResult = Assert.IsType<OkObjectResult>(result);
                var dataResult = Assert.IsType<MessageModel>(actionResult.Value);
                Assert.False(dataResult.ContainsCaseLaw);
            });
        }

        [Fact]
        public async Task AddRecordedMessage_UnauthorizedChat_ReturnsUnauthorized()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var messageService = new Mock<IMessageService>();
            var identityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            var chatModelPicker = new Mock<IChatModelPickerService>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            personalInformationCheckerService
                .Setup(x => x.CheckIfStringHasNames(It.IsAny<string>()))
                .Returns(false);

            chatModelPicker.Setup(x => x.GetModel(It.IsAny<string>())).Returns(chatCompletionService.Object);

            var controller = new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object);


            Guid chatIdentifier = Guid.NewGuid();
            Guid userIdentifier = Guid.NewGuid();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            chatService.Setup(x => x.GetByUniqueIdentifierWithMessages(It.IsAny<Guid>()))
                .ReturnsAsync(new Chat
                {
                    UniqueIdentifier = chatIdentifier,
                    UserIdentifier = Guid.NewGuid(),
                });

            chatCompletionService
                .Setup(x => x.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>()))
                .ReturnsAsync("new message");

            var result = await controller.AddRecordedChat(new AddMessageModel
            {
                ChatIdentifier = chatIdentifier,
                Message = "I am a test"
            });

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task AddRecordedMessage_AddFails_ThrowsException()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var messageService = new Mock<IMessageService>();
            var identityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            var chatModelPicker = new Mock<IChatModelPickerService>();

            chatModelPicker.Setup(x => x.GetModel(It.IsAny<string>())).Returns(chatCompletionService.Object);

            personalInformationCheckerService
                .Setup(x => x.CheckIfStringHasNames(It.IsAny<string>()))
            .Returns(false);

            var controller = new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object);

            Guid chatIdentifier = Guid.NewGuid();
            Guid userIdentifier = Guid.NewGuid();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(userIdentifier);

            chatService.Setup(x => x.GetByUniqueIdentifierWithMessages(It.IsAny<Guid>()))
                .ReturnsAsync(new Chat
                {
                    UniqueIdentifier = chatIdentifier,
                    UserIdentifier = userIdentifier,
                });

            chatCompletionService
                .Setup(x => x.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>()))
                .ReturnsAsync("new message");

            messageService
                .Setup(x => x.AddRange(It.IsAny<IEnumerable<Message>>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => controller.AddRecordedChat(new AddMessageModel
            {
                Message = "I am a test"
            }));
        }

        [Fact]
        public async Task AddRecordedMessage_UserMessageContainsName_BadRequestObjectResult()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var messageService = new Mock<IMessageService>();
            var identityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            var chatModelPicker = new Mock<IChatModelPickerService>();

            chatModelPicker.Setup(x => x.GetModel(It.IsAny<string>())).Returns(chatCompletionService.Object);

            string messageText = "This is a test Sam";

            personalInformationCheckerService
                .Setup(pis => pis.CheckIfStringHasNames(messageText))
                .Returns(true);

            var controller = new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object);

            var result = await controller.AddRecordedChat(new AddMessageModel()
            {
                Message = messageText
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddRecordedMessage_SystemMessageContainsName_BadRequestObjectResult()
        {
            var caseLawDetectionService = new Mock<ICaseLawDetectionService>();
            var chatService = new Mock<IChatService>();
            var chatCompletionService = new Mock<IChatCompletionService>();
            var messageService = new Mock<IMessageService>();
            var identityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();
            var personalInformationCheckerService = new Mock<IPersonalInformationCheckerService>();

            var chatModelPicker = new Mock<IChatModelPickerService>();

            chatModelPicker.Setup(x => x.GetModel(It.IsAny<string>())).Returns(chatCompletionService.Object);

            string messageText = "This is a system response Sam";

            chatCompletionService
                .Setup(ccs => ccs.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>()))
                .ReturnsAsync(messageText);

            personalInformationCheckerService.Setup(pis => pis.CheckIfStringHasNames(messageText)).Returns(true);

            var controller = new MessageController(caseLawDetectionService.Object, chatService.Object, chatModelPicker.Object,
                configuration.Object, identityService.Object, messageService.Object, personalInformationCheckerService.Object);

            var result = await controller.AddRecordedChat(new AddMessageModel()
            {
                Message = messageText
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}