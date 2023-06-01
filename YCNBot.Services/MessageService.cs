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

        public async Task<IEnumerable<Tuple<DateTime, int>>> GetDateBreakdown(int previousDays)
        {
            return await _unitOfWork.Message.GetDateBreakdown(previousDays);
        }

        public async Task<Message> GetByUniqueIdentifierWithChat(Guid uniqueIdentifier)
        {
            return await _unitOfWork.Message.GetByUniqueIdentifierWithChat(uniqueIdentifier);
        }

        public async Task<int> GetCount()
        {
            return await _unitOfWork.Message.GetCountAsync();
        }

        public async Task Update(Message message)
        {
            _unitOfWork.Message.Update(message);

            await _unitOfWork.CommitAsync();
        }
    }
}
