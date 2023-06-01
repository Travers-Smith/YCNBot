using Moq;
using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Services;

namespace YCNBot.UnitTest.Services
{
    public class UserFeedbackServiceTests
    {
        [Fact]
        public async Task Add_Success_AddsFeedback()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            UserFeedback userFeedback = new();

            unitOfWork.Setup(x => x.UserFeedback.AddAsync(userFeedback));

            await new UserFeedbackService(unitOfWork.Object).Add(userFeedback);

            unitOfWork.Verify(uow => uow.UserFeedback.AddAsync(userFeedback), Times.Once);

            unitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Add_Failure_ThrowsException()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            UserFeedback userFeedback = new();

            unitOfWork
                .Setup(x => x.UserFeedback.AddAsync(userFeedback))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new UserFeedbackService(unitOfWork.Object).Add(userFeedback));
        }
    }
}
