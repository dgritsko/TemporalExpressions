namespace TemporalExpressions.Compiler
{
    public static class Util
    {
        public const char ExprStart = '{';
        public const char ExprEnd = '}';
        public const char ArgumentsStart = '(';
        public const char ArgumentsEnd = ')';
        public const char IdentifierSeparator = ':';
        public const char ArgumentDelimiter = ',';
        public const char ListArgumentDelimiter = ';';

        public static bool IsExprStart(char c)
        {
            return c == ExprStart;
        }

        public static bool IsExprEnd(char c)
        {
            return c == ExprEnd;
        }

        public static bool IsArgumentsStart(char c)
        {
            return c == ArgumentsStart;
        }

        public static bool IsArgumentsEnd(char c)
        {
            return c == ArgumentsEnd;
        }

        public static bool IsIdentifierSeparator(char c)
        {
            return c == IdentifierSeparator;
        }

        public static bool IsArgumentDelimiter(char c)
        {
            return c == ArgumentDelimiter;
        }

        public static bool IsListArgumentDelimiter(char c)
        {
            return c == ListArgumentDelimiter;
        }
    }
}
