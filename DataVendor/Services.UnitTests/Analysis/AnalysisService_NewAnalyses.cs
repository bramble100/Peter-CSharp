using FluentAssertions;
using Infrastructure.Config;
using Models.Builders;
using Models.Interfaces;
using Moq;
using NUnit.Framework;
using Repositories.Interfaces;
using Services.Analysis;
using Services.DataVendor;
using System;
using System.Linq;

namespace Services.UnitTests.Analyses
{
    [TestFixture]
    class AnalysisService_NewAnalyses
    {
        IAnalysisService service;

        readonly Mock<IFundamentalAnalyser> _mockFundamentalAnalyser;
        readonly Mock<ITechnicalAnalyser> _mockTechnicalAnalyser;

        readonly Mock<IDataVendorService> _mockDatavendorService;

        readonly Mock<IAnalysesRepository> _mockAnalysesRepository;
        readonly Mock<IMarketDataRepository> _mockMarketDataRepository;
        readonly Mock<IRegistryRepository> _mockRegistryRepository;

        readonly Mock<IConfigReader> _mockConfigReader;

        const int slowMovingAverageDayCount = 21;
        const int fastMovingAverageDayCount = 7;

        public AnalysisService_NewAnalyses()
        {
            _mockFundamentalAnalyser = new Mock<IFundamentalAnalyser>();
            _mockTechnicalAnalyser = new Mock<ITechnicalAnalyser>();

            _mockDatavendorService = new Mock<IDataVendorService>();

            _mockAnalysesRepository = new Mock<IAnalysesRepository>();
            _mockMarketDataRepository = new Mock<IMarketDataRepository>();
            _mockRegistryRepository = new Mock<IRegistryRepository>();

            _mockConfigReader = new Mock<IConfigReader>();
        }

        [Test]
        public void WithoutIsins_ReturnNoAnalyses()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(0).ToArray();
            var names = TestDataFactory.NewCompanyNames(1).ToArray();
            var marketData = TestDataFactory.NewMarketData(names, slowMovingAverageDayCount).ToArray();
            var closingPrice = marketData.Where(d => d.DateTime == marketData.Max(d2 => d2.DateTime)).First().ClosingPrice;

            _mockConfigReader.Setup(m => m.Settings.BuyingPacketInEuro).Returns(1000);
            _mockConfigReader.Setup(m => m.Settings.FastMovingAverage).Returns(fastMovingAverageDayCount);
            _mockConfigReader.Setup(m => m.Settings.SlowMovingAverage).Returns(slowMovingAverageDayCount);

            _mockDatavendorService.Setup(m => m.FindIsinsWithLatestMarketData()).Returns(isins);

            service = new AnalysisService(
                _mockFundamentalAnalyser.Object,
                _mockTechnicalAnalyser.Object,
                _mockDatavendorService.Object,
                _mockAnalysesRepository.Object,
                _mockMarketDataRepository.Object,
                _mockRegistryRepository.Object,
                _mockConfigReader.Object);

            // Act
            var analyses = service.NewAnalyses().ToDictionary(a => a.Key, a => a.Value);

            // Assert
            analyses.Should().BeEmpty();
        }

        [Test]
        public void WithOneCompanyDataInRepos_ReturnsOneValidAnalysis()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(1).ToArray();
            var names = TestDataFactory.NewCompanyNames(1).ToArray();
            var marketData = TestDataFactory.NewMarketData(names, slowMovingAverageDayCount).ToArray();
            var closingPrice = marketData.Where(d => d.DateTime == marketData.Max(d2 => d2.DateTime)).First().ClosingPrice;
            var registryEntry = TestDataFactory.NewRegistryEntries(isins, names).ToArray().First();

            var expectedResult = new AnalysisBuilder()
                .SetClosingPrice(closingPrice)
                .SetName(names.First())
                .SetQtyInBuyingPacket((int)Math.Floor(1000 / closingPrice))
                .SetTechnicalAnalysis(new TechnicalAnalyser().NewAnalysis(marketData, fastMovingAverageDayCount, slowMovingAverageDayCount))
                .Build();

            _mockConfigReader.Setup(m => m.Settings.BuyingPacketInEuro).Returns(1000);
            _mockConfigReader.Setup(m => m.Settings.FastMovingAverage).Returns(fastMovingAverageDayCount);
            _mockConfigReader.Setup(m => m.Settings.SlowMovingAverage).Returns(slowMovingAverageDayCount);

            _mockTechnicalAnalyser
                .Setup(m => m.NewAnalysis(marketData, fastMovingAverageDayCount, slowMovingAverageDayCount))
                .Returns(new TechnicalAnalyser().NewAnalysis(marketData, fastMovingAverageDayCount, slowMovingAverageDayCount));
            _mockDatavendorService.Setup(m => m.FindIsinsWithLatestMarketData()).Returns(isins);
            _mockDatavendorService.Setup(m => m.FindMarketDataByIsin(isins[0])).Returns(marketData);
            _mockRegistryRepository.Setup(m => m.FindByIsin(isins[0])).Returns(registryEntry);

