using FluentAssertions;
using Models.Builders;
using Models.Interfaces;
using NUnit.Framework;
using Services.Analyses;
using System;
using System.Collections.Generic;

namespace AnalysesManager.UnitTests.Analyses
{
    [TestFixture]
    public class Service_ContainsDataWithoutIsin
    {
        [Test]
        public void WithDataWithoutIsin_ReturnsTrue()
        {
            var inputMarketData = new List<IMarketDataEntity>
            {
                new MarketDataEntityBuilder()
                    .SetClosingPrice(3.4m)
                    .SetDateTime(DateTime.Now)
                    .SetIsin(string.Empty)
                    .SetName("Company")
                    .Build()
            };
            Service.ContainsDataWithoutIsin(inputMarketData).Should().BeTrue();
        }

        [Test]
        public void WithDataWithIsin_ReturnsFalse()
        {
            var inputMarketData = new List<IMarketDataEntity>
            {
                new MarketDataEntityBuilder()
                    .SetClosingPrice(3.4m)
                    .SetDateTime(DateTime.Now)
                    .SetIsin("TestIsin")
                    .SetName("Company")
                    .Build(),
                new MarketDataEntityBuilder()
                    .SetClosingPrice(3.4m)
                    .SetDateTime(DateTime.Now)
                    .SetIsin(string.Empty)
                    .SetName("Company")
                    .Build()
            };
            Service.ContainsDataWithoutIsin(inputMarketData).Should().BeTrue();
        }

        [Test]
        public void WithMixedDataWithoutIsin_ReturnsTrue()
        {
            var inputMarketData = new List<IMarketDataEntity>
            {
                new MarketDataEntityBuilder()
                    .SetClosingPrice(3.4m)
                    .SetDateTime(DateTime.Now)
                    .SetIsin(string.Empty)
                    .SetName("Company")
                    .Build()
            };
            Service.ContainsDataWithoutIsin(inputMarketData).Should().BeTrue();
        }
    }
}