using System;
using System.Linq;
using System.Collections.Generic;

namespace TemporalExpressions.Parser.Parts
{
    public class TokenizedExpression
    {
        public string Identifier { get; set; }
        public List<TokenizedArgument> Arguments { get; set; }

        public T GetArgument<T>(string identifier)
        {
            var argument = Arguments.FirstOrDefault(arg => string.Equals(arg.Identifier, identifier));
            
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
    }
}
