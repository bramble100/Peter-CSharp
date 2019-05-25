using NUnit.Framework;
using Peter.Models.Enums;
using Services.Analyses;
using System;

namespace AnalysesManager.UnitTests
{
    public class ServiceTests_GetTrend
    {
        [TestCase(0, 1)]
        [TestCase(-1, 1)]
        [TestCase(1, 0)]
        [TestCase(1, -1)]
        public void ThrowsArgumentException(decimal fastSMA, decimal slowSMA)
        {
            Assert.Throws<ArgumentException>(() => TechnicalAnalyser.GetTrend(fastSMA, slowSMA));
        }

        [TestCase(3.1, 2.1, Trend.Up)]
        [TestCase(2.1, 3.1, Trend.Down)]
        [TestCase(2.1, 2.1, Trend.Undefined)]
        public void ReturnsCorrectResult(
            decimal fastSMA,
            decimal slowSMA,
            Trend expectedResult)
        {
            Assert.AreEqual(expectedResult, TechnicalAnalyser.GetTrend(fastSMA, slowSMA));
        }
    }
}
