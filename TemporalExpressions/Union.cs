using System;
using System.Collections.Generic;
using System.Linq;

namespace TemporalExpressions
{
    public class Union : TemporalExpression
    {
        public List<TemporalExpression> Elements { get; set; }

        public Union(List<TemporalExpression> elements)
        {
            this.Elements = elements;
        }

        public override bool Includes(DateTime date)
        {
            return Elements.Any(e => e.Includes(date));
        }
    }
}
