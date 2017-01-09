using System;

namespace TemporalExpressions
{
    public class RangeEachYear : TemporalExpression
    {
        public int StartMonth { get; set; }
        public int EndMonth { get; set; }
        public int StartDay { get; set; }
        public int EndDay { get; set; }
        
        public RangeEachYear(int startMonth, int endMonth, int startDay, int endDay)
        {
            this.StartMonth = startMonth;
            this.EndMonth = endMonth;
            this.StartDay = startDay;
            this.EndDay = endDay;
        }

        public RangeEachYear(int startMonth, int endMonth) : this(startMonth, endMonth, 0, 0) { }

        public RangeEachYear(int month) : this(month, month, 0, 0) { }

        public override bool Includes(DateTime date)
        {
            return MonthsInclude(date) || StartMonthIncludes(date) || EndMonthIncludes(date);
        }

        private bool MonthsInclude(DateTime date)
        {
            var month = date.Month;

            return month > StartMonth && month < EndMonth;
        }

        private bool StartMonthIncludes(DateTime date)
        {
            if (date.Month != StartMonth)
            {
                return false;
            }

            if (StartDay == 0)
            {
                return true;
            }

            return date.Day >= StartDay;
        }

        private bool EndMonthIncludes(DateTime date)
        {
            if (date.Month != EndMonth)
            {
                return false;
            }

            if (EndDay == 0)
            {
                return true;
            }

            return date.Day <= EndDay;
        }
    }
}
