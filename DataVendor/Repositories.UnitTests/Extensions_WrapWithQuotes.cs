using NUnit.Framework;
using System;

namespace Repositories.UnitTests
{
    [TestFixture]
    public class Extensions_WrapWithQuotes
    {
        [Test]
        public void WithInvalidInput_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => Peter.Repositories.Helpers.Extensions.WrapWithQuotes(null));
        }

        [TestCase("\"\"", "")]
        [TestCase("\" \"", " ")]
        [TestCase("\"hello\"", "hello")]
        public void WithValidInput_ReturnsCorrectResult(string expectedResult, string input) => 
            Assert.AreEqual(expectedResult, Peter.Repositories.Helpers.Extensions.WrapWithQuotes(input));
    }
}
