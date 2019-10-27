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
    class AnalysisService_SaveAnalyses
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

        public AnalysisService_SaveAnalyses()
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
        public void WithoutNullInput_ThrowsExceptions()
        {
            // Arrange
            IEnumerable<KeyValuePair<string, IAnalysis>> analyses = null;

            _mockConfigReader.Setup(m => m.Settings.BuyingPacketInEuro).Returns(1000);
            _mockConfigReader.Setup(m => m.Settings.FastMovingAverage).Returns(1);
            _mockConfigReader.Setup(m => m.Settings.SlowMovingAverage).Returns(2);

            service = new AnalysisService(
                _mockFundamentalAnalyser.Object,
                _mockTechnicalAnalyser.Object,
                _mockDatavendorService.Object,
                _mockAnalysesRepository.Object,
                _mockMarketDataRepository.Object,
                _mockRegistryRepository.Object,
                _mockConfigReader.Object);

            // Act
            void action() => service.SaveAnalyses(analyses);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(21)]
        public void WithValidInputs_SavesCorrectly(int count)
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(count).ToArray();
            var analyses = TestDataFactory.NewAnalysesWithIsins(isins).ToArray();

            _mockConfigReader.Setup(m => m.Settings.BuyingPacketInEuro).Returns(1000);
            _mockConfigReader.Setup(m => m.Settings.FastMovingAverage).Returns(1);
            _mockConfigReader.Setup(m => m.Settings.SlowMovingAverage).Returns(2);

            service = new AnalysisService(
                _mockFundamentalAnalyser.Object,
                _mockTechnicalAnalyser.Object,
                _mockDatavendorService.Object,
                _mockAnalysesRepository.Object,
                _mockMarketDataRepository.Object,
                _mockRegistryRepository.Object,
                _mockConfigReader.Object);

            // Act
            service.SaveAnalyses(analyses);

            // Assert
            _mockAnalysesRepository.Verify(m => m.AddRange(analyses), Times.Once);
        }
    }
}