            service = new AnalysisService(
                _mockFundamentalAnalyser.Object,
                _mockTechnicalAnalyser.Object,
                _mockDatavendorService.Object,
                _mockAnalysesRepository.Object,
                _mockMarketDataRepository.Object,
                _mockRegistryRepository.Object,
                _mockConfigReader.Object);

            // Act
            var analyses = (service as AnalysisService).NewAnalyses().ToDictionary(a => a.Key, a => a.Value);
            var analysis = analyses[isins[0]];

            // Assert
            analyses.Keys.Count.Should().Be(1);
            analysis.ClosingPrice.Should().Be(expectedResult.ClosingPrice);
            analysis.Name.Should().Be(expectedResult.Name);
            analysis.QtyInBuyingPacket.Should().Be(expectedResult.QtyInBuyingPacket);
            analysis.TechnicalAnalysis.Should().Be(expectedResult.TechnicalAnalysis);
        }

        [Test]
        public void WithTwoCompanyDataInRepos_ReturnsTwoValidAnalyses()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(2).ToArray();
            var names = TestDataFactory.NewCompanyNames(2).ToArray();
            var marketData = TestDataFactory.NewMarketData(names, slowMovingAverageDayCount).ToArray();
            var marketDataGroup1 = marketData.Where(d => string.Equals(names[0], d.Name)).ToArray();
            var marketDataGroup2 = marketData.Where(d => string.Equals(names[1], d.Name)).ToArray();

            var closingPrices = new decimal[]
            {
                marketDataGroup1
                    .Where(d => d.DateTime == marketData.Max(d2 => d2.DateTime))
                    .First()
                    .ClosingPrice,
                marketDataGroup2
                    .Where(d => d.DateTime == marketData.Max(d2 => d2.DateTime))
                    .First()
                    .ClosingPrice
            };

            var registryEntries = TestDataFactory.NewRegistryEntries(isins, names).ToArray();

            var expectedResults = new IAnalysis[]
            {
                new AnalysisBuilder()
                    .SetClosingPrice(closingPrices[0])
                    .SetName(names[0])
                    .SetQtyInBuyingPacket((int)Math.Floor(1000 / closingPrices[0]))
                    .SetTechnicalAnalysis(new TechnicalAnalyser().NewAnalysis(marketDataGroup1, fastMovingAverageDayCount, slowMovingAverageDayCount))
                    .Build(),
                new AnalysisBuilder()
                    .SetClosingPrice(closingPrices[1])
                    .SetName(names[1])
                    .SetQtyInBuyingPacket((int)Math.Floor(1000 / closingPrices[1]))
                    .SetTechnicalAnalysis(new TechnicalAnalyser().NewAnalysis(marketDataGroup2, fastMovingAverageDayCount, slowMovingAverageDayCount))
                    .Build()
            };

            _mockConfigReader.Setup(m => m.Settings.BuyingPacketInEuro).Returns(1000);
            _mockConfigReader.Setup(m => m.Settings.FastMovingAverage).Returns(fastMovingAverageDayCount);
            _mockConfigReader.Setup(m => m.Settings.SlowMovingAverage).Returns(slowMovingAverageDayCount);

            _mockTechnicalAnalyser
                .Setup(m => m.NewAnalysis(marketDataGroup1, fastMovingAverageDayCount, slowMovingAverageDayCount))
                .Returns(new TechnicalAnalyser().NewAnalysis(marketDataGroup1, fastMovingAverageDayCount, slowMovingAverageDayCount));
            _mockTechnicalAnalyser
                .Setup(m => m.NewAnalysis(marketDataGroup2, fastMovingAverageDayCount, slowMovingAverageDayCount))
                .Returns(new TechnicalAnalyser().NewAnalysis(marketDataGroup2, fastMovingAverageDayCount, slowMovingAverageDayCount));
            _mockDatavendorService.Setup(m => m.FindIsinsWithLatestMarketData()).Returns(isins);
            _mockDatavendorService.Setup(m => m.FindMarketDataByIsin(isins[0])).Returns(marketDataGroup1);
            _mockDatavendorService.Setup(m => m.FindMarketDataByIsin(isins[1])).Returns(marketDataGroup2);
            _mockRegistryRepository.Setup(m => m.FindByIsin(isins[0])).Returns(registryEntries[0]);
            _mockRegistryRepository.Setup(m => m.FindByIsin(isins[1])).Returns(registryEntries[1]);

            service = new AnalysisService(
                _mockFundamentalAnalyser.Object,
                _mockTechnicalAnalyser.Object,
                _mockDatavendorService.Object,
                _mockAnalysesRepository.Object,
                _mockMarketDataRepository.Object,
                _mockRegistryRepository.Object,
                _mockConfigReader.Object);

            // Act
            var analyses = (service as AnalysisService).NewAnalyses().ToDictionary(a => a.Key, a => a.Value);

            // Assert
            analyses.Keys.Count.Should().Be(2);

            analyses[isins[0]].ClosingPrice.Should().Be(expectedResults[0].ClosingPrice);
            analyses[isins[0]].Name.Should().Be(expectedResults[0].Name);
            analyses[isins[0]].QtyInBuyingPacket.Should().Be(expectedResults[0].QtyInBuyingPacket);
            analyses[isins[0]].TechnicalAnalysis.Should().Be(expectedResults[0].TechnicalAnalysis);

            analyses[isins[1]].ClosingPrice.Should().Be(expectedResults[1].ClosingPrice);
            analyses[isins[1]].Name.Should().Be(expectedResults[1].Name);
            analyses[isins[1]].QtyInBuyingPacket.Should().Be(expectedResults[1].QtyInBuyingPacket);
            analyses[isins[1]].TechnicalAnalysis.Should().Be(expectedResults[1].TechnicalAnalysis);
        }

        [Test]
        public void WithTwoCompanyDataInReposAndExcessiveDays_ReturnsTwoValidAnalyses()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(2).ToArray();
            var names = TestDataFactory.NewCompanyNames(2).ToArray();
            var marketData = TestDataFactory.NewMarketData(names, slowMovingAverageDayCount * 2).ToArray();
            var marketDataGroup1 = marketData.Where(d => string.Equals(names[0], d.Name)).ToArray();
            var marketDataGroup2 = marketData.Where(d => string.Equals(names[1], d.Name)).ToArray();

            var closingPrices = new decimal[]
            {
                marketDataGroup1
                    .Where(d => d.DateTime == marketData.Max(d2 => d2.DateTime))
                    .First()
                    .ClosingPrice,
                marketDataGroup2
                    .Where(d => d.DateTime == marketData.Max(d2 => d2.DateTime))
                    .First()
                    .ClosingPrice
            };

            var registryEntries = TestDataFactory.NewRegistryEntries(isins, names).ToArray();

            var expectedResults = new IAnalysis[]
            {
                new AnalysisBuilder()
                    .SetClosingPrice(closingPrices[0])
                    .SetName(names[0])
                    .SetQtyInBuyingPacket((int)Math.Floor(1000 / closingPrices[0]))
                    .SetTechnicalAnalysis(new TechnicalAnalyser().NewAnalysis(marketDataGroup1, fastMovingAverageDayCount, slowMovingAverageDayCount))
                    .Build(),
                new AnalysisBuilder()
                    .SetClosingPrice(closingPrices[1])
                    .SetName(names[1])
                    .SetQtyInBuyingPacket((int)Math.Floor(1000 / closingPrices[1]))
                    .SetTechnicalAnalysis(new TechnicalAnalyser().NewAnalysis(marketDataGroup2, fastMovingAverageDayCount, slowMovingAverageDayCount))
                    .Build()
            };

            _mockConfigReader.Setup(m => m.Settings.BuyingPacketInEuro).Returns(1000);
            _mockConfigReader.Setup(m => m.Settings.FastMovingAverage).Returns(fastMovingAverageDayCount);
            _mockConfigReader.Setup(m => m.Settings.SlowMovingAverage).Returns(slowMovingAverageDayCount);

            _mockTechnicalAnalyser
                .Setup(m => m.NewAnalysis(marketDataGroup1, fastMovingAverageDayCount, slowMovingAverageDayCount))
                .Returns(new TechnicalAnalyser().NewAnalysis(marketDataGroup1, fastMovingAverageDayCount, slowMovingAverageDayCount));
            _mockTechnicalAnalyser
                .Setup(m => m.NewAnalysis(marketDataGroup2, fastMovingAverageDayCount, slowMovingAverageDayCount))
                .Returns(new TechnicalAnalyser().NewAnalysis(marketDataGroup2, fastMovingAverageDayCount, slowMovingAverageDayCount));
            _mockDatavendorService.Setup(m => m.FindIsinsWithLatestMarketData()).Returns(isins);
            _mockDatavendorService.Setup(m => m.FindMarketDataByIsin(isins[0])).Returns(marketDataGroup1);
            _mockDatavendorService.Setup(m => m.FindMarketDataByIsin(isins[1])).Returns(marketDataGroup2);
            _mockRegistryRepository.Setup(m => m.FindByIsin(isins[0])).Returns(registryEntries[0]);
            _mockRegistryRepository.Setup(m => m.FindByIsin(isins[1])).Returns(registryEntries[1]);

            service = new AnalysisService(
                _mockFundamentalAnalyser.Object,
                _mockTechnicalAnalyser.Object,
                _mockDatavendorService.Object,
                _mockAnalysesRepository.Object,
                _mockMarketDataRepository.Object,
                _mockRegistryRepository.Object,
                _mockConfigReader.Object);

            // Act
            var analyses = (service as AnalysisService).NewAnalyses().ToDictionary(a => a.Key, a => a.Value);

            // Assert
            analyses[isins[0]].ClosingPrice.Should().Be(expectedResults[0].ClosingPrice);
            analyses[isins[0]].Name.Should().Be(expectedResults[0].Name);
            analyses[isins[0]].QtyInBuyingPacket.Should().Be(expectedResults[0].QtyInBuyingPacket);
            analyses[isins[0]].TechnicalAnalysis.Should().Be(expectedResults[0].TechnicalAnalysis);

            analyses[isins[1]].ClosingPrice.Should().Be(expectedResults[1].ClosingPrice);
            analyses[isins[1]].Name.Should().Be(expectedResults[1].Name);
            analyses[isins[1]].QtyInBuyingPacket.Should().Be(expectedResults[1].QtyInBuyingPacket);
            analyses[isins[1]].TechnicalAnalysis.Should().Be(expectedResults[1].TechnicalAnalysis);
        }

        [Test]
        public void WithOneIsinAndTwoCompanyDataInReposAndExcessiveDays_ReturnsTwoValidAnalyses()
        {
            // Arrange
            var isinsInitial = TestDataFactory.NewIsins(10).ToArray();
            var names = TestDataFactory.NewCompanyNames(10).ToArray();
            var marketData = TestDataFactory.NewMarketData(names, slowMovingAverageDayCount * 9).ToArray();
            var marketDataGroup = marketData.Where(d => string.Equals(names[0], d.Name)).ToArray();
            var isins = new string[] { isinsInitial.OrderBy(i => Guid.NewGuid()).First() };

            var closingPrices = new decimal[]
            {
                marketDataGroup
                    .Where(d => d.DateTime == marketData.Max(d2 => d2.DateTime))
                    .First()
                    .ClosingPrice
            };

            var registryEntries = TestDataFactory.NewRegistryEntries(isins, names).ToArray();

            var expectedResults = new IAnalysis[]
            {
                new AnalysisBuilder()
                    .SetClosingPrice(closingPrices[0])
                    .SetName(names[0])
                    .SetQtyInBuyingPacket((int)Math.Floor(1000 / closingPrices[0]))
                    .SetTechnicalAnalysis(new TechnicalAnalyser().NewAnalysis(marketDataGroup, fastMovingAverageDayCount, slowMovingAverageDayCount))
                    .Build()
            };

            _mockConfigReader.Setup(m => m.Settings.BuyingPacketInEuro).Returns(1000);
            _mockConfigReader.Setup(m => m.Settings.FastMovingAverage).Returns(fastMovingAverageDayCount);
            _mockConfigReader.Setup(m => m.Settings.SlowMovingAverage).Returns(slowMovingAverageDayCount);

            _mockTechnicalAnalyser
                .Setup(m => m.NewAnalysis(marketDataGroup, fastMovingAverageDayCount, slowMovingAverageDayCount))
                .Returns(new TechnicalAnalyser().NewAnalysis(marketDataGroup, fastMovingAverageDayCount, slowMovingAverageDayCount));
            _mockDatavendorService.Setup(m => m.FindIsinsWithLatestMarketData()).Returns(isins);
            _mockDatavendorService.Setup(m => m.FindMarketDataByIsin(isins[0])).Returns(marketDataGroup);
            _mockRegistryRepository.Setup(m => m.FindByIsin(isins[0])).Returns(registryEntries[0]);

            service = new AnalysisService(
                _mockFundamentalAnalyser.Object,
                _mockTechnicalAnalyser.Object,
                _mockDatavendorService.Object,
                _mockAnalysesRepository.Object,
                _mockMarketDataRepository.Object,
                _mockRegistryRepository.Object,
                _mockConfigReader.Object);

            // Act
            var analyses = (service as AnalysisService).NewAnalyses().ToDictionary(a => a.Key, a => a.Value);

            // Assert
            analyses.Keys.Count.Should().Be(1);
            analyses[isins[0]].ClosingPrice.Should().Be(expectedResults[0].ClosingPrice);
            analyses[isins[0]].Name.Should().Be(expectedResults[0].Name);
            analyses[isins[0]].QtyInBuyingPacket.Should().Be(expectedResults[0].QtyInBuyingPacket);
            analyses[isins[0]].TechnicalAnalysis.Should().Be(expectedResults[0].TechnicalAnalysis);
        }
    }
}
