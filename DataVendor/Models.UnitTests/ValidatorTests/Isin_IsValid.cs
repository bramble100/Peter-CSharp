﻿using FluentAssertions;
using NUnit.Framework;
using Peter.Models.Validators;

namespace Models.UnitTests.ValidatorTests
{
    [TestFixture]
    public class Isin_IsValid
    {
        [TestCase("AB1234567890")]
        public void ShouldReturnTrue_WhenInputValid(string input) => 
            Isin.IsValid(input).Should().BeTrue();

        [TestCase("")]
        [TestCase("AB123456789")] // too short
        [TestCase("AB12345678901")] // too long
        [TestCase("AB1234 67890")] // invalid char
        public void ShouldReturnFalse_WhenInputInvalid(string input) => 
            Isin.IsValid(input).Should().BeFalse();
    }
}
