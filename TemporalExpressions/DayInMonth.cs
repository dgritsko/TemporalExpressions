using System;

namespace TemporalExpressions
{
    public class DayInMonth : TemporalExpression
    {
        public int Count { get; set; }
        public DayOfWeek Day { get; set; }

        public DayInMonth(int count, DayOfWeek day)
        {
            this.Count = count;
            this.Day = day;
        }

        public override bool Includes(DateTime date)
        {
            return DayMatches(date) && WeekMatches(date);
        }

        private bool DayMatches(DateTime date)
        {
            return date.DayOfWeek == this.Day;
        }

        private bool WeekMatches(DateTime date)
        {
            return Count > 0
                ? WeekFromStartMatches(date)
                : WeekFromEndMatches(date);
        }

        private bool WeekFromStartMatches(DateTime date)
        {
            return this.WeekInMonth(date.Day) == Count;
        }

        private bool WeekFromEndMatches(DateTime date)
        {
            int daysFromMonthEnd = DateTime.DaysInMonth(date.Year, date.Month) - date.Day;
            return WeekInMonth(daysFromMonthEnd) == Math.Abs(Count);
        }

        private int WeekInMonth(int dayNumber)
        {
            return ((dayNumber - 1) / 7) + 1;
        }
    }
}
