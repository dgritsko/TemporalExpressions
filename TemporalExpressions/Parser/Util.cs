namespace TemporalExpressions.Parser
{
    public static class Util
    {
        public const char ExprStart = '{';
        public const char ExprEnd = '}';
        public const char ArgumentStart = '(';
        public const char ArgumentEnd = ')';
        public const char ArgumentSeparator = ':';
        public const char ArgumentDelimiter = ',';

        public static bool IsExprStart(char c)
        {
            return c == ExprStart;
        }

        public static bool IsExprEnd(char c)
        {
            return c == ExprEnd;
        }

        public static bool IsArgumentStart(char c)
        {
            return c == ArgumentStart;
        }

        public static bool IsArgumentEnd(char c)
        {
            return c == ArgumentEnd;
        }

        public static bool IsArgumentSeparator(char c)
        {
            return c == ArgumentSeparator;
        }

        public static bool IsArgumentDelimiter(char c)
        {
            return c == ArgumentDelimiter;
        }
    }
}
