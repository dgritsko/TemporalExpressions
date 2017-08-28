using FluentAssertions;
using NUnit.Framework;
using System;

namespace TemporalExpressions.Tests.Expressions
{
    [TestFixture]
    public class FalseTests
    {
        [Test]
        public void FalseExpression()
        {
            var expression = new False();
            var initialDate = DateTime.Parse("01/01/17");

            var count = Util.Util.TotalMatches(expression, initialDate, 1000);

            count.Should().Be(0);
        }
    }
}
