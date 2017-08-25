using System.Collections.Generic;

namespace TemporalExpressions.Parser.V2.Components
{
    public class Expression
    {
        public Identifier Identifier { get; set; }

        public List<Argument> Arguments { get; set; }

        public Expression(Identifier identifier, List<Argument> arguments)
        {
            Identifier = identifier;
            Arguments = arguments;
        }
    }

    public class Identifier
    {
        public string Value { get; set; }

        public Identifier(string value)
        {
            Value = value;
        }
    }

    public abstract class Argument
    {
        public Identifier Identifier { get; set; }
    }

    public class ExpressionsArgument : Argument
    {
        public List<Expression> Expressions { get; set; }

        public ExpressionsArgument(Identifier identifier, List<Expression> expressions)
        {
            Identifier = identifier;
            Expressions = expressions;
        }
    }

    public class ScalarArgument : Argument
    {
        public string Value { get; set; }

        public ScalarArgument(Identifier identifier, string value)
        {
            Identifier = identifier;
            Value = value;
        }
    }
}