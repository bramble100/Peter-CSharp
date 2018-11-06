using NUnit.Framework;

namespace Models.UnitTests
{
    [TestFixture]
    public class FinancialAnalysisBuilder
    {
        [Test]
        public void WithMissingEPS_ReturnsNull()
        {
            Assert.IsNull(new Peter.Models.Builders.FinancialAnalysisBuilder()
                .SetClosingPrice(10)
                .SetEPS(null)
                .SetMonthsInReport(3)
                .Build());
        }

        [Test]
        public void WithMissingMonthsInReport_ReturnsNull()
        {
            Assert.IsNull(new Peter.Models.Builders.FinancialAnalysisBuilder()
                .SetClosingPrice(10)
                .SetEPS(5)
                .SetMonthsInReport(null)
                .Build());
        }

        [TestCase(3, 3)]
        [TestCase(6, 6)]
        [TestCase(9, 9)]
        [TestCase(12, 12)]
        public void WithCorrectInput_ReturnsCorrectResult(int expectedPE, int months)
        {
            Assert.AreEqual(expectedPE, new Peter.Models.Builders.FinancialAnalysisBuilder()
                .SetClosingPrice(60)
                .SetEPS(5)
                .SetMonthsInReport(months)
                .Build().PE);
        }
    }
}
