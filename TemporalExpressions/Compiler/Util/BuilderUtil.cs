using TemporalExpressions.Compiler.Components;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TemporalExpressions.Compiler.Util
{
    public static class BuilderUtil
    {
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
