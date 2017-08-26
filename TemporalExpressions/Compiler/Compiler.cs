namespace TemporalExpressions.Compiler
{
    public static class Compiler
    {
        public static TemporalExpression Compile(string input)
        {
            if (!Analyzer.Analyze(input))
            {
                return null;
            }

            var parsed = Parser.ParseExpression(input);

            var expression = Builder.Build(parsed);

            return expression;
        }
    }
}
