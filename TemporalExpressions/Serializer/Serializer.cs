using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TemporalExpressions.Compiler.Util;

namespace TemporalExpressions.Serializer
{
    public static class Serializer
    {
        public static Dictionary<Type, Action<TemporalExpression, StringBuilder>> Serializers = new Dictionary<Type, Action<TemporalExpression, StringBuilder>>
        {
            { typeof(DayInMonth), SerializeDayInMonth },
            { typeof(RangeEachYear), SerializeRangeEachYear },
            { typeof(RegularInterval), SerializeRegularInterval },
            { typeof(Difference), SerializeDifference },
            { typeof(Intersection), SerializeIntersection },
            { typeof(Union), SerializeUnion },
            { typeof(True), SerializeTrue },
            { typeof(False), SerializeFalse },
            { typeof(Not), SerializeNot },
        };

        public static string Serialize(TemporalExpression expression)
        {
            var key = expression.GetType();

            if (!Serializers.ContainsKey(key))
            {
                throw new ArgumentException($"Unsupported type: {key}");
            }

            var output = new StringBuilder();

            Serializers[key](expression, output);

            return output.ToString();
        }

        public static void SerializeDayInMonth(TemporalExpression temporalExpression, StringBuilder output)
        {
            var dayInMonthExpression = temporalExpression as DayInMonth;

            var arguments = new List<string>()
                {
                    BuildArgument(Compiler.Identifiers.DayInMonth.Count, dayInMonthExpression.Count.ToString()),
                    BuildArgument(Compiler.Identifiers.DayInMonth.Day, dayInMonthExpression.Day.ToString().ToLower()),
                };

            BuildExpression(Compiler.Identifiers.Expressions.DayInMonth, BuildArgumentList(arguments), output);
        }

        public static void SerializeRangeEachYear(TemporalExpression temporalExpression, StringBuilder output)
        {
            var rangeEachYearExpression = temporalExpression as RangeEachYear;

            if (rangeEachYearExpression.StartMonth == rangeEachYearExpression.EndMonth && rangeEachYearExpression.StartDay == 0 && rangeEachYearExpression.EndDay == 0)
            {
                var argument = BuildArgument(Compiler.Identifiers.RangeEachYear.Month, rangeEachYearExpression.StartMonth.ToString());

                BuildExpression(Compiler.Identifiers.Expressions.RangeEachYear, argument, output);
            }
            else if (rangeEachYearExpression.StartDay == 0 && rangeEachYearExpression.EndDay == 0)
            {
                var arguments = new List<string>()
                {
                    BuildArgument(Compiler.Identifiers.RangeEachYear.StartMonth, rangeEachYearExpression.StartMonth.ToString()),
                    BuildArgument(Compiler.Identifiers.RangeEachYear.EndMonth, rangeEachYearExpression.EndMonth.ToString()),
                };

                BuildExpression(Compiler.Identifiers.Expressions.RangeEachYear, BuildArgumentList(arguments), output);
            }
            else
            {
                var arguments = new List<string>()
                {
                    BuildArgument(Compiler.Identifiers.RangeEachYear.StartMonth, rangeEachYearExpression.StartMonth.ToString()),
                    BuildArgument(Compiler.Identifiers.RangeEachYear.EndMonth, rangeEachYearExpression.EndMonth.ToString()),
                    BuildArgument(Compiler.Identifiers.RangeEachYear.StartDay, rangeEachYearExpression.StartDay.ToString()),
                    BuildArgument(Compiler.Identifiers.RangeEachYear.EndDay, rangeEachYearExpression.EndDay.ToString()),
                };

                BuildExpression(Compiler.Identifiers.Expressions.RangeEachYear, BuildArgumentList(arguments), output);
            }
        }

