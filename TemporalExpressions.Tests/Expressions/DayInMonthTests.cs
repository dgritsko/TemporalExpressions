using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace TemporalExpressions.Tests.Expressions
{
    [TestFixture]
    public class DayInMonthTests
    {
        [TestCase(1, DayOfWeek.Sunday, "01/01/17", true)]
        [TestCase(2, DayOfWeek.Sunday, "01/01/17", false)]
        [TestCase(2, DayOfWeek.Sunday, "01/08/17", true)]
        [TestCase(-1, DayOfWeek.Sunday, "01/29/17", true)]
        public void DayInMonth(int count, DayOfWeek day, string date, bool expectedResult)
        {
            var expression = new DayInMonth(count, day);

            expression.Includes(DateTime.Parse(date)).Should().Be(expectedResult);
        }
        
        [TestCase(DayOfWeek.Sunday)]
        [TestCase(DayOfWeek.Monday)]
        [TestCase(DayOfWeek.Tuesday)]
        [TestCase(DayOfWeek.Thursday)]
        [TestCase(DayOfWeek.Friday)]
        [TestCase(DayOfWeek.Saturday)]
        public void ComprehensiveTest(DayOfWeek dayOfWeek)
        {
            var weeks = Enumerable.Range(1,4);

            foreach (var week in weeks)
            {
                var expression = new DayInMonth(week, dayOfWeek);

                var initialDate = DateTime.Parse("01/01/17");

                var annualMatches = Util.Util.TotalMatches(expression, initialDate, 365);

                annualMatches.Should().Be(12);
            }
        }
    }
}
