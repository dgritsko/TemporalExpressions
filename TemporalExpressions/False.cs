using System;

namespace TemporalExpressions
{
    public class False : TemporalExpression
    {
        public override bool Includes(DateTime date)
        {
            return false;
        }
    }
}
