namespace TemporalExpressions.Compiler.Components
{
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
