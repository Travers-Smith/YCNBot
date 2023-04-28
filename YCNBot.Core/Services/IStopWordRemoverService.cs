namespace YCNBot.Core.Services
{
    public interface IStopWordRemoverService
    {
        IEnumerable<string> RemoveStopWords(IEnumerable<string> inputString);
    }
}