using NUnit.Framework;
using Peter.Models.Implementations;

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

        [Test]
        public void WithCorrectInput_ReturnsCorrectResult()
        {
            Assert.AreEqual(48, new Peter.Models.Builders.FinancialAnalysisBuilder()
                .SetClosingPrice(60)
                .SetEPS(5)
                .SetMonthsInReport(3)
                .Build().PE);
            Assert.AreEqual(24, new Peter.Models.Builders.FinancialAnalysisBuilder()
                .SetClosingPrice(60)
                .SetEPS(5)
                .SetMonthsInReport(6)
                .Build().PE);
            Assert.AreEqual(16, new Peter.Models.Builders.FinancialAnalysisBuilder()
                .SetClosingPrice(60)
                .SetEPS(5)
                .SetMonthsInReport(9)
                .Build().PE);
            Assert.AreEqual(12, new Peter.Models.Builders.FinancialAnalysisBuilder()
                .SetClosingPrice(60)
                .SetEPS(5)
                .SetMonthsInReport(12)
                .Build().PE);
        }
    }
}
