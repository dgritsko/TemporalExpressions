using System;
using System.Collections.Generic;

namespace TemporalExpressions.Parser.V2
{
    public static class Analyzer
    {
        public enum State
        {
            StartExpression,
            EndExpression,
            ExpressionIdentifier,
            Arguments,
            ArgumentIdentifier,
            ArgumentValue
        }

        public static Dictionary<State, Func<char, State?>> Transitions = new Dictionary<State, Func<char, State?>>()
        {
            { State.StartExpression, HandleStart },
            //{ State.EndExpression, HandleArgumentValue },
            { State.ExpressionIdentifier, HandleExpressionIdentifier },
            { State.Arguments, HandleArguments },
            { State.ArgumentIdentifier, HandleArgumentIdentifier },
            { State.ArgumentValue, HandleArgumentValue },
        };

        public static State? HandleStart(char c)
        {
            return Util.IsExprStart(c) ? State.ExpressionIdentifier : (State?)null;
        }

        public static State? HandleExpressionIdentifier(char c)
        {
            if (char.IsLetter(c))
            {
                return State.ExpressionIdentifier;
            }

            if (Util.IsArgumentsStart(c))
            {
                return State.Arguments;
            }

            return null;
        }

        public static State? HandleArguments(char c)
        {
            if (char.IsLetter(c))
            {
                return State.ArgumentIdentifier;
            }

            return null;
        }

        public static State? HandleArgumentIdentifier(char c)
        {
            if (char.IsLetter(c))
            {
                return State.ArgumentIdentifier;
            }

            if (Util.IsIdentifierSeparator(c))
            {
                return State.ArgumentValue;
            }

            return null;
        }

        public static State? HandleArgumentValue(char c)
        {
            if (Util.IsExprStart(c))
            {
                return State.ExpressionIdentifier;
            }

            if (char.IsLetterOrDigit(c))
            {
                return State.ArgumentValue;
            }

            if (Util.IsArgumentsEnd(c))
            {
                return State.ArgumentValue;
            }

            if (Util.IsListArgumentDelimiter(c))
            {
                return State.StartExpression;
            }

            if (Util.IsExprEnd(c))
            {
                return State.EndExpression;
            }

            if (Util.IsArgumentDelimiter(c))
            {
                return State.ArgumentIdentifier;
            }
            
            return null;
        }
        
        public static bool Analyze(string input)
        {
            var state = State.StartExpression;

            for (var i = 0; i < input.Length; i++)
            {
                var curr = input[i];

                var nextState = Transitions[state](curr);

                if (nextState.HasValue)
                {
                    state = nextState.Value;
                }
                else
                {
                    throw new ArgumentException($"Invalid character \"{curr}\" at position {i}");
                }
            }

            if (state != State.EndExpression)
            {
                throw new ArgumentException("Unexpected end of input");
            }

            return true;
        }
    }
}
