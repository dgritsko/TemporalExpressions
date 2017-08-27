using System;

namespace TemporalExpressions
{
    public class RangeEachInterval : TemporalExpression
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public int Count { get; set; }

        public UnitOfTime Unit { get; set; }

        public RangeEachInterval(int year, int month, int day, int count, UnitOfTime unit)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
            this.Count = count;
            this.Unit = unit;
        }

        public override bool Includes(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
