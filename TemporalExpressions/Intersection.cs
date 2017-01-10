using System;
using System.Collections.Generic;
using System.Linq;

namespace TemporalExpressions
{
    public class Intersection : TemporalExpression
    {
        public List<TemporalExpression> Elements { get; set; }

        public Intersection(List<TemporalExpression> elements)
        {
            this.Elements = elements;
        }

        public override bool Includes(DateTime date)
        {
            return Elements.All(e => e.Includes(date));
        }
    }
}
