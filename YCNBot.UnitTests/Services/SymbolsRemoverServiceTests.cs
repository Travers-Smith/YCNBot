using YCNBot.Services;

namespace YCNBot.UnitTest.Services
{
    public class SymbolsRemoverServiceTests
    {
        [Fact]
        public void RemoveSymbols_IncludeSymbols_RemovesSymbols()
        {
            IEnumerable<string> stopwords = new SymbolsRemoverService()
                .RemoveSymbols(new string[]
                {
                    "This",
                    "a",
                    "few",
                    "includes",
                    "Symbols",
                    "like",
                    "!",
                    "#",
                    "@",
                    ",",
                    "."
                });

            string[] symbolsInInput = new string[]
            {
                "!", "@", "#", ",", "."
            };

            Assert.DoesNotContain(stopwords, x => symbolsInInput.Contains(x));
        }

        [Fact]
        public void RemoveSymbols_NoSymbols_StringIsntChanged()
        {
            string[] inputString = new string[] { "This", "a", "few", "includes" };

            IEnumerable<string> stopwords = new SymbolsRemoverService()
                .RemoveSymbols(inputString);

            Assert.Equal(inputString, stopwords);
        }
    }
}
