using System;

namespace TemporalExpressions
{
    public class True : TemporalExpression
    {
        public override bool Includes(DateTime date)
        {
            return true;
        }
    }
}
