using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Validators;

namespace Models.UnitTests.ValidatorTests
{
    [TestFixture]
    public class Isin_IsValidOrEmpty
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("\t\n")]
        [TestCase("AB1234567890")]
        public void ShouldReturnTrue_WhenInputValid(string input) =>
            Isin.IsValidOrEmpty(input).Should().BeTrue();

        [TestCase("AB123456789")] // too short
        [TestCase("AB12345678901")] // too long
        [TestCase("AB1234 67890")] // invalid char
        [TestCase("1AB23456789")] // invalid first two char
        public void ShouldReturnFalse_WhenInputInvalid(string input) =>
            Isin.IsValidOrEmpty(input).Should().BeFalse();
    }
}
