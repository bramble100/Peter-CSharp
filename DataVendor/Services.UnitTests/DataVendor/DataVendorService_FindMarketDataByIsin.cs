using FluentAssertions;
using Moq;
using NUnit.Framework;
using Repositories.Interfaces;
using Services.DataVendor;
using System;
using System.Linq;

namespace Services.UnitTests.DataVendor
{
    [TestFixture]
    public class DataVendorService_FindMarketDataByIsin
    {
        IDataVendorService service;

        readonly Mock<IIsinsRepository> _mockIsinRepository;
        readonly Mock<IMarketDataRepository> _mockMarketDataRepository;

        public DataVendorService_FindMarketDataByIsin()
        {
            _mockIsinRepository = new Mock<IIsinsRepository>();
            _mockMarketDataRepository = new Mock<IMarketDataRepository>();
        }

        [Test]
        public void WithEmptyRepo_ReturnEmptyCollection()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(1).ToArray();
            var names = TestDataFactory.NewCompanyNames(1).ToArray();
            var marketData = TestDataFactory.NewMarketData(names, 0).ToArray();

            _mockMarketDataRepository
                .Setup(m => m.GetAll())
                .Returns(marketData);

            service = new DataVendorService(
                _mockIsinRepository.Object,
                _mockMarketDataRepository.Object);

            // Act
            var result = service.FindMarketDataByIsin(isins[0]);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void WithOneItem_ReturnOneItem()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(1).ToArray();
            var names = TestDataFactory.NewCompanyNames(1).ToArray();
            var marketData = TestDataFactory.NewMarketData(names, 1).ToArray();

            _mockMarketDataRepository
                .Setup(m => m.FindByNames(names))
                .Returns(marketData);
            _mockIsinRepository
                .Setup(m => m.FindNamesByIsin(isins[0]))
                .Returns(names);

            service = new DataVendorService(
                _mockIsinRepository.Object,
                _mockMarketDataRepository.Object);

            // Act
            var result = service.FindMarketDataByIsin(isins[0]);

            // Assert
            result.Should().BeEquivalentTo(marketData);
        }

        [Test]
        public void WithManyItem_ReturnManyItem()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(9).ToArray();
            var names = TestDataFactory.NewCompanyNames(9).ToArray();
            var marketData = TestDataFactory.NewMarketData(isins, names, 21).ToArray();

            _mockMarketDataRepository
                .Setup(m => m.FindByNames(new string[] { names[4] }))
                .Returns(marketData.Where(d => d.Isin == isins[4]).ToArray());

            for (int i = 0; i < names.Length; i++)
            {
                _mockIsinRepository
                    .Setup(m => m.FindNamesByIsin(isins[i]))
                    .Returns(new string[] { names[i] });
            }

            service = new DataVendorService(
                _mockIsinRepository.Object,
                _mockMarketDataRepository.Object);

            var expectedMarketData = marketData.Where(d => d.Isin == isins[4]).ToArray();

            // Act
            var result = service.FindMarketDataByIsin(isins[4]);

            // Assert
            result.Should().BeEquivalentTo(expectedMarketData);
        }
    }
}
