using System;
using System.Linq;

namespace TemporalExpressions.Tests.Util
{
    public static class Util
    {
        public static int TotalMatches(TemporalExpression expression, DateTime initialDate, int totalDays)
        {
            var annualMatches = Enumerable.Range(0, totalDays)
                .Select(x => initialDate.AddDays(x))
                .Select(x => expression.Includes(x))
                .Sum(x => x ? 1 : 0);

            return annualMatches;
        }
    }
}
