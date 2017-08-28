using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TemporalExpressions.Tests
{
    [TestFixture]
    public class ComplexCompilerTests
    {
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