        public static void SerializeRegularInterval(TemporalExpression temporalExpression, StringBuilder output)
        {
            var regularIntervalExpression = temporalExpression as RegularInterval;

            var arguments = new List<string>()
                {
                    BuildArgument(Compiler.Identifiers.RegularInterval.Year, regularIntervalExpression.StartDate.Year.ToString()),
                    BuildArgument(Compiler.Identifiers.RegularInterval.Month, regularIntervalExpression.StartDate.Month.ToString()),
                    BuildArgument(Compiler.Identifiers.RegularInterval.Day, regularIntervalExpression.StartDate.Day.ToString()),
                    BuildArgument(Compiler.Identifiers.RegularInterval.Count, regularIntervalExpression.Count.ToString()),
                    BuildArgument(Compiler.Identifiers.RegularInterval.Unit, regularIntervalExpression.Unit.ToString().ToLower()),
                };

            BuildExpression(Compiler.Identifiers.Expressions.RegularInterval, BuildArgumentList(arguments), output);
        }

        public static void SerializeDifference(TemporalExpression temporalExpression, StringBuilder output)
        {
            var differenceExpression = temporalExpression as Difference;

            var arguments = new List<string>()
            {
                BuildArgument(Compiler.Identifiers.Difference.Included, Serialize(differenceExpression.Included)),
                BuildArgument(Compiler.Identifiers.Difference.Excluded, Serialize(differenceExpression.Excluded)),
            };

            BuildExpression(Compiler.Identifiers.Expressions.Difference, BuildArgumentList(arguments), output);
        }

        public static void SerializeIntersection(TemporalExpression temporalExpression, StringBuilder output)
        {
            var intersectionExpression = temporalExpression as Intersection;

            var arguments = BuildListArgument(Compiler.Identifiers.Intersection.Elements, intersectionExpression.Elements);

            BuildExpression(Compiler.Identifiers.Expressions.Intersection, arguments, output);
        }

        public static void SerializeUnion(TemporalExpression temporalExpression, StringBuilder output)
        {
            var unionExpression = temporalExpression as Union;

            var arguments = BuildListArgument(Compiler.Identifiers.Union.Elements, unionExpression.Elements);

            BuildExpression(Compiler.Identifiers.Expressions.Union, arguments, output);
        }

        public static void SerializeTrue(TemporalExpression temporalExpression, StringBuilder output)
        {
            BuildExpression(Compiler.Identifiers.Expressions.True, string.Empty, output);
        }

        public static void SerializeFalse(TemporalExpression temporalExpression, StringBuilder output)
        {
            BuildExpression(Compiler.Identifiers.Expressions.False, string.Empty, output);
        }

        public static void SerializeNot(TemporalExpression temporalExpression, StringBuilder output)
        {
            var notExpression = temporalExpression as Not;

            if (notExpression != null)
            {
                var expression = Serialize(notExpression.Expression);

                var argument = BuildArgument(Compiler.Identifiers.Not.Expression, expression);

                BuildExpression(Compiler.Identifiers.Expressions.Not, argument, output);
            }            
        }

        public static void BuildExpression(string identifier, string arguments, StringBuilder builder)
        {
            builder.Append(GrammarUtil.ExprStart);

            builder.Append(identifier);

            if (arguments.Length > 0)
            {
                builder.Append(GrammarUtil.ArgumentsStart);

                builder.Append(arguments);

                builder.Append(GrammarUtil.ArgumentsEnd);
            }

            builder.Append(GrammarUtil.ExprEnd);
        }

        public static string BuildArgument(string identifier, string value)
        {
            var builder = new StringBuilder();

            builder.Append(identifier);

            builder.Append(GrammarUtil.IdentifierSeparator);

            builder.Append(value);

            return builder.ToString();
        }

        public static string BuildArgumentList(List<string> arguments)
        {
            return string.Join(GrammarUtil.ArgumentDelimiter.ToString(), arguments);
        }

        public static string BuildListArgument(string identifier, List<TemporalExpression> expressions)
        {
            var builder = new StringBuilder();

            builder.Append(identifier);

            builder.Append(GrammarUtil.IdentifierSeparator);

            var serializedExpressions = expressions.Select(Serialize).ToList();

            builder.Append(string.Join(GrammarUtil.ListArgumentDelimiter.ToString(), serializedExpressions));

            return builder.ToString();
        }
    }
}
