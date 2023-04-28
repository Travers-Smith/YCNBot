using Microsoft.Extensions.Configuration;
using YCNBot.Core.Repositories;

namespace YCNBot.Data.Repositories
{
    public class NameRepository : INameRepository
    {
        private readonly IConfiguration _configuration;

        public NameRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<string> GetNames(char firstLetter, char lastCharacter, int length)
        {
            string fileName = _configuration["SplitNamesDirectoryPath"] + $"{firstLetter}-{lastCharacter}-{length}.txt";

            if (File.Exists(fileName))
            {
                using StreamReader srr = new(fileName);

                string? line;

                while ((line = srr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
