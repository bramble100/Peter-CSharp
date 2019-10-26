using Moq;
using NUnit.Framework;
using Repositories.Interfaces;
using Services.DataVendor;

namespace Services.UnitTests.DataVendor
{
    [TestFixture]
    class DataVendorIsinAwareService_AddNewNames_Should
    {
        [Test]
        void AddMissingNewName()
        {
            var mockIsinsRepository = new Mock<IIsinsRepository>();
            var mockMarketDataRepository = new Mock<IMarketDataRepository>();

            mockIsinsRepository
                .Setup(r => r.GetNames())
                .Returns(new string[] { "Company1", "Company2" });

            mockMarketDataRepository
                .Setup(r => r.GetNames())
                .Returns(new string[] { "Company1", "Company3" });

            var service = new DataVendorService(
                mockIsinsRepository.Object,
                mockMarketDataRepository.Object);

            service.AddNewNames();



        }
    }
}
