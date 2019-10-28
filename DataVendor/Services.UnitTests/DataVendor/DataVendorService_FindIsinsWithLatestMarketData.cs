using FluentAssertions;
using Moq;
using NUnit.Framework;
using Repositories.Interfaces;
using Services.DataVendor;
using System.Linq;

namespace Services.UnitTests.DataVendor
{
    [TestFixture]
    public class DataVendorService_FindIsinsWithLatestMarketData
    {
        IDataVendorService service;

        readonly Mock<IIsinsRepository> _mockIsinRepository;
        readonly Mock<IMarketDataRepository> _mockMarketDataRepository;

        public DataVendorService_FindIsinsWithLatestMarketData()
        {
            _mockIsinRepository = new Mock<IIsinsRepository>();
            _mockMarketDataRepository = new Mock<IMarketDataRepository>();
        }

        [Test]
        public void WithEmptyRepo_ReturnEmptyCollection()
        {
            // Arrange
            var names = TestDataFactory.NewCompanyNames(0);
            var marketData = TestDataFactory.NewMarketData(names, 0);

            _mockMarketDataRepository
                .Setup(m => m.GetAll())
                .Returns(marketData);

            service = new DataVendorService(
                _mockIsinRepository.Object,
                _mockMarketDataRepository.Object);

            // Act
            var result = service.FindIsinsWithLatestMarketData();

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
                .Setup(m => m.GetAll())
                .Returns(marketData);
            _mockIsinRepository
                .Setup(m => m.FindIsinByName(names[0]))
                .Returns(isins[0]);

            service = new DataVendorService(
                _mockIsinRepository.Object,
                _mockMarketDataRepository.Object);

            // Act
            var result = service.FindIsinsWithLatestMarketData();

            // Assert
            result.Should().BeEquivalentTo(isins);
        }

        [Test]
        public void WithManyItem_ReturnManyItem()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(9).ToArray();
            var names = TestDataFactory.NewCompanyNames(9).ToArray();
            var marketData = TestDataFactory.NewMarketData(names, 21).ToArray();

            _mockMarketDataRepository
                .Setup(m => m.GetAll())
                .Returns(marketData);

            for (int i = 0; i < names.Length; i++)
            {
                _mockIsinRepository
                    .Setup(m => m.FindIsinByName(names[i]))
                    .Returns(isins[i]);
            }

            service = new DataVendorService(
                _mockIsinRepository.Object,
                _mockMarketDataRepository.Object);

            // Act
            var result = service.FindIsinsWithLatestMarketData();

            // Assert
            result.Should().BeEquivalentTo(isins);
        }

        [Test]
        public void WithManyMixedItem_ReturnOnlyValidItems()
        {
            // Arrange
            var isinsInitial = TestDataFactory.NewIsins(9).ToArray();
            var names = TestDataFactory.NewCompanyNames(9).ToArray();
            var marketData = TestDataFactory.NewMarketData(names, 21).ToArray();

            foreach (var data in marketData.Where(d=>d.Name == names.Last()))
            {
                data.DateTime = data.DateTime.AddDays(-1);
            }

            _mockMarketDataRepository
                .Setup(m => m.GetAll())
                .Returns(marketData);

            for (int i = 0; i < names.Length; i++)
            {
                _mockIsinRepository
                    .Setup(m => m.FindIsinByName(names[i]))
                    .Returns(isinsInitial[i]);
            }

            var expectedIsins = isinsInitial.Take(isinsInitial.Length - 1).ToArray();

            service = new DataVendorService(
                _mockIsinRepository.Object,
                _mockMarketDataRepository.Object);

            // Act
            var result = service.FindIsinsWithLatestMarketData().ToArray();

            // Assert
            result.Should().BeEquivalentTo(expectedIsins);
        }
    }
}
