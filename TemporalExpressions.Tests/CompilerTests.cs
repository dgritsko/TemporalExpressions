using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TemporalExpressions.Tests
{
    [TestFixture]
    public class CompilerTests
    {
        [TestCase("{dayinmonth(count:1,day:sunday)}", 1, DayOfWeek.Sunday)]
        [TestCase("{dayinmonth(count:5,day:monday)}", 5, DayOfWeek.Monday)]
        [TestCase("{dayinmonth(count:-1,day:tuesday)}", -1, DayOfWeek.Tuesday)]
        [TestCase("{dayinmonth(count:2,day:wednesday)}", 2, DayOfWeek.Wednesday)]
        [TestCase("{dayinmonth(count:1,day:thursday)}", 1, DayOfWeek.Thursday)]
        [TestCase("{dayinmonth(count:1,day:friday)}", 1, DayOfWeek.Friday)]
        [TestCase("{dayinmonth(count:1,day:saturday)}", 1, DayOfWeek.Saturday)]
        public void ShouldCompileDayInMonthExpressionCorrectly(string expressionRepresentation, int expectedCount, DayOfWeek expectedDayOfWeek)
        {
            var compiled = Compiler.Compiler.Compile(expressionRepresentation);

            var dayInMonthExpression = compiled as DayInMonth;

            dayInMonthExpression.Should().NotBeNull();

            dayInMonthExpression.Count.Should().Be(expectedCount);
            dayInMonthExpression.Day.Should().Be(expectedDayOfWeek);
        }

        [TestCase("{rangeeachyear(month:1)}", 1, 1, 0, 0)]
        [TestCase("{rangeeachyear(startmonth:1,endmonth:2)}", 1, 2, 0, 0)]
        [TestCase("{rangeeachyear(startmonth:1,endmonth:2,startday:3,endday:4)}", 1, 2, 3, 4)]
        public void ShouldParseRangeEachYearExpressionCorrectly(string expressionRepresentation, int expectedStartMonth, int expectedEndMonth, int expectedStartDay, int expectedEndDay)
        {
            var compiled = Compiler.Compiler.Compile(expressionRepresentation);

            var rangeEachYearExpression = compiled as RangeEachYear;

            rangeEachYearExpression.Should().NotBeNull();
            rangeEachYearExpression.StartMonth.Should().Be(expectedStartMonth);
            rangeEachYearExpression.EndMonth.Should().Be(expectedEndMonth);
            rangeEachYearExpression.StartDay.Should().Be(expectedStartDay);
            rangeEachYearExpression.EndDay.Should().Be(expectedEndDay);
        }

        [TestCase("{difference(included:{rangeeachyear(month:1)},excluded:{rangeeachyear(month:2)})}")]
        public void ShouldParseDifferenceExpressionCorrectly(string expressionRepresentation)
        {
            var compiled = Compiler.Compiler.Compile(expressionRepresentation);

            var differenceExpression = compiled as Difference;

            differenceExpression.Should().NotBeNull();
        }

        [TestCase("{intersection(elements:{rangeeachyear(month:1)};{rangeeachyear(month:1)})}")]
        public void ShouldParseIntersectionExpressionCorrectly(string expressionRepresentation)
        {
            var compiled = Compiler.Compiler.Compile(expressionRepresentation);

            var intersectionExpression = compiled as Intersection;

            intersectionExpression.Should().NotBeNull();
        }

        [TestCase("{union(elements:{rangeeachyear(month:1)};{rangeeachyear(month:1)})}")]
        public void ShouldParseUnionExpressionCorrectly(string expressionRepresentation)
        {
            var compiled = Compiler.Compiler.Compile(expressionRepresentation);

            var intersectionExpression = compiled as Union;

            intersectionExpression.Should().NotBeNull();
        }


        [TestCase("{intersection(elements:{intersection(elements:{intersection(elements:{intersection(elements:{intersection(elements:{rangeeachyear(month:1)})})})})})}")]
        public void ShouldParseNestedExpressionCorrectly(string expressionRepresentation)
        {
            var compiled = Compiler.Compiler.Compile(expressionRepresentation);
            
            compiled.Should().NotBeNull();

            var current = compiled;
            for (var i = 0; i < 5; i++)
            {
                var intersection = current as Intersection;

                intersection.Should().NotBeNull();
                intersection.Elements.Count.Should().Be(1);
                current = intersection.Elements.First();
            }

            current.Should().BeOfType<RangeEachYear>();
        }
    }
}
