using System;
using System.Collections.Generic;
using System.Linq;
using TemporalExpressions.Parser.Parts;

namespace TemporalExpressions.Parser
{
    public class Parser
    {
        public static string Serialize(TemporalExpression temporalExpression)
        {
            // TODO: Create string representation of TemporalExpression adhering to the appropriate grammar

            throw new NotImplementedException();
        }

        public static TemporalExpression Parse(string expression)
        {
            var noWhitespace = new string(expression.Where(c => !char.IsWhiteSpace(c)).ToArray());

            var tokenized = TokenizeExpression(noWhitespace);

            // TODO: Construct instances of appropriate classes
            
            throw new NotImplementedException();
        }

        public static TokenizedExpression TokenizeExpression(string expression)
        {
            if (!Util.IsExprStart(expression[0]) || !Util.IsExprEnd(expression[expression.Length - 1]))
            {
                throw new ParserException($"Expression must begin with {Util.ExprStart} and end with {Util.ExprEnd}");
            }

            var index = expression.IndexOf(Util.IdentifierSeparator);

            if (index == -1)
            {
                throw new ParserException($"Expression must contain identifier separator: {Util.IdentifierSeparator}");
            }

            var identifier = expression.Substring(1, index - 1);
            var arguments = expression.Substring(index + 1, expression.Length - (index + 2));

            var tokenizedArguments = TokenizeArguments(arguments);

            return new TokenizedExpression() { Identifier = identifier, Arguments = tokenizedArguments };
        }

        public static List<TokenizedArgument> TokenizeArguments(string arguments)
        {
            if (!Util.IsArgumentsStart(arguments[0]) || !Util.IsArgumentsEnd(arguments[arguments.Length - 1]))
            {
                throw new ParserException($"Arguments must begin with {Util.ArgumentsStart} and end with {Util.ArgumentsEnd}");
            }

            var results = new List<string>();

            var depth = 0;

            var argumentStartIndex = 1;

            for (var i = 1; i < arguments.Length - 1; i++)
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
                results.Add(arguments.Substring(argumentStartIndex, arguments.Length - (argumentStartIndex + 1)));
            }

            return results.Select(TokenizeArgument).ToList();
        }

        public static TokenizedArgument TokenizeArgument(string argument)
        {
            var index = argument.IndexOf(Util.IdentifierSeparator);

            if (index == -1)
            {
                throw new ParserException($"Argument must contain identifier separator: {Util.IdentifierSeparator}");
            }

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

        public class ParserException : Exception
        {
            public ParserException(string message) : base(message) { }
        }
    }
}