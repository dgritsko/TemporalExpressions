using FluentAssertions;
using NUnit.Framework;
using System;

namespace TemporalExpressions.Tests.Expressions
{
    [TestFixture]
    public class NotTests
    {
        [Test]
        public void NotTrueExpression()
        {
            var expression = new Not(new True());
            var initialDate = DateTime.Parse("01/01/17");

            var count = Util.Util.TotalMatches(expression, initialDate, 1000);

            count.Should().Be(0);
        }

        [Test]
        public void NotFalseExpression()
        {
            var expression = new Not(new False());
            var initialDate = DateTime.Parse("01/01/17");

            var count = Util.Util.TotalMatches(expression, initialDate, 1000);

            count.Should().Be(1000);
        }
    }
}
