using Moq;
using YCNBot.Core;
using YCNBot.Core.Services;
using YCNBot.Services;

namespace YCNBot.UnitTest.Services
{
    public class PersonalInformationCheckerServiceTests
    {
        [Fact]
        public void CheckIfStringHasNames_ContainsNames_ReturnsTrue()
        {
            Mock<ISymbolsRemoverService> SymbolsRemove = new();
            Mock<IStopWordRemoverService> stopwordRemover = new();
            Mock<IUnitOfWork> unitOfWork = new();

            IEnumerable<string> names = new List<string> { "Tom" };

            IEnumerable<string> stringWithNoPunc = new string[] { "tom", "is", "in", "a", "test" };
            IEnumerable<string> stringWithNoStopword = new string[] { "tom", "test" };

            SymbolsRemove
                .Setup(x => x.RemoveSymbols(It.IsAny<IEnumerable<string>>()))
                .Returns(stringWithNoPunc);

            stopwordRemover
                .Setup(x => x.RemoveStopWords(It.IsAny<IEnumerable<string>>()))
                .Returns(stringWithNoStopword);

            unitOfWork.Setup(uow => uow.Name.GetNames(It.IsAny<char>(), It.IsAny<char>(), It.IsAny<int>()))
                .Returns(names);

            Assert.True(new PersonalInformationCheckerService(
                SymbolsRemove.Object,
                stopwordRemover.Object,
                unitOfWork.Object).CheckIfStringHasNames("Tom is in a test"));
        }

        [Fact]
        public void CheckIfStringHasNames_NoNames_ReturnsFalse()
        {
            Mock<ISymbolsRemoverService> SymbolsRemove = new();
            Mock<IStopWordRemoverService> stopwordRemover = new();
            Mock<IUnitOfWork> unitOfWork = new();

            IEnumerable<string> names = new List<string> { "Tom" };

            IEnumerable<string> stringWithNoPunc = new string[] { "tom", "is", "in", "a", "test" };
            IEnumerable<string> stringWithNoStopword = new string[] { "tom", "test" };

            SymbolsRemove
                .Setup(x => x.RemoveSymbols(It.IsAny<IEnumerable<string>>()))
                .Returns(stringWithNoPunc);

            stopwordRemover
                .Setup(x => x.RemoveStopWords(It.IsAny<IEnumerable<string>>()))
                .Returns(stringWithNoStopword);

            unitOfWork.Setup(uow => uow.Name.GetNames(It.IsAny<char>(), It.IsAny<char>(), It.IsAny<int>()))
                .Returns(names);

            Assert.True(new PersonalInformationCheckerService(
                SymbolsRemove.Object,
                stopwordRemover.Object,
                unitOfWork.Object).CheckIfStringHasNames("This is in a test"));
        }
    }
}
