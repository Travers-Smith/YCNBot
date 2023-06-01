namespace YCNBot.Core.Repositories
{
    public interface INameRepository
    {
        IEnumerable<string> GetNames(char firstLetter, char lastCharacter, int length);
    }
}