using System.Collections.Generic;

namespace TemporalExpressions.Compiler.Components
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
}
