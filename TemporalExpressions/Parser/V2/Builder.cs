namespace TemporalExpressions.Parser.V2
{
    public static class Builder
    {
        public static TemporalExpression Build(string input)
        {
            if (!Analyzer.Analyze(input))
            {
                return null;
            }

            var parsed = TemporalExpressions.Parser.V2.Parser.ParseExpression(input);

            var compiled = TemporalExpressions.Parser.V2.Compiler.Compile(parsed);

            return compiled;
        }
    }
}
