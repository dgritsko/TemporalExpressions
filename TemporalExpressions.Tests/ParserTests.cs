using System;
using FluentAssertions;
using NUnit.Framework;
using TemporalExpressions.Parser.Parts;

namespace TemporalExpressions.Tests
{
    [TestFixture]
    public class ParserTests
    {
        [TestCase("a:1", ArgumentType.Scalar, "a", "1")]
        [TestCase("a:123", ArgumentType.Scalar, "a", "123")]
        [TestCase("a:{foo(bar:baz)}", ArgumentType.Expression, "a", "{foo(bar:baz)}")]
        public void ShouldTokenizeArgumentCorrectly(string argument, ArgumentType expectedType, string expectedIdentifier, string expectedValue)
        {
            var tokenized = Parser.Parser.TokenizeArgument(argument);

            tokenized.Type.Should().Be(expectedType);
            tokenized.Identifier.Should().Be(expectedIdentifier);
            tokenized.Value.Should().Be(expectedValue);
        }

        [TestCase("(a:b)", 1)]
        [TestCase("(a:b,c:d,e:f)", 3)]
        [TestCase("(a:{b(c:d)})", 1)]
        [TestCase("(a:b,c:{d(e:f)},g:{h(i:j)})", 3)]
        [TestCase("(a:{b(c:d)};{e(f:g)})", 1)]
        public void ShouldTokenizeArgumentsCorrectly(string arguments, int expectedCount)
        {
            var tokenized = Parser.Parser.TokenizeArguments(arguments);

            tokenized.Count.Should().Be(expectedCount);
        }

        [TestCase("{a(b:c)}", "a", 1)]
        public void ShouldTokenizeExpressionCorrectly(string expression, string expectedIdentifier, int expectedArgumentCount)
        {
            var tokenized = Parser.Parser.TokenizeExpression(expression);

            tokenized.Identifier.Should().Be(expectedIdentifier);
            tokenized.Arguments.Count.Should().Be(expectedArgumentCount);
        }

        [TestCase("{dayinmonth(count:1,day:sunday)}", 1, DayOfWeek.Sunday)]
        [TestCase("{dayinmonth(count:5,day:monday)}", 5, DayOfWeek.Monday)]
        [TestCase("{dayinmonth(count:-1,day:tuesday)}", -1, DayOfWeek.Tuesday)]
        [TestCase("{dayinmonth(count:2,day:wednesday)}", 2, DayOfWeek.Wednesday)]
        [TestCase("{dayinmonth(count:1,day:thursday)}", 1, DayOfWeek.Thursday)]
        [TestCase("{dayinmonth(count:1,day:friday)}", 1, DayOfWeek.Friday)]
        [TestCase("{dayinmonth(count:1,day:saturday)}", 1, DayOfWeek.Saturday)]
        public void ShouldParseDayInMonthExpressionCorrectly(string expressionRepresentation, int expectedCount, DayOfWeek expectedDayOfWeek)
        {
            var expression = Parser.Parser.Parse(expressionRepresentation);

            var dayInMonthExpression = expression as DayInMonth;

            dayInMonthExpression.Should().NotBeNull();

            dayInMonthExpression.Count.Should().Be(expectedCount);
            dayInMonthExpression.Day.Should().Be(expectedDayOfWeek);
        }

        [TestCase("{rangeeachyear(month:1)}", 1, 1, 0, 0)]
        [TestCase("{rangeeachyear(startmonth:1,endmonth:2)}", 1, 2, 0, 0)]
        [TestCase("{rangeeachyear(startmonth:1,endmonth:2,startday:3,endday:4)}", 1, 2, 3, 4)]
        public void ShouldParseRangeEachYearExpressionCorrectly(string expressionRepresentation, int expectedStartMonth, int expectedEndMonth, int expectedStartDay, int expectedEndDay)
        {
            var expression = Parser.Parser.Parse(expressionRepresentation);

            var rangeEachYearExpression = expression as RangeEachYear;

            rangeEachYearExpression.Should().NotBeNull();
            rangeEachYearExpression.StartMonth.Should().Be(expectedStartMonth);
            rangeEachYearExpression.EndMonth.Should().Be(expectedEndMonth);
            rangeEachYearExpression.StartDay.Should().Be(expectedStartDay);
            rangeEachYearExpression.EndDay.Should().Be(expectedEndDay);
        }

        [TestCase("{difference(included:{rangeeachyear(month:1)},excluded:{rangeeachyear(month:2)})}")]
        public void ShouldParseDifferenceExpressionCorrectly(string expressionRepresentation)
        {
            var expression = Parser.Parser.Parse(expressionRepresentation);

            var differenceExpression = expression as Difference;

            differenceExpression.Should().NotBeNull();
        }

        [TestCase("{intersection(elements:{rangeeachyear(month:1)};{rangeeachyear(month:1)})}")]
        public void ShouldParseIntersectionExpressionCorrectly(string expressionRepresentation)
        {
            var expression = Parser.Parser.Parse(expressionRepresentation);

            var intersectionExpression = expression as Intersection;

            intersectionExpression.Should().NotBeNull();
        }
    }
}
