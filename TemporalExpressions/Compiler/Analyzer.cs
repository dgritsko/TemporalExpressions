﻿using System;
using System.Collections.Generic;

namespace TemporalExpressions.Compiler
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

        public static Dictionary<State, Func<char?, char, State?>> Transitions = new Dictionary<State, Func<char?, char, State?>>()
        {
            { State.StartExpression, HandleStart },
            { State.EndExpression, HandleEndExpression },
            { State.ExpressionIdentifier, HandleExpressionIdentifier },
            { State.Arguments, HandleArguments },
            { State.ArgumentIdentifier, HandleArgumentIdentifier },
            { State.ArgumentValue, HandleArgumentValue },
        };

        public static State? HandleStart(char? prev, char curr)
        {
            return Util.IsExprStart(curr) ? State.ExpressionIdentifier : (State?)null;
        }

        public static State? HandleExpressionIdentifier(char? prev, char curr)
        {
            if (char.IsLetter(curr))
            {
                return State.ExpressionIdentifier;
            }

            if (Util.IsArgumentsStart(curr))
            {
                return State.Arguments;
            }

            return null;
        }

        public static State? HandleArguments(char? prev, char curr)
        {
            if (char.IsLetter(curr))
            {
                return State.ArgumentIdentifier;
            }

            return null;
        }

        public static State? HandleArgumentIdentifier(char? prev, char curr)
        {
            if (char.IsLetter(curr))
            {
                return State.ArgumentIdentifier;
            }

            if (Util.IsIdentifierSeparator(curr))
            {
                return State.ArgumentValue;
            }

            return null;
        }

        public static State? HandleArgumentValue(char? prev, char curr)
        {
            if (Util.IsExprStart(curr))
            {
                return State.ExpressionIdentifier;
            }

            if (prev.HasValue && Util.IsIdentifierSeparator(prev.Value) && curr == '-')
            {
                return State.ArgumentValue;
            }

            if (char.IsLetterOrDigit(curr))
            {
                return State.ArgumentValue;
            }

            if (Util.IsArgumentsEnd(curr))
            {
                return State.ArgumentValue;
            }

            if (Util.IsListArgumentDelimiter(curr))
            {
                return State.StartExpression;
            }

            if (Util.IsExprEnd(curr))
            {
                return State.EndExpression;
            }

            if (Util.IsArgumentDelimiter(curr))
            {
                return State.ArgumentIdentifier;
            }
            
            return null;
        }

        public static State? HandleEndExpression(char? prev, char curr)
        {
            if (Util.IsArgumentDelimiter(curr))
            {
                return State.ArgumentIdentifier;
            }

            if (Util.IsArgumentsEnd(curr))
            {
                return State.EndExpression;
            }

            if (Util.IsExprEnd(curr))
            {
                return State.EndExpression;
            }

            if (Util.IsListArgumentDelimiter(curr))
            {
                return State.StartExpression;
            }

            return null;
        }
        
        public static bool Analyze(string input)
        {
            var state = State.StartExpression;

            for (var i = 0; i < input.Length; i++)
            {
                var prev = i == 0 ? (char?) null : input[i - 1];
                var curr = input[i];

                var nextState = Transitions[state](prev, curr);

#if DEBUG
                Console.WriteLine($"{input.Substring(0, i)}[{curr}]{input.Substring(i + 1)}");
#endif

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