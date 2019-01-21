using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Validators;

namespace Models.UnitTests.ValidatorTests
{
    [TestFixture]
    public class NameToIsin_TryParse
    {
        [TestCase("DummyName", "AA1234567890")]
        public void ReturnsTrue_WhenInputIsValid(string name, string isin)
        {
            NameToIsin.TryParse(new string[] { name, isin },
                    out string nameResult,
                    out string isinResult)
                .Should()
                .BeTrue();
            nameResult.Should().Be(name);
            isinResult.Should().Be(isin);
        }

        [TestCase("DummyName", "AA123456789")]
        [TestCase("", "AA1234567890")]
        [TestCase(null, "AA1234567890")]
        public void ReturnsFalse_WhenInputIsInvalid(string name, string isin) => 
            NameToIsin.TryParse(new string[] { name, isin },
                    out string _,
                    out string _)
                .Should()
                .BeFalse();
    }
}
