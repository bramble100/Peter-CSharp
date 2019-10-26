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
using System.Collections.Generic;
using System.Linq;

namespace Services.UnitTests.Analyses
{
    [TestFixture]
    class AnalysisService_TryNewAnalysis
    {
        IAnalysisService service;

        readonly Mock<IFundamentalAnalyser> _mockFundamentalAnalyser;
        readonly Mock<ITechnicalAnalyser> _mockTechnicalAnalyser;

        readonly Mock<IDataVendorService> _mockDatavendorService;

        readonly Mock<IAnalysesRepository> _mockAnalysesRepository;
        readonly Mock<IMarketDataRepository> _mockMarketDataRepository;
        readonly Mock<IRegistryRepository> _mockRegistryRepository;

        readonly Mock<IConfigReader> _mockConfigReader;

        public AnalysisService_TryNewAnalysis()
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
        public void WithAnalysisBuilderReturnsNull_ReturnsFalse()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(1).ToArray();
            var names = TestDataFactory.NewCompanyNames(1).ToArray();
            var marketData = TestDataFactory.NewMarketData(names, 2).ToArray();
            var registryEntry = TestDataFactory.NewRegistryEntries(isins, names).ToArray().FirstOrDefault();

            registryEntry.Name = string.Empty;

            _mockConfigReader.Setup(m => m.Settings.BuyingPacketInEuro).Returns(1000);
            _mockConfigReader.Setup(m => m.Settings.FastMovingAverage).Returns(1);
            _mockConfigReader.Setup(m => m.Settings.SlowMovingAverage).Returns(2);

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
            var success = (service as AnalysisService).TryNewAnalysis(isins[0], out var analysis);

            // Assert
            success.Should().BeFalse();
            analysis.Should().BeNull();
        }

        [Test]
        public void WithInvalidMarketData_ReturnsFalse()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(1).ToArray();
            var names = TestDataFactory.NewCompanyNames(1).ToArray();
            var registryEntry = TestDataFactory.NewRegistryEntries(isins, names).ToArray().FirstOrDefault();
            var marketData = TestDataFactory.NewMarketData(names, 2).ToArray();

            marketData[1].DateTime = marketData[0].DateTime;

            _mockConfigReader.Setup(m => m.Settings.BuyingPacketInEuro).Returns(1000);
            _mockConfigReader.Setup(m => m.Settings.FastMovingAverage).Returns(1);
            _mockConfigReader.Setup(m => m.Settings.SlowMovingAverage).Returns(2);

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
            var success = (service as AnalysisService).TryNewAnalysis(isins[0], out var analysis);

