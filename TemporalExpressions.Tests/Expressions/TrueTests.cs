using FluentAssertions;
using NUnit.Framework;
using System;

namespace TemporalExpressions.Tests.Expressions
{
    [TestFixture]
    public class TrueTests
    {
        [Test]
        public void TrueExpression()
        {
            var expression = new True();
            var initialDate = DateTime.Parse("01/01/17");

            var count = Util.Util.TotalMatches(expression, initialDate, 1000);

            count.Should().Be(1000);
        }
    }
}
