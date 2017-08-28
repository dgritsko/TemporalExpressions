using System.Collections.Generic;
using System.Linq;
using TemporalExpressions.Compiler.Components;
using TemporalExpressions.Compiler.Util;

namespace TemporalExpressions.Compiler
{
    public static class Parser
    {
        public static Expression ParseExpression(string input)
        {
            var expressionBody = input.Substring(1, input.Length - 2);

            var index = expressionBody.IndexOf(GrammarUtil.ArgumentsStart);

            Identifier identifier;
            List<Argument> arguments;

            if (index == -1)
            {
                identifier = ParseIdentifier(expressionBody);
                arguments = new List<Argument>();
            }
            else
            {
                var identifierInput = expressionBody.Substring(0, index);
                var argumentsInput = expressionBody.Substring(index, expressionBody.Length - index);

                identifier = ParseIdentifier(identifierInput);
                arguments = ParseArguments(argumentsInput);
            }            

            return new Expression(identifier, arguments);
        }

        public static Identifier ParseIdentifier(string input)
        {
            return new Identifier(input);
        }

        public static List<Argument> ParseArguments(string input)
        {
            var argumentsBody = input.Substring(1, input.Length - 2);

            var argumentInputs = new List<string>();

            var index = 0;
            var depth = 0;

            for (var i = 0; i < argumentsBody.Length; i++)
            {
                var curr = argumentsBody[i];

                if (GrammarUtil.IsExprStart(curr))
                {
                    depth++;
                }

                if (GrammarUtil.IsExprEnd(curr))
                {
                    depth--;
                }

                if (GrammarUtil.IsArgumentDelimiter(curr) && depth == 0)
                {
                    argumentInputs.Add(argumentsBody.Substring(index, i - index));
                    index = i + 1;
                }
            }

            if (index < argumentsBody.Length)
            {
                argumentInputs.Add(argumentsBody.Substring(index, argumentsBody.Length - index));
            }

            return argumentInputs.Select(ParseArgument).ToList();
        }

        public static Argument ParseArgument(string input)
        {
            var argumentComponents = input.Split(new[] { GrammarUtil.IdentifierSeparator }, 2);

            var identifierInput = argumentComponents[0];
            var argumentsInput = argumentComponents[1];

            var index = 0;
            var depth = 0;

            var isScalar = true;
            var buffer = new List<string>();

            for (var i = 0; i < argumentsInput.Length; i++)
            {
                var curr = argumentsInput[i];

                if (GrammarUtil.IsExprStart(curr))
                {
                    isScalar = false;
                    depth++;
                }

                if (GrammarUtil.IsExprEnd(curr))
                {
                    depth--;
                }

                if (GrammarUtil.IsListArgumentDelimiter(curr) && depth == 0)
                {
                    buffer.Add(argumentsInput.Substring(index, i - index));
                    index = i + 1;
                }
            }

            if (index < argumentsInput.Length)
            {
                buffer.Add(argumentsInput.Substring(index, argumentsInput.Length - index));
            }

            var identifier = new Identifier(identifierInput);

            if (isScalar)
            {
                return new ScalarArgument(identifier, buffer[0]);
            }

            var expressions = buffer.Select(ParseExpression).ToList();

            return new ExpressionsArgument(identifier, expressions);
        }
    }
}
