using FluentAssertions;
using Models.Enums;
using NUnit.Framework;
using Services.Analysis;
using System;

namespace AnalysisManager.UnitTests.Analysis
{
    public class Service_GetTrend
    {
        [TestCase(0, 1.8)]
        [TestCase(-1.2, 1)]
        [TestCase(1.9, 0)]
        [TestCase(1.1, -1.2)]
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
            TechnicalAnalyser.GetTrend(fastSMA, slowSMA).Should().Be(expectedResult);
        }
    }
}
