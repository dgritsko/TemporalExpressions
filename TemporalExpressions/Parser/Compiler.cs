using System;
using System.Collections.Generic;
using TemporalExpressions.Parser.Parts;

namespace TemporalExpressions.Parser
{
    public static class Compiler
    {
        public static Dictionary<string, Func<TokenizedExpression, TemporalExpression>> ExpressionCompilers = new Dictionary<string, Func<TokenizedExpression, TemporalExpression>>
        {
            { "dayinmonth", BuildDayInMonth },
            { "rangeeachyear", BuildRangeEachYear },
            { "difference", BuildDifference },
        };

        public static TemporalExpression BuildDayInMonth(TokenizedExpression tokenizedExpression)
        {
            var count = tokenizedExpression.GetValueArgument<int>("count");
            var day = tokenizedExpression.GetValueArgument<DayOfWeek>("day");
            return new DayInMonth(count, day);
        }

        public static TemporalExpression BuildRangeEachYear(TokenizedExpression tokenizedExpression)
        {
            var month = tokenizedExpression.GetValueArgument<int?>("month");
            var startMonth = tokenizedExpression.GetValueArgument<int?>("startmonth");
            var endMonth = tokenizedExpression.GetValueArgument<int?>("endmonth");
            var startDay = tokenizedExpression.GetValueArgument<int?>("startday");
            var endDay = tokenizedExpression.GetValueArgument<int?>("endday");

            if (startMonth.HasValue && endMonth.HasValue && startDay.HasValue && endDay.HasValue)
            {
                return new RangeEachYear(startMonth.Value, endMonth.Value, startDay.Value, endDay.Value);
            }

            if (startMonth.HasValue && endMonth.HasValue)
            {
                return new RangeEachYear(startMonth.Value, endMonth.Value);
            }

            if (month.HasValue)
            {
                return new RangeEachYear(month.Value);
            }

            throw new InvalidOperationException();
        }

        public static TemporalExpression BuildDifference(TokenizedExpression tokenizedExpression)
        {
            var included = tokenizedExpression.GetExpressionArgument("included");
            var excluded = tokenizedExpression.GetExpressionArgument("excluded");

            return new Difference(included, excluded);
        }

        public static TemporalExpression Compile(TokenizedExpression tokenized)
        {
            var builder = ExpressionCompilers[tokenized.Identifier];

            return builder(tokenized);
        }
    }
}
