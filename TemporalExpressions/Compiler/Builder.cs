using TemporalExpressions.Compiler.Components;
using System;
using System.Linq;
using System.Collections.Generic;
using TemporalExpressions.Compiler.Util;

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
            { TemporalExpressions.Compiler.Identifiers.Expressions.RegularInterval, CompileRegularInterval },
            { TemporalExpressions.Compiler.Identifiers.Expressions.True, CompileTrue },
            { TemporalExpressions.Compiler.Identifiers.Expressions.False, CompileFalse },
            { TemporalExpressions.Compiler.Identifiers.Expressions.Not, CompileNot },
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
            var count = BuilderUtil.GetScalarArgument<int>(expression, TemporalExpressions.Compiler.Identifiers.DayInMonth.Count);
            var day = BuilderUtil.GetScalarArgument<DayOfWeek>(expression, TemporalExpressions.Compiler.Identifiers.DayInMonth.Day);
            return new DayInMonth(count, day);
        }

        public static TemporalExpression CompileRangeEachYear(Expression expression)
        {
            var month = BuilderUtil.GetScalarArgument<int?>(expression, TemporalExpressions.Compiler.Identifiers.RangeEachYear.Month);
            var startMonth = BuilderUtil.GetScalarArgument<int?>(expression, TemporalExpressions.Compiler.Identifiers.RangeEachYear.StartMonth);
            var endMonth = BuilderUtil.GetScalarArgument<int?>(expression, TemporalExpressions.Compiler.Identifiers.RangeEachYear.EndMonth);
            var startDay = BuilderUtil.GetScalarArgument<int?>(expression, TemporalExpressions.Compiler.Identifiers.RangeEachYear.StartDay);
            var endDay = BuilderUtil.GetScalarArgument<int?>(expression, TemporalExpressions.Compiler.Identifiers.RangeEachYear.EndDay);

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
            var includedExpression = BuilderUtil.GetExpressionArgument(expression, TemporalExpressions.Compiler.Identifiers.Difference.Included);
            var excludedExpression = BuilderUtil.GetExpressionArgument(expression, TemporalExpressions.Compiler.Identifiers.Difference.Excluded);

            var included = Build(includedExpression);
            var excluded = Build(excludedExpression);

            return new Difference(included, excluded);
        }

        public static TemporalExpression CompileIntersection(Expression expression)
        {
            var elementsExpressions = BuilderUtil.GetExpressionsArgument(expression, TemporalExpressions.Compiler.Identifiers.Intersection.Elements);

            var elements = elementsExpressions.Select(Build).ToList();

            return new Intersection(elements);
        }

        public static TemporalExpression CompileUnion(Expression expression)
        {
            var elementsExpressions = BuilderUtil.GetExpressionsArgument(expression, TemporalExpressions.Compiler.Identifiers.Union.Elements);

            var elements = elementsExpressions.Select(Build).ToList();

            return new Union(elements);
        }

        public static TemporalExpression CompileRegularInterval(Expression expression)
        {
            var year = BuilderUtil.GetScalarArgument<int>(expression, TemporalExpressions.Compiler.Identifiers.RegularInterval.Year);
            var month = BuilderUtil.GetScalarArgument<int>(expression, TemporalExpressions.Compiler.Identifiers.RegularInterval.Month);
            var day = BuilderUtil.GetScalarArgument<int>(expression, TemporalExpressions.Compiler.Identifiers.RegularInterval.Day);

            var count = BuilderUtil.GetScalarArgument<int>(expression, TemporalExpressions.Compiler.Identifiers.RegularInterval.Count);
            var unit = BuilderUtil.GetScalarArgument<UnitOfTime>(expression, TemporalExpressions.Compiler.Identifiers.RegularInterval.Unit);
            return new RegularInterval(year, month, day, count, unit);
        }

        public static TemporalExpression CompileTrue(Expression expression)
        {
            return new True();
        }

        public static TemporalExpression CompileFalse(Expression expression)
        {
            return new False();
        }

        public static TemporalExpression CompileNot(Expression expression)
        {
            var childExpression = BuilderUtil.GetExpressionArgument(expression, TemporalExpressions.Compiler.Identifiers.Not.Expression);

            var child = Build(childExpression);

            return new Not(child);
        }
    }
}
