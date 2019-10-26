using Autofac;
using FluentAssertions;
using Infrastructure;
using Infrastructure.Config;
using Moq;
using NUnit.Framework;
using Repositories.Implementations;
using Repositories.Interfaces;
using System.Text;

namespace Repositories.IntegrationTests
{
    [TestFixture]
    public class IsinsCsvFileRepositoryTests
    {
        [Test]
        public void ContainsName()
        {
            var mockFileSystem = new Mock<IFileSystemFacade>();

            mockFileSystem
                .Setup(facade => facade.Load("mock"))
                .Returns("Name;ISIN\n\"1+1 DRILLISCH AG O.N.\"; DE0005545503");

            mockFileSystem
                .Setup(facade => facade.ReadLines("mock", Encoding.UTF8))
                .Returns(new string[] { "Name;ISIN", "1+1 DRILLISCH AG O.N.\"; DE0005545503" });

            var builder = new ContainerBuilder();
            builder.RegisterInstance(mockFileSystem.Object).As<IFileSystemFacade>();
            builder.RegisterType<ConfigReader>().As<IConfigReader>();
            builder.RegisterType<IsinsCsvFileRepository>().As<IIsinsRepository>();
            var container = builder.Build();

            using(var scope = container.BeginLifetimeScope())
            {
                var repository = scope.Resolve<IIsinsRepository>();
                var result = repository.GetNames();
                result.Should().Contain("1+1 DRILLISCH AG O.N.");
                result.Should().HaveCount(1);
            }
        }
    }
}
