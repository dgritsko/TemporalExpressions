using System.Collections.Generic;

namespace TemporalExpressions.Parser.Parts
{
    public class TokenizedExpression
    {
        public string Identifier { get; set; }
        public List<TokenizedArgument> Arguments { get; set; }
    }
}
