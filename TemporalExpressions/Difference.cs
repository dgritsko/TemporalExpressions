using System;

namespace TemporalExpressions
{
    public class Difference : TemporalExpression
    {
        public TemporalExpression Included { get; set; }
        public TemporalExpression Excluded { get; set; }

        public Difference(TemporalExpression included, TemporalExpression excluded)
        {
            this.Included = included;
            this.Excluded = excluded;
        }

        public override bool Includes(DateTime date)
        {
            return this.Included.Includes(date) && !this.Excluded.Includes(date);
        }
    }
}
