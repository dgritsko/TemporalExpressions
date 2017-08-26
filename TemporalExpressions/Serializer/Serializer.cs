using System;
using System.Collections.Generic;
using TemporalExpressions.Compiler.Identifiers;

namespace TemporalExpressions.Serializer
{
    public static class Serializer
    {
        public static Dictionary<Type, Func<TemporalExpression, string>> Serializers = new Dictionary<Type, Func<TemporalExpression, string>>
        {
            { typeof(RangeEachYear), SerializeRangeEachYear }
        };

        public static string Serialize(TemporalExpression expression)
        {
            var key = expression.GetType();

            if (!Serializers.ContainsKey(key))
            {
                throw new ArgumentException($"Unsupported type: {key}");
            }

            return Serializers[key](expression);
        }

        public static string SerializeRangeEachYear(TemporalExpression temporalExpression)
        {
            var expression = temporalExpression as RangeEachYear;

            if (expression.StartMonth == expression.EndMonth && expression.StartDay == 0 && expression.EndDay == 0)
            {
                var argument = BuildArgument(Compiler.Identifiers.RangeEachYear.Month, expression.StartMonth.ToString());

                return BuildExpression(Compiler.Identifiers.Expressions.RangeEachYear, argument);
            }

            throw new NotImplementedException();
        }

        public static string BuildExpression(string identifier, string arguments)
        {
            return $"{{{identifier}({arguments})}}";
        }

        public static string BuildArgument(string identifier, string value)
        {
            return $"{identifier}:{value}";
        }
    }
}
