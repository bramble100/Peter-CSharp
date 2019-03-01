using NUnit.Framework;
using Peter.Models.Enums;
using Services.Analyses;
using System;

namespace AnalysesManager.UnitTests
{
    public class ServiceTests_GetTaz
    {
        [TestCase(0, 1, 1)]
        [TestCase(-1, 1, 1)]
        [TestCase(1, 0, 1)]
        [TestCase(1, -1, 1)]
        [TestCase(1, 1, 0)]
        [TestCase(1, 1, -1)]
        public void ThrowsArgumentException(decimal closingPrice, decimal fastSMA, decimal slowSMA)
        {
            Assert.Throws<ArgumentException>(() => Service.GetTAZ(closingPrice, fastSMA, slowSMA));
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
            Assert.AreEqual(expectedResult, Service.GetTAZ(closingPrice, fastSMA, slowSMA));
        }
    }
}
