using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Validators;

namespace Models.UnitTests.ValidatorTests
{
    [TestFixture]
    public class RegistryEntry_TryParseLink
    {
        [TestCase(null, null)]
        [TestCase("", null)]
        [TestCase("http://dummy.com:1/dummy?a=1&b=two", "http://dummy.com:1/dummy?a=1&b=two")]
        [TestCase(" http://dummy.com:1/dummy?a=1&b=two ", "http://dummy.com:1/dummy?a=1&b=two")]
        [TestCase(" \t\n http://dummy.com:1/dummy?a=1&b=two ", "http://dummy.com:1/dummy?a=1&b=two")]
        public void ReturnsTrue_WhenInputIsValid(string input, string expected)
        {
            RegistryEntry.TryParseLink(input, out var output).Should().BeTrue();
            output.Should().Be(expected);
        }

        [TestCase("dummy.com")]
        [TestCase("Dummy Company")]
        [TestCase("http://")]
        [TestCase("dummy")]
        [TestCase("/dummy?a=1&b=two")]
        public void ReturnsFalse_WhenInputIsInvalid(string input) =>
            RegistryEntry.TryParseLink(input, out var _).Should().BeFalse();
    }
}
