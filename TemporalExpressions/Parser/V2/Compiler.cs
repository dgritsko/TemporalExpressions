using System;
using System.Collections.Generic;
using System.Linq;
using TemporalExpressions.Parser.V2.Components;

namespace TemporalExpressions.Parser.V2
{
    namespace Identifiers
    {
        public class Expressions
        {
            public const string DayInMonth = "dayinmonth";
            public const string Difference = "difference";
            public const string Intersection = "intersection";
            public const string RangeEachYear = "rangeeachyear";
            public const string Union = "union";
        }

        public class DayInMonth
        {
            public const string Count = "count";
            public const string Day = "day";
        }

        public class RangeEachYear
        {
            public const string Month = "month";
            public const string StartMonth = "startmonth";
            public const string EndMonth = "endmonth";
            public const string StartDay = "startday";
            public const string EndDay = "endday";
        }

        public class Difference
        {
            public const string Included = "included";
            public const string Excluded = "excluded";
        }

        public class Intersection
        {
            public const string Elements = "elements";
        }
    }

    public static class Compiler
    {
        public static Dictionary<string, Func<Expression, TemporalExpression>> ExpressionCompilers = new Dictionary<string, Func<Expression, TemporalExpression>>
        {
            { Identifiers.Expressions.DayInMonth, CompileDayInMonth },
            { Identifiers.Expressions.RangeEachYear, CompileRangeEachYear },
            { Identifiers.Expressions.Difference, CompileDifference },
            { Identifiers.Expressions.Intersection, CompileIntersection },
        };

        public static TemporalExpression Compile(Expression expression)
        {
            var expressionCompiler = ExpressionCompilers[expression.Identifier.Value];

            return expressionCompiler(expression);
        }

        public static TemporalExpression CompileDayInMonth(Expression expression)
        {
            var count = GetScalarArgument<int>(expression, Identifiers.DayInMonth.Count);
            var day = GetScalarArgument<DayOfWeek>(expression, Identifiers.DayInMonth.Day);
            return new DayInMonth(count, day);
        }

        public static TemporalExpression CompileRangeEachYear(Expression expression)
        {
            var month = GetScalarArgument<int?>(expression, Identifiers.RangeEachYear.Month);
            var startMonth = GetScalarArgument<int?>(expression, Identifiers.RangeEachYear.StartMonth);
            var endMonth = GetScalarArgument<int?>(expression, Identifiers.RangeEachYear.EndMonth);
            var startDay = GetScalarArgument<int?>(expression, Identifiers.RangeEachYear.StartDay);
            var endDay = GetScalarArgument<int?>(expression, Identifiers.RangeEachYear.EndDay);

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

        public static TemporalExpression CompileDifference(Expression expression)
        {
            var includedExpression = GetExpressionArgument(expression, Identifiers.Difference.Included);
            var excludedExpression = GetExpressionArgument(expression, Identifiers.Difference.Excluded);

            var included = Compile(includedExpression);
            var excluded = Compile(excludedExpression);

            return new Difference(included, excluded);
        }

        public static TemporalExpression CompileIntersection(Expression expression)
        {
            var elementsExpressions = GetExpressionsArgument(expression, Identifiers.Intersection.Elements);

            var elements = elementsExpressions.Select(Compile).ToList();

            return new Intersection(elements);
        }

        public static T GetScalarArgument<T>(Expression expression, string identifier)
        {
            var argument = expression.Arguments.OfType<ScalarArgument>().FirstOrDefault(arg => string.Equals(arg.Identifier.Value, identifier));

            if (typeof(T) == typeof(string))
            {
                return (T)(object)argument?.Value;
            }

            if (Nullable.GetUnderlyingType(typeof(T)) == typeof(int))
            {
                return argument == null
                    ? (T)(object)null
                    : (T)(object)Int32.Parse(argument.Value);
            }

            if (typeof(T) == typeof(int))
            {
                return (T)(object)Int32.Parse(argument?.Value);
            }

            if (typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), argument?.Value, true);
            }

            throw new InvalidOperationException();
        }

        public static Expression GetExpressionArgument(Expression expression, string identifier)
        {
            var argument = expression.Arguments.OfType<ExpressionsArgument>().FirstOrDefault(arg => string.Equals(arg.Identifier.Value, identifier));

            return argument.Expressions.FirstOrDefault();
        }

        public static List<Expression> GetExpressionsArgument(Expression expression, string identifier)
        {
            var argument = expression.Arguments.OfType<ExpressionsArgument>().FirstOrDefault(arg => string.Equals(arg.Identifier.Value, identifier));

            return argument.Expressions;
        }
    }
}
