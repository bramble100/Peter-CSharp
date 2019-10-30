using FluentAssertions;
using Moq;
using NUnit.Framework;
using Repositories.Exceptions;
using Repositories.Interfaces;
using Services.DataVendor;
using System;
using System.Linq;

namespace Services.UnitTests.DataVendor
{
    [TestFixture]
    public class DataVendorService_HaveAllMarketDataNamesIsins
    {
        IDataVendorService service;

        readonly Mock<IIsinsRepository> _mockIsinRepository;
        readonly Mock<IMarketDataRepository> _mockMarketDataRepository;

        public DataVendorService_HaveAllMarketDataNamesIsins()
        {
            _mockIsinRepository = new Mock<IIsinsRepository>();
            _mockMarketDataRepository = new Mock<IMarketDataRepository>();
        }

        [Test]
        public void WithEmptyRepos_ReturnTrue()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(0).ToArray();
            var names = TestDataFactory.NewCompanyNames(0).ToArray();

            _mockMarketDataRepository
                .Setup(m => m.GetNames())
                .Returns(names);

            service = new DataVendorService(
                _mockIsinRepository.Object,
                _mockMarketDataRepository.Object);

            // Act
            var result = service.HaveAllMarketDataNamesIsins;

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void WithOneIsinAndNoName_ReturnFalse()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(1).ToArray();
            var names = TestDataFactory.NewCompanyNames(0).ToArray();

            _mockMarketDataRepository
                .Setup(m => m.GetNames())
                .Returns(names);

            service = new DataVendorService(
                _mockIsinRepository.Object,
                _mockMarketDataRepository.Object);

            // Act
            var result = service.HaveAllMarketDataNamesIsins;

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void WithTwoIsinAndTwoName_ReturnTrue()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(2).ToArray();
            var names = TestDataFactory.NewCompanyNames(2).ToArray();

            _mockMarketDataRepository
                .Setup(m => m.GetNames())
                .Returns(names);
            _mockIsinRepository
                .Setup(m => m.GetIsins())
                .Returns(isins);
            _mockIsinRepository
                .Setup(m => m.FindIsinByName(names[0]))
                .Returns(isins[0]);
            _mockIsinRepository
                .Setup(m => m.FindIsinByName(names[1]))
                .Returns(isins[1]);
            service = new DataVendorService(
                _mockIsinRepository.Object,
                _mockMarketDataRepository.Object);

            // Act
            var result = service.HaveAllMarketDataNamesIsins;

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void WithOneIsinAndTwoName_ReturnTrue()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(1).ToArray();
            var names = TestDataFactory.NewCompanyNames(2).ToArray();

            _mockMarketDataRepository
                .Setup(m => m.GetNames())
                .Returns(names);
            _mockIsinRepository
                .Setup(m => m.GetIsins())
                .Returns(isins);
            _mockIsinRepository
                .Setup(m => m.FindIsinByName(names[0]))
                .Returns(isins[0]);
            _mockIsinRepository
                .Setup(m => m.FindIsinByName(names[1]))
                .Returns(isins[0]);
            service = new DataVendorService(
                _mockIsinRepository.Object,
                _mockMarketDataRepository.Object);

            // Act
            var result = service.HaveAllMarketDataNamesIsins;

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void WithTwoIsinAndOneName_ReturnTrue()
        {
            // Arrange
            var isins = TestDataFactory.NewIsins(2).ToArray();
            var names = TestDataFactory.NewCompanyNames(1).ToArray();

            _mockMarketDataRepository
                .Setup(m => m.GetNames())
                .Returns(names);
            _mockIsinRepository
                .Setup(m => m.GetIsins())
                .Returns(isins);
            _mockIsinRepository
                .Setup(m => m.FindIsinByName(names[0]))
                .Throws<RepositoryException>();
            service = new DataVendorService(
                _mockIsinRepository.Object,
                _mockMarketDataRepository.Object);

            // Act
            var result = service.HaveAllMarketDataNamesIsins;

            // Assert
            result.Should().BeFalse();
        }
    }
}
