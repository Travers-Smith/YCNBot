using Moq;
using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Services;

namespace YCNBot.UnitTest.Services
{
    public class MessageServiceTests
    {
        [Fact]
        public async Task AddRange_Successful_SavesMessage()
        {
            var mock = new Mock<IUnitOfWork>();

            var messages = new Message[]
            {
                new Message()
            };

            mock.Setup(x => x.Message.AddRangeAsync(messages));

            await new MessageService(mock.Object).AddRange(new Message[]
            {
                new Message()
            });

            Assert.Multiple(() =>
            {
                mock.Verify(x => x.Message.AddRangeAsync(It.IsAny<IEnumerable<Message>>()), Times.Once);
                mock.Verify(x => x.CommitAsync(), Times.Once);
            });
        }

        [Fact]
        public async Task AddRange_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.Message.AddRangeAsync(It.IsAny<IEnumerable<Message>>())).Throws(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new MessageService(mock.Object).AddRange(new Message[]
            {
                new Message()
            }));
        }

        [Fact]
        public async Task GetByUniqueIdentifierWithChat_Success_ReturnsChat()
        {
            var mock = new Mock<IUnitOfWork>();

            var message = new Message();

            mock.Setup(x => x.Message.GetByUniqueIdentifierWithChat(It.IsAny<Guid>())).ReturnsAsync(message);

            Message result = await new MessageService(mock.Object).GetByUniqueIdentifierWithChat(Guid.NewGuid());

            Assert.Equal(message, result);
        }

        [Fact]
        public async Task GetByUniqueIdentifierWithChat_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            Message message = new();

            mock.Setup(x => x.Message.GetByUniqueIdentifierWithChat(It.IsAny<Guid>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new MessageService(mock.Object).GetByUniqueIdentifierWithChat(Guid.NewGuid()));
        }

        [Fact]
        public async Task Update_Succcess_UpdatesMessage()
        {
            var mock = new Mock<IUnitOfWork>();

            var message = new Message();

            mock.Setup(x => x.Message.Update(message));

            await new MessageService(mock.Object).Update(message);

            Assert.Multiple(() =>
            {
                mock.Verify(x => x.Message.Update(message), Times.Once);
                mock.Verify(x => x.CommitAsync(), Times.Once);
            });
        }

        [Fact]
        public async Task Update_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            var message = new Message();

            mock.Setup(x => x.Message.Update(message)).Throws(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new MessageService(mock.Object).Update(message));
        }
    }
}
