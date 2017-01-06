using System;

namespace TemporalExpressions
{
    public class DayInMonth : TemporalExpression
    {
        private int count;
        private DayOfWeek day;

        public DayInMonth(int count, DayOfWeek day)
        {
            this.count = count;
            this.day = day;
        }

        public override bool Includes(DateTime date)
        {
            return DayMatches(date) && WeekMatches(date);
        }

        private bool DayMatches(DateTime date)
        {
            return date.DayOfWeek == this.day;
        }

        private bool WeekMatches(DateTime date)
        {
            return count > 0
                ? WeekFromStartMatches(date)
                : WeekFromEndMatches(date);
        }

        private bool WeekFromStartMatches(DateTime date)
        {
            return this.WeekInMonth(date.Day) == count;
        }

        private bool WeekFromEndMatches(DateTime date)
        {
            int daysFromMonthEnd = DateTime.DaysInMonth(date.Year, date.Month) - date.Day;
            return WeekInMonth(daysFromMonthEnd) == Math.Abs(count);
        }

        private int WeekInMonth(int dayNumber)
        {
            return ((dayNumber - 1) / 7) + 1;
        }
    }
}
