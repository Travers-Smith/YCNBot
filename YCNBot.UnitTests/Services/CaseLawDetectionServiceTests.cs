using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YCNBot.Services;

namespace Traverssmith.LawBot.UnitTest.Services
{
    public class CaseLawDetectionServiceTests
    {
        [Fact]
        public void CheckContainsCaseLaw_ContainsCaseLaw_ReturnsTrue()
        {
            Assert.True(new CaseLawDetectionService().CheckContainsCaseLaw("this is a case Lister v Hesley Hall Ltd"));
        }

        [Fact]
        public void CheckContainsCaseLaw_NoCaseLaw_ReturnsFalse()
        {
            Assert.False(new CaseLawDetectionService().CheckContainsCaseLaw("there is no case"));
        }
    }
}
