using YCNBot.Core;
using YCNBot.Core.Entities;
using YCNBot.Core.Services;

namespace YCNBot.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddRange(IEnumerable<Message> messages)
        {
            await _unitOfWork.Message.AddRangeAsync(messages);

            await _unitOfWork.CommitAsync();
        }

        public async Task<Message> GetByUniqueIdentifierWithChat(Guid uniqueIdentifier)
        {
            return await _unitOfWork.Message.GetByUniqueIdentifierWithChat(uniqueIdentifier);
        }

        public async Task Update(Message message)
        {
            _unitOfWork.Message.Update(message);

            await _unitOfWork.CommitAsync();
        }
    }
}
