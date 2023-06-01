using Microsoft.Extensions.Configuration;
using YCNBot.Core.Repositories;

namespace YCNBot.Data.Repositories
{
    public class StopWordRepository : IStopWordRepository
    {
        private readonly IConfiguration _configuration;

        public StopWordRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public HashSet<string> GetAllStopwords()
        {
            string? stopWordFilepath = _configuration["StopWordsFilepath"] ??
                throw new Exception("Invalid stopword filepath");

            return new HashSet<string>(File.ReadAllLines(stopWordFilepath));
        }
    }
}
