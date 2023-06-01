using YCNBot.Core;
using YCNBot.Core.Services;

namespace YCNBot.Services
{
    public class PersonalInformationCheckerService : IPersonalInformationCheckerService
    {
        private readonly ISymbolsRemoverService _symbolsRemoverService;
        private readonly IStopWordRemoverService _stopWordRemoverService;
        private readonly IUnitOfWork _unitOfWork;

        public PersonalInformationCheckerService(ISymbolsRemoverService symbolsRemoverService,
            IStopWordRemoverService stopWordRemoverService, IUnitOfWork unitOfWork)
        {
            _symbolsRemoverService = symbolsRemoverService;
            _stopWordRemoverService = stopWordRemoverService;
            _unitOfWork = unitOfWork;
        }

        public bool CheckIfStringHasNames(string text)
        {
            IEnumerable<string> filteredWords = _stopWordRemoverService.RemoveStopWords(text.ToLower().Split(" "));

            filteredWords = _symbolsRemoverService
                .RemoveSymbols(filteredWords);

            foreach (string word in filteredWords)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    foreach (string? name in _unitOfWork.Name.GetNames(word.First(), word.Last(), word.Length))
                    {
                        if (name.ToLower() == word)
                        {
                            return true;
                        }
                    }
                }

            }

            return false;
        }
    }
}