            // Assert
            success.Should().BeFalse();
            analysis.Should().BeNull();
        }

        [Test]
        public void WithNullMarketData_ReturnsFalse()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(5).ToArray();
            var names = TestDataFactory.NewCompanyNames(5).ToArray();
            var registryEntry = TestDataFactory.NewRegistryEntries(isins, names).ToArray().FirstOrDefault();
            IEnumerable<IMarketDataEntity> marketData = null;

            _mockConfigReader.Setup(m => m.Settings.BuyingPacketInEuro).Returns(1000);
            _mockConfigReader.Setup(m => m.Settings.FastMovingAverage).Returns(1);
            _mockConfigReader.Setup(m => m.Settings.SlowMovingAverage).Returns(2);

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
            var success = (service as AnalysisService).TryNewAnalysis(isins[0], out var analysis);

            // Assert
            success.Should().BeFalse();
            analysis.Should().BeNull();
        }

        [Test]
        public void WithNullRegistryEntry_ReturnsFalse()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(5).ToArray();
            var names = TestDataFactory.NewCompanyNames(5).ToArray();
            IRegistryEntry registryEntry = null;
            var marketData = TestDataFactory.NewMarketData(names, 5);

            _mockConfigReader.Setup(m => m.Settings.BuyingPacketInEuro).Returns(1000);
            _mockConfigReader.Setup(m => m.Settings.FastMovingAverage).Returns(1);
            _mockConfigReader.Setup(m => m.Settings.SlowMovingAverage).Returns(2);

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
            var success = (service as AnalysisService).TryNewAnalysis(isins[0], out var analysis);

            // Assert
            success.Should().BeFalse();
            analysis.Should().BeNull();
        }

        [Test]
        public void WithValidData_ReturnsValidAnalysis()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(1).ToArray();
            var names = TestDataFactory.NewCompanyNames(1).ToArray();
            var marketData = TestDataFactory.NewMarketData(names, 2).ToArray();
            var closingPrice = marketData.Where(d => d.DateTime == marketData.Max(d2 => d2.DateTime)).First().ClosingPrice;
            var registryEntry = TestDataFactory.NewRegistryEntries(isins, names).ToArray().First();

            var expectedResult = new AnalysisBuilder()
                .SetClosingPrice(closingPrice)
                .SetName(names.First())
                .SetQtyInBuyingPacket((int)Math.Floor(1000 / closingPrice))
                .SetTechnicalAnalysis(new TechnicalAnalyser().NewAnalysis(marketData, 1, 2))
                .Build();

            _mockConfigReader.Setup(m => m.Settings.BuyingPacketInEuro).Returns(1000);
            _mockConfigReader.Setup(m => m.Settings.FastMovingAverage).Returns(1);
            _mockConfigReader.Setup(m => m.Settings.SlowMovingAverage).Returns(2);

            _mockTechnicalAnalyser
                .Setup(m => m.NewAnalysis(marketData, 1, 2))
                .Returns(new TechnicalAnalyser().NewAnalysis(marketData, 1, 2));
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
            var success = (service as AnalysisService).TryNewAnalysis(isins[0], out var analysis);

            // Assert
            success.Should().BeTrue();
            analysis.Should().NotBeNull();
            analysis.ClosingPrice.Should().Be(expectedResult.ClosingPrice);
            analysis.Name.Should().Be(expectedResult.Name);
            analysis.QtyInBuyingPacket.Should().Be(expectedResult.QtyInBuyingPacket);
            analysis.TechnicalAnalysis.Should().Be(expectedResult.TechnicalAnalysis);
        }

        [Test]
        public void WithoutTechnicalAnalysis_ReturnsFalse()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(1).ToArray();
            var names = TestDataFactory.NewCompanyNames(1).ToArray();
            var marketData = TestDataFactory.NewMarketData(names, 2).ToArray();
            var registryEntry = TestDataFactory.NewRegistryEntries(isins, names).ToArray().First();

            ITechnicalAnalysis technicalAnalysis = null;

            _mockConfigReader.Setup(m => m.Settings.BuyingPacketInEuro).Returns(1000);
            _mockConfigReader.Setup(m => m.Settings.FastMovingAverage).Returns(1);
            _mockConfigReader.Setup(m => m.Settings.SlowMovingAverage).Returns(2);

            _mockTechnicalAnalyser
                .Setup(m => m.NewAnalysis(marketData, 1, 2))
                .Returns(technicalAnalysis);
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
            var success = (service as AnalysisService).TryNewAnalysis(isins[0], out var analysis);

            // Assert
            success.Should().BeFalse();
            analysis.Should().BeNull();
        }

        [Test]
        public void WithoutMarketData_ReturnsFalse()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(1).ToArray();
            var names = TestDataFactory.NewCompanyNames(1).ToArray();
            var marketData = TestDataFactory.NewMarketData(Enumerable.Empty<string>(), 0);
            var registryEntry = TestDataFactory.NewRegistryEntries(isins, names).ToArray().FirstOrDefault();

            _mockConfigReader.Setup(m => m.Settings.BuyingPacketInEuro).Returns(1000);
            _mockConfigReader.Setup(m => m.Settings.FastMovingAverage).Returns(1);
            _mockConfigReader.Setup(m => m.Settings.SlowMovingAverage).Returns(2);

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
            var success = (service as AnalysisService).TryNewAnalysis(isins[0], out var analysis);

            // Assert
            success.Should().BeFalse();
            analysis.Should().BeNull();
        }
    }
}
