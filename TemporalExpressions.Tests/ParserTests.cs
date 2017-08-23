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
        public void ShouldTokenizeArgumentsCorrectly(string arguments, int expectedCount)
        {
            var tokenized = Parser.Parser.TokenizeArguments(arguments);

            tokenized.Count.Should().Be(expectedCount);
        }

        [TestCase("{a:(b:c)}", "a", 1)]
        public void ShouldTokenizeExpressionCorrectly(string expression, string expectedIdentifier, int expectedArgumentCount)
        {
            var tokenized = Parser.Parser.TokenizeExpression(expression);

            tokenized.Identifier.Should().Be(expectedIdentifier);
            tokenized.Arguments.Count.Should().Be(expectedArgumentCount);
        }
    }
}
