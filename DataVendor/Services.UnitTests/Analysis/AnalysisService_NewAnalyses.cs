using FluentAssertions;
using Infrastructure.Config;
using Models.Builders;
using Models.Interfaces;
using Moq;
using NUnit.Framework;
using Repositories.Interfaces;
using Services.Analysis;
using Services.DataVendor;
using System.Collections.Generic;
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
        public void WithEmptyRepos_ReturnNoAnalyses()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(0).ToArray();
            _mockConfigReader.Setup(m => m.Settings.BuyingPacketInEuro).Returns(1000);
            _mockConfigReader.Setup(m => m.Settings.FastMovingAverage).Returns(1);
            _mockConfigReader.Setup(m => m.Settings.SlowMovingAverage).Returns(2);

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
            var result = service.NewAnalyses().ToDictionary(a => a.Key, a => a.Value);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void WithOneCompanyDataInRepos_ReturnsOneValidAnalysis()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(1).ToArray();
            var names = TestDataFactory.NewCompanyNames(1).ToArray();
            var marketData = TestDataFactory.NewMarketData(names, 2).ToArray();

            _mockConfigReader.Setup(m => m.Settings.BuyingPacketInEuro).Returns(1000);
            _mockConfigReader.Setup(m => m.Settings.FastMovingAverage).Returns(1);
            _mockConfigReader.Setup(m => m.Settings.SlowMovingAverage).Returns(2);

            _mockDatavendorService.Setup(m => m.FindIsinsWithLatestMarketData()).Returns(isins);

            service = new AnalysisService(
                _mockFundamentalAnalyser.Object,
                _mockTechnicalAnalyser.Object,
                _mockDatavendorService.Object,
                _mockAnalysesRepository.Object,
                _mockMarketDataRepository.Object,
                _mockRegistryRepository.Object,
                _mockConfigReader.Object);

            var expectedResult = new KeyValuePair<string, IAnalysis>(isins[0], new AnalysisBuilder().Build());

            // Act
            var result = (service as AnalysisService).NewAnalyses().ToDictionary(a => a.Key, a => a.Value);

            // Assert
            result.Keys.ToArray().Should().BeEquivalentTo(isins);
        }
    }
}
