using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using YCNBot.Controllers;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;
using YCNBot.Models;

namespace YCNBot.UnitTest.Controllers.ChatControllerTests
{
    public class ChatControllerTests
    {
        [Fact]
        public async Task GetAllByUser_Success_ReturnsData()
        {
            var mockChat = new Mock<IChatService>();
            var mockIdentityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            Guid chatIdentifier = Guid.NewGuid();

            var results = new List<Chat>
            {
                new Chat()
                {
                    UniqueIdentifier = chatIdentifier,
                }
            };

            configuration.SetupGet(x => x["PageSize"]).Returns("100");
            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(Guid.NewGuid());
            mockChat.Setup(x => x.GetAllByUser(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(results);

            IActionResult result = await new ChatController(mockChat.Object, configuration.Object, mockIdentityService.Object).GetAllByUser(1);

            Assert.Multiple(() =>
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsAssignableFrom<IEnumerable<ChatModel>>(okResult.Value);
                Assert.Equal(returnValue.Count(), results.Count());
                Assert.Equal(returnValue.Select(x => x.UniqueIdentifier).First(), results.Select(x => x.UniqueIdentifier).First());
            });
        }

        [Fact]
        public async Task GetAllByUser_DataFetchFails_ThrowsException()
        {
            var mockChat = new Mock<IChatService>();
            var mockIdentityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            Guid chatIdentifier = Guid.NewGuid();

            List<Chat> results = new()
            {
                new Chat()
                {
                    UniqueIdentifier = chatIdentifier,
                }
            };

            configuration.SetupGet(x => x["PageSize"]).Returns("100");
            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(Guid.NewGuid());
            mockChat.Setup(x => x.GetAllByUser(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new ChatController(mockChat.Object, configuration.Object, mockIdentityService.Object).GetAllByUser(1));

        }

        [Fact]
        public async Task GetAllByUser_NotLoggedIn_ReturnUnauthorized()
        {
            var mockChat = new Mock<IChatService>();
            var mockIdentityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            configuration.SetupGet(x => x["PageSize"]).Returns("100");

            IActionResult result = await new ChatController(mockChat.Object, configuration.Object, mockIdentityService.Object).GetAllByUser(1);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetByChatIdentifier_Success_ReturnsCorrectChat()
        {
            var mockChat = new Mock<IChatService>();
            var configuration = new Mock<IConfiguration>();
            var mockIdentityService = new Mock<IIdentityService>();

            Guid userGuid = Guid.NewGuid();

            Guid chatIdentifier = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(userGuid);

            Chat chat = new()
            {
                UserIdentifier = userGuid,
                UniqueIdentifier = chatIdentifier
            };

            mockChat.Setup(x => x.GetByUniqueIdentifierWithMessages(It.IsAny<Guid>())).ReturnsAsync(chat);

            IActionResult result = await new ChatController(mockChat.Object, configuration.Object, mockIdentityService.Object).GetByChatIdentifier(Guid.NewGuid());

            Assert.Multiple(() =>
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<ChatModel>(okResult.Value);
                Assert.Equal(returnValue.UniqueIdentifier, chat.UniqueIdentifier);
            });
        }

        [Fact]
        public async Task GetByChatIdentifier_FailsToGetChat_ThrowsException()
        {
            var mockChat = new Mock<IChatService>();
            var mockIdentityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            Guid userGuid = Guid.NewGuid();

            Guid chatIdentifier = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(userGuid);

            Chat chat = new()
            {
                UserIdentifier = userGuid,
                UniqueIdentifier = chatIdentifier
            };

            mockChat.Setup(x => x.GetByUniqueIdentifierWithMessages(It.IsAny<Guid>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new ChatController(mockChat.Object, configuration.Object, mockIdentityService.Object).GetByChatIdentifier(Guid.NewGuid()));
        }

        [Fact]
        public async Task GetByChatIdentifier_UnauthorizedUser_ReturnUnauthorized()
        {
            var mockChat = new Mock<IChatService>();
            var mockIdentityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            Guid userGuid = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(userGuid);

            Chat chatFromOtherUser = new Chat
            {
                UserIdentifier = Guid.NewGuid()
            };

            mockChat.Setup(x => x.GetByUniqueIdentifierWithMessages(It.IsAny<Guid>())).ReturnsAsync(chatFromOtherUser);

            IActionResult result = await new ChatController(mockChat.Object, configuration.Object, mockIdentityService.Object).GetByChatIdentifier(Guid.NewGuid());

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Delete_Success_ReturnSuccess()
        {
            var mockChat = new Mock<IChatService>();
            var mockIdentityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            Guid userGuid = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(userGuid);
            mockChat.Setup(x => x.GetByUniqueIdentifier(It.IsAny<Guid>())).ReturnsAsync(new Chat
            {
                UserIdentifier = userGuid
            });

            IActionResult result = await new ChatController(mockChat.Object, configuration.Object, mockIdentityService.Object).Delete(Guid.NewGuid());

            Assert.Multiple(() =>
            {
                var statusResult = Assert.IsType<StatusCodeResult>(result);
                Assert.Equal(204, statusResult.StatusCode);
            });
        }

        [Fact]
        public async Task Delete_DeleteFails_ThrowsException()
        {
            var mockChat = new Mock<IChatService>();
            var mockIdentityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            Guid userGuid = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(userGuid);

            mockChat.Setup(x => x.GetByUniqueIdentifier(It.IsAny<Guid>())).ReturnsAsync(new Chat
            {
                UserIdentifier = userGuid
            });

            mockChat.Setup(x => x.Update(It.IsAny<Chat>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new ChatController(mockChat.Object, configuration.Object, mockIdentityService.Object).Delete(Guid.NewGuid()));

        }

        [Fact]
        public async Task Delete_UnauthorizedUser_ReturnUnauthorized()
        {
            var mockChat = new Mock<IChatService>();
            var mockIdentityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            Guid userGuid = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(userGuid);

            Chat chatFromOtherUser = new Chat
            {
                UserIdentifier = Guid.NewGuid()
            };

            mockChat.Setup(x => x.GetByUniqueIdentifier(It.IsAny<Guid>())).ReturnsAsync(chatFromOtherUser);

            IActionResult result = await new ChatController(mockChat.Object, configuration.Object, mockIdentityService.Object).Delete(Guid.NewGuid());

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Delete_UnauthorizedUser_DoesntUpdate()
        {
            var mockChat = new Mock<IChatService>();
            var mockIdentityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            Guid userGuid = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(userGuid);

            Chat chatFromOtherUser = new()
            {
                UserIdentifier = Guid.NewGuid()
            };

            mockChat.Setup(x => x.GetByUniqueIdentifier(It.IsAny<Guid>())).ReturnsAsync(chatFromOtherUser);

            IActionResult result = await new ChatController(mockChat.Object, configuration.Object, mockIdentityService.Object).Delete(Guid.NewGuid());

            mockChat.Verify(x => x.Update(It.IsAny<Chat>()), Times.Never);
        }

        [Fact]
        public async Task Update_Success_ReturnSuccessCode()
        {
            var mockChat = new Mock<IChatService>();
            var mockIdentityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            Guid userGuid = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(userGuid);
            mockChat.Setup(x => x.GetByUniqueIdentifier(It.IsAny<Guid>())).ReturnsAsync(new Chat
            {
                UserIdentifier = userGuid
            });

            IActionResult result = await new ChatController(mockChat.Object, configuration.Object, mockIdentityService.Object).Update(new UpdateChatModel());

            Assert.Multiple(() =>
            {
                var statusResult = Assert.IsType<StatusCodeResult>(result);
                Assert.Equal(204, statusResult.StatusCode);
            });
        }

        [Fact]
        public async Task Update_UpdateFails_ThrowsException()
        {
            var mockChat = new Mock<IChatService>();
            var mockIdentityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            Guid userGuid = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(userGuid);

            mockChat.Setup(x => x.GetByUniqueIdentifier(It.IsAny<Guid>())).ReturnsAsync(new Chat
            {
                UserIdentifier = userGuid
            });

            mockChat.Setup(x => x.Update(It.IsAny<Chat>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new ChatController(mockChat.Object, configuration.Object, mockIdentityService.Object).Update(new UpdateChatModel()));
        }

        [Fact]
        public async Task Update_UnauthorizedUser_ReturnUnauthorized()
        {
            var mockChat = new Mock<IChatService>();
            var mockIdentityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            Guid userGuid = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(userGuid);

            Chat chatFromOtherUser = new()
            {
                UserIdentifier = Guid.NewGuid()
            };

            mockChat.Setup(x => x.GetByUniqueIdentifier(It.IsAny<Guid>())).ReturnsAsync(chatFromOtherUser);

            IActionResult result = await new ChatController(mockChat.Object, configuration.Object, mockIdentityService.Object).Update(new UpdateChatModel());

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Update_UnauthorizedUser_DoesntUpdate()
        {
            var mockChat = new Mock<IChatService>();
            var mockIdentityService = new Mock<IIdentityService>();
            var configuration = new Mock<IConfiguration>();

            Guid userGuid = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(userGuid);

            Chat chatFromOtherUser = new()
            {
                UserIdentifier = Guid.NewGuid()
            };

            mockChat.Setup(x => x.GetByUniqueIdentifier(It.IsAny<Guid>())).ReturnsAsync(chatFromOtherUser);

            IActionResult result = await new ChatController(mockChat.Object, configuration.Object, mockIdentityService.Object).Update(new UpdateChatModel());

            mockChat.Verify(x => x.Update(It.IsAny<Chat>()), Times.Never);
        }
    }
}