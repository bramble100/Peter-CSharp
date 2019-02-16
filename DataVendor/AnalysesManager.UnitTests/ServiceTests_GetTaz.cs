using AnalysesManager.Services.Implementations;
using NUnit.Framework;
using Peter.Models.Builders;
using Peter.Models.Enums;
using System;

namespace AnalysesManager.UnitTests
{
    public class ServiceTests_GetTaz
    {
        [Test]
        public void ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Service.GetTAZ(null));

            var analysis = new AnalysisBuilder()
                .SetClosingPrice(1)
                .SetFinancialAnalysis(null)
                .SetName("test")
                .SetQtyInBuyingPacket(1)
                .SetTechnicalAnalysis(null)
                .Build();
            Assert.Throws<ArgumentNullException>(() => Service.GetTAZ(analysis));
        }

        [TestCase(0, 1, 1)]
        [TestCase(-1, 1, 1)]
        [TestCase(1, 0, 1)]
        [TestCase(1, -1, 1)]
        [TestCase(1, 1, 0)]
        [TestCase(1, 1, -1)]
        public void ThrowsArgumentException(decimal closingPrice, decimal fastSMA, decimal slowSMA)
        {
            var analysis = new AnalysisBuilder()
                .SetClosingPrice(1)
                .SetFinancialAnalysis(null)
                .SetName("test")
                .SetQtyInBuyingPacket(1)
                .SetTechnicalAnalysis(new TechnicalAnalysisBuilder()
                    .SetFastSMA(1)
                    .SetSlowSMA(1)
                    .Build())
                .Build();
            analysis.ClosingPrice = closingPrice;
            analysis.TechnicalAnalysis.FastSMA = fastSMA;
            analysis.TechnicalAnalysis.SlowSMA = slowSMA;

            Assert.Throws<ArgumentException>(() => Service.GetTAZ(analysis));
        }

        [TestCase(3.1, 2.1, 1.1, TAZ.AboveTAZ)]
        [TestCase(3.1, 1.1, 2.1, TAZ.AboveTAZ)]
        [TestCase(0.1, 2.1, 1.1, TAZ.BelowTAZ)]
        [TestCase(0.1, 1.1, 2.1, TAZ.BelowTAZ)]
        [TestCase(1.6, 2.1, 1.1, TAZ.InTAZ)]
        [TestCase(1.6, 1.1, 2.1, TAZ.InTAZ)]
        [TestCase(2.1, 2.1, 1.1, TAZ.InTAZ)]
        [TestCase(1.1, 2.1, 1.1, TAZ.InTAZ)]
        [TestCase(2.1, 1.1, 2.1, TAZ.InTAZ)]
        [TestCase(1.1, 1.1, 2.1, TAZ.InTAZ)]
        public void ReturnsCorrectResult(
            decimal closingPrice,
            decimal fastSMA,
            decimal slowSMA,
            TAZ expectedResult)
        {
            var analysis = new AnalysisBuilder()
                .SetClosingPrice(closingPrice)
                .SetFinancialAnalysis(null)
                .SetName("test")
                .SetQtyInBuyingPacket(1)
                .SetTechnicalAnalysis(new TechnicalAnalysisBuilder()
                    .SetFastSMA(fastSMA)
                    .SetSlowSMA(slowSMA)
                    .Build())
                .Build();

            Assert.AreEqual(expectedResult, Service.GetTAZ(analysis));
        }
    }
}
