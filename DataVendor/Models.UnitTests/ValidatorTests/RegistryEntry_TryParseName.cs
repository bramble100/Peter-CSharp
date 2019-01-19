using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Validators;

namespace Models.UnitTests.ValidatorTests
{
    [TestFixture]
    public class RegistryEntry_TryParseName
    {
        [TestCase("Dummy Name", "Dummy Name")]
        [TestCase(" Dummy Name ", "Dummy Name")]
        [TestCase(" \t\nDummy Name ", "Dummy Name")]
        public void ReturnsTrue_WhenInputIsValid(string input, string expected)
        {
            RegistryEntry.TryParseName(input, out var output)
                .Should()
                .BeTrue();
            output.Should().Be(expected);
        }

        [TestCase(null)]
        [TestCase("")]
        public void ReturnsFalse_WhenInputIsInvalid(string input) => 
            RegistryEntry.TryParseName(input, out var _).Should().BeFalse();
    }
}
