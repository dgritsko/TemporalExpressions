using System.Collections.Generic;

namespace TemporalExpressions.Compiler.Components
{
    public class ExpressionsArgument : Argument
    {
        public List<Expression> Expressions { get; set; }

        public ExpressionsArgument(Identifier identifier, List<Expression> expressions)
        {
            Identifier = identifier;
            Expressions = expressions;
        }
    }
}
