using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Validators;

namespace Models.UnitTests.ValidatorTests
{
    [TestFixture]
    public class Isin_IsValid
    {
        private readonly string _valid = "AB1234567890";

        private readonly string _empty = string.Empty;
        private readonly string _tooShort = "AB123456789";
        private readonly string _tooLong = "AB12345678901";
        private readonly string _invalidChar = "AB1234 67890";

        [Test]
        public void WithValid_ShouldReturnTrue() => Isin.IsValid(_valid).Should().BeTrue();

        [Test]
        public void WithNull_ShouldReturnFalse() => Isin.IsValid(_empty).Should().BeFalse();

        [Test]
        public void WithTooShort_ShouldReturnFalse() => Isin.IsValid(_tooShort).Should().BeFalse();

        [Test]
        public void WithTooLong_ShouldReturnFalse() => Isin.IsValid(_tooLong).Should().BeFalse();

        [Test]
        public void WithInvalidChar_ShouldReturnFalse() => Isin.IsValid(_invalidChar).Should().BeFalse();
    }
}
