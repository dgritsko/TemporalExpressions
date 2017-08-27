using System;
using FluentAssertions;
using NUnit.Framework;

namespace TemporalExpressions.Tests.Expressions
{
    [TestFixture]
    public class RangeEachIntervalTests
    {
        [TestCase(2017, 1, 1, 1, UnitOfTime.Day, "12/31/16", false)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Day, "01/01/17", true)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Day, "01/02/17", true)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Day, "01/03/17", true)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Day, "01/04/17", true)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Day, "01/01/17", true)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Day, "01/02/17", false)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Day, "01/03/17", true)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Day, "01/04/17", false)]
        [TestCase(2017, 1, 1, 3, UnitOfTime.Day, "01/01/17", true)]
        [TestCase(2017, 1, 1, 3, UnitOfTime.Day, "01/02/17", false)]
        [TestCase(2017, 1, 1, 3, UnitOfTime.Day, "01/03/17", false)]
        [TestCase(2017, 1, 1, 3, UnitOfTime.Day, "01/04/17", true)]
        public void RangeEachIntervalDay(int year, int month, int day, int count, UnitOfTime unit, string date, bool expectedResult)
        {
            var expression = new RangeEachInterval(year, month, day, count, unit);

            expression.Includes(DateTime.Parse(date)).Should().Be(expectedResult);
        }

        [TestCase(2017, 1, 1, 1, UnitOfTime.Week, "12/31/16", false)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Week, "01/01/17", true)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Week, "01/02/17", true)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Week, "01/03/17", true)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Week, "01/04/17", true)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Week, "01/05/17", true)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Week, "01/06/17", true)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Week, "01/07/17", true)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Week, "01/08/17", true)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Week, "01/01/17", true)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Week, "01/07/17", true)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Week, "01/08/17", false)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Week, "01/14/17", false)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Week, "01/15/17", true)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Week, "01/21/17", true)]
        public void RangeEachIntervalWeek(int year, int month, int day, int count, UnitOfTime unit, string date, bool expectedResult)
        {
            var expression = new RangeEachInterval(year, month, day, count, unit);

            expression.Includes(DateTime.Parse(date)).Should().Be(expectedResult);
        }

        [TestCase(2017, 1, 1, 1, UnitOfTime.Month, "12/31/16", false)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Month, "01/01/17", true)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Month, "01/31/17", true)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Month, "02/01/17", true)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Month, "01/01/17", true)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Month, "01/31/17", true)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Month, "02/01/17", false)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Month, "02/28/17", false)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Month, "03/01/17", true)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Month, "03/31/17", true)]
        public void RangeEachIntervalMonth(int year, int month, int day, int count, UnitOfTime unit, string date, bool expectedResult)
        {
            var expression = new RangeEachInterval(year, month, day, count, unit);

            expression.Includes(DateTime.Parse(date)).Should().Be(expectedResult);
        }

        [TestCase(2017, 1, 1, 1, UnitOfTime.Year, "12/31/16", false)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Year, "01/01/17", true)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Year, "12/31/17", true)]
        [TestCase(2017, 1, 1, 1, UnitOfTime.Year, "01/01/18", true)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Year, "01/01/17", true)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Year, "12/31/17", true)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Year, "01/01/18", false)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Year, "12/31/18", false)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Year, "01/01/19", true)]
        [TestCase(2017, 1, 1, 2, UnitOfTime.Year, "12/31/19", true)]
        public void RangeEachIntervalYear(int year, int month, int day, int count, UnitOfTime unit, string date, bool expectedResult)
        {
            var expression = new RangeEachInterval(year, month, day, count, unit);

            expression.Includes(DateTime.Parse(date)).Should().Be(expectedResult);
        }
    }
}
