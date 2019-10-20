using NUnit.Framework;
using Peter.Models.Enums;
using Services.Analyses;
using System;

namespace AnalysesManager.UnitTests.Analyses
{
    public class Service_GetTaz
    {
        [TestCase(0)]
        [TestCase(-1.1)]
        public void WithInvalidClosingPrice_ThrowsArgumentException(decimal closingPrice)
        {
            Assert.Throws<ArgumentException>(() => TechnicalAnalyser.GetTAZ(closingPrice, 1.6m, 1.2m));
        }

        [TestCase(0)]
        [TestCase(-1.8)]
        public void WithInvalidFastSma_ThrowsArgumentException(decimal fastSMA)
        {
            Assert.Throws<ArgumentException>(() => TechnicalAnalyser.GetTAZ(1.2m, fastSMA, 1.5m));
        }

        [TestCase(0)]
        [TestCase(-3.8)]
        public void WithInvalidSlowSma_ThrowsArgumentException(decimal slowSMA)
        {
            Assert.Throws<ArgumentException>(() => TechnicalAnalyser.GetTAZ(5.7m, 2.6m, slowSMA));
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
            Assert.AreEqual(expectedResult, TechnicalAnalyser.GetTAZ(closingPrice, fastSMA, slowSMA));
        }
    }
}
