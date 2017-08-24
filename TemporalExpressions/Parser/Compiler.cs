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
        };

        public static TemporalExpression BuildDayInMonth(TokenizedExpression tokenizedExpression)
        {
            var count = tokenizedExpression.GetArgument<int>("count");
            var day = tokenizedExpression.GetArgument<DayOfWeek>("day");
            return new DayInMonth(count, day);
        }

        public static TemporalExpression BuildRangeEachYear(TokenizedExpression tokenizedExpression)
        {
            var month = tokenizedExpression.GetArgument<int?>("month");
            var startMonth = tokenizedExpression.GetArgument<int?>("startmonth");
            var endMonth = tokenizedExpression.GetArgument<int?>("endmonth");
            var startDay = tokenizedExpression.GetArgument<int?>("startday");
            var endDay = tokenizedExpression.GetArgument<int?>("endday");

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

        public static TemporalExpression Compile(TokenizedExpression tokenized)
        {
            var builder = ExpressionCompilers[tokenized.Identifier];

            return builder(tokenized);
        }
    }
}
