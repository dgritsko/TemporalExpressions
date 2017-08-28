using System;
using System.Collections.Generic;
using System.Text;
using TemporalExpressions.Compiler.Util;

namespace TemporalExpressions.Serializer
{
    public static class Serializer
    {
        public static Dictionary<Type, Action<TemporalExpression, StringBuilder>> Serializers = new Dictionary<Type, Action<TemporalExpression, StringBuilder>>
        {
            { typeof(RangeEachYear), SerializeRangeEachYear },
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

        public static void SerializeRangeEachYear(TemporalExpression temporalExpression, StringBuilder output)
        {
            var expression = temporalExpression as RangeEachYear;

            if (expression.StartMonth == expression.EndMonth && expression.StartDay == 0 && expression.EndDay == 0)
            {
                var argument = BuildArgument(Compiler.Identifiers.RangeEachYear.Month, expression.StartMonth.ToString());

                BuildExpression(Compiler.Identifiers.Expressions.RangeEachYear, argument, output);
            }
            else if (expression.StartDay == 0 && expression.EndDay == 0)
            {
                var arguments = new List<string>()
                {
                    BuildArgument(Compiler.Identifiers.RangeEachYear.StartMonth, expression.StartMonth.ToString()),
                    BuildArgument(Compiler.Identifiers.RangeEachYear.EndMonth, expression.EndMonth.ToString()),
                };

                BuildExpression(Compiler.Identifiers.Expressions.RangeEachYear, BuildArgumentList(arguments), output);
            }
            else
            {
                var arguments = new List<string>()
                {
                    BuildArgument(Compiler.Identifiers.RangeEachYear.StartMonth, expression.StartMonth.ToString()),
                    BuildArgument(Compiler.Identifiers.RangeEachYear.EndMonth, expression.EndMonth.ToString()),
                    BuildArgument(Compiler.Identifiers.RangeEachYear.StartDay, expression.StartDay.ToString()),
                    BuildArgument(Compiler.Identifiers.RangeEachYear.EndDay, expression.EndDay.ToString()),
                };

                BuildExpression(Compiler.Identifiers.Expressions.RangeEachYear, BuildArgumentList(arguments), output);
            }
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
    }
}
