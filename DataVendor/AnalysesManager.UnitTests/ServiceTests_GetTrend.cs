using AnalysesManager.Services.Implementations;
using NUnit.Framework;
using Peter.Models.Builders;
using Peter.Models.Enums;
using System;

namespace AnalysesManager.UnitTests
{
    public class ServiceTests_GetTrend
    {
        [Test]
        public void GetTrend_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Service.GetTrend(null));
        }
    }
}
