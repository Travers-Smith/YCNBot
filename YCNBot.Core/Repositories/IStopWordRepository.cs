namespace YCNBot.Core.Repositories
{
    public interface IStopWordRepository
    {
        HashSet<string> GetAllStopwords();
    }
}