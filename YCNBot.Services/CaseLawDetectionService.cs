using YCNBot.Core.Services;

namespace YCNBot.Services
{
    public class CaseLawDetectionService : ICaseLawDetectionService
    {
        public bool CheckContainsCaseLaw(string text)
        {
            return text.Contains(" v ") || text.Contains(" v. ");
        }
    }
}
