using FluentAssertions;
using Moq;
using NUnit.Framework;
using Peter.Repositories.Implementations;
using Peter.Repositories.Interfaces;

namespace Repositories.IntegrationTests
{
    [TestFixture]
    public class IsinsCsvFileRepositoryTests
    {
        [Test]
        public void ContainsName()
        {
            var mock = new Mock<IFileSystemFacade>();
            mock
                .Setup(facade => facade.Load("mock"))
                .Returns("Name;ISIN\n\"1+1 DRILLISCH AG O.N.\"; DE0005545503");
            IIsinsRepository repository = new IsinsCsvFileRepository(mock.Object);
            var result = repository.GetNames();
            result.Should().Contain("1+1 DRILLISCH AG O.N.");
            result.Should().HaveCount(1);
        }
    }
}
