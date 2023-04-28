using YCNBot.Core;
using YCNBot.Core.Services;

namespace YCNBot.Services
{
    public class StopWordRemoverService : IStopWordRemoverService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StopWordRemoverService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<string> RemoveStopWords(IEnumerable<string> inputString)
        {
            HashSet<string> stopWords = _unitOfWork.StopWord.GetAllStopwords();

            return inputString
                 .Where(word => !stopWords.Contains(word.ToLower()))
                 .ToArray();
        }
    }
}
