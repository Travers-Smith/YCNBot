using YCNBot.Core.Services;

namespace YCNBot.Services
{
    public class SymbolsRemoverService : ISymbolsRemoverService
    {
        public IEnumerable<string> RemoveSymbols(IEnumerable<string> tokens)
        {
            char[] symbolsToRemove = new char[] { ',', '.', '!', '?', '@', '#' };

            return tokens.Select(token => token.Trim(symbolsToRemove));
        }
    }
}
