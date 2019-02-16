using AnalysesManager.Services.Implementations;
using NUnit.Framework;
using Peter.Models.Builders;
using Peter.Models.Enums;
using System;

namespace AnalysesManager.UnitTests
{
    public class ServiceTests_GetTrend
    {
        [Test]
        public void ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Service.GetTrend(null));
        }

        [TestCase(0, 1)]
        [TestCase(-1, 1)]
        [TestCase(1, 0)]
        [TestCase(1, -1)]
        public void ThrowsArgumentException(decimal fastSMA, decimal slowSMA)
        {
            var analysis = new TechnicalAnalysisBuilder()
                    .SetFastSMA(1)
                    .SetSlowSMA(1)
                    .Build();

            analysis.FastSMA = fastSMA;
            analysis.SlowSMA = slowSMA;

            Assert.Throws<ArgumentException>(() => Service.GetTrend(analysis));
        }

        [TestCase(3.1, 2.1, Trend.Up)]
        [TestCase(2.1, 3.1, Trend.Down)]
        [TestCase(2.1, 2.1, Trend.Undefined)]
        public void ReturnsCorrectResult(
            decimal fastSMA,
            decimal slowSMA,
            Trend expectedResult)
        {
            var analysis = new TechnicalAnalysisBuilder()
                    .SetFastSMA(fastSMA)
                    .SetSlowSMA(slowSMA)
                    .Build();

            Assert.AreEqual(expectedResult, Service.GetTrend(analysis));
        }
    }
}
