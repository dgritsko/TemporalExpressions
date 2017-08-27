using TemporalExpressions.Compiler.Components;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TemporalExpressions.Compiler
{
    public static class Builder
    {
        public static Dictionary<string, Func<Expression, TemporalExpression>> ExpressionCompilers = new Dictionary<string, Func<Expression, TemporalExpression>>
        {
            { TemporalExpressions.Compiler.Identifiers.Expressions.DayInMonth, CompileDayInMonth },
            { TemporalExpressions.Compiler.Identifiers.Expressions.RangeEachYear, CompileRangeEachYear },
            { TemporalExpressions.Compiler.Identifiers.Expressions.Difference, CompileDifference },
            { TemporalExpressions.Compiler.Identifiers.Expressions.Intersection, CompileIntersection },
            { TemporalExpressions.Compiler.Identifiers.Expressions.Union, CompileUnion },
        };

        public static TemporalExpression Build(Expression expression)
        {
            if (!ExpressionCompilers.ContainsKey(expression.Identifier.Value))
            {
                throw new ArgumentException($"Unsupported expression identifier: {expression.Identifier.Value}");
            }

            var expressionCompiler = ExpressionCompilers[expression.Identifier.Value];

            return expressionCompiler(expression);
        }

        public static TemporalExpression CompileDayInMonth(Expression expression)
        {
            var count = GetScalarArgument<int>(expression, TemporalExpressions.Compiler.Identifiers.DayInMonth.Count);
            var day = GetScalarArgument<DayOfWeek>(expression, TemporalExpressions.Compiler.Identifiers.DayInMonth.Day);
            return new DayInMonth(count, day);
        }

        public static TemporalExpression CompileRangeEachYear(Expression expression)
        {
            var month = GetScalarArgument<int?>(expression, TemporalExpressions.Compiler.Identifiers.RangeEachYear.Month);
            var startMonth = GetScalarArgument<int?>(expression, TemporalExpressions.Compiler.Identifiers.RangeEachYear.StartMonth);
            var endMonth = GetScalarArgument<int?>(expression, TemporalExpressions.Compiler.Identifiers.RangeEachYear.EndMonth);
            var startDay = GetScalarArgument<int?>(expression, TemporalExpressions.Compiler.Identifiers.RangeEachYear.StartDay);
            var endDay = GetScalarArgument<int?>(expression, TemporalExpressions.Compiler.Identifiers.RangeEachYear.EndDay);

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
            var includedExpression = GetExpressionArgument(expression, TemporalExpressions.Compiler.Identifiers.Difference.Included);
            var excludedExpression = GetExpressionArgument(expression, TemporalExpressions.Compiler.Identifiers.Difference.Excluded);

            var included = Build(includedExpression);
            var excluded = Build(excludedExpression);

            return new Difference(included, excluded);
        }

        public static TemporalExpression CompileIntersection(Expression expression)
        {
            var elementsExpressions = GetExpressionsArgument(expression, TemporalExpressions.Compiler.Identifiers.Intersection.Elements);

            var elements = elementsExpressions.Select(Build).ToList();

            return new Intersection(elements);
        }

        public static TemporalExpression CompileUnion(Expression expression)
        {
            var elementsExpressions = GetExpressionsArgument(expression, TemporalExpressions.Compiler.Identifiers.Intersection.Elements);

            var elements = elementsExpressions.Select(Build).ToList();

            return new Union(elements);
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
