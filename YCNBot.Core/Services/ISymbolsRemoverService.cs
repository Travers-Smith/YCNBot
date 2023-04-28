namespace YCNBot.Core.Services
{
    public interface ISymbolsRemoverService
    {
        IEnumerable<string> RemoveSymbols(IEnumerable<string> tokens);
    }
}