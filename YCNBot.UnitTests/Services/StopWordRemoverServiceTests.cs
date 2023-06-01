using Moq;
using YCNBot.Core;
using YCNBot.Services;

namespace YCNBot.UnitTest.Services
{
    public class StopWordRemoverServiceTests
    {
        [Fact]
        public void RemoveStopWords_IncludeStopWords_RemovesStopWord()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            unitOfWork
                .Setup(x => x.StopWord.GetAllStopwords())
                .Returns(new HashSet<string>
                {
                    "this",
                    "a",
                    "few",
                    "includes"
                });

            IEnumerable<string> stopwords = new StopWordRemoverService(unitOfWork.Object)
                .RemoveStopWords(new string[] { "This", "a", "few", "includes", "stopwords" });

            Assert.Single(stopwords);
            Assert.Equal("stopwords", stopwords.First());
        }

        [Fact]
        public void RemoveStopWords_NoStopWords_RemovesStopWord()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            unitOfWork
                .Setup(x => x.StopWord.GetAllStopwords())
                .Returns(new HashSet<string>
                {
                    "this",
                    "a",
                    "few",
                    "includes"
                });

            string[] inputString = new[]
            {
                "shawn",
                "sam",
                "tom"
            };

            IEnumerable<string> stopwords = new StopWordRemoverService(unitOfWork.Object)
                .RemoveStopWords(inputString);

            Assert.Equal(inputString, stopwords);
        }
    }
}
