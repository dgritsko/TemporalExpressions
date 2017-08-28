using System;

namespace TemporalExpressions
{
    public class Not : TemporalExpression
    {
        public TemporalExpression Expression { get; set; }

        public Not(TemporalExpression expression)
        {
            this.Expression = expression;
        }

        public override bool Includes(DateTime date)
        {
            return !Expression.Includes(date);
        }
    }
}
