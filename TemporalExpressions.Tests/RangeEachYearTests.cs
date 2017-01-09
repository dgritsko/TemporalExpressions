using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace TemporalExpressions.Tests
{
    [TestFixture]
    public class RangeEachYearTests
    {
        [TestCase(1, 1, 0, 0, "01/01/17", true)]
        [TestCase(1, 1, 1, 2, "01/01/17", true)]
        [TestCase(1, 1, 2, 3, "01/01/17", true)]
        public void RangeEachYear(int startMonth, int endMonth, int startDay, int endDay, string date, bool expectedResult)
        {
            var expression = new RangeEachYear(startMonth, endMonth, startDay, endDay);

            expression.Includes(DateTime.Parse(date)).Should().Be(expectedResult);
        }

        [TestCase(1, 12, 0, 0, 365)]
        public void ComprehensiveTest(int startMonth, int endMonth, int startDay, int endDay, int expectedResult)
        {
            var expression = new RangeEachYear(startMonth, endMonth, startDay, endDay);

            var initialDate = DateTime.Parse("01/01/17");

            var annualMatches = Enumerable.Range(0, 365)
                .Select(x => initialDate.AddDays(x))
                .Select(x => expression.Includes(x))
                .Sum(x => x ? 1 : 0);

            annualMatches.Should().Be(expectedResult);
        }
    }
}
