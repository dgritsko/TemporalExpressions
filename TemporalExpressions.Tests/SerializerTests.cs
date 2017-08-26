using NUnit.Framework;
using FluentAssertions;

namespace TemporalExpressions.Tests
{
    [TestFixture]
    public class SerializerTests
    {
        [TestCase("{rangeeachyear(month:1)}")]
        public void ShouldSerializeCorrectly(string input)
        {
            var compiled = Compiler.Compiler.Compile(input);

            var serialized = Serializer.Serializer.Serialize(compiled);

            serialized.Should().Be(input);
        }
    }
}
