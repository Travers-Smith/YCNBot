using Moq;
using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Services;

namespace YCNBot.UnitTest.Services
{
    public class ChatServiceTests
    {
        [Fact]
        public async Task Add_Successful_Saved()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.Chat.AddAsync(It.IsAny<Chat>()));

            await new ChatService(mock.Object).Add(new Chat()
            {
                Name = "Test"
            });

            mock.Verify(p => p.Chat.AddAsync(It.IsAny<Chat>()), Times.Once);
            mock.Verify(p => p.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Add_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.Chat.AddAsync(It.IsAny<Chat>()));

            mock.Setup(x => x.CommitAsync()).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new ChatService(mock.Object).Add(new Chat()
            {
                Name = "Test"
            })
            );
        }

        [Fact]
        public async Task GetAllByUser_Successful_ReturnsChats()
        {
            var mock = new Mock<IUnitOfWork>();

            var chats = new Chat[]
            {
                new Chat()
                {
                    Name = "Test"
                }
            };

            mock.Setup(x => x.Chat.GetAllByUser(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(chats);

            IEnumerable<Chat> result = await new ChatService(mock.Object).GetAllByUser(Guid.NewGuid(), It.IsAny<int>(), It.IsAny<int>());

            Assert.Equal(result, chats);
        }

        [Fact]
        public async Task GetAllByUser_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            var chats = new Chat[]
            {
                new Chat()
                {
                    Name = "Test"
                }
            };

            mock.Setup(x => x.Chat.GetAllByUser(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new ChatService(mock.Object).GetAllByUser(Guid.NewGuid(), It.IsAny<int>(), It.IsAny<int>()));
        }


        [Fact]
        public async Task GetByUniqueIdentifier_Success_ReturnsChat()
        {
            var mock = new Mock<IUnitOfWork>();

            var chat = new Chat();

            mock.Setup(x => x.Chat.GetByUniqueIdentifier(It.IsAny<Guid>())).ReturnsAsync(chat);

            var result = await new ChatService(mock.Object).GetByUniqueIdentifier(Guid.NewGuid());

            Assert.Equal(chat, result);
        }

        [Fact]
        public async Task GetByUniqueIdentifier_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            var chats = new Chat[]
            {
                new Chat()
                {
                    Name = "Test"
                }
            };

            mock.Setup(x => x.Chat.GetByUniqueIdentifier(It.IsAny<Guid>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new ChatService(mock.Object).GetByUniqueIdentifier(Guid.NewGuid()));
        }

        [Fact]
        public async Task GetByUniqueIdentifierWithMessages_Success_ReturnSuccessCode()
        {
            var mock = new Mock<IUnitOfWork>();

            Chat chat = new();

            mock.Setup(x => x.Chat.GetByUniqueIdentifierWithMessages(It.IsAny<Guid>())).ReturnsAsync(chat);

            Chat result = await new ChatService(mock.Object).GetByUniqueIdentifierWithMessages(Guid.NewGuid());

            Assert.Equal(chat, result);
        }

        [Fact]
        public async Task GetByUniqueIdentifierWithMessages_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            Chat[] chats = new Chat[]
            {
                new Chat()
                {
                    Name = "Test"
                }
            };

            mock.Setup(x => x.Chat.GetByUniqueIdentifierWithMessages(It.IsAny<Guid>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new ChatService(mock.Object).GetByUniqueIdentifierWithMessages(Guid.NewGuid()));
        }

        [Fact]
        public async Task GetUsersUsage_Success_ReturnsUsageDictionary()
        {
            var unitOfWork = new Mock<IUnitOfWork>();

            Dictionary<Guid, int> usageDictionary = new();

            int skip = 10;
            int take = 10;

            unitOfWork
                .Setup(x => x.Chat.GetUsersUsage(skip, take))
                .ReturnsAsync(usageDictionary);

            Assert.Equal(usageDictionary, await new ChatService(unitOfWork.Object).GetUsersUsage(skip, take));
        }

        [Fact]
        public async Task GetUsersUsage_Fails_ThrowsException()
        {
            var unitOfWork = new Mock<IUnitOfWork>();

            Dictionary<Guid, int> usageDictionary = new();

            int skip = 10;
            int take = 10;

            unitOfWork
                .Setup(x => x.Chat.GetUsersUsage(skip, take))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new ChatService(unitOfWork.Object).GetUsersUsage(skip, take));
        }

        [Fact]
        public async Task Update_Successful_Saved()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.Chat.Update(It.IsAny<Chat>()));

            await new ChatService(mock.Object).Update(new Chat()
            {
                Name = "Test"
            });

            mock.Verify(p => p.Chat.Update(It.IsAny<Chat>()), Times.Once);
            mock.Verify(p => p.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.Chat.Update(It.IsAny<Chat>()));

            mock.Setup(x => x.CommitAsync()).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new ChatService(mock.Object).Update(new Chat()
            {
                Name = "Test"
            })
            );
        }
    }
}

