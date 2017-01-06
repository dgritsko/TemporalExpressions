using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemporalExpressions
{
    public abstract class TemporalExpression
    {
        public abstract bool Includes(DateTime date);
    }
}
