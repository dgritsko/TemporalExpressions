using NUnit.Framework;
using TemporalExpressions.Parser.V2;

namespace TemporalExpressions.Tests
{
    [TestFixture]
    public class AnalyzerTests
    {
        [TestCase("{a(b:c)}")]
        [TestCase("{a(b:c,d:e)}")]
        public void ShouldAnalyzeCorrectly(string input)
        {
            var analyzed = Analyzer.Analyze(input);
        }
    }
}
