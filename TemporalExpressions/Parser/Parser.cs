using System;
using System.Collections.Generic;
using System.Linq;
using TemporalExpressions.Parser.Parts;

namespace TemporalExpressions.Parser
{
    public class Parser
    {
        public static TemporalExpression Parse(string expression)
        {
            var noWhitespace = new string(expression.Where(c => !char.IsWhiteSpace(c)).ToArray());

            throw new NotImplementedException();
        }

        public static TokenizedExpression TokenizeExpression(string expression)
        {
            throw new NotImplementedException();
        }

        public static List<TokenizedArgument> TokenizeArguments(string arguments)
        {
            var results = new List<string>();

            var depth = 0;

            var argumentStartIndex = 0;

            for (var i = 0; i < arguments.Length; i++)
            {
                var curr = arguments[i];

                if (Util.IsExprStart(curr))
                {
                    depth++;
                }

                if (Util.IsExprEnd(curr))
                {
                    depth--;
                }

                if (Util.IsArgumentDelimiter(curr) && depth == 0)
                {
                    results.Add(arguments.Substring(argumentStartIndex, i - argumentStartIndex));
                    argumentStartIndex = i + 1;
                }
            }

            if (argumentStartIndex < arguments.Length)
            {
                results.Add(arguments.Substring(argumentStartIndex));
            }

            return results.Select(TokenizeArgument).ToList();
        }

        public static TokenizedArgument TokenizeArgument(string argument)
        {
            var index = argument.IndexOf(Util.ArgumentSeparator);

            var identifier = argument.Substring(0, index);
            var value = argument.Substring(index + 1);
            var type = Util.IsExprStart(value[0]) ? ArgumentType.Expression : ArgumentType.Scalar;

            return new TokenizedArgument
            {
                Identifier = identifier,
                Value = value,
                Type = type
            };
        }

    }
}